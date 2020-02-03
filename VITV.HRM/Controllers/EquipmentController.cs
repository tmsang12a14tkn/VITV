using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using Microsoft.AspNet.Identity;

namespace VITV.HRM.Controllers
{
    public class EquipmentController : Controller
    {
        private VITVSecondContext db = new VITVSecondContext();

        // GET: Equipment
        public ActionResult Index()
        {
            return View(db.Equipments);
        }

        // GET: Equipment/Create
        public ActionResult Create()
        {
            ViewBag.Employees = db.Employees.ToList();
            return View();
        }

        // POST: Equipment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase imageFile, Equipment equipment)
        {
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                imageFile.SaveAs(Server.MapPath("~/Upload/Equipment/" + imageFile.FileName));
                var link = ("/Upload/Equipment/" + imageFile.FileName);
                equipment.EquipPicture = link;
            }
            ModelState.Clear();
            TryValidateModel(equipment);

            if (ModelState.IsValid)
            {
                db.Equipments.Add(equipment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Employees = db.Employees.ToList();
            return View(equipment);
        }

        // GET: Equipment/Edit
        public ActionResult Edit(int id)
        {
            var equip = db.Equipments.Find(id);
            ViewBag.Employees = db.Employees.ToList();
            return View(equip);
        }

        // POST: Equipment/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Employees = db.Employees.ToList();
            return View(equipment);
        }

        // GET: Delete/Edit
        public PartialViewResult Delete(int id)
        {
            var equip = db.Equipments.Find(id);
            return PartialView("_Delete", equip);
        }

        // POST: Delete/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(Equipment equipment)
        {
            var equip = db.Equipments.Find(equipment.Id);
            db.Equipments.Remove(equip);
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult UploadPicture(int id, HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/Upload/Equipment/" + file.FileName));
            var link = ("/Upload/Equipment/" + file.FileName);
            var equip = db.Equipments.Find(id);
            equip.EquipPicture = link;
            db.SaveChanges();
            return Json(new { link = link });
        }

        public ActionResult GetList()
        {
            var context = new VITVSecondContext();
            return Json(context.Equipments.Select(e => new { id = e.Id, text = e.Name }).ToList(), JsonRequestBehavior.AllowGet);
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