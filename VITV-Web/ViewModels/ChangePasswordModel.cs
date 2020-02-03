using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV_Web.ViewModels
{
    public class ChangePasswordModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "Mật khẩu cũ không được rỗng")]
        [StringLength(100, ErrorMessage = "{0} phải dài ít nhất {2} kí tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được rỗng")]
        [StringLength(100, ErrorMessage = "{0} phải dài ít nhất {2} kí tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Nhập lại mật khẩu chưa khớp.")]
        public string ConfirmNewPassword { get; set; }
    }
}