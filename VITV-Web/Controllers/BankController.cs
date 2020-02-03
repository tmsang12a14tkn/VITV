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
    [RoleAuthorize(Site= "admin")]
    public class BankController : Controller
    {
        private readonly VITVContext context;
        private readonly IBankRepository _bankRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public BankController()
        {
            context = new VITVContext();
            _bankRepository = new BankRepository(context);
        }

        
        public ActionResult Management()
        {
            return View();
        }

        public JsonResult GetBanks()
        {
            var banks = _bankRepository.All.OrderByDescending(h => h.Name).AsEnumerable().Select(spec => new
                                                                                        {
                                                                                            Id = spec.Id,
                                                                                            Name = spec.Name,
                                                                                            Thumbnail = spec.Thumbnail,
                                                                                            IssueDate = String.Format("{0:d/M/yyyy}", spec.IssueDate),
                                                                                            UpdateTime = String.Format("{0:d/M/yyyy}", spec.UpdateTime),
                                                                                            CharterCapital = spec.CharterCapital
                                                                                        }).ToList();
            var total = banks.Count;
            return Json(new { banks, total }, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase thumbnail, BankModel bank)
        {
            if (thumbnail != null && thumbnail.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnail.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/bank";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    thumbnail.SaveAs(filePath);
                }
                bank.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }
            else
            {
                bank.Thumbnail = "/Images/default-avatar.png";
            }

            ModelState.Clear();
            TryValidateModel(bank);

            if (ModelState.IsValid)
            {
                var bankEn = AutoMapper.Mapper.Map<BankModel, Bank>(bank);
                _bankRepository.InsertOrUpdate(bankEn);
                _bankRepository.Save();
                return RedirectToAction("Management");
            }
            return View(bank);
        }

        
        public ActionResult Edit(int id)
        {
            var bank = _bankRepository.Find(id);
            return View(AutoMapper.Mapper.Map<Bank, BankModel>(bank));
        }

        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase thumbnail, BankModel bank)
        {
            if (thumbnail != null && thumbnail.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnail.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/bank";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    thumbnail.SaveAs(filePath);
                }
                bank.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }
            else if (String.IsNullOrEmpty(bank.Thumbnail))
            {
                bank.Thumbnail = "/Images/default-avatar.png";
            }

            ModelState.Clear();
            TryValidateModel(bank);

            if (ModelState.IsValid)
            {
                var bankEn = AutoMapper.Mapper.Map<BankModel, Bank>(bank);
                _bankRepository.InsertOrUpdate(bankEn);
                _bankRepository.Save();
                return RedirectToAction("Management");
            }
            return View(bank);
        }

        
        public ActionResult Delete(int id)
        {
            return PartialView(_bankRepository.Find(id));
        }

        
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string confirmText)
        {
            var bank = _bankRepository.Find(id);
            var success = false;
            string error = "";
            if (bank == null)
            {
                error = "Không tìm thấy ngân hàng";
            }
            else if (confirmText.ToLower() != "đồng ý")
            {
                error = "Chuỗi nhập vào chưa đúng";
            }
            else
            {
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    string filePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, bank.Thumbnail);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _bankRepository.Delete(id);
                _bankRepository.Save();
                success = true;
            }
            return Json(new { success = success, id = id, error = error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bankRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}