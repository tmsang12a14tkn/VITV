using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Data.ViewModels
{
    public class AccessData
    {
        public int id { get; set; }
        public string name { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int month { get; set; }
    }

    public class AccessDataChart
    {
        public string name { get; set; }
        public List<AccessData> data { get; set; }
    }

    public class CatAccessDataChart
    {
        public int id { get; set; }
        public int? typeid { get; set; }
        public string name { get; set; }
        public IEnumerable<ArrayList> data { get; set; }
    }

    public class CatAccessDataChartDetail
    {
        public int id { get; set; }
        public int? typeid { get; set; }
        public string name { get; set; }
        public ArrayList data { get; set; }
    }
}