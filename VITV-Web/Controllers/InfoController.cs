using System.Web.Mvc;
using VITV.Data.Models;
using VITV.Data.Repositories;

namespace VITV.Web.Controllers
{   
    public class InfoController : Controller
    {
		private readonly IInfoRepository _infoRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public InfoController() : this(new InfoRepository())
        {
        }

        public InfoController(IInfoRepository infoRepository)
        {
			_infoRepository = infoRepository;
        }

        //
        // GET: /Info/
        [Authorize(Roles = "Admin")]
        public ViewResult Index(string id="about")
        {
            var info = _infoRepository.Find(id);
            return View("Index", info);
        }

        [Authorize(Roles = "Admin")]
        public ViewResult Management()
        {
            return View(_infoRepository.All);
        }
        //
        // GET: /Info/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Info/Create
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Info info)
        {
            if (ModelState.IsValid) {
                _infoRepository.InsertOrUpdate(info);
                _infoRepository.Save();
                return RedirectToAction("Management");
            }
            return View();
        }

        //
        // GET: /Info/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
             return View(_infoRepository.Find(id));
        }

        //
        // POST: /Info/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Info info)
        {
            if (ModelState.IsValid) {
                _infoRepository.InsertOrUpdate(info);
                _infoRepository.Save();
                return RedirectToAction("Management");
            }
            return View();
        }

        //
        // GET: /Info/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            return View(_infoRepository.Find(id));
        }

        //
        // POST: /Info/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            _infoRepository.Delete(id);
            _infoRepository.Save();

            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _infoRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

