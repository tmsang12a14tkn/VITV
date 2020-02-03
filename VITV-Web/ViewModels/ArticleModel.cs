using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VITV_Web.ViewModels
{
    public class ArticleModel
    {
        public ArticleModel()
        {
            PublishedTime = DateTime.Now;
            PublishImmediately = true;
            IsPublished = false;
        }

        public int Id { get; set; }
        [Required(ErrorMessage="Bắt buộc")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Bắt buộc")]
        [Display(Name = "Tóm tắt")]
        public string ShortBrief { get; set; }

        [Required]
        [Display(Name = "Ảnh đại diện")]
        public string Thumbnail { get; set; }

        [Required]
        [Display(Name = "Nội dung bài viết")]
        public string ArticleContent { get; set; }

        [Required]
        public bool PublishImmediately { get; set; }

        [Required]
        [Display(Name = "Ngày đăng"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime PublishedTime { get; set; }

        [Required]
        [Display(Name = "Chuyên mục")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "Phóng viên")]
        public int ReporterId { get; set; }

        [Display(Name = "Từ khóa")]
        public string Keywords { get; set; }

        public bool IsPublished { get; set; }

        public String SEO_FocusKeywords { get; set; }
        public String SEO_Title { get; set; }
        public String SEO_MetaDescription { get; set; }

        public String GetSEOTitle()
        {
            return String.IsNullOrWhiteSpace(SEO_Title) ? String.Format("{0} - VITV", Title) : SEO_Title;
        }
        public String GetSEOMetaDescription()
        {
            var description = String.IsNullOrWhiteSpace(SEO_MetaDescription) ? ShortBrief : SEO_MetaDescription;
            if(description.Length > 160)
            {
                description = description.Substring(0, 160);      
            }
            return description;
        }
    }

    //public class ArticleMapping : IStartable
    //{
    //    public void Start()
    //    {
    //        Mapper.CreateMap<ArticleModel, Article>();
    //        Mapper.CreateMap<Article, ArticleModel>();
    //    }
    //}
}