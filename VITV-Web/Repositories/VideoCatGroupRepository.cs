using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV_Web.DAL;
using VITV_Web.Models;

namespace VITV_Web.Repositories
{ 
    public class VideoCatGroupRepository : IVideoCatGroupRepository
    {
        private readonly VITVContext _context;

        public VideoCatGroupRepository(VITVContext context)
        {
            _context = context;
        }

        public VideoCatGroupRepository()
        {
            _context = new VITVContext();
        }
        public IQueryable<VideoCatGroup> All
        {
            get { return _context.VideoCatGroups.OrderBy(group=> group.Order); }
        }

        public IQueryable<VideoCatGroup> AllIncluding(params Expression<Func<VideoCatGroup, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<VideoCatGroup, object>>, IQueryable<VideoCatGroup>>(_context.VideoCatGroups, (current, includeProperty) => current.Include(includeProperty));
        }

        public VideoCatGroup Find(int id)
        {
            return _context.VideoCatGroups.Find(id);
        }

        public VideoCatGroup Find(string unique)
        {
            return _context.VideoCatGroups.FirstOrDefault(v => v.UniqueTitle == unique);
        }
        public IEnumerable<VideoCatGroup> GetMany<TKey>(Expression<Func<VideoCatGroup, bool>> where, Expression<Func<VideoCatGroup, TKey>> orderBy)
        {
            return _context.VideoCatGroups.Where(where).OrderBy(orderBy).ToList();
        }
        public void InsertOrUpdate(VideoCatGroup videoCatGroup)
        {
            if (videoCatGroup.Id == default(int)) {
                // New entity
                _context.VideoCatGroups.Add(videoCatGroup);
            } else {
                // Existing entity
                _context.Entry(videoCatGroup).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var videoCatGroup = _context.VideoCatGroups.Find(id);
            _context.VideoCatGroups.Remove(videoCatGroup);
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

    public interface IVideoCatGroupRepository : IDisposable
    {
        IQueryable<VideoCatGroup> All { get; }
        IQueryable<VideoCatGroup> AllIncluding(params Expression<Func<VideoCatGroup, object>>[] includeProperties);
        VideoCatGroup Find(int id);
        VideoCatGroup Find(string unique);
        IEnumerable<VideoCatGroup> GetMany<TKey>(Expression<Func<VideoCatGroup, bool>> where, Expression<Func<VideoCatGroup, TKey>> orderBy);
        void InsertOrUpdate(VideoCatGroup videoCatGroup);
        void Delete(int id);
        void Save();
    }
}