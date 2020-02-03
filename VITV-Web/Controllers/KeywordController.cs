using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
using VITV.Data.Models;
using VITV.Data.Repositories;

namespace VITV.Web.Controllers
{
    public class KeywordController : Controller
    {
        private readonly IKeywordRepository _keywordRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public KeywordController()
            : this(new KeywordRepository())
        {
        }

        public KeywordController(IKeywordRepository keywordRepository)
        {
            _keywordRepository = keywordRepository;
        }

        [Authorize(Roles = "Admin")]
        public ViewResult Index()
        {
            return View(_keywordRepository.AllIncluding(keyword => keyword.Videos, keyword => keyword.Articles));
        }

        [Authorize(Roles = "Admin")]
        public ViewResult Details(int id)
        {
            return View(_keywordRepository.Find(id));
        }

        [Authorize(Roles = "Admin")]
        public ViewResult Management()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public JsonResult GetKeywords()
        {
            var keywords = _keywordRepository.AllIncluding(keyword => keyword.Videos, keyword => keyword.Articles)
                                            .Select(keyword => new
                                            {
                                                Id = keyword.Id,
                                                Value = keyword.Value,
                                                VideoCount = keyword.Videos.Count,
                                                ArticleCount = keyword.Articles.Count 
                                            }).ToList();
            var total = keywords.Count;
            return Json(new { keywords, total }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Keyword/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Keyword/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Keyword keyword)
        {
            if (ModelState.IsValid)
            {
                _keywordRepository.InsertOrUpdate(keyword);
                _keywordRepository.Save();
                return RedirectToAction("Management");
            }
            return View();
        }

        //
        // GET: /Keyword/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View(_keywordRepository.Find(id));
        }

        //
        // POST: /Keyword/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Keyword keyword)
        {
            if (ModelState.IsValid)
            {
                _keywordRepository.InsertOrUpdate(keyword);
                _keywordRepository.Save();
                return RedirectToAction("Management");
            }
            return View();
        }

        //
        // GET: /Keyword/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View(_keywordRepository.Find(id));
        }

        //
        // POST: /Keyword/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _keywordRepository.Delete(id);
            _keywordRepository.Save();

            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _keywordRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

