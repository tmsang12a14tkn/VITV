using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using VITV_Web.DAL;
using VITV_Web.Helpers;
using VITV_Web.Models;
using VITV_Web.ViewModels;

namespace VITV_Web.Repositories
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
            get { return _context.Videos.OrderByDescending(video=> video.PublishedTime); }
        }

        public IQueryable<Video> AllIncluding(params Expression<Func<Video, object>>[] includeProperties)
        {
            IQueryable<Video> query = _context.Videos.OrderByDescending(video=> video.PublishedTime);
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(Video video, Expression<Func<Video, ICollection<TElement>>> includeProperty) where TElement : class
        {
            var oldVideo = _context.Videos.Find(video.Id);
            if(oldVideo != null)
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
            return _context.Videos.Where(where).OrderByDescending(v=>v.PublishedTime);
        }

        public void InsertOrUpdate(Video video)
        {
            if (video.Id == default(int)) {
                // New entity
                _context.Videos.Add(video);
            } else {
                // Existing entity
                _context.Entry(video).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var video = _context.Videos.Find(id);
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


        public int UpdateAccessCount(string url, string thumbnail, string title, string ipAddress)
        {
            var now = DateTime.Now;
            var page = _context.Pages.Find(url);
            if(page == null)
            {
                page = new Page
                {
                    Url = url,
                    Thumbnail = thumbnail,
                    Title = title
                };
                _context.Pages.Add(page);
            }
            else
            {
                page.Title = title; page.Thumbnail = thumbnail;
            }
            var pageAccess = _context.PageAccesses.Find(url, ipAddress, now.Date);
            if(pageAccess == null)
            {
                pageAccess = new PageAccess
                {
                    Count = 1,
                    IPAdress = ipAddress,
                    LastAccess = now,
                    Date = now.Date,
                    PageUrl = url
                };
                _context.PageAccesses.Add(pageAccess);
            }
            else if(DateTime.Now.Subtract(pageAccess.LastAccess).Minutes >= 5)
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
            return _context.PageAccesses.Where(va => va.Date > startDate && va.Date <= endDate).GroupBy(va => va.Date).OrderBy(g=>g.Key).Select(g => new PageAccessDate
            {
                Date = g.Key,
                IPCount = g.GroupBy(gi => gi.IPAdress).Count(),
                IPViewCount = g.Count(),
                HomePageCount = g.Where(gi=>gi.PageUrl == "/").Sum(gp=>gp.Count),
                PageViewCount = g.Sum(gp=>gp.Count)
            }).ToList();
        }

        public List<PageAccessDate> GetWeekAccessData(DateTime startDate, DateTime endDate)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var firstMonday = GetFirstMonday();
            return _context.PageAccesses.Where(va => va.Date > startDate && va.Date <= endDate).GroupBy(va => new { week = DbFunctions.DiffDays(firstMonday, va.Date).Value/7 }).OrderBy(g => g.Key).Select(g => new PageAccessDate
            {
                Date = DbFunctions.AddDays(firstMonday, 7*g.Key.week).Value,
                IPCount = g.GroupBy(gi => gi.IPAdress).Count(),
                IPViewCount = g.Count(),
                HomePageCount = g.Where(gi => gi.PageUrl == "/").Sum(gp => gp.Count),
                PageViewCount = g.Sum(gp => gp.Count)
            }).ToList();
        }

        public List<PageAccessDate> GetMonthAccessData(DateTime startDate, DateTime endDate)
        {
            return _context.PageAccesses.Where(va => va.Date > startDate && va.Date <= endDate).GroupBy(va => DbFunctions.CreateDateTime(va.Date.Year,va.Date.Month, 1,0,0,0)).OrderBy(g => g.Key).Select(g => new PageAccessDate
            {
                Date =g.Key.Value,
                IPCount = g.GroupBy(gi => gi.IPAdress).Count(),
                IPViewCount = g.Count(),
                HomePageCount = g.Where(gi => gi.PageUrl == "/").Sum(gp => gp.Count),
                PageViewCount = g.Sum(gp => gp.Count)
            }).ToList();
        }

        public List<PeriodVideoAccess> GetAccessDataByDate(DateTime date)
        {
            var result = _context.PageAccesses.Where(va => va.Date == date).GroupBy(va => va.Page).Select(gr => new PeriodVideoAccess
            {
                IPViewCount = gr.Count(),
                PageViewCount = gr.Sum(g=>g.Count),
                Url = gr.Key.Url,
                Thumbnail = gr.Key.Thumbnail,
                Title = gr.Key.Title
            }).OrderByDescending(ac=>ac.PageViewCount).ToList();
            return result;
        }
        public List<PeriodVideoAccess> GetAccessDataByWeek(DateTime date)
        {
            var firstMonday = GetFirstMonday();
            var result = _context.PageAccesses.Where(va => DbFunctions.DiffDays(date, va.Date) >= 0 && DbFunctions.DiffDays(date, va.Date) < 7).GroupBy(va => va.Page).Select(gr => new PeriodVideoAccess
            {
                IPViewCount = gr.Count(),
                PageViewCount = gr.Sum(g => g.Count),
                Url = gr.Key.Url,
                Thumbnail = gr.Key.Thumbnail,
                Title = gr.Key.Title
            }).OrderByDescending(ac => ac.PageViewCount).ToList();
            return result;
        }

        public List<PeriodVideoAccess> GetAccessDataByMonth(DateTime date)
        {
            var result = _context.PageAccesses.Where(va => va.Date.Month == date.Month && va.Date.Year == date.Year).GroupBy(va => va.Page).Select(gr => new PeriodVideoAccess
            {
                IPViewCount = gr.Count(),
                PageViewCount = gr.Sum(g => g.Count),
                Url = gr.Key.Url,
                Thumbnail = gr.Key.Thumbnail,
                Title = gr.Key.Title
            }).OrderByDescending(ac => ac.PageViewCount).ToList();
            return result;
        }

        private DateTime GetFirstMonday()
        {
            DateTime jan1 = new DateTime(1970, 1, 1);
            var mondayOffset = 7 - (jan1.DayOfWeek - DayOfWeek.Monday);
            mondayOffset = mondayOffset == 7 ? 0 : mondayOffset;
            return jan1.AddDays(mondayOffset);
        }
    }

    public interface IVideoRepository : IDisposable
    {
        IQueryable<Video> All { get; }
        IQueryable<Video> AllIncluding(params Expression<Func<Video, object>>[] includeProperties);
        void Load<TElement>(Video video, Expression<Func<Video, ICollection<TElement>>> includeProperty) where TElement : class;
        Video Find(int id);
        int UpdateAccessCount(string url, string thumbnail, string title, string ipAddress);
        //int AccessCount(int id);
        List<PageAccessDate> GetDateAccessData(DateTime startDate, DateTime endDate);
        List<PageAccessDate> GetWeekAccessData(DateTime startDate, DateTime endDate);
        List<PageAccessDate> GetMonthAccessData(DateTime startDate, DateTime endDate);
        List<PeriodVideoAccess> GetAccessDataByDate(DateTime date);
        List<PeriodVideoAccess> GetAccessDataByWeek(DateTime date);
        List<PeriodVideoAccess> GetAccessDataByMonth(DateTime date);
        IQueryable<Video> GetMany(Expression<Func<Video, bool>> where);
        void InsertOrUpdate(Video video);
        void Delete(int id);
        DbPropertyEntry GetProperty<TProperty>(Video video, Expression<Func<Video, TProperty>> property);
        void Save();
    }
}