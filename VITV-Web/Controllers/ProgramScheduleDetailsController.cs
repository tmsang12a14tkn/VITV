
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Schema;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;
using VITV.Data.Helpers;

namespace VITV.Web.Controllers
{
    
    public class ProgramScheduleDetailsController : Controller
    {
        private readonly IProgramScheduleDetailRepository _scheduleDetailRepository;
        private readonly IVideoCategoryRepository _videoCategoryRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IProgramScheduleRepository _scheduleRepository;

        public ProgramScheduleDetailsController()
        {
            var context = new VITVContext();
            _scheduleDetailRepository = new ProgramScheduleDetailRepository(context);
            _videoCategoryRepository = new VideoCategoryRepository(context);
            _videoRepository = new VideoRepository(context);
            _scheduleRepository = new ProgramScheduleRepository(context);
        }

        public ActionResult Index(DateTime? date)
        {
            if (!date.HasValue)
            {
                date = DateTime.Now.Date;
            }
            var scheduleDetails =
                _scheduleDetailRepository.GetMany(p => DbFunctions.TruncateTime(p.DateTime) == date,
                    p => p.DateTime).ToList();
            if (scheduleDetails.Count == 0)
            {
                var dayOfWeek = date.Value.DayOfWeek;

                scheduleDetails = _scheduleRepository.GetMany(s => s.DayOfWeek == dayOfWeek, s => s.Time)
                    .Select(s => new ProgramScheduleDetail
                    {
                        VideoCategoryId = s.VideoCategoryId,
                        DateTime = date.Value.Add(s.Time),
                        VideoCategory = s.VideoCategory,
                        IsNew = s.IsNew
                    }).ToList();

            }
            ViewBag.Date = date.Value;
            return View(scheduleDetails);
        }

        [RoleAuthorize(Site="admin")]
        // GET: ProgramScheduleDetails/Management
        public ActionResult Management(DateTime? date)
        {
            if (!date.HasValue)
            {
                date = DateTime.Now.Date;
            }

            DateTime dateValue = date.Value;
            var list = new List<ScheduleDetailFilterView>();
            for (int i = 0; i < 7; i++)
            {
                var view = new ScheduleDetailFilterView
                {
                    DateTime = dateValue,
                    ScheduleDetails =
                        _scheduleDetailRepository.GetMany(p => DbFunctions.TruncateTime(p.DateTime) == dateValue,
                            p => p.DateTime).ToList()
                };
                list.Add(view);
                dateValue = dateValue.AddDays(1);
            }

            return View(list);
        }

