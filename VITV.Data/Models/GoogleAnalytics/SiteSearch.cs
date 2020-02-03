using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VITV.Data.Models.GoogleAnalytics
{
    public class SiteSearch
    {
        [Key, Column(Order = 1)]
        public DateTime CreateDate { get; set; }
        [Key, Column(Order = 2)]
        public string PageUrl { get; set; }
        public int? VideoId { get; set; }
        public int? VideoCatId { get; set; }
        public double TotalUniqueSearch { get; set; }
        public double AvgSearchResultViews { get; set; }
        public double SearchExitRate { get; set; }
        public double PercentSearchRefinements { get; set; }
        public double SearchDuration { get; set; }
        public double AvgSearchDepth { get; set; }
        public int PageView { get; set; }
    }
}
