using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class ProgramCategory
    {
        public int Id { get; set; }
        public string Name { get; set;}

        public virtual ICollection<Program> Programs { get; set; }
      
    }
}