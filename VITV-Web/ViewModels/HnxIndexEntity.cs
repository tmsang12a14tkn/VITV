using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Web.ViewModels
{
    public class HnxIndexEntity
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double BreakingTime { get; set; }
        public float RefPrice { get; set; }
        public List<float> PriceData { get; set; }
        public HnxIndexEntity()
        {
            PriceData = new List<float>();
        }
    }

    public class EmployeePositionModel
    {
        public string NameEmp { get; set; }

        public string NamePosi { get; set; }

    }

    
}