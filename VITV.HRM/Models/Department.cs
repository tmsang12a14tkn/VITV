using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.HRM.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int? BranchId { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

    }
}
