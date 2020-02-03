using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Helpers;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;

namespace VITV.Startup.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoCategoryRepository _videoCategoryRepository;
        private readonly IVideoRepository _videoRepository;

        public VideoController()
        {
            var context = new VITVContext();
            _videoCategoryRepository = new VideoCategoryRepository(context);
            _videoRepository = new VideoRepository(context);
        }

        public ActionResult CategoryDetails(int id)
        {
            var model = _videoCategoryRepository.All.Where(v => v.Id == id).FirstOrDefault();
            return View(model);
        }

        public PartialViewResult GetMoreVideos(int catId, int? month, int numRecord = 0)
        {
            var videosQuery = _videoRepository.GetMany(v => v.CategoryId == catId);
            if (month.HasValue && month > 0)
            {
                videosQuery = videosQuery.Where(v => v.DisplayTime.Month == month);
            }

            if (numRecord == 0)
                videosQuery = videosQuery.Take(10);
            else
                videosQuery = videosQuery.Skip(numRecord).Take(5);
            var videos = videosQuery.Where(v => v.IsPublished && v.PublishedTime <= DateTime.Now).OrderByDescending(v => v.DisplayTime).ToList();
            
            ViewBag.month = month;
            ViewBag.CurNumRecord = numRecord + videos.Count;
            ViewBag.HasMoreVideo = videos.Count > 0 ? 1 : 0;
            return PartialView("_Videos", videos);
        }

        public PartialViewResult GetVideoCategories()
        {
            var model = _videoCategoryRepository.GetMany(vc => vc.PageGroupId == (int)PageType.Startup, vc => vc.Order);
            return PartialView(model);
        }
    }
}