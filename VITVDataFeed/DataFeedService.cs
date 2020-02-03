using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using VITV.Data.Repositories;
using VITVDataFeed.DataFeeder;


namespace VITVDataFeed
{
    public partial class DataFeedService : ServiceBase
    {
        //private Timer serviceTimer;
        private Timer gaTimer;

        public DataFeedService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {            
           // TimerCallback timerDelegate = new TimerCallback(DoWork);
            // create timer and attach our method delegate to it
            //serviceTimer = new Timer(timerDelegate, null, 0, 60000); //60s

            TimerCallback gaDelegate = new TimerCallback(PullData);
            // create timer and attach our method delegate to it
            gaTimer = new Timer(gaDelegate, null, 0, 3600000); //1h
 
        }

        private void DoWork(object state)
        {
            try
            {
                //Giá vàng trong nước - quốc tế
                GiaVangNet.GetData();
                //Tỷ giá USD
                VITVFeeder.GetDataUSD();
                //Tỷ giá VND
                Vietcombank.GetData();
                //Giá dầu
                CNBC.GetOilData();
                //Giá kim loại
                CNBC.GetMetalData();
                //Chứng khoán thế giới
                YahooFinance.GetStockData();          
            } 
            catch
            {
                //serviceTimer.Dispose();
            }            
        }

        private void PullData(object state)
        {
            try
            {
                var from = GA.GetLastUpdate();
                var to = DateTime.Now.AddDays(-1).Date;
                if (from < to)
                {
                    from = from.AddDays(1);
                    var start = DateTime.Now;
                    Logger.Write("Start: " + start.ToString());
                    GA.GetSiteContentData(from, to);
                    GA.GetSearchContentData(from, to);
                    GA.GetSocialNetworkData(from, to);
                    GA.GetSocialActivityContentUrlData(from, to);
                    GA.GetAgeData(from, to);
                    GA.GetGenderData(from, to);
                    GA.GetCountryData(from, to);
                    GA.GetCountryIsoCodeData(from, to);
                    GA.GetCityData(from, to);
                    GA.GetContinentData(from, to);
                    GA.GetSubContinentData(from, to);
                    GA.GetUserTypeData(from, to);
                    GA.GetBrowserData(from, to);
                    Logger.Write("GetBrowserData Completed: " + DateTime.Now.ToString());
                    GA.GetOSData(from, to);
                    Logger.Write("GetOSData Completed: " + DateTime.Now.ToString());
                    GA.GetScreenResolutionData(from, to);
                    Logger.Write("GetScreenResolutionData Completed: " + DateTime.Now.ToString());
                    GA.GetDeviceCategoryData(from, to);
                    Logger.Write("GetDeviceCategoryData Completed: " + DateTime.Now.ToString());
                    GA.GetDeviceInfoData(from, to);
                    Logger.Write("GetDeviceInfoData Completed: " + DateTime.Now.ToString());
                    GA.GetDeviceBrandingData(from, to);
                    Logger.Write("GetDeviceBrandingData Completed: " + DateTime.Now.ToString());
                    GA.GetAgeOverviewData(to);
                    Logger.Write("GetAgeOverviewData Completed: " + DateTime.Now.ToString());
                    GA.GetGenderOverviewData(to);
                    Logger.Write("GetGenderOverviewData Completed: " + DateTime.Now.ToString());

                    var end = DateTime.Now;
                    Logger.Write("End: " + end.ToString());
                    GA.SetLastUpdate(to);
                }
            }
            catch(Exception ex)
            {
                Logger.Write("Stopped - Error: " + ex.Message);
            }
        }
        
        protected override void OnStop()
        {
            //serviceTimer.Dispose();
            gaTimer.Dispose();
        }

        //"HKEY_LOCAL_MACHINE/SYSTEM/CurrentControlSet/Services"
    }
}
