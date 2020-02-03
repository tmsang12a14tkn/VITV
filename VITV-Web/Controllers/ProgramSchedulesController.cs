using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;

namespace VITV.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ProgramSchedulesController : Controller
    {
        private readonly IProgramScheduleRepository _programScheduleRepository;
        private readonly IVideoCategoryRepository _videoCategoryRepository;

        public ProgramSchedulesController()
        {
            var context = new VITVContext();
            _programScheduleRepository = new ProgramScheduleRepository(context);
            _videoCategoryRepository = new VideoCategoryRepository(context);
        }

        // GET: ProgramSchedules
        [RoleAuthorize(Site = "admin")]
        public ActionResult Index(DayOfWeek? dayOfWeek)
        {
            var view = new ProgramSchedulesView
            {
                DayOfWeek = dayOfWeek.HasValue ? dayOfWeek.Value : DayOfWeek.Monday
            };

            return View(view);
        }

        public PartialViewResult DowList(DayOfWeek dayOfWeek) //
        {

            var programSchedules = _programScheduleRepository.GetMany(p => p.DayOfWeek == dayOfWeek, p => p.Time).ToList();
            var view = new DowScheduleView
            {
                ProgramSchedules = programSchedules,
                DayOfWeek = dayOfWeek
            };
            return PartialView(view);
        }


        // GET: ProgramSchedules/Create
        public PartialViewResult Create(DayOfWeek dayOfWeek)
        {
            ViewBag.VideoCategoryId = new SelectList(
                _videoCategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name),
                "Id",
                "Name");
            return PartialView(new ProgramSchedule { DayOfWeek = dayOfWeek });
        }

        // POST: ProgramSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VideoCategoryId,DayOfWeek,Time,IsNew")] ProgramSchedule programSchedule)
        {
            if (ModelState.IsValid)
            {
                _programScheduleRepository.InsertOrUpdate(programSchedule);
                _programScheduleRepository.Save();
                return RedirectToAction("Index", "ProgramSchedules", new { dayOfWeek = programSchedule.DayOfWeek });
            }

            ViewBag.VideoCategoryId = new SelectList(
                _videoCategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name),
                "Id",
                "Name",
                programSchedule.VideoCategoryId);

            return View(programSchedule);
        }


        // GET: ProgramSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramSchedule programSchedule = _programScheduleRepository.Find(id.Value);
            if (programSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.VideoCategoryId = new SelectList(
                _videoCategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name),
                "Id",
                "Name",
                programSchedule.VideoCategoryId);
            return View(programSchedule);
        }

        // POST: ProgramSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VideoCategoryId,DayOfWeek,Time,IsNew")] ProgramSchedule programSchedule)
        {
            if (ModelState.IsValid)
            {
                _programScheduleRepository.InsertOrUpdate(programSchedule);
                _programScheduleRepository.Save();
                return RedirectToAction("Index", new { dayOfWeek = programSchedule.DayOfWeek });
            }
            ViewBag.VideoCategoryId = new SelectList(
                _videoCategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name),
                "Id",
                "Name",
                programSchedule.VideoCategoryId);
            return View(programSchedule);
        }


        // GET: ProgramSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramSchedule programSchedule = _programScheduleRepository.Find(id.Value);
            if (programSchedule == null)
            {
                return HttpNotFound();
            }
            return PartialView(programSchedule);
        }

        // POST: ProgramSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string confirmText)
        {
            var error = "";
            if (confirmText.ToLower() == "đồng ý")
            {
                var programSchedule = _programScheduleRepository.Delete(id);

                _programScheduleRepository.Save();
                return Json(new { success = 1, id = id });
            }
            else
            {
                error = "Chuỗi nhập vào không đúng";
            }
            return Json(new { success = 0, content = error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _programScheduleRepository.Dispose();
                _videoCategoryRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
