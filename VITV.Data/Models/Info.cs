using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class Info
    {
        [Key, Required, MaxLength(160)]
        [Display(Name="Id (không trùng)")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required]
        [Display(Name="Nội dung")]
        public string Description { get; set; }

    }
}