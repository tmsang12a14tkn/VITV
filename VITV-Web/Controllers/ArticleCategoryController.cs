using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.Helpers;

namespace VITV.Web.Controllers
{
    public class ArticleCategoryController : Controller
    {
        private readonly IArticleCategoryRepository _articlecategoryRepository;

        public ArticleCategoryController()
            : this(new ArticleCategoryRepository())
        {
        }

        public ArticleCategoryController(IArticleCategoryRepository articlecategoryRepository)
        {
            _articlecategoryRepository = articlecategoryRepository;
        }

        public ViewResult Index(int id)
        {
            return View(_articlecategoryRepository.Find(id));
        }

        [RoleAuthorize(Site = "admin")]
        public ViewResult Management()
        {
            return View(_articlecategoryRepository.AllIncluding(articlecategory => articlecategory.Articles));
        }

        [RoleAuthorize(Site = "admin")]
        public PartialViewResult Create()
        {
            return PartialView("_Create");
        }

        [RoleAuthorize(Site = "admin")]
        public PartialViewResult GetParentNode(int choicedCat)
        {
            var parents = _articlecategoryRepository.GetMany(atc => atc.ParentId == null || atc.ParentId == 0);
            ViewBag.curParentName = choicedCat != 0 ? _articlecategoryRepository.Find(choicedCat).Name : "Không có danh mục";
            return PartialView(parents);
        }

        [RoleAuthorize(Site = "admin")]
        public PartialViewResult GetParentCheckNode()
        {
            var parents = _articlecategoryRepository.GetMany(atc => atc.ParentId == null || atc.ParentId == 0);
            return PartialView(parents);
        }

        [RoleAuthorize(Site = "admin")]
        public PartialViewResult DrawChildNode(int pId)
        {
            var childs = _articlecategoryRepository.GetMany(atc => atc.ParentId == pId);
            return PartialView(childs);
        }

        [RoleAuthorize(Site = "admin")]
        public PartialViewResult DrawChildCheckNode(int pId)
        {
            var childs = _articlecategoryRepository.GetMany(atc => atc.ParentId == pId);
            return PartialView(childs);
        }

        [RoleAuthorize(Site = "admin")]
        public PartialViewResult DrawChildRowNode(int pId, int lvl)
        {
            var childs = _articlecategoryRepository.GetMany(atc => atc.ParentId == pId);
            ViewBag.nodeLevel = lvl;
            return PartialView(childs);
        }

        //
        // POST: /ArticleCategory/Create
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase profileFile, ArticleCategory articlecategory)
        {
            articlecategory.UniqueTitle = VITV.Web.Helpers.UrlHelper.URLFriendly(articlecategory.Name);
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
                    articlecategory.ProfilePhoto = currentDomain + "/" + catFolder + "/" + fileName;
                    ModelState.Clear();
                    TryValidateModel(articlecategory);
                }
            }
            if (ModelState.IsValid)
            {
                _articlecategoryRepository.InsertOrUpdate(articlecategory);
                _articlecategoryRepository.Save();
                return RedirectToAction("Management");
            }
            return View();
        }

        //
        // GET: /ArticleCategory/Edit/5
        [RoleAuthorize(Site = "admin")]
        public ActionResult Edit(int id)
        {
            var model = _articlecategoryRepository.Find(id);
            return View(model);
        }

        //
        // POST: /ArticleCategory/Edit/5
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase profileFile, ArticleCategory articlecategory)
        {
            articlecategory.UniqueTitle = VITV.Web.Helpers.UrlHelper.URLFriendly(articlecategory.Name);
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

                articlecategory.ProfilePhoto = currentDomain + "/" + catFolder + "/" + fileName;
            }
            else if (string.IsNullOrEmpty(articlecategory.ProfilePhoto))
            {
                articlecategory.ProfilePhoto = "/Images/NoImage.png";
            }

            ModelState.Clear();
            TryValidateModel(articlecategory);

            if (ModelState.IsValid)
            {
                _articlecategoryRepository.InsertOrUpdate(articlecategory);
                _articlecategoryRepository.Save();
                return RedirectToAction("Management");
            }
            return View(articlecategory);
        }

        //
        // GET: /ArticleCategory/Delete/5
        [RoleAuthorize(Site = "admin")]
        public ActionResult Delete(int id)
        {
            return View(_articlecategoryRepository.Find(id));
        }

        //
        // POST: /ArticleCategory/Delete/5
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var articleCat = _articlecategoryRepository.Find(id);
            if (articleCat != null)
            {
                try
                {
                    using (new Impersonator("uploaduser", "", "Upload@@123"))
                    {
                        string filePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, articleCat.ProfilePhoto);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                catch { }
            }
            _articlecategoryRepository.Delete(id);
            _articlecategoryRepository.Save();

            return RedirectToAction("Management");
        }

        [Authorize, HttpPost]
        public ActionResult ChangeOrder(int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var catArticle = _articlecategoryRepository.Find(id);
                if (catArticle != null)
                {
                    catArticle.Order = i;
                    _articlecategoryRepository.InsertOrUpdate(catArticle);
                }
            }
            _articlecategoryRepository.Save();
            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _articlecategoryRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

