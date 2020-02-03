using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class MenuModel
    {
        public IEnumerable<VideoCatGroup> VideoCatGroupMenu { get; set; }
        public IEnumerable<VideoCategory> VideoCatMenu { get; set; }
        public IEnumerable<ArticleCategory> ArticleCatMenu { get; set; }
    }
}