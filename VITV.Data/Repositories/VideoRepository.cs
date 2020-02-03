using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.ViewModels;
using VITV.Data.ViewModels.Portal;

namespace VITV.Data.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly VITVContext _context;

        public VideoRepository(VITVContext context)
        {
            _context = context;
        }

        public VideoRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Video> All
        {
            get { return _context.Videos.OrderByDescending(video => video.DisplayTime); }
        }

        public IQueryable<Video> AllIncluding(params Expression<Func<Video, object>>[] includeProperties)
        {
            IQueryable<Video> query = _context.Videos.OrderByDescending(video => video.DisplayTime);
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(Video video, Expression<Func<Video, ICollection<TElement>>> includeProperty) where TElement : class
        {
            var oldVideo = _context.Videos.Find(video.Id);
            if (oldVideo != null)
                _context.Entry(oldVideo).State = EntityState.Detached;

            _context.Videos.Attach(video);
            _context.Entry(video).Collection(includeProperty).Load();

        }

        public DbPropertyEntry GetProperty<TProperty>(Video video, Expression<Func<Video, TProperty>> property)
        {
            return _context.Entry(video).Property(property);
        }

        public Video Find(int id)
        {
            return _context.Videos.Find(id);
        }

        public IQueryable<Video> GetMany(Expression<Func<Video, bool>> where)
        {
            return _context.Videos.Where(where).OrderByDescending(v => v.DisplayTime);
        }

        public void InsertOrUpdate(Video video)
        {
            if (video.Id == default(int))
            {
                // New entity
                _context.Videos.Add(video);
            }
            else
            {
                // Existing entity
                _context.Entry(video).State = EntityState.Modified;
            }
        }

        public void Delete(int id, string userName, string note)
        {
            var video = _context.Videos.Find(id);
            var pages = _context.Pages.Where(p => p.VideoId == id).ToList();
            foreach (var page in pages)
            {
                _context.Pages.Remove(page);
            }
            var delLog = new DeletionLog
            {
                DeletionTime = DateTime.Now,
                CategoryName = video.Category.Name,
                CreationUser = video.UploadUser.UserName,
                DisplayTime = video.DisplayTime,
                VideoId = video.Id,
                VideoName = video.Title,
                DeletionUser = userName,
                Note = note
            };
            _context.DeletionLogs.Add(delLog);
            _context.Videos.Remove(video);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public int UpdateAccessCount(string url, string thumbnail, string title, string ipAddress, string os, string deviceModel, string deviceType, string deviceVendor, int? videoId, int? videoCatId, int? articleId, int? articleCatId)
        {
            var now = DateTime.Now;
            
            //LƯU THEO VIDEO ID
            
            if (videoCatId.HasValue && videoId.HasValue)
            {

                var catAccess = _context.CategoryAccesses.Find(videoCatId.Value, now.Date);
                if (catAccess == null)
                {
                    catAccess = new CategoryAccess
                    {
                        CategoryId = videoCatId.Value,
                        Date = now.Date
                    };
                    _context.CategoryAccesses.Add(catAccess);
                }
            
                var videoAccess = _context.VideoAccesses.Find(videoId.Value, now.Date);
                //Chưa có record nào trong database - video này chưa có bất kỳ lượt coi nào
                if (videoAccess == null)
                {
                    videoAccess = new VideoAccess
                    {
                        VideoId = videoId.Value,
                        Date = now.Date,
                        IPViewCount = 1,
                        PageViewCount = 1
                    };
                    catAccess.IPViewCount++;
                    catAccess.PageViewCount++;
                    _context.VideoAccesses.Add(videoAccess);
                }
                //Kiểm tra xem video đã được ip này coi chưa
                else
                {
                    var recentPageAccess = _context.PageAccesses.Where(p => p.IPAdress == ipAddress && p.Date == now.Date && p.Page.VideoId == videoId.Value).OrderByDescending(p => p.LastAccess).FirstOrDefault();
                    //Nếu ip này chưa coi
                    if (recentPageAccess==null)
                    {
                        videoAccess.IPViewCount++;
                        videoAccess.PageViewCount++;
                        catAccess.IPViewCount++;
                        catAccess.PageViewCount++;
                    }
                    //Nếu đã coi - chỉ cộng thêm cho PageViewCount
                    else if (DateTime.Now.Subtract(recentPageAccess.LastAccess).Minutes >= 5)
                    {
                        videoAccess.PageViewCount++;
                        catAccess.PageViewCount++;
                    }
                }

            }
            //LƯU ARTICLE - THEO ARTICLE ID
            

            //LƯU THEO URL
            var page = _context.Pages.Find(url);
            if (page == null)
            {
                page = new Page
                {
                    Url = url,
                    Thumbnail = thumbnail,
                    Title = title,
                    VideoId = videoId,
                    VideoCatId = videoCatId,
                    ArticleId = articleId,
                    ArticleCatId = articleCatId
                };
                _context.Pages.Add(page);
            }
            else
            {
                page.Title = title; page.Thumbnail = thumbnail; page.VideoId = videoId; page.VideoCatId = videoCatId;
            }

            var pageAccess = _context.PageAccesses.Find(url, ipAddress, now.Date, deviceType, deviceModel, os);
            if (pageAccess == null)
            {
                pageAccess = new PageAccess
                {
                    Count = 1,
                    OSName = os,
                    DeviceModel = deviceModel,
                    DeviceType = deviceType,
                    DeviceVendor = deviceVendor,
                    IPAdress = ipAddress,
                    LastAccess = now,
                    Date = now.Date,
                    PageUrl = url
                };

                _context.PageAccesses.Add(pageAccess);
            }
            else if (DateTime.Now.Subtract(pageAccess.LastAccess).Minutes >= 5)
            {
                pageAccess.Count += 1;
                pageAccess.LastAccess = DateTime.Now;
            }
            
            _context.SaveChanges();
            return _context.PageAccesses.Where(ac => ac.PageUrl == url).Sum(ac => ac.Count);
        }

        //public int AccessCount(int id)
        //{
        //    return _context.VideoAccesses.Where(ac => ac.VideoId == id).Sum(ac => ac.Count);
        //}


        public List<PageAccessDate> GetDateAccessData(DateTime startDate, DateTime endDate)
        {
            return _context.PageAccesses.Where(va => va.Date >= startDate && va.Date <= endDate).GroupBy(va => va.Date).OrderBy(g => g.Key).Select(g => new PageAccessDate
            {
                Date = g.Key,
                IPCount = g.GroupBy(gi => gi.IPAdress).Count(),
                IPViewCount = g.Count(),
                HomePageCount = g.Where(gi => gi.PageUrl == "/").Select(gp => (int?)gp.Count).Sum() ?? 0,
                PageViewCount = g.Sum(gp => gp.Count)
            }).ToList();
        }

        public List<PageAccessDate> GetWeekAccessData(DateTime startDate, DateTime endDate)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var firstMonday = GetFirstMonday(startDate.Year);
            return _context.PageAccesses.Where(va => va.Date >= startDate && va.Date <= endDate).GroupBy(va => new { week = DbFunctions.DiffDays(firstMonday, va.Date).Value / 7 }).OrderBy(g => g.Key).Select(g => new PageAccessDate
            {
                Date = DbFunctions.AddDays(firstMonday, 7 * g.Key.week).Value,
                IPCount = g.GroupBy(gi => gi.IPAdress).Count(),
                IPViewCount = g.Count(),
                HomePageCount = g.Where(gi => gi.PageUrl == "/").Sum(gp => gp.Count),
                PageViewCount = g.Sum(gp => gp.Count)
            }).ToList();
        }

        public List<PageAccessDate> GetMonthAccessData(DateTime startDate, DateTime endDate)
        {
            return _context.PageAccesses.Where(va => va.Date >= startDate && va.Date <= endDate).GroupBy(va => DbFunctions.CreateDateTime(va.Date.Year, va.Date.Month, 1, 0, 0, 0)).OrderBy(g => g.Key).Select(g => new PageAccessDate
            {
                Date = g.Key.Value,
                IPCount = g.GroupBy(gi => gi.IPAdress).Count(),
                IPViewCount = g.Count(),
                HomePageCount = g.Where(gi => gi.PageUrl == "/").Sum(gp => gp.Count),
                PageViewCount = g.Sum(gp => gp.Count)
            }).ToList();
        }

        public List<PageAccessDate> GetYearAccessData(DateTime startDate, DateTime endDate)
        {
            return _context.PageAccesses.Where(va => va.Date >= startDate && va.Date <= endDate).GroupBy(va => DbFunctions.CreateDateTime(va.Date.Year, 1, 1, 0, 0, 0)).OrderBy(g => g.Key).Select(g => new PageAccessDate
            {
                Date = g.Key.Value,
                IPCount = g.GroupBy(gi => gi.IPAdress).Count(),
                IPViewCount = g.Count(),
                HomePageCount = g.Where(gi => gi.PageUrl == "/").Sum(gp => gp.Count),
                PageViewCount = g.Sum(gp => gp.Count)
            }).ToList();
        }

        public CatAccessDataChart GetAllCatAccessDataByDate(DateTime startDate, DateTime endDate, string cat) //lay json hien thi len bieu do so sanh cac chuyen muc
        {
            //Category cha
            //var parentAccessData = _context.CategoryAccesses.Where(va => va.Date >= startDate && va.Date <= endDate  && va.Category.ParentId != null).GroupBy(pa => pa.Category.Parent).AsEnumerable().Select
            //    (g => new CatAccessDataChart
            //    {
            //        id = g.Key.Id,
            //        typeid = g.Key.TypeId,
            //        name = g.Key.Name2,
            //        data = g.GroupBy(ca => ca.Date).OrderBy(g2 => g2.Key).Select(g2 => new ArrayList { ToUnixTimestamp(g2.Key), g2.Sum(ca => ca.IPViewCount) })
            //    });
            int catId;
            if (int.TryParse(cat, out catId))
            {
                var videoCat = _context.VideoCategories.Find(catId);
                //Parent Category
                if (videoCat.Children != null && videoCat.Children.Count > 0)
                {
                    var result =
                        new CatAccessDataChart
                        {
                            data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.Category.ParentId == catId).OrderBy(ca => ca.Date).GroupBy(ca => ca.Date)
                                .AsEnumerable()
                                .Select(
                                   g => new ArrayList
                                    { ToUnixTimestamp(g.Key), g.Sum(ca=>ca.IPViewCount) }
                                ),
                            id = videoCat.Id,
                            typeid = videoCat.TypeId,
                            name = videoCat.Name2
                        };
                    return result;
                }
                //Child
                else
                {
                    var result =
                       new CatAccessDataChart
                       {
                           data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.CategoryId == catId).OrderBy(ca => ca.Date)
                               .AsEnumerable()
                               .Select(
                                   ca => new ArrayList
                                   { ToUnixTimestamp(ca.Date), ca.IPViewCount }
                               ),
                           id = videoCat.Id,
                           typeid = videoCat.TypeId,
                           name = videoCat.Name2
                       };
                    return result;
                }
            }   
            //Average
            else if (cat.StartsWith("tb_"))
            {
                int typeId;
                if(int.TryParse(cat.Substring(3), out typeId))
                {
                    var catType = _context.VideoCatTypes.Find(typeId);
                    var result = new CatAccessDataChart
                    {
                        data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.Category.TypeId == typeId).OrderBy(ca=>ca.Date).GroupBy(ca=>ca.Date)
                            .AsEnumerable()
                            .Select(
                                g => new ArrayList
                                { ToUnixTimestamp(g.Key), g.Sum(ca => ca.IPViewCount)/g.GroupBy(ca=>ca.CategoryId).Count() }
                            ),
                        typeid = typeId,
                        name = "Trung bình - " + catType.Name
                    };
                    return result;
                }
                return new CatAccessDataChart();
            }
            else
                return new CatAccessDataChart();
        }
        public CatAccessDataChart GetAllCatAccessDataByWeek(DateTime startDate, DateTime endDate, string cat) //lay json hien thi len bieu do so sanh cac chuyen muc theo tuan
        {
            var firstMonday = GetFirstMonday(startDate.Year);

            int catId;
            if (int.TryParse(cat, out catId))
            {
                var videoCat = _context.VideoCategories.Find(catId);
                //Parent Category
                if (videoCat.Children != null && videoCat.Children.Count > 0)
                {
                    var result =
                        new CatAccessDataChart
                        {
                            data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.Category.ParentId == catId).GroupBy(ca => DbFunctions.DiffDays(firstMonday, ca.Date) / 7).OrderBy(g => g.Key)
                                .AsEnumerable()
                                .Select(
                                   g => new ArrayList
                                    { ToUnixTimestamp(firstMonday.AddDays(7 * g.Key.Value)), g.Sum(ca=>ca.IPViewCount) }
                                ),
                            id = videoCat.Id,
                            typeid = videoCat.TypeId,
                            name = videoCat.Name2
                        };
                    return result;
                }
                //Child
                else
                {
                    var result =
                       new CatAccessDataChart
                       {
                           data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.CategoryId == catId).GroupBy(ca => DbFunctions.DiffDays(firstMonday, ca.Date) / 7).OrderBy(g => g.Key)
                               .AsEnumerable()
                               .Select(
                                   g => new ArrayList
                                   { ToUnixTimestamp(firstMonday.AddDays(7 * g.Key.Value)), g.Sum(ca=>ca.IPViewCount) }
                               ),
                           id = videoCat.Id,
                           typeid = videoCat.TypeId,
                           name = videoCat.Name2
                       };
                    return result;
                }
            }
            //Average
            else if (cat.StartsWith("tb_"))
            {
                int typeId;
                if (int.TryParse(cat.Substring(3), out typeId))
                {
                    var catType = _context.VideoCatTypes.Find(typeId);
                    var result = new CatAccessDataChart
                    {
                        data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.Category.TypeId == typeId).GroupBy(ca => DbFunctions.DiffDays(firstMonday, ca.Date) / 7).OrderBy(g=>g.Key)
                            .AsEnumerable()
                            .Select(
                                g => new ArrayList
                                { ToUnixTimestamp(firstMonday.AddDays(7 * g.Key.Value)), g.Sum(ca => ca.IPViewCount)/g.GroupBy(ca=>ca.CategoryId).Count() }
                            ),
                        typeid = typeId,
                        name = "Trung bình - " + catType.Name
                    };
                    return result;
                }
                return new CatAccessDataChart();
            }
            else
                return new CatAccessDataChart();
        }
        //test2
        public Tuple<List<CatAccessDataChart>, double, double> GetAllCatAccessDetailByWeekForChart(int catid, int year) //lay json hien thi len bieu do so sanh cac chuyen muc theo tuan
        {
            var firstMonday = GetFirstSunday(year);
            var from = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);
            List<CatAccessDataChart> list = new List<CatAccessDataChart>();
            double countVideoCate = _context.VideoCategories.Where(v => v.TypeId != null).ToList().Count();

            var result = _context.CategoryAccesses.Where(ca => ca.Date >= from && ca.Date <= end && ca.CategoryId == catid).GroupBy(catAccess => catAccess.Category).AsEnumerable().Select(
                    g => new CatAccessDataChart()
                    {
                        id = g.Key.Id,
                        typeid = g.Key.TypeId,
                        name = g.Key.Name2,
                        data = g.GroupBy(ca => (ca.Date - firstMonday).Days / 7 + 1).Select(g2 => new ArrayList { g2.Key, g2.Sum(ca => ca.IPViewCount) })
                    }
                 );

            var resultAvg = _context.CategoryAccesses
                .Where(ca => ca.Date >= from && ca.Date <= end)
                .GroupBy(ca => DbFunctions.DiffDays(firstMonday, ca.Date) / 7 + 1).AsEnumerable()
                .Select(
                    g => new ArrayList
                    { 
                        g.Key, Math.Round((double)g.Sum(ca => ca.IPViewCount) / countVideoCate, 2)
                    }
                );
            double avgAllVideoInWeek = Math.Round(resultAvg.Sum(v => (double)v[1]) / resultAvg.Count(), 2);
            CatAccessDataChart cat = new CatAccessDataChart();
            cat.id = 0;
            cat.typeid = 0;
            cat.name = "Trung bình tất cả video trong tuần";
            cat.data = resultAvg;
            list = result.ToList();
            list.Add(cat);

            var resultAvgAllWeek = _context.CategoryAccesses.Where(ca => ca.Date >= from && ca.Date <= end && ca.CategoryId == catid).AsEnumerable()
                .GroupBy(ca => (ca.Date - firstMonday).Days / 7 + 1).Select(
                    g => new { Id = catid, IpViewCount = g.Sum(ca => ca.IPViewCount), PageViewCount = g.Sum(ca => ca.PageViewCount), Week = g.Key }
                ).ToList();

            var countWeek = resultAvgAllWeek.Select(c => c.Week).Count();
            double countIpVewCount = resultAvgAllWeek.Select(c => c.IpViewCount).Sum();
            double avgAllWeek = Math.Round(countIpVewCount / countWeek, 2);
            return new Tuple<List<CatAccessDataChart>, double, double>(list, avgAllWeek, avgAllVideoInWeek);

        }
        //testsang
        //Xem trong tuan dau/Xem lai/Trung Binh
        public CompareVideoModel GetAllCatAccessDataType1(int catid, int year)
        {
            int? catTypeId = null;
            var specializeIds = new List<int> { 3, 4 }; //chi co chuyen de
            var cat = _context.VideoCategories.Find(catid);

            catTypeId = cat.TypeId;

            if (!catTypeId.HasValue || !specializeIds.Contains(catTypeId.Value))
            {
                return new CompareVideoModel();
            }
            var firstDayYear = new DateTime(year, 1, 1);
            var from = firstDayYear.AddDays(-(((int)firstDayYear.DayOfWeek + 6) % 7 + 1) + 1); //thứ 2 đầu tuần + 1
            var end = new DateTime(year, 12, 31);
            var videoReports = _context.Videos.Where(v => v.CategoryId == catid && v.DisplayTime >= from && v.DisplayTime <= end).AsEnumerable()
            .Select(v => new CompareVideoItemModel()
            {
                VideoId = v.Id,
                Title = v.Title,
                Month = v.DisplayTime.Month,
                Week = (int)(v.DisplayTime.Date - from).TotalDays / 7,
                VideoDuration = v.Duration.ToString(@"hh\:mm\:ss"),
                VideoPeriod = (DateTime.Now - v.DisplayTime).Days,
                DisplayTime = v.DisplayTime
            }).ToList();

            foreach (var vd in videoReports)
            {
                var firstWeekReport = GetFirstWeekReport(vd.VideoId, vd.DisplayTime, year);
                vd.FirstWeek = firstWeekReport;

                var alltimeReport = GetAllTimeReport(vd.VideoId);
                vd.AllTime = alltimeReport;

                vd.ReviewReport = new VideoReviewReport
                {
                    PageView = vd.AllTime.PageView - vd.FirstWeek.PageView,
                    ViewDateCount = vd.AllTime.ViewDateCount - vd.FirstWeek.ViewDateCount,
                };
            }

            var model = new CompareVideoModel
            {
                Videos = videoReports,
                FirstWeekView = videoReports.Sum(v => v.FirstWeek.PageView),
                AllTimeView = videoReports.Sum(v => v.AllTime.PageView),
                FirstWeekAverage = Math.Round(videoReports.Average(v => v.FirstWeek.PageView), 2),
                AllTimeAverage = Math.Round(videoReports.Average(v => v.AllTime.PageView), 2)
            };
            foreach (var vd in videoReports)
            {
                vd.FirstWeek.AverageChange = vd.FirstWeek.PageView - model.FirstWeekAverage;
                vd.FirstWeek.PercentPageView = Math.Round(vd.FirstWeek.PageView * 100D / model.FirstWeekView, 2);

                vd.AllTime.AverageChange = vd.AllTime.PageView - model.AllTimeAverage;
                vd.AllTime.PercentPageView = Math.Round(vd.AllTime.PageView * 100D / model.AllTimeView, 2);

                vd.ReviewReport.PercentPageView = Math.Round(vd.ReviewReport.PageView * 100D / vd.AllTime.PageView, 2);
            };
            return model;
        }
        //So sánh các tuần (không trùng, trùng)
        public CatVideosReport GetAllCatAccessDataType2(int catid, int year)
        {
            var specializeIds = new List<int> { 3, 4 }; //chi co chuyen de
            var cat = _context.VideoCategories.Find(catid);

            if (!cat.TypeId.HasValue || !specializeIds.Contains(cat.TypeId.Value))
            {
                return new CatVideosReport();
            }
            var firstMonday = GetFirstSunday(year);
            var firstDayYear = new DateTime(year, 1, 1);
            var from = firstDayYear.AddDays(-(((int)firstDayYear.DayOfWeek + 6) % 7 + 1) + 1); //thứ 2 đầu tuần + 1
            //var from = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);
            var list = new List<AccessDataChart>();
            var videoIds = _context.Videos.Where(v => v.CategoryId == catid && v.DisplayTime >= from && v.DisplayTime <= end).Select(v => v.Id).ToList();
            var allData = _context.PageAccesses
                    .Where(pa => pa.Page.VideoId != null && videoIds.Contains(pa.Page.VideoId.Value))
                    .GroupBy(pa => pa.Page.Video).AsEnumerable();
            var result = new CatVideosReport();
            result.Videos = allData.Select(vGroup => new VideoAccessReport
                {
                    Id = vGroup.Key.Id,
                    IPViewCount = vGroup.Count(),
                    Month = vGroup.Key.DisplayTime.Month,
                    Week = (int)(vGroup.Key.DisplayTime - from).TotalDays / 7,
                    PageViewCount = vGroup.Sum(p => p.Count),
                    Title = vGroup.Key.Title
                }).ToList();
            result.AvgIPViewCount = Math.Round(result.Videos.Average(c => c.IPViewCount), 2);
            result.AvgPageViewCount = Math.Round(result.Videos.Average(c => c.PageViewCount), 2);
            result.TotalIPViewCount = result.Videos.Sum(c => c.IPViewCount);
            result.TotalPageViewCount = result.Videos.Sum(c => c.PageViewCount);
            foreach (var vReport in result.Videos)
            {
                vReport.ReviewCount = vReport.PageViewCount - vReport.IPViewCount;
                vReport.IPViewChange = vReport.IPViewCount - result.AvgIPViewCount;
                vReport.PercentIPViewChange = Math.Round(vReport.IPViewChange * 100D / result.AvgIPViewCount);
                vReport.PageViewChange = vReport.PageViewCount - result.AvgPageViewCount;
                vReport.PercentPageViewChange = Math.Round(vReport.PageViewChange * 100D / result.AvgPageViewCount);
                vReport.PercentPageReviewCount = Math.Round(vReport.ReviewCount * 100D / vReport.PageViewCount, 2);
            }

            return result;
        }
        //public List<AccessDataChart> GetAllCatAccessDataType2(int catid, int year)
        //{
        //    //var firstMonday = GetFirstSunday(year);
        //    var firstDayYear = new DateTime(year, 1, 1);
        //    var from = firstDayYear.AddDays(-(((int)firstDayYear.DayOfWeek + 6) % 7 + 1) + 1); //thứ 2 đầu tuần + 1
        //    //var from = new DateTime(year, 1, 1);
        //    var end = new DateTime(year, 12, 31);
        //    var list = new List<AccessDataChart>();
        //    var videoIds = _context.Videos.Where(v => v.CategoryId == catid && v.DisplayTime >= from && v.DisplayTime <= end).Select(v => v.Id).ToList();
        //    var allData = _context.PageAccesses
        //            .Where(pa => pa.Page.VideoId != null && videoIds.Contains(pa.Page.VideoId.Value))
        //            .GroupBy(pa => pa.Page.Video).AsEnumerable();

        //    var ipView = new AccessDataChart()
        //    {
        //        name = "Xem mới",
        //    };
        //    var review = new AccessDataChart()
        //    {
        //        name = "Xem lại",
        //    };
        //    var ipViewData = new List<AccessData>();
        //    var reviewData = new List<AccessData>();
        //    foreach (var pageData in allData)
        //    {
        //        var ipViewCount = pageData.Count();
        //        var pageViewCount = pageData.Sum(pa => pa.Count);
        //        var reviewCount = pageViewCount - ipViewCount;
        //        var week = (int)(pageData.Key.DisplayTime - from).TotalDays / 7;
        //        ipViewData.Add(new AccessData
        //        {
        //            x = week,
        //            y = ipViewCount,
        //            name = pageData.Key.Title,
        //            //id = pageData.Key.Id,
        //            //month = pageData.Key.DisplayTime.Month
        //        });
        //        reviewData.Add(new AccessData
        //        {
        //            x = week,
        //            y = reviewCount,
        //            name = pageData.Key.Title
        //        });

        //    }
        //    ipView.data = ipViewData;
        //    review.data = reviewData;
        //    list.Add(review);
        //    list.Add(ipView);


        //    return list;
        //}

        public Tuple<List<AccessDataChart>, double> GetAllCatAccessDataType3(int catid, int year)
        {
            var specializeIds = new List<int> { 3, 4 }; //chi co chuyen de
            var cat = _context.VideoCategories.Find(catid);

            if (!cat.TypeId.HasValue || !specializeIds.Contains(cat.TypeId.Value))
            {
                return null;
            }
            //var firstMonday = GetFirstSunday(year);
            //var from = new DateTime(year, 1, 1);
            var firstDayYear = new DateTime(year, 1, 1);
            var from = firstDayYear.AddDays(-(((int)firstDayYear.DayOfWeek + 6) % 7 + 1) + 1); //thứ 2 đầu tuần + 1
            var end = new DateTime(year, 12, 31);
            var list = new List<AccessDataChart>();
            var newProgramData = new List<AccessData>();
            var otherProgramData = new List<AccessData>();
            var newProgram = new AccessDataChart()
            {
                name = "Chương trình mới",
            };
            var otherProgram = new AccessDataChart()
            {
                name = "Chương trình khác",
            };
            var videos = _context.Videos.Where(v => v.CategoryId == catid && v.DisplayTime >= from && v.DisplayTime <= end).ToList();
            foreach (var video in videos)
            {
                var week = (int)(video.DisplayTime - from).TotalDays / 7;
                var startDate = video.DisplayTime.Date;
                var endDate = video.DisplayTime.AddDays(6).Date;
                var weekViewCount = _context.PageAccesses.Where(pa => pa.Date >= startDate && pa.Date <= endDate && pa.Page.VideoCatId == video.CategoryId).Count();
                var newProgramViewCount = _context.PageAccesses.Where(pa => pa.Date >= startDate && pa.Date <= endDate && pa.Page.VideoId == video.Id).Count();

                newProgramData.Add(new AccessData
                {
                    x = week,
                    y = newProgramViewCount,
                    name = video.Title,
                    id = video.Id,
                    month = video.DisplayTime.Month
                });
                otherProgramData.Add(new AccessData
                {
                    x = week,
                    y = weekViewCount - newProgramViewCount,
                    name = video.Title
                });
            }

            newProgram.data = newProgramData;
            otherProgram.data = otherProgramData;
            list.Add(otherProgram);
            list.Add(newProgram);

            return new Tuple<List<AccessDataChart>, double>(list, 0);
        }

        public List<PageViewerShip> GetAllCatAccessDataByNumWeek(int catid, int year)
        {
            //var firstMonday = GetFirstSunday(year);

            var firstDayYear = new DateTime(year, 1, 1);
            var from = firstDayYear.AddDays(-(((int)firstDayYear.DayOfWeek + 6) % 7 + 1) + 1); //thứ 2 đầu tuần + 1

            //var from = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);

            var result = _context.CategoryAccesses.Where(ca => ca.Date >= from && ca.Date <= end && ca.CategoryId == catid).AsEnumerable()
                .GroupBy(ca => (ca.Date - from).Days / 7 + 1).Select(
                    g => new { Id = catid, IpViewCount = g.Sum(ca => ca.IPViewCount), PageViewCount = g.Sum(ca => ca.PageViewCount), Week = g.Key }
                ).OrderBy(x => x.Week).ToList();
            double AvgIpViewCount = (double)result.Sum(a => a.IpViewCount) / (double)result.Count();
            double AvgPageViewCount = (double)result.Sum(a => a.PageViewCount) / (double)result.Count();

            List<PageViewerShip> result1 = new List<PageViewerShip>();
            for (int i = 0; i < result.Count(); i++)
            {
                var g = result[i];
                var firstDay = from.AddDays(g.Week * 7 - 7);
                var lastDay = from.AddDays(g.Week * 7);
                var lastDayDisplay = from.AddDays(g.Week * 7 - 1);
                var videos = _context.Videos.Where(v => v.DisplayTime >= firstDay && v.DisplayTime < lastDay && v.CategoryId == catid).ToList();

                var pws = new PageViewerShip
                       {
                           Name = firstDay.ToString("dd/MM/yyyy") + " - " + lastDayDisplay.ToString("dd/MM/yyyy"),
                           Week = g.Week,
                           IPViewCount = g.IpViewCount,
                           AvgIPViewCount = AvgIpViewCount,
                           ChangeIPViewCount = 0,
                           PercentIPViewCount = 0,
                           ChangeAvgIPViewCount = Math.Round(g.IpViewCount - AvgIpViewCount, 2),
                           PerChangeAvgIPViewCount = Math.Round((g.IpViewCount - AvgIpViewCount) / AvgIpViewCount * 100, 2),
                           PageViewCount = g.PageViewCount,
                           AvgPageViewCount = AvgPageViewCount,
                           ChangePageViewCount = 0,
                           PercentPageViewCount = 0,
                           ChangeAvgPageViewCount = Math.Round(g.PageViewCount - AvgPageViewCount, 2),
                           PerChangeAvgPageViewCount = Math.Round((g.PageViewCount - AvgPageViewCount) / AvgPageViewCount * 100, 2),
                           ReviewCount = g.PageViewCount - g.IpViewCount,
                           PercentReviewCount = Math.Round(((double)g.PageViewCount - (double)g.IpViewCount) / (double)g.PageViewCount * 100.0, 2),
                           VideoCount = videos.Count(),
                           VideoTime = Math.Round(videos.Sum(v => v.Duration.TotalHours), 2),
                       };
                if (i > 0)
                {
                    pws.ChangeIPViewCount = Math.Round(pws.IPViewCount - result1[i - 1].IPViewCount, 2);
                    pws.PercentIPViewCount = Math.Round((pws.IPViewCount - result1[i - 1].IPViewCount) / result1[i - 1].IPViewCount * 100, 2);
                    pws.ChangePageViewCount = Math.Round(pws.PageViewCount - result1[i - 1].PageViewCount, 2);
                    pws.PercentPageViewCount = Math.Round((pws.PageViewCount - result1[i - 1].PageViewCount) / result1[i - 1].PageViewCount * 100, 2);
                }
                result1.Add(pws);
            }

            return result1;
        }

        public CatAccessDataChart GetAllCatAccessDataByMonth(DateTime startDate, DateTime endDate, string cat) //lay json hien thi len bieu do so sanh cac chuyen muc theo thang
        {
            int catId;
            if (int.TryParse(cat, out catId))
            {
                var videoCat = _context.VideoCategories.Find(catId);
                //Parent Category
                if (videoCat.Children != null && videoCat.Children.Count > 0)
                {
                    var result =
                        new CatAccessDataChart
                        {
                            data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.Category.ParentId == catId).GroupBy(ca => ca.Date.Year*12 + ca.Date.Month -1).OrderBy(g2 => g2.Key)
                                .AsEnumerable()
                                .Select(
                                   g => new ArrayList
                                    { ToUnixTimestamp(new DateTime(g.Key/12, g.Key%12 + 1,1)), g.Sum(ca=>ca.IPViewCount) }
                                ).ToList(),
                            id = videoCat.Id,
                            typeid = videoCat.TypeId,
                            name = videoCat.Name2
                        };
                    return result;
                }
                //Child
                else
                {
                    var result =
                       new CatAccessDataChart
                       {
                           data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.CategoryId == catId).GroupBy(ca => ca.Date.Year * 12 + ca.Date.Month - 1).OrderBy(g2 => g2.Key)
                               .AsEnumerable()
                               .Select(
                                   g => new ArrayList
                                   { ToUnixTimestamp(new DateTime(g.Key/12, g.Key%12 + 1,1)), g.Sum(ca=>ca.IPViewCount) }
                               ),
                           id = videoCat.Id,
                           typeid = videoCat.TypeId,
                           name = videoCat.Name2
                       };
                    return result;
                }
            }
            //Average
            else if (cat.StartsWith("tb_"))
            {
                int typeId;
                if (int.TryParse(cat.Substring(3), out typeId))
                {
                    var catType = _context.VideoCatTypes.Find(typeId);
                    var result = new CatAccessDataChart
                    {
                        data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.Category.TypeId == typeId).GroupBy(ca => ca.Date.Year * 12 + ca.Date.Month - 1).OrderBy(g2 => g2.Key)
                            .AsEnumerable()
                            .Select(
                                g => new ArrayList
                                { ToUnixTimestamp(new DateTime(g.Key/12, g.Key%12 + 1,1)), g.Sum(ca => ca.IPViewCount)/g.GroupBy(ca=>ca.CategoryId).Count() }
                            ),
                        typeid = typeId,
                        name = "Trung bình - " + catType.Name
                    };
                    return result;
                }
                return new CatAccessDataChart();
            }
            else
                return new CatAccessDataChart();
            
        }
        public CatAccessDataChart GetAllCatAccessDataByQuarter(DateTime startDate, DateTime endDate, string cat) //lay json hien thi len bieu do so sanh cac chuyen muc theo quy
        {
            int catId;
            if (int.TryParse(cat, out catId))
            {
                var videoCat = _context.VideoCategories.Find(catId);
                //Parent Category
                if (videoCat.Children != null && videoCat.Children.Count > 0)
                {
                    var result =
                        new CatAccessDataChart
                        {
                            data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.Category.ParentId == catId).GroupBy(ca => ca.Date.Year * 12 + ((ca.Date.Month - 1) / 3) * 3 + 1).OrderBy(g2 => g2.Key)
                                .AsEnumerable()
                                .Select(
                                   g => new ArrayList
                                    { ToUnixTimestamp(new DateTime(g.Key/12, g.Key%12, 1)), g.Sum(ca=>ca.IPViewCount) }
                                ),
                            id = videoCat.Id,
                            typeid = videoCat.TypeId,
                            name = videoCat.Name2
                        };
                    return result;
                }
                //Child
                else
                {
                    var result =
                       new CatAccessDataChart
                       {
                           data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.CategoryId == catId).GroupBy(ca => ca.Date.Year * 12 + ((ca.Date.Month - 1) / 3) * 3 + 1).OrderBy(g2 => g2.Key)
                               .AsEnumerable()
                               .Select(
                                   g => new ArrayList
                                   { ToUnixTimestamp(new DateTime(g.Key/12, g.Key%12, 1)), g.Sum(ca=>ca.IPViewCount) }
                               ),
                           id = videoCat.Id,
                           typeid = videoCat.TypeId,
                           name = videoCat.Name2
                       };
                    return result;
                }
            }
            //Average
            else if (cat.StartsWith("tb_"))
            {
                int typeId;
                if (int.TryParse(cat.Substring(3), out typeId))
                {
                    var catType = _context.VideoCatTypes.Find(typeId);
                    var result = new CatAccessDataChart
                    {
                        data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.Category.TypeId == typeId).GroupBy(ca => ca.Date.Year * 12 + ((ca.Date.Month - 1) / 3) * 3 + 1).OrderBy(g2 => g2.Key)
                            .AsEnumerable()
                            .Select(
                                g => new ArrayList
                                { ToUnixTimestamp(new DateTime(g.Key / 12, g.Key % 12, 1)), g.Sum(ca => ca.IPViewCount)/g.GroupBy(ca=>ca.CategoryId).Count() }
                            ),
                        typeid = typeId,
                        name = "Trung bình - " + catType.Name
                    };
                    return result;
                }
                return new CatAccessDataChart();
            }
            else
                return new CatAccessDataChart();
            
        }
        public CatAccessDataChart GetAllCatAccessDataByYear(DateTime startDate, DateTime endDate, string cat) //lay json hien thi len bieu do so sanh cac chuyen muc theo nam
        {
            int catId;
            if (int.TryParse(cat, out catId))
            {
                var videoCat = _context.VideoCategories.Find(catId);
                //Parent Category
                if (videoCat.Children != null && videoCat.Children.Count > 0)
                {
                    var result =
                        new CatAccessDataChart
                        {
                            data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.Category.ParentId == catId).GroupBy(ca => ca.Date.Year).OrderBy(g2 => g2.Key)
                                .AsEnumerable()
                                .Select(
                                   g => new ArrayList
                                    { ToUnixTimestamp(new DateTime(g.Key, 1, 1)), g.Sum(ca=>ca.IPViewCount) }
                                ),
                            id = videoCat.Id,
                            typeid = videoCat.TypeId,
                            name = videoCat.Name2
                        };
                    return result;
                }
                //Child
                else
                {
                    var result =
                       new CatAccessDataChart
                       {
                           data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.CategoryId == catId).GroupBy(ca => ca.Date.Year).OrderBy(g2 => g2.Key)
                               .AsEnumerable()
                               .Select(
                                   g => new ArrayList
                                   { ToUnixTimestamp(new DateTime(g.Key, 1, 1)), g.Sum(ca=>ca.IPViewCount) }
                               ),
                           id = videoCat.Id,
                           typeid = videoCat.TypeId,
                           name = videoCat.Name2
                       };
                    return result;
                }
            }
            //Average
            else if (cat.StartsWith("tb_"))
            {
                int typeId;
                if (int.TryParse(cat.Substring(3), out typeId))
                {
                    var catType = _context.VideoCatTypes.Find(typeId);
                    var result = new CatAccessDataChart
                    {
                        data = _context.CategoryAccesses.Where(ca => ca.Date >= startDate && ca.Date <= endDate && ca.Category.TypeId == typeId).GroupBy(ca => ca.Date.Year).OrderBy(g2 => g2.Key)
                            .AsEnumerable()
                            .Select(
                                g => new ArrayList
                                { ToUnixTimestamp(new DateTime(g.Key, 1, 1)), g.Sum(ca => ca.IPViewCount)/g.GroupBy(ca=>ca.CategoryId).Count() }
                            ),
                        typeid = typeId,
                        name = "Trung bình - " + catType.Name
                    };
                    return result;
                }
                return new CatAccessDataChart();
            }
            else
                return new CatAccessDataChart();
        }
        public List<PeriodVideoAccess> GetAccessDataInADate(DateTime date)
        {
            var result = _context.PageAccesses.Where(va => va.Date == date)
                                            .GroupBy(va => va.Page)
                                            .Select(gr => new PeriodVideoAccess
                                            {
                                                IPViewCount = gr.Count(),
                                                PageViewCount = gr.Sum(g => g.Count),
                                                Url = gr.Key.Url,
                                                Thumbnail = gr.Key.Thumbnail,
                                                Title = gr.Key.Title,
                                                VideoCatName = gr.Key.VideoCatId == null ? "" : gr.Key.VideoCat.Name
                                            }).OrderByDescending(ac => ac.PageViewCount).ToList();

            return result;
        }

        public List<DeviceReport> GetDeviceAccessDataByDate(DateTime from, DateTime to)
        {
            var all = _context.PageAccesses.Where(va => va.Date >= from && va.Date <= to);
            var totalIPView = all.Count();
            var totalPageView = all.Sum(pa => pa.Count);
            var result = _context.PageAccesses.Where(va => va.Date >= from && va.Date <= to)
                                            .GroupBy(va => va.DeviceType)
                                            .Select(gr => new DeviceReport
                                            {
                                                IPViewCount = gr.Count(),
                                                PageViewCount = gr.Sum(g => g.Count),
                                                Name = gr.Key
                                            }).ToList();
            foreach (var deviceReport in result)
            {
                deviceReport.PercentIPViewCount = Math.Round(deviceReport.IPViewCount * 100D / totalIPView, 2);
                deviceReport.PercentPageViewCount = Math.Round(deviceReport.PageViewCount * 100D / totalPageView, 2);
                deviceReport.DuplicateViewCount = deviceReport.PageViewCount - deviceReport.IPViewCount;
                deviceReport.PercentDuplicateViewCount = Math.Round(deviceReport.DuplicateViewCount * 100D / deviceReport.PageViewCount, 2);
            }

            return result;
        }
        public List<PeriodVideoAccess> GetCatAccessDataInADate(DateTime date)
        {
            List<IGrouping<int, PageAccess>> a = _context.PageAccesses.Where(va => va.Page.VideoCatId.HasValue && va.Date == date).GroupBy(va => va.Page.VideoCatId.Value).ToList();
            foreach (IGrouping<int, PageAccess> b in a)
            {

                List<PageAccess> z = b.ToList();
                var n = z.Count();
            }
            var result = _context.PageAccesses.Where(va => va.Page.VideoCatId.HasValue && va.Date == date).GroupBy(va => va.Page.VideoCat).Select(gr => new PeriodVideoAccess
            {
                IPViewCount = gr.Count(),
                PageViewCount = gr.Sum(g => g.Count),
                Url = "/chuong-trinh/" + gr.Key.UniqueTitle,
                Thumbnail = gr.Key.MobileProfilePhoto,
                Title = gr.Key.Name
            }).OrderByDescending(ac => ac.PageViewCount).ToList();
            return result;
        }
        public List<PeriodVideoAccess> GetAccessDataInAWeek(DateTime date)
        {
            var firstMonday = GetFirstMonday(date.Year);
            var result = _context.PageAccesses.Where(va => DbFunctions.DiffDays(date, va.Date) >= 0 && DbFunctions.DiffDays(date, va.Date) < 7).GroupBy(va => va.Page)
                .Select(gr => new PeriodVideoAccess
            {
                IPViewCount = gr.Count(),
                PageViewCount = gr.Sum(g => g.Count),
                Url = gr.Key.Url,
                Thumbnail = gr.Key.Thumbnail,
                Title = gr.Key.Title,
                VideoCatName = gr.Key.VideoCatId == null ? "" : gr.Key.VideoCat.Name
            }).OrderByDescending(ac => ac.PageViewCount).ToList();
            return result;
        }
        public List<PeriodVideoAccess> GetCatAccessDataInAWeek(DateTime date)
        {
            var firstMonday = GetFirstMonday(date.Year);
            var result = _context.PageAccesses.Where(va => va.Page.VideoCatId.HasValue && DbFunctions.DiffDays(date, va.Date) >= 0 && DbFunctions.DiffDays(date, va.Date) < 7).GroupBy(va => va.Page.VideoCat).Select(gr => new PeriodVideoAccess
            {
                IPViewCount = gr.Count(),
                PageViewCount = gr.Sum(g => g.Count),
                Url = "/chuong-trinh/" + gr.Key.UniqueTitle,
                Thumbnail = gr.Key.MobileProfilePhoto,
                Title = gr.Key.Name
            }).OrderByDescending(ac => ac.PageViewCount).ToList();
            return result;
        }

        public List<PeriodVideoAccess> GetAccessDataInAMonth(DateTime date)
        {
            var result = _context.PageAccesses.Where(va => va.Date.Month == date.Month && va.Date.Year == date.Year).GroupBy(va => va.Page)
                .Select(gr => new PeriodVideoAccess
            {
                IPViewCount = gr.Count(),
                PageViewCount = gr.Sum(g => g.Count),
                Url = gr.Key.Url,
                Thumbnail = gr.Key.Thumbnail,
                Title = gr.Key.Title,
                VideoCatName = gr.Key.VideoCatId == null ? "" : gr.Key.VideoCat.Name
            }).OrderByDescending(ac => ac.PageViewCount).ToList();
            return result;
        }
        public List<PeriodVideoAccess> GetCatAccessDataInAMonth(DateTime date)
        {
            var result = _context.PageAccesses.Where(va => va.Page.VideoCatId.HasValue && va.Date.Month == date.Month && va.Date.Year == date.Year).GroupBy(va => va.Page.VideoCat).Select(gr => new PeriodVideoAccess
            {
                IPViewCount = gr.Count(),
                PageViewCount = gr.Sum(g => g.Count),
                Url = "/chuong-trinh/" + gr.Key.UniqueTitle,
                Thumbnail = gr.Key.MobileProfilePhoto,
                Title = gr.Key.Name
            }).OrderByDescending(ac => ac.PageViewCount).ToList();
            return result;
        }
        public List<PeriodVideoAccess> GetAccessDataInAYear(DateTime date)
        {
            var result = _context.PageAccesses.Where(va => va.Date.Year == date.Year).GroupBy(va => va.Page)
                .Select(gr => new PeriodVideoAccess
            {
                IPViewCount = gr.Count(),
                PageViewCount = gr.Sum(g => g.Count),
                Url = gr.Key.Url,
                Thumbnail = gr.Key.Thumbnail,
                Title = gr.Key.Title,
                VideoCatName = gr.Key.VideoCatId == null ? "" : gr.Key.VideoCat.Name
            }).OrderByDescending(ac => ac.PageViewCount).ToList();
            return result;
        }
        public List<PeriodVideoAccess> GetCatAccessDataInAYear(DateTime date)
        {
            var result = _context.CategoryAccesses.Where(va => va.Date.Year == date.Year).GroupBy(va => va.Category).Select(gr => new PeriodVideoAccess
            {
                IPViewCount = gr.Sum(ca => ca.IPViewCount),
                PageViewCount = gr.Sum(ca => ca.PageViewCount),
                Url = "/chuong-trinh/" + gr.Key.UniqueTitle,
                Thumbnail = gr.Key.MobileProfilePhoto,
                Title = gr.Key.Name
            }).OrderByDescending(ac => ac.PageViewCount).ToList();
            return result;
        }
        //tam1
        public IEnumerable<AllCatReport> GetAccessDataBetween(DateTime from, DateTime to)
        {
            var start = DateTime.Now;
            var allCatAccess = _context.CategoryAccesses.Where(ca => ca.Date >= from && ca.Date <= to);
            if (allCatAccess.Count() == 0)
                return new List<AllCatReport>();
            var totalIPView = allCatAccess.Sum(ca => ca.IPViewCount);
            var totalPageView = allCatAccess.Sum(ca => ca.PageViewCount);

            var result = allCatAccess
                            .GroupBy(ca => (ca.Category.Parent != null ? ca.Category.Parent : ca.Category)).AsEnumerable()
                            .Select(gr => new AllCatReport
                            {
                                ParentId = gr.Key.Children.Count == 0 ? (int?)null : gr.Key.Id,
                                IPViewCount = gr.Sum(ca => ca.IPViewCount),
                                PageViewCount = gr.Sum(ca => ca.PageViewCount),
                                Title = gr.Key.Name2,
                                VideoCatId = gr.Key.Id,
                                TypeId = gr.Key.TypeId,
                                //PercentIPViewCount = Math.Round(gr.Sum(ca => ca.IPViewCount) * 100D / totalIPView, 2),
                                //PercentPageViewCount = Math.Round(gr.Sum(ca => ca.PageViewCount) * 100D / totalPageView, 2),
                                Type = gr.Key.Type != null ? gr.Key.Type.Name : "",
                                TotalVideos = gr.Key.Children.Count == 0 ? gr.Key.Videos.Count : gr.Key.Children.Sum(c => c.Videos.Count),
                                TotalDays = (gr.Max(ca => ca.Date) - gr.Min(ca => ca.Date)).Days,
                                TotalHourDuration = gr.Key.Children.Count == 0 ? gr.Key.Videos.Sum(v => v.Duration.TotalHours) : gr.Key.Children.SelectMany(v => v.Videos).Sum(v => v.Duration.TotalHours),
                                ViewDetail = gr.Key.Children.Count > 0,
                                MinDayView = gr.GroupBy(ca => ca.Date).Min(g2 => g2.Sum(ca => ca.IPViewCount)),
                                MaxDayView = gr.GroupBy(ca => ca.Date).Max(g2 => g2.Sum(ca => ca.IPViewCount)),
                            }).ToList();
            var midTime = DateTime.Now;
            foreach (var rp in result)
            {
                rp.PercentIPViewCount = Math.Round(rp.IPViewCount * 100D / totalIPView, 2);
                rp.PercentPageViewCount = Math.Round(rp.PageViewCount * 100D / totalPageView, 2);
                //var ipViewCountWeeks = allCatAccess.Where(ca => rp.VideoCatId == rp.ParentId ? ca.Category.ParentId == rp.ParentId : ca.CategoryId == rp.VideoCatId).GroupBy(ca => SqlFunctions.DatePart("week", ca.Date)).Select(g => g.Sum(ca => ca.IPViewCount)).ToList();
                //var avg = ipViewCountWeeks.Average(c => c);
                //rp.Stability = ipViewCountWeeks.Average(c => Math.Abs(c - avg)) / avg;
                var highestVideogroup = _context.VideoAccesses.Where(va => va.Date >= from && va.Date <= to && va.Video.CategoryId == rp.VideoCatId).OrderByDescending(va => va.IPViewCount).FirstOrDefault();
                    //_context.PageAccesses.Where(pa => pa.Page.VideoCatId == rp.VideoCatId && pa.Date >= from && pa.Date <= to).GroupBy(pa => pa.Page).OrderByDescending(g => g.Count()).FirstOrDefault();
                //rp.HighestVideo = highestVideogroup.Key.Video;
                if (highestVideogroup != null)
                {
                    rp.HighestVideoId = highestVideogroup.VideoId;
                    rp.HighestVideoCount = highestVideogroup.IPViewCount;
                }
            }
            var endTime = DateTime.Now;
            
            return result;
        }

        public List<AllCatReport> GetChildrenAccessDataBetween(DateTime from, DateTime to, int parentId)
        {
            var allCatAccess = _context.CategoryAccesses.Where(ca => ca.Date >= from && ca.Date <= to);
            var totalIPView = allCatAccess.Sum(ca => ca.IPViewCount);
            var totalPageView = allCatAccess.Sum(ca => ca.PageViewCount);
            var result = _context.CategoryAccesses.Where(ca => ca.Category.ParentId == parentId && ca.Date >= from && ca.Date <= to)
                                            .GroupBy(va => (va.Category)).AsEnumerable()
                                            .Select(gr => new AllCatReport
                                            {
                                                ParentId = gr.Key.Id,
                                                IPViewCount = gr.Sum(g => g.IPViewCount),
                                                PageViewCount = gr.Sum(g => g.PageViewCount),
                                                Title = "<a href='/VideoAccess/AllDetailReports?catid=" + gr.Key.Id.ToString() + "'>" + gr.Key.Name + "<a/>",
                                                VideoCatId = gr.Key.Id,
                                                TypeId = gr.Key.TypeId,
                                                //PercentIPViewCount = Math.Round(gr.Sum(g => g.IPViewCount) * 100D / totalIPView, 2),
                                                //PercentPageViewCount = Math.Round(gr.Sum(g => g.PageViewCount) * 100D / totalPageView, 2),
                                                Type = gr.Key.Type != null ? gr.Key.Type.Name : "",
                                                TotalVideos = gr.Key.Videos.Count,
                                                TotalDays = (gr.Max(ca => ca.Date) - gr.Min(ca => ca.Date)).Days,
                                                TotalHourDuration = gr.Key.Videos.Sum(v => v.Duration.TotalHours),
                                                MinDayView = gr.GroupBy(ca => ca.Date).Min(g2 => g2.Sum(ca => ca.IPViewCount)),
                                                MaxDayView = gr.GroupBy(ca => ca.Date).Max(g2 => g2.Sum(ca => ca.IPViewCount)),
                                            }).ToList();
            foreach (var rp in result)
            {
                rp.PercentIPViewCount = Math.Round(rp.IPViewCount * 100D / totalIPView, 2);
                rp.PercentPageViewCount = Math.Round(rp.PageViewCount * 100D / totalPageView, 2);
                //var ipViewCountWeeks = allCatAccess.Where(ca => rp.VideoCatId == rp.ParentId ? ca.Category.ParentId == rp.ParentId : ca.CategoryId == rp.VideoCatId).GroupBy(ca => SqlFunctions.DatePart("week", ca.Date)).Select(g => g.Sum(ca => ca.IPViewCount)).ToList();
                //var avg = ipViewCountWeeks.Average(c => c);
                //rp.Stability = ipViewCountWeeks.Average(c => Math.Abs(c - avg)) / avg;
                var highestVideogroup = _context.VideoAccesses.Where(va => va.Date >= from && va.Date <= to && va.Video.CategoryId == rp.VideoCatId).OrderByDescending(va => va.IPViewCount).FirstOrDefault();
                //_context.PageAccesses.Where(pa => pa.Page.VideoCatId == rp.VideoCatId && pa.Date >= from && pa.Date <= to).GroupBy(pa => pa.Page).OrderByDescending(g => g.Count()).FirstOrDefault();
                //rp.HighestVideo = highestVideogroup.Key.Video;
                if (highestVideogroup != null)
                {
                    rp.HighestVideoId = highestVideogroup.VideoId;
                    rp.HighestVideoCount = highestVideogroup.IPViewCount;
                }
            }
            return result;
        }

        public List<PeriodVideoAccess> GetAccessNotCombineDataBetween(DateTime from, DateTime to)
        {
            var allCatAccess = _context.CategoryAccesses.Where(ca => ca.Date >= from && ca.Date <= to);
            var totalIPView = allCatAccess.Sum(ca => ca.IPViewCount);
            var totalPageView = allCatAccess.Sum(ca => ca.PageViewCount);

            var result = allCatAccess.GroupBy(va => va.Category)
                            .Select(gr => new PeriodVideoAccess
                            {
                                IPViewCount = gr.Sum(g => g.IPViewCount),
                                PageViewCount = gr.Sum(g => g.PageViewCount),
                                Title = gr.Key.Name,
                                VideoCatId = gr.Key.Id
                            }).OrderByDescending(ac => ac.PageViewCount).ToList();
            result = result.OrderBy(p => p.TypeId).ToList();
            foreach (var p in result)
            {
                p.PercentIPViewCount = Math.Round(p.IPViewCount * 100D / totalIPView, 2);
                p.PercentPageViewCount = Math.Round(p.PageViewCount * 100D / totalPageView, 2);
                p.PercentReViewCount = Math.Round(Convert.ToDouble(p.PageViewCount - p.IPViewCount) / p.PageViewCount * 100D, 2);
            }
            return result;
        }

        public List<PeriodVideoAccess> GetCatAccessDataByTypeBetween(int typeId, DateTime from, DateTime to)
        {
            var result = _context.CategoryAccesses.Where(ca=>ca.Date >= from && ca.Date <= to && ca.Category.TypeId == typeId)
                                            .GroupBy(ca => ca.Category)
                                            .Select(gr => new PeriodVideoAccess
                                            {
                                                IPViewCount = gr.Sum(g=>g.IPViewCount),
                                                PageViewCount = gr.Sum(g => g.PageViewCount),
                                                Title = gr.Key.Name,
                                                VideoCatId = gr.Key.Id
                                            }).OrderByDescending(ac => ac.PageViewCount).ToList();

            var totalIPViewCount = result.Sum(p => p.IPViewCount);
            var totalPageViewCount = result.Sum(p => p.PageViewCount);

            result.ForEach(p =>
            {
                p.PercentIPViewCount = Math.Round(p.IPViewCount * 100D / totalIPViewCount, 2);
                p.PercentPageViewCount = Math.Round(p.IPViewCount * 100D / totalPageViewCount, 2);
            });
            
            return result;
        }

        public List<PeriodVideoAccess> GetCatCombineDataByParentBetween(CatCombineView catCombine, DateTime from, DateTime to)
        {
            var result = _context.CategoryAccesses.Where(ca => ca.Date >= from && ca.Date <= to && ca.Category.ParentId == catCombine.Id)
                                            .GroupBy(ca => ca.Category).AsEnumerable()
                                            .Select(gr => new PeriodVideoAccess
                                            {
                                                IPViewCount = gr.Sum(g => g.IPViewCount),
                                                PageViewCount = gr.Sum(g => g.PageViewCount),
                                                Title = gr.Key.Name,
                                                VideoCatId = gr.Key.Id
                                            }).OrderByDescending(ac => ac.PageViewCount).ToList();

            var totalIPViewCount = result.Sum(p => p.IPViewCount);
            var totalPageViewCount = result.Sum(p => p.PageViewCount);

            result.ForEach(p =>
            {
                p.PercentIPViewCount = Math.Round(p.IPViewCount * 100D / totalIPViewCount, 2);
                p.PercentPageViewCount = Math.Round(p.IPViewCount * 100D / totalPageViewCount, 2);
            });

            return result;
        }

        public List<PeriodVideoAccess> GetAccessDataByTypeBetween(int typeId, DateTime from, DateTime to)
        {
            var result = _context.VideoAccesses.Where(va => va.Date >= from && va.Date <= to && va.Video.Category.TypeId == typeId)
                                            .GroupBy(va => va.Video)
                                            .Select(gr => new PeriodVideoAccess
                                            {
                                                IPViewCount = gr.Sum(va=>va.IPViewCount),
                                                PageViewCount = gr.Sum(va => va.PageViewCount),
                                                Title = gr.Key.Title,
                                                VideoCatId = gr.Key.CategoryId,
                                                Video = gr.Key
                                                //Page = gr.FirstOrDefault().Page,
                                            }).OrderByDescending(ac => ac.PageViewCount).ToList();

            var totalIPViewCount = result.Sum(p => p.IPViewCount);
            var totalPageViewCount = result.Sum(p => p.PageViewCount);

            result.ForEach(p =>
            {
                p.PercentIPViewCount = p.IPViewCount * 100D / totalIPViewCount;
                p.PercentPageViewCount = p.IPViewCount * 100D / totalPageViewCount;
            });

            return result;
        }

        public CatVideosReport GetAccessDataByCatBetween(int catId, DateTime from, DateTime to)
        {
            var videos = _context.PageAccesses.Where(va => va.Page.VideoCatId.HasValue && va.Date >= from && va.Date <= to && va.Page.VideoCatId == catId)
                                            .GroupBy(va => va.Page.Video).AsEnumerable()
                                            .Select(gr => new VideoAccessReport
                                            {
                                                Id = gr.Key.Id,
                                                Month = gr.Key.DisplayTime.Month,
                                                Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(gr.Key.DisplayTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                                                IPViewCount = gr.Count(),
                                                PageViewCount = gr.Sum(g => g.Count),
                                                Title = gr.Key.Title,
                                                VideoCatId = gr.Key.CategoryId,
                                                VideoCatName = gr.Key.Category.Name,
                                                //Page = gr.Key,
                                            }).OrderByDescending(ac => ac.PageViewCount).ToList();

            var totalIPViewCount = videos.Sum(p => p.IPViewCount);
            var totalPageViewCount = videos.Sum(p => p.PageViewCount);
            var avgIPViewCount = Math.Round(totalIPViewCount * 1D / videos.Count, 2);
            var avgPageViewCount = Math.Round(totalPageViewCount * 1D / videos.Count, 2);

            videos.ForEach(p =>
            {
                p.ReviewCount = p.PageViewCount - p.IPViewCount;
                p.PercentPageReviewCount = Math.Round(p.ReviewCount * 100D / p.PageViewCount, 2);
                p.PercentIPViewCount = Math.Round(p.IPViewCount * 100D / totalIPViewCount, 2);
                p.PercentPageViewCount = Math.Round(p.IPViewCount * 100D / totalPageViewCount);
                p.IPViewChange = Math.Round(p.IPViewCount - avgIPViewCount, 2);
                p.PageViewChange = Math.Round(p.PageViewCount - avgPageViewCount, 2);
                p.PercentIPViewChange = Math.Round(p.IPViewChange * 100D / avgIPViewCount, 2);
                p.PercentPageViewChange = Math.Round(p.PageViewChange * 100D / avgPageViewCount, 2);
            });
            var result = new CatVideosReport
            {
                Videos = videos,
                TotalIPViewCount = totalIPViewCount,
                TotalPageViewCount = totalPageViewCount,
                TotalPageReviewCount = videos.Sum(p => p.ReviewCount),
                AvgIPViewCount = avgIPViewCount,
                AvgPageViewCount = avgPageViewCount
            };
            return result;
        }
        //testsang
        //Lấy dữ liệu cho trang thông kê từng video
        public IEnumerable<VideoPeriodAccessReport> GetVideoAccess(int videoId, string groupBy, DateTime from, DateTime to)
        {
            var result = _context.PageAccesses.Where(pa => pa.Page.VideoId == videoId && pa.Date >= from && pa.Date <= to).GroupBy(pa => pa.Date)
                .Select(g => new VideoPeriodAccessReport
                {
                    IPViewCount = g.Count(),
                    PageViewCount = g.Sum(pa => pa.Count),
                    Date = g.Key,


                }).OrderBy(r => r.Date)
                .AsEnumerable();

            return result;
        }
        public List<VideoCatType> CatTypeList()
        {
            return _context.VideoCatTypes.ToList();
        }

        public List<VideoUploadReportModel> GetVideoUploader(DateTime from, DateTime to)
        {
            var result = _context.Videos.Where(v => v.PublishedTime >= from && v.PublishedTime <= to && v.UploaderId != null)
                                        .GroupBy(v => v.UploadUser).AsEnumerable()
                                        .Select(g => new VideoUploadReportModel
                                        {
                                            Id = g.Key.Id,
                                            Username = g.Key.UserName,
                                            VideoCount = g.Count()
                                        }).OrderByDescending(g => g.VideoCount).ToList();

            return result;
        }

        public List<CatCombineView> GetCatCombineList(DateTime? from, DateTime? to)
        {
            var result = _context.CategoryAccesses.Where(ca => ca.Date >= from && ca.Date <= to && ca.Category.ParentId.HasValue).GroupBy(va => va.Category.Parent).Select(gr => new CatCombineView
            {
                Id = gr.Key.Id,
                Title = gr.Key.Name,
                IPViewCount = gr.Sum(ca => ca.IPViewCount),
                PageViewCount = gr.Sum(ca => ca.PageViewCount),
                PercentIPViewCount = 100,
                PercentPageViewCount = 100,
            }).ToList();

            return result;
        }

        public List<UploadReportModel> GetVideoUploadReport(DateTime today, string employeeid)
        {
            List<UploadReportModel> videos = new List<UploadReportModel>();
            if (String.IsNullOrEmpty(employeeid))
            {
                videos = _context.Videos.Where(v => v.UploadedTime.HasValue && v.UploadedTime.Value.Day == today.Day && v.UploadedTime.Value.Month == today.Month && v.UploadedTime.Value.Year == today.Year && v.UploaderId != null).AsEnumerable()
                .Select(u => new UploadReportModel
                {
                    empId = u.UploaderId,
                    Name = u.UploadUser.UserName,
                    Title = u.Title,
                    CategoryName = u.Category != null ? u.Category.Name : "",
                    //LiveTime = u.ScheduleDetails.Where(p => p.IsNew == true).OrderBy(p => p.DateTime).FirstOrDefault().DateTime,
                    PublishTime = u.PublishedTime,
                    UploadTime = u.UploadedTime,
                    DisplayTime = u.DisplayTime,
                    IsCheck = u.DisplayTime.Hour - u.ScheduleDetails.Where(p => p.IsNew == true).OrderBy(p => p.DateTime).FirstOrDefault().DateTime.Hour == 1 && u.PublishedTime < u.DisplayTime,
                }).ToList();
            }
            else
            {
                videos = _context.Videos.Where(v => v.UploadedTime.HasValue && v.UploadedTime.Value.Day == today.Day && v.UploadedTime.Value.Month == today.Month && v.UploadedTime.Value.Year == today.Year && v.UploaderId == employeeid && v.UploaderId != null).AsEnumerable()
                .Select(u => new UploadReportModel
                {
                    empId = u.UploaderId,
                    Name = u.UploadUser.UserName,
                    Title = u.Title,
                    CategoryName = u.Category != null ? u.Category.Name : "",
                    //LiveTime = u.ScheduleDetails.Where(p => p.IsNew == true).OrderBy(p => p.DateTime).FirstOrDefault().DateTime,
                    PublishTime = u.PublishedTime,
                    UploadTime = u.UploadedTime,
                    DisplayTime = u.DisplayTime,
                    IsCheck = u.DisplayTime.Hour - u.ScheduleDetails.Where(p => p.IsNew == true).OrderBy(p => p.DateTime).FirstOrDefault().DateTime.Hour == 1 && u.PublishedTime < u.DisplayTime,
                }).ToList();
            }
            return videos;
        }

        public IEnumerable<VideoAccessReport> GetTopVideo(int catId, DateTime from, string periodType)
        {
            var to = periodType == "week" ? from.AddDays(6) : periodType == "month" ? from.AddMonths(1).AddDays(-1) : periodType == "quater" ? from.AddMonths(3).AddDays(-1) : periodType == "year" ? from.AddYears(1).AddDays(-1) : from;
            var videos = _context.PageAccesses.Where(p => p.Date >= from && p.Date <= to && p.Page.VideoId != null && p.Page.VideoCatId == catId)
                .GroupBy(p => p.Page.Video)
                .OrderByDescending(g => g.Count())
                .Select(g => new VideoAccessReport
                {
                    Id = g.Key.Id,
                    IPViewCount = g.Count(),
                    Title = g.Key.Title
                })
                .Take(3);
            return videos;
        }
        public VideoFirstWeekReport GetFirstWeekReport(int videoId, DateTime? displayTime, int? year)
        {
            if (!displayTime.HasValue)
            {
                var video = _context.Videos.Find(videoId);
                displayTime = video.DisplayTime;
            }
            var fromDate = displayTime.Value.Date;
            var next7Days = displayTime.Value.Date.AddDays(6);
            if (year.HasValue)
            {
                if (displayTime.Value.Year < year)
                {
                    fromDate = new DateTime(year.Value, 1, 1); //tuần đầu tiên
                }
                if (next7Days.Year > displayTime.Value.Year)
                {
                    next7Days = new DateTime(displayTime.Value.Year, 12, 31); //tuần cuối
                }
            }


            var all = _context.PageAccesses.Where(pa => pa.Page.VideoId == videoId && pa.Date >= fromDate && pa.Date <= next7Days).ToList();
            var groupByDate = all.GroupBy(pa => pa.Date).OrderByDescending(g => g.Count());
            var highest = groupByDate.FirstOrDefault();
            var lowest = groupByDate.LastOrDefault();
            var result = new VideoFirstWeekReport
            {
                PageView = all.Count(),
                Highest = highest != null && highest.Count() > 0 ? highest.Count() : 0,
                Lowest = lowest != null && lowest.Count() > 0 ? lowest.Count() : 0,
                ViewDateCount = groupByDate.Count()
            };

            return result;
        }

        //testsang
        public VideoAllTimeReport GetAllTimeReport(int videoId)
        {
            var all = _context.PageAccesses.Where(pa => pa.Page.VideoId == videoId).ToList();
            var groupByDate = all.GroupBy(pa => pa.Date).OrderByDescending(g => g.Count());
            var highest = groupByDate.FirstOrDefault();
            var lowest = groupByDate.LastOrDefault();
            var result = new VideoAllTimeReport
            {
                PageView = all.Count(),
                Highest = highest != null && highest.Count() > 0 ? highest.Count() : 0,
                Lowest = lowest != null && lowest.Count() > 0 ? lowest.Count() : 0,
                ViewDateCount = groupByDate.Count()
            };

            return result;
        }
        //public List<UploadReportModel> GetVideoUploadReportByMonth(int month, int year, string employeeid)
        //{
        //    List<UploadReportModel> videos = new List<UploadReportModel>();
        //    if (String.IsNullOrEmpty(employeeid))
        //    {
        //        videos = _context.Videos.Where(v => v.PublishedTime.Month == month && v.PublishedTime.Year == year && v.UploaderId != null).AsEnumerable()
        //        .Select(u => new UploadReportModel
        //        {
        //            empId = u.UploaderId,
        //            Name = u.UploadUser.UserName,
        //            Title = u.Title,
        //            CategoryName = u.Category != null ? u.Category.Name : "",
        //            LiveTime = u.ScheduleDetails.Where(p => p.IsNew == true).OrderBy(p => p.DateTime).FirstOrDefault().DateTime,
        //            PublishTime = u.PublishedTime,
        //            UploadTime = u.UploadedTime,
        //            DisplayTime = u.DisplayTime,
        //            IsCheck = u.DisplayTime.Hour - u.ScheduleDetails.Where(p => p.IsNew == true).OrderBy(p => p.DateTime).FirstOrDefault().DateTime.Hour == 1 && u.PublishedTime < u.DisplayTime,
        //        }).ToList();
        //    }
        //    else
        //    {
        //        videos = _context.Videos.Where(v =>v.PublishedTime.Month == month && v.PublishedTime.Year == year && v.UploaderId == employeeid).AsEnumerable()
        //        .Select(u => new UploadReportModel
        //        {
        //            empId = u.UploaderId,
        //            Name = u.UploadUser.UserName,
        //            Title = u.Title,
        //            CategoryName = u.Category != null ? u.Category.Name : "",
        //            LiveTime = u.ScheduleDetails.Where(p => p.IsNew == true).OrderBy(p => p.DateTime).FirstOrDefault().DateTime,
        //            PublishTime = u.PublishedTime,
        //            UploadTime = u.UploadedTime,
        //            DisplayTime = u.DisplayTime,
        //            IsCheck = u.DisplayTime.Hour - u.ScheduleDetails.Where(p => p.IsNew == true).OrderBy(p => p.DateTime).FirstOrDefault().DateTime.Hour == 1 && u.PublishedTime < u.DisplayTime,
        //        }).ToList();
        //    }
        //    return videos;
        //}

        private DateTime GetFirstMonday(int year)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            var mondayOffset = 7 - (jan1.DayOfWeek - DayOfWeek.Monday);
            mondayOffset = mondayOffset == 7 ? 0 : mondayOffset;
            return jan1.AddDays(mondayOffset);
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
    }

    public interface IVideoRepository : IDisposable
    {
        IQueryable<Video> All { get; }
        IQueryable<Video> AllIncluding(params Expression<Func<Video, object>>[] includeProperties);
        void Load<TElement>(Video video, Expression<Func<Video, ICollection<TElement>>> includeProperty) where TElement : class;
        Video Find(int id);
        int UpdateAccessCount(string url, string thumbnail, string title, string ipAddress, string os, string deviceModel, string deviceType, string deviceVendor, int? videoId, int? videoCatId, int? articleId, int? articleCatId);
        //int AccessCount(int id);
        List<PageAccessDate> GetDateAccessData(DateTime startDate, DateTime endDate);
        List<PageAccessDate> GetWeekAccessData(DateTime startDate, DateTime endDate);
        List<PageAccessDate> GetMonthAccessData(DateTime startDate, DateTime endDate);
        List<PageAccessDate> GetYearAccessData(DateTime startDate, DateTime endDate);
        CatAccessDataChart GetAllCatAccessDataByDate(DateTime startDate, DateTime endDate, string cat); //lay du lieu hien thi len bieu do tong the chuyen muc - AllReports
        CatAccessDataChart GetAllCatAccessDataByWeek(DateTime startDate, DateTime endDate, string cat);
        CatAccessDataChart GetAllCatAccessDataByMonth(DateTime startDate, DateTime endDate, string cat);
        CatAccessDataChart GetAllCatAccessDataByQuarter(DateTime startDate, DateTime endDate, string cat);
        CatAccessDataChart GetAllCatAccessDataByYear(DateTime startDate, DateTime endDate, string cat);
        List<DeviceReport> GetDeviceAccessDataByDate(DateTime from, DateTime to);
        List<PeriodVideoAccess> GetAccessDataInADate(DateTime date); //lay du lieu truy cap theo tung video trong 1 ngay
        List<PeriodVideoAccess> GetCatAccessDataInADate(DateTime date); //lay du lieu truy cap theo tung chuyen muc trong 1 ngay
        List<PeriodVideoAccess> GetAccessDataInAWeek(DateTime date);
        List<PeriodVideoAccess> GetCatAccessDataInAWeek(DateTime date);
        List<PeriodVideoAccess> GetAccessDataInAMonth(DateTime date);
        List<PeriodVideoAccess> GetCatAccessDataInAMonth(DateTime date);
        List<PeriodVideoAccess> GetAccessDataInAYear(DateTime date);
        List<PeriodVideoAccess> GetCatAccessDataInAYear(DateTime date);
        IEnumerable<AllCatReport> GetAccessDataBetween(DateTime from, DateTime to);
        List<AllCatReport> GetChildrenAccessDataBetween(DateTime from, DateTime to, int parentId);

        //List<PeriodVideoAccess> GetCatAccessDataBetween  //lay du lieu truy cap cac chuyen muc gom nhom theo ngay trong khoang thoi gian
        List<PageViewerShip> GetAllCatAccessDataByNumWeek(int catid, int year);
        Tuple<List<CatAccessDataChart>, double, double> GetAllCatAccessDetailByWeekForChart(int catid, int year);
        CompareVideoModel GetAllCatAccessDataType1(int catid, int year);
        CatVideosReport GetAllCatAccessDataType2(int catid, int year);
        Tuple<List<AccessDataChart>, double> GetAllCatAccessDataType3(int catid, int year);
        List<PeriodVideoAccess> GetAccessNotCombineDataBetween(DateTime from, DateTime to);
        List<PeriodVideoAccess> GetCatAccessDataByTypeBetween(int typeId, DateTime from, DateTime to);
        List<PeriodVideoAccess> GetCatCombineDataByParentBetween(CatCombineView catCombine, DateTime from, DateTime to);
        List<PeriodVideoAccess> GetAccessDataByTypeBetween(int typeId, DateTime from, DateTime to);
        CatVideosReport GetAccessDataByCatBetween(int catId, DateTime from, DateTime to);
        IEnumerable<VideoPeriodAccessReport> GetVideoAccess(int videoId, string groupBy, DateTime from, DateTime to);
        List<VideoCatType> CatTypeList();
        List<VideoUploadReportModel> GetVideoUploader(DateTime from, DateTime to);

        List<CatCombineView> GetCatCombineList(DateTime? from, DateTime? to);
        List<UploadReportModel> GetVideoUploadReport(DateTime today, string employeeid);
        //List<UploadReportModel> GetVideoUploadReportByMonth(int month, int year, string employeeid);

        VideoFirstWeekReport GetFirstWeekReport(int videoId, DateTime? displayTime, int? year);
        VideoAllTimeReport GetAllTimeReport(int videoId);
        IEnumerable<VideoAccessReport> GetTopVideo(int catId, DateTime from, string periodType);
        IQueryable<Video> GetMany(Expression<Func<Video, bool>> where);
        void InsertOrUpdate(Video video);
        void Delete(int id, string userName, string note);
        DbPropertyEntry GetProperty<TProperty>(Video video, Expression<Func<Video, TProperty>> property);
        void Save();




    }
}