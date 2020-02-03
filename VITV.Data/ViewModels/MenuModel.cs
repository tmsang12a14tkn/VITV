using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class MenuModel
    {
        public IEnumerable<VideoCatGroup> VideoCatGroupMenu { get; set; }
        public IEnumerable<VideoCategory> VideoCatMenu { get; set; }
        public IEnumerable<ArticleCategory> ArticleCatMenu { get; set; }
    }
}