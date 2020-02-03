using System.Web.Mvc;
using VITV.Data.Models;
using VITV.Data.Repositories;

namespace VITV.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public ProductController()
            : this(new ProductRepository())
        {
        }

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //
        // GET: /Product/
        [Authorize(Roles = "Admin")]
        public ViewResult Index()
        {
            return View(_productRepository.All);
        }

        //
        // GET: /Product/Details/5

        public ViewResult Details(int id)
        {
            return View(_productRepository.Find(id));
        }
        [Authorize(Roles = "Admin")]
        public ViewResult Management()
        {
            return View(_productRepository.All);
        }
        //
        // GET: /Product/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Product/Create
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.InsertOrUpdate(product);
                _productRepository.Save();
                return RedirectToAction("Management");
            }
            return View(product);
        }

        //
        // GET: /Product/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View(_productRepository.Find(id));
        }

        //
        // POST: /Product/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.InsertOrUpdate(product);
                _productRepository.Save();
                return RedirectToAction("Management");
            }
            return View(product);
        }

        //
        // GET: /Product/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View(_productRepository.Find(id));
        }

        //
        // POST: /Product/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            _productRepository.Save();

            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

