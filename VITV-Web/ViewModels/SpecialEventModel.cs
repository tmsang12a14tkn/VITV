using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV_Web.ViewModels
{
    public class SpecialEventModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tên")]
        public string Name { get; set; }
        [Display(Name = "Ảnh nền thanh")]
        public string BarBkgr { get; set; }
        [Required]
        [Display(Name = "Bắt đầu")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Kết thúc")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}