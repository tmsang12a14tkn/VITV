using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.ViewModels;

namespace VITV.HRM.Controllers
{
    public class HolidayController : Controller
    {
        private readonly VITVSecondContext context;

        public HolidayController()
        {
            context = new VITVSecondContext();
        }

        // GET: Holiday
        public ActionResult Management()
        {
            return View(context.Dayoffs.ToList());
        }


        public JsonResult GetHolidays()
        {
            var holidays = context.Dayoffs.OrderByDescending(h => h.End).AsEnumerable().Select(spec => new
            {
                Id = spec.Id,
                Name = spec.Note,
                StartDate = String.Format("{0:d/M/yyyy}", spec.Begin),
                EndDate = String.Format("{0:d/M/yyyy}", spec.End)
            }).ToList();
            var total = holidays.Count;
            return Json(new { holidays, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(string title, DateTime start, DateTime end)
        {
            var db = new VITVSecondContext();
            var day = new Dayoff() {
                Note = title,
                Begin = start,
                End = end,
            };
            db.Dayoffs.Add(day);
            db.SaveChanges();
            //TODO
            return Json(new { success = true, id = day.Id });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var db = new VITVSecondContext();
            db.Dayoffs.Remove(db.Dayoffs.Find(id));
            db.SaveChanges();
            return Json(new { success = true, id = id });
        }

        [HttpPost]
        public ActionResult Edit(int id, string title)
        {
            var db = new VITVSecondContext();
            var employee = db.Dayoffs.Find(id);
            if (employee != null)
            {
                employee.Note = title;
                db.SaveChanges();
                return Json(new { success = true, id = id });
            }

            return Json(new { success = false, id = id });
        }
    }
}