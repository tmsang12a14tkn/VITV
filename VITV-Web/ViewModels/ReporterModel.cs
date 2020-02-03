using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.ViewModels
{
    public class ReporterModel
    {
        public int Id { get; set; }
        [Index("TitleUniqueIndex", IsUnique = true)]
        [StringLength(450)]
        public string UniqueTitle { get; set; }
        [Required]
        [Display(Name = "Họ tên")]
        public string Name { get; set; }

        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Ảnh đại diện")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Thông tin")]
        public string Biography { get; set; }

        [Display(Name = "Giới thiệu")]
        public string Introduction { get; set; }

        [Display(Name = "Cho phép hiển thị trên trang thành viên VITV")]
        [DefaultValue(true)]
        public bool IsShow { get; set; }

        [Display(Name = "Chức danh")]
        public int? PositionId { get; set; }
        [Display(Name = "Vai trò")]
        public List<int> Roles { get; set; }
    }
}