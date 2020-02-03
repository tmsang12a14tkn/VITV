using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.Models.StoreInfo
{
    public class Stock_Day
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Stock")]
        public string Ticker { get; set; }
        [Key, Column(Order = 2)]
        public DateTime Date { get; set; }

        public float Open { get; set; }
        public float Close { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public int Volumn { get; set; }

        public virtual Stock Stock { get; set; }
    }
    public class StockMarket_Day
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Market")]
        public string MarketName { get; set; }
        [Key, Column(Order = 2)]
        public DateTime Date { get; set; }
        public float Open { get; set; }
        public float Close { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public int Volumn { get; set; }

        public virtual StockMarket Market { get; set; }
        
        
    }
}