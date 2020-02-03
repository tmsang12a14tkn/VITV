using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }

        public virtual ICollection<VNDExchangeRate> VNDExchangeRates { get; set; }
    }
}