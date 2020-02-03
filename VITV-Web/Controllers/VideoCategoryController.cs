using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.DAL;
using VITV.Web.Helpers;
using System.Web.Mvc;

namespace VITV.Web.Controllers
{
    public class VideoCategoryController : Controller
    {
        private readonly IVideoCategoryRepository _videocategoryRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IVideoCatGroupRepository _catGroupRepository;
        private readonly IVideoCatTypeRepository _catTypeRepository;
        private readonly ICategoryIntroRepository _catIntroRepository;
        private readonly IVideoTVCRepository _tvcRepository;
        private readonly IPageGroupRepository _pageGroupRepository;
        private readonly IVideoCatSponsorRepository _videoCatSponsorRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public VideoCategoryController()
        {
            var context = new VITVContext();
            _videocategoryRepository = new VideoCategoryRepository(context);
            _videoRepository = new VideoRepository(context);
            _catGroupRepository = new VideoCatGroupRepository(context);
            _catTypeRepository = new VideoCatTypeRepository(context);
            _catIntroRepository = new CategoryIntroRepository(context);
            _tvcRepository = new VideoTVCRepository(context);
            _pageGroupRepository = new PageGroupRepository(context);
            _videoCatSponsorRepository = new VideoCatSponsorRepository(context);
        }

        public VideoCategoryController(IVideoCategoryRepository videocategoryRepository, IVideoCatSponsorRepository videoCatSponsorRepository, IPageGroupRepository pageGroupRepository, IVideoRepository videoRepository, IVideoCatGroupRepository catGroupRepository, ICategoryIntroRepository catIntroRepository)
        {
            _videocategoryRepository = videocategoryRepository;
            _videoRepository = videoRepository;
            _catGroupRepository = catGroupRepository;
            _catIntroRepository = catIntroRepository;
            _pageGroupRepository = pageGroupRepository;
            _videoCatSponsorRepository = videoCatSponsorRepository;
        }

        //
        // GET: /VideoCategory/
        public ViewResult Index()
        {
            ViewBag.CatGroups = _catGroupRepository.All.ToList();
            return View(_videocategoryRepository.GetMany(vc => vc.TypeId != null, vc => vc.Order));
        }

        public ViewResult Details(string title)
        {
            int curType = 0;
            var ortherCat = new VideoCategory();
            if (title.StartsWith("into-") || title.StartsWith("xay-dung-") || title.StartsWith("xuat-nhap-khau-"))
            {
                curType = 1;
                if (!title.EndsWith("-cuoi-tuan"))
                    ortherCat = _videocategoryRepository.All.Where(v => v.UniqueTitle.StartsWith(title) && v.UniqueTitle.EndsWith("-cuoi-tuan")).First();
                else
                {
                    var _otitle = title.Replace("-cuoi-tuan", "");
                    ortherCat = _videocategoryRepository.All.Where(v => v.UniqueTitle == _otitle).First();
                }
            }

            ViewBag.curType = curType;
            ViewBag.ortherCat = ortherCat;

            return View(_videocategoryRepository.Find(title));
        }

        public PartialViewResult Videos(int catId, int? month, int? year, string page = "1")
        {
            var videosQuery = _videoRepository.GetMany(v => v.CategoryId == catId);
            if (month.HasValue && month > 0)
            {
                videosQuery = videosQuery.Where(v => v.DisplayTime.Month == month);
            }

            if (year.HasValue && year > 0)
            {
                videosQuery = videosQuery.Where(v => v.DisplayTime.Year == year);
            }

            const int noResult = 8;
            var pageCount = (int)Math.Ceiling((double)videosQuery.Count() / noResult);
            int iPage;
            if (int.TryParse(page, out iPage))
            {
                videosQuery = videosQuery.Skip((iPage - 1) * noResult).Take(noResult);
            }

            var videos = videosQuery.Where(v => v.IsPublished && v.PublishedTime <= DateTime.Now).OrderByDescending(v => v.DisplayTime).ToList();

            ViewBag.catId = catId;
            ViewBag.Page = iPage;
            ViewBag.PageCount = pageCount;
            ViewBag.month = month;
            ViewBag.year = year;
            return PartialView("_Videos", videos);
        }