         [RoleAuthorize(Site = "admin")]
        [HttpPost]
        public ActionResult Management(ScheduleDetailFilterView view)
        {
            view.ScheduleDetails = _scheduleDetailRepository.GetMany(p=>DbFunctions.TruncateTime(p.DateTime) == view.DateTime.Date, p=>p.DateTime) .ToList();

            return View(view);
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult UpdateVideoLink(int id)
        {
            var detail = _scheduleDetailRepository.Find(id);
            var maxDate = detail.DateTime;
            var minDate = detail.DateTime.AddDays(-14);
            ViewBag.VideoId =
                _scheduleDetailRepository.GetMany(
                    dt =>
                        dt.IsNew == true && dt.DateTime <= maxDate && dt.DateTime >= minDate &&
                        dt.VideoCategoryId == detail.VideoCategoryId && dt.VideoId!=null, dt => dt.DateTime)
                        .Select(dt => new SelectListItem
                    {
                        Text = dt.Video.Title,
                        Value = dt.VideoId.ToString()
                    });
                        
            return View(detail);
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost]
        public ActionResult UpdateVideoLink(ProgramScheduleDetail detail)
        {
            if (detail.VideoId != null)
            {
                _scheduleDetailRepository.UpdateVideo(detail.Id, detail.VideoId.Value);
                _scheduleDetailRepository.Save();
            }

            return RedirectToAction("Management", new {date = detail.DateTime.ToShortDateString()});
        }

        //public PartialViewResult Today()
        //{
        //    var today = DateTime.Now.Date;
        //    var scheduleDetails =
        //        _scheduleDetailRepository.GetMany(p => DbFunctions.TruncateTime(p.DateTime) == today,
        //            p => p.DateTime).ToList();
        //    if (scheduleDetails.Count == 0)
        //    {
        //        var dayOfWeek = today.DayOfWeek;

        //        scheduleDetails = _scheduleRepository.GetMany(s => s.DayOfWeek == dayOfWeek, s => s.Time)
        //            .Select(s => new ProgramScheduleDetail
        //            {
        //                VideoCategoryId = s.VideoCategoryId,
        //                DateTime = today.Date.Add(s.Time),
        //                VideoCategory = s.VideoCategory,
        //                IsNew = s.IsNew
        //            }).ToList();
               
        //    }
        //    return PartialView("_ListView", scheduleDetails);
        //}

        public PartialViewResult WeekSchedules()
        {
            var beginDay = DateTime.Now.Date.AddDays(-3);

            Dictionary<DateTime, List<ProgramScheduleDetail>> weekSchedules = new Dictionary<DateTime, List<ProgramScheduleDetail>>();
            for (int i = 0; i < 7; i++)
            {
                var currentDay = beginDay.AddDays(i);
                var scheduleDetails =
                    _scheduleDetailRepository.GetMany(p => DbFunctions.TruncateTime(p.DateTime) == currentDay,
                        p => p.DateTime).ToList();
                if (scheduleDetails.Count == 0)
                {
                    var dayOfWeek = currentDay.DayOfWeek;

                    scheduleDetails = _scheduleRepository.GetMany(s => s.DayOfWeek == dayOfWeek, s => s.Time)
                        .Select(s => new ProgramScheduleDetail
                        {
                            VideoCategoryId = s.VideoCategoryId,
                            DateTime = currentDay.Date.Add(s.Time),
                            VideoCategory = s.VideoCategory,
                            IsNew = s.IsNew
                        }).ToList();
                }
                weekSchedules.Add(currentDay, scheduleDetails);
            }
            return PartialView("_ListView", weekSchedules);
        }

        [RoleAuthorize(Site = "admin")]
        // GET: ProgramScheduleDetails/Create
        public ActionResult Create(DateTime? date)
        {
            if (!date.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.VideoCategories = _videoCategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name);
            //Lấy thông tin từ khung chương trình
            var dayOfWeek = date.Value.DayOfWeek;

            var detailList = _scheduleRepository.GetMany(s => s.DayOfWeek == dayOfWeek, s => s.Time)
                .Select(s => new ProgramScheduleDetail
                {
                    VideoCategoryId = s.VideoCategoryId,
                    DateTime = date.Value.Date.Add(s.Time),
                    VideoCategory = s.VideoCategory,
                    IsNew = s.IsNew
                }).ToList();

            var model = new DowScheduleDetailModel()
            {
                Date = date.Value,
                Details = detailList
            };

            return View(model);
        }

        // POST: ProgramScheduleDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleAuthorize(Site = "admin")]
        [HttpPost]
        public ActionResult Create(DowScheduleDetailModel model)
        {
            foreach (var programScheduleDetail in model.Details)
            {
                programScheduleDetail.DateTime = model.Date.Date.Add(programScheduleDetail.DateTime.TimeOfDay);
                 _scheduleDetailRepository.InsertOrUpdate(programScheduleDetail);
            }
            _scheduleDetailRepository.Save();
            return RedirectToAction("Management", new {date = model.Date.ToShortDateString()});
        }

        [RoleAuthorize(Site = "admin")]
        public PartialViewResult Insert(DateTime? date)
        {
            if(date == null)
                date = DateTime.Now;
            var model = new ProgramScheduleDetail
            {
                DateTime = date.Value
            };
            ViewBag.VideoCategoryId = new SelectList(_videoCategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name),
                "Id",
                "Name");
            return PartialView("_Insert",model);
        }

