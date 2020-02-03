using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;

namespace VITV.Web.Controllers
{
    public class SpecialEventController : Controller
    {
        private readonly VITVContext context;
        private readonly ISpecialEventRepository _specialEventRepository;
        private readonly IVideoRepository _videoRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public SpecialEventController()
        {
            context = new VITVContext();
            _specialEventRepository = new SpecialEventRepository(context);
            _videoRepository = new VideoRepository(context);
        }

        [Authorize(Roles = "Admin")]
        // GET: SpecialEvent
        public ActionResult Management()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public JsonResult GetSpecialEvents()
        {
            var specialevents = _specialEventRepository.All.OrderByDescending(e=>e.EndDate).AsEnumerable()
                                                .Select(spec => new
                                                {
                                                    Id = spec.Id,
                                                    Name = spec.Name,
                                                    BarBkgr = spec.BarBkgr,
                                                    StartDate = String.Format("{0:d/M/yyyy HH:mm:ss}", spec.StartDate),
                                                    EndDate = String.Format("{0:d/M/yyyy HH:mm:ss}", spec.EndDate)
                                                }).ToList();
            var total = specialevents.Count;
            return Json(new { specialevents, total }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SpecialEvent/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase BarBkgrFile, SpecialEventModel specialEvent)
        {
            if (BarBkgrFile != null && BarBkgrFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + BarBkgrFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/backgroundimage";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    BarBkgrFile.SaveAs(filePath);
                }
                specialEvent.BarBkgr = currentDomain + "/" + folder + "/" + fileName;
            }
            else
            {
                specialEvent.BarBkgr = "/Images/default-avatar.png";
            }

            ModelState.Clear();
            TryValidateModel(specialEvent);

            if (ModelState.IsValid)
            {
                var specialEventEn = AutoMapper.Mapper.Map<SpecialEventModel, SpecialEvent>(specialEvent);
                _specialEventRepository.InsertOrUpdate(specialEventEn);
                _specialEventRepository.Save();
                return RedirectToAction("Management");
            }
            return View(specialEvent);
        }

        //
        // GET: /SpecialEvent/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var specialEvent = _specialEventRepository.Find(id);
            return View(AutoMapper.Mapper.Map<SpecialEvent, SpecialEventModel>(specialEvent));
        }

        //
        // POST: /SpecialEvent/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase BarBkgrFile, SpecialEventModel specialEvent)
        {
            if (BarBkgrFile != null && BarBkgrFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + BarBkgrFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/backgroundimage";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    BarBkgrFile.SaveAs(filePath);
                }
                specialEvent.BarBkgr = currentDomain + "/" + folder + "/" + fileName;
            }
            else if (String.IsNullOrEmpty(specialEvent.BarBkgr))
            {
                specialEvent.BarBkgr = "/Images/default-avatar.png";
            }

           
            ModelState.Clear();
            TryValidateModel(specialEvent);

            if (ModelState.IsValid)
            {
                var specialEventEn = AutoMapper.Mapper.Map<SpecialEventModel, SpecialEvent>(specialEvent);
                _specialEventRepository.InsertOrUpdate(specialEventEn);
                _specialEventRepository.Save();
                return RedirectToAction("Management");
            }
            return View(specialEvent);
        }

        //
        // GET: /SpecialEvent/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return PartialView(_specialEventRepository.Find(id));
        }

        //
        // POST: /SpecialEvent/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string confirmText)
        {
            var specialEvent = _specialEventRepository.Find(id);
            var success = false;
            string error = "";
            if (specialEvent == null)
            {
                error = "Không tìm thấy sự kiện";
            }else if(confirmText.ToLower() != "đồng ý")
            {
                error = "Chuỗi nhập vào chưa đúng";
            }
            else
            {
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    string filePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, specialEvent.BarBkgr);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                var video = _videoRepository.GetMany(v => v.SpecialEventId == specialEvent.Id);
                foreach(Video v in video)
                    v.SpecialEventId = null;
                _specialEventRepository.Delete(id);
                _specialEventRepository.Save();
                _videoRepository.Save();
                success = true;
            }
            return Json(new { success = success, id = id, error = error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _specialEventRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}