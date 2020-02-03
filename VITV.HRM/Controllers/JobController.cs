using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.IO;
using VITV.HRM.Models.View;

namespace VITV.HRM.Controllers
{
    public class JobController : Controller
    {
        public ActionResult Details(int id)
        {
            var context = new VITVSecondContext();
            var job = context.Jobs.Find(id);
            if (job != null)
            {
                var userId = User.Identity.GetUserId();
                ViewBag.Me = context.Employees.Find(userId);
                var project = context.Projects.Find(job.JobList.TaskRequest.ProjectId);
                ViewBag.CanEdit = User.IsInRole("Admin") || project.Employees.Any(e=>e.Id==userId);
                if (Request.IsAjaxRequest())
                    return PartialView(job);
                else
                    return View(job);
            }
            else
            {
                return new HttpNotFoundResult();
            }
        }

        [HttpPost]
        public ActionResult Add(Job job, string[] EmployeeIds, string[] EquipmentIds)
        {
            if (ModelState.IsValid)
            {
                var context = new VITVSecondContext();
                if (EmployeeIds != null)
                {
                    job.Employees = new List<Employee>();
                    foreach (var empId in EmployeeIds)
                    {
                        var employee = context.Employees.Find(empId);
                        if (employee != null) job.Employees.Add(employee);
                    }
                }

                if(EquipmentIds!=null)
                {
                    job.Equipments = new List<Equipment>();
                    foreach(var equipId in EquipmentIds)
                    {
                        var equip = context.Equipments.Find(Convert.ToInt32(equipId));
                        if (equip != null) job.Equipments.Add(equip);
                    }
                }

                context.Jobs.Add(job);
                context.SaveChanges();
                var taskRequestId = context.JobLists.Find(job.JobListId).TaskRequestId;
                var notification = new Notification()
                {
                    FromId = User.Identity.GetUserId(),
                    NotificationTypeId = NotificationTypes.AddedToJob,
                    JobId = job.Id,
                    Url = "/TaskRequest/Details/" + taskRequestId,
                    CreatedDate = DateTime.Now
                };

                context.Notifications.Add(notification);
                context.SaveChanges();

                if (job.Employees != null && job.Employees.Count > 0)
                {
                    foreach (var employee in job.Employees)
                    {
                        var userNotification = new UserNotification()
                        {
                            //IsRead = false,
                            //CreatedTime = DateTime.Now,
                            EmployeeId = employee.Id,
                            Notification = notification
                        };
                        context.UserNotifications.Add(userNotification);
                    }
                    context.SaveChanges();

                    foreach (var employee in job.Employees)
                    {
                        Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).reloadNotification();
                        Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).popupNotification(employee.Id, notification.Id, job.Title);
                    }
                }

