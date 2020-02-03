using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Web.Helpers;

namespace VITV.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BackgroundController : Controller
    {
        // GET: Background
        public ActionResult Management()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Management(HttpPostedFileBase liveStreamBkgr, HttpPostedFileBase vodBkgr)
        {
            if (liveStreamBkgr != null && liveStreamBkgr.ContentLength > 0)
            {
                string folder = Server.MapPath("~/Content/Images/");
                string filePath = Path.Combine(folder,"stream_bg.jpg");
                string backupFilePath = Path.Combine(folder, "stream_bg_"+ Guid.NewGuid().ToString() +".jpg");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Move(filePath, backupFilePath);
                }
                liveStreamBkgr.SaveAs(filePath);
            }

            if (vodBkgr != null && vodBkgr.ContentLength > 0)
            {
                string folder = Server.MapPath("~/Content/Images/");
                string filePath = Path.Combine(folder, "player_bg.jpg");
                string backupFilePath = Path.Combine(folder, "player_bg_" + Guid.NewGuid().ToString() + ".jpg");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Move(filePath, backupFilePath);
                }
                vodBkgr.SaveAs(filePath);
            }

            ViewBag.Message = "Thay đổi thành công";
            return View();
        }
    }
}