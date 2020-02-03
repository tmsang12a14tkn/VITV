using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models.StoreInfo;

namespace VITV.Data.ViewModels
{
    public class ExchangeRateView
    {
        public List<VNDExchangeRate> vndExchangeRates{get;set;}
        public List<USDExchangeRate> usdExchangeRates { get; set; }
    }
}