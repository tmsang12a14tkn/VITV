using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class ArticleHLCustomView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public bool IsSeries { get; set; }
        public bool IsExpire { get; set; }
        public int Order { get; set; }
    }

    public class ArticleHighlightView
    {
        public List<ArticleHLCustomView> HLAlls { get; set; }
        public List<ArticleHLCustomView> HLCats { get; set; }
    }
}
