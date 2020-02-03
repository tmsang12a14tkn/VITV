using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.Models.View;

namespace VITV.HRM.Controllers
{
    public class TaskRequestController : Controller
    {
        // GET: TaskRequest
        [Authorize]
        public ActionResult MyRequests()
        {
            var context = new VITVSecondContext();
            var userId = User.Identity.GetUserId();
            var requestsToMe = context.TaskRequests.Where(tr => tr.ReceivedEmployees.Any(e=>e.Id == userId)).ToList();

            var model = new EmployeeRequests
            {
                RequestsToMe = requestsToMe
            };

            return View(model);
        }
        public ActionResult Details(int id)
        {
            var context = new VITVSecondContext();
            var request = context.TaskRequests.Find(id);
            if (request != null)
            {
                ViewBag.Me = context.Employees.Find(User.Identity.GetUserId());
                ViewBag.CanEdit = User.IsInRole("Admin") || request.Project.Employees.Any(e => e.Id == ViewBag.Me.Id);
                if (Request.IsAjaxRequest())
                    return PartialView(request);
                else
                {
                    return View(request);
                }
            }
            else
            {
                return new HttpNotFoundResult();
            }
        }


        public ActionResult Add(int? projectId)
        {
            var userId = User.Identity.GetUserId();
            var context = new VITVSecondContext();
            ViewBag.Employees = context.Employees.ToList();
            ViewBag.Projects = context.Projects.ToList();
            ViewBag.Equipments = context.Equipments.Where(e => e.EmployeeId == userId).ToList();
            TaskRequest request = new TaskRequest();
            if (projectId.HasValue)
            {
                var project = context.Projects.Find(projectId);
                request.ProjectId = projectId;
                request.Project = project;
            }
            return PartialView(request);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(TaskRequest request, List<HttpPostedFileBase> attachments)
        {
            var context = new VITVSecondContext();
            request.RequestedEmployeeId = User.Identity.GetUserId();
            //request.Piority = 1;
            request.Status = 0;
            request.CreatedTime = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(request.ReceivedEmployeeIds))
            {
                request.ReceivedEmployees = new List<Employee>();
                var employeeIds = request.ReceivedEmployeeIds.Split(',');
                foreach (string eId in employeeIds)
                {
                    if (!string.IsNullOrWhiteSpace(eId))
                    {
                        var eIdNormalize = eId.Trim();
                        var employee = context.Employees.Find(eIdNormalize);
                        if (employee != null)
                            request.ReceivedEmployees.Add(employee);
                    }
                }
            }
            //if(attachments!=null)
            //{
            //    request.Attachments = new List<JobAttachment>();
            //    foreach(var attachment in attachments)
            //    {
            //        if (attachment != null)
            //        {
            //            attachment.SaveAs(Server.MapPath("~/Upload/Attachment/" + attachment.FileName));
            //            var link = ("/Upload/Attachment/" + attachment.FileName);
            //            request.Attachments.Add(new JobAttachment
            //            {
            //                AttachmentLink = link
            //            });
            //        }
            //    }
            //}

            context.TaskRequests.Add(request);
            context.SaveChanges();
            var notification = new Notification()
            {
                FromId = User.Identity.GetUserId(),
                NotificationTypeId = NotificationTypes.AddedToTask,
                RequestId = request.Id,
                Url = "/TaskRequest/Details/" + request.Id,
                CreatedDate = DateTime.Now
            };

            context.Notifications.Add(notification);
            context.SaveChanges();

            if (request.ReceivedEmployees != null && request.ReceivedEmployees.Count > 0)
            {
                foreach (var employee in request.ReceivedEmployees)
                {
                    var userNotification = new UserNotification()
                    {
                        //IsRead = false,
                        EmployeeId = employee.Id,
                        Notification = notification
                    };
                    context.UserNotifications.Add(userNotification);
                    context.SaveChanges();
                }

                foreach (var employee in request.ReceivedEmployees)
                {
                    Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).reloadNotification();
                    Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).popupNotification(employee.Id, notification.Id, request.Content);
                }
            }
            return Json(new { success = true});
            //return RedirectToAction("MyRequests");

        }

        public ActionResult Edit(int id)
        {
            var context = new VITVSecondContext();
            var request = context.TaskRequests.Find(id);
            ViewBag.Employees = context.Employees.ToList();
            return PartialView(request);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(TaskRequest request, List<HttpPostedFileBase> attachments, ICollection<string> uploadedAttachments)
        {
            var context = new VITVSecondContext();
            var entry = context.TaskRequests.Find(request.Id);
           
            context.Entry(entry).State = EntityState.Detached;

            request.CreatedTime = entry.CreatedTime;
            request.Status = entry.Status;

            context.TaskRequests.Attach(request);

            if (!string.IsNullOrWhiteSpace(request.ReceivedEmployeeIds))
            {
                context.Entry<TaskRequest>(request).Collection(r => r.ReceivedEmployees).Load();
                request.ReceivedEmployees.Clear();

                var employeeIds = request.ReceivedEmployeeIds.Split(',');
                foreach (string eId in employeeIds)
                {
                    if (!string.IsNullOrWhiteSpace(eId))
                    {
                        var eIdNormalize = eId.Trim();
                        var employee = context.Employees.Find(eIdNormalize);
                        if (employee != null)
                            request.ReceivedEmployees.Add(employee);
                    }
                }
            }

            //context.Entry<TaskRequest>(request).Collection(r => r.Attachments).Load();
            //request.Attachments.Clear();
            //if (uploadedAttachments != null)
            //{
            //    foreach (var uAttachment in uploadedAttachments)
            //    {
            //        request.Attachments.Add(new JobAttachment
            //        {
            //            AttachmentLink = uAttachment
            //        });
            //    }
            //}
            //if (attachments != null)
            //{
            //    foreach (var attachment in attachments)
            //    {
            //        if (attachment != null)
            //        {
            //            attachment.SaveAs(Server.MapPath("~/Upload/Attachment/" + attachment.FileName));
            //            var link = ("/Upload/Attachment/" + attachment.FileName);
            //            request.Attachments.Add(new JobAttachment
            //            {
            //                AttachmentLink = link
            //            });
            //        }
            //    }
            //}
            context.Entry(request).State = EntityState.Modified;
            
            context.SaveChanges();
            return Json(new { success = true, id = request.Id });
            //return RedirectToAction("MyRequests");
        }

        public ActionResult Delete(int? id)
        {
            var context = new VITVSecondContext();
            var request = context.TaskRequests.Find(id);

            return PartialView(request);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, bool permanent)
        {
            var context = new VITVSecondContext();
            var request = context.TaskRequests.Find(id);
            if (permanent)
            {
                var notifications = context.Notifications.Where(n=>n.RequestId == id).ToList();
                context.Notifications.RemoveRange(notifications);

                foreach(var joblist in request.JobLists)
                {
                    foreach(var job in joblist.Jobs)
                    {
                        var jobNotifications = context.Notifications.Where(n => n.JobId == job.Id).ToList();
                        context.Notifications.RemoveRange(jobNotifications);
                    }
                }
                
                context.TaskRequests.Remove(request);
            }
            else
            {
                request.IsDeleted = true;
            }
            context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult ChangeStatus(int id, int status)
        {
            var context = new VITVSecondContext();
            var request = context.TaskRequests.Find(id);
            request.Status = status;
            context.SaveChanges();
            return Json(new { success = true, id = id });
        }
    }
}