using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VITV.WebApi.Entities
{
    public class ExchangeRateEntity
    {
        
        public string Name { get; set; }
        
        public double y { get; set; }
        
        public double Change { get; set; }
        
        public double Percent { get; set; }
        
        public double x { get; set; }

    }
    //public class StockChartEntity
    //{
    //    
    //    public string name { get; set; }
    //    
    //    public float y { get; set; }
    //    
    //    public double x { get; set; }

    //}

}