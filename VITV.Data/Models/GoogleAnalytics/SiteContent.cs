using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VITV.Data.Models.GoogleAnalytics
{
    public class SiteContent
    {
        [Key, Column(Order = 1)]
        public DateTime CreateDate { get; set; }
        [Key, Column(Order = 2)]
        public string PageUrl { get; set; }
        public int? VideoId { get; set; }
        public int? VideoCatId { get; set; }
        public double PageViews { get; set; }
        public double UniquePageViews { get; set; }
        public double AvgTimeOnPage { get; set; }
        public double Entrances { get; set; }
        public double BounceRate { get; set; }
        public double ExitRate { get; set; }
    }
}

