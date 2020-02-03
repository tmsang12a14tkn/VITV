using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeftFixedBkgr { get; set; }
        public string RightFixedBkgr { get; set; }

        public bool RepeatYear { get; set; }
    }
}