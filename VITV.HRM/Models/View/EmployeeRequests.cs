using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models.View
{
    public class EmployeeRequests
    {
        public List<TaskRequest> RequestsByMe { get; set; }
        public List<TaskRequest> RequestsToMe { get; set; }
    }
}