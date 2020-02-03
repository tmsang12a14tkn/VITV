using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VITV.Data.Models
{
    public class PageGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        [Index("TitleUniqueIndex", IsUnique = true)]
        [StringLength(450)]
        public string UniqueTitle { get; set; }

        public virtual ICollection<VideoCategory> VideoCats { get; set; }
    }
}
