using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class PositionLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public virtual ICollection<EmployeeWorkInfo> EmployeeWorkerInfos { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}