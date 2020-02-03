using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class ScheduleDetailFilterView
    {
        [Display(Name = "Ngày"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateTime { get; set; }
        public List<ProgramScheduleDetail> ScheduleDetails { get; set; }

    }
}