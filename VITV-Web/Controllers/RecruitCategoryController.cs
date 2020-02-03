using System;
using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.Helpers;

namespace VITV.Web.Controllers
{
    public class RecruitCategoryController : Controller
    {
        private readonly IRecruitCategoryRepository _recruitcategoryRepository;
        private readonly IRecruitExtraInfoRepository _recruitExtraInfoRepository;
        private readonly ICityRepository _cityRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public RecruitCategoryController()
        {
            var context = new VITVContext();
            _recruitcategoryRepository = new RecruitCategoryRepository(context);
            _recruitExtraInfoRepository = new RecruitExtraInfoRepository(context);
            _cityRepository = new CityRepository(context);
        }

        public RecruitCategoryController(IRecruitCategoryRepository recruitcategoryRepository, IRecruitExtraInfoRepository recruitExtraInfoRepository, ICityRepository cityRepository)
        {
            _recruitcategoryRepository = recruitcategoryRepository;
            _recruitExtraInfoRepository = recruitExtraInfoRepository;
            _cityRepository = cityRepository;
        }

        [RoleAuthorize(Site = "admin")]
        // GET: RecruitCategories
        public ActionResult Index()
        {
            ViewBag.RecruitRule = _recruitExtraInfoRepository.Find(1);
            return View(_cityRepository.GetCityForRecruit());
        }

        [RoleAuthorize(Site = "admin")]
        public ViewResult Management()
        {
            return View();
        }

        [RoleAuthorize(Site = "admin")]
        public JsonResult GetRecruitCategories()
        {
            var cRes = _recruitcategoryRepository.All.Select(cre => new { Id = cre.Id, Name = cre.Name, Description = cre.Description }).ToList();
            var total = cRes.Count;
            return Json(new { cRes, total }, JsonRequestBehavior.AllowGet);
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost]
        public ActionResult Create(RecruitCategory recruitCategory)
        {
            if (ModelState.IsValid)
            {
                _recruitcategoryRepository.InsertOrUpdate(recruitCategory);
                _recruitcategoryRepository.Save();
                return RedirectToAction("Management");
            }

            return View();
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recruitCat = _recruitcategoryRepository.Find(id.Value);
            if (recruitCat == null)
            {
                return HttpNotFound();
            }
            return View(recruitCat);
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost]
        public ActionResult Edit(RecruitCategory recruitCategory)
        {
            if (ModelState.IsValid)
            {
                _recruitcategoryRepository.InsertOrUpdate(recruitCategory);
                _recruitcategoryRepository.Save();
                return RedirectToAction("Management");
            }
            return View(recruitCategory);
        }

        // GET: RecruitCategories/Delete/5
        [RoleAuthorize(Site = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recruitCat = _recruitcategoryRepository.Find(id.Value);
            if (recruitCat == null)
            {
                return HttpNotFound();
            }
            return View(recruitCat);
        }

        // POST: RecruitCategories/Delete/5
      [RoleAuthorize(Site = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _recruitcategoryRepository.Delete(id);
            _recruitcategoryRepository.Save();

            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _recruitcategoryRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
