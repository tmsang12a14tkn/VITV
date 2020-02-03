using System;
using System.Collections.Generic;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class ProgramSchedulesView
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<ProgramSchedule> ProgramSchedules { get; set; } 
    }
}