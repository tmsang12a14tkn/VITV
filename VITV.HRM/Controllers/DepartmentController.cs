using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.ViewModels;

namespace VITV.HRM.Controllers
{
    public class DepartmentController : Controller
    {
        private VITVSecondContext db = new VITVSecondContext();
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetByBranchId(int id)
        {
            var data = new ArrayList();
            var model = db.Branches.Find(id);

            if (model.Departments.Count > 0)
            {
                foreach (var dp in model.Departments)
                {
                    var lvl1Item = new TreeModel()
                    {
                        name = dp.Name,
                        type = "folder",
                        id = "department-" + dp.Id
                    };
                    
                    data.Add(lvl1Item);
                }
            }

            var addRow = new TreeModel()
            {
                name = "Thêm phòng ban",
                type = "button",
                id = "add-department-" + id
            };
            data.Add(addRow);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Department/Create
        public PartialViewResult Create(int id)
        {
            return PartialView("~/Views/Department/_addDepartment.cshtml", new Department { BranchId = id });
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index", "Department");
            }

            return PartialView("~/Views/Department/_addDepartment.cshtml", department);
        }

        public PartialViewResult Delete(int? id)
        {
            if (id == null)
            {
                return PartialView("_Delete");
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return PartialView("_Delete");
            }
            return PartialView("_Delete", department);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Include("Groups").FirstOrDefault(d => d.Id == id);
            if (department.Groups != null)
                foreach (Group group in department.Groups.ToList())
                {
                    foreach (var positionLevel in group.PositionLevels.ToList())
                    {
                        group.PositionLevels.Remove(positionLevel);
                    }
                    foreach (var ewInfo in group.EmployeeWorkInfos.ToList())
                    {
                        ewInfo.PositionLevelId = null;
                        ewInfo.GroupId = null;
                    }
                    db.Groups.Remove(group);

                }
            db.Departments.Remove(department);

            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}