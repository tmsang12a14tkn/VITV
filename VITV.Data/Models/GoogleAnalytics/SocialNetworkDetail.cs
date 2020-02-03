using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VITV.Data.Models.GoogleAnalytics
{
    public class SocialNetworkDetail
    {
        [Key, Column(Order = 1)]
        public DateTime CreateDate { get; set; }
        [Key, Column(Order = 2)]
        public string Name { get; set; }
        [Key, Column(Order = 3)]
        public string PageUrl { get; set; }
        public string Url { get; set; }
        public int? VideoId { get; set; }
        public int? VideoCatId { get; set; }
        public double SessionCount { get; set; }
        public double PageView { get; set; }
        public double PageViewPerSession { get; set; }
        public double AvgSessionDurration { get; set; }
    }
}
