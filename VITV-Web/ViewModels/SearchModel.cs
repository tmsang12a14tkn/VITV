using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class SearchModel
    {
        public string Query {get;set;}
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<Video> Videos { get; set; }

        public int ResultCount { get; set; }
    }
}