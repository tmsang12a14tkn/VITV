using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class Article
    {
        public Article()
        {
            //PublishedTime = DateTime.Now;
            Keywords = new List<Keyword>();
        }

        public int Id { get; set; }
        [Index("TitleUniqueIndex", IsUnique = true)]
        [StringLength(450)]
        public string UniqueTitle { get; set; }
        [Required]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Tóm tắt")]
        public string ShortBrief { get; set; }
        [Required]
        public string ArticleContent { get; set; }
        [Required]
        [Display(Name = "Hình")]
        public string Thumbnail { get; set; }
        [Required]
        [Display(Name="Đăng lúc")]
        public DateTime PublishedTime { get; set; }

        [Required, ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required, ForeignKey("Reporter")]
        public int ReporterId { get; set; }

        public bool PublishImmediately { get; set; }
        public bool IsPublished { get; set; }

        public String SEO_FocusKeywords { get; set; }
        public String SEO_Title { get; set; }
        public String SEO_MetaDescription { get; set; }

        public virtual Reporter Reporter { get; set; }
        public virtual ArticleCategory Category { get; set; }
        public virtual ICollection<Keyword> Keywords { get; set; }
    }
}