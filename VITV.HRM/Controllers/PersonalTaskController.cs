using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.Helpers;
using Microsoft.AspNet.Identity;

namespace VITV.HRM.Controllers
{
    public class PersonalTaskController : Controller
    {
        // POST
        [HttpPost]
        public ActionResult Add(string title, DateTime start, DateTime end)
        {
            var db = new VITVSecondContext();
            var task = new PersonalJob()
                {
                    EmployeeId = User.Identity.GetUserId(),
                    End = end,
                    //IsAbsent = isabsent,
                    Start = start,
                    Title = title
                };
            db.PersonalJobs.Add(task);
            db.SaveChanges();
            //TODO
            return Json(new { success = true, id = task.Id});
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var db = new VITVSecondContext();
            var pJob = db.PersonalJobs.Find(id);
            if(pJob != null)
                db.PersonalJobs.Remove(pJob);
            db.SaveChanges();
            return Json(new { success = true, id = id });
        }

        [HttpPost]
        public ActionResult EditTime(int id, DateTime start, DateTime end)
        {
            var db = new VITVSecondContext();
            var employee = db.PersonalJobs.Find(id);
            if(employee != null)
            {
                employee.Start = start;
                employee.End = end;
                db.SaveChanges();
                return Json(new { success = true, id = id });
            }
            
            return Json(new { success = false, id = id });
        }

        [HttpPost]
        public ActionResult Edit(int id, string title)
        {
            var db = new VITVSecondContext();
            var employee = db.PersonalJobs.Find(id);
            if (employee != null)
            {
                employee.Title = title;
                db.SaveChanges();
                return Json(new { success = true, id = id });
            }

            return Json(new { success = false, id = id });
        }

        [HttpPost]
        public ActionResult CheckDayOff(DateTime start, DateTime end)
        {
            var db = new VITVSecondContext();
            var days = db.Dayoffs.ToList();
            DateTime tempEnd = end.AddDays(-1);
            foreach(Dayoff d in days)
            {
                if (Utilities.DoesIncludeSundaySaturday(start, tempEnd) || (start >= d.Begin && start <= d.End.AddDays(-1)) || (start <= d.Begin && tempEnd >= d.Begin) || (start <= d.End.AddDays(-1) && tempEnd >= d.End.AddDays(-1)) || (d.Begin >= start && d.End.AddDays(-1) <= start) || (d.Begin >= tempEnd && d.End.AddDays(-1) <= tempEnd))
                    return Json(new { success = false });
            }
            return Json(new { success = true});
        }
    }
}