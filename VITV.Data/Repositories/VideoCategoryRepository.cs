using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.ViewModels;

namespace VITV.Data.Repositories
{ 
    public class VideoCategoryRepository : IVideoCategoryRepository
    {
        private readonly VITVContext _context;

        public VideoCategoryRepository(VITVContext context)
        {
            _context = context;
        }

        public VideoCategoryRepository()
        {
            _context = new VITVContext();
        }
        public IQueryable<VideoCategory> All
        {
            get { return _context.VideoCategories; }
        }
        public IEnumerable<VideoCatType> AllType
        {
            get
            {
                var combineCatType = new VideoCatType {
                    Name = "Nhóm khác",
                    
                    VideoCategories = _context.VideoCategories.Where(vc=>vc.Children.Count >0 ).ToList()
                };
                var result = _context.VideoCatTypes.ToList();
                result.Add(combineCatType);
                return result;
            }
        }
        public IQueryable<VideoCategory> AllIncluding(params Expression<Func<VideoCategory, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<VideoCategory, object>>, IQueryable<VideoCategory>>(_context.VideoCategories, (current, includeProperty) => current.Include(includeProperty));
        }

        public VideoCategory Find(int id)
        {
            return _context.VideoCategories.Find(id);
        }
        public VideoCategory Find(string title)
        {
            return _context.VideoCategories.FirstOrDefault(cat=> cat.UniqueTitle == title);
        }
        public IEnumerable<VideoCategory> GetMany<TKey>(Expression<Func<VideoCategory, bool>> where, Expression<Func<VideoCategory, TKey>> orderBy)
        {
            return _context.VideoCategories.Where(where).OrderBy(orderBy).ToList();
        }

        public IEnumerable<VideoCategory> GetMany<TKey>(Expression<Func<VideoCategory, bool>> where)
        {
            return _context.VideoCategories.Where(where).ToList();
        }

