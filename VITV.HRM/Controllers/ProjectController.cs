using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using Microsoft.AspNet.Identity;

namespace VITV.HRM.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MyProjects()
        {
            var context = new VITVSecondContext();
            var employee = context.Employees.Find(User.Identity.GetUserId());

            return View(employee.Projects.Where(p=>!p.IsDeleted).ToList());
        }
        public ActionResult Menu()
        {
            var context = new VITVSecondContext();
            var userId = User.Identity.GetUserId();
            var employee = context.Employees.Find(userId);
            var joinedProjects = employee.Jobs.Select(j => j.JobList.TaskRequest.Project).Where(p => !p.IsDeleted).ToList();
            var allProjects = joinedProjects.Union(employee.Projects.Where(p => !p.IsDeleted).ToList());

            return PartialView(allProjects);
        }
        [Authorize(Roles="Admin")]
        public ActionResult All()
        {
            var context = new VITVSecondContext();

            return View(context.Projects.Where(p => !p.IsDeleted).ToList());
        }
        public ActionResult Trash()
        {

            var context = new VITVSecondContext();

            return View(context.Projects.Where(p => p.IsDeleted).ToList());
        }
        public ActionResult Details(int id)
        {
            var context = new VITVSecondContext();
            var userId = User.Identity.GetUserId();
            var project = context.Projects.Find(id);
            if(project == null)
                return HttpNotFound("Project is not exist");
            
            ViewBag.Me = context.Employees.Find(userId);
            ViewBag.CanEdit = User.IsInRole("Admin") || project.Employees.Any(e => e.Id == userId);

            if (!project.IsPrivate || ViewBag.CanEdit == true)
            {
                return View(project);
            }
            else
                return HttpNotFound("You do not have permission to access this project");
        }

        public ActionResult Create()
        {
            var context = new VITVSecondContext();
            ViewBag.Employees = context.Employees.ToList();
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(Project project, List<HttpPostedFileBase> attachments)
        {
            var context = new VITVSecondContext();
            context.Projects.Add(project);
            
            if (!string.IsNullOrWhiteSpace(project.EmployeeIds))
            {
                project.Employees = new List<Employee>();
                var employeeIds = project.EmployeeIds.Split(',');
                foreach (string eId in employeeIds)
                {
                    if (!string.IsNullOrWhiteSpace(eId))
                    {
                        var eIdNormalize = eId.Trim();
                        var employee = context.Employees.Find(eIdNormalize);
                        if (employee != null)
                            project.Employees.Add(employee);
                    }
                }
            }
            if (attachments != null)
            {
                project.Attachments = new List<ProjectAttachment>();
                foreach (var attachment in attachments)
                {
                    if (attachment != null)
                    {
                        attachment.SaveAs(Server.MapPath("~/Upload/Attachment/" + attachment.FileName));
                        var link = ("/Upload/Attachment/" + attachment.FileName);
                        project.Attachments.Add(new ProjectAttachment
                        {
                            AttachmentLink = link
                        });
                    }
                }
            }

            context.SaveChanges();

            var notification = new Notification()
            {
                FromId = User.Identity.GetUserId(),
                NotificationTypeId = NotificationTypes.AddedToProject,
                ProjectId = project.Id,
                Url = "/Project/Details/" + project.Id,
                CreatedDate = DateTime.Now
            };
            context.Notifications.Add(notification);
            foreach (var employee in project.Employees)
            {
                var userNotification = new UserNotification()
                {
                    //IsRead = false,
                    EmployeeId = employee.Id,
                    Notification = notification
                };
                context.UserNotifications.Add(userNotification);
            }
            context.SaveChanges();
            foreach (var employee in project.Employees)
            {
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).reloadNotification();
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).popupNotification(employee.Id, notification.Id, project.Content);
            }
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var context = new VITVSecondContext();
            var project = context.Projects.Find(id);
            if (project == null)
                return new HttpNotFoundResult();
            ViewBag.Employees = context.Employees.ToList();
            return PartialView(project);
        }
        [HttpPost]
        public ActionResult Edit(Project project, List<HttpPostedFileBase> attachments, ICollection<string> uploadedAttachments)
        {
            var context = new VITVSecondContext();
            var entry = context.Projects.Find(project.Id);
            context.Entry(entry).State = EntityState.Detached;

            context.Projects.Attach(project);

            if (!string.IsNullOrWhiteSpace(project.EmployeeIds))
            {
                context.Entry<Project>(project).Collection(p => p.Employees).Load();
                project.Employees.Clear();

                var employeeIds = project.EmployeeIds.Split(',');
                foreach (string eId in employeeIds)
                {
                    if (!string.IsNullOrWhiteSpace(eId))
                    {
                        var eIdNormalize = eId.Trim();
                        var employee = context.Employees.Find(eIdNormalize);
                        if (employee != null)
                            project.Employees.Add(employee);
                    }
                }
            }

            context.Entry<Project>(project).Collection(r => r.Attachments).Load();
            project.Attachments.Clear();
            if (uploadedAttachments != null)
            {
                foreach (var uAttachment in uploadedAttachments)
                {
                    project.Attachments.Add(new ProjectAttachment
                    {
                        AttachmentLink = uAttachment
                    });
                }
            }
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    if (attachment != null)
                    {
                        attachment.SaveAs(Server.MapPath("~/Upload/Attachment/" + attachment.FileName));
                        var link = ("/Upload/Attachment/" + attachment.FileName);
                        project.Attachments.Add(new ProjectAttachment
                        {
                            AttachmentLink = link
                        });
                    }
                }
            }
            context.Entry(project).State = EntityState.Modified;
            context.SaveChanges();
            return Json(new { success = true });
            //return RedirectToAction("All");
        }
        public ActionResult Delete(int? id)
        {
            var context = new VITVSecondContext();
            var project = context.Projects.Find(id);
           
            return PartialView(project);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, bool permanent)
        {
            var context = new VITVSecondContext();
            var project = context.Projects.Find(id);
            if(permanent)
            {
                context.Projects.Remove(project);
            }
            else
            {
                project.IsDeleted = true;
            }
            context.SaveChanges();
            return Json(new { success = true });
        }


    }
}