        public PartialViewResult GetVideoByCategory(int cat, int grp)
        {
            var now = DateTime.Now;
            List<Video> lstVideo = null;
            if (cat == 0)
            {
                var lstVidCat = _videocategoryRepository.GetMany(v => v.GroupId == grp, v => v.Order);
                var lstVidCatId = lstVidCat.Select(v => v.Id).ToList();
                lstVideo = _videoRepository.GetMany(v => lstVidCatId.Contains(v.CategoryId)).GroupBy(v => v.CategoryId)
                                            .Select(g => g.OrderByDescending(v => v.DisplayTime).FirstOrDefault())
                                            .OrderBy(v => v.Category.Order).ToList();
            }
            else
                lstVideo = _videoRepository.GetMany(v => v.CategoryId == cat && v.DisplayTime < now).OrderByDescending(v => v.DisplayTime).Take(12).ToList();

            return PartialView("~/Views/Home/_ListVideo.cshtml", lstVideo);
        }
        
        [RoleAuthorize(Site = "admin")]
        public ActionResult Create(int? groupId, int? typeId)
        {
            ViewBag.PossibleGroups = _catGroupRepository.All.ToList();
            ViewBag.videocattypes = _catTypeRepository.All.ToList();
            ViewBag.catIntro = _catIntroRepository.All.ToList();
            ViewBag.pageGroups = _pageGroupRepository.All.OrderBy(pg => pg.Order).ToList();
            var model = new VideoCategory
            {
                GroupId = groupId,
                TypeId = typeId
            };
            return View(model);
        }
        
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase profileFile, VideoCategory videocategory)
        {
            videocategory.UniqueTitle = VITV.Web.Helpers.UrlHelper.URLFriendly(videocategory.Name);
            if (profileFile != null && profileFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + profileFile.FileName;
                string mFileName = "Mobile_" + fileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/VideoCategory";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;
                string mFilePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + mFileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    profileFile.SaveAs(filePath);
                    ImageHelper.ScaleBySize(filePath, mFilePath, 250);
                }
                videocategory.ProfilePhoto = currentDomain + "/" + folder + "/" + fileName;
                videocategory.MobileProfilePhoto = currentDomain + "/" + folder + "/" + mFileName;
            }

            ModelState.Clear();
            TryValidateModel(videocategory);

            if (ModelState.IsValid)
            {
                if (videocategory.GroupId != null)
                    videocategory.Name2 = videocategory.Name;

                //if (videocategory.CategoryIntroId == null || videocategory.CategoryIntroId == 0)
                //    videocategory.CategoryIntroId = null;

                _videocategoryRepository.InsertOrUpdate(videocategory);
                _videocategoryRepository.Save();
                //return RedirectToAction("Details", "VideoCatGroups", new { id = videocategory.GroupId });
                return RedirectToAction("Index", "VideoCatGroups");
            }

