using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class JobList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TaskRequestId { get; set; }
        public virtual TaskRequest TaskRequest { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

    }
}