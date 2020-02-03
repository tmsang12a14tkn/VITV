using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using Microsoft.AspNet.Identity;

namespace VITV.HRM.Controllers
{
    public class NotificationController : Controller
    {
        public ActionResult Get()
        {
            var context = new VITVSecondContext();
            var userId = User.Identity.GetUserId();
            DateTime last7Days = DateTime.Now.AddDays(-7);
            var userNotifications = context.UserNotifications.Where(n => n.EmployeeId == userId && n.Notification.CreatedDate >= last7Days).OrderByDescending(n=>n.Notification.CreatedDate).ToList();
            return PartialView(userNotifications);
        }

        public ActionResult Count()
        {
            var context = new VITVSecondContext();
            var userId = User.Identity.GetUserId();
            var notifiCount = context.UserNotifications.Count(n => n.EmployeeId == userId);
            return Content(notifiCount.ToString());
        }

        public ActionResult Popup(string employeeId, int notifiId)
        {
            var context = new VITVSecondContext();
            var userNotifi = context.UserNotifications.Find(employeeId, notifiId);
            return Json(new { userNotifi.EmployeeId, userNotifi.Notification.Content, userNotifi.Employee.ProfilePicture, userNotifi.Notification.Url }, JsonRequestBehavior.AllowGet);
            //return PartialView(userNotifi);
        }
    }
}