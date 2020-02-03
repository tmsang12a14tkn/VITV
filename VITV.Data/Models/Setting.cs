using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.Models
{
    public class Setting
    {
        [Key]
        [MaxLength(80)]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
