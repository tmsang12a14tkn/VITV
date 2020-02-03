using System;
using System.Collections.Generic;
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
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IVideoRepository _videoRepository;

        public HomeController()
        {
            var context = new VITVContext();
            _videoCategoryRepository = new VideoCategoryRepository(context);
            _articleCategoryRepository = new ArticleCategoryRepository(context);
            _videoRepository = new VideoRepository(context);
            _articleRepository = new ArticleRepository(context);
        }

        public ActionResult Index()
        {
            var homeModel = new HomeModel
            {
                HotArticles = _articleRepository.All.OrderByDescending(art => art.PublishedTime).ToList()
            };

            return View(homeModel);
        }
    }
}