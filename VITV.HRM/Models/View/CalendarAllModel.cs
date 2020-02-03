using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models.View
{
    public class CalendarAllModel
    {
        public int GroupId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<EmployeeAttendance> EmployeeAttendances { get; set; }
    }

    public class EmployeeAttendance
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<PersonalDayoff> Dayoffs { get; set; }
    }
}