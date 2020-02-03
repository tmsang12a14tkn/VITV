using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models.StoreInfo
{
   
    public class GoldPriceVietNam
    {
        [Key]
        public string Name { get; set; }
        public double Buy { get; set; }
        public double Sell { get; set; }
        public double YesterdayBuy { get; set; }
        public double YesterdaySell { get; set; }
        public DateTime VCreateDate { get; set; }
        public int IsShow { get; set; }//0: ẩn, 1: hiện
        public int Order { get; set; }
        public string Type { get; set; }
    }
    public class GoldPriceVietNam_Day
    {
        [Key, Column(Order=1)]
        public string Name { get; set; }
        public double Buy { get; set; }
        public double Sell { get; set; }
        [Key, Column(Order = 2)]
        public DateTime VCreateDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}