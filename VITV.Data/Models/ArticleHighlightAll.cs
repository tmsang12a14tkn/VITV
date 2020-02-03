using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class ArticleHighlightAll
    {
        public ArticleHighlightAll()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        [Key]
        [ForeignKey("Article")]
        public int Id { get; set; }

        public int HighlightType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public int Order { get; set; }

        public virtual Article Article { get; set; }
    }
}
