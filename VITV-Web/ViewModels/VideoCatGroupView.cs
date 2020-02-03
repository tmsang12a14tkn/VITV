using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class VideoCatGroupView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UniqueTitle { get; set; }
        public List<Video> NewestVideos { get; set; }
    }
}