using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.HRM.Models
{
    public class Equipment
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Ảnh thiết bị")]
        public string EquipPicture { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public string EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }

    }
}