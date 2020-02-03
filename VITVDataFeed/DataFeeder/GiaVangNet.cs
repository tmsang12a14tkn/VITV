using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VITV.Data.DAL;
using VITV.Data.Models.StoreInfo;

namespace VITVDataFeed.DataFeeder
{
    public static class GiaVangNet
    {
        public static void GetData()
        {
            StoreInfoContext context = new StoreInfoContext();
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load("http://vip2.giavang.net/teline2/data/GoldenPrice2.php");
                var vnGoldItems = doc.DocumentNode.Descendants().Where(x => x.Name == "tr" && x.Descendants("td").Count() == 3);
                var worldGoldItem = doc.DocumentNode.Descendants().FirstOrDefault(x => x.Name == "tr" && x.Descendants("td").Count() == 4);

                var updatedTime = DateTime.Now;

                var wBuy = worldGoldItem.Descendants("td").ElementAt(0).InnerText;
                var wSell = worldGoldItem.Descendants("td").ElementAt(0).InnerText;
                var wName = "VÀNG THẾ GIỚI";
                SaveRate(context, wName, wBuy, wSell, updatedTime);

                foreach (HtmlNode item in vnGoldItems)
                {

                    var name = item.Descendants("td").ElementAt(0).InnerText;
                    var buy = item.Descendants("td").ElementAt(1).InnerText;
                    var sell = item.Descendants("td").ElementAt(2).InnerText;

                    SaveRate(context, name, buy, sell, updatedTime);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GiaVangNet.GetData: " + ex.Message);
            }

        }
        static void SaveRate(StoreInfoContext context, string name, string buy, string sell, DateTime updatedTime)
        {
            try
            {
                //Cập nhật vào bảng VNDExchangeRates : tạo mới nếu không có, cập nhật nếu thời gian cập nhật cũ hơn
                GoldPriceVietNam exchangeRate = context.GoldPriceVietNams.FirstOrDefault(e => e.Name == name);
                var buyValue = Convert.ToDouble(buy);
                var sellValue = Convert.ToDouble(sell);
                if (exchangeRate == null)
                {
                    GoldPriceVietNam e = new GoldPriceVietNam
                    {
                        Name = name,
                        Buy = buyValue,
                        Sell = sellValue,
                        VCreateDate = updatedTime
                    };
                    context.GoldPriceVietNams.Add(e);
                }
                else if (exchangeRate.VCreateDate < updatedTime)
                {
                    if (exchangeRate.VCreateDate.Date != updatedTime.Date)
                    {
                        exchangeRate.YesterdayBuy = exchangeRate.Buy;
                        exchangeRate.YesterdaySell = exchangeRate.Sell;
                    }
                    exchangeRate.Buy = buyValue;
                    exchangeRate.Sell = sellValue;
                    exchangeRate.VCreateDate = updatedTime;

                }

                //Cập nhật vào bảng VNDExchangeRate_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
                GoldPriceVietNam_Day exchangeRateDay = context.GoldPriceVietNam_Days.FirstOrDefault(e => e.Name == name && DbFunctions.TruncateTime(e.VCreateDate) == updatedTime.Date);
                if (exchangeRateDay == null)
                {
                    GoldPriceVietNam_Day e = new GoldPriceVietNam_Day
                    {
                        Name = name,
                        Buy = buyValue,
                        Sell = sellValue,
                        VCreateDate = updatedTime.Date,
                        LastUpdated = updatedTime
                    };
                    context.GoldPriceVietNam_Days.Add(e);
                }
                else if (exchangeRateDay.LastUpdated < updatedTime)
                {
                    exchangeRateDay.Buy = buyValue;
                    exchangeRateDay.Sell = sellValue;
                    exchangeRateDay.LastUpdated = updatedTime;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("GiaVangNet.SaveRate: {0} - {1}", name, ex.Message));
            }
        }
    }
}
