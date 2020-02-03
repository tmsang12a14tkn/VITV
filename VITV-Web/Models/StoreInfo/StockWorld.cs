using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.Models.StoreInfo
{
    public class StockWorld
    {
        [Key]
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
        public int Volumn { get; set; }
        public DateTime VCreateDate { get; set; }
        public int IsShow { get; set; }
        public int Order { get; set; }
    }
    public class StockWorld_Day
    {
        [Key, Column(Order = 1)]
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
        [Key, Column(Order = 2)]
        public DateTime VCreateDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}