using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class WorkDay
    {
        [Key, Column(Order = 0)]
        public string EmployeeId { get; set; }
        [Key, Column(Order = 1)]
        public DateTime Date { get; set; }
        public int WorkHour { get; set; }
        public string Note { get; set; }

        public Employee Employee { get; set; }
    }
}