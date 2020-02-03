using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class SearchModel
    {
        public string Query {get;set;}
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<Video> Videos { get; set; }

        public int ResultCount { get; set; }
    }
}