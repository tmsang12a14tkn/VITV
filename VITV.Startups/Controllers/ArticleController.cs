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
    public class ArticleController : Controller
    {
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IArticleRepository _articleRepository;

        public ArticleController()
        {
            var context = new VITVContext();
            _articleCategoryRepository = new ArticleCategoryRepository(context);
            _articleRepository = new ArticleRepository(context);
        }

        public ActionResult Index()
        {
            var model = _articleRepository.All.OrderByDescending(art => art.PublishedTime).ToList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var model = _articleRepository.Find(id);
            return View(model);
        }
    }
}