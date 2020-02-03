using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Web.Helpers;
using VITV.Data.Helpers;

namespace VITV.Web.Controllers
{
    public class RecruitController : Controller
    {
        private readonly IRecruitRepository _recruitRepository;
        private readonly IRecruitCategoryRepository _recruitCategoryRepository;
        private readonly ICityRepository _cityRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public RecruitController()
        {
            var context = new VITVContext();
            _recruitRepository = new RecruitRepository(context);
            _recruitCategoryRepository = new RecruitCategoryRepository(context);
            _cityRepository = new CityRepository(context);
        }

        public RecruitController(IRecruitRepository recruitRepository, IRecruitCategoryRepository recruitCategoryRepository, ICityRepository cityRepository)
        {
            _recruitRepository = recruitRepository;
            _recruitCategoryRepository = recruitCategoryRepository;
            _cityRepository = cityRepository;
        }

        [RoleAuthorize(Site="admin")]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recruits = _recruitRepository.GetMany(r => r.CityId == id.Value || r.CityId == 3);
            if (recruits == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Views/Recruit/_Index.cshtml", recruits);
        }

        // GET: Recruits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recruit = _recruitRepository.Find(id.Value);
            if (recruit == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Views/Recruit/_Details.cshtml", recruit);
        }

        [RoleAuthorize(Site="admin")]
        public ViewResult Management()
        {
            return View();
        }

        [RoleAuthorize(Site="admin")]
        public JsonResult GetRecruits()
        {
            var recruits = _recruitRepository.All.Select(re => new { Id = re.Id, CategoryName = re.Category.Name, JobPosition = re.JobPosition, DateExpire = re.DateExpire }).ToList();
            var total = recruits.Count;
            return Json(new { recruits, total }, JsonRequestBehavior.AllowGet);
        }

        [RoleAuthorize(Site="admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_recruitCategoryRepository.All, "Id", "Name");
            ViewBag.CityId = new SelectList(_cityRepository.All, "Id", "Name");
            return View();
        }

        [RoleAuthorize(Site="admin")]
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase thumbnail, Recruit recruit)
        {
            if (thumbnail != null && thumbnail.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnail.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/Partner";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    thumbnail.SaveAs(filePath);
                }
                recruit.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }
            else
            {
                recruit.Thumbnail = "/Images/NoImage.png";
            }
            ModelState.Clear();
            TryValidateModel(recruit);

            if (ModelState.IsValid)
            {
                _recruitRepository.InsertOrUpdate(recruit);
                _recruitRepository.Save();
                return RedirectToAction("Management");
            }
            ViewBag.CategoryId = new SelectList(_recruitCategoryRepository.All, "Id", "Name");
            ViewBag.CityId = new SelectList(_cityRepository.All, "Id", "Name");
            return View();
        }

        [RoleAuthorize(Site="admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recruit = _recruitRepository.Find(id.Value);
            if (recruit == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_recruitCategoryRepository.All, "Id", "Name", recruit.CategoryId);
            ViewBag.CityId = new SelectList(_cityRepository.All, "Id", "Name", recruit.CityId);
            return View(recruit);
        }

        [RoleAuthorize(Site="admin")]
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase thumbnail, Recruit recruit)
        {
            if (thumbnail != null && thumbnail.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnail.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/Partner";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    thumbnail.SaveAs(filePath);
                }
                recruit.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }
            else if (String.IsNullOrEmpty(recruit.Thumbnail))
            {
                recruit.Thumbnail = "/Images/NoImage.png";
            }
            ModelState.Clear();
            TryValidateModel(recruit);

            if (ModelState.IsValid)
            {
                _recruitRepository.InsertOrUpdate(recruit);
                _recruitRepository.Save();
                return RedirectToAction("Management");
            }
            ViewBag.CategoryId = new SelectList(_recruitCategoryRepository.All, "Id", "Name", recruit.CategoryId);
            ViewBag.CityId = new SelectList(_cityRepository.All, "Id", "Name", recruit.CityId);
            return View(recruit);
        }

        [RoleAuthorize(Site="admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recruit = _recruitRepository.Find(id.Value);
            if (recruit == null)
            {
                return HttpNotFound();
            }
            return View(recruit);
        }

        [RoleAuthorize(Site="admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var recruit = _recruitRepository.Find(id);
            if (recruit != null)
            {
                string filePath = Server.MapPath(recruit.Thumbnail);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _recruitRepository.Delete(id);
            _recruitRepository.Save();

            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _recruitRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
