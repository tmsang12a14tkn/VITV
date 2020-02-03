using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV.HRM.ViewModels
{
    public class EmployeeEditModel
    {

        public string Id { get; set; }
        [Required]
        [Display(Name = "Thư điện tử")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Hình đại diện")]
        public string ProfilePicture { get; set; }

        [Required]
        [Display(Name = "Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDay { get; set; }

        [Display(Name = "Ngày vào làm")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Vai trò")]
        public string Role { get; set; }
    }
}