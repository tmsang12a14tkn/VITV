using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class PositionLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}