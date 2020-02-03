using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class EmployeeSkill
    {
        [Key, Column(Order=0)]
        public string EmployeeId { get; set; }
        [Key, Column(Order = 1)]
        public int SkillId { get; set; }
        public int Rate { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Skill Skill { get; set; }
    }
}