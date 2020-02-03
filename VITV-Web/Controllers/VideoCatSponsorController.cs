using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;
using VITV.Data.DAL;
using VITV.Web.Helpers;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using System.Web.Routing;
using System.IO;
using System.Collections.Generic;

namespace VITV_Web.Controllers
{
    public class VideoCatSponsorController : Controller
    {
        private readonly IVideoCatSponsorRepository _videoCatSponsorRepository;

        public VideoCatSponsorController()
        {
            var context = new VITVContext();
            _videoCatSponsorRepository = new VideoCatSponsorRepository(context);
        }

        public VideoCatSponsorController(IVideoCatSponsorRepository videoCatSponsorRepository)
        {
            _videoCatSponsorRepository = videoCatSponsorRepository;
        }

        public ActionResult Management()
        {
            return View(_videoCatSponsorRepository.All);
        }
        
        [RoleAuthorize(Site = "admin")]
        public ActionResult Create()
        {
            return PartialView();
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase thumbnailFile, VideoCategorySponsor vCatSponsor)
        {
            if (thumbnailFile != null && thumbnailFile.ContentLength > 0)
            {
                string extension = Path.GetExtension(thumbnailFile.FileName);
                string timeUpload = DateTime.Now.ToString("yyyyMMddHHmmss");
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnailFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/VideoThumbnail";
                string physicalStoragePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain];
                string filePath = physicalStoragePath + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    thumbnailFile.SaveAs(filePath);
                }
                vCatSponsor.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }

            ModelState.Clear();
            TryValidateModel(vCatSponsor);

            if (ModelState.IsValid)
            {
                _videoCatSponsorRepository.InsertOrUpdate(vCatSponsor);
                _videoCatSponsorRepository.Save();

                return RedirectToAction("Management");
            }
            
            return View(vCatSponsor);
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Edit(int id)
        {
            var vCatSponsor = _videoCatSponsorRepository.Find(id);
            if (vCatSponsor != null)
            {
                return PartialView(vCatSponsor);
            }

            return new HttpNotFoundResult("Không tìm thấy trang yêu cầu");
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase thumbnailFile, VideoCategorySponsor vCatSponsor, string returnUrl)
        {
            string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
            var physicalStoragePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain];

            //Thay thế file thumbnail
            if (thumbnailFile != null && thumbnailFile.ContentLength > 0)
            {
                //Xóa file thumbnail cũ
                string oldFilePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, vCatSponsor.Thumbnail);
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                string extension = Path.GetExtension(thumbnailFile.FileName);
                string timeUpload = DateTime.Now.ToString("yyyyMMddHHmmss");
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnailFile.FileName;
                string folder = "UploadedImages/VideoThumbnail";
                string filePath = physicalStoragePath + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    thumbnailFile.SaveAs(filePath);
                }

                vCatSponsor.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }

            ModelState.Clear();
            TryValidateModel(vCatSponsor);

            if (ModelState.IsValid)
            {
                _videoCatSponsorRepository.InsertOrUpdate(vCatSponsor);
                _videoCatSponsorRepository.Save();

                return Json(new { success = true, id = vCatSponsor.Id });
            }
            
            return Json(new { success = false, view = this.RenderRazorViewToString("Edit", vCatSponsor) });
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Delete(int id)
        {
            return PartialView(_videoCatSponsorRepository.Find(id));
        }

        [ValidateAntiForgeryToken]
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string confirmText)
        {
            var vCatSponsor = _videoCatSponsorRepository.Find(id);
            var success = false;
            string error = "";
            if (vCatSponsor == null)
            {
                error = "Không tìm thấy sponsor";
            }
            else if (confirmText.ToLower() != "đồng ý")
            {
                error = "Chuỗi nhập vào chưa đúng";
            }
            else
            {
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    string thumbnailPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, vCatSponsor.Thumbnail);
                    if (System.IO.File.Exists(thumbnailPath))
                    {
                        System.IO.File.Delete(thumbnailPath);
                    }
                }
                _videoCatSponsorRepository.Delete(id);
                _videoCatSponsorRepository.Save();
                success = true;
            }

            return Json(new { success = success, id = id, error = error });
        }

        public ActionResult PartialDetail()
        {
            List<VideoCategorySponsor> sponsors = _videoCatSponsorRepository.All.ToList();
            return PartialView(sponsors);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Management", "VideoCatSponsor");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _videoCatSponsorRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
