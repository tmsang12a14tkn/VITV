using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV_Web.Models;

namespace VITV_Web.ViewModels
{
    public class DowScheduleView
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<ProgramSchedule> ProgramSchedules { get; set; } 
    }
}