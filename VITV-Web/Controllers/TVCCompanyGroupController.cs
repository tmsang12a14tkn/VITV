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
    public class TVCCompanyGroupController : Controller
    {
        private VITVContext db = new VITVContext();

        // GET: TVCCompanyGroup
        public ActionResult Index()
        {
            return View(db.TVCCompanyGroups.ToList());
        }

        // GET: TVCCompanyGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCCompanyGroup tVCCompanyGroup = db.TVCCompanyGroups.Find(id);
            if (tVCCompanyGroup == null)
            {
                return HttpNotFound();
            }
            return View(tVCCompanyGroup);
        }

        // GET: TVCCompanyGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TVCCompanyGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] TVCCompanyGroup tVCCompanyGroup)
        {
            if (ModelState.IsValid)
            {
                db.TVCCompanyGroups.Add(tVCCompanyGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tVCCompanyGroup);
        }

        // GET: TVCCompanyGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCCompanyGroup tVCCompanyGroup = db.TVCCompanyGroups.Find(id);
            if (tVCCompanyGroup == null)
            {
                return HttpNotFound();
            }
            return View(tVCCompanyGroup);
        }

        // POST: TVCCompanyGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] TVCCompanyGroup tVCCompanyGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tVCCompanyGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tVCCompanyGroup);
        }

        // GET: TVCCompanyGroup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCCompanyGroup tVCCompanyGroup = db.TVCCompanyGroups.Find(id);
            if (tVCCompanyGroup == null)
            {
                return HttpNotFound();
            }
            return View(tVCCompanyGroup);
        }

        // POST: TVCCompanyGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TVCCompanyGroup tVCCompanyGroup = db.TVCCompanyGroups.Find(id);
            db.TVCCompanyGroups.Remove(tVCCompanyGroup);
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
