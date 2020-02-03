namespace VITV.HRM.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VITV.HRM.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<VITV.HRM.Models.VITVSecondContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VITV.HRM.Models.VITVSecondContext context)
        {
            context.NotificationTypes.AddOrUpdate(
            nt => nt.Id,
            new NotificationType { Id = NotificationTypes.AddedToProject, Format = "Bạn được thêm vào dự án {1}"},
            new NotificationType { Id = NotificationTypes.AddedToTask, Format = "Bạn được thêm vào nhiệm vụ - {1}"},
            new NotificationType { Id = NotificationTypes.AddedToJob, Format = "Bạn nhận được công việc - {1}"},
            new NotificationType { Id = NotificationTypes.ReceivedAResponse, Format = "{0} bình luận trong công việc - {1}"}
            );

            //var user1 = new ApplicationUser { UserName = "nguyenchihieu", Email = "nguyenchihieu@vitcorp.vn", PasswordHash = "AByLc65vUYXNzRSqb+WXyLMwed12PU6P79tmT9F+yvM0QPfWq5hWRVMwe5BmqKq7AQ==", SecurityStamp = Guid.NewGuid().ToString(), PhoneNumber = "0000000" };
            //context.Users.AddOrUpdate(
            //nt => nt.UserName,
            //user1
            ////new ApplicationUser { Id = NotificationTypes.AddedToTask, Format = "Bạn được thêm vào nhiệm vụ - {1}" },
            ////new ApplicationUser { Id = NotificationTypes.AddedToJob, Format = "Bạn nhận được công việc - {1}" },
            ////new ApplicationUser { Id = NotificationTypes.ReceivedAResponse, Format = "{0} bình luận trong công việc - {1}" }
            //);

            var userStore = new UserStore<ApplicationUser>(context);
            var roleStore = new RoleStore<IdentityRole>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var roleManager = new RoleManager<IdentityRole>(roleStore);


            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole("Admin");
                roleManager.Create(role);
            }

            if (userManager.FindByName("nguyenchihieu") == null)
            {
                var user = new ApplicationUser { UserName = "nguyenchihieu", PhoneNumber = "0797697898" };
                userManager.Create(user, "vitv123");
                userManager.AddToRole(user.Id, "Admin");

                var employee = new Employee()
                {
                    Id = user.Id,
                    Name = "Nguyễn Chí Hiếu",
                    UniqueTitle = "nguyen-chi-hieu",
                    ProfilePicture = "/",
                    BirthDay = new DateTime(1986, 12, 12),
                    WorkInfo = new EmployeeWorkInfo()
                };
                context.Employees.Add(employee);
                context.SaveChanges();

            }
            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
