using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class RecruitExtraInfo
    {
        public int Id { get; set; }
        [Display(Name = "Tiêu đề")]
        public string Name { get; set; }
        [Display(Name = "Quy trình tuyển dụng")]
        public string RecruitRule { get; set; }
        [Display(Name = "Mẫu tuyển dụng")]
        public string RecruitForm { get; set; }
    }
}