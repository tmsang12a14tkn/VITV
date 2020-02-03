using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class DashboardView
    {
        public int ReporterCount { get; set; }
        public int VideoCount { get; set; }
        public long VideoStorage { get; set; }
        public int ArticleCount { get; set; }
        public List<Video> ErrorVideos { get; set; }
        public bool HasSchedule { get; set; }

        public string ModNotification { get; set; }

    }
}