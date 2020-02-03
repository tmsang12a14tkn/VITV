using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class ArticleRevision
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public DateTime ChangedDate { get; set; }
        public string Title { get; set; }
        public string ShortBrief { get; set; }
        public string ArticleContent { get; set; }
        public string CreateUserId { get; set; }
        //public int ArticleStatus { get; set; }
        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
        [ForeignKey("CreateUserId")]
        public virtual ApplicationUser CreateUser { get; set; }
    }
}
