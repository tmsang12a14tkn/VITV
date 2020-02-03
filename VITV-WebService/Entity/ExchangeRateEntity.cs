using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VITV.DataService.Entity
{
    public class ExchangeRateEntity
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double y { get; set; }
        [DataMember]
        public double Change { get; set; }
        [DataMember]
        public double Percent { get; set; }
        [DataMember]
        public double x { get; set; }

    }
    //public class StockChartEntity
    //{
    //    [DataMember]
    //    public string name { get; set; }
    //    [DataMember]
    //    public float y { get; set; }
    //    [DataMember]
    //    public double x { get; set; }

    //}

}