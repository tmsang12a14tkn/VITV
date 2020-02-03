using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace VITV.HRM.Models
{
    public class VITVSecondContext : IdentityDbContext<ApplicationUser> 
    {
        public VITVSecondContext()
            : base("VITVSecondContext")
        {

        }
        public static VITVSecondContext Create()
        {
            return new VITVSecondContext();
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeWorkInfo> EmployeeWorkInfos { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<PositionLevel> PositionLevels { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public DbSet<PersonalJob> PersonalJobs { get; set; }
        public DbSet<PersonalDayoff> PersonalDayoffs { get; set; }
        public DbSet<Dayoff> Dayoffs { get; set; }
        public DbSet<GroupNotification> GroupNotifications { get; set; }
        public DbSet<TaskRequest> TaskRequests { get; set; }
        public DbSet<TaskResponse> TaskResponses { get; set; }
        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<JobAttachment> JobAttachments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<JobList> JobLists { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobResponse> JobResponses { get; set; }
        public DbSet<DeviceUser> DeviceUsers { get; set; }
    }
}