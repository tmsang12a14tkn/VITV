using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV_Web.Controllers
{
    public class TVCProductGroupController : Controller
    {
        private VITVContext db = new VITVContext();

        // GET: TVCProductGroup
        public ActionResult Index()
        {
            return View(db.TVCProductGroups.ToList());
        }

        // GET: TVCProductGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCProductGroup tVCProductGroup = db.TVCProductGroups.Find(id);
            if (tVCProductGroup == null)
            {
                return HttpNotFound();
            }
            return View(tVCProductGroup);
        }

        // GET: TVCProductGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TVCProductGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] TVCProductGroup tVCProductGroup)
        {
            if (ModelState.IsValid)
            {
                db.TVCProductGroups.Add(tVCProductGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tVCProductGroup);
        }

        // GET: TVCProductGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCProductGroup tVCProductGroup = db.TVCProductGroups.Find(id);
            if (tVCProductGroup == null)
            {
                return HttpNotFound();
            }
            return View(tVCProductGroup);
        }

        // POST: TVCProductGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] TVCProductGroup tVCProductGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tVCProductGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tVCProductGroup);
        }

        // GET: TVCProductGroup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCProductGroup tVCProductGroup = db.TVCProductGroups.Find(id);
            if (tVCProductGroup == null)
            {
                return HttpNotFound();
            }
            return View(tVCProductGroup);
        }

        // POST: TVCProductGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TVCProductGroup tVCProductGroup = db.TVCProductGroups.Find(id);
            db.TVCProductGroups.Remove(tVCProductGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
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
