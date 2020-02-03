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
    public class SpecialEventRepository : ISpecialEventRepository
    {
        private readonly VITVContext _context;

        public SpecialEventRepository(VITVContext context)
        {
            _context = context;
        }

        public SpecialEventRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<SpecialEvent> All
        {
            get { return _context.SpecialEvents; }
        }

        public IQueryable<SpecialEvent> AllIncluding(params Expression<Func<SpecialEvent, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<SpecialEvent, object>>, IQueryable<SpecialEvent>>(_context.SpecialEvents, (current, includeProperty) => current.Include(includeProperty));
        }

        public SpecialEvent Find(int id)
        {
            return _context.SpecialEvents.Find(id);
        }

        public SpecialEvent Find(string name)
        {
            return _context.SpecialEvents.FirstOrDefault(r => r.Name == name);
        }

        public void Load<TElement>(SpecialEvent SpecialEvent, Expression<Func<SpecialEvent, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.SpecialEvents.Attach(SpecialEvent);
            _context.Entry(SpecialEvent).Collection(includeProperty).Load();
        }

        public IEnumerable<SpecialEvent> GetMany(Expression<Func<SpecialEvent, bool>> where)
        {
            return _context.SpecialEvents.Where(where).ToList();
        }

        public void InsertOrUpdate(SpecialEvent SpecialEvent)
        {
            if (SpecialEvent.Id == default(int)) {
                // New entity
                _context.SpecialEvents.Add(SpecialEvent);
            } else {
                // Existing entity
                _context.Entry(SpecialEvent).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var SpecialEvent = _context.SpecialEvents.Find(id);
            _context.SpecialEvents.Remove(SpecialEvent);
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
    public interface ISpecialEventRepository : IDisposable
    {
        IQueryable<SpecialEvent> All { get; }
        IQueryable<SpecialEvent> AllIncluding(params Expression<Func<SpecialEvent, object>>[] includeProperties);
        SpecialEvent Find(int id);
        SpecialEvent Find(string title);
        IEnumerable<SpecialEvent> GetMany(Expression<Func<SpecialEvent, bool>> where);
        void Load<TElement>(SpecialEvent SpecialEvent, Expression<Func<SpecialEvent, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(SpecialEvent SpecialEvent);
        void Delete(int id);
        void Save();
    }
}