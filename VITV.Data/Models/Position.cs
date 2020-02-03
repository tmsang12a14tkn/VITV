using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int PositionLevelId { get; set; }

        public virtual PositionLevel PositionLevel { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

    }
}