using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class ArticleSeries
    {
        public int Id { get; set; }

        [Index("TitleUniqueIndex", IsUnique = true)]
        [StringLength(450)]
        public string UniqueTitle { get; set; }

        [Required, Display(Name="Tên series")]
        public string Name { get; set; }

        [Display(Name = "Mô tả về series")]
        public string Description { get; set; }

        [Display(Name = "Hình hiển thị")]
        public string ProfilePhoto { get; set; }

        public int Order { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

    }
}
