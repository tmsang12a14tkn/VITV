using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VITV.Data.ViewModels
{
    //0: bản nháp - 1: cần duyệt - 2: cần chỉnh sửa - 3: Loại bỏ - 4: Tốt
    public enum ArticleStatus : int
    {
        NeedReview = 1,
        NeedEdit = 2,
        Reject = 3,
        Good = 4
    }

    public class ArticleModel
    {
        public ArticleModel()
        {
            PublishedTime = DateTime.Now;
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

        [Display(Name = "Nội dung bài viết")]
        public string ArticleContent { get; set; }

        [Required]
        [Display(Name = "Bản nháp bài viết")]
        public string ArticleDraft { get; set; }

        [Required]
        [Display(Name = "Ngày đăng"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? PublishedTime { get; set; }

        [Required]
        [Display(Name = "Chuyên mục")]
        public string CategoryIds { get; set; }

        [Required]
        [Display(Name = "Phóng viên")]
        public string ReporterIds { get; set; }

        [Display(Name = "Series")]
        public int? ArticleSeriesId { get; set; }

        public string SeriesName { get; set; }

        [Display(Name = "Từ khóa")]
        public string Keywords { get; set; }

        public bool IsPublished { get; set; } //trạng thái trước
        public bool Published { get; set; } //trạng thái sau

        public int ArticleStatus { get; set; }

        public int Order { get; set; }

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
}