using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Collections.Generic;

namespace VITVDataFeed
{
    public class GoogleAnalytics
    {
        public AnalyticsService Service { get; set; }

        public GoogleAnalytics(string keyPath, string accountEmailAddress)
        {
            var certificate = new X509Certificate2(keyPath, "notasecret", X509KeyStorageFlags.Exportable);

            var credentials = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer(accountEmailAddress)
               {
                   Scopes = new[] { AnalyticsService.Scope.AnalyticsReadonly }
               }.FromCertificate(certificate));

            Service = new AnalyticsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = "VITV"
            });
        }

        public AnalyticDataPoint GetAnalyticsData(string profileId, string[] metrics, string[] dimensions, DateTime startDate, DateTime endDate)
        {
            AnalyticDataPoint data = new AnalyticDataPoint();
            if (!profileId.Contains("ga:"))
                profileId = string.Format("ga:{0}", profileId);

            GaData response = null;
            do
            {
                int startIndex = 1;
                if (response != null && !string.IsNullOrEmpty(response.NextLink))
                {
                    Uri uri = new Uri(response.NextLink);
                    var paramerters = uri.Query.Split('&');
                    string s = paramerters.First(i => i.Contains("start-index")).Split('=')[1];
                    startIndex = int.Parse(s);
                }

                var request = BuildAnalyticRequest(profileId, metrics, dimensions, startDate, endDate, startIndex);
                response = request.Execute();
                data.ColumnHeaders = response.ColumnHeaders;
                data.Rows.AddRange(response.Rows);

            }
            while (!string.IsNullOrEmpty(response.NextLink));

            return data;
        }

        public DataResource.GaResource.GetRequest BuildAnalyticRequest(string profileId, string[] metrics, string[] dimensions, DateTime startDate, DateTime endDate, int startIndex)
        {
            DataResource.GaResource.GetRequest request = Service.Data.Ga.Get(profileId, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), string.Join(",", metrics));
            request.StartIndex = startIndex;
            request.Dimensions = string.Join(",", dimensions);
            return request;
        }

        public class AnalyticDataPoint
        {
            public AnalyticDataPoint()
            {
                Rows = new List<IList<string>>();
            }

            public IList<GaData.ColumnHeadersData> ColumnHeaders { get; set; }
            public List<IList<string>> Rows { get; set; }
        }
    }
}