using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV.HRM.ViewModels
{
    public class HolidayModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        
        [Required]
        [Display(Name = "Bắt đầu")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Begin { get; set; }
        [Required]
        [Display(Name = "Kết thúc")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }
    }
}