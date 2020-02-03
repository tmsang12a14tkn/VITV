using System;
using System.ComponentModel.DataAnnotations;

namespace VITV_Web.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Chưa nhập họ tên"), Display(Name = "Họ tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Chưa nhập email"), Display(Name = "Email"), DataType(DataType.EmailAddress, ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Chưa nhập tiêu đề"), Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Chưa nhập nội dung"), Display(Name = "Nội dung")]
        public string ContactContent { get; set; }
    }
}