                var checkItems = context.Jobs.Where(it => it.JobListId == job.JobListId);
                var done = checkItems.Count(it => it.Done);
                var all = checkItems.Count();
                return Json(new { success = true, checkItemId = job.Id, checklistId = job.JobListId, done, all });
            }
            else
                return Json(new { success = false });
        }
        [HttpPost]

        public ActionResult Check(int id, bool value)
        {

            var context = new VITVSecondContext();
            var item = context.Jobs.Find(id);
            item.Done = value;
            context.SaveChanges();
            var done = item.JobList.Jobs.Count(it => it.Done);
            var all = item.JobList.Jobs.Count();
            return Json(new { success = true, checkItemId = item.Id, checklistId = item.JobListId, done, all });

        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditProperty(int pk, string name, object value)
        {

            var context = new VITVSecondContext();
            var job = context.Jobs.Find(pk);
            if (name == "Employees")
            {

                var idList = value is string[]? (value as string[]).ToList(): new List<string>();
                var idOldList = job.Employees.Select(e => e.Id).ToList();

                var notification = context.Notifications.FirstOrDefault(n => n.NotificationTypeId == NotificationTypes.AddedToJob && n.JobId == pk);
                if(notification==null)
                {
                    notification = new Notification
                    {
                        JobId = pk,
                        NotificationTypeId = NotificationTypes.AddedToJob,
                        FromId = User.Identity.GetUserId(),
                        Url = "/TaskRequest/Details/" + job.JobList.TaskRequestId,
                        CreatedDate = DateTime.Now
                    };
                    context.Notifications.Add(notification);
                    context.SaveChanges();
                }
                foreach (var id in idList)
                {
                    if (!idOldList.Contains(id))
                    {
                        var employee = context.Employees.Find(id);
                        job.Employees.Add(employee);
                        
                        //notify to user
                        var userNotification = new UserNotification()
                        {
                            //IsRead = false,
                            //CreatedTime = DateTime.Now,
                            EmployeeId = employee.Id,
                            Notification = notification
                        };
                        context.UserNotifications.Add(userNotification);
                        context.SaveChanges();

                        Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).reloadNotification();
                        Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).popupNotification(employee.Id, notification.Id, job.Title);
                    }
                }
                

                foreach (var id in idOldList)
                {
                    if (!idList.Contains(id))
                    {
                        var employee = context.Employees.Find(id);
                        if (employee != null)
                        {
                            job.Employees.Remove(employee);

                            var userNotification = context.UserNotifications.Find(id, notification.Id);
                            if (userNotification != null) context.UserNotifications.Remove(userNotification);
                        }
                    }
                }
            }
            else
            {
                job.GetType().GetProperty(name).SetValue(job, (value as string[])[0]);
            }
            context.SaveChanges();

            var checkItems = context.Jobs.Where(it => it.JobListId == job.JobListId);
            var done = checkItems.Count(it => it.Done);
            var all = checkItems.Count();

            return Json(new { success = true, checkItemId = job.Id, checklistId = job.JobListId, done, all });

        }
        public ActionResult Delete(int? id)
        {
            var context = new VITVSecondContext();
            var job = context.Jobs.Find(id);

            return PartialView(job);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var context = new VITVSecondContext();
            var job = context.Jobs.Find(id);
            
            var notifications = context.Notifications.Where(n => n.JobId == id).ToList();
            context.Notifications.RemoveRange(notifications);

            if (job != null)
            {
                context.Jobs.Remove(job);
                context.SaveChanges();
                return Json(new { success = true, id });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public ActionResult EditAttachments(int id, List<HttpPostedFileBase> attachments, ICollection<string> uploadedAttachments)
        {
            var context = new VITVSecondContext();
            var job = context.Jobs.Find(id);
            //context.Entry<Job>(job).Collection(r => r.Attachments).Load();
            //job.Attachments.Clear();
            foreach(var attachment in job.Attachments.ToList())
            {
                if(uploadedAttachments==null || !uploadedAttachments.Contains(attachment.AttachmentLink))
                {
                    job.Attachments.Remove(attachment);
                }
            }
            //if (uploadedAttachments != null)
            //{
            //    foreach (var uAttachment in uploadedAttachments)
            //    {
            //        job.Attachments.Add(new JobAttachment
            //        {
            //            AttachmentLink = uAttachment
            //        });
            //    }
            //}
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    if (attachment != null)
                    {
                        var path = Server.MapPath("~/Upload/Attachment/" + DateTime.Now.ToString("yyyy/MM/dd"));
                        if (!Directory.Exists(path))
                        { 
                            Directory.CreateDirectory(path);
                        }
                        attachment.SaveAs(path + "/" + attachment.FileName);
                        var link = ("/Upload/Attachment/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + attachment.FileName);
                        job.Attachments.Add(new JobAttachment
                        {
                            AttachmentLink = link
                        });
                    }
                }
            }
            context.Entry(job).State = EntityState.Modified;

            context.SaveChanges();
            return Json(new { success = true, id = job.Id });
        }

        public ActionResult MyJobs()
        {
            var userId = User.Identity.GetUserId();
            var context = new VITVSecondContext();
            var onProgressJobs = context.Jobs.Where(j => j.Employees.Any(e => e.Id == userId) && j.Done == false).ToList();
            var doneJobs = context.Jobs.Where(j => j.Employees.Any(e => e.Id == userId) && j.Done == true).ToList();
            var model = new MyJobs()
            {
                OnProgress = onProgressJobs,
                Done = doneJobs
            };
            return View(model);
        }
    }
}