using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.Models.StoreInfo
{
    public class StockMarket
    {
        [Key]
        public string Name {get;set;}
        public string Ticker { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
    public class Stock
    {
        [Key]
        public string Ticker { get; set; }
        public string CompanyName { get; set; }
        [ForeignKey("Market")]
        public string MarketName { get; set; }
        public virtual StockMarket Market { get; set; }
        public virtual ICollection<Stock_Day> Stock_Days { get; set; }

    }
}