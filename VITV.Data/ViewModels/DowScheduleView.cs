using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class DowScheduleView
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<ProgramSchedule> ProgramSchedules { get; set; } 
    }
}