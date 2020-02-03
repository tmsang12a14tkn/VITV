using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class PageAccessDate
    {
        public DateTime Date { get; set; }
        public int IPCount { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }
        public int HomePageCount { get; set; }
    }
    public class PeriodVideoAccess
    {
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }
        public string Url { get; set; }
        public string Thumbnail {get;set;}
        public string Title { get; set; }
        
    }
    public enum PeriodType
    {
        Daily,
        Weekly,
        Monthly
    }
    public class VideoAccessDateView
    {
        public List<PeriodVideoAccess> AccessData { get; set; }
        public DateTime Date { get; set; }
        public PeriodType PeriodType { get; set; }
    }
}