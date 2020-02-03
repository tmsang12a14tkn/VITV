using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VITV.Data.Models
{
    public class VideoAccess
    {
        [Key]
        [Column(Order = 1)]
        public int VideoId { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime Date { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }

        public virtual Video Video { get; set; }
    }
}
