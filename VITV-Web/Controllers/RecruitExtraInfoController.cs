using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.Repositories;

namespace VITV.Web.Controllers
{
    public class RecruitExtraInfoController : Controller
    {
        private readonly IRecruitExtraInfoRepository _recruitExtraInfoRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public RecruitExtraInfoController()
        {
            var context = new VITVContext();
            _recruitExtraInfoRepository = new RecruitExtraInfoRepository(context);
        }

        public RecruitExtraInfoController(IRecruitExtraInfoRepository recruitExtraInfoRepository)
        {
            _recruitExtraInfoRepository = recruitExtraInfoRepository;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Management()
        {
            return View(_recruitExtraInfoRepository.All);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recruitExtraInfo = _recruitExtraInfoRepository.Find(id.Value);
            if (recruitExtraInfo == null)
            {
                return HttpNotFound();
            }
            return View(recruitExtraInfo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase fileupload, RecruitExtraInfo recruitExtraInfo)
        {
            if (fileupload != null && fileupload.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + fileupload.FileName;
                string filePath = Path.Combine(Server.MapPath("~/UploadedImages"), fileName);
                fileupload.SaveAs(filePath);

                recruitExtraInfo.RecruitForm = string.Format("{0}/{1}", "/UploadedImages", fileName);
            }
            else if (String.IsNullOrEmpty(recruitExtraInfo.RecruitForm))
            {
                recruitExtraInfo.RecruitForm = "";
            }
            ModelState.Clear();
            TryValidateModel(recruitExtraInfo);

            if (ModelState.IsValid)
            {
                _recruitExtraInfoRepository.InsertOrUpdate(recruitExtraInfo);
                _recruitExtraInfoRepository.Save();
                return RedirectToAction("Management");
            }
            return View(recruitExtraInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _recruitExtraInfoRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
