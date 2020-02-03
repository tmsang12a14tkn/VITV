using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.Models
{
    public class EmployeeWorkInfo
    {
        [Key]
        [ForeignKey("Employee")]
        public int Id { get; set; }
        [ForeignKey("Agency")]
        public int AgencyId { get; set; }
        public int PositionLevelId { get; set; }
        public string PositionTitle { get; set; }

        public virtual Agency Agency { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual PositionLevel PositionLevel { get; set; }

    }
}
