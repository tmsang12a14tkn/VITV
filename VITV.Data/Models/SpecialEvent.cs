using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class SpecialEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BarBkgr { get; set; }

        public virtual ICollection<Video> Videos { get; set; }
    }
}