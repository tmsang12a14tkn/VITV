using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int JobListId { get; set; }
        public bool Done { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual JobList JobList { get;set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<JobAttachment> Attachments { get; set; }
        public virtual ICollection<JobResponse> Responses { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
       

       
        public string EmployeesString(string propertyName)
        {
            return String.Join(", ", Employees.Select(e => e.GetType().GetProperty(propertyName).GetValue(e)));
        }
        public string EquipmentsString(string propertyName)
        {
            return String.Join(", ", Equipments.Select(e => e.GetType().GetProperty(propertyName).GetValue(e)));
        }
    }
}