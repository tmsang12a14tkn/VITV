using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV_Web.Models.StoreInfo
{
    public class VNDExchangeRate
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Buy { get; set; }
        public double Sell { get; set; }
        public double Transfer { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int Order { get; set; }
    }
    public class VNDExchangeRate_Day
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Buy { get; set; }
        public double Sell { get; set; }
        public double Transfer { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}