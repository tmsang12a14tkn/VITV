using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VITV.Data.ViewModels
{
    public class VideoModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Tiêu đề")]
        public string Title { get; set; }

        [Index("TitleUniqueIndex", IsUnique = true)]
        [StringLength(450)]
        public string UniqueTitle { get; set; }

        [Required]
        [Display(Name = "Ảnh đại diện")]
        public string Thumbnail { get; set; }
        public string MobileThumbnail { get; set; }
        [Required]
        [Display(Name = "Video")]
        public string Url { get; set; }
        [Display(Name= "Youtube")]
        public string YoutubeUrl { get; set; }
        public int VideoPiority { get; set; } //0: Youtube Host - 1: VITV Host

        [Display(Name = "Nội dung")]
        public string Content { get; set; }
        [Required]
        [Display(Name = "Chuyên mục")]
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày hẹn đăng"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime PublishedTime { get; set; } //Thời gian người dùng hẹn đăng
        public DateTime UploadedTime { get; set; } //Thời gian người dùng tải lên

        public DateTime DisplayTime { get; set; } //THời gian phát sóng truyền hình

        [Display(Name = "Từ khóa")]
        public String Keywords { get; set; }

        [Display(Name = "Thực hiện")]
        public string ReporterIds { get; set; }

        public bool IsHot { get; set; }

        [Required]
        public bool PublishImmediately { get; set; }

        public bool IsPublished { get; set; }

        public int ScheduleDetailId { get; set; }

        public int? SpecialEventId { get; set; }

        public string UploaderId { get; set; }

        public int HourSkip { get; set; }

        public int MinuteSkip { get; set; }

        public int SecondSkip { get; set; }

        public int VideoWidth { get; set; }

        public int VideoHeight { get; set; }
        public TimeSpan Duration { get; set; }

        public string Subtitle { get; set; }

        public VideoModel()
        {
            PublishedTime = DateTime.Now;
            PublishImmediately = true;
            IsPublished = false;
            HourSkip = 0;
            MinuteSkip = 0;
            SecondSkip = 0;
            VideoWidth = 0;
            VideoHeight = 0;
        }
    }
}