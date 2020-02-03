using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VITV.Portal.Controllers
{
    public class Result
    {
        
            public int Id { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }
            public string Operate { get; set; }
        
    }
    public class ReportVideoAccessController : Controller
    {
        // GET: ReportVideoAccess
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData()
        {
            var data = new Result[] { 
                new Result{Id = 1, Name = "1", Price = "1", Operate = "1"},
                new Result{Id = 2, Name = "2", Price = "2", Operate = "1"},
                new Result{Id = 3, Name = "3", Price = "3", Operate = "1"},
                new Result{Id = 4, Name = "4", Price = "4", Operate = "1"},
                new Result{Id = 5, Name = "5", Price = "5", Operate = "1"},
                new Result{Id = 6, Name = "6", Price = "6", Operate = "1"},
                new Result{Id = 7, Name = "7", Price = "7", Operate = "1"},
                new Result{Id = 8, Name = "8", Price = "8", Operate = "1"},
                new Result{Id = 9, Name = "9", Price = "9", Operate = "1"},
                new Result{Id = 10, Name = "10", Price = "10"},
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}