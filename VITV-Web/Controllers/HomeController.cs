using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Helpers;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;

namespace VITV.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVideoCategoryRepository _videoCategoryRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IInfoRepository _infoRepository;
        private readonly IVNDExchangeRateRepository _vndExchangeRateRepository;
        private readonly IVideoCatGroupRepository _videoCatGroupRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly ISpecialEventRepository _specialEventRepository;
        private readonly IHolidayRepository _holidayRepository;
        public HomeController()
        {
            var context = new VITVContext();
            _videoCategoryRepository = new VideoCategoryRepository(context);
            _articleCategoryRepository = new ArticleCategoryRepository(context);
            _videoRepository = new VideoRepository(context);
            _articleRepository = new ArticleRepository(context);
            _infoRepository = new InfoRepository(context);
            _vndExchangeRateRepository = new VNDExchangeRateRepository(context);
            _videoCatGroupRepository = new VideoCatGroupRepository(context);
            _partnerRepository = new PartnerRepository(context);
            _specialEventRepository = new SpecialEventRepository(context);
            _holidayRepository = new HolidayRepository(context);
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var now = DateTime.Now;
            var specialEvent = _specialEventRepository.GetMany(e => e.StartDate <= now && e.EndDate >= now).FirstOrDefault();
            var homeModel = new HomeModel
            {
                //&& v.PublishedTime >= v.SpecialEvent.StartDate &&  v.PublishedTime <= v.SpecialEvent.EndDate
                //HotVideos = _videoRepository.Take(20).ToList(),
                //HotArticles = _articleRepository.All.OrderByDescending(art => art.PublishedTime).Take(4).ToList(),
                //ShortIntro = _infoRepository.GetInfo("shortintro").Description,
                RateAndPrices = GetRateAndPrices(),
                Partners = _partnerRepository.All.ToList(),
                VideoCatGroups = _videoCatGroupRepository.All.Select(g => new VideoCatGroupView { Id = g.Id, Name = g.Name, UniqueTitle = g.UniqueTitle }).ToList(),
                SpecialEvent = AutoMapper.Mapper.Map<SpecialEventView>(specialEvent)
            };

            foreach (var catGroup in homeModel.VideoCatGroups)
            {
                catGroup.NewestVideos = _videoRepository.GetMany(v => v.Category.GroupId == catGroup.Id).GroupBy(v => v.CategoryId)
                                                        .Select(g => g.Where(p => p.IsPublished && p.PublishedTime <= DateTime.Now).OrderByDescending(p => p.DisplayTime).FirstOrDefault())
                                                        .OrderBy(v => v.Category.Order).ToList();
            }

            //var videos = _videoRepository.All.ToList();
            //foreach (var v in videos)
            //{
            //    using (WebClient webClient = new WebClient())
            //    {
            //        byte[] data = webClient.DownloadData(v.Thumbnail);
            //        using (MemoryStream mem = new MemoryStream(data))
            //        {
            //            using (var img = Image.FromStream(mem))
            //            {
            //                v.VideoWidth = img.Width;
            //                v.VideoHeight = img.Height;
            //            }
            //        }
            //    }
            //    _videoRepository.InsertOrUpdate(v);
            //    _videoRepository.Save();
            //}

            return View(homeModel);
        }

        public ActionResult AnalyticPages(string url, string thumbnail, string title, string os, string dvModel, string dvType, string dvVendor, int? vId, int? vCatId, int? aId, int? aCatId)
        {
            if ((string.IsNullOrEmpty(dvModel) || dvModel == "undefined") && (os == "Mac OS" || os == "Windows")) dvModel = "Desktop";
            if ((string.IsNullOrEmpty(dvType) || dvType == "undefined") && (os == "Mac OS" || os == "Windows")) dvType = "desktop";

            if ((string.IsNullOrEmpty(os) || dvModel == "undefined") && dvType == "desktop" && os == "Mac OS") dvVendor = "Apple";
            if ((string.IsNullOrEmpty(os) || dvModel == "undefined") && dvType == "desktop" && os == "Windows") dvVendor = "Microsoft";

            _videoRepository.UpdateAccessCount(url, thumbnail, title, Request.UserHostAddress, os, dvModel, dvType, dvVendor, vId, vCatId, aId, aCatId);

            return Content(null);
        }

        public ActionResult GetMenu()
        {
            var model = _videoCatGroupRepository.AllIncluding(v => v.VideoCats).OrderBy(v => v.Order).ToList();
            return PartialView("_Menu", model);
        }

        public ActionResult GetHoliday()
        {
            var now = DateTime.Now;
            var nowDate = now.Date;
            var nowVal = now.Month * 100 + now.Day;
            var holiday = _holidayRepository.All.FirstOrDefault(h => (h.RepeatYear == true && h.StartDate.Month * 100 + h.StartDate.Day <= nowVal && h.EndDate.Month * 100 + h.EndDate.Day >= nowVal)
                                                                    || (h.RepeatYear == false && DbFunctions.TruncateTime(h.StartDate) <= nowDate && DbFunctions.TruncateTime(h.EndDate) >= nowDate));
            return PartialView("_Holiday", holiday);
        }

        public ViewResult Search(string cat, string query)
        {
            var model = new SearchModel
            {
                Query = query,
                Articles = _articleRepository.GetMany(a => a.Title.Contains(query), a => a.Order),
                Videos = _videoRepository.GetMany(v => v.Title.Contains(query)).Take(100).ToList()
            };
            return View(model);
        }

        public ActionResult About()
        {
            var info = _infoRepository.GetInfo("msg");
            return View(info);
        }

        //[Authorize(Roles = "Admin, Mod")]
        [HttpPost]
        public ActionResult FroalaUploadImage(List<HttpPostedFileBase> files)
        {

            List<string> links = new List<string>();
            foreach (HttpPostedFileBase file in files)
            {
                //var fileName = Path.GetFileName(file.FileName);
                //var rootPath = Server.MapPath("~/UploadedImages/Froala/");
                //if (fileName != null)
                //{
                //    file.SaveAs(Path.Combine(rootPath, fileName));
                //}
                //links.Add("/UploadedImages/Froala/" + fileName);

                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + file.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/Article";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    file.SaveAs(filePath);
                }
                links.Add(currentDomain + "/" + folder + "/" + fileName);
            }
            return Json(new { links = links }, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "Admin, Mod")]
        public ActionResult FroalaMediaManager()
        {
            var imagesList = new List<string>();
            //var rootPath = Server.MapPath("~/UploadedImages/Froala/");
            string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
            var rootPath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + "/UploadedImages/Article/";

            DirectoryInfo Folder = new DirectoryInfo(rootPath);
            FileInfo[] Images = Folder.GetFiles();

            for (int i = 0; i < Images.Length; i++)
            {
                imagesList.Add(rootPath + Images[i].Name);
            }

            return Json(imagesList, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, Mod")]
        [HttpPost]
        public ActionResult FroalaDelete(string src)
        {
            var fullPath = Server.MapPath(src);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            return new EmptyResult();
        }

        public PartialViewResult MarketInfo()
        {
            RateAndPrices model = GetRateAndPrices();
            return PartialView("_MarketInfo", model);
        }

        //TODO: Chuyển sang file web config
        private RateAndPrices GetRateAndPrices()
        {
            var context = new StoreInfoContext();
            var result = new RateAndPrices();
            //result.LocalGolds = new List<ReuterIndex> { 
            //    _reuterIndexRepository.GetByCode("SJC=")
            //};

            //result.GlobalGolds = new List<ReuterIndex> { 
            //    _reuterIndexRepository.GetByCode("XAU=")
            //};

            //result.Oils = new List<ReuterIndex> { 
            //    _reuterIndexRepository.GetByCode(".NAT_GAS"),
            //    _reuterIndexRepository.GetByCode(".OIL"),
            //    _reuterIndexRepository.GetByCode(".BRENT"),
            //    _reuterIndexRepository.GetByCode(".GASOLINE"),
            //};
            //result.Metals = new List<ReuterIndex> { 
            //    _reuterIndexRepository.GetByCode("XAG="),
            //    _reuterIndexRepository.GetByCode("XPT="),
            //};


            //result.VNDRates = _vndExchangeRateRepository.GetLastRates();

            //var usdcodes = new List<string> { "EUR=", "INR=", "AUD=", "GBP=" };

            //result.GoldVNs = context.GoldPriceVietNams.Where(g => g.IsShow == 1).OrderBy(g => g.Order).ToList();
            //result.GoldWorlds = context.GoldPriceWorlds.OrderBy(e => e.Order).ToList();
            //result.Metals = context.MetalPrices.Where(e => e.Order > 0).OrderBy(e => e.Order).ToList();
            //result.Oils = context.OilPrices.OrderBy(e => e.Order).ToList();
            //result.USDRates = context.USDExchangeRates.OrderBy(e => e.Order).ToList();
            //result.VNDRates = context.VNDExchangeRates.OrderBy(e => e.Order).ToList();
            //result.WorldStocks = context.StockWorlds.ToList();
            //result.VNStocks = context.StockMarket_RealTimes.Where(s => s.IsShown).OrderBy(s => s.Order).ToList();

            //usdcodes.ForEach(code => result.USDRates.Add(_reuterIndexRepository.GetByCode(code)));

            //var stockcodes = new List<string> { ".HNXI", ".VNI", ".DJI", ".IXIC", ".GSPC", "WTC-", "/.FCHI", "/.STOXX", "/.FTSE", ".N225", "/.HSI", "/.CSI300" };
            //result.GlobalStockList = new List<ReuterIndex>();
            //stockcodes.ForEach(code => result.GlobalStockList.Add(_reuterIndexRepository.GetByCode(code)));

            //result.VnMarketIndices = _vnMarketIndexRepository.GetLastIndices();

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _videoCategoryRepository.Dispose();
                _articleCategoryRepository.Dispose();
                _articleRepository.Dispose();
                _infoRepository.Dispose();
                _videoCatGroupRepository.Dispose();
                _videoRepository.Dispose();
                _vndExchangeRateRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}