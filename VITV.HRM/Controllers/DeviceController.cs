using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;

namespace VITV.HRM.Controllers
{
    public class DeviceController : Controller
    {
        // GET: Device
        [HttpPost]
        public ActionResult Register(string devicetoken, string deviceid)
        {
            var context = new VITVSecondContext();
            var device = new DeviceUser { 
                UserId = "35d16a0d-7790-4280-9288-679645c37d71",
                DeviceId = deviceid,
                DeviceType = 1,
                DeviceToken = devicetoken,
                ExpiredTime = DateTime.Now,
                Logged = true
            };
            context.DeviceUsers.Add(device);
            context.SaveChanges();

            return Json(new { });
        }
    }
}