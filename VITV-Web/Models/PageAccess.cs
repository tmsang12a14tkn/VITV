using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class Page
    {
        [Key]
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }

    }
    public class PageAccess
    {
        [Key, Column(Order = 0)]
        public string PageUrl { get; set; }
        [Key, Column(Order = 1)]
        public string IPAdress { get; set; }
        [Key, Column(Order = 2)]
        public DateTime Date { get; set; }
        public string Thumbnail { get; set; }
        public int Count { get; set; }

        public DateTime LastAccess { get; set; }

        [ForeignKey("PageUrl")]
        public virtual Page Page { get; set; }

    }
}