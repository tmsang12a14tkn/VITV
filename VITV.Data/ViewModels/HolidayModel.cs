using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV.Data.ViewModels
{
    public class HolidayModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tên")]
        public string Name { get; set; }
        [Display(Name = "Ảnh nền trái")]
        [Required]
        public string LeftFixedBkgr { get; set; }
        [Required]
        [Display(Name = "Ảnh nền phải")]
        public string RightFixedBkgr { get; set; }
        [Required]
        [Display(Name = "Bắt đầu")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Kết thúc")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "Lặp lại mỗi năm")]
        public bool RepeatYear { get; set; }
    }
}