﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV.Data.ViewModels
{
    public class EditUserModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Vai trò")]
        public List<string> Roles { get; set; }

        public List<string> OldRoles { get; set; }

        public bool Locked { get; set; }
        
    }
}