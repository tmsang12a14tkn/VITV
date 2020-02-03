using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using VITVDataFeed.VITVService;
using System.Xml.XPath;
using System.Xml.Linq;
 using VITVDataFeed.DataFeeder;
using VITV.Data.Repositories;
using VITV.Data.DAL;
using VITV.Data.Models;
namespace VITVDataFeed
{
    //public class CatAccessDate 
    //{
    //    public DateTime Date {get;set;}
    //    public int IPViewCount {get;set;}
    //    public int PageViewCount {get;set;}
    //}
    //public class CatAccessData
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }

    //    public List<CatAccessDate> Data {get;set;}
    //}
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            var from = new DateTime(2015, 2, 25);// GA.GetLastUpdate();
            var to = DateTime.Now.AddDays(-1).Date;
            //if (from < to)
            //{
            //    from = from.AddDays(1);
            //    var start = DateTime.Now;
            //    Logger.Write("Start: " + start.ToString());
            //    GA.GetSiteContentData(from, to);
            //    GA.GetSearchContentData(from, to);
            //    GA.GetSocialNetworkData(from, to);
            //    GA.GetSocialActivityContentUrlData(from, to);
            //    GA.GetAgeData(from, to);
            //    GA.GetGenderData(from, to);
            //    GA.GetCountryData(from, to);
            //    GA.GetCountryIsoCodeData(from, to);
            //    GA.GetCityData(from, to);
            //    GA.GetContinentData(from, to);
            //    GA.GetSubContinentData(from, to);
            //    GA.GetUserTypeData(from, to);
            //    GA.GetBrowserData(from, to);
            //    Logger.Write("GetBrowserData Completed: " + DateTime.Now.ToString());
            //    GA.GetOSData(from, to);
            //    Logger.Write("GetOSData Completed: " + DateTime.Now.ToString());
            //    GA.GetScreenResolutionData(from, to);
            //    Logger.Write("GetScreenResolutionData Completed: " + DateTime.Now.ToString());
            //    GA.GetDeviceCategoryData(from, to);
            //    Logger.Write("GetDeviceCategoryData Completed: " + DateTime.Now.ToString());
            //    GA.GetDeviceInfoData(from, to);
            //    Logger.Write("GetDeviceInfoData Completed: " + DateTime.Now.ToString());
            //    GA.GetDeviceBrandingData(from, to);
            //    Logger.Write("GetDeviceBrandingData Completed: " + DateTime.Now.ToString());

            //    GA.GetAgeOverviewData(from, to);
            //    GA.GetGenderOverviewData(from, to);
            //    var end = DateTime.Now;
            //    Logger.Write("End: " + end.ToString());
            //    GA.SetLastUpdate(to);
            //}



            //var context = new VITVContext();
            //IVideoRepository _videoRepository = new VideoRepository(context);

            //var data = context.PageAccesses.Where(va => va.Page.VideoCatId!=null).GroupBy(pa => pa.Page.VideoCat).Select
            //    (g => new CatAccessData
            //    {
            //        Id = g.Key.Id,
            //        Name = g.Key.Name2,
            //        Data = g.GroupBy(p => p.Date).Select(g2 => new CatAccessDate { Date = g2.Key, IPViewCount= g2.Count(), PageViewCount = g2.Sum(c=>c.Count)}).ToList()
            //    }).ToList();
            //foreach(var catData in data)
            //{
            //    foreach(var row in catData.Data)
            //    {
            //        var catAccess = context.CategoryAccesses.Find(catData.Id, row.Date);
            //        if (catAccess == null)
            //            context.CategoryAccesses.Add(new CategoryAccess
            //                {
            //                    CategoryId = catData.Id,
            //                    Date = row.Date,
            //                    IPViewCount = row.IPViewCount,
            //                    PageViewCount = row.PageViewCount
            //                });
            //        else
            //        {
            //            catAccess.IPViewCount = row.IPViewCount;
            //            catAccess.PageViewCount = row.PageViewCount;
            //        }

            //    }
            //}
            //context.SaveChanges();
            //Console.WriteLine("Completed");
            //Console.ReadLine();
            var context = new VITVContext();
            var now = DateTime.Now.Date;
            var vAccesses = context.PageAccesses.Where(pa => pa.Page.VideoId != null && pa.Date < now).GroupBy(pa => new { VideoId = pa.Page.VideoId.Value, Date = pa.Date}).AsEnumerable().Select(
                g => new VideoAccess {
                    VideoId = g.Key.VideoId,
                    Date = g.Key.Date,
                    IPViewCount = g.Count(),
                    PageViewCount = g.Sum(p=>p.Count)
                }).ToList();
           
            context.VideoAccesses.AddRange(vAccesses);
            
            context.SaveChanges();

            //Cophieu68.ParseDailyData();
            //GiaVangNet.GetData();
            //VITV.GetDataUSD();
            //Vietcombank.GetData();
            //CNBC.GetMetalData();
            //YahooFinance.GetStockData();

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new DataFeedService() 
            //};
            //ServiceBase.Run(ServicesToRun);

            //var client = new SSIWebService.AjaxWebServiceSoapClient("AjaxWebServiceSoap");
            //string hnx30Index = client.GetHnxIndexChart();
            //Console.WriteLine(hnx30Index);
        }
    }
}
