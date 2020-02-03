using Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;
using VITV.Web.Helpers;
using VITV_Web.ViewModels;
using HtmlAgilityPack;


namespace VITV.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IReporterRepository _reporterRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IKeywordRepository _keywordRepository;
        private readonly IArticleSeriesRepository _articleSeriesRepository;
        private readonly IArticleHighlightAllRepository _articleHighlightAllRepository;
        private readonly IArticleHighlightCatRepository _articleHighlightCatRepository;
        private readonly IArticleRevisionRepository _articleRevisionRepository;

        private static System.Globalization.DateTimeFormatInfo dfi = System.Globalization.DateTimeFormatInfo.CurrentInfo;
        private static System.Globalization.Calendar cal = dfi.Calendar;
        private readonly VITVContext context;
        // If you are using Dependency Injection, you can delete the following constructor
        public ArticleController()
        {
           context = new VITVContext();
            _articleCategoryRepository = new ArticleCategoryRepository(context);
            _reporterRepository = new ReporterRepository(context);
            _articleRepository = new ArticleRepository(context);
            _keywordRepository = new KeywordRepository(context);
            _articleSeriesRepository = new ArticleSeriesRepository(context);
            _articleHighlightAllRepository = new ArticleHighlightAllRepository(context);
            _articleHighlightCatRepository = new ArticleHighlightCatRepository(context);
            _articleRevisionRepository = new ArticleRevisionRepository(context);
        }

        public ArticleController(IArticleCategoryRepository articleCategoryRepository, IArticleRevisionRepository articleRevisionRepository, IArticleHighlightCatRepository articleHighlightCatRepository, IArticleHighlightAllRepository articleHighlightAllRepository, IReporterRepository reporterRepository, IArticleRepository articleRepository, IArticleSeriesRepository articleSeriesRepository, IKeywordRepository keywordRepository)
        {
            _articleCategoryRepository = articleCategoryRepository;
            _reporterRepository = reporterRepository;
            _articleRepository = articleRepository;
            _keywordRepository = keywordRepository;
            _articleSeriesRepository = articleSeriesRepository;
            _articleHighlightAllRepository = articleHighlightAllRepository;
            _articleHighlightCatRepository = articleHighlightCatRepository;
            _articleRevisionRepository = articleRevisionRepository;
        }

        public ViewResult Index()
        {
            var homeView = new ArticleHomeView
            {
                Articles = _articleRepository.All.OrderByDescending(v => v.PublishedTime).Take(5).ToList(),
                ArticleCategories = _articleCategoryRepository.All.ToList(),
                CountArticle = _articleRepository.All.Count()
            };
            return View(homeView);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult Details(int id)
        {
            var article = _articleRepository.Find(id);
            if (article != null)
                return View(AutoMapper.Mapper.Map<ArticleModel>(article));

            return new HttpNotFoundResult("Không tìm thấy bài viết");
        }

        [RoleAuthorize(Site = "admin")]
        [HttpGet]
        public ViewResult Management(DateTime? begin, DateTime? end, int prevnext = 0, int step = 5, string cat = "all", string rep = "", string title = "", int weektab = 0)
        {
            var now = DateTime.Now;
            var articlesQuery = _articleRepository.GetMany(art => art.ArticleStatus > 0, art => art.PublishedTime);

            if (begin == null || end == null)
            {
                end = Utilities.GetLastDateOfWeek(now, DayOfWeek.Sunday);
                begin = end.Value.AddDays(-7 * step).AddDays(1);
            }
            else
            {
                if (prevnext == 0)
                {
                    end = Utilities.GetLastDateOfWeek(now, DayOfWeek.Sunday);
                    begin = end.Value.AddDays(-7 * step);
                }
                else if (prevnext == -1)
                {
                    end = Utilities.GetFirstDateOfWeek(begin.Value, DayOfWeek.Monday).AddDays(-1);
                    begin = end.Value.AddDays(-7 * step).AddDays(1);
                }
                else if (prevnext == 1)
                {
                    begin = Utilities.GetLastDateOfWeek(end.Value, DayOfWeek.Sunday).AddDays(1);
                    end = begin.Value.AddDays(7 * step);
                }
            }
            articlesQuery = articlesQuery.Where(v => v.PublishedTime.HasValue && v.PublishedTime.Value.Date >= begin && v.PublishedTime.Value.Date <= end);
            
            int catId;
            if (cat != "all" && int.TryParse(cat, out catId))
            {
                articlesQuery = articlesQuery.Where(v => v.Categories.Any(c => c.Id == catId));
            }

            if (rep != "")
            {
                articlesQuery = articlesQuery.Where(v => v.Reporters.Any(r => r.Name.Contains(rep)));
            }

            if (title != "")
            {
                articlesQuery = articlesQuery.Where(v => v.Title.Contains(title));
            }

            var startDate = begin.Value;
            var endDate = end.Value;
            var list = new List<ArticleCustomDateView>();
            for (int i = 0; i < 5; i++)
            {
                endDate = startDate.AddDays(6);
                var view = new ArticleCustomDateView
                {
                    Week = cal.GetWeekOfYear(startDate, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday),
                    StartDate = startDate,
                    EndDate = endDate,
                    Articles = articlesQuery.Where(art => art.PublishedTime.HasValue && art.PublishedTime.Value.Date >= startDate && art.PublishedTime.Value.Date <= endDate)
                                            .GroupJoin(_articleSeriesRepository.All, art => art.ArticleSeriesId, asr => asr.Id, (art, asr) => new { Art = art, Srs = asr })
                                            .SelectMany(xy => xy.Srs.DefaultIfEmpty(), (x, y) => new { Art = x.Art, Srs = y })
                                            .Select(a => new ArticleCustomView()
                                            {
                                                Id = a.Art.Id,
                                                ArticleName = a.Art.Title,
                                                SeriesName = a.Srs != null && a.Srs.Articles.Count > 0 ? a.Srs.Name : "",
                                                SeriesId = a.Srs != null ? a.Srs.Id : 0,
                                                IsPublished = a.Art.IsPublished,
                                                Order = a.Art.Order,
                                                PublishedTime = a.Art.PublishedTime,
                                                Thumbnail = a.Art.Thumbnail,
                                                CatThumbnail = a.Art.Categories.Count > 0? a.Art.Categories.ElementAt(0).ProfilePhoto:"",
                                                CatName = String.Join(", ", a.Art.Categories.OrderBy(r => r.Order).Select(r => r.Name)),
                                                Reporters = a.Art.Reporters.ToList(),
                                                UniqueTitle = a.Art.UniqueTitle,
                                                ArticleStatus = a.Art.ArticleStatus,
                                                
                                            }).ToList()
                };

                foreach (var art in view.Articles)
                {
                    var hlAll = _articleHighlightAllRepository.Find(art.Id);
                    var hlCat = _articleHighlightCatRepository.Find(art.Id);

                    art.IsHighlightAll = hlAll == null ? false : true;
                    art.IsHighlightCat = hlCat == null ? false : true;
                    var firstWeekReport = _articleRepository.GetFirstWeekReport(art.Id);
                    var allTimeReport = _articleRepository.GetAllTimeReport(art.Id);
                    art.AllTimeCount = allTimeReport.PageView;
                    art.FirstWeekCount = firstWeekReport.PageView;
                }

                list.Add(view);
                startDate = endDate.AddDays(1);
            }

            var filterView = new ArticleFilterView
            {
                Category = cat,
                Reporter = rep,
                Title = title,
                Begin = begin,
                End = end,
                CustomArticles = list
            };

            ViewBag.weektab = weektab;
            ViewBag.ArticleCat = _articleCategoryRepository.All.ToList();
            return View(filterView);
        }
        [Authorize]
        public ActionResult ManagementTab(int? week, int? year, string tab = "needreview")
        { 
            
            if (User.IsInRole("Writter")) tab = "neededit";
            ViewBag.tab = tab;

            MyArticlesView view = new MyArticlesView();
            view.NeedEditList = _articleRepository.All.Where(a => a.ArticleStatus == (int)ArticleStatus.NeedEdit).ToList();
            view.NeedReviewList = _articleRepository.All.Where(a => a.ArticleStatus == (int)ArticleStatus.NeedReview).ToList();
            view.GoodList = _articleRepository.All.Where(a => a.ArticleStatus == (int)ArticleStatus.Good && !a.IsPublished).ToList();
            view.RejectList = _articleRepository.All.Where(a => a.ArticleStatus == (int)ArticleStatus.Reject).ToList();
            //view.PublishedList = _articleRepository.All.Where(a => a.IsPublished).ToList();

            var now = DateTime.Now;
            var articlesQuery = _articleRepository.GetMany(art => art.ArticleStatus > 0, art => art.PublishedTime);
            DateTime begin, end;
            if (!year.HasValue)
                year = now.Year;
            if(week.HasValue)
            {
                end = Utilities.GetLastDateOfWeek(year.Value, week.Value, DayOfWeek.Sunday);
            }
            else
                end = Utilities.GetLastDateOfWeek(now, DayOfWeek.Sunday);
            begin = end.AddDays(-7 * 5).AddDays(1);
            
            articlesQuery = articlesQuery.Where(v => v.PublishedTime.HasValue && v.PublishedTime.Value.Date >= begin && v.PublishedTime.Value.Date <= end);

           
            var startDate = begin;
            DateTime endDate;
            var publishedList = new List<ArticleCustomDateView>();
            for (int i = 0; i < 5; i++)
            {
                endDate = startDate.AddDays(6);
                var weekList = new ArticleCustomDateView
                {
                    Week = cal.GetWeekOfYear(startDate, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday),
                    StartDate = startDate,
                    EndDate = endDate,
                    Articles = articlesQuery.Where(art => art.IsPublished && art.PublishedTime.HasValue && art.PublishedTime.Value.Date >= startDate && art.PublishedTime.Value.Date <= endDate)
                                            .GroupJoin(_articleSeriesRepository.All, art => art.ArticleSeriesId, asr => asr.Id, (art, asr) => new { Art = art, Srs = asr })
                                            .SelectMany(xy => xy.Srs.DefaultIfEmpty(), (x, y) => new { Art = x.Art, Srs = y })
                                            .Select(a => new ArticleCustomView()
                                            {
                                                Id = a.Art.Id,
                                                ArticleName = a.Art.Title,
                                                SeriesName = a.Srs != null && a.Srs.Articles.Count > 0 ? a.Srs.Name : "",
                                                SeriesId = a.Srs != null ? a.Srs.Id : 0,
                                                IsPublished = a.Art.IsPublished,
                                                Order = a.Art.Order,
                                                PublishedTime = a.Art.PublishedTime,
                                                Thumbnail = a.Art.Thumbnail,
                                                CatThumbnail = a.Art.Categories.Count > 0 ? a.Art.Categories.ElementAt(0).ProfilePhoto : "",
                                                CatName = String.Join(", ", a.Art.Categories.OrderBy(r => r.Order).Select(r => r.Name)),
                                                Reporters = a.Art.Reporters.ToList(),
                                                UniqueTitle = a.Art.UniqueTitle,
                                                ArticleStatus = a.Art.ArticleStatus,

                                            }).ToList()
                };

                foreach (var art in weekList.Articles)
                {
                    var hlAll = _articleHighlightAllRepository.Find(art.Id);
                    var hlCat = _articleHighlightCatRepository.Find(art.Id);

                    art.IsHighlightAll = hlAll == null ? false : true;
                    art.IsHighlightCat = hlCat == null ? false : true;
                    var firstWeekReport = _articleRepository.GetFirstWeekReport(art.Id);
                    var allTimeReport = _articleRepository.GetAllTimeReport(art.Id);
                    art.AllTimeCount = allTimeReport.PageView;
                    art.FirstWeekCount = firstWeekReport.PageView;
                }

                publishedList.Add(weekList);
                startDate = endDate.AddDays(1);
            }

            view.PublishedList = publishedList;

            return View(view);
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult AdminSearch(string query)
        {
            List<ArticleCustomView> searchResult = _articleRepository.GetMany(a => a.Title.ToLower().Contains(query), a => a.CreatedTime).Select(a => new ArticleCustomView()
                                            {
                                                Id = a.Id,
                                                ArticleName = a.Title,
                                                SeriesName = a.Series != null ? a.Series.Name : "",
                                                SeriesId = a.Series != null ? a.Series.Id : 0,
                                                IsPublished = a.IsPublished,
                                                Order = a.Order,
                                                PublishedTime = a.PublishedTime,
                                                Thumbnail = a.Thumbnail,
                                                CatThumbnail = a.Categories.Count > 0 ? a.Categories.ElementAt(0).ProfilePhoto : "",
                                                CatName = String.Join(", ", a.Categories.OrderBy(r => r.Order).Select(r => r.Name)),
                                                Reporters = a.Reporters.ToList(),
                                                UniqueTitle = a.UniqueTitle,
                                                ArticleStatus = a.ArticleStatus
                                            }).ToList();

            return PartialView("_ListEdit",searchResult);
        }

        [RoleAuthorize(Site = "admin")]
        [HttpGet]
        public JsonResult GetDateHaveArticle()
        {
            var result = _articleRepository.All.GroupBy(art => DbFunctions.TruncateTime(art.PublishedTime)).ToList().Select(g => g.Key.Value.ToString("MM/dd/yyyy 12:00:00")).ToArray();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [RoleAuthorize(Site = "admin")]
        [HttpGet]
        public ViewResult HighLightManagement()
        {
            var now = DateTime.Now.Date;
            var artCategory = _articleCategoryRepository.GetMany(c => c.ParentId == null || c.ParentId == 0);
            var firstCat = artCategory.First();

            var model = new ArticleHighlightView
            {
                HLAlls = _articleHighlightAllRepository.All.Select(art => new ArticleHLCustomView()
                                                            {
                                                                Id = art.Id,
                                                                Name = art.Article.Title,
                                                                Series = art.Article.Series == null ? "" : art.Article.Series.Articles.Count() + " bài",
                                                                IsSeries = art.Article.Series == null ? false : true,
                                                                IsExpire = DbFunctions.TruncateTime(art.StartDate) <= now && DbFunctions.TruncateTime(art.EndDate) >= now,
                                                                Order = art.Order
                                                            })
                                                            .OrderBy(hl => hl.Order).ToList(),
                HLCats = _articleHighlightCatRepository.All.Where(a => a.Article.Categories.Any(c => c.Id == firstCat.Id || c.ParentId == firstCat.Id))
                                                            .Select(art => new ArticleHLCustomView()
                                                            {
                                                                Id = art.Id,
                                                                Name = art.Article.Title,
                                                                Series = art.Article.Series == null ? "" : art.Article.Series.Articles.Count() + " bài",
                                                                IsSeries = art.Article.Series == null ? false : true,
                                                                IsExpire = DbFunctions.TruncateTime(art.StartDate) <= now && DbFunctions.TruncateTime(art.EndDate) >= now,
                                                                Order = art.Order
                                                            })
                                                            .OrderBy(hl => hl.Order).ToList()
            };

            ViewBag.artCategory = artCategory;
            return View(model);
        }

        [RoleAuthorize(Site = "admin")]
        [HttpGet]
        public PartialViewResult GetHighlightByCat(int id)
        {
            var now = DateTime.Now.Date;
            var hlCat = _articleHighlightCatRepository.All.Where(hl => hl.Article.Categories.Any(c => c.Id == id || c.ParentId == id))
                                                        .Select(art => new ArticleHLCustomView()
                                                        {
                                                            Id = art.Id,
                                                            Name = art.Article.Title,
                                                            Series = art.Article.Series == null ? "" : art.Article.Series.Articles.Count() + " bài",
                                                            IsSeries = art.Article.Series == null ? false : true,
                                                            IsExpire = DbFunctions.TruncateTime(art.StartDate) <= now && DbFunctions.TruncateTime(art.EndDate) >= now,
                                                            Order = art.Order
                                                        })
                                                        .OrderBy(hl => hl.Order).ToList();
            return PartialView(hlCat);
        }

        [RoleAuthorize(Site = "admin")]
        public void UpdateRevision(int id)
        {
            var article = _articleRepository.Find(id);
            if (article != null)
            {
                var revs = _articleRevisionRepository.All.Where(ar => ar.ArticleId == article.Id);
                var lastRev = revs.OrderByDescending(ar => ar.ChangedDate).FirstOrDefault();
                if (lastRev == null || lastRev.ArticleContent != article.ArticleContent || lastRev.Title != article.Title || lastRev.ShortBrief != article.ShortBrief)
                {
                    //if (revs.Count() == 25)
                    //{
                    //    var firstRev = revs.OrderBy(ar => ar.ChangedDate).FirstOrDefault();
                    //    _articleRepository.Delete(firstRev.Id);
                    //    _articleRepository.Save();
                    //}

                    var rev = new ArticleRevision
                    {
                        ArticleId = article.Id,
                        ChangedDate = DateTime.Now,
                        Title = article.Title,
                        ShortBrief = article.ShortBrief,
                        ArticleContent = article.ArticleContent,
                        CreateUserId = User.Identity.GetUserId(),
                        //ArticleStatus = article.ArticleStatus
                    };

                    _articleRevisionRepository.InsertOrUpdate(rev);
                    _articleRevisionRepository.Save();
                }
            }
        }

        public ActionResult Revision(int id)
        {
            var revision1 = _articleRevisionRepository.Find(id);
            var article = revision1.Article;
            var revisions = article.ArticleRevisions.OrderBy(ar => ar.ChangedDate).ToList();
            var revision2 = revisions.LastOrDefault();

            HtmlDiff diffHelper = new HtmlDiff(revision1.ArticleContent, revision2.ArticleContent);
            string diffOutput = diffHelper.Build();


            var viewModel = new RevisionCompareView();

            viewModel.Revision1 = AutoMapper.Mapper.Map<RevisionView>(revision1);
            viewModel.Revision1.Order = revisions.IndexOf(revision1) + 1;

            viewModel.Revision2 = AutoMapper.Mapper.Map<RevisionView>(revision2);
            viewModel.Revision2.Order = revisions.Count();

            viewModel.DiffOutput = diffOutput;
            viewModel.Title = article.Title;
            viewModel.Count = revisions.Count();
            viewModel.ArticleId = article.Id;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CompareRevision(int articleid, int order1, int order2)
        {
            var article = _articleRepository.Find(articleid);
            var revisions = article.ArticleRevisions.OrderBy(ar => ar.ChangedDate).ToList();
            var revision1 = revisions[order1 - 1];
            var revision2 = revisions[order2 - 1];

            HtmlDiff diffHelper = new HtmlDiff(revision1.ArticleContent, revision2.ArticleContent);
            string diffOutput = diffHelper.Build();

            var viewModel = new RevisionCompareView();

            viewModel.Revision1 = AutoMapper.Mapper.Map<RevisionView>(revision1);
            viewModel.Revision1.Order = revisions.IndexOf(revision1) + 1;

            viewModel.Revision2 = AutoMapper.Mapper.Map<RevisionView>(revision2);
            viewModel.Revision2.Order = revisions.Count();

            return Json(new { source = new { content = revision1.ArticleContent, time = revision1.ChangedDate.ToString("dd/MM/yyyy hh:mm:ss") }, result = new { content = diffOutput, time = revision2.ChangedDate.ToString("dd/MM/yyyy hh:mm:ss") } });

        }

        //
        // GET: /Article/Create
        [RoleAuthorize(Site = "admin")]
        public ActionResult Create(int? seriesId)
        {
            var model = new ArticleModel() { 
                ArticleContent = ""
            };
            if (seriesId.HasValue)
            {
                model.ArticleSeriesId = seriesId.Value;
                model.SeriesName = _articleSeriesRepository.Find(seriesId.Value).Name;
            }

            //var qParam = Request.UrlReferrer != null ? HttpUtility.ParseQueryString(Request.UrlReferrer.Query) : null;
            //var subUrl = qParam == null || qParam.Count == 0 ? "?weektab=" + week : (qParam["weektab"] == null ? "&weektab=" + week : "");
            //ViewBag.returnUrl = Request.UrlReferrer + subUrl;
            ViewBag.PossibleReporters = _reporterRepository.All.ToList();
            ViewBag.PossibleKeywords = _keywordRepository.All;
            ViewBag.User = context.Users.Find(User.Identity.GetUserId());

            return View(model);
        }

        //
        // POST: /Article/Create
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase thumbnail, string returnUrl, ArticleModel articleModel)
        {
            if (thumbnail != null && thumbnail.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnail.FileName.Replace(' ', '-');
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/Article";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    thumbnail.SaveAs(filePath);
                }
                articleModel.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }
            else if (string.IsNullOrEmpty(articleModel.Thumbnail))
            {
                articleModel.Thumbnail = "/Images/NoImage.png";
            }

            ModelState.Clear();
            TryValidateModel(articleModel);

            if (ModelState.IsValid)
            {
                var article = AutoMapper.Mapper.Map<ArticleModel, Article>(articleModel);
                if (article.ArticleContent == null)
                    article.ArticleContent = "";
                _articleRepository.Load(article, v => v.Reporters);
                article.Reporters.Clear();
                if (!string.IsNullOrWhiteSpace(articleModel.ReporterIds))
                {
                    var reporters = articleModel.ReporterIds.Split(',');
                    foreach (string rpId in reporters)
                    {
                        if (!string.IsNullOrWhiteSpace(rpId))
                        {
                            var rpIdNormalize = rpId.Trim();
                            var reporter = _reporterRepository.Find(Convert.ToInt32(rpIdNormalize));
                            if (reporter != null)
                                article.Reporters.Add(reporter);
                        }
                    }
                }

                _articleRepository.Load(article, a => a.Keywords);
                article.Keywords.Clear();
                if (!string.IsNullOrWhiteSpace(articleModel.Keywords))
                {
                    var keywords = articleModel.Keywords.Split(',');
                    foreach (string kw in keywords)
                    {
                        if (!string.IsNullOrWhiteSpace(kw))
                        {
                            var kwNormalize = kw.Trim();
                            var keyword = _keywordRepository.FindValue(kwNormalize);
                            if (keyword == null)
                                keyword = new Keyword() { Value = kwNormalize };
                            article.Keywords.Add(keyword);
                        }
                    }
                }

                _articleRepository.Load(article, a => a.Categories);
                article.Categories.Clear();
                if (!string.IsNullOrWhiteSpace(articleModel.CategoryIds))
                {
                    var cats = articleModel.CategoryIds.Split(',');
                    foreach (string ct in cats)
                    {
                        var ctId = Convert.ToInt32(ct);
                        var cat = _articleCategoryRepository.Find(ctId);
                        if (cat != null)
                            article.Categories.Add(cat);
                    }
                }
                article.CreatedTime = DateTime.Now;

                if (articleModel.Published)
                {
                    article.IsPublished = true; 
                    article.ArticleStatus = (int)ArticleStatus.Good;
                    article.ArticleContent = NormalizeContent(articleModel.ArticleDraft);
                    article.PublishedTime = DateTime.Now;
                }
                else
                {
                    article.IsPublished = false; //Mac dinh la false
                    article.ArticleStatus = (int)ArticleStatus.NeedReview; //Mặc định bài đăng chưa được duyệt
                }
               
                if (article.ArticleSeriesId.HasValue)
                    article.SeriesOrder = _articleRepository.GetLastOrder(article.ArticleSeriesId.Value);
                _articleRepository.InsertOrUpdate(article);
                _articleRepository.Save();

                UpdateRevision(article.Id);

                return RedirectToAction("Management", "Article");
                //return Json(new { success = true, time = DateTime.Now.ToString("dd/MM/yyyy HH:mm"), id = article.Id, UniqueTitle = article.UniqueTitle, date = article.PublishedTime.ToString("dd-MM-yyyy") });
            }

            ViewBag.PossibleReporters = _reporterRepository.All.ToList();
            ViewBag.PossibleKeywords = _keywordRepository.All;
            ViewBag.User = context.Users.Find(User.Identity.GetUserId());
            return View(articleModel);
            
        }

        //
        // GET: /Article/Edit/5
        [RoleAuthorize(Site = "admin")]
        public ActionResult Edit(int id, string returnUrl)
        {
            //var qParam = Request.UrlReferrer != null ? HttpUtility.ParseQueryString(Request.UrlReferrer.Query) : null;
            //var subUrl = qParam == null || qParam.Count == 0 ? "?weektab=" + week : (qParam["weektab"] == null ? "&weektab=" + week : "");
            ViewBag.returnUrl = returnUrl;
            ViewBag.PossibleReporters = _reporterRepository.All.ToList();
            ViewBag.PossibleKeywords = _keywordRepository.All;
            //ViewBag.IsApprove = false;
            ViewBag.User = context.Users.Find(User.Identity.GetUserId());
            var article = _articleRepository.Find(id);
            ViewBag.Revisions = article.ArticleRevisions.ToList();
            //ViewBag.Comments = article.ArticleComments.ToList();
            return View(AutoMapper.Mapper.Map<Article, ArticleModel>(article));
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase thumbnail, string returnUrl, ArticleModel articleModel)
        {
            if (thumbnail != null && thumbnail.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnail.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/Article";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    thumbnail.SaveAs(filePath);
                }
                articleModel.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
            }
            else if (string.IsNullOrEmpty(articleModel.Thumbnail))
            {
                articleModel.Thumbnail = "/Images/NoImage.png";
            }

            ModelState.Clear();
            TryValidateModel(articleModel);

            if (ModelState.IsValid)
            {
                var article = AutoMapper.Mapper.Map<ArticleModel, Article>(articleModel);

                _articleRepository.Load(article, v => v.Reporters);
                article.Reporters.Clear();
                if (!string.IsNullOrWhiteSpace(articleModel.ReporterIds))
                {
                    var reporters = articleModel.ReporterIds.Split(',');
                    foreach (string rpId in reporters)
                    {
                        if (!string.IsNullOrWhiteSpace(rpId))
                        {
                            var rpIdNormalize = rpId.Trim();
                            var reporter = _reporterRepository.Find(Convert.ToInt32(rpIdNormalize));
                            if (reporter != null)
                                article.Reporters.Add(reporter);
                        }
                    }
                }

                _articleRepository.Load(article, a => a.Keywords);
                article.Keywords.Clear();
                if (!string.IsNullOrWhiteSpace(articleModel.Keywords))
                {
                    var keywords = articleModel.Keywords.Split(',');
                    foreach (string kw in keywords)
                    {
                        if (!string.IsNullOrWhiteSpace(kw))
                        {
                            var kwNormalize = kw.Trim();
                            var keyword = _keywordRepository.FindValue(kwNormalize);
                            if (keyword == null)
                                keyword = new Keyword() { Value = kwNormalize };
                            article.Keywords.Add(keyword);
                        }
                    }
                }

                _articleRepository.Load(article, a => a.Categories);
                article.Categories.Clear();
                if (!string.IsNullOrWhiteSpace(articleModel.CategoryIds))
                {
                    var cats = articleModel.CategoryIds.Split(',');
                    foreach (string ct in cats)
                    {
                        var ctId = Convert.ToInt32(ct);
                        var cat = _articleCategoryRepository.Find(ctId);
                        if (cat != null)
                            article.Categories.Add(cat);
                    }
                }
                
                if (User.IsInRole("Writter"))
                {
                    //Lưu nháp
                    //Lưu cần duyệt + phiên bản
                    if (articleModel.ArticleStatus == (int)ArticleStatus.NeedReview)
                    {
                        UpdateRevision(article.Id);
                    }

                }
                else if(User.IsInRole("Reviewer") || User.IsInRole("Publisher") || User.IsInRole("Admin"))
                {
                    //Lưu loại bỏ - Lưu cần chỉnh - Lưu tốt
                    //Lưu phiên bản
                    UpdateRevision(article.Id);
                }
                //Thay đổi trạng thái từ chưa/ngừng đăng sang đăng
                if (articleModel.Published && article.IsPublished == false && (User.IsInRole("Publisher") || User.IsInRole("Admin")))
                {
                    article.ArticleStatus = (int)ArticleStatus.Good;
                    article.IsPublished = true;
                    //Chuyển bản nháp qua bản chính thức
                    article.ArticleContent = NormalizeContent(articleModel.ArticleDraft);
                }
                //Thay đổi trạng thái từ đăng sang ngừng đăng
                else if (articleModel.Published == false && article.IsPublished && (User.IsInRole("Publisher") || User.IsInRole("Admin")))
                {
                    article.IsPublished = false;
                }
                    //Cập nhật bản đăng
                else if (articleModel.Published && article.IsPublished == true && (User.IsInRole("Publisher") || User.IsInRole("Admin")))
                {
                    article.ArticleContent = NormalizeContent(articleModel.ArticleDraft);
                }
                _articleRepository.InsertOrUpdate(article);
                _articleRepository.Save();

                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("ManagementTab", "Article");
                else
                    return Redirect(returnUrl);
                
                //return Json(new { success = true, time = DateTime.Now.ToString("dd/MM/yyyy HH:mm"), id = article.Id, UniqueTitle = article.UniqueTitle, date = article.PublishedTime.ToString("dd-MM-yyyy") });
            }

            if (articleModel.IsPublished)
            {
                ViewBag.PossibleReporters = _reporterRepository.All.ToList();
                ViewBag.PossibleKeywords = _keywordRepository.All;
                return View(articleModel);
            }
            else
                return Json(new { success = false });
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Approve(int id, int week = 0)
        {
            var qParam = Request.UrlReferrer != null ? HttpUtility.ParseQueryString(Request.UrlReferrer.Query) : null;
            var subUrl = qParam == null || qParam.Count == 0 ? "?weektab=" + week : (qParam["weektab"] == null ? "&weektab=" + week : "");
            ViewBag.returnUrl = Request.UrlReferrer + subUrl;
            ViewBag.PossibleReporters = _reporterRepository.All.ToList();
            ViewBag.PossibleKeywords = _keywordRepository.All;
            ViewBag.IsApprove = true;
            var article = _articleRepository.Find(id);
            return View(AutoMapper.Mapper.Map<Article, ArticleModel>(article));
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(HttpPostedFileBase thumbnail, string returnUrl, ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                var article = AutoMapper.Mapper.Map<ArticleModel, Article>(articleModel);

                _articleRepository.InsertOrUpdate(article);
                _articleRepository.Save();

                UpdateRevision(article.Id);

                return Redirect(returnUrl);
            }

            if (articleModel.IsPublished)
            {
                ViewBag.PossibleReporters = _reporterRepository.All.ToList();
                ViewBag.PossibleKeywords = _keywordRepository.All;
                return View(articleModel);
            }
            else
                return Json(new { success = false });
        }

        //
        // GET: /Article/Delete/5
        [RoleAuthorize(Site = "admin")]
        public ActionResult Delete(int id)
        {
            var qParam = Request.UrlReferrer != null ? HttpUtility.ParseQueryString(Request.UrlReferrer.Query) : null;
            var article = _articleRepository.Find(id);
            if (article != null)
                return View(article);
            else
                return HttpNotFound();
        }

        //
        // POST: /Article/Delete/5
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var article = _articleRepository.Find(id);
            if (article != null)
            {
                try
                {
                    using (new Impersonator("uploaduser", "", "Upload@@123"))
                    {
                        string thumbnailPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, article.Thumbnail);
                        if (System.IO.File.Exists(thumbnailPath))
                        {
                            System.IO.File.Delete(thumbnailPath);
                        }
                    }
                }
                catch { }
            }
            _articleRepository.Delete(id);
            _articleRepository.Save();
            if(string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/Article/ManagementTab";
            }
            return Redirect(returnUrl);
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult ChangeDate(int id)
        {
            var model = _articleRepository.Find(id);
            return PartialView("_ChangeDate", model);
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult ChangeDate(Article model, DateTime PublishedTimetxt)
        {
            var art = _articleRepository.Find(model.Id);
            art.PublishedTime = PublishedTimetxt;

            _articleRepository.InsertOrUpdate(art);
            _articleRepository.Save();
            return Json(new { success = true });
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult HighlightArticle(int id, int hlType)
        {
            ViewBag.Article = _articleRepository.Find(id);

            if (hlType == 0)
            {
                var model = _articleHighlightAllRepository.Find(id);
                if (model == null)
                    return PartialView("_HighlightArticleAll", new ArticleHighlightAll());
                else
                    return PartialView("_HighlightArticleAll", model);
            }
            else
            {
                var model = _articleHighlightCatRepository.Find(id);
                if (model == null)
                    return PartialView("_HighlightArticleCat", new ArticleHighlightCat());
                else
                    return PartialView("_HighlightArticleCat", model);
            }
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult HighlightArticleAll(ArticleHighlightAll model)
        {
            var chkHL = _articleHighlightAllRepository.All.Count();
            if (chkHL <= 60)
            {
                model.Order = _articleHighlightAllRepository.GetLastOrder();
                _articleHighlightAllRepository.InsertOrUpdate(model);
                _articleHighlightAllRepository.Save();
                return Json(new { success = true, id = model.Id, type = 0 });
            }
            else
                return Json(new { success = false });
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult HighlightArticleCat(ArticleHighlightCat model)
        {
            var chkHL = _articleHighlightCatRepository.All.Count();
            if (chkHL <= 60)
            {
                model.Order = _articleHighlightCatRepository.GetLastOrder();
                _articleHighlightCatRepository.InsertOrUpdate(model);
                _articleHighlightCatRepository.Save();
                return Json(new { success = true, id = model.Id, type = 1 });
            }
            else
                return Json(new { success = false });
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteHighlightArticle(int id, int hlType)
        {
            try
            {
                if (hlType == 0)
                {
                    _articleHighlightAllRepository.Delete(id);
                    _articleHighlightAllRepository.Save();
                }
                else
                {
                    _articleHighlightCatRepository.Delete(id);
                    _articleHighlightCatRepository.Save();
                }

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [Authorize, HttpPost]
        public ActionResult ChangeOrder(int? seriesId, DateTime? date, int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var article = _articleRepository.Find(id);
                if (article != null)
                {
                    if (seriesId.HasValue)
                        article.SeriesOrder = i;
                    else
                        article.Order = i;
                    _articleRepository.InsertOrUpdate(article);
                }
            }
            _articleRepository.Save();

            if (seriesId.HasValue)
                return RedirectToAction("Details", "ArticleSeries", new { id = seriesId });
            else
                return RedirectToAction("Management", new { date = date });
        }

        [Authorize, HttpPost]
        public ActionResult ChangeHLAOrder(int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var hl = _articleHighlightAllRepository.Find(id);
                if (hl != null)
                {
                    hl.Order = i;
                    _articleHighlightAllRepository.InsertOrUpdate(hl);
                }
            }
            _articleHighlightAllRepository.Save();

            return RedirectToAction("HighLightManagement");
        }

        [Authorize, HttpPost]
        public ActionResult ChangeHLCOrder(int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var hl = _articleHighlightCatRepository.Find(id);
                if (hl != null)
                {
                    hl.Order = i;
                    _articleHighlightCatRepository.InsertOrUpdate(hl);
                }
            }
            _articleHighlightCatRepository.Save();

            return RedirectToAction("HighLightManagement");
        }

        [Authorize]
        public ActionResult Review(int id)
        {
            var article = _articleRepository.Find(id);

            return View(article);
        }

        [Authorize]
        public ActionResult TextEditor(int id)
        {
            var article = _articleRepository.Find(id);

            return View(article);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveReview(int id, string content)
        {
            var articleReview = new ArticleReview
            {
                ArticleId = id,
                CreatedTime = DateTime.Now,
                ReviewContent = content
            };
            _articleRepository.InsertReview(articleReview);
            _articleRepository.Save();
            return Json(new { success = true });
        }
        private string NormalizeContent(string content)
        {
            var document = new HtmlDocument();
            var root = document.DocumentNode;
            root.InnerHtml = content;
            var spanNodes = document.DocumentNode.SelectNodes("//span");
            if (spanNodes != null)
            {
                foreach (var spanNode in spanNodes)
                {
                    if (spanNode.GetAttributeValue("class", "").Contains("comment"))
                        spanNode.ParentNode.RemoveChild(spanNode, true);
                }
            }
            return document.DocumentNode.OuterHtml;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _articleCategoryRepository.Dispose();
                _reporterRepository.Dispose();
                _articleRepository.Dispose();
                _keywordRepository.Dispose();
                _articleSeriesRepository.Dispose();
                _articleHighlightAllRepository.Dispose();
                _articleHighlightCatRepository.Dispose();
                _articleRevisionRepository.Dispose();
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

