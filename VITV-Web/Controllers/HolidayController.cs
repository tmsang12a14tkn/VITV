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
     [RoleAuthorize(Site = "admin")]
    public class HolidayController : Controller
    {
        private readonly VITVContext context;
        private readonly IHolidayRepository _holidayRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public HolidayController()
        {
            context = new VITVContext();
            _holidayRepository = new HolidayRepository(context);
        }

        // GET: Holiday
        public ActionResult Management()
        {
            return View();
        }

       
        public JsonResult GetHolidays()
        {
            var Holidays = _holidayRepository.All.OrderByDescending(h=>h.EndDate).AsEnumerable().Select(spec => new
                                                {
                                                    Id = spec.Id,
                                                    Name = spec.Name,
                                                    LeftFixedBkgr = spec.LeftFixedBkgr,
                                                    RightFixedBkgr = spec.RightFixedBkgr,
                                                    StartDate = String.Format("{0:d/M/yyyy}", spec.StartDate),
                                                    EndDate = String.Format("{0:d/M/yyyy}", spec.EndDate)
                                                }).ToList();
            var total = Holidays.Count;
            return Json(new { Holidays, total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Holiday/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase LeftFixedBkgrFile, HttpPostedFileBase RightFixedBkgrFile, HolidayModel holiday)
        {
            if (LeftFixedBkgrFile != null && LeftFixedBkgrFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + LeftFixedBkgrFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/backgroundimage";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    LeftFixedBkgrFile.SaveAs(filePath);
                }
                holiday.LeftFixedBkgr = currentDomain + "/" + folder + "/" + fileName;
            }
            else
            {
                holiday.LeftFixedBkgr = "/Images/default-avatar.png";
            }

            if (RightFixedBkgrFile != null && RightFixedBkgrFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + RightFixedBkgrFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/backgroundimage";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    RightFixedBkgrFile.SaveAs(filePath);
                }
                holiday.RightFixedBkgr = currentDomain + "/" + folder + "/" + fileName;
            }
            else
            {
                holiday.RightFixedBkgr = "/Images/default-avatar.png";
            }

            ModelState.Clear();
            TryValidateModel(holiday);

            if (ModelState.IsValid)
            {
                var holidayEn = AutoMapper.Mapper.Map<HolidayModel, Holiday>(holiday);
                _holidayRepository.InsertOrUpdate(holidayEn);
                _holidayRepository.Save();
                return RedirectToAction("Management");
            }
            return View(holiday);
        }

        //
        // GET: /Holiday/Edit/5
        public ActionResult Edit(int id)
        {
            var holiday = _holidayRepository.Find(id);
            return View(AutoMapper.Mapper.Map<Holiday, HolidayModel>(holiday));
        }

        //
        // POST: /Holiday/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase LeftFixedBkgrFile, HttpPostedFileBase RightFixedBkgrFile, HolidayModel holiday)
        {
            if (LeftFixedBkgrFile != null && LeftFixedBkgrFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + LeftFixedBkgrFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/backgroundimage";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    LeftFixedBkgrFile.SaveAs(filePath);
                }
                holiday.LeftFixedBkgr = currentDomain + "/" + folder + "/" + fileName;
            }
            else if (String.IsNullOrEmpty(holiday.LeftFixedBkgr))
            {
                holiday.LeftFixedBkgr = "/Images/default-avatar.png";
            }

            if (RightFixedBkgrFile != null && RightFixedBkgrFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + RightFixedBkgrFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/backgroundimage";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    RightFixedBkgrFile.SaveAs(filePath);
                }
                holiday.RightFixedBkgr = currentDomain + "/" + folder + "/" + fileName;
            }
            else if (String.IsNullOrEmpty(holiday.RightFixedBkgr))
            {
                holiday.RightFixedBkgr = "/Images/default-avatar.png";
            }

            ModelState.Clear();
            TryValidateModel(holiday);

            if (ModelState.IsValid)
            {
                var holidayEn = AutoMapper.Mapper.Map<HolidayModel, Holiday>(holiday);
                _holidayRepository.InsertOrUpdate(holidayEn);
                _holidayRepository.Save();
                return RedirectToAction("Management");
            }
            return View(holiday);
        }

        //
        // GET: /Holiday/Delete/5
        public ActionResult Delete(int id)
        {
            return PartialView(_holidayRepository.Find(id));
        }

        //
        // POST: /Holiday/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string confirmText)
        {
            var holiday = _holidayRepository.Find(id);
            var success = false;
            string error = "";
            if (holiday == null)
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
                    string filePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, holiday.LeftFixedBkgr);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    filePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, holiday.RightFixedBkgr);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _holidayRepository.Delete(id);
                _holidayRepository.Save();
                success = true;
            }
            return Json(new { success = success, id = id, error = error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _holidayRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}