using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class ReporterInfo
    {
        [Key]
        [ForeignKey("Reporter")]
        public int Id { get; set; }
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Thông tin")]
        public string Biography { get; set; }

        [Display(Name = "Giới thiệu")]
        public string Introduction { get; set; }

        public virtual Reporter Reporter { get; set; }
    }
}