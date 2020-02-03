using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using Microsoft.AspNet.Identity;

namespace VITV.HRM.Controllers
{
    public class JobResponseController : Controller
    {
        // GET: JobResponse
        [Authorize]
        [HttpPost]
        public ActionResult Create(JobResponse response, List<HttpPostedFileBase> attachments)
        {
            var context = new VITVSecondContext();
            response.EmployeeId = User.Identity.GetUserId();
            response.CreatedTime = DateTime.Now;
            if (attachments != null)
            {
                response.Attachments = new List<ResponseAttachment>();
                foreach (var attachment in attachments)
                {
                    if (attachment != null)
                    {
                        attachment.SaveAs(Server.MapPath("~/Upload/Attachment/" + attachment.FileName));
                        var link = ("/Upload/Attachment/" + attachment.FileName);
                        response.Attachments.Add(new ResponseAttachment
                        {
                            AttachmentLink = link
                        });
                    }
                }
            }
            context.JobResponses.Add(response);
            context.SaveChanges();

            Job job = context.Jobs.Find(response.JobId);

            var notification = new Notification()
            {
                FromId = User.Identity.GetUserId(),
                NotificationTypeId = NotificationTypes.ReceivedAResponse,
                RequestId = response.Job.JobList.TaskRequestId,
                Url = "/TaskRequest/Details/" + response.Job.JobList.TaskRequestId,
                CreatedDate = DateTime.Now
            };
            context.Notifications.Add(notification);
            var employees = new HashSet<Employee>(context.JobResponses.Where(r => r.JobId == response.JobId && r.EmployeeId != response.EmployeeId).Select(r => r.Employee).Distinct());

            foreach (var employee in employees)
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

            foreach (var employee in employees)
            {
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).reloadNotification();
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).popupNotification(employee.Id, notification.Id, response.Content);
            }

            return Json(new { success = true, id = response.Id });
        }

        public ActionResult Details(int id)
        {
            var context = new VITVSecondContext();
            var response = context.JobResponses.Find(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Details", response);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Delete(int? id)
        {
            var context = new VITVSecondContext();
            var response = context.JobResponses.Find(id);

            return PartialView(response);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var context = new VITVSecondContext();
            var response = context.JobResponses.Find(id);

            context.JobResponses.Remove(response);

            context.SaveChanges();
            return Json(new { success = true });
        }

    }
}