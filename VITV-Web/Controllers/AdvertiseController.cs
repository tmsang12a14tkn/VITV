using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VITV_Web.Controllers
{
    public class AdvertiseController : Controller
    {
        // GET: Advertise/Management
        public ActionResult Management()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Edit()
        //{
        //    return View();
        //}
        public ActionResult Create()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Create()
        //{
        //    return View();
        //}
    }
}