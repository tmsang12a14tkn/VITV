using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace VITV.Data.Models.StoreInfo
{
    public class StockMarket_RealTime
    {
        [Key]
        public string Ticker { get; set; }
        public float Close { get; set; }
        public int Volumn { get; set; }
        public float PriceChange { get; set; }
        public float PercentChange { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsShown { get; set; }
        public int Order { get; set; }
    }
}