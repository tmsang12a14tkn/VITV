using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;

namespace VITV.Portal.Controllers
{
    public class VideoRateController : Controller
    {
        private readonly IProgramScheduleDetailRepository _scheduleDetailRepository;
        private readonly IVideoCategoryRepository _videoCategoryRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IProgramScheduleRepository _scheduleRepository;

        public VideoRateController()
        {
            var context = new VITVContext();
            _scheduleDetailRepository = new ProgramScheduleDetailRepository(context);
            _videoCategoryRepository = new VideoCategoryRepository(context);
            _videoRepository = new VideoRepository(context);
            _scheduleRepository = new ProgramScheduleRepository(context);
        }
        // GET: VideoRate
        public ActionResult Index(DateTime? date)
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