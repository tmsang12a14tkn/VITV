using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class PositionGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
    }
}