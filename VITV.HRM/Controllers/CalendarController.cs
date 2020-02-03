using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using Microsoft.AspNet.Identity;
using VITV.HRM.Models.View;
namespace VITV.HRM.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index(string id)
        {   
            if (string.IsNullOrEmpty(id))
                id = User.Identity.GetUserId();
            return RedirectToAction("Year", "Calendar", new{ id = id});
                
            //var db = new VITVSecondContext();
            
            //var personalJobs = db.PersonalJobs.Where(t => t.EmployeeId == id).ToList();
            //var jobs = db.Jobs.Where(t => t.StartTime.HasValue && t.EndTime.HasValue && t.Employees.Any(e => e.Id == id)).ToList();
            //var personalDayoffs = db.PersonalDayoffs.Where(d => d.EmployeeId == id).ToList();
            //var calendarModel = new CalendarModel()
            //{
            //    Conferences = db.Conferences.ToList(),
            //    Dayoffs = db.Dayoffs.ToList(),
            //    Employee = db.Employees.Find(id),
            //    Jobs = jobs,
            //    PersonalJobs = personalJobs,
            //    PersonalDayOffs = personalDayoffs
            //};

            //return View();
        }
      
        public ActionResult Year(string id, int? y)
        {
            if (string.IsNullOrEmpty(id))
                id = User.Identity.GetUserId();
            if (!y.HasValue)
                y = DateTime.Now.Year;
            var db = new VITVSecondContext();
            var model = new CalendarYearModel() { 
                Year = y.Value,
                Employee = db.Employees.Find(id)
            };
            return View(model);
        }

        public ActionResult Month(string id, int? m, int? y)
        {
            var db = new VITVSecondContext();
            if (string.IsNullOrEmpty(id))
                id = User.Identity.GetUserId();
            if (!m.HasValue)
                m = DateTime.Now.Month;
            if (!y.HasValue)
                y = DateTime.Now.Year;
            var model = new CalendarMonthModel()
            {
                Year = y.Value,
                Month = m.Value,
                Employee = db.Employees.Find(id)
            };

            return View(model);
        }

        [Authorize(Roles="Admin")]
        public ActionResult All(int? group,int? m, int? y)
        {
            var db = new VITVSecondContext();
            if (!group.HasValue) group = 0;
            if(!m.HasValue||!y.HasValue)
            {
                m = DateTime.Now.Month;
                y = DateTime.Now.Year;
            };
            var empAttendance = db.Employees.Where(e=>group==0||e.WorkInfo.GroupId==group).Select(e => new EmployeeAttendance
            {
                EmployeeId = e.Id,
                EmployeeName = e.Name,
                Dayoffs = e.PersonalDayoffs.Where(d=>d.Start.Month == m && d.Start.Year==y).ToList(),
            }).ToList();
            var model = new CalendarAllModel()
            {
                GroupId = group.Value,
                Month = m.Value,
                Year = y.Value,
                EmployeeAttendances = empAttendance
            };
            ViewBag.Groups = db.Groups.Where(g => g.EmployeeWorkInfos.Count != 0).ToList();
            return View(model);
        }
        public ActionResult AttendanceSheet(string userId, int year)
        {
            var db = new VITVSecondContext();
            if(string.IsNullOrEmpty(userId))
                userId = User.Identity.GetUserId();
            var dayoffs = db.PersonalDayoffs.Where(d => d.Start.Year == year && d.EmployeeId == userId).ToList().Select(d => 
                new { 
                    date = d.Start.ToShortDateString(),
                    allDay = d.AllDay.ToString().ToLower(),
                    start = d.Start.ToShortTimeString(),
                    end = d.End.ToShortTimeString(),
                    title = d.Title,
                    id = d.Id
                }
            );

            return Json(new { dayoffs = dayoffs }, JsonRequestBehavior.AllowGet);
        }
    }
}