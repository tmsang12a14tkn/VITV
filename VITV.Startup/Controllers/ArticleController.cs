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
using VITV.Data.Models;

namespace VITV.Startups.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IArticleHighlightAllRepository _articleHighlightAllRepository;
        private readonly IArticleHighlightCatRepository _articleHighlightCatRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IKeywordRepository _keywordRepository;

        public ArticleController()
        {
            var context = new VITVContext();
            _articleCategoryRepository = new ArticleCategoryRepository(context);
            _articleHighlightAllRepository = new ArticleHighlightAllRepository(context);
            _articleHighlightCatRepository = new ArticleHighlightCatRepository(context);
            _articleRepository = new ArticleRepository(context);
            _keywordRepository = new KeywordRepository(context);
        }

        public ActionResult Index()
        {
            var now = DateTime.Now.Date;
            var highlights = _articleHighlightAllRepository.GetMany(hl => DbFunctions.TruncateTime(hl.StartDate) <= now && DbFunctions.TruncateTime(hl.EndDate) >= now && hl.Article.ArticleStatus == (int)ArticleStatus.Good)
                                                            .OrderBy(hl => hl.Order).Take(4).ToList();
            if (highlights.Count < 4)
                highlights = _articleHighlightAllRepository.All.Where(hl => hl.Article.ArticleStatus == (int)ArticleStatus.Good).OrderBy(hl => hl.Article.PublishedTime).Take(4).ToList();

            var articles = _articleRepository.All.Where(a => a.SeriesOrder <= 1 && a.IsPublished && a.ArticleStatus == (int)ArticleStatus.Good && DbFunctions.TruncateTime(a.PublishedTime) <= now)
                                                .OrderByDescending(a => a.PublishedTime)
                                                .ThenBy(a => a.Order).ToList();
            articles = articles.Except(articles.Where(art => highlights.Any(hl => hl.Id == art.Id))).ToList();

            ViewBag.Highlights = highlights.Select(hl => hl.Article).ToList();
            ViewBag.artCategory = _articleCategoryRepository.GetMany(c => c.ParentId == null || c.ParentId == 0);

            return View(articles);
        }

        public ActionResult CategoryDetails(int id)
        {
            var model = _articleCategoryRepository.Find(id);
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var now = DateTime.Now.Date;
            var model = _articleRepository.Find(id);

            if (model.ArticleStatus == (int)ArticleStatus.Good)
            {
                var artCat = _articleCategoryRepository.GetMany(a => a.ParentId == null || a.ParentId == 0).OrderBy(a => a.Order).First();
                ViewBag.RelativeArticles = _articleRepository.All.Where(a => a.Categories.Any(ac => ac.Id == artCat.Id)
                                                                            && a.SeriesOrder <= 1
                                                                            && a.IsPublished
                                                                            && a.ArticleStatus == (int)ArticleStatus.Good
                                                                            && DbFunctions.TruncateTime(a.PublishedTime) <= now)
                                                            .OrderByDescending(a => a.PublishedTime)
                                                            .ThenBy(a => a.Order)
                                                            .Take(3).ToList();
                return View(model);
            }
            else
                return RedirectToAction("Index");
        }

        public PartialViewResult GetArticleCategories()
        {
            var model = _articleCategoryRepository.GetMany(ac => ac.ParentId == null || ac.ParentId == 0).OrderBy(ac => ac.Order);
            return PartialView(model);
        }

        public PartialViewResult GetPopularNews()
        {
            var now = DateTime.Now.Date;
            var model = _articleHighlightAllRepository.GetMany(hl => DbFunctions.TruncateTime(hl.StartDate) <= now 
                                                                    && DbFunctions.TruncateTime(hl.EndDate) >= now
                                                                    && hl.Article.ArticleStatus == (int)ArticleStatus.Good)
                                                        .OrderBy(hl => hl.Order).Take(5);
            if (model.Count() < 5)
            {
                model = _articleHighlightAllRepository.All.Where(hl => hl.Article.ArticleStatus == (int)ArticleStatus.Good).OrderBy(hl => hl.Order).Take(5);
            }
            return PartialView(model);
        }

        public PartialViewResult GetMoreArticles(int numRecord, int groupId)
        {
            var now = DateTime.Now.Date;
            var hlAll = _articleHighlightAllRepository.GetMany(hl => DbFunctions.TruncateTime(hl.StartDate) <= now
                                                                    && DbFunctions.TruncateTime(hl.EndDate) >= now
                                                                    && hl.Article.ArticleStatus == (int)ArticleStatus.Good)
                                                    .OrderBy(hl => hl.Order).ToList();
            if (hlAll.Count < 4)
                hlAll = _articleHighlightAllRepository.All.Where(hl => hl.Article.ArticleStatus == (int)ArticleStatus.Good).OrderBy(hl => hl.Order).Take(4).ToList();

            var articles = _articleRepository.All.Where(a => a.SeriesOrder <= 1 && a.IsPublished && a.ArticleStatus == (int)ArticleStatus.Good && DbFunctions.TruncateTime(a.PublishedTime) <= now)
                                                .OrderByDescending(a => a.PublishedTime)
                                                .ThenBy(a => a.Order).ToList();

            var highlights = hlAll.Select(m => m.Article).ToList();
            if (groupId > 0)
            {
                var hlCat = _articleHighlightCatRepository.GetMany(hl => DbFunctions.TruncateTime(hl.StartDate) <= now
                                                                        && DbFunctions.TruncateTime(hl.EndDate) >= now
                                                                        && hl.Article.Categories.Any(c => c.Id == groupId || c.ParentId == groupId))
                                                        .OrderBy(hl => hl.Order).ToList();
                if (hlCat.Count >= 4)
                    highlights = hlCat.Select(m => m.Article).ToList();
                else
                {
                    var tempHLCat = _articleHighlightCatRepository.GetMany(hl => hl.Article.Categories.Any(c => c.Id == groupId || c.ParentId == groupId))
                                                                    .OrderBy(hl => hl.Order).ToList();
                    highlights = tempHLCat.Select(m => m.Article).ToList();
                }
            }

            articles = articles.Except(articles.Where(art => highlights.Any(hl => hl.Id == art.Id))).ToList();
            if (groupId > 0)
                articles = articles.Where(a => a.Categories.Any(c => c.Id == groupId || c.ParentId == groupId)).Skip(numRecord).Take(5).ToList();
            else
                articles = articles.Skip(numRecord).Take(5).ToList();
            ViewBag.CurNumRecord = numRecord + articles.Count;
            ViewBag.HasMoreArticle = articles.Count > 0 ? 1 : 0;
            return PartialView(articles);
        }

        public PartialViewResult GetArticleBySeries(int id, int crArtId)
        {
            var now = DateTime.Now.Date;
            var model = _articleRepository.GetMany(a => a.ArticleSeriesId == id 
                                                        && a.IsPublished 
                                                        && a.ArticleStatus == (int)ArticleStatus.Good
                                                        && DbFunctions.TruncateTime(a.PublishedTime) <= now, a => a.SeriesOrder).ToList();
            ViewBag.CurrentArticleId = crArtId;
            return PartialView(model);
        }

        public PartialViewResult GetArticleByGroup(int id)
        {
            var now = DateTime.Now.Date;
            var cat = _articleCategoryRepository.Find(id);
            var hlAll = _articleHighlightAllRepository.GetMany(hl => DbFunctions.TruncateTime(hl.StartDate) <= now
                                                                    && DbFunctions.TruncateTime(hl.EndDate) >= now
                                                                    && hl.Article.ArticleStatus == (int)ArticleStatus.Good)
                                                    .OrderBy(hl => hl.Order).ToList();
            if (hlAll.Count < 4)
                hlAll = _articleHighlightAllRepository.All.Where(hl => hl.Article.ArticleStatus == (int)ArticleStatus.Good).OrderBy(hl => hl.Order).Take(4).ToList();

            var articles = _articleRepository.All.Where(a => a.SeriesOrder <= 1 && a.IsPublished && a.ArticleStatus == (int)ArticleStatus.Good && DbFunctions.TruncateTime(a.PublishedTime) <= now)
                                                .OrderByDescending(a => a.PublishedTime)
                                                .ThenBy(a => a.Order).ToList();

            var highlights = hlAll.Select(m => m.Article).ToList();
            if (id > 0)
            {
                var hlCat = _articleHighlightCatRepository.GetMany(hl => DbFunctions.TruncateTime(hl.StartDate) <= now
                                                                        && DbFunctions.TruncateTime(hl.EndDate) >= now
                                                                        && hl.Article.Categories.Any(c => c.Id == id || c.ParentId == id))
                                                        .OrderBy(hl => hl.Order).ToList();
                if (hlCat.Count >= 4)
                    highlights = hlCat.Select(m => m.Article).ToList();
                else
                {
                    var tempHLCat = _articleHighlightCatRepository.GetMany(hl => hl.Article.Categories.Any(c => c.Id == id || c.ParentId == id))
                                                                    .OrderBy(hl => hl.Order).ToList();
                    highlights = tempHLCat.Select(m => m.Article).ToList();
                }
                
                articles = articles.Where(a => a.Categories.Any(c => c.Id == id || c.ParentId == id)).ToList();
            }
            articles = articles.Except(articles.Where(art => highlights.Any(hl => hl.Id == art.Id))).ToList();


            ViewBag.artCategory = _articleCategoryRepository.GetMany(c => c.ParentId == null || c.ParentId == 0);
            ViewBag.Highlights = highlights;
            ViewBag.CurrentGrpId = id;
            ViewBag.CurrentGrpName = cat != null ? cat.Name : "Tất cả";

            return PartialView(articles);
        }

        public ActionResult GetArticleByKeyword(int id)
        {
            var now = DateTime.Now.Date;
            var model = _articleRepository.All.Where(a => a.Keywords.Any(kw => kw.Id == id) && a.SeriesOrder <= 1 && a.IsPublished && a.ArticleStatus == (int)ArticleStatus.Good && DbFunctions.TruncateTime(a.PublishedTime) <= now)
                                        .OrderByDescending(a => a.PublishedTime)
                                        .ThenBy(a => a.Order).ToList();

            ViewBag.Keyword = _keywordRepository.Find(id);
            return View(model);
        }
    }
}