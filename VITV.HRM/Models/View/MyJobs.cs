using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models.View
{
    public class MyJobs
    {
        public List<Job> OnProgress { get; set; }
        public List<Job> Done { get; set; }
    }
}