using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.Models.View;
namespace VITV.HRM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new VITVSecondContext();
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);
            var userId = User.Identity.GetUserId();

            var todayJobs = context.Jobs.Where(j => (!j.StartTime.HasValue || DbFunctions.TruncateTime(j.StartTime.Value) <= today) && j.EndTime.HasValue && DbFunctions.TruncateTime(j.EndTime.Value) >= today && j.Employees.Any(e => e.Id == userId)).ToList();
            var tomorrowJobs = context.Jobs.Where(j => (!j.StartTime.HasValue || DbFunctions.TruncateTime(j.StartTime.Value) <= tomorrow) && j.EndTime.HasValue && DbFunctions.TruncateTime(j.EndTime.Value) >= tomorrow && j.Employees.Any(e => e.Id == userId)).ToList();
            var model = new HomeModel
            {
                TodayJobs = todayJobs,
                TomorrowJobs = tomorrowJobs
            };

            return View(model);
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult TopNotification()
        {
            var userId = User.Identity.GetUserId();
            var context = new VITVSecondContext();
            ViewBag.Employee = context.Employees.Find(userId);
            var jobs = context.Jobs.Where(r => r.Employees.Any(e => e.Id == userId)).ToList();
            return PartialView(jobs);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public PartialViewResult Get10Last(string userId)
        {
            var context = new VITVSecondContext();
            var myId = User.Identity.GetUserId();
            var messages = context.Messages.Where(m => (m.SenderId == myId && m.ReceiverId == userId) || (m.SenderId == userId && m.ReceiverId == myId)).OrderByDescending(m => m.CreatedTime).Take(10).ToList();
            messages.Reverse();
            ViewBag.MyId = myId;
            return PartialView("_Last10", messages);
        }
    }
}