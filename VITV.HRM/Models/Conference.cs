using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class Conference
    {
        public Conference()
        {
            Employees = new List<Employee>();
        }
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}