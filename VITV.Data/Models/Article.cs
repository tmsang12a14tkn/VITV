using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class Article
    {
        public Article()
        {
            //PublishedTime = DateTime.Now;
            Keywords = new List<Keyword>();
            Categories = new List<ArticleCategory>();
            Reporters = new HashSet<Employee>();
            ArticleRevisions = new HashSet<ArticleRevision>();
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
        
        public string ArticleContent { get; set; }
        public string ArticleDraft { get; set; }
        [Required]
        [Display(Name = "Hình")]
        public string Thumbnail { get; set; }
       
        [Display(Name="Đăng lúc")]
        public DateTime? PublishedTime { get; set; }

        public DateTime? CreatedTime { get; set; }

        [ForeignKey("Series")]
        public int? ArticleSeriesId { get; set; }

        public int Order { get; set; }

        public int SeriesOrder { get; set; }

        public int ArticleStatus { get; set; } //0: bản nháp - 1: cần duyệt - 2: cần chỉnh sửa - 3: Loại bỏ - 4: Tốt

        public bool IsPublished { get; set; }

        public String SEO_FocusKeywords { get; set; }
        public String SEO_Title { get; set; }
        public String SEO_MetaDescription { get; set; }

        public virtual ICollection<Employee> Reporters { get; set; }
        public virtual ArticleSeries Series { get; set; }
        public virtual ICollection<ArticleCategory> Categories { get; set; }
        public virtual ICollection<Keyword> Keywords { get; set; }
        public virtual ICollection<ArticleRevision> ArticleRevisions { get; set; }
        public virtual ICollection<ArticleComment> ArticleComments { get; set; }
        //public virtual ICollection<ArticleReview> ArticleReviews { get; set; }
       
    }
}