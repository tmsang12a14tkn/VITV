using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;
using VITV.Data.DAL;
using VITV.Web.Helpers;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using System.Web.Routing;
using System.IO;
using System.Collections.Generic;
using VITV_Web.ViewModels;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace VITV.Web.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoCategoryRepository _videocategoryRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IReporterRepository _reporterRepository;
        private readonly IKeywordRepository _keywordRepository;
        private readonly IVideoCatGroupRepository _videoCatGroupRepository;
        private readonly IProgramScheduleDetailRepository _scheduleDetailRepository;
        private readonly ISpecialEventRepository _specialEventRepository;
        private readonly IVideoTranscriptRepository _videoTranscriptRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IVideoCatSponsorRepository _videoCatSponsorRepository;

        public VideoController()
        {
            var context = new VITVContext();
            _videocategoryRepository = new VideoCategoryRepository(context);
            _videoRepository = new VideoRepository(context);
            _reporterRepository = new ReporterRepository(context);
            _keywordRepository = new KeywordRepository(context);
            _videoCatGroupRepository = new VideoCatGroupRepository(context);
            _scheduleDetailRepository = new ProgramScheduleDetailRepository(context);
            _specialEventRepository = new SpecialEventRepository(context);
            _videoTranscriptRepository = new VideoTranscriptRepository(context);
            _settingRepository = new SettingRepository(context);
            _videoCatSponsorRepository = new VideoCatSponsorRepository(context);
        }

        public VideoController(IVideoCategoryRepository videocategoryRepository, IVideoRepository videoRepository, IVideoCatSponsorRepository videoCatSponsorRepository,
            IReporterRepository reporterRepository, IKeywordRepository keywordRepository, IVideoCatGroupRepository videoCatGroupRepository, IVideoTranscriptRepository videoTranscriptRepository)
        {
            _videocategoryRepository = videocategoryRepository;
            _videoRepository = videoRepository;
            _reporterRepository = reporterRepository;
            _keywordRepository = keywordRepository;
            _videoCatGroupRepository = videoCatGroupRepository;
            _videoTranscriptRepository = videoTranscriptRepository;
            _videoCatSponsorRepository = videoCatSponsorRepository;
        }

        public ViewResult Index(SearchVideoModel model)
        {
            var now = DateTime.Now;
            var videosQuery = _videoRepository.All.Where(v => v.IsPublished && v.PublishedTime <= now);
            if (model.rangeType == RangeType.All)
            {
                //videosQuery = _videoRepository.All.Where(v => v.IsPublished);
            }
            else if (model.rangeType == RangeType.Day)
            {
                var last1Day = DateTime.Now.AddDays(-1);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last1Day);
            }
            else if (model.rangeType == RangeType.Week)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= lastWeek);
            }
            else if (model.rangeType == RangeType.OneMonth)
            {
                var last1Month = DateTime.Now.AddMonths(-1);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last1Month);
            }
            else if (model.rangeType == RangeType.ThreeMonth)
            {
                var last3Month = DateTime.Now.AddMonths(-3);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last3Month);
            }
            else if (model.rangeType == RangeType.SixMonth)
            {
                var last6Month = DateTime.Now.AddMonths(-6);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last6Month);
            }
            else if (model.rangeType == RangeType.Custom) //custom
            {
                videosQuery = videosQuery.Where(v => v.DisplayTime >= model.begin && v.DisplayTime <= model.end);
            }


            var cat = _videoCatGroupRepository.Find(model.cat);
            if (cat != null)
            {
                videosQuery = videosQuery.Where(v => v.Category.Group.Id == cat.Id);
            }
            else if (model.cat != "all")
            {
                videosQuery = videosQuery.Where(v => v.Category.UniqueTitle == model.cat);
            }


            //int catId;
            //if (model.cat == "chuyenmuc")
            //{
            //    videosQuery = videosQuery.Where(v => v.Category.Group.Id == 1);
            //}
            //else if (model.cat == "tintuc")
            //{
            //    videosQuery = videosQuery.Where(v => v.Category.Group.Id == 2);
            //}
            //else if (model.cat == "giaitri")
            //{
            //    videosQuery = videosQuery.Where(v => v.Category.Group.Id == 3);
            //}else if (model.cat != "all" && int.TryParse(model.cat, out catId))
            //{
            //    videosQuery = videosQuery.Where(v => v.CategoryId == catId);
            //}

            if (!String.IsNullOrEmpty(model.rep))
            {
                videosQuery = videosQuery.Where(v => v.Reporters.Any(r => r.Name.Contains(model.rep)));
            }

            if (!String.IsNullOrEmpty(model.title))
            {
                videosQuery = videosQuery.Where(v => v.Title.Contains(model.title));
            }

            if (model.evtId.HasValue)
            {
                videosQuery = videosQuery.Where(v => v.SpecialEventId == model.evtId);
            }
            const int noResult = 16;
            var pageCount = (int)Math.Ceiling((double)videosQuery.Count() / noResult);
            int iPage;
            if (int.TryParse(model.page, out iPage))
            {
                videosQuery = videosQuery.Skip((iPage - 1) * noResult).Take(noResult);
            }

            var videos = videosQuery.OrderByDescending(v => v.DisplayTime).ToList();
            var filterView = new VideoFilterView
            {
                Category = model.cat,
                Page = iPage,
                PageCount = pageCount,
                Videos = videos,
                Range = model.rangeType,
                Reporter = model.rep
            };

            var dayLastYear = DateTime.Now.AddYears(-1);

            var homeView = new VideoHomeView
            {
                SearchVideoModel = model,
                VideoCatGroups = _videoCatGroupRepository.AllIncluding(v => v.VideoCats).OrderBy(v => v.Order).ToList(),
                SpecialEvents = _specialEventRepository.GetMany(e => e.EndDate > dayLastYear).Select(e => new SpecialEventDetail { Id = e.Id, Name = e.Name }).ToList(),

                VideoFilterView = filterView,
            };


            return View(homeView);
        }

        public ActionResult Details(int id, int? t)
        {
            var video = _videoRepository.Find(id);
            if (video == null)
                return new HttpNotFoundResult("Không tìm thấy video");
            if (!User.Identity.IsAuthenticated && (!video.IsPublished || video.PublishedTime > DateTime.Now))
                return new HttpNotFoundResult("Video chưa được đăng");
            var relatedVideos = _videoRepository.GetMany(v => v.CategoryId == video.CategoryId && v.Id != video.Id && v.IsPublished && v.PublishedTime <= DateTime.Now)
                                                .OrderByDescending(v => v.DisplayTime)
                                                .Take(8)
                                                .ToList();

            ViewBag.sponsorList = video.Category.Sponsors.ToList();
            ViewBag.RelatedVideos = relatedVideos;
            ViewBag.videoId = id;
            ViewBag.videoCatId = video.CategoryId;
            ViewBag.jumpTime = t.HasValue ? t : 0;
            ViewBag.skipIntroTime = video.HourSkip * 60 * 60 + video.MinuteSkip * 60 + video.SecondSkip;
            ViewBag.transcripts = _videoTranscriptRepository.GetMany(vt => vt.VideoId == video.Id)
                                                            .OrderBy(vt => vt.HourSeek)
                                                            .ThenBy(vt => vt.MinuteSeek)
                                                            .ThenBy(vt => vt.SecondSeek)
                                                            .ToList();
            var advertiseOrder = _settingRepository.GetValue("AdvertiseOrder");
            var skipTime = _settingRepository.GetValue("SkipTime");
            var rand = new Random(DateTime.Now.Millisecond);
            var randomAd = video.Category.VideoTVCs.Skip(rand.Next(video.Category.VideoTVCs.Count)).FirstOrDefault();
            var intro = video.Category.Intros.FirstOrDefault(i => i.Selected);
            var introPreroll = intro != null ? "{ \"source\": { \"src\": \"" + intro.Url + "\", \"type\": \"video/mp4\" }, \"allowSkip\": false }" : "";
            var adsPreroll = video.Category.HasAds == true ? 
                (randomAd != null ? "{ \"source\": { \"src\": \"" + randomAd.Url + "\", \"type\": \"video/mp4\" }, \"allowSkip\":true, \"skipTime\": " + skipTime + "}" : "")
                : "";
            var prerollArray = new string[]{adsPreroll, introPreroll};
            if (advertiseOrder != "1")
                prerollArray = prerollArray.Reverse().ToArray();
           
            ViewBag.VideoPreroll = "[" +String.Join(",", prerollArray.Where(s => !string.IsNullOrEmpty(s))) +"]";
            return View(video);
        }

        private int ParseSeconds(string time)
        {
            var times = time.Split(':');
            var seconds = 0;

            if (times.Count() == 3)
                seconds = Convert.ToInt32(times[0]) * 60 * 60 + Convert.ToInt32(times[1]) * 60 + Convert.ToInt32(times[2]);
            else
                seconds = Convert.ToInt32(times[0]) * 60 + Convert.ToInt32(times[1]);

            return seconds;
        }

        //Dang search
        [RoleAuthorize(Site = "admin")]
        [HttpGet]
        public ViewResult Management(int? evtId, DateTime? begin, DateTime? end, string cat = "all", string rep = "", string orderby = "time", string title = "", string page = "1", RangeType rangeType = RangeType.All)
        {
            var now = DateTime.Now;
            var videosQuery = _videoRepository.All;
            if (rangeType == RangeType.All)
            {

            }
            else if (rangeType == RangeType.Day)
            {
                var last1Day = DateTime.Now.AddDays(-1);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last1Day);
            }
            else if (rangeType == RangeType.Week)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= lastWeek);
            }
            else if (rangeType == RangeType.OneMonth)
            {
                var last1Month = DateTime.Now.AddMonths(-1);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last1Month);
            }
            else if (rangeType == RangeType.ThreeMonth)
            {
                var last3Month = DateTime.Now.AddMonths(-3);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last3Month);
            }
            else if (rangeType == RangeType.SixMonth)
            {
                var last6Month = DateTime.Now.AddMonths(-6);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last6Month);
            }
            else if (rangeType == RangeType.Custom) //custom
            {
                videosQuery = videosQuery.Where(v => v.DisplayTime >= begin && v.DisplayTime <= end);
            }

            int catId;
            if (cat != "all" && int.TryParse(cat, out catId))
            {
                videosQuery = videosQuery.Where(v => v.CategoryId == catId);
            }

            if (rep != "")
            {
                videosQuery = videosQuery.Where(v => v.Reporters.Any(r => r.Name.Contains(rep)));
            }

            if (title != "")
            {
                videosQuery = videosQuery.Where(v => v.Title.Contains(title));
            }

            if (evtId.HasValue)
            {
                videosQuery = videosQuery.Where(v => v.SpecialEventId == evtId);
            }

            //dung cho version cu mau xanh la
            const int noResult = 56;
            var pageCount = (int)Math.Ceiling((double)videosQuery.Count() / noResult);
            int iPage;
            var video = videosQuery.ToList();
            var hello = video;
            List<Video> videos = null;
            if (int.TryParse(page, out iPage))
            {
                //videosQuery = videosQuery.Skip((iPage - 1) * noResult).Take(noResult);
                videos = GetVideosByPage(iPage, noResult, videosQuery);
            }
            //if (orderby == "time") videosQuery = videosQuery.OrderBy(v => v.DisplayTime);
            ////else if (orderby == "view") videosQuery = videosQuery.OrderByDescending(v => v.PageAccesses.Sum(va=>va.Count));
            //else if (orderby == "title") videosQuery = videosQuery.OrderBy(v => v.Title);
            
            var filterView = new VideoFilterView
            {
                Category = cat,
                Page = iPage,
                PageCount = pageCount,
                Videos = videos,
                Range = rangeType,
                EventId = evtId,
                Reporter = rep,
                Title = title,
                Begin = begin,
                End = end
            };

            var dayLastYear = DateTime.Now.AddYears(-1);
            ViewBag.VideoCatGroups = _videoCatGroupRepository.AllIncluding(v => v.VideoCats).OrderBy(v => v.Order).ToList();
            ViewBag.SpecialEvents = _specialEventRepository.GetMany(e => e.EndDate > dayLastYear).Select(e => new SpecialEventDetail { Id = e.Id, Name = e.Name }).ToList();
            return View("Management", filterView);
        }

        //dang scroll
        [RoleAuthorize(Site = "admin")]
        [HttpGet]
        public ViewResult ManagementScroll(int? evtId, DateTime? begin, DateTime? end, string cat = "all", string rep = "", string orderby = "time", string title = "", string page = "1", RangeType rangeType = RangeType.All)
        {
            var now = DateTime.Now;
            var videosQuery = _videoRepository.All;
            if (rangeType == RangeType.All)
            {
                DateTime endLastDate = now;
                DateTime beginLastDate = endLastDate.AddDays(-7);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= beginLastDate && v.DisplayTime <= endLastDate);

            }
            else if (rangeType == RangeType.Day)
            {
                var last1Day = DateTime.Now.AddDays(-1);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last1Day);
            }
            else if (rangeType == RangeType.Week)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= lastWeek);
            }
            else if (rangeType == RangeType.OneMonth)
            {
                var last1Month = DateTime.Now.AddMonths(-1);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last1Month);
            }
            else if (rangeType == RangeType.ThreeMonth)
            {
                var last3Month = DateTime.Now.AddMonths(-3);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last3Month);
            }
            else if (rangeType == RangeType.SixMonth)
            {
                var last6Month = DateTime.Now.AddMonths(-6);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last6Month);
            }
            else if (rangeType == RangeType.Custom) //custom
            {
                videosQuery = videosQuery.Where(v => v.DisplayTime >= begin && v.DisplayTime <= end);
            }

            int catId;
            if (cat != "all" && int.TryParse(cat, out catId))
            {
                videosQuery = videosQuery.Where(v => v.CategoryId == catId);
            }

            if (rep != "")
            {
                videosQuery = videosQuery.Where(v => v.Reporters.Any(r => r.Name.Contains(rep)));
            }

            if (title != "")
            {
                videosQuery = videosQuery.Where(v => v.Title.Contains(title));
            }

            if (evtId.HasValue)
            {
                videosQuery = videosQuery.Where(v => v.SpecialEventId == evtId);
            }

            //dung cho version cu mau xanh la
            //const int noResult = 15;
            //var pageCount = (int)Math.Ceiling((double)videosQuery.Count() / noResult);
            //int iPage;
            //if (int.TryParse(page, out iPage))
            //{
            //    videosQuery = videosQuery.Skip((iPage - 1) * noResult).Take(noResult);
            //}
            if (orderby == "time") videosQuery = videosQuery.OrderByDescending(v => v.DisplayTime);
            //else if (orderby == "view") videosQuery = videosQuery.OrderByDescending(v => v.PageAccesses.Sum(va=>va.Count));
            else if (orderby == "title") videosQuery = videosQuery.OrderBy(v => v.Title);
            var videos = videosQuery.ToList();
            var filterView = new VideoFilterView
            {
                Category = cat,
                //Page = iPage,
                //PageCount = pageCount,
                Videos = videos,
                Range = rangeType,
                EventId = evtId,
                Reporter = rep,
                Title = title,
                Begin = begin,
                End = end
            };

            var dayLastYear = DateTime.Now.AddYears(-1);
            ViewBag.VideoCatGroups = _videoCatGroupRepository.AllIncluding(v => v.VideoCats).OrderBy(v => v.Order).ToList();
            ViewBag.SpecialEvents = _specialEventRepository.GetMany(e => e.EndDate > dayLastYear).Select(e => new SpecialEventDetail { Id = e.Id, Name = e.Name }).ToList();
            return View("ManagementScroll", filterView);
        }

        //Dang tab
        [RoleAuthorize(Site = "admin")]
        [HttpGet]
        public ViewResult ManagementTab(int? evtId, DateTime? date, string cat = "all", string rep = "", string title = "", int issearch = 0, int weektab = 0)
        {
            var now = DateTime.Now;
            var videosQuery = _videoRepository.All;
            if (!date.HasValue)
                date = now;
            var offset = (date.Value.DayOfWeek - DayOfWeek.Monday + 7)%7;
            var end = date.Value.AddDays(-offset + 7);
            var begin = end.AddDays(-35);
            videosQuery = videosQuery.Where(v => v.DisplayTime >= begin && v.DisplayTime < end);

            int catId;
            if (cat != "all" && int.TryParse(cat, out catId))
            {
                videosQuery = videosQuery.Where(v => v.CategoryId == catId);
            }

            if (rep != "")
            {
                videosQuery = videosQuery.Where(v => v.Reporters.Any(r => r.Name.Contains(rep)));
            }

            if (title != "")
            {
                videosQuery = videosQuery.Where(v => v.Title.Contains(title));
            }

            if (evtId.HasValue)
            {
                videosQuery = videosQuery.Where(v => v.SpecialEventId == evtId);
            }

           
            var firstDayYear = Utilities.GetFirstDateOfWeek(new DateTime(now.Year, 1, 1), DayOfWeek.Monday);

            var result = videosQuery.OrderBy(v => v.DisplayTime).AsEnumerable()
                .GroupBy(ca => (ca.DisplayTime - firstDayYear).Days / 7 + 1).Select(
                    g => new VideoTabFilterView {
                        Begin = firstDayYear.AddDays(7* g.Key),
                        Videos = g.ToList()
                    }
                ).OrderBy(x => x.Begin).ToList();
            

            var filterView = new VideoFilterView
            {
                DateSelected = date.Value,
                VideosTab = result,
                Category = cat,
                EventId = evtId,
                Reporter = rep,
                Title = title,
                Begin = begin,
                End = end
            };

            var dayLastYear = DateTime.Now.AddYears(-1);
            ViewBag.VideoCatGroups = _videoCatGroupRepository.AllIncluding(v => v.VideoCats).OrderBy(v => v.Order).ToList();
            ViewBag.SpecialEvents = _specialEventRepository.GetMany(e => e.EndDate > dayLastYear).Select(e => new SpecialEventDetail { Id = e.Id, Name = e.Name }).ToList();
            ViewBag.weektab = weektab;
            return View("ManagementTab", filterView);
        }

        [HttpPost]
        public ActionResult UpdateIsPublishedVideo(int videoid)
        {
            var video = _videoRepository.Find(videoid);
            if(video != null)
            {
                video.IsPublished = !video.IsPublished;
                _videoRepository.InsertOrUpdate(video);
                _videoRepository.Save();
                return Json(new { success = true, ispublished = video.IsPublished });
            }
            return Json(new { success = false, content = "Không tồn tại video !"});
        }

        [HttpPost]
        public ActionResult UpdateYoutubePlay(int videoid, string youtubeurl, int videopiority)
        {
            var video = _videoRepository.Find(videoid);
            if(video != null)
            {
                
                video.VideoPiority = videopiority;
                video.YoutubeUrl = youtubeurl;
                _videoRepository.InsertOrUpdate(video);
                _videoRepository.Save();
                if (!string.IsNullOrEmpty(youtubeurl))
                {
                    if(video.VideoPiority == 0)//co link youtube va uu tien youtube
                        return Json(new { code = 1, VideoPiority = video.VideoPiority, YoutubeUrl = video.YoutubeUrl });
                    else//co link youtube va tu luu tru vitv
                        return Json(new { code = 2, VideoPiority = video.VideoPiority, YoutubeUrl = video.YoutubeUrl });
                }else//chua co link youtube
                {
                    return Json(new { code = 3, VideoPiority = video.VideoPiority, YoutubeUrl = video.YoutubeUrl });
                }
            }
            return Json(new { code = 0 });
        }

        public List<Video> GetVideosByPage(int page, int noresult, IQueryable<Video> videos)
        {
            List<Video> lstVideos = new List<Video>();
            int begin = (page - 1) * noresult;
            int end = begin + noresult - 1;
            int count = 0;
            var video = videos.ToList();
            var hello = video;
            IEnumerable<IGrouping<DateTime?, Video>> videolistgroups = videos.GroupBy(b => DbFunctions.TruncateTime(b.DisplayTime)).OrderByDescending(b => b.Key).ToList();
            foreach (IGrouping<DateTime?, Video> videogroup in videolistgroups)
            {
                List<Video> lstVideo = videogroup.OrderBy(v => v.DisplayTime).ToList();
                for(int i = 0; i < lstVideo.Count(); i++)
                {
                    if(count >= begin && count <= end)
                    {
                        lstVideos.Add(lstVideo[i]);
                    }
                    count++;
                }
            }
            return lstVideos;
        }

        public ActionResult GetScrollListVideo(int? evtId, DateTime? begin, DateTime? end, DateTime? lastdate, string cat = "all", string rep = "", string orderby = "time", string title = "", RangeType rangeType = RangeType.All)
        {

            var now = DateTime.Now;
            var videosQuery = _videoRepository.All;
            if (rangeType == RangeType.All)
            {
                //videosQuery = _videoRepository.All;
                DateTime endLastDate = lastdate.Value;
                DateTime beginLastDate = endLastDate.AddDays(-7);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= beginLastDate && v.DisplayTime <= endLastDate);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            if (evtId.HasValue)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            int catId;
            if (cat != "all" && int.TryParse(cat, out catId))
            {
                videosQuery = videosQuery.Where(v => v.CategoryId == catId);
            }

            if (rep != "")
            {
                videosQuery = videosQuery.Where(v => v.Reporters.Any(r => r.Name.Contains(rep)));
            }

            if (title != "")
            {
                videosQuery = videosQuery.Where(v => v.Title.Contains(title));
            }

            

            if (orderby == "time") videosQuery = videosQuery.OrderByDescending(v => v.DisplayTime);
            //else if (orderby == "view") videosQuery = videosQuery.OrderByDescending(v => v.PageAccesses.Sum(va=>va.Count));
            else if (orderby == "title") videosQuery = videosQuery.OrderBy(v => v.Title);
            var videos = videosQuery.ToList();
            var filterView = new VideoFilterView
            {
                Category = cat,
                Videos = videos,
                Range = rangeType,
                EventId = evtId,
                Reporter = rep,
                Title = title,
                Begin = begin,
                End = end,
                Lastdate = lastdate
            };
            string view = Utilities.RenderRazorViewToString(this, "~/Views/Video/_ListVideo.cshtml", filterView);
            return Json(view, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult AdminFilter(int? evtId, DateTime? begin, DateTime? end, string cat = "all", string rep = "", string title = "", string page = "1", RangeType rangeType = RangeType.All)
        {
            var now = DateTime.Now;
            var videosQuery = _videoRepository.All;
            if (rangeType == RangeType.All)
            {
            }
            else if (rangeType == RangeType.Day)
            {
                var last1Day = DateTime.Now.AddDays(-1);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last1Day);
            }
            else if (rangeType == RangeType.Week)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= lastWeek);
            }
            else if (rangeType == RangeType.OneMonth)
            {
                var last1Month = DateTime.Now.AddMonths(-1);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last1Month);
            }
            else if (rangeType == RangeType.ThreeMonth)
            {
                var last3Month = DateTime.Now.AddMonths(-3);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last3Month);
            }
            else if (rangeType == RangeType.SixMonth)
            {
                var last6Month = DateTime.Now.AddMonths(-6);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last6Month);
            }
            else if (rangeType == RangeType.Custom) //custom
            {
                videosQuery = videosQuery.Where(v => v.DisplayTime >= begin && v.DisplayTime <= end);
            }

            int catId;
            if (cat != "all" && int.TryParse(cat, out catId))
            {
                videosQuery = videosQuery.Where(v => v.CategoryId == catId);
            }

            if (rep != "")
            {
                videosQuery = videosQuery.Where(v => v.Reporters.Any(r => r.Name.Contains(rep)));
            }

            if (title != "")
            {
                videosQuery = videosQuery.Where(v => v.Title.Contains(title));
            }

            if (evtId.HasValue)
            {
                videosQuery = videosQuery.Where(v => v.SpecialEventId == evtId);
            }
            const int noResult = 15;
            var pageCount = (int)Math.Ceiling((double)videosQuery.Count() / noResult);
            int iPage;
            if (int.TryParse(page, out iPage))
            {
                videosQuery = videosQuery.Skip((iPage - 1) * noResult).Take(noResult);
            }

            var videos = videosQuery.OrderByDescending(v => v.DisplayTime).ToList();
            var filterView = new VideoFilterView
            {
                Category = cat,
                Page = iPage,
                PageCount = pageCount,
                Videos = videos,
                Range = rangeType,
                Reporter = rep
            };

            return PartialView("_AdminList", filterView);
        }

        public PartialViewResult ForCategory(int catId, DateTime? date, string groupBy = "")
        {
            var videoGroup = ForCat(catId, date, groupBy);
            return PartialView("ForCategory", videoGroup);
            
        }
        public ActionResult ForCategoryContent(int catId, DateTime? date, string groupBy = "")
        {
            var videoGroup = ForCat(catId, date, groupBy);
            return PartialView("ForCategoryContent", videoGroup);
        }
        public VideoGroup ForCat(int catId, DateTime? date, string groupBy)
        {
            var cat = _videocategoryRepository.Find(catId);
            if(! date.HasValue)
                date = DateTime.Now.Date;
            int type = 0;
            //default
            if (string.IsNullOrEmpty(groupBy))
            {
                if (cat.TypeId == 1 || cat.TypeId == 2) //tin tức
                    type = 0;
                else
                    type = 1;                
            }
            else 
            {
                type = groupBy == "week" ? 0 : groupBy == "month" ? 1 : 2;
            }
            if (type == 0) //tuần
            {
                var firstDay = Utilities.GetFirstDateOfWeek(date.Value, DayOfWeek.Monday);
                var lastDay = firstDay.AddDays(7);
                var videoGroup =  new VideoGroup
                    {
                        Date = firstDay,
                        Type = type,
                        Videos = _videoRepository.GetMany(v => v.CategoryId == catId && v.DisplayTime >= firstDay && v.DisplayTime < lastDay).ToList(),
                        CatId = catId
                    };
                return videoGroup;
            }
            else if(type == 1) //tháng
            {
                var firstDay = new DateTime(date.Value.Year, date.Value.Month, 1);
                var lastDay = firstDay.AddMonths(1);
                var videoGroup = new VideoGroup
                   {
                        Date = firstDay,
                        Type = type,
                        Videos =_videoRepository.GetMany(v => v.CategoryId == catId && v.DisplayTime >= firstDay && v.DisplayTime < lastDay).ToList(),
                        CatId = catId
                    };
                return videoGroup;
            }
            else//năm
            {
                var firstDay = new DateTime(date.Value.Year, 1, 1);
                var lastDay = new DateTime(date.Value.Year, 12, 31);
                var videoGroup = new VideoGroup
                {
                    Date = firstDay,
                    Type = type,
                    Videos = _videoRepository.GetMany(v => v.CategoryId == catId && v.DisplayTime >= firstDay && v.DisplayTime < lastDay).ToList(),
                    CatId = catId
                };
                return videoGroup;
            }
        }
        public PartialViewResult Filter(int? evtId, DateTime? begin, DateTime? end, string cat = "all", string rep = "", string title = "", string page = "1", RangeType rangeType = RangeType.All)
        {
            var now = DateTime.Now;
            var videosQuery = _videoRepository.All.Where(v => v.IsPublished && v.PublishedTime <= now);
            if (rangeType == RangeType.All)
            {
                //videosQuery = _videoRepository.All;
            }
            else if (rangeType == RangeType.Day)
            {
                var last1Day = DateTime.Now.AddDays(-1);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last1Day);
            }
            else if (rangeType == RangeType.Week)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= lastWeek);
            }
            else if (rangeType == RangeType.OneMonth)
            {
                var last1Month = DateTime.Now.AddMonths(-1);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last1Month);
            }
            else if (rangeType == RangeType.ThreeMonth)
            {
                var last3Month = DateTime.Now.AddMonths(-3);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last3Month);
            }
            else if (rangeType == RangeType.SixMonth)
            {
                var last6Month = DateTime.Now.AddMonths(-6);
                videosQuery = videosQuery.Where(v => v.DisplayTime >= last6Month);
            }
            else if (rangeType == RangeType.Custom) //custom
            {
                videosQuery = videosQuery.Where(v => v.IsPublished && v.DisplayTime >= begin && v.DisplayTime <= end);
            }

            int catId;
            if (cat != "all" && int.TryParse(cat, out catId))
            {
                videosQuery = videosQuery.Where(v => v.CategoryId == catId);
            }

            if (rep != "")
            {
                videosQuery = videosQuery.Where(v => v.Reporters.Any(r => r.Name.Contains(rep)));
            }

            if (title != "")
            {
                videosQuery = videosQuery.Where(v => v.Title.Contains(title));
            }

            if (evtId.HasValue)
            {
                videosQuery = videosQuery.Where(v => v.SpecialEventId == evtId);
            }
            const int noResult = 8;
            var pageCount = (int)Math.Ceiling((double)videosQuery.Count() / noResult);
            int iPage;
            if (int.TryParse(page, out iPage))
            {
                videosQuery = videosQuery.Skip((iPage - 1) * noResult).Take(noResult);
            }

            var videos = videosQuery.OrderByDescending(v => v.DisplayTime).ToList();
            var filterView = new VideoFilterView
            {
                Category = cat,
                Page = iPage,
                PageCount = pageCount,
                Videos = videos,
                Range = rangeType,
                //Type = type,
                Reporter = rep
            };

            return PartialView("_List", filterView);
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Create(int? scheduleDetailId)
        {
            if (scheduleDetailId == null)
            {
                return new HttpNotFoundResult();
            }

            var scheduleDetail = _scheduleDetailRepository.Find(scheduleDetailId.Value);
            ViewBag.categoryname = scheduleDetail.VideoCategory.UniqueTitle;
            ViewBag.catname = scheduleDetail.VideoCategory.Name;
            ViewBag.Reporters = _reporterRepository.All.ToList();
            ViewBag.PossibleCategories = _videocategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name);
            ViewBag.PossibleKeywords = _keywordRepository.All;
           
            ViewBag.PossibleSpecialEvent = _specialEventRepository.GetMany(s => s.StartDate <= scheduleDetail.DateTime && s.EndDate >= scheduleDetail.DateTime).ToList();
            String title;
            //Tin tức
            if (scheduleDetail.VideoCategory.GroupId == 2) //not good
            {
                title = "Bản tin " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(scheduleDetail.VideoCategory.Name.ToLower()) + " phát sóng " + scheduleDetail.DateTime.ToString("HH:mm dd/MM/yyyy");
                ViewBag.CanEditTitle = false;
            }
            else
            {
                title = scheduleDetail.Name;
                ViewBag.CanEditTitle = true;
            }

            var model = new VideoModel
            {
                CategoryId = scheduleDetail.VideoCategoryId,
                Title = title,
                DisplayTime = scheduleDetail.DateTime,
                ScheduleDetailId = scheduleDetailId.Value,
                UploaderId = User.Identity.GetUserId()
            };

            return View(model);
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase thumbnailFile, HttpPostedFileBase subtitleFile, VideoModel videoModel)
        {
            videoModel.UploadedTime = DateTime.Now;
            string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
            string physicalStoragePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain];
            if (thumbnailFile != null && thumbnailFile.ContentLength > 0)
            {
                VideoCategory vc = _videocategoryRepository.Find(videoModel.CategoryId);
                string categoryName = vc.UniqueTitle;
                DateTime displayTime = videoModel.DisplayTime;
                //string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnailFile.FileName;
                string extension = Path.GetExtension(thumbnailFile.FileName);
                string timeUpload = DateTime.Now.ToString("yyyyMMddHHmmss");
                string fileName = categoryName + "_" + displayTime.Year.ToString() + "-" + displayTime.Month.ToString() + "-" + displayTime.Day.ToString() + "-" + displayTime.Hour + "h" + displayTime.Minute + "m_" + timeUpload + extension;
                string mFileName = "Mobile_" + fileName;
                
                string folder = "UploadedImages/VideoThumbnail";
                
                string filePath = physicalStoragePath + @"\" + folder + @"\" + fileName;
                string mFilePath = physicalStoragePath + @"\" + folder + @"\" + mFileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    thumbnailFile.SaveAs(filePath);
                    ImageHelper.ScaleBySize(filePath, mFilePath, 200);
                }
                videoModel.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
                videoModel.MobileThumbnail = currentDomain + "/" + folder + "/" + mFileName;

                ModelState.Clear();
                TryValidateModel(videoModel);
            }
            //Subtitle
            if (subtitleFile != null && subtitleFile.ContentLength > 0)
            {
                var subExtention = ".vtt";
                var subFileName = Path.GetFileNameWithoutExtension(videoModel.Url) + subExtention;
                string filePath = physicalStoragePath + @"\uploadedvideos\" + subFileName;
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    subtitleFile.SaveAs(filePath);
                }
                videoModel.Subtitle = currentDomain + "/uploadedvideos/" + subFileName;
            }
            //if (!string.IsNullOrEmpty(videoModel.YoutubeUrl) && !VideoHelper.IsYouTubeUrl(videoModel.YoutubeUrl))
            //{
            //    ModelState.AddModelError("YoutubeUrl", "Đường dẫn youtube không đúng");
            //}

            if (ModelState.IsValid)
            {
                if (videoModel.PublishImmediately)
                {
                    videoModel.PublishedTime = DateTime.Now;
                    videoModel.PublishImmediately = false;
                }

                var video = AutoMapper.Mapper.Map<VideoModel, Video>(videoModel);

                if (!string.IsNullOrWhiteSpace(videoModel.Keywords))
                {
                    _videoRepository.Load(video, v => v.Keywords);
                    var keywords = videoModel.Keywords.Split(',');
                    foreach (string kw in keywords)
                    {
                        if (!string.IsNullOrWhiteSpace(kw))
                        {
                            var kwNormalize = kw.Trim();
                            var keyword = _keywordRepository.FindValue(kwNormalize);
                            if (keyword == null)
                                keyword = new Keyword() { Value = kwNormalize };
                            video.Keywords.Add(keyword);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(videoModel.ReporterIds))
                {
                    var reporters = videoModel.ReporterIds.Split(',');
                    foreach (string rpId in reporters)
                    {
                        if (!string.IsNullOrWhiteSpace(rpId))
                        {
                            var rpIdNormalize = rpId.Trim();
                            var reporter = _reporterRepository.Find(Convert.ToInt32(rpIdNormalize));
                            if (reporter != null)
                                video.Reporters.Add(reporter);
                        }
                    }
                }
                //video.Keywords = StringToKeywordList(videoView.Keywords);
                //Subtitle
                if (!string.IsNullOrEmpty(videoModel.Subtitle))
                {
                    video.Subtitles = new List<Subtitle> { new Subtitle {
                        Language = "en",
                        Source = videoModel.Subtitle
                    } };
                }
                _videoRepository.InsertOrUpdate(video);
                _videoRepository.Save();

                var scheduleDetail = _scheduleDetailRepository.Find(videoModel.ScheduleDetailId);
                scheduleDetail.VideoId = video.Id;
                _scheduleDetailRepository.Save();

                if (videoModel.IsPublished)
                    return RedirectToAction("Management", "ProgramScheduleDetails",
                                        new { date = scheduleDetail.DateTime.ToShortDateString() });
                else
                    return Json(new { success = true, time = DateTime.Now.ToString("dd/MM/yyyy HH:mm"), id = video.Id });

            }
            if (videoModel.IsPublished)
            {
                videoModel.IsPublished = false;
                ViewBag.Reporters = _reporterRepository.All;
                ViewBag.PossibleCategories = _videocategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name);
                ViewBag.PossibleKeywords = _keywordRepository.All;
                return View(videoModel);
            }
            else
                return Json(new { success = false });

        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Edit(int id, string tab = "edit", int week = 0)
        {
            var qParam = Request.UrlReferrer != null ? HttpUtility.ParseQueryString(Request.UrlReferrer.Query) : null;
            var subUrl = qParam == null || qParam.Count == 0 ? "?weektab=" + week : (qParam["weektab"] == null ? "&weektab=" + week : "");
            ViewBag.returnUrl = Request.UrlReferrer + subUrl;

            ViewBag.Reporters = _reporterRepository.All;
            ViewBag.PossibleCategories = _videocategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name);
            ViewBag.PossibleKeywords = _keywordRepository.All;
           
            var video = _videoRepository.Find(id);
            ViewBag.PossibleSpecialEvent = _specialEventRepository.GetMany(s => s.StartDate <= video.DisplayTime && s.EndDate >= video.DisplayTime).ToList();
            ViewBag.categoryname = video.Category.UniqueTitle;
            ViewBag.catname = video.Category.Name;
            ViewBag.tab = tab;
            if (video != null)
            {
                if (video.Category.GroupId == 2) //not good
                {
                    ViewBag.CanEditTitle = false;
                }
                else
                {
                    ViewBag.CanEditTitle = true;
                }
                var videoView = AutoMapper.Mapper.Map<Video, VideoModel>(video);
                return View(videoView);
            }

            return new HttpNotFoundResult("Không tìm thấy trang yêu cầu");
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase thumbnailFile, HttpPostedFileBase subtitleFile, VideoModel videoModel, string returnUrl)
        {
            string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
            var physicalStoragePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain];
            //Thay thế file thumbnail
            if (thumbnailFile != null && thumbnailFile.ContentLength > 0)
            {
                //Xóa file thumbnail cũ
                string oldFilePath = Helpers.UrlHelper.GetPhysicalPath(this, videoModel.Thumbnail);
                string oldMobileFilePath = Helpers.UrlHelper.GetPhysicalPath(this, videoModel.MobileThumbnail);
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    if (System.IO.File.Exists(oldMobileFilePath))
                    {
                        System.IO.File.Delete(oldMobileFilePath);
                    }
                }

                //string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnailFile.FileName;
                VideoCategory vc = _videocategoryRepository.Find(videoModel.CategoryId);
                string categoryName = vc.UniqueTitle;
                DateTime displayTime = videoModel.DisplayTime;
                //string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + thumbnailFile.FileName;
                string extension = Path.GetExtension(thumbnailFile.FileName);
                string timeUpload = DateTime.Now.ToString("yyyyMMddHHmmss");
                string fileName = categoryName + "_" + displayTime.Year.ToString() + "-" + displayTime.Month.ToString() + "-" + displayTime.Day.ToString() + "-" + displayTime.Hour + "h" + displayTime.Minute + "m_" + timeUpload + extension;
                string mFileName = "Mobile_" + fileName;
                string folder = "UploadedImages/VideoThumbnail";
                string filePath = physicalStoragePath + @"\" + folder + @"\" + fileName;
                string mFilePath = physicalStoragePath + @"\" + folder + @"\" + mFileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    thumbnailFile.SaveAs(filePath);
                    ImageHelper.ScaleBySize(filePath, mFilePath, 250);
                }
                
                videoModel.Thumbnail = currentDomain + "/" + folder + "/" + fileName;
                videoModel.MobileThumbnail = currentDomain + "/" + folder + "/" + mFileName;


                ModelState.Clear();
                TryValidateModel(videoModel);
            }
            //Subtitle
            if (subtitleFile != null && subtitleFile.ContentLength > 0)
            {
                var subExtention = ".vtt";
                var subFileName = Path.GetFileNameWithoutExtension(videoModel.Url) + subExtention;
                string filePath = physicalStoragePath + @"\uploadedvideos\" + subFileName;
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    subtitleFile.SaveAs(filePath);
                }
                videoModel.Subtitle = currentDomain + "/uploadedvideos/" + subFileName;
            }
            //if (!string.IsNullOrEmpty(videoModel.YoutubeUrl) && !VideoHelper.IsYouTubeUrl(videoModel.YoutubeUrl))
            //{
            //    ModelState.AddModelError("YoutubeUrl", "Đường dẫn youtube không đúng");
            //}

            if (ModelState.IsValid)
            {
                var oldVideo = _videoRepository.Find(videoModel.Id);
                var video = AutoMapper.Mapper.Map<VideoModel, Video>(videoModel);
                video.UploadedTime = oldVideo.UploadedTime;
                //Xóa video cũ
                if (oldVideo != null && oldVideo.Url != video.Url)
                {
                    string filePath = Helpers.UrlHelper.GetPhysicalPath(this, oldVideo.Url);
                    using (new Impersonator("uploaduser", "", "Upload@@123"))
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                //Keywords
                _videoRepository.Load(video, v => v.Keywords);
                video.Keywords.Clear();
                if (!string.IsNullOrWhiteSpace(videoModel.Keywords))
                {
                    var keywords = videoModel.Keywords.Split(',');
                    foreach (string kw in keywords)
                    {
                        if (!string.IsNullOrWhiteSpace(kw))
                        {
                            var kwNormalize = kw.Trim();
                            var keyword = _keywordRepository.FindValue(kwNormalize);
                            if (keyword == null)
                                keyword = new Keyword() { Value = kwNormalize };
                            video.Keywords.Add(keyword);
                        }
                    }
                }
                //Reporters
                if (!string.IsNullOrWhiteSpace(videoModel.ReporterIds))
                {
                    _videoRepository.Load(video, v => v.Reporters);
                    video.Reporters.Clear();
                    var reporters = videoModel.ReporterIds.Split(',');

                    foreach (string rpId in reporters)
                    {
                        if (!string.IsNullOrWhiteSpace(rpId))
                        {
                            var rpIdNormalize = rpId.Trim();
                            var reporter = _reporterRepository.Find(Convert.ToInt32(rpIdNormalize));
                            if (reporter != null)
                                video.Reporters.Add(reporter);
                        }
                    }
                }
                //Subtitle
                if (!string.IsNullOrEmpty(videoModel.Subtitle))
                {
                    video.Subtitles = new List<Subtitle> { new Subtitle {
                        Language = "en",
                        Source = videoModel.Subtitle
                    } };
                }
                _videoRepository.InsertOrUpdate(video);
                _videoRepository.Save();

                return RedirectToLocal(returnUrl);
            }

            ViewBag.Reporters = _reporterRepository.All;
            ViewBag.PossibleCategories = _videocategoryRepository.GetMany(cat => cat.Children.Count == 0, cat => cat.Name);
            ViewBag.PossibleKeywords = _keywordRepository.All;
            return View(videoModel);
        }

        public ActionResult GetThumbnail(string videoUrl, string time)
        {
            if (string.IsNullOrEmpty(videoUrl) || string.IsNullOrEmpty(time))
            {
                return Json(new { success = false, error = "video or time is null", errorcode = 1 }, JsonRequestBehavior.AllowGet);
            }

            string randomName = ImageHelper.RandomFileString();
            string physicalFolder = Helpers.UrlHelper.GetPhysicalFolder(this, videoUrl);
            string hostName = Helpers.UrlHelper.GetHostName(this, videoUrl);

            string thumbUrl = hostName + "/UploadedImages/VideoThumbnail/" + randomName + ".jpg";
            string thumbPath = physicalFolder + @"\uploadedimages\videothumbnail\" + randomName + ".jpg";

            string videoPath = Helpers.UrlHelper.GetPhysicalPath(this, videoUrl).Replace("/", @"\");

            string username = "uploaduser";
            string password = "Upload@@123";
            System.Security.SecureString ssPwd = new System.Security.SecureString();
            for (int x = 0; x < password.Length; x++)
            {
                ssPwd.AppendChar(password[x]);
            }
            string thumbArgs = "-i \"" + videoPath + "\" -vframes 1 -ss " + time + " -f image2 -vcodec mjpeg \"" +
                                thumbPath + "\"";

            var thumbProc = new Process
            {
                StartInfo =
                {
                    FileName = physicalFolder + @"\FFmpeg\ffmpeg.exe",
                    Arguments = thumbArgs,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    UserName = username,
                    Password = ssPwd,
                    LoadUserProfile = false,

                    RedirectStandardOutput = false
                }
            };

            try
            {
                thumbProc.Start();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message, errorcode = 2 }, JsonRequestBehavior.AllowGet);
            }
            thumbProc.WaitForExit();
            thumbProc.Close();


            string mobileThumbUrl = hostName + "/uploadedimages/videothumbnail/" + randomName + "_Mobile.jpg";
            string mobileThumbPath = physicalFolder + @"\uploadedimages\videothumbnail\" + randomName + "_Mobile.jpg";
            try
            {
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    ImageHelper.ScaleBySize(thumbPath, mobileThumbPath, 200);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message, errorcode = 3 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, url = thumbUrl, mobile_url = mobileThumbUrl }, JsonRequestBehavior.AllowGet);
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost]
        public ActionResult SetHot(int id)
        {
            var video = _videoRepository.Find(id);
            video.IsHot = !video.IsHot;
            _videoRepository.InsertOrUpdate(video);
            _videoRepository.Save();
            return RedirectToAction("Management");

        }
        
        public JsonResult DeleteTempFile(string tempVideoUrl)
        {
            using (new Impersonator("uploaduser", "", "Upload@@123"))
            {
                string filePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, tempVideoUrl);

                //var filePath = Server.MapPath(tempVideoUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReplaceVideo(string videoUrl, string tempVideoUrl)
        {
            string tempFilePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, tempVideoUrl);
            string filePath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, videoUrl);
            using (new Impersonator("uploaduser", "", "Upload@@123"))
            {
                if (System.IO.File.Exists(tempFilePath))
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    System.IO.File.Move(tempFilePath, filePath);
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [RoleAuthorize(Site = "admin")]
        public ActionResult Delete(int id)
        {
            return PartialView(_videoRepository.Find(id));
        }

        [ValidateAntiForgeryToken]
        [RoleAuthorize(Site = "admin")]
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id, string confirmText)
        {
            var video = _videoRepository.Find(id);
            var success = false;
            string error = "";
            if (video == null)
            {
                error = "Không tìm thấy video";
            }
            else if (confirmText.ToLower() != "đồng ý")
            {
                error = "Chuỗi nhập vào chưa đúng";
            }
            else
            {
                if (video.ScheduleDetails != null)
                {
                    video.ScheduleDetails.Clear();
                }

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    string thumbnailPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, video.Thumbnail);
                    if (System.IO.File.Exists(thumbnailPath))
                    {
                        System.IO.File.Delete(thumbnailPath);
                    }

                    string videoPath = VITV.Web.Helpers.UrlHelper.GetPhysicalPath(this, video.Url);

                    if (System.IO.File.Exists(videoPath))
                    {
                        System.IO.File.Delete(videoPath);
                    }
                }
                _videoRepository.Delete(id, User.Identity.Name, "Xóa video");
                _videoRepository.Save();
                success = true;
            }

            return Json(new { success = success, id = id, error = error });
        }
        public ActionResult FirstWeekPopover(int videoId)
        {
            var report = _videoRepository.GetFirstWeekReport(videoId, null, null);
            return PartialView("_FirstWeekReportPopover", report);
        }
        public ActionResult AllTimePopover(int videoId)
        {
            var report = _videoRepository.GetAllTimeReport(videoId);
            return PartialView("_AllTimeReportPopover", report);
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Management", "Video");
        }

        public ActionResult ShowOrHideVideo(int videoid)
        {
            var video = _videoRepository.Find(videoid);
            return PartialView("_ShowHideVideo", video);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _videocategoryRepository.Dispose();
                _videoRepository.Dispose();
                _keywordRepository.Dispose();
                _reporterRepository.Dispose();
                _videoTranscriptRepository.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}

