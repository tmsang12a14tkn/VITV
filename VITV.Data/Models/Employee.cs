using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class Employee
    {
        public Employee()
        {
            Roles = new List<Role>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name="Họ tên")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Ảnh đại diện")]
        public string ProfilePicture { get; set; }

        [Index("TitleUniqueIndex", IsUnique = true)]
        [StringLength(450)]
        public string UniqueTitle { get; set; }

        [Display(Name = "Chức danh")]
        public int? PositionId { get; set; }

        [Required]
        [Display(Name = "Ẩn/Hiện")]
        [DefaultValue(true)]
        public bool IsShow { get; set; }
        [ForeignKey("Group")]
        public int? GroupId { get; set; }

        [NotMapped]
        public List<VideoCategory> SpecialCategories
        {
            get
            {
                return Videos.GroupBy(v => v.Category).OrderBy(vg => vg.Count()).Select(vg => vg.Key).Take(3).ToList();
            }
        }

        public virtual Position Position { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<VideoTranscript> VideoTranscripts { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual EmployeePersonalInfo EmployeePersonalInfo { get; set; }
        public virtual Group Group { get; set; }
        
    }
}