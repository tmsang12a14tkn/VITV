using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models.View
{
    public class CalendarMonthModel
    {
        public Employee Employee { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}