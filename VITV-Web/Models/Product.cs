using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, Display(Name = "Tiêu đề")]
        public string Title { get; set; }
        [Required, Display(Name = "Nội dung")]
        public string Content { get; set; }
    }
}