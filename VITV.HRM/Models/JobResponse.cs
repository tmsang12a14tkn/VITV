using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class JobResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int JobId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string EmployeeId { get; set; }


        public virtual Job Job { get; set; }
        public virtual ICollection<ResponseAttachment> Attachments { get; set; }
        public virtual Employee Employee { get; set; }


    }
}