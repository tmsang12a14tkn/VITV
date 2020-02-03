using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VITV_Web.ViewModels
{
    public class VideoModel
    {

        public int Id { get; set; }
        [Required]
        [Display(Name="Tiêu đề")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Ảnh đại diện")]
        public string Thumbnail { get; set; }
        public string MobileThumbnail { get; set; }
        [Required]
        [Display(Name = "Video")]
        public string Url { get; set; }
        [Display(Name= "Youtube")]
        public string YoutubeUrl { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }
        [Required]
        [Display(Name = "Chuyên mục")]
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày đăng"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime PublishedTime { get; set; }

        [Display(Name = "Từ khóa")]
        public String Keywords { get; set; }

        [Display(Name = "Thực hiện")]
        public string ReporterIds { get; set; }

        public bool IsHot { get; set; }

        public int ScheduleDetailId { get; set; }
        public int? SpecialEventId { get; set; }

        public VideoModel()
        {
            PublishedTime = DateTime.Now;
        }
    }
}