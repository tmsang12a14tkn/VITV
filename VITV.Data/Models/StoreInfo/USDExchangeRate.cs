using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models.StoreInfo
{
    public class USDExchangeRate
    {
        [Key]
        public string Name { get; set; }
        public double Value { get; set; }
        public double Change { get; set; }
        public double Percent { get; set; }
        public DateTime VCreateDate { get; set; }
        public int Order { get; set; }
    }
    public class USDExchangeRate_Day
    {
        [Key, Column(Order = 1)]
        public string Name { get; set; }
        public double Value { get; set; }
        public double Change { get; set; }
        public double Percent { get; set; }
        [Key, Column(Order = 2)]
        public DateTime VCreateDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}