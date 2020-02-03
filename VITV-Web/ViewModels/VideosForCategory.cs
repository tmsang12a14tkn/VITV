using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV_Web.ViewModels
{
    public class VideoGroup
    {
        public DateTime Date { get; set; }
        public int Type { get; set; } //0: week, 1: month
        public List<Video> Videos { get; set; }
        public int CatId { get; set; }
    }

    public class VideosForCategory
    {
        public List<VideoGroup> VideoGroups { get; set; }
    }
}