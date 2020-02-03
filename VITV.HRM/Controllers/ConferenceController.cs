using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.ViewModels;
using VITV.HRM.Helpers;

namespace VITV.HRM.Controllers
{
    public class ConferenceController : Controller
    {
        // GET: Conference
        public ActionResult Index()
        {
            var db = new VITVSecondContext();
            return View(db.Conferences.ToList());
        }

        [HttpPost]
        public ActionResult Add(string content, DateTime start, DateTime end, List<string> employees)
        {
            var db = new VITVSecondContext();
            if(!String.IsNullOrEmpty(content))
            {
                var confer = new Conference()
                {
                    Content = content,
                    Start = start,
                    End = end,

                };
                db.Conferences.Add(confer);
                db.SaveChanges();

                if (employees != null)
                {
                    foreach (string emp in employees)
                    {
                        Employee r = db.Employees.Find(emp);
                        if (r != null)
                            confer.Employees.Add(r);
                    }
                }
                db.SaveChanges();
                return Json(new { success = true, id = confer.Id });
            }
            //TODO
            return Json(new { success = false});
        }

        [HttpPost]
        public ActionResult Edit(int id, string content, List<string> employees)
        {
            var db = new VITVSecondContext();
            var confer = db.Conferences.Find(id);
            if (confer != null && !String.IsNullOrEmpty(content))
            {
                confer.Content = content;
                db.SaveChanges();

                db.Conferences.Attach(confer);
                db.Entry(confer).Collection(e => e.Employees).Load();

                List<Employee> lstEmp = confer.Employees.ToList();
                foreach (Employee r in lstEmp)
                    confer.Employees.Remove(r);

                if (employees != null)
                {
                    foreach (string emp in employees)
                    {
                        Employee r = db.Employees.Find(emp);
                        if (r != null)
                            confer.Employees.Add(r);
                    }
                }
                db.SaveChanges();
                return Json(new { success = true, id = confer.Id });
            }
            return Json(new { success = false, id = confer.Id });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var db = new VITVSecondContext();
            var confer = db.Conferences.Find(id);
            if(confer != null)
            {
                db.Conferences.Remove(confer);
                db.SaveChanges();
                return Json(new { success = true, id = confer.Id });
            }
            return Json(new { success = false, id = confer.Id });
        }

        [HttpPost]
        public ActionResult GetDialogAddConference()
        {
            string dialog = Utilities.RenderRazorViewToString(this, "/Views/Conference/_GetDialogConference.cshtml", null);
            return Json(dialog);
        }

        [HttpPost]
        public ActionResult GetDialogEditDeleteConference(int id, string content)
        {
            var db = new VITVSecondContext();
            var confer = db.Conferences.Find(id);
            if (confer != null)
            {
                db.Conferences.Attach(confer);
                db.Entry(confer).Collection(e => e.Employees).Load();
                ViewBag.employees = confer.Employees.ToList();
            }
            ViewBag.content = content;
            string dialog = Utilities.RenderRazorViewToString(this, "/Views/Conference/_GetDialogEditDeleteConference.cshtml", null);
            return Json(dialog);
        }
    }
}