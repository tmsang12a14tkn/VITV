using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class Keyword
    {
        public Keyword()
        {
            Videos = new List<Video>();
            Articles = new List<Article>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}