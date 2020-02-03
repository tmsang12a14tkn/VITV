using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VITV.Data.DAL;
using VITV.Data.Models.StoreInfo;

namespace VITVDataFeed
{
    public class Cophieu68
    {
        public static void ParseCompanyData()
        {
            StoreInfoContext context = new StoreInfoContext();
            var reader = new StreamReader("company_list.csv");
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var values = line.Split('\t');
                Stock stock = new Stock
                {
                    Ticker = values[0],
                    MarketName = values[1],
                    CompanyName = values[22]
                };
                context.Stocks.Add(stock);
            }
            context.SaveChanges();
        }
        public static void ParseDailyData()
        {
             StoreInfoContext context = new StoreInfoContext();
            var reader = new StreamReader("metastock_all_data.txt");
            while(!reader.EndOfStream)
            {
                var stockParams = reader.ReadLine().Split(',');
                if (stockParams.Length == 7)
                {
                    try
                    {
                        var ticker = stockParams[0];
                        DateTime date;
                        float open, high, low, close;
                        int volumn;

                        if (DateTime.TryParseExact(stockParams[1], "yyyyMMdd", new CultureInfo("en-US"), DateTimeStyles.None, out date)
                            && float.TryParse(stockParams[2], out open)
                            && float.TryParse(stockParams[3], out high)
                            && float.TryParse(stockParams[4], out low)
                            && float.TryParse(stockParams[5], out close)
                            && int.TryParse(stockParams[6], out volumn))
                        {
                            if (context.Stock_Days.Find(ticker, date) == null)
                            {
                                var stockDaily = new Stock_Day()
                                {
                                    Date = date,
                                    Open = open,
                                    High = high,
                                    Low = low,
                                    Close = close,
                                    Volumn = volumn,
                                    Ticker = ticker
                                };
                                context.Stock_Days.Add(stockDaily);
                                context.SaveChanges();
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Logger.Write(e.Message);
                    }
                }
            }
        }

        public static void ParseMarketDailyData()
        {
            StoreInfoContext context = new StoreInfoContext();
            var dictMarkets = new Dictionary<string, string> { { "^HASTC", "HNX" }, { "^VNINDEX", "HOSE" } };
            var indexFiles = new string[] { "^vnindex.txt", "^hastc.txt" };
            foreach(string fileName in indexFiles)
            {
                var reader = new StreamReader(fileName);

                while (!reader.EndOfStream)
                {
                    var stockParams = reader.ReadLine().Split(',');
                    if (stockParams.Length == 7)
                    {
                        try
                        {
                            var ticker = stockParams[0];
                            var marketName = dictMarkets[ticker];
                            DateTime date;
                            float open, high, low, close;
                            int volumn;

                            if (DateTime.TryParseExact(stockParams[1], "yyyyMMdd", new CultureInfo("en-US"), DateTimeStyles.None, out date)
                                && float.TryParse(stockParams[2], out open)
                                && float.TryParse(stockParams[3], out high)
                                && float.TryParse(stockParams[4], out low)
                                && float.TryParse(stockParams[5], out close)
                                && int.TryParse(stockParams[6], out volumn))
                            {
                                if (context.StockMarket_Days.Find(marketName, date) == null)
                                {
                                    var marketDaily = new StockMarket_Day()
                                    {
                                        Date = date,
                                        Open = open,
                                        High = high,
                                        Low = low,
                                        Close = close,
                                        Volumn = volumn,
                                        MarketName = marketName
                                    };
                                    context.StockMarket_Days.Add(marketDaily);
                                    context.SaveChanges();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Write(e.Message);
                        }
                    }
                }
            }
            
        }

    }
}
