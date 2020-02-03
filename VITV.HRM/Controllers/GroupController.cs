using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.ViewModels;
using Microsoft.AspNet.Identity;

namespace VITV.HRM.Controllers
{
    public class GroupController : Controller
    {
        private VITVSecondContext db = new VITVSecondContext();

        // GET: Group
        public ActionResult Index()
        {
            var departments = db.Groups.Include(d => d.Department);
            return View(departments.ToList());
        }

        public ActionResult MyGroup()
        {
            var userId = User.Identity.GetUserId();
            var employee = db.Employees.Find(userId);
            var group = employee.WorkInfo.Group;
            return View("Details",group);
        }

        public ActionResult Details(int id)
        {
            var group = db.Groups.Find(id);
            return View(group);
        }

        public PartialViewResult RemovePositionLevel(int groupId, int positionLevelId)
        {
            var groupPositionLevel = new GroupPositionLevelView
            {
                GroupId = groupId,
                PositionLevelId = positionLevelId
            };
            return PartialView("_ConfirmRemovePositionLevel", groupPositionLevel);
        }

        [HttpPost]
        public JsonResult RemovePositionLevel(GroupPositionLevelView groupPositionLevel)
        {
            var group = db.Groups.Find(groupPositionLevel.GroupId);

            group.PositionLevels.Remove(group.PositionLevels.FirstOrDefault(pl => pl.Id == groupPositionLevel.PositionLevelId));
            
            var ungroupEmployees = group.EmployeeWorkInfos.Where(ep => ep.PositionLevelId == groupPositionLevel.PositionLevelId).ToList();
            foreach (EmployeeWorkInfo ewInfo in ungroupEmployees)
            {
                ewInfo.PositionLevelId = null;
                group.EmployeeWorkInfos.Remove(ewInfo);
            }
            db.SaveChanges();
            return Json(new { success = true });
        }

        public JsonResult GetByDepartmentId(int id)
        {
            var data = new ArrayList();
            var model = db.Departments.Find(id);

            if (model.Groups.Count > 0)
            {
                foreach (var gp in model.Groups)
                {
                    var lvl1Item = new TreeModel()
                    {
                        name = gp.Name,
                        type = "item",
                        id = "group-" + gp.Id
                    };

                    data.Add(lvl1Item);
                }
            }

            var addRow = new TreeModel()
            {
                name = "Thêm tổ",
                type = "button",
                id = "add-group-" + id
            };
            data.Add(addRow);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
                
        public PartialViewResult Create(int id)
        {
            return PartialView("~/Views/Group/_AddGroup.cshtml", new Group { DepartmentId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index", "Department");
            }

            return PartialView("~/Views/Group/_AddGroup.cshtml", group);
        }

        // GET: Group/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group department = db.Groups.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", department.DepartmentId);
            return View(department);
        }

        // POST: Group/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DepartmentId")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", group.DepartmentId);
            return View(group);
        }

        // GET: Group/Delete/5
        public PartialViewResult Delete(int? id)
        {
            if (id == null)
            {
                return PartialView("_Delete");
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return PartialView("_Delete");
            }
            return PartialView("_Delete", group);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            foreach (var ewInfo in group.EmployeeWorkInfos.ToList())
            {
                ewInfo.PositionLevelId = null;
            }
            db.Groups.Remove(group);
            db.SaveChanges();
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
