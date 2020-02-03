using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class SpecialEventDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class SpecialEventView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BarBkgr { get; set; }

        public List<Video> Videos { get; set; }
    }
}