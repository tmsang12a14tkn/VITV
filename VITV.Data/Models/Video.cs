using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class Video
    {
        //DisplayTime: thời gian hiển thị trên trang web
        //UploadedTime: thời gian người dùng upload lên lần đầu tiên
        //PublishedTime: thời gian video xuất hiện trên web (mặc định là ngay lập tức sau khi đăng tức là dùng uploadtime)
        //IsPublished: được đăng hoặc là lưu nháp (false = "red" | true = "green")
        //PublishImmediately: chua su dung
        public Video()
        {
            PublishedTime = DateTime.Now;
            Keywords = new List<Keyword>();
            Reporters = new HashSet<Employee>();
            IsHot = false;
            HourSkip = 0;
            MinuteSkip = 0;
            SecondSkip = 0;
            VideoWidth = 0;
            VideoHeight = 0;
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

        public int VideoPiority { get; set; } //0: Youtube Host - 1: VITV Host
        
        [Required]
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        [Required]
        public DateTime DisplayTime { get; set; } //thời gian phát sóng
        public DateTime? UploadedTime { get; set; } //thời gian đăng

        [Required]
        public DateTime PublishedTime { get; set; } //thời gian hẹn đăng

        [Required, ForeignKey("Category")]

        public int CategoryId { get; set; }

        public int? SpecialEventId { get; set; }

        public string UploaderId { get; set; }

        public int HourSkip { get; set; }

        public int MinuteSkip { get; set; }

        public int SecondSkip { get; set; }

        public int VideoWidth { get; set; }

        public int VideoHeight { get; set; }
        public TimeSpan Duration { get; set; }

        [Required]
        public bool IsHot { get; set; }

        public bool PublishImmediately { get; set; }

        public bool IsPublished { get; set; }

        [ForeignKey("UploaderId")]
        public virtual ApplicationUser UploadUser { get; set; }

        public virtual VideoCategory Category { get; set; }

        public virtual SpecialEvent SpecialEvent { get; set; }
        
        public virtual ICollection<Keyword> Keywords { get; set; }

        public virtual ICollection<ProgramScheduleDetail> ScheduleDetails { get; set; }

        public virtual ICollection<Employee> Reporters { get;set; }

        public virtual ICollection<VideoTranscript> VideoTranscripts { get; set; }

        public virtual ICollection<VideoRate> VideoRates { get; set; }
        public virtual ICollection<Subtitle> Subtitles { get; set; }

    }
}