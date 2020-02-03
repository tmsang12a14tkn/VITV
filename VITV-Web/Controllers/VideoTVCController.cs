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
    public class VideoTVCController : Controller
    {
        private readonly IVideoTVCRepository _videoTVCRepository;

        public VideoTVCController()
        {
            var context = new VITVContext();
            _videoTVCRepository = new VideoTVCRepository(context);
        }

        public VideoTVCController(IVideoTVCRepository videoTVCRepository)
        {
            _videoTVCRepository = videoTVCRepository;
        }

        public ActionResult Management()
        {
            return View(_videoTVCRepository.All);
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(_videoTVCRepository.AllProduct.ToList(), "Id", "Name");
            ViewBag.CompanyId = new SelectList(_videoTVCRepository.AllCompany.ToList(), "Id", "Name");
            ViewBag.ProductGroupId = new SelectList(_videoTVCRepository.AllProductGroup.ToList(), "Id", "Name");
            ViewBag.CompanyGroupId = new SelectList(_videoTVCRepository.AllCompanyGroup.ToList(), "Id", "Name");
            return PartialView();
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase thumbnailFile, VideoTVC videoTVC)
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
                videoTVC.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }

            ModelState.Clear();
            TryValidateModel(videoTVC);

            if (ModelState.IsValid)
            {
                _videoTVCRepository.InsertOrUpdate(videoTVC);
                _videoTVCRepository.Save();

                return RedirectToAction("Management");
            }
            ViewBag.ProductId = new SelectList(_videoTVCRepository.AllProduct.ToList(), "Id", "Name");
            ViewBag.CompanyId = new SelectList(_videoTVCRepository.AllCompany.ToList(), "Id", "Name");
            ViewBag.ProductGroupId = new SelectList(_videoTVCRepository.AllProductGroup.ToList(), "Id", "Name");
            ViewBag.CompanyGroupId = new SelectList(_videoTVCRepository.AllCompanyGroup.ToList(), "Id", "Name");
            return View(videoTVC);
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Edit(int id)
        {
            var videoTVC = _videoTVCRepository.Find(id);
            if (videoTVC != null)
            {
                ViewBag.ProductId = new SelectList(_videoTVCRepository.AllProduct.Where(p=>p.CompanyId == videoTVC.Product.CompanyId).ToList(), "Id", "Name", videoTVC.ProductId);
                ViewBag.CompanyId = new SelectList(_videoTVCRepository.AllCompany.ToList(), "Id", "Name", videoTVC.Product.CompanyId);
                ViewBag.ProductGroupId = new SelectList(_videoTVCRepository.AllProductGroup.ToList(), "Id", "Name", videoTVC.ProductGroupId);
                ViewBag.CompanyGroupId = new SelectList(_videoTVCRepository.AllCompanyGroup.ToList(), "Id", "Name", videoTVC.CompanyGroupId);
                return PartialView(videoTVC);
            }

            return new HttpNotFoundResult("Không tìm thấy trang yêu cầu");
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase thumbnailFile, VideoTVC videoTVC, string returnUrl)
        {
            string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
            var physicalStoragePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain];

            //Thay thế file thumbnail
            if (thumbnailFile != null && thumbnailFile.ContentLength > 0)
            {
                //Xóa file thumbnail cũ
                string oldFilePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, videoTVC.Thumbnail);
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

                videoTVC.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }

            ModelState.Clear();
            TryValidateModel(videoTVC);

            if (ModelState.IsValid)
            {
                var oldVideo = _videoTVCRepository.Find(videoTVC.Id);
                //Xóa video cũ
                if (oldVideo != null && oldVideo.Url != videoTVC.Url)
                {
                    string filePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, oldVideo.Url);
                    using (new Impersonator("uploaduser", "", "Upload@@123"))
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }

                _videoTVCRepository.InsertOrUpdate(videoTVC);
                _videoTVCRepository.Save();

                return Json(new { success = true, id = videoTVC.Id });
            }
            ViewBag.ProductId = new SelectList(_videoTVCRepository.AllProduct.Where(p => p.CompanyId == videoTVC.Product.CompanyId).ToList(), "Id", "Name", videoTVC.ProductId);
            ViewBag.CompanyId = new SelectList(_videoTVCRepository.AllCompany.ToList(), "Id", "Name", videoTVC.Product.CompanyId);
            ViewBag.ProductGroupId = new SelectList(_videoTVCRepository.AllProductGroup.ToList(), "Id", "Name", videoTVC.ProductGroupId);
            ViewBag.CompanyGroupId = new SelectList(_videoTVCRepository.AllCompanyGroup.ToList(), "Id", "Name", videoTVC.CompanyGroupId);
            return Json(new{ success = false, view = this.RenderRazorViewToString("Edit",videoTVC)});
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Delete(int id)
        {
            return PartialView(_videoTVCRepository.Find(id));
        }

        [ValidateAntiForgeryToken]
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id, string confirmText)
        {
            var videoTVC = _videoTVCRepository.Find(id);
            var success = false;
            string error = "";
            if (videoTVC == null)
            {
                error = "Không tìm thấy video";
            }
            else if (confirmText.ToLower() != "đồng ý")
            {
                error = "Chuỗi nhập vào chưa đúng";
            }
            else
            {
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    string thumbnailPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, videoTVC.Thumbnail);
                    if (System.IO.File.Exists(thumbnailPath))
                    {
                        System.IO.File.Delete(thumbnailPath);
                    }

                    string videoPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, videoTVC.Url);

                    if (System.IO.File.Exists(videoPath))
                    {
                        System.IO.File.Delete(videoPath);
                    }
                }
                _videoTVCRepository.Delete(id);
                _videoTVCRepository.Save();
                success = true;
            }

            return Json(new { success = success, id = id, error = error });
        }

        public ActionResult PartialDetail()
        {
            List<VideoTVC> tvc = _videoTVCRepository.All.ToList();
            return PartialView(tvc);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Management", "VideoTVC");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _videoTVCRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}