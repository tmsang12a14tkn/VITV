using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class Bank
    {
        public Bank()
        {
            InterestRates = new List<InterestRate>();
        }

        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Tên ngân hàng")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Hình")]
        public string Thumbnail { get; set; }

        [Display(Name = "Ngày cấp phép")]
        public string IssueDate { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdateTime { get; set; }

        [Display(Name = "Vốn điều lệ (Tỷ đồng)")]
        public double? CharterCapital { get; set; }

        public virtual ICollection<InterestRate> InterestRates { get; set; }
    }
}