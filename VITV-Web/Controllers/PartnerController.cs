using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VITV.Web.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.Helpers;

namespace VITV.Web.Controllers
{
   [RoleAuthorize(Site="admin")]
    public class PartnerController : Controller
    {
        private readonly IPartnerRepository repository;
        public PartnerController()
        {
            repository = new PartnerRepository();
        }
        // GET: Partner
        public ActionResult Index()
        {
            return View(repository.All.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase photoFile, Partner partner)
        {
            if (photoFile != null && photoFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + photoFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/Partner";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    photoFile.SaveAs(filePath);
                }
                partner.PhotoUrl = currentDomain + "/" + folder + "/" + fileName;
                ModelState.Clear();
                TryValidateModel(partner);
            }
            if (ModelState.IsValid)
            {
                repository.InsertOrUpdate(partner);
                repository.Save();
                return RedirectToAction("Index");
            }
            return View(partner);
        }

        public ActionResult Edit(int? Id)
        {
            if (!Id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(repository.Find(Id.Value));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase photoFile, Partner partner)
        {
            if (photoFile != null && photoFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + photoFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/Partner";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    photoFile.SaveAs(filePath);
                }
                partner.PhotoUrl = currentDomain + "/" + folder + "/" + fileName;
                ModelState.Clear();
                TryValidateModel(partner);
            }
            if (ModelState.IsValid)
            {
                repository.InsertOrUpdate(partner);
                repository.Save();
                return RedirectToAction("Index");
            }
            return View(partner);
        }

        public ActionResult Delete(int? Id)
        {
            if (!Id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(repository.Find(Id.Value));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            var partner = repository.Find(Id);
            if (partner != null)
            {
                string filePath = Server.MapPath(partner.PhotoUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                repository.Delete(Id);
                repository.Save();
            }
            return RedirectToAction("Index", "Partner");
        }
    }
}