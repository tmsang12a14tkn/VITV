using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.ViewModels;

namespace VITV.HRM.Controllers
{
    public class BranchController : Controller
    {
        private VITVSecondContext db = new VITVSecondContext();

        // GET: Branch
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll() 
        {
            var data = new ArrayList();
            var model = db.Branches;

            foreach (var b in model)
            {
                var item = new TreeModel()
                {
                    name = b.Name,
                    type = "folder",
                    id = "branch-" + b.Id
                };

                data.Add(item);
            }

            var addRow = new TreeModel()
            {
                name = "Thêm chi nhánh",
                type = "button",
                id = "add-branch"
            };
            data.Add(addRow);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Branch/Create
        public PartialViewResult Create()
        {
            return PartialView("~/Views/Branch/_addBranch.cshtml");
        }

        // POST: Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Branch branch)
        {
            if (ModelState.IsValid)
            {
                db.Branches.Add(branch);
                db.SaveChanges();
                return RedirectToAction("Index", "Department");
            }

            return PartialView("~/Views/Branch/_addBranch.cshtml", branch);
        }
    }
}