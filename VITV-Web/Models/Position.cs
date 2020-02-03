using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int GroupId { get; set; }

        public virtual PositionGroup Group { get; set; }
        public virtual ICollection<Reporter> Reporters { get; set; }
    }
}