using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class ArticleHomeView
    {
        public List<Article> Articles { get; set; }
        public List<ArticleCategory> ArticleCategories { get; set; }
        public int CountArticle { get; set; }
        
    }
}