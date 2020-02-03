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
    public class TVCCompanyController : Controller
    {
        private VITVContext db = new VITVContext();

        // GET: TVCCompany
        public ActionResult Index()
        {
            return View(db.TVCCompanies.ToList());
        }

        // GET: TVCCompany/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCCompany tVCCompany = db.TVCCompanies.Find(id);
            if (tVCCompany == null)
            {
                return HttpNotFound();
            }
            return View(tVCCompany);
        }

        // GET: TVCCompany/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TVCCompany/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] TVCCompany tVCCompany)
        {
            if (ModelState.IsValid)
            {
                db.TVCCompanies.Add(tVCCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tVCCompany);
        }

        // GET: TVCCompany/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCCompany tVCCompany = db.TVCCompanies.Find(id);
            if (tVCCompany == null)
            {
                return HttpNotFound();
            }
            return View(tVCCompany);
        }

        // POST: TVCCompany/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] TVCCompany tVCCompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tVCCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tVCCompany);
        }

        // GET: TVCCompany/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCCompany tVCCompany = db.TVCCompanies.Find(id);
            if (tVCCompany == null)
            {
                return HttpNotFound();
            }
            return View(tVCCompany);
        }

        // POST: TVCCompany/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TVCCompany tVCCompany = db.TVCCompanies.Find(id);
            db.TVCCompanies.Remove(tVCCompany);
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
