using System;
using System.ComponentModel.DataAnnotations;

namespace VITV_Web.Models
{
    public class ProgramSchedule
    {
        public int Id { get; set; }

        [Display(Name = "Chuyên mục")]
        public int VideoCategoryId { get; set; }
        [Display(Name = "Ngày trong tuần")]
        public DayOfWeek DayOfWeek
        {
            get
            ;
            set;
        }

        [Display(Name = "Thời gian")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan Time { get; set; }

        [Display(Name = "Phát mới")]
        public bool IsNew { get; set; }

        public virtual VideoCategory VideoCategory { get; set; }

    }
}