            ViewBag.PossibleGroups = _catGroupRepository.All.ToList();
            ViewBag.videocattypes = _catTypeRepository.All.ToList();
            ViewBag.catIntro = _catIntroRepository.All.ToList();
            ViewBag.pageGroups = _pageGroupRepository.All.OrderBy(pg => pg.Order).ToList();
            return View(videocategory);
        }
        
        [RoleAuthorize(Site = "admin")]
        public ActionResult Edit(int id)
        {
            ViewBag.PossibleGroups = _catGroupRepository.All.ToList();
            ViewBag.videocattypes = _catTypeRepository.All.ToList();
            ViewBag.catIntro = _catIntroRepository.GetMany(i => i.VideoCategoryId == id).ToList();
            ViewBag.tvcList = _tvcRepository.All.ToList();
            ViewBag.sponsorList = _videoCatSponsorRepository.All.ToList();
            ViewBag.pageGroups = _pageGroupRepository.All.OrderBy(pg=>pg.Order).ToList();

            var videocat = _videocategoryRepository.Find(id);
           
            return View(videocat);
        }
        
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase profileFile, VideoCategory videocategory)
        {
            try
            {
                videocategory.UniqueTitle = VITV.Web.Helpers.UrlHelper.URLFriendly(videocategory.Name);
                if (profileFile != null && profileFile.ContentLength > 0)
                {
                    //Xóa file ảnh profile
                    string oldFilePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, videocategory.ProfilePhoto);
                    string oldMobileFilePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, videocategory.MobileProfilePhoto);
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);

                    if (System.IO.File.Exists(oldMobileFilePath))
                        System.IO.File.Delete(oldMobileFilePath);

                    if (profileFile != null && profileFile.ContentLength > 0)
                    {
                        string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + profileFile.FileName;
                        string mFileName = "Mobile_" + fileName;
                        string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                        string folder = "UploadedImages/VideoCategory";
                        string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;
                        string mFilePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + mFileName;

                        using (new Impersonator("uploaduser", "", "Upload@@123"))
                        {
                            profileFile.SaveAs(filePath);
                            ImageHelper.ScaleBySize(filePath, mFilePath, 250);
                        }
                        videocategory.ProfilePhoto = currentDomain + "/" + folder + "/" + fileName;
                        videocategory.MobileProfilePhoto = currentDomain + "/" + folder + "/" + mFileName;
                    }
                }

                ModelState.Clear();
                TryValidateModel(videocategory);

                if (ModelState.IsValid)
                {
                    if (videocategory.GroupId != null)
                        videocategory.Name2 = videocategory.Name;

                    //if (videocategory.CategoryIntroId == null || videocategory.CategoryIntroId == 0)
                    //    videocategory.CategoryIntroId = null;

                    _videocategoryRepository.InsertOrUpdate(videocategory);
                    _videocategoryRepository.Save();
                    //return RedirectToAction("Details", "VideoCatGroups", new { id = videocategory.GroupId });
                    return RedirectToAction("Index", "VideoCatGroups");
                }

                ViewBag.PossibleGroups = _catGroupRepository.All.ToList();
                ViewBag.videocattypes = _catTypeRepository.All.ToList();
                ViewBag.catIntro = _catIntroRepository.GetMany(i=>i.VideoCategoryId == videocategory.Id).ToList();
                ViewBag.pageGroups = _pageGroupRepository.All.OrderBy(pg => pg.Order).ToList();
                return View(videocategory);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [HttpPost]
        public ActionResult EditTvc(int[] tvcList, int catId)
        {
            var cat = _videocategoryRepository.Find(catId);
            cat.VideoTVCs.Clear();
            if (tvcList != null)
            {
                for (int i = 0; i < tvcList.Length; i++)
                {
                    var tvc = _tvcRepository.Find(tvcList[i]);
                    cat.VideoTVCs.Add(tvc);
                }
            }
            _videocategoryRepository.Save();
            return RedirectToAction("Edit", new { id = catId });
        }

        [HttpPost]
        public ActionResult EditSponsor(int[] sponsorList, int catId)
        {
            var cat = _videocategoryRepository.Find(catId);
            cat.Sponsors.Clear();
            if (sponsorList != null)
            {
                for (int i = 0; i < sponsorList.Length; i++)
                {
                    var spn = _videoCatSponsorRepository.Find(sponsorList[i]);
                    cat.Sponsors.Add(spn);
                }
            }
            _videocategoryRepository.Save();
            return RedirectToAction("Edit", new { id = catId });
        }

        [HttpPost]
        public ActionResult EditIntro(int introId, int catId, bool select)
        {
            var cat = _videocategoryRepository.Find(catId);
            var intro = cat.Intros.FirstOrDefault(i => i.Id == introId);
            if(intro == null)
            {
                return Json(new { success = false });
            }
            else
            {
                if (select == true)
                {
                    cat.Intros.ToList().ForEach(i => i.Selected = false);   
                }
                intro.Selected = select;
                
                _videocategoryRepository.Save();
                return Json(new { success = true });
            }
        }

        [HttpPost]
        public ActionResult EditHasTranscript(int catId, bool value)
        {
            var cat = _videocategoryRepository.Find(catId);
            if(cat != null)
            {
                cat.HasTranscript = value;
                _videocategoryRepository.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult EditHasYoutube(int catId, bool value)
        {
            var cat = _videocategoryRepository.Find(catId);
            if (cat != null)
            {
                cat.HasYoutube = value;
                _videocategoryRepository.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult EditHasAds(int catId, bool value)
        {
            var cat = _videocategoryRepository.Find(catId);
            if (cat != null)
            {
                cat.HasAds = value;
                _videocategoryRepository.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        
        [HttpPost]
        public ActionResult ChangeOrder(int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var videoCat = _videocategoryRepository.Find(id);
                if (videoCat != null)
                {
                    videoCat.Order = i;
                    _videocategoryRepository.InsertOrUpdate(videoCat);
                }
            }
            _videocategoryRepository.Save();
            return Json(new { text = "Đã thay đổi thứ thự thành công ." });
        }
        
        [RoleAuthorize(Site = "admin")]
        public ActionResult Delete(int id)
        {
            return View(_videocategoryRepository.Find(id));
        }
        
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var videoCat = _videocategoryRepository.Find(id);
            var groupId = 0;
            if (videoCat != null)
            {
                groupId = videoCat.GroupId.Value;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    string filePath = Helpers.UrlHelper.GetPhysicalPath(this, videoCat.ProfilePhoto);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    
                }
                
                _videocategoryRepository.Delete(id);
                _videocategoryRepository.Save();
            }
        
            return RedirectToAction("Index", "VideoCatGroups");
        }        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _videocategoryRepository.Dispose();
                _videoRepository.Dispose();
                _pageGroupRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

