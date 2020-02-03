using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.Models
{
    public class VideoCatType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<VideoCategory> VideoCategories { get; set; }
    }
}
