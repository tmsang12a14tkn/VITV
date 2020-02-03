using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using VITV.Data.DAL;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Models.GoogleAnalytics;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;
using VITV.Data.ViewModels.Portal;
using VITV.Portal.Helpers;
using VITV.Portal.ViewModel;

namespace VITV.Web.Controllers
{
    [Authorize]
    public class VideoAccessController : Controller
    {
        // GET: VideoAccess
        private readonly IVideoRepository _videoRepository;
        private readonly IVideoCategoryRepository _videoCatRepository;
        private readonly IPageRepository _pageCatRepository;
        private readonly IProgramScheduleDetailRepository _programScheduleDetailRepository;
        private readonly IProgramScheduleRepository _programScheduleRepository;
        public VideoAccessController()
        {
            _videoRepository = new VideoRepository();
            _videoCatRepository = new VideoCategoryRepository();
            _pageCatRepository = new PageRepository();
            _programScheduleDetailRepository = new ProgramScheduleDetailRepository();
            _programScheduleRepository = new ProgramScheduleRepository();
        }
         [RoleAuthorize(Site = "portal")]
        public ActionResult Index()
        {
            return View();
        }

        [RoleAuthorize(Site = "portal")]
        public ActionResult Report(DateTime? from, DateTime? to, string dateType = "week")
        {
            DateTime d = DateTime.Today;

            //Mặc định tuần
            if (!from.HasValue || !to.HasValue)
            {
                int offset = d.DayOfWeek - DayOfWeek.Monday;
                DateTime monday = d.AddDays(-offset);
                from = monday;
                to = monday.AddDays(6);
                dateType = "week";
            }

            var periods = _videoRepository.GetAccessDataBetween(from.Value, to.Value);
            var periodsNotCombine = _videoRepository.GetAccessNotCombineDataBetween(from.Value, to.Value);
            var catCombines = _videoRepository.GetCatCombineList(from.Value, to.Value);

            var data = new Dictionary<CatCombineView, List<PeriodVideoAccess>>();
            foreach (var catCombine in catCombines)
            {
                data.Add(catCombine, _videoRepository.GetCatCombineDataByParentBetween(catCombine, from.Value, to.Value));
            }

            ViewBag.data = data;
            ViewBag.periodsNotCombine = periodsNotCombine;
            ViewBag.dateType = dateType;
            ViewBag.fromDate = from.Value.Date.ToString("dd/MM/yyyy");
            ViewBag.toDate = to.Value.Date.ToString("dd/MM/yyyy");

            return View(periods);
        }

        [HttpGet]
        public ActionResult GetItemCombineReport(int id, DateTime? from, DateTime? to, string dateType = "week")
        {
            //string data = Utilities.RenderRazorViewToString(this, "/Views/VideoAccess/GetItemCombineReport.cshtml", null);
            ViewBag.dateType = dateType;
            ViewBag.fromDate = from != null ?  from.Value.Date.ToString("dd/MM/yyyy") : "";
            ViewBag.toDate = to != null ? to.Value.Date.ToString("dd/MM/yyyy") : "";
            var catvideo = _videoCatRepository.Find(id);
            ViewBag.image = catvideo == null ? "" : catvideo.ProfilePhoto;
            return PartialView(id);
        }

        [HttpPost]
        public ActionResult GetItemCombineReportPost(int id, DateTime? from, DateTime? to, string dateType = "year")
        {
            DateTime d = DateTime.Today;

            var itemCombines = _videoRepository.GetCatCombineDataByParentBetween(new CatCombineView() { Id = id }, from.Value, to.Value);
            var videoCat = _videoCatRepository.Find(id);

            return Json(new { data = itemCombines, name = videoCat.Name });
        }

        [RoleAuthorize(Site = "portal")]
        public ActionResult VideoCatReportByType(DateTime? from, DateTime? to, string dateType)
        {
            DateTime d = DateTime.Today;
            //Mặc định tuần
            if(!from.HasValue||!to.HasValue)
            {
                int offset = d.DayOfWeek - DayOfWeek.Monday;
                DateTime monday = d.AddDays(-offset);
                from = monday;
                to = monday.AddDays(6);
                dateType = "week";
            }


            var catTypes = _videoRepository.CatTypeList();
            var data = new Dictionary<VideoCatType, List<PeriodVideoAccess>>();
            foreach (var catType in catTypes)
            {
                data.Add(catType, _videoRepository.GetCatAccessDataByTypeBetween(catType.Id, from.Value, to.Value));
            }

            ViewBag.dateType = dateType;
            ViewBag.fromDate = from.Value.Date.ToString("dd/MM/yyyy");
            ViewBag.toDate = to.Value.Date.ToString("dd/MM/yyyy");

            return View(data);
        }

        [RoleAuthorize(Site = "portal")]
        public ActionResult VideoReportByType(DateTime? from, DateTime? to, string dateType = "week")
        {
            DateTime d = DateTime.Today;
            //Mặc định tuần
            if (!from.HasValue || !to.HasValue)
            {
                int offset = d.DayOfWeek - DayOfWeek.Monday;
                DateTime monday = d.AddDays(-offset);
                from = monday;
                to = monday.AddDays(6);
                dateType = "week";
            }
     
            var catTypes = _videoRepository.CatTypeList();
            var data = new Dictionary<VideoCatType, List<PeriodVideoAccess>>();
            foreach (var catType in catTypes)
            {
                data.Add(catType, _videoRepository.GetAccessDataByTypeBetween(catType.Id, from.Value, to.Value));
            }

            ViewBag.dateType = dateType;
            ViewBag.fromDate = from.Value.Date.ToString("dd/MM/yyyy");
            ViewBag.toDate = to.Value.Date.ToString("dd/MM/yyyy");

            return View(data);
        }

