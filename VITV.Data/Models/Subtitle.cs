using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.Models
{
    public class Subtitle
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public string Source { get; set; }
        public int VideoId {get;set;}
        public virtual Video Video { get; set; }
    }
}
