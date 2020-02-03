using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV_Web.DAL;
using VITV_Web.Models;

namespace VITV_Web.Repositories
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
    }

    public interface IVideoCategoryRepository : IDisposable
    {
        IQueryable<VideoCategory> All { get; }
        IQueryable<VideoCategory> AllIncluding(params Expression<Func<VideoCategory, object>>[] includeProperties);
        VideoCategory Find(int id);
        VideoCategory Find(string title);
        IEnumerable<VideoCategory> GetMany<TKey>(Expression<Func<VideoCategory, bool>> where, Expression<Func<VideoCategory, TKey>> orderBy);
        IEnumerable<VideoCategory> GetMany<TKey>(Expression<Func<VideoCategory, bool>> where);
        void InsertOrUpdate(VideoCategory videocategory);
        void Delete(int id);
        void Save();
    }
}