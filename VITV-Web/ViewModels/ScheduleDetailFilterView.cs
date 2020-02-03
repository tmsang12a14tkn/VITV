using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class ScheduleDetailFilterView
    {
        [Display(Name = "Ngày"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateTime { get; set; }
        public List<ProgramScheduleDetail> ScheduleDetails { get; set; }

    }
}