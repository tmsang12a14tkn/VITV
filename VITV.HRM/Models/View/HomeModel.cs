using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models.View
{
    public class HomeModel
    {
        public List<Job> TodayJobs { get; set; }
        public List<Job> TomorrowJobs { get; set; }
    }
}