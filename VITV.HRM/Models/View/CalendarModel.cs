using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models.View
{
    public class CalendarModel
    {
        public Employee Employee { get; set; }
        public List<Dayoff> Dayoffs { get; set; }
        public List<PersonalDayoff> PersonalDayOffs { get; set; }
        public List<Conference> Conferences { get; set; }
        public List<PersonalJob> PersonalJobs { get; set; }
        public List<Job> Jobs { get; set; }
    }
}