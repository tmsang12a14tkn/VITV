using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV.Data.ViewModels
{
    public class BankModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên ngân hàng")]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Hình")]
        public string Thumbnail { get; set; }

        [Display(Name = "Ngày cấp phép")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string IssueDate { get; set; }

        [Display(Name = "Ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdateTime { get; set; }

        [Display(Name = "Vốn điều lệ (Tỷ đồng)")]
        public double CharterCapital { get; set; }
    }
}