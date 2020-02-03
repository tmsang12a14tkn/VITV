using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VITV.Data.Models
{
    public class ProgramScheduleDetail
    {
        public int Id { get; set; }
        
        [Display(Name = "Chương trình")]
        public int VideoCategoryId { get; set; }

        public int? VideoId { get; set; }

        [Display(Name = "Thời gian"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name="Chiếu mới")]
        public bool IsNew { get; set; }

        public virtual VideoCategory VideoCategory { get; set; }

        public virtual Video Video { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {   
                if(!VideoId.HasValue)
                    return string.Format("<span>{0}</span>", VideoCategory.Name);
                else
                    return string.Format("<span>{0}: </span>{1}", VideoCategory.Name, Video.Title);
            }
        }

    }
}