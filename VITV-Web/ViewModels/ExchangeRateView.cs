using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV_Web.Models.StoreInfo;

namespace VITV_Web.ViewModels
{
    public class ExchangeRateView
    {
        public List<VNDExchangeRate> vndExchangeRates{get;set;}
        public List<USDExchangeRate> usdExchangeRates { get; set; }
    }
}