       [RoleAuthorize(Site = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Insert(ProgramScheduleDetail scheduleDetail, DateTime date)
        {
            scheduleDetail.DateTime = date.Add(scheduleDetail.DateTime.TimeOfDay);
            _scheduleDetailRepository.InsertOrUpdate(scheduleDetail);
            _scheduleDetailRepository.Save();

            var scheduleDetails =
                _scheduleDetailRepository.AllIncluding(s=>s.VideoCategory).Where(p => DbFunctions.TruncateTime(p.DateTime) == date).OrderBy(p => p.DateTime).ToList();
           
            //return new EmptyResult();
            return PartialView("_ListEdit", scheduleDetails);
        }

        // GET: ProgramScheduleDetails/Edit/5
        [RoleAuthorize(Site = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramScheduleDetail programScheduleDetail = _scheduleDetailRepository.Find(id.Value);
            if (programScheduleDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.VideoCategoryId = new SelectList(_videoCategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name),
                "Id",
                "Name",
                programScheduleDetail.VideoCategoryId);
            return View(programScheduleDetail);
        }

        // POST: ProgramScheduleDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleAuthorize(Site = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VideoCategoryId,VideoId,DateTime,Name, IsNew")] ProgramScheduleDetail programScheduleDetail, DateTime date)
        {
            if (ModelState.IsValid)
            {
                programScheduleDetail.DateTime = date.Add(programScheduleDetail.DateTime.TimeOfDay);
                _scheduleDetailRepository.InsertOrUpdate(programScheduleDetail);
                _scheduleDetailRepository.Save();
                return RedirectToAction("Management", new {date=programScheduleDetail.DateTime.ToShortDateString()});
            }
            ViewBag.VideoCategoryId = new SelectList(_videoCategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name),
                "Id",
                "Name",
                programScheduleDetail.VideoCategoryId);
            return View(programScheduleDetail);
        }

        // GET: ProgramScheduleDetails/Delete/5
        [RoleAuthorize(Site = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramScheduleDetail programScheduleDetail = _scheduleDetailRepository.Find(id.Value);
            return PartialView(programScheduleDetail);
        }

        // POST: ProgramScheduleDetails/Delete/5
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id, string confirmText)
        {
            var success = false;
            var error = "";
            List<int> ids = new List<int>();
            if (confirmText.ToLower() == "đồng ý")
            {
                var scheduleDetail = _scheduleDetailRepository.Find(id);
                if (scheduleDetail != null)
                {
                    if (scheduleDetail.VideoId.HasValue && scheduleDetail.IsNew == true)
                    {
                        var videoId = scheduleDetail.VideoId.Value;
                        var video = _videoRepository.Find(videoId);
                        //Xóa toàn bộ các chương trình liên quan
                        for (int i = video.ScheduleDetails.Count - 1; i >= 0; i--)
                        {
                            var scheduleDetailId = video.ScheduleDetails.ElementAt(i).Id;
                            ids.Add(scheduleDetailId);
                            _scheduleDetailRepository.Delete(scheduleDetailId);
                        }
                        _scheduleDetailRepository.Save();

                        //Xóa video
                        using (new Impersonator("uploaduser", "", "Upload@@123"))
                        {
                            string thumbnailPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, video.Thumbnail);
                            if (System.IO.File.Exists(thumbnailPath))
                            {
                                System.IO.File.Delete(thumbnailPath);
                            }

                            string videoPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this,video.Url);
                        
                            if (System.IO.File.Exists(videoPath))
                            {
                                System.IO.File.Delete(videoPath);
                            }
                        }
                        _videoRepository.Delete(videoId, User.Identity.Name, "Xóa lịch phát sóng");
                        _videoRepository.Save();
                    }

                    else
                    {
                        ids.Add(id);
                        //Xóa chương trình
                        _scheduleDetailRepository.Delete(id);
                        _scheduleDetailRepository.Save();
                    }
                    success = true;
                }
                else
                {
                    error = "Không tìm thấy chương trình";
                }
            }
            else
            {
                error = "Chuỗi nhập vào không đúng";
            }
            return Json(new { success = success, ids = ids, error = error});
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost]
        public ActionResult ChangeNew(int id, bool deleteVideo)
        {
            ProgramScheduleDetail programScheduleDetail = _scheduleDetailRepository.Find(id);
            
            
            if(deleteVideo == true && programScheduleDetail.VideoId.HasValue)
            {
                //Xóa video
                var videoId = programScheduleDetail.VideoId.Value;
                
                var video = _videoRepository.Find(videoId);
                video.ScheduleDetails.ForEach(sch => sch.VideoId = null);
                _videoCategoryRepository.Save();

                //Xóa file
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    string thumbnailPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, video.Thumbnail);
                    if (System.IO.File.Exists(thumbnailPath))
                    {
                        System.IO.File.Delete(thumbnailPath);
                    }

                    string videoPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this,video.Url);
                
                    if (System.IO.File.Exists(videoPath))
                    {
                        System.IO.File.Delete(videoPath);
                    }
                }
                _videoRepository.Delete(videoId, User.Identity.Name, "Thay đổi phát mới/phát lại");
                _videoRepository.Save();
            }

            programScheduleDetail.IsNew = !programScheduleDetail.IsNew;
            programScheduleDetail.VideoId = null;
            _videoCategoryRepository.Save();
            var url = programScheduleDetail.IsNew ? Url.Action("Create", "Video", new { scheduleDetailId = programScheduleDetail.Id })
                : Url.Action("UpdateVideoLink", "ProgramScheduleDetails", new { id = programScheduleDetail.Id });
            return Json(new { success = true, isnew = programScheduleDetail.IsNew, url = url });
            
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _scheduleDetailRepository.Dispose();
                _videoCategoryRepository.Dispose();
                _scheduleRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
