using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;

namespace VITV_Web.Controllers
{
    public class SettingController : Controller
    {
        private readonly ISettingRepository _settingRepository;

        public SettingController()
        {
            var context = new VITVContext();
            _settingRepository = new SettingRepository(context);
        }
        // GET: Setting
        public ActionResult Video()
        {
            var videoSetting = new VideoSetting
            {
                AdvertiserOrder = _settingRepository.GetValue("AdvertiserOrder"),
                SkipTime = _settingRepository.GetValue("SkipTime"),
            };

            return View(videoSetting);
        }
        [HttpPost]
        public ActionResult Video(VideoSetting videoSetting)
        {
            var adOrderSetting = new Setting()
            {
                Name = "AdvertiseOrder",
                Value = videoSetting.AdvertiserOrder
            };
            _settingRepository.InsertOrUpdate(adOrderSetting);

            var skipTimeSetting = new Setting()
            {
                Name = "SkipTime",
                Value = videoSetting.SkipTime
            };
            _settingRepository.InsertOrUpdate(skipTimeSetting);

            _settingRepository.Save();

            return View(videoSetting);
        }
    }
}