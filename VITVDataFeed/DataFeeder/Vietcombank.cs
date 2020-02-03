using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using VITV.Data.DAL;
using VITV.Data.Models.StoreInfo;
using VITV.Data.Repositories;

namespace VITVDataFeed
{
    public static class Vietcombank
    {
        public static void GetData()
        {
            try
            {
                StoreInfoContext context = new StoreInfoContext();

                //IVNDExchangeRateRepository repository = new VNDExchangeRateRepository();
                String URLString = "http://vietcombank.com.vn/ExchangeRates/ExrateXML.aspx";
                XPathDocument doc = new XPathDocument(URLString);

                var nav = doc.CreateNavigator();
                XPathExpression expr = nav.Compile("/ExrateList/DateTime");
                var nodeIter = nav.Select(expr);
                DateTime datetime = DateTime.Now;
                foreach (XPathNavigator item in nodeIter)
                {
                    if (DateTime.TryParseExact(item.Value, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture,
                               DateTimeStyles.None, out datetime))
                        break;
                }

                var exrateNodes = nav.Select("/ExrateList/Exrate");
                foreach (XPathNavigator exrateNode in exrateNodes)
                {
                    string currencyCode = exrateNode.GetAttribute("CurrencyCode", "");
                    string currencyName = exrateNode.GetAttribute("CurrencyName", "");
                    string buyStr = exrateNode.GetAttribute("Buy", "");
                    string transferStr = exrateNode.GetAttribute("Transfer", "");
                    string sellStr = exrateNode.GetAttribute("Sell", "");
                    double buy, transfer, sell;
                    if (Double.TryParse(buyStr, out buy) && Double.TryParse(transferStr, out transfer) && Double.TryParse(sellStr, out sell))
                    {
                        //Cập nhật vào bảng USDExchangeRates : tạo mới nếu không có, cập nhật nếu thời gian cập nhật cũ hơn
                        VNDExchangeRate exchangeRate = context.VNDExchangeRates.FirstOrDefault(e => e.Name == currencyName);
                        if (exchangeRate == null)
                        {
                            VNDExchangeRate e = new VNDExchangeRate
                            {
                                Name = currencyName,
                                Code = currencyCode,
                                Buy = buy,
                                Sell = sell,
                                Transfer = transfer,
                                UpdatedTime = datetime
                            };
                            context.VNDExchangeRates.Add(e);
                        }
                        else if (exchangeRate.UpdatedTime < datetime)
                        {
                            exchangeRate.Name = currencyName;
                            exchangeRate.Buy = buy;
                            exchangeRate.Code = currencyCode;
                            exchangeRate.Sell = sell;
                            exchangeRate.Transfer = transfer;
                            exchangeRate.UpdatedTime = datetime;
                        }

                        //Cập nhật vào bảng USDExchangeRate_Days: cập nhật dữ liệu cuối cùng của mỗi ngày
                        VNDExchangeRate_Day exchangeRateDay = context.VNDExchangeRate_Days.FirstOrDefault(e => e.Name == currencyName && DbFunctions.TruncateTime(e.UpdatedTime) == datetime.Date);
                        if (exchangeRateDay == null)
                        {
                            VNDExchangeRate_Day e = new VNDExchangeRate_Day
                            {
                                Name = currencyName,
                                Buy = buy,
                                Code = currencyCode,
                                Sell = sell,
                                Transfer = transfer,
                                UpdatedTime = datetime
                            };
                            context.VNDExchangeRate_Days.Add(e);
                        }
                        else if (exchangeRateDay.UpdatedTime < datetime)
                        {
                            exchangeRateDay.Name = currencyName;
                            exchangeRateDay.Buy = buy;
                            exchangeRate.Code = currencyCode;
                            exchangeRateDay.Sell = sell;
                            exchangeRateDay.Transfer = transfer;
                            exchangeRateDay.UpdatedTime = datetime;
                        }
                    }
                    context.SaveChanges();
                };
            }
            catch (Exception ex)
            {
                Logger.Write("Vietcombank.GetData: " + ex.Message);
            }

        }
    }
}
