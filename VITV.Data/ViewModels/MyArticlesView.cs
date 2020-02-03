using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class MyArticlesView
    {
        public List<Article> NeedEditList { get; set; }
        public List<Article> NeedReviewList { get; set; }
        public List<Article> GoodList { get; set; }
        public List<Article> RejectList { get; set; }
        public List<ArticleCustomDateView> PublishedList { get; set; }
    }
    
}
