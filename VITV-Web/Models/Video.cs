using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class Video
    {
        public Video()
        {
            PublishedTime = DateTime.Now;
            Keywords = new List<Keyword>();
            Reporters = new HashSet<Reporter>();
            IsHot = false;
            
        }

        public int Id { get; set; }

        [Index("TitleUniqueIndex", IsUnique = true)]
        [StringLength(450)]
        public string UniqueTitle { get; set; }

        [Required]
        public string Thumbnail { get; set; }

        public string MobileThumbnail { get; set; }

        [Required]
        public string Url { get; set; }
        
        public string YoutubeUrl { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        [Required]
        public DateTime PublishedTime { get; set; }

        [Required, ForeignKey("Category")]

        public int CategoryId { get; set; }

        public int? SpecialEventId { get; set; }

        [Required]
        public bool IsHot { get; set; }

        public virtual VideoCategory Category { get; set; }
        public virtual SpecialEvent SpecialEvent { get; set; }
        
        public virtual ICollection<Keyword> Keywords { get; set; }

        public virtual ICollection<ProgramScheduleDetail> ScheduleDetails { get; set; }
        public virtual ICollection<Reporter> Reporters { get;set; }

    }
}