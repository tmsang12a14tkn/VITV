using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TaskRequestId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string EmployeeId { get; set; }
        public bool IsDeleted { get; set; }


        public virtual TaskRequest TaskRequest { get; set; }
        public virtual Employee Employee { get; set; }


    }
}