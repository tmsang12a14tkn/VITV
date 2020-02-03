using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên")]
        public string Name { get; set; }
        
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}