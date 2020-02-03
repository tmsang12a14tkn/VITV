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

namespace VITV.Startups.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVideoCategoryRepository _videoCategoryRepository;
        private readonly IVideoCatGroupRepository _videoCatGroupRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleHighlightAllRepository _articleHighlightAllRepository;
        private readonly IVideoRepository _videoRepository;

        public HomeController()
        {
            var context = new VITVContext();
            _videoCategoryRepository = new VideoCategoryRepository(context);
            _articleCategoryRepository = new ArticleCategoryRepository(context);
            _videoRepository = new VideoRepository(context);
            _articleRepository = new ArticleRepository(context);
            _articleHighlightAllRepository = new ArticleHighlightAllRepository(context);
            _videoCatGroupRepository = new VideoCatGroupRepository(context);
        }

        public ActionResult Index()
        {
            var now = DateTime.Now.Date;
            var highlights = _articleHighlightAllRepository.GetMany(hl => DbFunctions.TruncateTime(hl.StartDate) <= now && DbFunctions.TruncateTime(hl.EndDate) >= now && hl.Article.ArticleStatus == (int)ArticleStatus.Good)
                                                            .OrderBy(hl => hl.Order).Take(4).ToList();

            if (highlights.Count < 4)
                highlights = _articleHighlightAllRepository.All.Where(hl => hl.Article.ArticleStatus == (int)ArticleStatus.Good).OrderBy(hl => hl.Article.PublishedTime).Take(4).ToList();

            //Get All Article with conditions
            var list = _articleRepository.All.Where(a => a.SeriesOrder <= 1 && a.IsPublished && a.ArticleStatus == (int)ArticleStatus.Good && DbFunctions.TruncateTime(a.PublishedTime) <= now)
                                            .OrderByDescending(a => a.PublishedTime)
                                            .ThenBy(a => a.Order).ToList();

            // Get Top video
            var vdCatGroups = _videoCategoryRepository.All.Where(vc => vc.PageGroupId == (int)PageType.Startup)
                                                        .Select(g => new VideoCatGroupView { Id = g.Id, Name = g.Name, UniqueTitle = g.UniqueTitle }).ToList();

            foreach (var cat in vdCatGroups)
            {
                cat.NewestVideos = _videoRepository.GetMany(v => v.Category.Id == cat.Id && v.IsPublished && v.PublishedTime <= DateTime.Now)
                                                    .OrderByDescending(v => v.DisplayTime).Take(3).ToList();
            }
            
            var homeModel = new HomeModel
            {
                HotArticles = list.Except(list.Where(art => highlights.Any(hl => hl.Id == art.Id))).ToList(),
                VideoCatGroups = vdCatGroups
            };

            ViewBag.Highlights = highlights;
            ViewBag.artCategory = _articleCategoryRepository.GetMany(c => c.ParentId == null || c.ParentId == 0);

            return View(homeModel);
        }

        public ActionResult GetMenu()
        {
            var model = _videoCatGroupRepository.AllIncluding(v => v.VideoCats).OrderBy(v => v.Order).ToList();
            return PartialView("_Menu", model);
        }

        public ActionResult GetSideMenu()
        {
            return PartialView("_SideMenu");
        }

        public ActionResult AnalyticPages(string url, string thumbnail, string title, string os, string dvModel, string dvType, string dvVendor, int? vId, int? vCatId, int? aId, int? aCatId)
        {
            if ((string.IsNullOrEmpty(dvModel) || dvModel == "undefined") && (os == "Mac OS" || os == "Windows")) dvModel = "Desktop";
            if ((string.IsNullOrEmpty(dvType) || dvType == "undefined") && (os == "Mac OS" || os == "Windows")) dvType = "desktop";

            if ((string.IsNullOrEmpty(os) || dvModel == "undefined") && dvType == "desktop" && os == "Mac OS") dvVendor = "Apple";
            if ((string.IsNullOrEmpty(os) || dvModel == "undefined") && dvType == "desktop" && os == "Windows") dvVendor = "Microsoft";

            _videoRepository.UpdateAccessCount(url, thumbnail, title, Request.UserHostAddress, os, dvModel, dvType, dvVendor, vId, vCatId, aId, aCatId);

            return Content(null);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}