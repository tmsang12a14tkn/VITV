using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class CategoryIntro
    {
        public int Id { get; set; }
        [Required, Display(Name = "Tiêu đề")]
        public string Title { get; set; }
        [Required, Display(Name = "Hình hiển thị")]
        public string Thumbnail { get; set; }
        [Required, Display(Name = "Đường dẫn video")]
        public string Url { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Tên chuyên mục")]
        public int VideoCategoryId { get; set; }
        public bool Selected { get; set; }
        public virtual VideoCategory VideoCategory { get; set; }
    }
}
