using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using VITV_Web.DAL;
using VITV_Web.Models;

namespace VITV_Web.Repositories
{
    public class PositionGroupRepository : IPositionGroupRepository
    {
    private readonly VITVContext _context;

        public PositionGroupRepository(VITVContext context)
        {
            _context = context;
        }

        public PositionGroupRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<PositionGroup> All
        {
            get { return _context.PositionGroups; }
        }

        public IQueryable<PositionGroup> AllIncluding(params Expression<Func<PositionGroup, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<PositionGroup, object>>, IQueryable<PositionGroup>>(_context.PositionGroups, (current, includeProperty) => current.Include(includeProperty));
        }

        public PositionGroup Find(int id)
        {
            return _context.PositionGroups.Find(id);
        }

        public IEnumerable<PositionGroup> GetMany(Expression<Func<PositionGroup, bool>> where)
        {
            return _context.PositionGroups.Where(where).ToList();
        }

        public void InsertOrUpdate(PositionGroup PositionGroup)
        {
            if (PositionGroup.Id == default(int)) {
                // New entity
                _context.PositionGroups.Add(PositionGroup);
            } else {
                // Existing entity
                _context.Entry(PositionGroup).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var PositionGroup = _context.PositionGroups.Find(id);
            _context.PositionGroups.Remove(PositionGroup);
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

    public interface IPositionGroupRepository : IDisposable
    {
        IQueryable<PositionGroup> All { get; }
        IQueryable<PositionGroup> AllIncluding(params Expression<Func<PositionGroup, object>>[] includeProperties);
        PositionGroup Find(int id);
        IEnumerable<PositionGroup> GetMany(Expression<Func<PositionGroup, bool>> where);
        void InsertOrUpdate(PositionGroup PositionGroup);
        void Delete(int id);
        void Save();
    }
}