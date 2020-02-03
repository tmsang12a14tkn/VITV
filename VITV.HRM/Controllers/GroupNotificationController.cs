using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;

namespace VITV.HRM.Controllers
{
    public class GroupNotificationController : Controller
    {
        [HttpGet]
        public ActionResult Add(int groupId)
        {
            return PartialView(groupId);
        }
        // GET: Notification
        [HttpPost]
        public JsonResult Add(GroupNotification notifi)
        {
            var db = new VITVSecondContext();
            db.GroupNotifications.Add(notifi);
            db.SaveChanges();
            return Json(new { success = true});
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new VITVSecondContext();
            var noti = db.GroupNotifications.Find(id);
            return PartialView(noti);
        }
        [HttpPost]
        public JsonResult Edit(GroupNotification notifi)
        {
            var db = new VITVSecondContext();
            db.Entry(notifi).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
             return Json(new { success = true});
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new VITVSecondContext();
            var noti = db.GroupNotifications.Find(id);
            return PartialView(noti);
        }
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirm(int id)
        {
            var db = new VITVSecondContext();
            var noti = db.GroupNotifications.Find(id);
            db.GroupNotifications.Remove(noti);
            db.SaveChanges();
            return Json(new { success = true });
        }


    }
}