        public void InsertOrUpdate(VideoCategory videocategory)
        {
            if (videocategory.Id == default(int)) {
                // New entity
                
                _context.VideoCategories.Add(videocategory);
            } else {
                // Existing entity
                _context.Entry(videocategory).State = EntityState.Modified;
            }
        }
        public CatWeekDateReport GetCatReportWeekDate(int catId, DateTime? from, DateTime? to)
        {
            var _from = from;
            var firstMonday = GetFirstMonday();

            var cat = _context.VideoCategories.Find(catId);
            var totalIPViewCount = _context.CategoryAccesses.Where(ca => ca.CategoryId == catId && (!from.HasValue || ca.Date >= from) && (!to.HasValue || ca.Date <= to)).Sum(ca => ca.IPViewCount);
            var daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>().ToList();
            var data = (from dow in daysOfWeek
                       from ca in _context.CategoryAccesses.Where(ca => SqlFunctions.DatePart("dw", ca.Date) - 1 == (int)dow && ca.CategoryId == catId && (!_from.HasValue || ca.Date >= _from) && (!to.HasValue || ca.Date <= to)).DefaultIfEmpty()
                        select new {dow, ca})
                        .GroupBy(x=>x.dow)
                        .Select(g =>new CatWeekDate
                        {
                            DOW = g.Key,
                            IPViewCount = g.Sum(x => x.ca == null? 0 : x.ca.IPViewCount),
                            PageViewCount = g.Sum(x => x.ca == null? 0 : x.ca.PageViewCount)
                        }).OrderBy(cwd=>cwd.DOW).ToList();
            foreach(var catWeekDate in data)
            {
                catWeekDate.PercentIPViewCount = catWeekDate.IPViewCount * 100D / totalIPViewCount;
            }
         
            var report = new CatWeekDateReport
            {
                CatId = cat.Id,
                CatName = cat.Name,
                Data = data
            };
            return report;
        }
        public CatWeekChangeReport GetCatReportWeekChange(int catId, DateTime from, DateTime to)
        {
            var _from = from;
            var from2 = from.AddDays(-7);
            var to2 = to.AddDays(-7);

            var cat = _context.VideoCategories.Find(catId);
            var totalIPViewCount = _context.CategoryAccesses.Where(ca => ca.CategoryId == catId && ca.Date >= from && ca.Date <= to).Sum(ca => ca.IPViewCount);
            var lastTotalIPViewCount = _context.CategoryAccesses.Where(ca => ca.CategoryId == catId && ca.Date >= from2 && ca.Date <= to2).DefaultIfEmpty().Sum(ca => ca == null?0:ca.IPViewCount);

            var daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>().ToList();
            var data = (from dow in daysOfWeek
                        from ca in _context.CategoryAccesses.Where(ca => SqlFunctions.DatePart("dw", ca.Date) - 1 == (int)dow && ca.CategoryId == catId && ca.Date >= _from && ca.Date <= to).DefaultIfEmpty()
                        select new { dow, ca })
                        .GroupBy(x => x.dow)
                        .Select(g => new CatWeekChange
                        {
                            DOW = g.Key,
                            IPViewCount = g.Sum(x => x.ca == null ? 0 : x.ca.IPViewCount),
                        }).OrderBy(cwd => cwd.DOW).ToList();

            var data2 = (from dow in daysOfWeek
                        from ca in _context.CategoryAccesses.Where(ca => SqlFunctions.DatePart("dw", ca.Date) - 1 == (int)dow && ca.CategoryId == catId && ca.Date >= from2 && ca.Date <= to2).DefaultIfEmpty()
                        select new { dow, ca })
                        .GroupBy(x => x.dow).ToDictionary(g=>g.Key, g=>g.Sum(x => x.ca == null ? 0 : x.ca.IPViewCount));

            foreach (var catWeekChange in data)
            {
                catWeekChange.LastIPViewCount = data2[catWeekChange.DOW];
                catWeekChange.Change = catWeekChange.IPViewCount - data2[catWeekChange.DOW];
                catWeekChange.PercentDayChange = catWeekChange.Change * 100D / catWeekChange.LastIPViewCount;
            }

            var report = new CatWeekChangeReport
            {
                CatId = cat.Id,
                CatName = cat.Name,
                Total = totalIPViewCount,
                LastTotal = lastTotalIPViewCount,
                Data = data
            };
            return report;
        }
        public CatWeekAvgChangeReport GetCatReportWeekAvgChange(int catId, DateTime from, DateTime to, double avgWeek)
        {
            var _from = from;
            var avgDay = avgWeek / 7;
            var cat = _context.VideoCategories.Find(catId);
            var totalIPViewCount = _context.CategoryAccesses.Where(ca => ca.CategoryId == catId && ca.Date >= from && ca.Date <= to).Sum(ca => ca.IPViewCount);

            var daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>().ToList();
            var data = (from dow in daysOfWeek
                        from ca in _context.CategoryAccesses.Where(ca => SqlFunctions.DatePart("dw", ca.Date) - 1 == (int)dow && ca.CategoryId == catId && ca.Date >= _from && ca.Date <= to).DefaultIfEmpty()
                        select new { dow, ca })
                        .GroupBy(x => x.dow)
                        .Select(g => new CatWeekAvgChange
                        {
                            DOW = g.Key,
                            IPViewCount = g.Sum(x => x.ca == null ? 0 : x.ca.IPViewCount),
                        }).OrderBy(cwd => cwd.DOW).ToList();

            foreach (var catWeekChange in data)
            {
                catWeekChange.Change = Math.Round(catWeekChange.IPViewCount - avgDay, 2);
            }

            var report = new CatWeekAvgChangeReport
            {
                CatId = cat.Id,
                CatName = cat.Name,
                Total = totalIPViewCount,
                TotalChange = Math.Round(totalIPViewCount - avgWeek,2),
                Data = data
            };
            return report;
        }
        public ParentCatWeekReport GetParentCategoryReport(int catId, int year)
        {
             var firstMonday = GetFirstMonday(year);
            var cat = _context.VideoCategories.Find(catId);
            var report = new ParentCatWeekReport
            {
                Id = cat.Id,
                CatName = cat.Name2,
                Weeks = new List<string>(),
                Children = _context.CategoryAccesses.Where(ca => ca.Category.ParentId == catId && ca.Date.Year == year).GroupBy(ca => ca.Category).Select(
                    g => new CatWeekReport
                    {
                        Id = g.Key.Id,
                        CatName = g.Key.Name2,
                        Data = g.GroupBy(ca => DbFunctions.DiffDays(firstMonday, ca.Date) / 7).Select(
                           g2 => new CatWeek
                           {
                               Week = g2.Key.Value,
                               IPViewCount = g2.Sum(ca => ca.IPViewCount),
                               PageViewCount = g2.Sum(ca => ca.PageViewCount),
                           }
                        )
                    }
                )
            };
            for(int i=0;i<54;i++)
            {
                report.Weeks.Add(firstMonday.AddDays(i * 7).ToString("dd/MM/yyyy") + " - " + firstMonday.AddDays(i * 7 + 7).ToString("dd/MM/yyyy"));
            }
            return report;
        }
        public void Delete(int id)
        {
            var videocategory = _context.VideoCategories.Find(id);
            _context.VideoCategories.Remove(videocategory);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose() 
        {
            _context.Dispose();
        }

        private DateTime GetFirstMonday(int year=1970)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            var mondayOffset = 7 - (jan1.DayOfWeek - DayOfWeek.Monday);
            mondayOffset = mondayOffset == 7 ? 0 : mondayOffset;
            return jan1.AddDays(mondayOffset);
        }
    }

    public interface IVideoCategoryRepository : IDisposable
    {
        IQueryable<VideoCategory> All { get; }
        IEnumerable<VideoCatType> AllType { get; }
        IQueryable<VideoCategory> AllIncluding(params Expression<Func<VideoCategory, object>>[] includeProperties);
        VideoCategory Find(int id);
        VideoCategory Find(string title);
        IEnumerable<VideoCategory> GetMany<TKey>(Expression<Func<VideoCategory, bool>> where, Expression<Func<VideoCategory, TKey>> orderBy);
        IEnumerable<VideoCategory> GetMany<TKey>(Expression<Func<VideoCategory, bool>> where);
        void InsertOrUpdate(VideoCategory videocategory);
        CatWeekDateReport GetCatReportWeekDate(int catId, DateTime? from, DateTime? to);
        CatWeekChangeReport GetCatReportWeekChange(int catId, DateTime from, DateTime to);
        CatWeekAvgChangeReport GetCatReportWeekAvgChange(int catId, DateTime from, DateTime to, double avgWeek);

        ParentCatWeekReport GetParentCategoryReport(int catId, int year);
        void Delete(int id);
        void Save();

        
    }
}