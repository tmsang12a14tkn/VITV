using System;
using System.Collections.Generic;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class ArticleFilterView
    {        
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryUniqueTitle { get; set; }
        public string UniqueTitle { get; set; }        
        public string Title { get; set; }        
        public string ShortBrief { get; set; }       
        public string Thumbnail { get; set; }        
        public DateTime PublishedTime { get; set; }
    }
}