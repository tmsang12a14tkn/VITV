using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class EmployeeWorkInfo
    {
        [Key, ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public DateTime? StartDate { get; set; } //Ngày vào làm

        public DateTime? EndDate { get; set; } //Ngày kết thúc làm
        [ForeignKey("Group")]
        public int? GroupId { get; set; } //Tổ nhóm

        public int? PositionLevelId { get; set; } //Chức vụ

        public virtual Group Group { get; set; }
        public virtual PositionLevel PositionLevel { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ICollection<EmployeeSkill> Skills { get; set; }
    }
}