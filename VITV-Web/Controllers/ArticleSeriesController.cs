using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.Helpers;
using System.Net;

namespace VITV_Web.Controllers
{
    public class ArticleSeriesController : Controller
    {
        private readonly IArticleSeriesRepository _articleSeriesRepository;

        public ArticleSeriesController()
            : this(new ArticleSeriesRepository())
        {
        }

        public ArticleSeriesController(IArticleSeriesRepository articleSeriesRepository)
        {
            _articleSeriesRepository = articleSeriesRepository;
        }

        public ViewResult Index(int id)
        {
            return View(_articleSeriesRepository.Find(id));
        }

        [RoleAuthorize(Site = "admin")]
        public ViewResult Management()
        {
            return View(_articleSeriesRepository.AllIncluding(art => art.Articles));
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var articleSeries = _articleSeriesRepository.Find(id.Value);
            if (articleSeries == null)
            {
                return HttpNotFound();
            }
            return View(articleSeries);
        }

        [RoleAuthorize(Site = "admin")]
        public JsonResult QuickCreate(string name)
        {
            var model = new ArticleSeries
            {
                Name = name,
                UniqueTitle = VITV.Web.Helpers.UrlHelper.URLFriendly(name),
                Order = _articleSeriesRepository.GetLastOrder(),
                ProfilePhoto = "/Images/NoImage.png"
            };

            _articleSeriesRepository.InsertOrUpdate(model);
            _articleSeriesRepository.Save();
            return Json(new { success = true, id = model.Id, name = model.Name }, JsonRequestBehavior.AllowGet);
        }

        [RoleAuthorize(Site = "admin")]
        public PartialViewResult Create()
        {
            return PartialView("_Create");
        }

        //
        // POST: /ArticleCategory/Create
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase profileFile, ArticleSeries articleSeries)
        {
            articleSeries.UniqueTitle = VITV.Web.Helpers.UrlHelper.URLFriendly(articleSeries.Name);
            if (profileFile != null && profileFile.ContentLength > 0)
            {
                string fileName = profileFile.FileName;
                //string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds).ToString() + "_" + profileFile.FileName;
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];

                    string catFolder = "UploadedImages/ArticleCategory";
                    string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + catFolder + @"\" + fileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        if (new FileInfo(filePath).Length != profileFile.ContentLength)
                        {
                            fileName = Guid.NewGuid().ToString() + profileFile.FileName;
                            profileFile.SaveAs(filePath);
                        }
                        //else use old file
                    }
                    else
                    {
                        profileFile.SaveAs(filePath);
                    }
                    articleSeries.ProfilePhoto = currentDomain + "/" + catFolder + "/" + fileName;
                }
                articleSeries.Order = _articleSeriesRepository.GetLastOrder();
                ModelState.Clear();
                TryValidateModel(articleSeries);
            }
            if (ModelState.IsValid)
            {
                _articleSeriesRepository.InsertOrUpdate(articleSeries);
                _articleSeriesRepository.Save();
                return RedirectToAction("Management");
            }
            return View();
        }

        //
        // GET: /ArticleCategory/Edit/5
        [RoleAuthorize(Site = "admin")]
        public ActionResult Edit(int id)
        {
            var model = _articleSeriesRepository.Find(id);
            return View(model);
        }

        //
        // POST: /ArticleCategory/Edit/5
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase profileFile, ArticleSeries articleSeries)
        {
            articleSeries.UniqueTitle = VITV.Web.Helpers.UrlHelper.URLFriendly(articleSeries.Name);
            if (profileFile != null && profileFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + profileFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string catFolder = "UploadedImages/ArticleCategory";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + catFolder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        if (new FileInfo(filePath).Length != profileFile.ContentLength)
                        {
                            fileName = Guid.NewGuid().ToString() + profileFile.FileName;
                        }
                    }
                    profileFile.SaveAs(filePath);
                }

                articleSeries.ProfilePhoto = currentDomain + "/" + catFolder + "/" + fileName;
            }
            else if (string.IsNullOrEmpty(articleSeries.ProfilePhoto))
            {
                articleSeries.ProfilePhoto = "/Images/NoImage.png";
            }

            ModelState.Clear();
            TryValidateModel(articleSeries);

            if (ModelState.IsValid)
            {
                _articleSeriesRepository.InsertOrUpdate(articleSeries);
                _articleSeriesRepository.Save();
                return RedirectToAction("Management");
            }
            return View(articleSeries);
        }

        //
        // GET: /ArticleCategory/Delete/5
        [RoleAuthorize(Site = "admin")]
        public ActionResult Delete(int id)
        {
            return View(_articleSeriesRepository.Find(id));
        }

        //
        // POST: /ArticleCategory/Delete/5
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var articleSeries = _articleSeriesRepository.Find(id);
            if (articleSeries != null)
            {
                try
                {
                    using (new Impersonator("uploaduser", "", "Upload@@123"))
                    {
                        string filePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, articleSeries.ProfilePhoto);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    articleSeries.Articles.Clear();
                }
                catch { }
            }
            _articleSeriesRepository.Delete(id);
            _articleSeriesRepository.Save();

            return RedirectToAction("Management");
        }

        [Authorize, HttpPost]
        public ActionResult ChangeOrder(int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var catArticle = _articleSeriesRepository.Find(id);
                if (catArticle != null)
                {
                    catArticle.Order = i;
                    _articleSeriesRepository.InsertOrUpdate(catArticle);
                }
            }
            _articleSeriesRepository.Save();
            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _articleSeriesRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}