        public ActionResult VideoReportByCat(int catid, DateTime? from, DateTime? to, string dateType = "week")
        {
            DateTime d = DateTime.Today;
            if (!to.HasValue)
            {
                if (dateType == "day")
                {
                    to = from.Value;
                }
                else if (dateType == "week")
                {
                    to = from.Value.AddDays(6);
                }
                else if (dateType == "month")
                {
                    to = from.Value.AddMonths(1).AddDays(-1);
                }
                else if (dateType == "year")
                {
                    to = from.Value.AddYears(1).AddDays(-1);
                }
            }
            var data = _videoRepository.GetAccessDataByCatBetween(catid, from.Value, to.Value);

            ViewBag.catId = catid;
            ViewBag.catName = _videoCatRepository.Find(catid).Name;
            ViewBag.dateType = dateType;
            ViewBag.fromDate = from.Value.Date.ToString("dd/MM/yyyy");
            ViewBag.toDate = to.Value.Date.ToString("dd/MM/yyyy");

            return View(data);
        }

        public JsonResult GetDateAccessData(double? max, double? min)
        {
            DateTime startDate, endDate;
            if (!max.HasValue || !min.HasValue)
            {
                startDate = DateTime.Now.Date.AddYears(-5);
                endDate = DateTime.Now.Date;
            }
            else
            {
                startDate = UnixTimeStampToDateTime(min.Value).Date;
                endDate = UnixTimeStampToDateTime(max.Value).Date;
            }
            var data = _videoRepository.GetDateAccessData(startDate, endDate).Select(ac => new ArrayList { ToUnixTimestamp(ac.Date), ac.IPCount, ac.IPViewCount, ac.PageViewCount, ac.HomePageCount });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWeekAccessData(double? max, double? min)
        {
            DateTime startDate, endDate;
            if (!max.HasValue || !min.HasValue)
            {
                startDate = DateTime.Now.Date.AddYears(-5);
                endDate = DateTime.Now.Date;
            }
            else
            {
                startDate = UnixTimeStampToDateTime(min.Value).Date;
                endDate = UnixTimeStampToDateTime(max.Value).Date;
            }
            var data = _videoRepository.GetWeekAccessData(startDate, endDate).Select(ac => new ArrayList { ToUnixTimestamp(ac.Date), ac.IPCount, ac.IPViewCount, ac.PageViewCount, ac.HomePageCount });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonthAccessData(double? max, double? min)
        {
            DateTime startDate, endDate;
            if (!max.HasValue || !min.HasValue)
            {
                startDate = DateTime.Now.Date.AddYears(-5);
                endDate = DateTime.Now.Date;
            }
            else
            {
                startDate = UnixTimeStampToDateTime(min.Value).Date;
                endDate = UnixTimeStampToDateTime(max.Value).Date;
            }
            var data = _videoRepository.GetMonthAccessData(startDate, endDate).Select(ac => new ArrayList { ToUnixTimestamp(ac.Date), ac.IPCount, ac.IPViewCount, ac.PageViewCount, ac.HomePageCount });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetYearAccessData(double? max, double? min)
        {
            DateTime startDate, endDate;
            if (!max.HasValue || !min.HasValue)
            {
                startDate = DateTime.Now.Date.AddYears(-5);
                endDate = DateTime.Now.Date;
            }
            else
            {
                startDate = UnixTimeStampToDateTime(min.Value).Date;
                endDate = UnixTimeStampToDateTime(max.Value).Date;
            }
            var data = _videoRepository.GetYearAccessData(startDate, endDate).Select(ac => new ArrayList { ToUnixTimestamp(ac.Date), ac.IPCount, ac.IPViewCount, ac.PageViewCount, ac.HomePageCount });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllCatAccessDataGroupBy( string groupBy,DateTime startDate, DateTime endDate, string catId) //lay so lieu de hien thi tren bieu do so sanh cac chuyen muc
        {
            CatAccessDataChart data = new CatAccessDataChart();
            if(groupBy=="day")
                data = _videoRepository.GetAllCatAccessDataByDate(startDate, endDate, catId);
            else if (groupBy == "week")
                data = _videoRepository.GetAllCatAccessDataByWeek(startDate, endDate, catId);
            else if (groupBy == "month")
                data = _videoRepository.GetAllCatAccessDataByMonth(startDate, endDate, catId);
            else if (groupBy == "quarter")
                data = _videoRepository.GetAllCatAccessDataByQuarter(startDate, endDate, catId);
            else if (groupBy == "year")
                data = _videoRepository.GetAllCatAccessDataByYear(startDate, endDate, catId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [RoleAuthorize(Site = "portal")]
        //Báo cáo theo loại thiết bị
        public ActionResult DeviceReportByDate(DateTime? from, DateTime? to, string dateType = "week")
        {
            DateTime d = DateTime.Today;
            if(!to.HasValue) to = d;
            if (!from.HasValue)
                from = to.Value.AddDays(-7);
        
            //var model = _videoRepository.GetDeviceAccessDataByDate(from.Value, to.Value);

            ViewBag.dateType = dateType;
            ViewBag.fromDate = from.Value.Date.ToString("dd/MM/yyyy");
            ViewBag.toDate = to.Value.Date.ToString("dd/MM/yyyy");

            return View();
        }

        public JsonResult GetDeviceReportByDate(DateTime from, DateTime to)
        {
            var result = _videoRepository.GetDeviceAccessDataByDate(from, to);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetAccessDataInADate(DateTime date)
        {
            VideoAccessDateView model = new VideoAccessDateView
            {
                AccessData = _videoRepository.GetAccessDataInADate(date),
                Date = date,
                PeriodType = PeriodType.Daily,
                GroupByType = GroupByType.Video
            };
            return PartialView("_AccessDateByDate", model);
        }

        public PartialViewResult GetCatAccessDataInADate(DateTime date)
        {
            VideoAccessDateView model = new VideoAccessDateView
            {
                AccessData = _videoRepository.GetCatAccessDataInADate(date),
                Date = date,
                PeriodType = PeriodType.Daily,
                GroupByType = GroupByType.VideoCat
            };
            return PartialView("_AccessDateByDate", model);
        }

        public PartialViewResult GetAccessDataInAWeek(DateTime date)
        {
            VideoAccessDateView model = new VideoAccessDateView
            {
                AccessData = _videoRepository.GetAccessDataInAWeek(date),
                Date = date,
                PeriodType = PeriodType.Weekly,
                GroupByType = GroupByType.Video
            };
            return PartialView("_AccessDateByDate", model);
        }

        public PartialViewResult GetCatAccessDataInAWeek(DateTime date)
        {
            VideoAccessDateView model = new VideoAccessDateView
            {
                AccessData = _videoRepository.GetCatAccessDataInAWeek(date),
                Date = date,
                PeriodType = PeriodType.Weekly,
                GroupByType = GroupByType.VideoCat
            };
            return PartialView("_AccessDateByDate", model);
        }

        public PartialViewResult GetAccessDataInAMonth(DateTime date)
        {
            VideoAccessDateView model = new VideoAccessDateView
            {
                AccessData = _videoRepository.GetAccessDataInAMonth(date),
                Date = date,
                PeriodType = PeriodType.Monthly,
                GroupByType = GroupByType.Video
            };
            return PartialView("_AccessDateByDate", model);
        }

        public PartialViewResult GetCatAccessDataInAMonth(DateTime date)
        {
            VideoAccessDateView model = new VideoAccessDateView
            {
                AccessData = _videoRepository.GetCatAccessDataInAMonth(date),
                Date = date,
                PeriodType = PeriodType.Monthly,
                GroupByType = GroupByType.VideoCat
            };
            return PartialView("_AccessDateByDate", model);
        }

        public PartialViewResult GetAccessDataInAYear(DateTime date)
        {
            VideoAccessDateView model = new VideoAccessDateView
            {
                AccessData = _videoRepository.GetAccessDataInAYear(date),
                Date = date,
                PeriodType = PeriodType.Year,
                GroupByType = GroupByType.Video
            };
            return PartialView("_AccessDateByDate", model);
        }

        public PartialViewResult GetCatAccessDataInAYear(DateTime date)
        {
            VideoAccessDateView model = new VideoAccessDateView
            {
                AccessData = _videoRepository.GetCatAccessDataInAYear(date),
                Date = date,
                PeriodType = PeriodType.Year,
                GroupByType = GroupByType.VideoCat
            };
            return PartialView("_AccessDateByDate", model);
        }

        public ActionResult ExportData(DateTime? from, DateTime? to)
        {
            if (!from.HasValue) from = new DateTime(2015, 1, 1);
            if (!to.HasValue) to = DateTime.Now;
            var gv = new System.Web.UI.WebControls.GridView();
            gv.DataSource = _videoRepository.GetAccessDataBetween(from.Value, to.Value);
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=VITV_" + from.Value.ToShortDateString() + "_" + to.Value.ToShortDateString() + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");
        }

        [RoleAuthorize(Site="portal")]
        public ActionResult AllReports()
        {
            ViewBag.CategoryTypes = _videoCatRepository.AllType.ToList();
            return View();
        }
        //sang
        public JsonResult GetScrollWeek(DateTime date)
        {
            string view = Utilities.RenderRazorViewToString(this, "~/Views/VideoAccess/_GetScrollWeek.cshtml", date);
            return Json(view, JsonRequestBehavior.AllowGet); 
        }

        public JsonResult GetScrollMonth(DateTime date)
        {
            string view = Utilities.RenderRazorViewToString(this, "~/Views/VideoAccess/_GetScrollMonth.cshtml", date);
            return Json(view, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetScrollQuarter(DateTime date)
        {
            string view = Utilities.RenderRazorViewToString(this, "~/Views/VideoAccess/_GetScrollQuarter.cshtml", date);
            return Json(view, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetScrollYear(DateTime date)
        {
            string view = Utilities.RenderRazorViewToString(this, "~/Views/VideoAccess/_GetScrollYear.cshtml", date);
            return Json(view, JsonRequestBehavior.AllowGet);
        }
        //testsang
         [RoleAuthorize(Site = "portal")]
        public ActionResult VideoReport(int videoId)
        {
            var vitvContext = new VITVContext();
            var video = _videoRepository.Find(videoId);
            var gaContext = new GoogleAnalyticContext();
            
            List<AgeReport> ages = gaContext.Ages.Where(a => a.VideoId == videoId).GroupBy(a=>a.Name).Select(
            
                g => new AgeReport
                {
                    Name = g.Key,
                    SessionCount = g.Sum(a => a.SessionCount),
                    NewUser = g.Sum(a => a.NewUser),
                    PageView = g.Sum(a => a.PageView)
                }
            ).ToList();
            List<GenderReport> genders = gaContext.Genders.Where(a => a.VideoId == videoId).GroupBy(a => a.Name).Select(

                g => new GenderReport
                {
                    Name = g.Key,
                    SessionCount = g.Sum(a => a.SessionCount),
                    NewUser = g.Sum(a => a.NewUser),
                    PageView = g.Sum(a => a.PageView)
                }
            ).ToList();
            List<SocialNetworkReport> socialNetworks = gaContext.SocialNetworkDetails.Where(a => a.VideoId == videoId).GroupBy(a => a.Name).Select(

                g => new SocialNetworkReport
                {
                    Name = g.Key,
                    SessionCount = g.Sum(a => a.SessionCount),
                    PageView = g.Sum(a => a.PageView)
                }
            ).ToList();

            List<CountryIsoCodeReport> countries = gaContext.CountryIsoCodes.Where(a => a.VideoId == videoId).GroupBy(a => new { Name = a.Name , FullName = a.FullName}).Select(
                g => new CountryIsoCodeReport
                {
                    Name = g.Key.Name,
                    FullName = g.Key.FullName,
                    SessionCount = g.Sum(a => a.SessionCount),
                    PageView = g.Sum(a => a.PageView),
                    NewUser = g.Sum(a => a.NewUser)
                }
            ).ToList();

            //    g => new CityReport
            //    {
            //        Name = g.Key,
            //        SessionCount = g.Sum(a => a.SessionCount),
            //        NewUser = g.Sum(a => a.NewUser)
            //    }
            //).ToList();

            List<UserTypeReport> userTypes = gaContext.UserTypes.Where(a => a.VideoId == videoId).GroupBy(a => a.Name).Select(

                g => new UserTypeReport
                {
                    Name = g.Key,
                    SessionCount = g.Sum(a => a.SessionCount),
                    PageView = g.Sum(a=>a.PageView)
                }
            ).ToList();

            List<OSReport> OSs = gaContext.OSs.Where(a => a.VideoId == videoId).GroupBy(a => a.Name).Select(

                g => new OSReport
                {
                    Name = g.Key,
                    SessionCount = g.Sum(a => a.SessionCount),
                    NewUser = g.Sum(a => a.NewUser),
                    PageView = g.Sum(a => a.PageView)
                }
            ).ToList();


            ViewBag.Ages = ages;
            ViewBag.Genders = genders;
            ViewBag.SocialNetworks = socialNetworks;
            ViewBag.Countries = countries;
            ViewBag.UserTypes = userTypes;
            ViewBag.OSs = OSs;
            return View(video);
        }


        public ActionResult CompareVideoReport()
        {
            return PartialView("_CompareVideoReport");
        }

        public ActionResult CompareVideoReportNewReview()
        {
            //var model = _videoRepository.GetAccessDataByCatBetween(catId, new DateTime(year, 1, 1), new DateTime(year, 12, 31));
            return PartialView("_CompareVideoReport-New-Review");
        }

        public ActionResult CompareVideoReportNewOther(int catId, int year)
        {

            //var model = _videoRepository.GetAccessDataByCatBetween(catId, new DateTime(year, 1, 1), new DateTime(year, 12, 31));
            return PartialView("_CompareVideoReport-New-Other");
        }

        public ActionResult CompareVideoSameWeekReport(int videoId)
        {
            return PartialView("_CompareVideoSameWeekReport");
        }
        public JsonResult AjaxCompareVideoSameWeekReport(int videoId)
        {
            var video = _videoRepository.Find(videoId);
            
            var dateOffset = (video.DisplayTime.DayOfWeek - DayOfWeek.Monday + 7)%7;
            var monday = video.DisplayTime.AddDays(-dateOffset);
            var nextMonday = monday.AddDays(7);

            var catTypeId = video.Category.TypeId;
            var specializeIds = new List<int> { 3, 4 };
            if(!catTypeId.HasValue || !specializeIds.Contains(catTypeId.Value))
            {
                return Json(new { success = false}, JsonRequestBehavior.AllowGet);
            }
            //var week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(video.DisplayTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var videoReports = _videoRepository.GetMany(v => v.Category.TypeId.HasValue && specializeIds.Contains(v.Category.TypeId.Value) && v.DisplayTime < nextMonday && v.DisplayTime >= monday).AsEnumerable()
            .Select(v => new CompareVideoItemModel()
            {
                VideoId = v.Id,
                Title = v.Title,
                CategoryName = v.Category.Name,
                VideoDuration = v.Duration.ToString(@"hh\:mm\:ss"),
                VideoPeriod = (DateTime.Now - v.DisplayTime).Days,
                DisplayTime = v.DisplayTime
            }).ToList();

            foreach (var vd in videoReports)
            {
                var firstWeekReport = _videoRepository.GetFirstWeekReport(vd.VideoId, vd.DisplayTime, null);
                vd.FirstWeek = firstWeekReport;

                var alltimeReport = _videoRepository.GetAllTimeReport(vd.VideoId);
                vd.AllTime = alltimeReport;

                vd.ReviewReport = new VideoReviewReport
                {
                    PageView = vd.AllTime.PageView - vd.FirstWeek.PageView,
                    ViewDateCount = vd.AllTime.ViewDateCount - vd.FirstWeek.ViewDateCount,
                };
            }

            var tabledata = new CompareVideoModel
            {
                CurrentId = videoId,
                Videos = videoReports,
                FirstWeekView = videoReports.Sum(v => v.FirstWeek.PageView),
                AllTimeView = videoReports.Sum(v => v.AllTime.PageView),
                FirstWeekAverage = Math.Round(videoReports.Average(v => v.FirstWeek.PageView), 2),
                AllTimeAverage = Math.Round(videoReports.Average(v => v.AllTime.PageView), 2)
            };
            foreach (var vd in videoReports)
            {
                vd.FirstWeek.AverageChange = Math.Round(vd.FirstWeek.PageView - tabledata.FirstWeekAverage,2);
                vd.FirstWeek.PercentPageView = Math.Round(vd.FirstWeek.PageView * 100D / tabledata.FirstWeekView, 2);

                vd.AllTime.AverageChange = Math.Round(vd.AllTime.PageView - tabledata.AllTimeAverage, 2);
                vd.AllTime.PercentPageView = Math.Round(vd.AllTime.PageView * 100D / tabledata.AllTimeView, 2);

                vd.ReviewReport.PercentPageView = vd.AllTime.PageView == 0? 0 : Math.Round(vd.ReviewReport.PageView * 100D / vd.AllTime.PageView, 2);
            };

            var chartData = new ArrayList();
            var firstWeek = new { name = "Tuần đầu", data = new ArrayList() };
            var review = new { name = "Xem lại", data = new ArrayList() };
            var chartCategories = new ArrayList();

            foreach(var vd in tabledata.Videos.OrderBy(v=>v.CategoryName))
            {
                chartCategories.Add(vd.CategoryName);
                //foreach (var vd in cat)
                {
                    firstWeek.data.Add(new { name = vd.Title, y = vd.FirstWeek.PageView });
                    review.data.Add(new { name = vd.Title, y = vd.ReviewReport.PageView });
                }
            }
            chartData.Add(review); 
            chartData.Add(firstWeek);
           
            return Json(new { table = tabledata, chart = chartData, categories = chartCategories }, JsonRequestBehavior.AllowGet);
            //return PartialView("_CompareVideoSameWeekReport", model);
        }

        [RoleAuthorize(Site = "portal")]
        //TODO: báo cáo theo tuần chương trình cha
        public ActionResult ParentCategoryReport(int catId)
        {
            var parentCat = _videoCatRepository.Find(catId);
            if (parentCat == null)
                return HttpNotFound();
            if(parentCat.Children.Count==0)
            {
                //tro ve trang bao cao cho chuyen muc le
            }
            return View(parentCat);
        }
        public JsonResult GetParentCategoryReport(int catId, int year)
        {
            var result = _videoCatRepository.GetParentCategoryReport(catId, year);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //TODO: báo cáo chương trình theo ngày trong tuần
        public ActionResult CategoryReportWeekDate()
        {
            ViewBag.CatTypes = _videoRepository.CatTypeList();
            return View();
        }
        public JsonResult GetCategoryReportWeekDate(int catId)
        {
            var result = _videoCatRepository.GetCatReportWeekDate(catId, null, null);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult PartialCatWeekDateReport(int catId, DateTime from, DateTime to)
        {
            var result = _videoCatRepository.GetCatReportWeekDate(catId, from, to);
            return PartialView("_CatWeekDateReport", result);
        }
        //Partial khi so sánh sự thay đổi của các ngày trong tuần với trung bình cả năm
         public PartialViewResult PartialCatWeekChangeReport(int catId, DateTime from, DateTime to)
        {
            var result = _videoCatRepository.GetCatReportWeekChange(catId, from, to);
            return PartialView("_CatWeekChangeReport", result);
        }
         public PartialViewResult PartialCatWeekAvgChangeReport(int catId, DateTime from, DateTime to, double avgWeek)
         {
             var result = _videoCatRepository.GetCatReportWeekAvgChange(catId, from, to, avgWeek);
             return PartialView("_CatWeekAvgChangeReport", result);
         }

        //testsang
        //24806 tuan 33 số 17
        public ActionResult GetVideoReport(int videoId)
        {
            
            var video = _videoRepository.Find(videoId);
            if (video == null)
                return HttpNotFound();
            var year = DateTime.Now.Year;
            DateTime firstDate = new DateTime(year, 1, 1);
            var data = _videoRepository.GetVideoAccess(videoId, "day", video.DisplayTime.Date, new DateTime(year,12,31)).OrderBy(v => v.Date).ToList();
            var page = _pageCatRepository.Find(video.Id);
            var lstVideo = _videoRepository.GetMany(v => v.DisplayTime >= firstDate && v.DisplayTime <= video.DisplayTime && v.CategoryId == video.CategoryId).OrderBy(v => v.DisplayTime).ToList();
            var lstProSchedDetail = _programScheduleDetailRepository.GetMany(p => p.VideoCategoryId == video.CategoryId && p.DateTime >= firstDate && p.DateTime <= video.DisplayTime && p.IsNew, order => order.DateTime).ToList();
            List<ProgramSchedule> lstProSched = _programScheduleRepository.GetMany(p => p.VideoCategoryId == video.CategoryId && p.IsNew, order => order.VideoCategoryId).ToList();
            var lastDayOfYear = video.DisplayTime;
            var firstDayOfYear = lstVideo.Count() > 0 ? lstVideo.FirstOrDefault().DisplayTime : firstDate;
            int weekNoProSched = GetVideoByProSched(firstDayOfYear, lastDayOfYear, lstProSched);
            int weekNo = lstProSchedDetail.Count() > 0 ? lstProSchedDetail.Count() : 0;
            int videoNo = lstVideo.Count() > 0 ? lstVideo.IndexOf(video) + 1 : 0;
            
            List<ArrayList> lstData = new List<ArrayList>();
            DateTime last = data.LastOrDefault().Date;
            
            for (DateTime dateTime = video.DisplayTime.Date; dateTime <= last; dateTime += TimeSpan.FromDays(1))
            {
                VideoPeriodAccessReport vireport = data.Where(d => d.Date == dateTime).FirstOrDefault();
                ArrayList arr = new ArrayList();
                if (vireport != null)
                {
                    arr.Add(ToUnixTimestamp(vireport.Date));
                    arr.Add(vireport.IPViewCount);
                    arr.Add(vireport.PageViewCount);
                     
                }
                else
                {
                    arr.Add(ToUnixTimestamp(dateTime));
                    arr.Add(0);
                    arr.Add(0);
                }
                lstData.Add(arr);
            }

            return Json(new
            {
                Id = video.Id,
                name = video.Title,
                PublishedTime = video.PublishedTime.ToString("dd-MM-yyyy") + " vào lúc " + video.PublishedTime.ToString("HH:mm"),
                UploadTime = video.UploadedTime.Value.ToString("dd-MM-yyyy") + " vào lúc " + video.UploadedTime.Value.ToString("HH:mm"),
                IsPublishFail = video.PublishedTime < video.UploadedTime,
                Duration = video.Duration, 
                Category = video.Category.Name, 
                CategoryId = video.Category.Id,
                TypeCat = video.Category.Type.Name,
                Editor = video.UploadUser != null ? string.IsNullOrEmpty(video.UploadUser.UserName) ? "": video.UploadUser.UserName: "",
                LinkVideo = page.Url,
                weekNoProSched = weekNoProSched,
                lstProSched = lstProSched.Count(),
                weekNo = weekNo,
                videoNo = videoNo,
                data = lstData.ToList() }, JsonRequestBehavior.AllowGet);
        }
        [RoleAuthorize(Site = "portal")]
        //Báo cáo các tuần trong năm của 1 Chuyên mục
        public ActionResult AllDetailReports(int catid)
        {
            var db = new VITVContext();
            var gaContext = new GoogleAnalyticContext();

            var vc = db.VideoCategories.Find(catid);
            if (vc == null)
                return Content("<b>Không có dữ liệu !</b>");
            List<AgeReport> ages = gaContext.AgeOverviews.Where(a => a.VideoCatId == catid).GroupBy(a => a.Name).Select(

                g => new AgeReport
                {
                    Name = g.Key,
                    SessionCount = g.Sum(a => a.SessionCount),
                    NewUser = g.Sum(a => a.NewUser)
                }
            ).ToList();
            List<GenderReport> genders = gaContext.GenderOverviews.Where(a => a.VideoCatId == catid).GroupBy(a => a.Name).Select(

                g => new GenderReport
                {
                    Name = g.Key,
                    SessionCount = g.Sum(a => a.SessionCount),
                    NewUser = g.Sum(a => a.NewUser)
                }
            ).ToList();
            List<SocialNetworkReport> socialNetworks = gaContext.SocialNetworkDetails.Where(a => a.VideoCatId == catid).GroupBy(a => a.Name).Select(

                g => new SocialNetworkReport
                {
                    Name = g.Key,
                    SessionCount = g.Sum(a => a.SessionCount),
                    PageView = g.Sum(a => a.PageView)
                }
            ).ToList();

            List<CityReport> cities = gaContext.Cities.Where(a => a.VideoCatId == catid).GroupBy(a => a.Name).Select(

                g => new CityReport
                {
                    Name = g.Key,
                    SessionCount = g.Sum(a => a.SessionCount),
                    NewUser = g.Sum(a => a.NewUser)
                }
            ).ToList();

            ViewBag.Ages = ages;
            ViewBag.Genders = genders;
            ViewBag.SocialNetworks = socialNetworks;
            ViewBag.Cities = cities;

            if (vc.Children != null && vc.Children.Count > 0)
            {
                //tro ve trang bao cao cho chuyen muc cha
                return View("ParentCategoryReport", vc);
            }
            return View(vc);
        }
       
        public ActionResult GetAllCatAccessDataByNumWeek(int catid, int year)
        {
            var data = _videoRepository.GetAllCatAccessDataByNumWeek(catid, year);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllCatAccessDetailByWeekForChart(int catid, int year) //lay so lieu de hien thi tren bieu do so sanh cac chuyen muc
        {

            Tuple<List<CatAccessDataChart>, double, double> data = _videoRepository.GetAllCatAccessDetailByWeekForChart(catid, year);
            return Json(new { chartdata = data.Item1, avg = data.Item2, avgAllVideoInWeek = data.Item3
            }, JsonRequestBehavior.AllowGet);
        }
        //testsang
        public ActionResult GetInfoBase(int catid, int year)
        {
            var infobase = GetCompareVideoReport(catid, year);
            return Json(new
            {
                FirstWeekHighestVideoId = infobase.FirstWeekHighestVideoId,
                FirstWeekLowestVideoId = infobase.FirstWeekLowestVideoId,
                AllTimeHighestVideoId = infobase.AllTimeHighestVideoId,
                AllTimeLowestVideoId = infobase.AllTimeLowestVideoId,
                AllTimeAverageHighest = infobase.AllTimeAverageHighest,
                FirstWeekAverageHighest = infobase.FirstWeekAverageHighest,
                VideoCount = infobase.VideoCount,
                CategoryTypeName = infobase.CategoryTypeName,
                CategoryName = infobase.CategoryName,
                Review = infobase.Review,
                AllTimeMedian = infobase.AllTimeMedian,
                FirstWeekMedian = infobase.FirstWeekMedian,
                FirstWeekHighest = infobase.FirstWeekHighest,
                FirstWeekLowest = infobase.FirstWeekLowest,
                AllTimeHighest = infobase.AllTimeHighest,
                AllTimeLowest = infobase.AllTimeLowest,
                AllTimeAverage = infobase.AllTimeAverage,
                AllTimeView = infobase.AllTimeView,
                FirstWeekAverage = infobase.FirstWeekAverage,
                FirstWeekView = infobase.FirstWeekView,
            }, JsonRequestBehavior.AllowGet);
        }
        //testsang
        //Xem trong tuan dau, xem lai, trung binh
        public ActionResult GetAllCatAccessDetailType1ForChart(int catid, int year) //lay so lieu de hien thi tren bieu do so sanh các video cùng chương trình
        {

            var tabledata = _videoRepository.GetAllCatAccessDataType1(catid, year);
            //Không phải là chuyên đề
            if(tabledata.Videos == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            var firstWeekData = new List<AccessData>();
            var reviewData = new List<AccessData>();
            var firstWeek = new AccessDataChart()
            {
                name = "Tuần đầu",
                data = firstWeekData
            };
            var review = new AccessDataChart()
            {
                name = "Coi lại",
                data = reviewData
            };
            List<AccessDataChart> chartdata = new List<AccessDataChart> { review, firstWeek };
            foreach (var videoItem in tabledata.Videos)
            {
                firstWeekData.Add(new AccessData
                {
                    x = videoItem.Week,
                    y = videoItem.FirstWeek.PageView,
                    name = videoItem.Title
                });
                reviewData.Add(new AccessData
                {
                    x = videoItem.Week,
                    y = videoItem.AllTime.PageView - videoItem.FirstWeek.PageView,
                    name = videoItem.Title
                });
            }
            return Json(new { success= true, tabledata = tabledata, chartdata = chartdata }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllCatAccessDetailType2ForChart(int catid, int year) //lay so lieu de hien thi tren bieu do so sanh cac chuyen muc
        {

            CatVideosReport tabledata = _videoRepository.GetAllCatAccessDataType2(catid, year);
            if (tabledata.Videos == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            var ipViewData = new List<AccessData>();
            var reviewData = new List<AccessData>();
            var ipView = new AccessDataChart()
            {
                name = "Xem không trùng",
                data = ipViewData
            };
            var review = new AccessDataChart()
            {
                name = "Xem lại",
                data = reviewData
            };
            List<AccessDataChart> chartdata = new List<AccessDataChart> { review, ipView };
            foreach (var videoItem in tabledata.Videos)
            {
                ipViewData.Add(new AccessData
                {
                    x = videoItem.Week,
                    y = videoItem.IPViewCount,
                    name = videoItem.Title
                });
                reviewData.Add(new AccessData
                {
                    x = videoItem.Week,
                    y = videoItem.ReviewCount,
                    name = videoItem.Title
                });
            }
            return Json(new { success = true, tabledata = tabledata, chartdata = chartdata, avg = 0 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllCatAccessDetailType3ForChart(int catid, int year) //lay so lieu de hien thi tren bieu do so sanh cac chuyen muc
        {

            Tuple<List<AccessDataChart>, double> data = _videoRepository.GetAllCatAccessDataType3(catid, year);
            if (data == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            } 
            return Json(new { success = true, chartdata = data.Item1, avg = Math.Round(data.Item2, 2) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopVideo(int catId, DateTime from, string dateType)
        {
            var topVideos = _videoRepository.GetTopVideo(catId, from, dateType);
            ViewBag.catid = catId;
            return PartialView(topVideos);
        }

        public JsonResult GetAllReportData(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
            {
                startDate = DateTime.Now.Date.AddYears(-1);
                endDate = DateTime.Now.Date;
            }
            var data = _videoRepository.GetAccessDataBetween(startDate.Value, endDate.Value);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChildenReportData(int parentId, DateTime startDate, DateTime endDate)
        {
            var data = _videoRepository.GetChildrenAccessDataBetween(startDate, endDate, parentId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //testsang
        private CompareVideoModel GetCompareVideoReport(int catId, int year)
        {
            var cat = _videoCatRepository.Find(catId);
            var videoReports = _videoRepository.GetMany(v => v.CategoryId == catId && v.DisplayTime.Year == year).AsEnumerable()
            .Select(v => new CompareVideoItemModel()
            {
                CategoryName = v.Category.Name,
                CategoryTypeName = v.Category.Type.Name,
                VideoId = v.Id,
                Title = v.Title,
                Month = v.DisplayTime.Month,
                Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(v.DisplayTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                VideoDuration = v.Duration.ToString(@"hh\:mm\:ss"),
                VideoPeriod = (DateTime.Now - v.DisplayTime).Days,
                DisplayTime = v.DisplayTime
            }).ToList();
            
            foreach (var vd in videoReports)
            {
                var firstWeekReport = _videoRepository.GetFirstWeekReport(vd.VideoId, vd.DisplayTime, null);
                vd.FirstWeek = firstWeekReport;

                var alltimeReport = _videoRepository.GetAllTimeReport(vd.VideoId);
                vd.AllTime = alltimeReport;

                vd.ReviewReport = new VideoReviewReport
                {
                    PageView = vd.AllTime.PageView - vd.FirstWeek.PageView,
                    ViewDateCount = vd.AllTime.ViewDateCount - vd.FirstWeek.ViewDateCount,
                };
            }

            var model = new CompareVideoModel
            {
                FirstWeekHighestVideoId = videoReports.Count() == 0 ? 0 : videoReports.Where(v => v.FirstWeek.Highest == videoReports.Max(s => s.FirstWeek.Highest)).FirstOrDefault().VideoId,
                FirstWeekLowestVideoId = videoReports.Count() == 0 ? 0 : videoReports.Where(v => v.FirstWeek.Lowest == videoReports.Min(s => s.FirstWeek.Lowest)).FirstOrDefault().VideoId,
                AllTimeHighestVideoId = videoReports.Count() == 0 ? 0 : videoReports.Where(v => v.AllTime.Highest == videoReports.Max(s => s.AllTime.Highest)).FirstOrDefault().VideoId,
                AllTimeLowestVideoId = videoReports.Count() == 0 ? 0 : videoReports.Where(v => v.AllTime.Lowest == videoReports.Min(s => s.AllTime.Lowest)).FirstOrDefault().VideoId,
                AllTimeAverageHighest = videoReports.Count() == 0 ? 0 : Math.Round((double)videoReports.Sum(v => v.AllTime.Highest) / videoReports.Select(v => v.AllTime.Highest).Count(), 2),
                FirstWeekAverageHighest = videoReports.Count() == 0 ? 0 : Math.Round((double)videoReports.Sum(v => v.FirstWeek.Highest) / videoReports.Select(v => v.FirstWeek.Highest).Count(), 2),
                VideoCount = videoReports.Count(),
                CategoryTypeName = cat.Type !=null? cat.Type.Name: "",
                CategoryName = cat.Name,
                FirstWeekView = videoReports.Count() == 0 ? 0 : videoReports.Sum(x => x.FirstWeek.PageView),
                AllTimeView = videoReports.Count() == 0 ? 0 : videoReports.Sum(v => v.AllTime.PageView),
                Review = videoReports.Count() == 0 ? 0 : videoReports.Sum(v => v.AllTime.PageView) - videoReports.Sum(x => x.FirstWeek.PageView),
                AllTimeMedian = videoReports.Count() == 0 ? 0 : GetMedian(videoReports, 1),
                FirstWeekMedian = videoReports.Count() == 0 ? 0 : GetMedian(videoReports, 2),
                FirstWeekHighest = videoReports.Count() == 0 ? 0 : videoReports.Max(m => m.FirstWeek.Highest),
                FirstWeekLowest = videoReports.Count() == 0 ? 0 : videoReports.Min(m => m.FirstWeek.Lowest),
                AllTimeHighest = videoReports.Count() == 0 ? 0 : videoReports.Max(m => m.AllTime.Highest),
                AllTimeLowest =videoReports.Count() == 0 ? 0 : videoReports.Max(m => m.AllTime.Lowest),
                FirstWeekAverage = videoReports.Count() == 0 ? 0 : Math.Round(videoReports.Average(v => v.FirstWeek.PageView), 2),
                AllTimeAverage = videoReports.Count() == 0 ? 0 : Math.Round(videoReports.Average(v => v.AllTime.PageView), 2)
            };
            //foreach (var vd in videoReports)
            //{
            //    vd.FirstWeek.AverageChange = vd.FirstWeek.PageView - model.FirstWeekAverage;
            //    vd.FirstWeek.PercentPageView = Math.Round(vd.FirstWeek.PageView * 100D / model.FirstWeekView, 2);

            //    vd.AllTime.AverageChange = vd.AllTime.PageView - model.AllTimeAverage;
            //    vd.AllTime.PercentPageView = Math.Round(vd.AllTime.PageView * 100D / model.AllTimeView, 2);

            //    vd.ReviewReport.PercentPageView = Math.Round(vd.ReviewReport.PageView * 100D / vd.AllTime.PageView, 2);
            //};
            return model;
        }

        private int GetVideoByProSched(DateTime from, DateTime to, List<ProgramSchedule> lstProSched)
        {
            int count = 0;
            for (DateTime dateTime = from.Date; dateTime <= to; dateTime += TimeSpan.FromDays(1))
            {
                if(lstProSched.Where(p => p.DayOfWeek == dateTime.DayOfWeek).FirstOrDefault() != null)
                    count++;
            }
            return count;
        }

        //type = 1 => "ALL" - //type = 2 => "FIRSTWEEK"
        private double GetMedian(List<CompareVideoItemModel> values, int type) {
            List<CompareVideoItemModel> list = null;
            if (type == 1)
            {
                list = values.OrderBy(x => x.AllTime.PageView).ToList();
                double mid = (list.Count() - 1) / 2.0;
                return (list.ElementAt((int)(mid)).AllTime.PageView + list.ElementAt((int)(mid + 0.5)).AllTime.PageView) / 2;
            }
            list = values.OrderBy(x => x.FirstWeek.PageView).ToList();
            double midFirst = (list.Count() - 1) / 2.0;
            return (list.ElementAt((int)(midFirst)).FirstWeek.PageView + list.ElementAt((int)(midFirst + 0.5)).FirstWeek.PageView) / 2;
        }

        private DateTime GetFirstSunday(int year)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            return jan1.AddDays(-(jan1.DayOfWeek - DayOfWeek.Sunday));
        }

        private double ToUnixTimestamp(DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            var unixTimestamp = (target - date).TotalMilliseconds;
            return unixTimestamp;
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp / 1000).ToLocalTime();
            return dtDateTime;
        }
    }
}