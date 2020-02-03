using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class Page
    {
        [Key]
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public int? VideoId { get; set; }
        public int? VideoCatId { get; set; }
        public int? ArticleId { get; set; }
        public int? ArticleCatId { get; set; }
        public virtual Video Video { get; set; }
        public virtual VideoCategory VideoCat { get; set; }
        public virtual Article Article { get; set; }
        public virtual ArticleCategory ArticleCat { get; set; }

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
        [Key, Column(Order = 3)]
        public string DeviceType { get; set; }
        [Key, Column(Order = 4)]
        public string DeviceModel { get; set; }
        public string DeviceVendor { get; set; }
        [Key, Column(Order = 5)]
        public string OSName { get; set; }       
        public DateTime LastAccess { get; set; }

        [ForeignKey("PageUrl")]
        public virtual Page Page { get; set; }

    }
}