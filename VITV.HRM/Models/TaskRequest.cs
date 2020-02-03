using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class TaskRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //public int? EquipmentId { get; set; }
        [ForeignKey("RequestedEmployee")]
        public string RequestedEmployeeId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? RequestFrom { get; set; }
        public DateTime? RequestTo { get; set; }
        public int Piority { get; set; } //0: not neccesary, 1: normal, 2: neccesary
        public int Status { get; set; } //0: opened, 1: closed
        public int? ProjectId { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Employee RequestedEmployee { get; set; }
        public virtual ICollection<Employee> ReceivedEmployees { get; set; }
        //public virtual ICollection<JobAttachment> Attachments { get; set; }
        public virtual ICollection<TaskResponse> Responses { get; set; }
        public virtual Project Project { get; set; }
        //public virtual Equipment Equipment { get; set; }
        public virtual ICollection<JobList> JobLists { get; set; }

        [NotMapped]
        public string ReceivedEmployeeIds { get; set; }


    }
}