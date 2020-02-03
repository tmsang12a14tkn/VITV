using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV_Web.Models.StoreInfo
{
    public class ExchangeRate
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Change { get; set; }
        public double Percent { get; set; }
        public DateTime VCreateDate { get; set; }
    }
}