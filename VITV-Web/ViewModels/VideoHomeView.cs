using System.Collections.Generic;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class VideoHomeView
    {
        public SearchVideoModel SearchVideoModel { get; set; }
        public List<VideoCatGroup> VideoCatGroups { get; set; }
        public List<SpecialEventDetail> SpecialEvents { get; set; }
        public VideoFilterView VideoFilterView { get; set; }
    }
}