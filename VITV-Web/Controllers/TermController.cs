using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Web.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Web.ViewModels;
using VITV.Data.Helpers;

namespace VITV.Web.Controllers
{
    [RoleAuthorize(Site = "admin")]
    public class TermController : Controller
    {
        private readonly VITVContext context;
        private readonly ITermRepository _termRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public TermController()
        {
            context = new VITVContext();
            _termRepository = new TermRepository(context);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Management()
        {
            return View();
        }

        public JsonResult GetTerms()
        {
            var terms = _termRepository.All.OrderByDescending(h => h.MonthValue).ToList();
            var total = terms.Count;
            return Json(new { terms, total }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Term term)
        {
            if (ModelState.IsValid)
            {
                _termRepository.InsertOrUpdate(term);
                _termRepository.Save();
                return RedirectToAction("Management");
            }
            return View(term);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var term = _termRepository.Find(id);
            return View(term);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Term term)
        {
            if (ModelState.IsValid)
            {
                _termRepository.InsertOrUpdate(term);
                _termRepository.Save();
                return RedirectToAction("Management");
            }
            return View(term);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return PartialView(_termRepository.Find(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string confirmText)
        {
            var term = _termRepository.Find(id);
            var success = false;
            string error = "";
            if (term == null)
            {
                error = "Không tìm thấy kỳ hạn";
            }
            else if (confirmText.ToLower() != "đồng ý")
            {
                error = "Chuỗi nhập vào chưa đúng";
            }
            else
            {
                _termRepository.Delete(id);
                _termRepository.Save();
                success = true;
            }
            return Json(new { success = success, id = id, error = error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _termRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}