using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPrivate { get; set; }
        public virtual ICollection<TaskRequest> TaskRequests { get; set; }
        public virtual ICollection<ProjectAttachment> Attachments { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        [NotMapped]
        public string EmployeeIds { get; set; }
        
    }
}