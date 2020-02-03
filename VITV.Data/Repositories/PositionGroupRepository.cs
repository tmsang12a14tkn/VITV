using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class PositionLevelRepository : IPositionLevelRepository
    {
    private readonly VITVContext _context;

        public PositionLevelRepository(VITVContext context)
        {
            _context = context;
        }

        public PositionLevelRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<PositionLevel> All
        {
            get { return _context.PositionLevels; }
        }

        public IQueryable<PositionLevel> AllIncluding(params Expression<Func<PositionLevel, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<PositionLevel, object>>, IQueryable<PositionLevel>>(_context.PositionLevels, (current, includeProperty) => current.Include(includeProperty));
        }

        public PositionLevel Find(int id)
        {
            return _context.PositionLevels.Find(id);
        }

        public IEnumerable<PositionLevel> GetMany(Expression<Func<PositionLevel, bool>> where)
        {
            return _context.PositionLevels.Where(where).ToList();
        }

        public void InsertOrUpdate(PositionLevel PositionLevel)
        {
            if (PositionLevel.Id == default(int)) {
                // New entity
                _context.PositionLevels.Add(PositionLevel);
            } else {
                // Existing entity
                _context.Entry(PositionLevel).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var PositionLevel = _context.PositionLevels.Find(id);
            _context.PositionLevels.Remove(PositionLevel);
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

    public interface IPositionLevelRepository : IDisposable
    {
        IQueryable<PositionLevel> All { get; }
        IQueryable<PositionLevel> AllIncluding(params Expression<Func<PositionLevel, object>>[] includeProperties);
        PositionLevel Find(int id);
        IEnumerable<PositionLevel> GetMany(Expression<Func<PositionLevel, bool>> where);
        void InsertOrUpdate(PositionLevel PositionLevel);
        void Delete(int id);
        void Save();
    }
}