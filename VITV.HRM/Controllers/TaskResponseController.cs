using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using Microsoft.AspNet.Identity;

namespace VITV.HRM.Controllers
{
    public class TaskResponseController : Controller
    {
        // GET: TaskResponse
        [Authorize]
        [HttpPost]
        public ActionResult Create(TaskResponse response, List<HttpPostedFileBase> attachments)
        {
            var context = new VITVSecondContext();
            response.EmployeeId = User.Identity.GetUserId();
            response.CreatedTime = DateTime.Now;
            //if (attachments != null)
            //{
            //    response.Attachments = new List<ResponseAttachment>();
            //    foreach (var attachment in attachments)
            //    {
            //        if (attachment != null)
            //        {
            //            attachment.SaveAs(Server.MapPath("~/Upload/Attachment/" + attachment.FileName));
            //            var link = ("/Upload/Attachment/" + attachment.FileName);
            //            response.Attachments.Add(new ResponseAttachment
            //            {
            //                AttachmentLink = link
            //            });
            //        }
            //    }
            //}
            context.TaskResponses.Add(response);
            context.SaveChanges();

            var request = context.TaskRequests.Find(response.TaskRequestId);

            var notification = new Notification()
            {
                FromId = User.Identity.GetUserId(),
                NotificationTypeId = NotificationTypes.ReceivedAResponse,
                RequestId = response.TaskRequestId,
                Url = "/TaskRequest/Details/" + response.TaskRequestId,
                CreatedDate = DateTime.Now
            };
            context.Notifications.Add(notification);
            var employees = new HashSet<Employee>(context.TaskResponses.Where(r => r.TaskRequestId == response.TaskRequestId && r.EmployeeId != response.EmployeeId).Select(r => r.Employee).Distinct());
            
            if(response.EmployeeId != request.RequestedEmployeeId)
                employees.Add(request.RequestedEmployee);

            foreach (var employee in employees)
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

            foreach (var employee in employees)
            {
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).reloadNotification();
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.User(employee.Id).popupNotification(employee.Id, notification.Id, response.Content);
            }

            return Json(new { success = true, id = response.Id});
        }

        public ActionResult Details(int id)
        {
            var context = new VITVSecondContext();
            var response = context.TaskResponses.Find(id);
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
            var response = context.TaskResponses.Find(id);

            return PartialView(response);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, bool permanent)
        {
            var context = new VITVSecondContext();
            var response = context.TaskResponses.Find(id);
            if (permanent)
            {
                context.TaskResponses.Remove(response);
            }
            else
            {
                response.IsDeleted = true;
            }
            context.SaveChanges();
            return Json(new { success = true });
        }

    }
}