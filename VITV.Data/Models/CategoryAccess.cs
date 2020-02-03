using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.Models
{
    public class CategoryAccess
    {
        [Key]
        [Column(Order=1)]
        public int CategoryId {get;set;}
        [Key]
        [Column(Order = 2)]
        public DateTime Date { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }

        public virtual VideoCategory Category { get; set; }
    }

    public class ArticleCategoryAccess
    {
        [Key]
        [Column(Order = 1)]
        public int CategoryId { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime Date { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }

        public virtual ArticleCategory Category { get; set; }
    }
}
