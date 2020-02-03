using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class ArticleHomeView
    {
        public List<Article> Articles { get; set; }
        public List<ArticleCategory> ArticleCategories { get; set; }
        public int CountArticle { get; set; }
        
    }
}