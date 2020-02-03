using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class HomeModel
    {
        public string ShortIntro { get; set; }

        public IEnumerable<Video> HotVideos { get; set; }

        public IEnumerable<Article> HotArticles { get; set; }

        public RateAndPrices RateAndPrices { get; set; }

        public List<Partner> Partners { get; set; }

        public List<VideoCatGroupView> VideoCatGroups { get; set; }

        public SpecialEventView SpecialEvent { get; set; }
    }
}