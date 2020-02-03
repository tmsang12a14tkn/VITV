using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class VideoCategory
    {
        public int Id { get; set; }
        [Index("TitleUniqueIndex", IsUnique = true)]
        [StringLength(450)]
        public string UniqueTitle { get; set; }

        [Required, Display(Name="Tên chuyên mục")]
        public string Name { get; set; }

        [Required, Display(Name= "Mô tả về chuyên mục")]
        public string Description { get; set; }

        [Required, Display(Name="Giới thiệu về chuyên mục")]
        public string Introduction { get; set; }

        [Display(Name = "Giới thiệu về chuyên mục (thêm)")]
        public string IntroductionBonus { get; set; }

        [Required, Display(Name="Hình hiển thị")]
        public string ProfilePhoto { get; set; }

        public string MobileProfilePhoto { get; set; }
        
        [Display(Name = "Khung giờ")]
        public string TimeFrame { get; set; }
        
        [Required, Display(Name="Xuất hiện trên menu")]
        public bool ShowInMenu { get; set; }

        public int Order { get; set; }

        [ForeignKey("Group")]
        public int? GroupId { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        public virtual VideoCatGroup Group { get; set; }

        public VideoCategory()
        {
            ShowInMenu = true;
        }
    }
}