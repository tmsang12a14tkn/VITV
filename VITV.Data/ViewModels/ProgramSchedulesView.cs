using System;
using System.Collections.Generic;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class ProgramSchedulesView
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<ProgramSchedule> ProgramSchedules { get; set; } 
    }
}