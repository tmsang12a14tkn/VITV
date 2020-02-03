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
    public class CategoryIntroController : Controller
    {
        private readonly ICategoryIntroRepository _categoryIntroRepository;
        private readonly IVideoCategoryRepository _categoryRepository;
        private readonly IVideoCatGroupRepository _videoCatGroupRepository;
        private readonly IVideoCatTypeRepository _catTypeRepository;
        private readonly IVideoCatGroupRepository _catGroupRepository;
        public CategoryIntroController()
        {
            var context = new VITVContext();
            _categoryIntroRepository = new CategoryIntroRepository(context);
            _categoryRepository = new VideoCategoryRepository(context);
            _videoCatGroupRepository = new VideoCatGroupRepository(context);
            _catTypeRepository = new VideoCatTypeRepository();
            _catGroupRepository = new VideoCatGroupRepository();
        }

        public CategoryIntroController(ICategoryIntroRepository categoryIntroRepository)
        {
            _categoryIntroRepository = categoryIntroRepository;
        }

        public ActionResult Management()
        {
            var model = _categoryIntroRepository.All.GroupBy(i => i.VideoCategory).Select(g => new IntroByCategory
            {
                    Category = g.Key,
                    Intros = g.ToList()
                }).ToList();
            //ViewBag.videocattypes = _catTypeRepository.All.ToList();
            ViewBag.videocatgroups = _catGroupRepository.All.ToList();
            ViewBag.videocategories = _categoryRepository.All.ToList();
            return View(model);
        }
        public ActionResult PartialForCategory(int catId)
        {
            return PartialView("_List", _categoryIntroRepository.GetMany(i => i.VideoCategoryId == catId));
        }
        public ActionResult PartialManagement(int groupid)
        {
            var model = _categoryIntroRepository.All.GroupBy(i => i.VideoCategory).Select(g => new IntroByCategory
            {
                Category = g.Key,
                Intros = g.ToList()
            }).ToList();
            ViewBag.videocatgroups = _catGroupRepository.All.ToList();
            ViewBag.videocategories = _categoryRepository.All.ToList();
            ViewBag.groupid = groupid;
            return PartialView("_ListManagement", model);
        }
        public ActionResult ForCategory(int catId)
        {
            ViewBag.catId = catId;
            return PartialView(_categoryIntroRepository.GetMany(i=>i.VideoCategoryId == catId));
        }
        [RoleAuthorize(Site = "admin")]
        public ActionResult Create(int? catId)
        {
            //ViewBag.VideoCategoryId = new SelectList(_categoryRepository.All.ToList(), "Id", "Name", catId);
            ViewBag.VideoCatGroups = _videoCatGroupRepository.AllIncluding(v => v.VideoCats).OrderBy(v => v.Order).ToList();
            ViewBag.catid = catId;
            return PartialView();
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase thumbnailFile, CategoryIntro categoryIntro)
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
                categoryIntro.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }

            ModelState.Clear();
            TryValidateModel(categoryIntro);

            if (ModelState.IsValid)
            {
                _categoryIntroRepository.InsertOrUpdate(categoryIntro);
                _categoryIntroRepository.Save();
                VideoCategory category = _categoryRepository.Find(categoryIntro.VideoCategoryId);
                return Json(new { success = true, groupid = category.GroupId, catid = category.Id });
            }
            //ViewBag.VideoCategoryId = new SelectList(_categoryRepository.All.ToList(), "Id", "Name");
            ViewBag.VideoCatGroups = _videoCatGroupRepository.AllIncluding(v => v.VideoCats).OrderBy(v => v.Order).ToList();
            var viewStr = this.RenderRazorViewToString("Create", categoryIntro);
            return Json(new { success = false, view = viewStr });
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Edit(int id)
        {
            var categoryIntro = _categoryIntroRepository.Find(id);
            if (categoryIntro != null)
            {
                //ViewBag.VideoCategoryId = new SelectList(_categoryRepository.All.ToList(), "Id", "Name", categoryIntro.VideoCategoryId);
                ViewBag.VideoCatGroups = _videoCatGroupRepository.AllIncluding(v => v.VideoCats).OrderBy(v => v.Order).ToList();
                return PartialView(categoryIntro);
            }
            
            return new HttpNotFoundResult("Không tìm thấy trang yêu cầu");
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase thumbnailFile, CategoryIntro categoryIntro, string returnUrl)
        {
            string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
            var physicalStoragePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain];

            //Thay thế file thumbnail
            if (thumbnailFile != null && thumbnailFile.ContentLength > 0)
            {
                //Xóa file thumbnail cũ
                string oldFilePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, categoryIntro.Thumbnail);
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

                categoryIntro.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }

            ModelState.Clear();
            TryValidateModel(categoryIntro);

            if (ModelState.IsValid)
            {
                var oldVideo = _categoryIntroRepository.Find(categoryIntro.Id);
                //Xóa video cũ
                if (oldVideo != null && oldVideo.Url != categoryIntro.Url)
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

                _categoryIntroRepository.InsertOrUpdate(categoryIntro);
                _categoryIntroRepository.Save();
                VideoCategory category = _categoryRepository.Find(categoryIntro.VideoCategoryId);
                return Json(new { success = true, groupid = category.GroupId, catid = category.Id });
                //return RedirectToLocal(returnUrl);
            }
            //ViewBag.VideoCategoryId = new SelectList(_categoryRepository.All.ToList(), "Id", "Name", categoryIntro.VideoCategoryId);
            ViewBag.VideoCatGroups = _videoCatGroupRepository.AllIncluding(v => v.VideoCats).OrderBy(v => v.Order).ToList();
            var viewStr = this.RenderRazorViewToString("Edit", categoryIntro);
            return Json(new { success = false, view = viewStr });
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Delete(int id)
        {
            return PartialView(_categoryIntroRepository.Find(id));
        }

        [ValidateAntiForgeryToken]
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id, string confirmText)
        {
            var categoryIntro = _categoryIntroRepository.Find(id);
            var success = false;
            int VideoCategoryId = categoryIntro.VideoCategoryId;
            string error = "";
            if (categoryIntro == null)
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
                    string thumbnailPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, categoryIntro.Thumbnail);
                    if (System.IO.File.Exists(thumbnailPath))
                    {
                        System.IO.File.Delete(thumbnailPath);
                    }

                    string videoPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, categoryIntro.Url);

                    if (System.IO.File.Exists(videoPath))
                    {
                        System.IO.File.Delete(videoPath);
                    }
                }
                _categoryIntroRepository.Delete(id);
                _categoryIntroRepository.Save();
                success = true;
            }
            VideoCategory category = _categoryRepository.Find(VideoCategoryId);
            return Json(new { success = success, id = id, error = error, groupid = category.GroupId, catid = category.Id });
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Management", "CategoryIntro");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _categoryIntroRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
