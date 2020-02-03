using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VITV.Data.DAL;
using VITVDataFeed.VITVService;
using System.Xml.XPath;
using System.Xml.Linq;
using VITV.Data.Models.StoreInfo;
using System.Linq;
using System.Data.Entity;
using System.IO;
using HtmlAgilityPack;

namespace VITVDataFeed.DataFeeder
{
    public static class VITVFeeder
    {
        //public static void GetSSIStock()
        //{
        //    var client = new SSIWebService.AjaxWebServiceSoapClient("AjaxWebServiceSoap");
        //    string hnx30Index = client.GetHnxIndexChart();
        //    Console.WriteLine(hnx30Index);
        //}
        public static void GetDataUSD()
        {
            StoreInfoContext context = new StoreInfoContext();
            var client = new xmlVITVSoapClient();
            
            XElement tygiaUSDResult = null;
            try
            {
                tygiaUSDResult = client.getStock_xml();
                //tygiaUSDResult = client.getTygiaUSD_xml();
                var tygiaUSDList = tygiaUSDResult.Elements("StockInfo");
                foreach (XElement tygia in tygiaUSDList)
                {

                    var nameElement = tygia.Element("Name");//string
                    var valueElement = tygia.Element("Value");//double
                    var changeElement = tygia.Element("Change");//double
                    var percentElement = tygia.Element("Percent");//double
                    var updateTimeElement = tygia.Element("Vcreatedate");//datetime

                    var nameStr = nameElement.Value;
                    var lastUpdated = Convert.ToDateTime(updateTimeElement.Value);
                    //Cập nhật vào bảng USDExchangeRates : tạo mới nếu không có, cập nhật nếu thời gian cập nhật cũ hơn
                    USDExchangeRate exchangeRate = context.USDExchangeRates.FirstOrDefault(e => e.Name == nameStr);
                    if (exchangeRate == null)
                    {
                        USDExchangeRate e = new USDExchangeRate
                        {
                            Name = nameStr,
                            Value = Convert.ToDouble(valueElement.Value),
                            Change = Convert.ToDouble(changeElement.Value),
                            Percent = Convert.ToDouble(percentElement.Value),
                            VCreateDate = lastUpdated
                        };
                        context.USDExchangeRates.Add(e);
                    }
                    else if (exchangeRate.VCreateDate < lastUpdated)
                    {
                        exchangeRate.Value = Convert.ToDouble(valueElement.Value);
                        exchangeRate.Change = Convert.ToDouble(changeElement.Value);
                        exchangeRate.Percent = Convert.ToDouble(percentElement.Value);
                        exchangeRate.VCreateDate = lastUpdated;
                    }

                    //Cập nhật vào bảng USDExchangeRate_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
                    USDExchangeRate_Day exchangeRateDay = context.USDExchangeRate_Days.FirstOrDefault(e => e.Name == nameStr && DbFunctions.TruncateTime(e.VCreateDate) == lastUpdated.Date);
                    if (exchangeRateDay == null)
                    {
                        USDExchangeRate_Day e = new USDExchangeRate_Day
                        {
                            Name = nameStr,
                            Value = Convert.ToDouble(valueElement.Value),
                            Change = Convert.ToDouble(changeElement.Value),
                            Percent = Convert.ToDouble(percentElement.Value),
                            VCreateDate = lastUpdated.Date,
                            LastUpdate = lastUpdated
                        };
                        context.USDExchangeRate_Days.Add(e);
                    }
                    else if (exchangeRateDay.LastUpdate < lastUpdated)
                    {
                        exchangeRateDay.Value = Convert.ToDouble(valueElement.Value);
                        exchangeRateDay.Change = Convert.ToDouble(changeElement.Value);
                        exchangeRateDay.Percent = Convert.ToDouble(percentElement.Value);
                        exchangeRateDay.LastUpdate = lastUpdated;
                    }

                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                Logger.Write("GetDataUSD in Element: " + ex.Message);
            }
            

        }
        public static void GetDataVND()
        {
            StoreInfoContext context = new StoreInfoContext();
            var client = new xmlVITVSoapClient();
            try
            {
                //var stockResult = client.getStock_xml();
                var tygiaUSDResult = client.getTygiaVND_xml();
                var tygiaUSDList = tygiaUSDResult.Elements("StockInfo");
                foreach (XElement tygia in tygiaUSDList)
                {

                    var nameElement = tygia.Element("Name");//string
                    var valueElement = tygia.Element("Value");//double
                    var changeElement = tygia.Element("Change");//double
                    var percentElement = tygia.Element("Percent");//double
                    var updateTimeElement = tygia.Element("Vcreatedate");//datetime

                    var nameStr = nameElement.Value;
                    var vCreateDate = Convert.ToDateTime(updateTimeElement.Value);
                    //Cập nhật vào bảng VNDExchangeRates : tạo mới nếu không có, cập nhật nếu thời gian cập nhật cũ hơn
                    VNDExchangeRate exchangeRate = context.VNDExchangeRates.FirstOrDefault(e => e.Name == nameStr);
                    if (exchangeRate == null)
                    {
                        VNDExchangeRate e = new VNDExchangeRate
                        {
                            Name = nameStr,
                            Buy = Convert.ToDouble(valueElement.Value),
                            Sell = Convert.ToDouble(changeElement.Value),
                            Transfer = Convert.ToDouble(percentElement.Value),
                            UpdatedTime = vCreateDate
                        };
                        context.VNDExchangeRates.Add(e);
                    }
                    else if (exchangeRate.UpdatedTime < vCreateDate)
                    {
                        exchangeRate.Buy = Convert.ToDouble(valueElement.Value);
                        exchangeRate.Sell = Convert.ToDouble(changeElement.Value);
                        exchangeRate.Transfer = Convert.ToDouble(percentElement.Value);
                        exchangeRate.UpdatedTime = vCreateDate;
                    }

                    //Cập nhật vào bảng VNDExchangeRate_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
                    VNDExchangeRate_Day exchangeRateDay = context.VNDExchangeRate_Days.FirstOrDefault(e => e.Name == nameStr && DbFunctions.TruncateTime(e.UpdatedTime) == vCreateDate.Date);
                    if (exchangeRateDay == null)
                    {
                        VNDExchangeRate_Day e = new VNDExchangeRate_Day
                        {
                            Name = nameStr,
                            Buy = Convert.ToDouble(valueElement.Value),
                            Sell = Convert.ToDouble(changeElement.Value),
                            Transfer = Convert.ToDouble(percentElement.Value),
                            UpdatedTime = vCreateDate
                        };
                        context.VNDExchangeRate_Days.Add(e);
                    }
                    else if (exchangeRateDay.UpdatedTime < vCreateDate)
                    {
                        exchangeRateDay.Buy = Convert.ToDouble(valueElement.Value);
                        exchangeRateDay.Sell = Convert.ToDouble(changeElement.Value);
                        exchangeRateDay.Transfer = Convert.ToDouble(percentElement.Value);
                        exchangeRateDay.UpdatedTime = vCreateDate;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message);
            }

        }

        public static void GetDataGoldPriceVietNam()
        {
            StoreInfoContext context = new StoreInfoContext();
            var client = new xmlVITVSoapClient();
            try
            {
                var tygiaUSDResult = client.getVangVietnam_xml();
                var tygiaUSDList = tygiaUSDResult.Elements("StockInfo");
                foreach (XElement tygia in tygiaUSDList)
                {

                    var nameElement = tygia.Element("Name");//string
                    var valueElement = tygia.Element("Bid");//double
                    var changeElement = tygia.Element("Change");//double
                    var percentElement = tygia.Element("Ask");//double
                    var updateTimeElement = tygia.Element("Vcreatedate");//datetime

                    var nameStr = nameElement.Value;
                    var vCreateDate = (DateTime)updateTimeElement;
                    //Cập nhật vào bảng VNDExchangeRates : tạo mới nếu không có, cập nhật nếu thời gian cập nhật cũ hơn
                    GoldPriceVietNam exchangeRate = context.GoldPriceVietNams.FirstOrDefault(e => e.Name == nameStr);
                    if (exchangeRate == null)
                    {
                        GoldPriceVietNam e = new GoldPriceVietNam
                        {
                            Name = nameStr,
                            Buy = Convert.ToDouble(valueElement.Value),
                            Sell = Convert.ToDouble(changeElement.Value),
                            VCreateDate = vCreateDate
                        };
                        context.GoldPriceVietNams.Add(e);
                    }
                    else if (exchangeRate.VCreateDate < vCreateDate)
                    {
                        exchangeRate.Buy = Convert.ToDouble(valueElement.Value);
                        exchangeRate.Sell = Convert.ToDouble(changeElement.Value);
                        exchangeRate.VCreateDate = vCreateDate;
                    }

                    //Cập nhật vào bảng VNDExchangeRate_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
                    GoldPriceVietNam_Day exchangeRateDay = context.GoldPriceVietNam_Days.FirstOrDefault(e => e.Name == nameStr && DbFunctions.TruncateTime(e.VCreateDate) == vCreateDate.Date);
                    if (exchangeRateDay == null)
                    {
                        GoldPriceVietNam_Day e = new GoldPriceVietNam_Day
                        {
                            Name = nameStr,
                            Buy = Convert.ToDouble(valueElement.Value),
                            Sell = Convert.ToDouble(changeElement.Value),
                            VCreateDate = vCreateDate
                        };
                        context.GoldPriceVietNam_Days.Add(e);
                    }
                    else if (exchangeRateDay.VCreateDate < vCreateDate)
                    {
                        exchangeRate.Buy = Convert.ToDouble(valueElement.Value);
                        exchangeRate.Sell = Convert.ToDouble(changeElement.Value);
                        exchangeRate.VCreateDate = vCreateDate;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GetDataGoldPriceVietNam: " + ex.Message);
            }
        }

        public static void GetDataGoldPriceWorld()
        {
            StoreInfoContext context = new StoreInfoContext();
            var client = new xmlVITVSoapClient();
            try
            {
                var tygiaUSDResult = client.getVangTG_xml();
                var tygiaUSDList = tygiaUSDResult.Elements("StockInfo");
                foreach (XElement tygia in tygiaUSDList)
                {

                    var nameElement = tygia.Element("Name");//string
                    var valueElement = tygia.Element("Value");//double
                    var changeElement = tygia.Element("Change");//double
                    var percentElement = tygia.Element("Percent");//double
                    var updateTimeElement = tygia.Element("Vcreatedate");//datetime

                    var nameStr = nameElement.Value;
                    var vCreateDate = (DateTime)updateTimeElement;
                    //Cập nhật vào bảng VNDExchangeRates : tạo mới nếu không có, cập nhật nếu thời gian cập nhật cũ hơn
                    GoldPriceWorld exchangeRate = context.GoldPriceWorlds.FirstOrDefault(e => e.Name == nameStr);
                    if (exchangeRate == null)
                    {
                        GoldPriceWorld e = new GoldPriceWorld
                        {
                            Name = nameStr,
                            Value = Convert.ToDouble(valueElement.Value),
                            Change = Convert.ToDouble(changeElement.Value),
                            Percent = Convert.ToDouble(percentElement.Value),
                            VCreateDate = vCreateDate
                        };
                        context.GoldPriceWorlds.Add(e);
                    }
                    else if (exchangeRate.VCreateDate < vCreateDate)
                    {
                        exchangeRate.Value = Convert.ToDouble(valueElement.Value);
                        exchangeRate.Change = Convert.ToDouble(changeElement.Value);
                        exchangeRate.Percent = Convert.ToDouble(percentElement.Value);
                        exchangeRate.VCreateDate = vCreateDate;
                    }

                    //Cập nhật vào bảng VNDExchangeRate_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
                    GoldPriceWorld_Day exchangeRateDay = context.GoldPriceWorld_Days.FirstOrDefault(e => e.Name == nameStr && DbFunctions.TruncateTime(e.VCreateDate) == vCreateDate.Date);
                    if (exchangeRateDay == null)
                    {
                        GoldPriceWorld_Day e = new GoldPriceWorld_Day
                        {
                            Name = nameStr,
                            Value = Convert.ToDouble(valueElement.Value),
                            Change = Convert.ToDouble(changeElement.Value),
                            Percent = Convert.ToDouble(percentElement.Value),
                            VCreateDate = vCreateDate
                        };
                        context.GoldPriceWorld_Days.Add(e);
                    }
                    else if (exchangeRateDay.VCreateDate < vCreateDate)
                    {
                        exchangeRateDay.Value = Convert.ToDouble(valueElement.Value);
                        exchangeRateDay.Change = Convert.ToDouble(changeElement.Value);
                        exchangeRateDay.Percent = Convert.ToDouble(percentElement.Value);
                        exchangeRateDay.VCreateDate = vCreateDate;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GetDataGoldPriceWorld: " + ex.Message);
            }

        }

        public static void GetDataMetalPrice()
        {
            StoreInfoContext context = new StoreInfoContext();
            var client = new xmlVITVSoapClient();
            try
            {
                var tygiaUSDResult = client.getKimloai_xml();
                var tygiaUSDList = tygiaUSDResult.Elements("StockInfo");
                foreach (XElement tygia in tygiaUSDList)
                {

                    var nameElement = tygia.Element("Name");//string
                    var valueElement = tygia.Element("Value");//double
                    var changeElement = tygia.Element("Change");//double
                    var percentElement = tygia.Element("Percent");//double
                    var updateTimeElement = tygia.Element("Vcreatedate");//datetime

                    var nameStr = nameElement.Value;
                    var vCreateDate = (DateTime)updateTimeElement;
                    //Cập nhật vào bảng VNDExchangeRates : tạo mới nếu không có, cập nhật nếu thời gian cập nhật cũ hơn
                    MetalPrice exchangeRate = context.MetalPrices.FirstOrDefault(e => e.Name == nameStr);
                    if (exchangeRate == null)
                    {
                        MetalPrice e = new MetalPrice
                        {
                            Name = nameStr,
                            Value = Convert.ToDouble(valueElement.Value),
                            Change = Convert.ToDouble(changeElement.Value),
                            Percent = Convert.ToDouble(percentElement.Value),
                            VCreateDate = vCreateDate
                        };
                        context.MetalPrices.Add(e);
                    }
                    else if (exchangeRate.VCreateDate < vCreateDate)
                    {
                        exchangeRate.Value = Convert.ToDouble(valueElement.Value);
                        exchangeRate.Change = Convert.ToDouble(changeElement.Value);
                        exchangeRate.Percent = Convert.ToDouble(percentElement.Value);
                        exchangeRate.VCreateDate = vCreateDate;
                    }

                    //Cập nhật vào bảng VNDExchangeRate_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
                    MetalPrice_Day exchangeRateDay = context.MetalPrice_Days.FirstOrDefault(e => e.Name == nameStr && DbFunctions.TruncateTime(e.VCreateDate) == vCreateDate.Date);
                    if (exchangeRateDay == null)
                    {
                        MetalPrice_Day e = new MetalPrice_Day
                        {
                            Name = nameStr,
                            Value = Convert.ToDouble(valueElement.Value),
                            Change = Convert.ToDouble(changeElement.Value),
                            Percent = Convert.ToDouble(percentElement.Value),
                            VCreateDate = vCreateDate
                        };
                        context.MetalPrice_Days.Add(e);
                    }
                    else if (exchangeRateDay.VCreateDate < vCreateDate)
                    {
                        exchangeRateDay.Value = Convert.ToDouble(valueElement.Value);
                        exchangeRateDay.Change = Convert.ToDouble(changeElement.Value);
                        exchangeRateDay.Percent = Convert.ToDouble(percentElement.Value);
                        exchangeRateDay.VCreateDate = vCreateDate;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GetDataMetalPrice: " + ex.Message);
            }

        }

        public static void GetDataOilPrice()
        {
            StoreInfoContext context = new StoreInfoContext();
            var client = new xmlVITVSoapClient();
            try
            {
                var tygiaUSDResult = client.getDau_xml();
                var tygiaUSDList = tygiaUSDResult.Elements("StockInfo");
                foreach (XElement tygia in tygiaUSDList)
                {

                    var nameElement = tygia.Element("Name");//string
                    var valueElement = tygia.Element("Value");//double
                    var changeElement = tygia.Element("Change");//double
                    var percentElement = tygia.Element("Percent");//double
                    var updateTimeElement = tygia.Element("Vcreatedate");//datetime

                    var nameStr = nameElement.Value;
                    var vCreateDate = (DateTime)updateTimeElement;
                    //Cập nhật vào bảng VNDExchangeRates : tạo mới nếu không có, cập nhật nếu thời gian cập nhật cũ hơn
                    OilPrice exchangeRate = context.OilPrices.FirstOrDefault(e => e.Name == nameStr);
                    if (exchangeRate == null)
                    {
                        OilPrice e = new OilPrice
                        {
                            Name = nameStr,
                            Value = Convert.ToDouble(valueElement.Value),
                            Change = Convert.ToDouble(changeElement.Value),
                            Percent = Convert.ToDouble(percentElement.Value),
                            VCreateDate = vCreateDate
                        };
                        context.OilPrices.Add(e);
                    }
                    else if (exchangeRate.VCreateDate < vCreateDate)
                    {
                        exchangeRate.Value = Convert.ToDouble(valueElement.Value);
                        exchangeRate.Change = Convert.ToDouble(changeElement.Value);
                        exchangeRate.Percent = Convert.ToDouble(percentElement.Value);
                        exchangeRate.VCreateDate = vCreateDate;
                    }

                    //Cập nhật vào bảng VNDExchangeRate_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
                    OilPrice_Day exchangeRateDay = context.OilPrice_Days.FirstOrDefault(e => e.Name == nameStr && DbFunctions.TruncateTime(e.VCreateDate) == vCreateDate.Date);
                    if (exchangeRateDay == null)
                    {
                        OilPrice_Day e = new OilPrice_Day
                        {
                            Name = nameStr,
                            Value = Convert.ToDouble(valueElement.Value),
                            Change = Convert.ToDouble(changeElement.Value),
                            Percent = Convert.ToDouble(percentElement.Value),
                            VCreateDate = vCreateDate
                        };
                        context.OilPrice_Days.Add(e);
                    }
                    else if (exchangeRateDay.VCreateDate < vCreateDate)
                    {
                        exchangeRateDay.Value = Convert.ToDouble(valueElement.Value);
                        exchangeRateDay.Change = Convert.ToDouble(changeElement.Value);
                        exchangeRateDay.Percent = Convert.ToDouble(percentElement.Value);
                        exchangeRateDay.VCreateDate = vCreateDate;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GetDataOilPrice: " + ex.Message);
            }

        }
    }
}
