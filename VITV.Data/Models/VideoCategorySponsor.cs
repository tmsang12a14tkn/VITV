using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class VideoCategorySponsor
    {
        public int Id { get; set; }
        
        [Required, Display(Name = "Tên đối tác tài trợ")]
        public string Name { get; set; }
        
        [Display(Name = "Giới thiệu")]
        public string Description { get; set; }
        
        [Required, Display(Name = "Hình hiển thị")]
        public string Thumbnail { get; set; }

        [Required, Display(Name = "Website")]
        public string Url { get; set; }

        public int Order { get; set; }
                
        public virtual ICollection<VideoCategory> Categories { get; set; }
    }
}
