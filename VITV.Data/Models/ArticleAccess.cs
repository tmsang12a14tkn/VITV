using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VITV.Data.Models
{
    public class ArticleAccess
    {
        [Key]
        [Column(Order = 1)]
        public int ArticleId { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime Date { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }

        public virtual Article Article { get; set; }
    }
}
