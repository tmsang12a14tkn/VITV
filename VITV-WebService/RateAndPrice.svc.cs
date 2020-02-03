using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VITV.Data.DAL;
using VITV.DataService.Contract;
using VITV.DataService.Entity;

namespace VITV.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RateAndPrice" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RateAndPrice.svc or RateAndPrice.svc.cs at the Solution Explorer and start debugging.
    public class RateAndPrice : IRateAndPrice
    {
        public IEnumerable<ExchangeRateEntity> GetUSDExchangeRate(string name)
        {
            name = name.Replace(":", "/");
            var context = new StoreInfoContext();
            var result = context.USDExchangeRate_Days.Where(r => r.Name == name).ToList();
           return Mapper.Map<IEnumerable<ExchangeRateEntity>>(result);
        }
        //public IEnumerable<StockChartEntity> GetStockBy(string name)
        //{
        //    var context = new StoreInfoContext();
        //    var result = context.Stock_Days.Where(r => r.Ticker == name).ToList();
        //    return Mapper.Map<IEnumerable<StockChartEntity>>(result);
        //}
    }
}
