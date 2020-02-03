using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class Term
    {
        public Term()
        {
            InterestRates = new List<InterestRate>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Loại kỳ hạn")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Tháng")]
        public int MonthValue { get; set; }

        public virtual ICollection<InterestRate> InterestRates { get; set; }
    }
}