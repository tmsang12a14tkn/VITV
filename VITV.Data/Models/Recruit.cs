using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class Recruit
    {
        public int Id { get; set; }

        [Required, Display(Name = "Hiển thị đăng tuyển dụng")]
        public bool IsRecruiting { get; set; }

        [Required]
        [Display(Name = "Chức danh/Vị trí")]
        public string JobPosition { get; set; }

        [Display(Name = "Số lượng tuyển")]
        public string Amount { get; set; }
        
        [Display(Name = "Lĩnh vực ngành nghề")]
        public int CategoryId { get; set; }
                
        [Display(Name = "Địa điểm làm việc")]
        public int CityId { get; set; }
                
        [Display(Name = "Tính chất công việc")]
        public string JobFeature { get; set; }

        [Display(Name = "Mô tả công việc")]
        public string Description { get; set; }

        [Display(Name = "Yêu cầu công việc")]
        public string Requirement { get; set; }

        [Display(Name = "Kinh nghiệm")]
        public string Experience { get; set; }

        [Display(Name = "Trình độ")]
        public string Degree { get; set; }

        [Display(Name = "Giới tính")]
        public string Gender { get; set; }

        [Display(Name = "Hình thức làm việc")]
        public string JobType { get; set; }

        [Display(Name = "Mức lương")]
        public string Salary { get; set; }

        [Display(Name = "Thời gian thử việc")]
        public string Probation { get; set; }

        [Display(Name = "Các chế độ đãi ngộ, phúc lợi")]
        public string Remuneration { get; set; }

        [Display(Name = "Yêu cầu hồ sơ")]
        public string Resume { get; set; }

        [Display(Name = "Hạn nộp hồ sơ")]
        public string DateExpire { get; set; }

        [Display(Name = "Hình thức nộp hồ sơ")]
        public string Submission { get; set; }

        [Display(Name = "Thông tin liên hệ")]
        public string ContactInfo { get; set; }

        [Display(Name = "Hình")]
        public string Thumbnail { get; set; }

        public virtual RecruitCategory Category { get; set; }
        public virtual City City { get; set; }
    }
}