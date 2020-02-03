using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class InterestRate
    {
        public int Id { get; set; }

        [Required, ForeignKey("Bank")]
        public int BankId { get; set; }

        [Required, ForeignKey("Term")]
        public int TermId { get; set; }

        [Required, Display(Name = "Tỷ lệ")]
        public double PercentValue { get; set; }

        [Required, Display(Name = "Loại tiền gửi")]
        public int TypeValue { get; set; }

        [Required, Display(Name = "Đối tượng")]
        public int TargetValue { get; set; }

        public virtual Bank Bank { get; set; }

        public virtual Term Term { get; set; }
    }
}