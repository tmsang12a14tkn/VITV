using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Helpers;
using VITV.Data.Repositories;

namespace VITV.Portal.Controllers
{
    public class UploadReportController : Controller
    {
        private readonly IVideoRepository _videoRepository;

        public UploadReportController()
        {
            var context = new VITVContext();
            _videoRepository = new VideoRepository(context);
        }

        [RoleAuthorize(Site = "portal")]
        public ActionResult Summary(DateTime? from, DateTime? to, string dateType = "year")
        {
            DateTime d = DateTime.Today;

            //Mặc định tuần
            if (!from.HasValue || !to.HasValue)
            {
                from = new DateTime(d.Year, 1, 1);
                to = new DateTime(d.Year, 12, 31);
                dateType = "year";
            }

            var model = _videoRepository.GetVideoUploader(from.Value, to.Value);

            ViewBag.dateType = dateType;
            ViewBag.fromDate = from.Value.Date.ToString("dd/MM/yyyy");
            ViewBag.toDate = to.Value.Date.ToString("dd/MM/yyyy");

            return View(model);
        }

        [RoleAuthorize(Site = "portal")]
        public ActionResult Details(DateTime? date, string emp)
        {
            var context = new VITVContext();
            if (date == null)
                date = DateTime.Now;
            var videos = _videoRepository.GetVideoUploadReport(date.Value, emp);
            ViewBag.today = date;
            ViewBag.employeeid = emp;
            ViewBag.users = context.Users.ToList();
            return View(videos);
        }

        [RoleAuthorize(Site="portal")]
        public ActionResult DeletionLogs()
        {
            var context = new VITVContext();
            var deletionLogs = context.DeletionLogs.OrderByDescending(log=>log.DeletionTime).ToList();
            return View(deletionLogs);
        }


    }
}