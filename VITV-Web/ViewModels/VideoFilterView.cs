using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public enum RangeType: int
    {
        All = 1,
        Day = 2,
        Week = 3,
        OneMonth = 4,
        ThreeMonth = 5,
        SixMonth = 6,
        Custom = 7
    }
    public class VideoFilterView
    {
        public string Category { get; set; }
        //public string Type { get; set; }
        public RangeType Range { get; set; }
        public string Reporter { get; set; }

        public int PageCount { get; set; }
        public int Page { get; set; }
        public int? EventId { get; set; }
        public string Title { get; set; }
        public DateTime? End { get; set; }
        public DateTime? Begin { get; set; }
        public List<Video> Videos { get; set; }
    }
}