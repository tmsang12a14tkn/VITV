using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.Models
{
    public class VideoRate
    {
        [Key]
        [ForeignKey("Video")]
        [Column(Order=1)]
        public int VideoId { get; set; }
        [Key]
        [ForeignKey("User")]
        [Column(Order = 2)]
        public string UserId { get; set; }
        public double Rate1 { get; set; }
        public double Rate2 { get; set; }
        public double Rate3 { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Video Video { get; set; }
    }
}
