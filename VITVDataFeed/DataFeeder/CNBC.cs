using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using VITV.Data.DAL;
using VITV.Data.Models.StoreInfo;

namespace VITVDataFeed.DataFeeder
{
    public class CNBCData
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public double Change { get; set; }
        public double ChangePercent { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
    public class CNBC
    {
        public static void GetMetalData()
        {
            try
            {
                //Metal
                string urlStr = "http://quote.cnbc.com/quote-html-webservice/quote.htm?symbols=%40GC.1|%40SI.1|%40HG.1|%40PL.1|%40PA.1";
                List<CNBCData> data = GetData(urlStr);
                StoreInfoContext context = new StoreInfoContext();
                foreach (CNBCData metal in data)
                {
                    SaveMetal(context, metal);
                }
                context.SaveChanges();
            }
            catch(Exception e)
            {
                Logger.Write("CNBC.GetMetalData: " + e.Message);
            }
        }
        public static void GetOilData()
        {
            try
            {
                string urlStr = "http://quote.cnbc.com/quote-html-webservice/quote.htm?symbols=%40CL.1|%40LCO.1|%40NG.1|%40RB.1|%40HO.1|%40AC.1|%40UXX.1|%40QL.1";
                List<CNBCData> data = GetData(urlStr);
                StoreInfoContext context = new StoreInfoContext();
                foreach (CNBCData oil in data)
                {
                    SaveOil(context, oil);
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.Write("CNBC.GetOilData: " + e.Message);
            }
        }
        private static List<CNBCData> GetData(string urlStr)
        {
            List<CNBCData> result = new List<CNBCData>();
            try
            {    
                XPathDocument doc = new XPathDocument(urlStr);
                var nav = doc.CreateNavigator();

                XmlNamespaceManager nsMgr = new XmlNamespaceManager(nav.NameTable);
                nsMgr.AddNamespace("ns", "http://quote.cnbc.com/services/MultiQuote/2006");

                var quoteNodes = nav.Select("/ns:QuickQuoteResult/ns:QuickQuote", nsMgr);


                foreach (XPathNavigator quoteNode in quoteNodes)
                {
                    var name = quoteNode.SelectSingleNode("ns:shortName", nsMgr).Value;
                    var valueStr = quoteNode.SelectSingleNode("ns:last", nsMgr).Value;
                    var timeStr = quoteNode.SelectSingleNode("ns:last_time", nsMgr).Value;
                    var changeStr = quoteNode.SelectSingleNode("ns:change", nsMgr).Value;
                    var changePercentStr = quoteNode.SelectSingleNode("ns:change_pct", nsMgr).Value;

                    var value = Convert.ToDouble(valueStr);
                    var time = Convert.ToDateTime(timeStr);
                    var change = Convert.ToDouble(changeStr);
                    var changePercent = Convert.ToDouble(changePercentStr);

                    result.Add(new CNBCData { Name = name, UpdatedTime = time, Value = value, Change = change, ChangePercent = changePercent });
                }
            }
            catch (Exception ex)
            {
                Logger.Write("CNBC.GetData: " + ex.Message);
            }
            return result;
        }


        static void SaveOil(StoreInfoContext context, CNBCData data)
        {
            //Cập nhật vào bảng OilPrices : tạo mới nếu không có, cập nhật nếu thời gian cập nhật cũ hơn
            OilPrice oilPrice = context.OilPrices.FirstOrDefault(e => e.Name == data.Name);
            if (oilPrice == null)
            {
                OilPrice e = new OilPrice
                {
                    Name = data.Name,
                    Value = data.Value,
                    Change = data.Change,
                    Percent = data.ChangePercent,
                    VCreateDate = data.UpdatedTime
                };
                context.OilPrices.Add(e);
            }
            else if (oilPrice.VCreateDate < data.UpdatedTime)
            {
                oilPrice.Value = data.Value;
                oilPrice.Change = data.Change;
                oilPrice.Percent = data.ChangePercent;
                oilPrice.VCreateDate = data.UpdatedTime;
            }

            //Cập nhật vào bảng OilPrice_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
            OilPrice_Day oilPriceDay = context.OilPrice_Days.FirstOrDefault(e => e.Name == data.Name && DbFunctions.TruncateTime(e.VCreateDate) == data.UpdatedTime.Date);
            if (oilPriceDay == null)
            {
                OilPrice_Day e = new OilPrice_Day
                {
                    Name = data.Name,
                    Value = data.Value,
                    Change = data.Change,
                    Percent = data.ChangePercent,
                    VCreateDate = data.UpdatedTime
                };
                context.OilPrice_Days.Add(e);
            }
            else if (oilPriceDay.VCreateDate < data.UpdatedTime)
            {
                oilPrice.Value = data.Value;
                oilPrice.Change = data.Change;
                oilPrice.Percent = data.ChangePercent;
                oilPrice.VCreateDate = data.UpdatedTime;
            }
        }

        static void SaveMetal(StoreInfoContext context, CNBCData data)
        {
            //Cập nhật vào bảng MetalPrices : tạo mới nếu không có, cập nhật nếu thời gian cập nhật cũ hơn
            MetalPrice metalPrice = context.MetalPrices.FirstOrDefault(e => e.Name == data.Name);
            if (metalPrice == null)
            {
                MetalPrice e = new MetalPrice
                {
                    Name = data.Name,
                    Value = data.Value,
                    Change = data.Change,
                    Percent = data.ChangePercent,
                    VCreateDate = data.UpdatedTime
                };
                context.MetalPrices.Add(e);
            }
            else if (metalPrice.VCreateDate < data.UpdatedTime)
            {
                metalPrice.Value = data.Value;
                metalPrice.Change = data.Change;
                metalPrice.Percent = data.ChangePercent;
                metalPrice.VCreateDate = data.UpdatedTime;
            }

            //Cập nhật vào bảng MetalPrice_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
            MetalPrice_Day metalPriceDay = context.MetalPrice_Days.FirstOrDefault(e => e.Name == data.Name && DbFunctions.TruncateTime(e.VCreateDate) == data.UpdatedTime.Date);
            if (metalPriceDay == null)
            {
                MetalPrice_Day e = new MetalPrice_Day
                {
                    Name = data.Name,
                    Value = data.Value,
                    Change = data.Change,
                    Percent = data.ChangePercent,
                    VCreateDate = data.UpdatedTime
                };
                context.MetalPrice_Days.Add(e);
            }
            else if (metalPriceDay.VCreateDate < data.UpdatedTime)
            {
                metalPrice.Value = data.Value;
                metalPrice.Change = data.Change;
                metalPrice.Percent = data.ChangePercent;
                metalPrice.VCreateDate = data.UpdatedTime;
            }
        }
    }
}
