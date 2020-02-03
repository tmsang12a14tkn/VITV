using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.ViewModels
{
    public class TreeModel
    {
        public TreeModel()
        {
            data = new List<TreeModel>();
        }

        public string name { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public List<TreeModel> data { get; set; }
    }
}