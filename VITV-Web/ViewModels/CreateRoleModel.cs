using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV_Web.ViewModels
{
    public class CreateRoleModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Tên")]
        public string Name { get; set; } 
    }
}