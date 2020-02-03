using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VITV.Data.Models
{
    public class SitePage
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Slug { get; set; }
        [Required]
        [AllowHtml]
        public string MainContent { get; set; }
        [Required]
        public string CreatedUserId { get; set; }
        //public DateTime CreatedTime { get; set; }
        public DateTime Date { get; set; }
        public DateTime? Modified { get; set; }

        [ForeignKey("CreatedUserId")]
        public virtual ApplicationUser CreatedUser { get; set; }
    }
}
