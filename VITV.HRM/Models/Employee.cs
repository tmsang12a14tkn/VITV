using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class Employee
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        [Required]
        [Display(Name="Họ tên")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Ảnh đại diện")]
        public string ProfilePicture { get; set; }

        [Index("TitleUniqueIndex", IsUnique = true)]
        [StringLength(450)]
        public string UniqueTitle { get; set; }
        public string About { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }

        public virtual ICollection<WorkDay> WorkDays { get; set; }

        public virtual ICollection<PersonalJob> PersonalJobs { get; set; }
        public virtual ICollection<PersonalDayoff> PersonalDayoffs { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual EmployeeWorkInfo WorkInfo { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }

        [InverseProperty("RequestedEmployee")] 
        public virtual ICollection<TaskRequest> SendingRequests { get; set; }

        [InverseProperty("ReceivedEmployees")] 
        public virtual ICollection<TaskRequest> ReceivedRequests { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Conference> Conferences { get; set; }
    }
}