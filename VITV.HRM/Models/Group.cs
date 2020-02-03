using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.HRM.Models
{
    public class Group
    {
        public int Id { get;set;}
        [Required]
        public string Name { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<EmployeeWorkInfo> EmployeeWorkInfos { get; set; }
        public virtual ICollection<PositionLevel> PositionLevels { get; set; }
        public virtual ICollection<GroupNotification> GroupNotifications { get; set; }
    }
}
