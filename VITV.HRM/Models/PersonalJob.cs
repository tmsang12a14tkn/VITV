using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class PersonalJob
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string Title { get; set; }
        public DateTime Start {get;set;}
        public DateTime End { get; set; }
        public bool AllDay { get; set; }

        public virtual Employee Employee { get; set; }
    }
}