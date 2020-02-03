using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class TVCCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public virtual ICollection<TVCProduct> Products { get; set; }
    }

    public class TVCProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual TVCCompany Company { get; set; }
    }
    public class TVCCompanyGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } 
    }
    public class TVCProductGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } 
    }
    public class VideoTVC
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tên quảng cáo")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Hình ảnh")]
        public string Thumbnail { get; set; }
        [Required]
        [Display(Name = "Chọn quảng cáo")]
        public string Url { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [ForeignKey("CompanyGroup")]
        [Display(Name = "Nhóm công ty")]
        public int? CompanyGroupId { get; set; }
        [ForeignKey("ProductGroup")]
        [Display(Name = "Nhóm sản phẩm")]
        public int? ProductGroupId { get; set; }
        [ForeignKey("Product")]
        [Display(Name = "Sản phẩm")]
        public int? ProductId { get; set; }
        [Display(Name = "Phiên bản")]
        public int Version { get; set; }
        public int DisplayCount { get; set; }
        public virtual ICollection<VideoCategory> VideoCategoties { get; set; }
        public virtual TVCProduct Product { get; set; }
        public virtual TVCCompanyGroup CompanyGroup { get; set; }
        public virtual TVCProductGroup ProductGroup { get; set; }
       
       
    }
}
