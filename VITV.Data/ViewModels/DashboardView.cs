using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class DashboardView
    {
        public int ReporterCount { get; set; }
        public int VideoCount { get; set; }
        public long VideoStorage { get; set; }
        public int ArticleCount { get; set; }
        public List<Video> ErrorVideos { get; set; }
        public bool HasSchedule { get; set; }
        public List<ArticleHighlightAll> HLAlls { get; set; }
        public List<ArticleHighlightCat> HLCats { get; set; }

        public string ModNotification { get; set; }

    }
}