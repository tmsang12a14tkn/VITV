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
    public class HolidayRepository : IHolidayRepository
    {
        private readonly VITVContext _context;

        public HolidayRepository(VITVContext context)
        {
            _context = context;
        }

        public HolidayRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Holiday> All
        {
            get { return _context.Holidays; }
        }

        public IQueryable<Holiday> AllIncluding(params Expression<Func<Holiday, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Holiday, object>>, IQueryable<Holiday>>(_context.Holidays, (current, includeProperty) => current.Include(includeProperty));
        }

        public Holiday Find(int id)
        {
            return _context.Holidays.Find(id);
        }

        public Holiday Find(string name)
        {
            return _context.Holidays.FirstOrDefault(r => r.Name == name);
        }

        public void Load<TElement>(Holiday Holiday, Expression<Func<Holiday, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.Holidays.Attach(Holiday);
            _context.Entry(Holiday).Collection(includeProperty).Load();
        }

        public IEnumerable<Holiday> GetMany(Expression<Func<Holiday, bool>> where)
        {
            return _context.Holidays.Where(where).ToList();
        }

        public void InsertOrUpdate(Holiday Holiday)
        {
            if (Holiday.Id == default(int)) {
                // New entity
                _context.Holidays.Add(Holiday);
            } else {
                // Existing entity
                _context.Entry(Holiday).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var Holiday = _context.Holidays.Find(id);
            _context.Holidays.Remove(Holiday);
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
    
    public interface IHolidayRepository : IDisposable
    {
        IQueryable<Holiday> All { get; }
        IQueryable<Holiday> AllIncluding(params Expression<Func<Holiday, object>>[] includeProperties);
        Holiday Find(int id);
        Holiday Find(string title);
        IEnumerable<Holiday> GetMany(Expression<Func<Holiday, bool>> where);
        void Load<TElement>(Holiday Holiday, Expression<Func<Holiday, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(Holiday Holiday);
        void Delete(int id);
        void Save();
    }
}