using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class ArticlePaginationView
    {
        public int Page { get; set; }
        public int GroupId { get; set; }
        public List<Article> Articles { get; set; }
    }
}
