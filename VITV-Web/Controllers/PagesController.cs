using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Models;
using Microsoft.AspNet.Identity;

namespace VITV_Web.Controllers
{
    public class PagesController : Controller
    {
        private VITVContext db = new VITVContext();

        [Authorize(Roles = "Admin")]
        [Route("quan-ly-trang")]
        public ActionResult Management()
        {
            var pages = db.SitePages.ToList();
            return View(pages);
        }

        [Authorize(Roles = "Admin")]
        [Route("tao-trang")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Route("tao-trang")]
        public ActionResult Create(SitePage page, HttpPostedFileBase image)
        {
            page.CreatedUserId = User.Identity.GetUserId();

            ModelState.Clear();
            TryValidateModel(page);

            if (ModelState.IsValid)
            {
                try
                {
                  
                    if (string.IsNullOrEmpty(page.Slug))
                        page.Slug = VITV.Web.Helpers.UrlHelper.URLFriendly(page.Title);
                    page.Date = DateTime.Now;
                    db.SitePages.Add(page);
                    db.SaveChanges();
                    return RedirectToAction("Management");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(page);
        }

        [Route("{slug}")]
        public ActionResult Details(string slug = "")
        {
            var page = db.SitePages.FirstOrDefault(p => p.Slug == slug);
            if (page == null)
                return HttpNotFound();
            return View(page);
        }

        // GET: Articles/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
        [Route("sua-trang/{id:int}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var page = db.SitePages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("sua-trang/{id:int}")]
        public ActionResult Edit(SitePage page, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    page.Modified = DateTime.Now;
                    
                    db.Entry(page).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Management");
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return View(page);
        }
        // GET: Pages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SitePage page = db.SitePages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SitePage page = db.SitePages.Find(id);
            db.SitePages.Remove(page);
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
