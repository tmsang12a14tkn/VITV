using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels.Portal;
using VITV.Web.Helpers;
namespace VITV.Web.Controllers
{
    [RoleAuthorize(Site = "admin")]
    public class VideoCatGroupsController : Controller
    {
        private readonly IVideoCatGroupRepository _catGroupRepository;
        private readonly IVideoCatTypeRepository _catTypeRepository;
        private readonly IVideoCategoryRepository _videoCategoryRepository;
        private readonly IProgramScheduleRepository _programScheduleRepository;
        private readonly IVideoRepository _videoRepository;


        public VideoCatGroupsController()
        {
            _catGroupRepository = new VideoCatGroupRepository();
            _videoCategoryRepository = new VideoCategoryRepository();
            _catTypeRepository = new VideoCatTypeRepository();
            _programScheduleRepository = new ProgramScheduleRepository();
            _videoRepository = new VideoRepository();
        }
        
        public ActionResult Index(int type = 0)
        {
            if(type == 0)
                ViewBag.videocattypes = _catTypeRepository.All.ToList();
            else
                ViewBag.videocatgroups = _catGroupRepository.All.ToList();
            ViewBag.videocategories = _videoCategoryRepository.All.ToList();
            ViewBag.programschedules = _programScheduleRepository.All.ToList();
            return View(type);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoCatGroup videoCatGroup = _catGroupRepository.Find(id.Value);
            if (videoCatGroup == null)
            {
                return HttpNotFound();
            }
            return View(videoCatGroup);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Order")] VideoCatGroup videoCatGroup)
        {
            if (ModelState.IsValid)
            {
                videoCatGroup.UniqueTitle = VITV.Web.Helpers.UrlHelper.URLFriendly(videoCatGroup.Name);
                _catGroupRepository.InsertOrUpdate(videoCatGroup);
                _catGroupRepository.Save();
                return RedirectToAction("Index");
            }

            return View(videoCatGroup);
        }

        [HttpPost]
        public ActionResult Edit(int vcgid, string namevcg, int type)
        {
            if(!User.IsInRole("Admin"))
            {
                return Json(new { success = 0, text = "Không có quyền chỉnh sửa !" });
            }
            if(type == 1)
            {
                var vcg = _catGroupRepository.Find(vcgid);
                if (vcg != null)
                {
                    vcg.Name = namevcg;
                    _catGroupRepository.InsertOrUpdate(vcg);
                    _catGroupRepository.Save();
                    return Json(new { success = 1, text = "Đã cập nhật thành công", vcgid = vcgid, namevcg = namevcg, type = type });
                }
                return Json(new { success = 2, text = "Không tồn tại nhóm chuyên mục này !" });
            }else
            {
                var vcg = _catTypeRepository.Find(vcgid);
                if (vcg != null)
                {
                    vcg.Name = namevcg;
                    _catTypeRepository.InsertOrUpdate(vcg);
                    _catTypeRepository.Save();
                    return Json(new { success = 1, text = "Đã cập nhật thành công", vcgid = vcgid, namevcg = namevcg, type = type });
                }
                return Json(new { success = 2, text = "Không tồn tại thể loại chương trình này !" });
            }
            
        }
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    VideoCatGroup videoCatGroup =_catGroupRepository.Find(id.Value);
        //    if (videoCatGroup == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(videoCatGroup);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,Order")] VideoCatGroup videoCatGroup)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        videoCatGroup.UniqueTitle = VITV.Web.Helpers.UrlHelper.URLFriendly(videoCatGroup.Name);
        //        _catGroupRepository.InsertOrUpdate(videoCatGroup);
        //        _catGroupRepository.Save();
        //        return RedirectToAction("Index");
        //    }
        //    return View(videoCatGroup);
        //}

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoCatGroup videoCatGroup =_catGroupRepository.Find(id.Value);
            if (videoCatGroup == null)
            {
                return HttpNotFound();
            }
            return View(videoCatGroup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _catGroupRepository.Delete(id);
            _catGroupRepository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult FirstWeekAndAllViewVideoCatPopover(int catId, int year)
        {
            var videoReports = _videoRepository.GetMany(v => v.CategoryId == catId && v.DisplayTime.Year == year).AsEnumerable()
            .Select(v => new CompareVideoItemModel()
            {
                VideoId = v.Id,
                DisplayTime = v.DisplayTime
            }).ToList();

            foreach (var vd in videoReports)
            {
                var firstWeekReport = _videoRepository.GetFirstWeekReport(vd.VideoId, vd.DisplayTime, null);
                vd.FirstWeek = firstWeekReport;

                var alltimeReport = _videoRepository.GetAllTimeReport(vd.VideoId);
                vd.AllTime = alltimeReport;

            }

            var model = new CompareVideoModel
            {
                FirstWeekView = videoReports.Count() == 0 ? 0 : videoReports.Sum(x => x.FirstWeek.PageView),
                AllTimeView = videoReports.Count() == 0 ? 0 : videoReports.Sum(v => v.AllTime.PageView),
                FirstWeekHighest = videoReports.Count() == 0 ? 0 : videoReports.Max(m => m.FirstWeek.Highest),
                FirstWeekLowest = videoReports.Count() == 0 ? 0 : videoReports.Min(m => m.FirstWeek.Lowest),
                AllTimeHighest = videoReports.Count() == 0 ? 0 : videoReports.Max(m => m.AllTime.Highest),
                AllTimeLowest = videoReports.Count() == 0 ? 0 : videoReports.Max(m => m.AllTime.Lowest),
            };
            return PartialView("_FirstWeekAndAllViewVideoCatPopover", model);
        }

        [HttpPost]
        public ActionResult ChangeOrder(int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var catGroup = _catGroupRepository.Find(id);
                if (catGroup != null)
                {
                    catGroup.Order = i;
                    _catGroupRepository.InsertOrUpdate(catGroup);
                }
            }
            _catGroupRepository.Save();
            return Json(new { text = "Đã thay đổi thứ thự thành công ." });
        }

        public ActionResult ModalAll()
        {
            var videocattypes = _catTypeRepository.All.ToList();
            return PartialView(videocattypes);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _programScheduleRepository.Dispose();
                _catGroupRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
