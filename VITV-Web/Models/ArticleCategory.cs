using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class ArticleCategory
    {
        public int Id { get; set; }

        [Index("TitleUniqueIndex", IsUnique = true)]
        [StringLength(450)]
        public string UniqueTitle { get; set; }
        [Required, Display(Name="Tên chuyên mục")]
        public string Name { get; set; }
        [Required, Display(Name = "Mô tả về chuyên mục")]
        public string Description { get; set; }
        [Required, Display(Name = "Hình hiển thị")]
        public string ProfilePhoto { get; set; }
        [Required, Display(Name = "Giờ bắt đầu")]
       
        public bool ShowInMenu { get; set; }

        public int Order { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public ArticleCategory()
        {
            ShowInMenu = true;
        }
    }
}