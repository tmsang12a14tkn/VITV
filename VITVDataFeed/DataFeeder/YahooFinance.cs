using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using VITV.Data.DAL;
using VITV.Data.Models.StoreInfo;

namespace VITVDataFeed
{
    public static class YahooFinance
    {
        public static void GetCurrencyData()
        {
            XPathDocument doc = new XPathDocument("http://finance.yahoo.com/webservice/v1/symbols/allcurrencies/quote");
            XPathNavigator nav = doc.CreateNavigator();
            var resourceIter = nav.Select("/list/resources/resource");

            try
            {
                while (resourceIter.MoveNext())
                {
                    XPathNavigator nav2 = resourceIter.Current.Clone();
                    var name = nav2.SelectSingleNode("field[@name='name']").Value;
                    var price = nav2.SelectSingleNode("field[@name='price']").Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void GetStockData()
        {
            //Dow Jones Industrial Average: http://finance.yahoo.com/webservice/v1/symbols/%5EDJI/quote
            GetData("%5EDJI");
            //STXE6 EUR P http://finance.yahoo.com/webservice/v1/symbols/%5ESTOXX/quote
            GetData("%5ESTOXX");
            //NASDAQ Composite http://finance.yahoo.com/webservice/v1/symbols/%5EIXIC/quote
            GetData("%5EIXIC");
            //S&P 500 http://finance.yahoo.com/webservice/v1/symbols/%5EGSPC/quote
            GetData("%5EGSPC");
            //CAC 40 http://finance.yahoo.com/webservice/v1/symbols/%5EFCHI/quote
            GetData("%5EFCHI");
            //FTSE 100 http://finance.yahoo.com/webservice/v1/symbols/%5EFTSE/quote
            GetData("%5EFTSE");
            //NIKKEI 225 (NIKKEI) http://finance.yahoo.com/webservice/v1/symbols/%5EN225/quote
            GetData("%5EN225");
            //Hang Seng http://finance.yahoo.com/webservice/v1/symbols/%5EHSI/quote
            GetData("%5EHSI");
            //DAX Index http://finance.yahoo.com/webservice/v1/symbols/%5EGDAXI/quote
            GetData("%5EGDAXI");
            ////Shanghai Shenzhen CSI 300 Index http://finance.yahoo.com/webservice/v1/symbols/000300.SS/quote
            GetData("000300.SS");
        }

        public static void GetData(string code)
        {
            StoreInfoContext context = new StoreInfoContext();
            XPathDocument doc = new XPathDocument("http://finance.yahoo.com/webservice/v1/symbols/" + code + "/quote");
            XPathNavigator nav = doc.CreateNavigator();
            var resourceIter = nav.SelectSingleNode("/list/resources/resource");

            try
            {
                var name = resourceIter.SelectSingleNode("field[@name='name']").Value;
                var price = resourceIter.SelectSingleNode("field[@name='price']").Value;
                var symbol = resourceIter.SelectSingleNode("field[@name='symbol']").Value;
                var utcTime = resourceIter.SelectSingleNode("field[@name='utctime']").Value;
                var volumn = resourceIter.SelectSingleNode("field[@name='volumn']").Value;

                var utctime = Convert.ToDateTime(utcTime);
                StockWorld stock = context.StockWorlds.FirstOrDefault(e => e.Name == name);
                if (stock == null)
                {
                    StockWorld e = new StockWorld
                    {
                        Name = name,
                        Symbol = symbol,
                        Volumn = Convert.ToInt32(volumn),
                        Price = Convert.ToDouble(price),
                        VCreateDate = utctime
                    };
                    context.StockWorlds.Add(e);
                }
                else if (stock.VCreateDate < utctime)
                {
                    stock.Symbol = symbol;
                    stock.Volumn = Convert.ToInt32(volumn);
                    stock.Price = Convert.ToDouble(price);
                    stock.VCreateDate = utctime;
                }

                //Cập nhật vào bảng USDExchangeRate_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
                StockWorld_Day stock_Day = context.StockWorld_Days.FirstOrDefault(e => e.Name == name && DbFunctions.TruncateTime(e.VCreateDate) == utctime.Date);
                if (stock_Day == null)
                {
                    StockWorld_Day e = new StockWorld_Day
                    {
                        Name = name,
                        Symbol = symbol,
                        Price = Convert.ToDouble(price),
                        VCreateDate = utctime.Date,
                        LastUpdate = utctime
                    };
                    context.StockWorld_Days.Add(e);
                }
                else if (stock_Day.LastUpdate < utctime)
                {
                    stock_Day.Price = Convert.ToDouble(price);
                    stock_Day.LastUpdate = utctime;
                    
                }
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                Logger.Write("YahooFinance.GetData: " + ex.Message);
            }
        }
    }
}
