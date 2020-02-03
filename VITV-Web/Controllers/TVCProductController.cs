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
    public class TVCProductController : Controller
    {
        private VITVContext db = new VITVContext();

        // GET: TVCProduct
        public ActionResult Index()
        {
            var tVCProducts = db.TVCProducts.Include(t => t.Company);
            return View(tVCProducts.ToList());
        }

        // GET: TVCProduct/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCProduct tVCProduct = db.TVCProducts.Find(id);
            if (tVCProduct == null)
            {
                return HttpNotFound();
            }
            return View(tVCProduct);
        }

        // GET: TVCProduct/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.TVCCompanies, "Id", "Name");
            return View();
        }

        // POST: TVCProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CompanyId")] TVCProduct tVCProduct)
        {
            if (ModelState.IsValid)
            {
                db.TVCProducts.Add(tVCProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.TVCCompanies, "Id", "Name", tVCProduct.CompanyId);
            return View(tVCProduct);
        }

        // GET: TVCProduct/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCProduct tVCProduct = db.TVCProducts.Find(id);
            if (tVCProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.TVCCompanies, "Id", "Name", tVCProduct.CompanyId);
            return View(tVCProduct);
        }

        // POST: TVCProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CompanyId")] TVCProduct tVCProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tVCProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.TVCCompanies, "Id", "Name", tVCProduct.CompanyId);
            return View(tVCProduct);
        }

        // GET: TVCProduct/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVCProduct tVCProduct = db.TVCProducts.Find(id);
            if (tVCProduct == null)
            {
                return HttpNotFound();
            }
            return View(tVCProduct);
        }

        // POST: TVCProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TVCProduct tVCProduct = db.TVCProducts.Find(id);
            db.TVCProducts.Remove(tVCProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult FromCompany(int id)
        {
            var products = db.TVCProducts.Where(p => p.CompanyId == id).Select(p=>new {value = p.Id, text = p.Name}).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
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
