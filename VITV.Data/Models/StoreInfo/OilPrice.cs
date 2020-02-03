using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Data.Models.StoreInfo
{
    public class OilPrice : ExchangeRate
    {
        public int Order { get; set; }
    }
    public class OilPrice_Day : ExchangeRate
    {
    }
}