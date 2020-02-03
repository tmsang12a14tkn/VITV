using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class RecruitCategory
    {
        public int Id { get; set; }
        [Required, Display(Name="Tên ngành nghề")]
        public string Name { get; set; }
        [Required, Display(Name = "Mô tả về ngành nghề")]
        public string Description { get; set; }
        [Required, Display(Name = "Hiển thị đăng tuyển dụng")]
        public bool IsRecruiting { get; set; }

        public virtual ICollection<Recruit> Recruits { get; set; }

        public RecruitCategory()
        {
            IsRecruiting = true;
        }
    }
}