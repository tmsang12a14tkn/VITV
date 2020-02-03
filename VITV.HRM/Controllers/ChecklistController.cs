using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using VITV.HRM.Models;

namespace VITV.HRM.Controllers
{
    public class ChecklistController : Controller
    {
        public ActionResult List(int taskRequestId)
        {
            var context = new VITVSecondContext();
            var checklists = context.JobLists.Where(cl => cl.TaskRequestId == taskRequestId).ToList();
            return PartialView(checklists);
        }
        public ActionResult Details(int id)
        {
            var context = new VITVSecondContext();
            var checklist = context.JobLists.Find(id);
            var userId = User.Identity.GetUserId();
            ViewBag.Me = context.Employees.Find(userId);
            var project = context.Projects.Find(checklist.TaskRequest.ProjectId);
            ViewBag.CanEdit = User.IsInRole("Admin") || project.Employees.Any(e => e.Id == userId);
            return PartialView(checklist);
        }
        [HttpPost]
        public ActionResult Create(JobList checklist)
        {
            if (ModelState.IsValid)
            {
                var context = new VITVSecondContext();
                context.JobLists.Add(checklist);
                context.SaveChanges();
                return Json(new { success = true, checklistId = checklist.Id, taskRequestId = checklist.TaskRequestId });
            }
            else
                return Json(new { success = false });
        }
       
        public ActionResult Item(int id)
        {
            var context = new VITVSecondContext();
            var item = context.Jobs.Find(id);
            //var userId = User.Identity.GetUserId();
            //ViewBag.Me = context.Employees.Find(userId);
            return PartialView(item);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var context = new VITVSecondContext();
            var joblist = context.JobLists.Find(id);
            if (joblist != null)
            {
                foreach (var job in joblist.Jobs)
                {
                    var jobNotifications = context.Notifications.Where(n => n.JobId == job.Id).ToList();
                    context.Notifications.RemoveRange(jobNotifications);
                }
                
                context.JobLists.Remove(joblist);
                context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }

        [HttpPost]
        public ActionResult EditProperty(int pk, string name, string value)
        {
            var context = new VITVSecondContext();
            var checklist = context.JobLists.Find(pk);
            checklist.GetType().GetProperty(name).SetValue(checklist, value);
            context.SaveChanges();

            return Json(new {success= true});
        }
    }
}