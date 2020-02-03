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
    public class ReporterRepository : IReporterRepository
    {
        private readonly VITVContext _context;

        public ReporterRepository(VITVContext context)
        {
            _context = context;
        }

        public ReporterRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Reporter> All
        {
            get { return _context.Reporters; }
        }

        public IQueryable<Reporter> AllIncluding(params Expression<Func<Reporter, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Reporter, object>>, IQueryable<Reporter>>(_context.Reporters, (current, includeProperty) => current.Include(includeProperty));
        }

        public Reporter Find(int id)
        {
            return _context.Reporters.Find(id);
        }

        public Reporter Find(string title)
        {
            return _context.Reporters.FirstOrDefault(r => r.UniqueTitle == title);
        }

        public void Load<TElement>(Reporter reporter, Expression<Func<Reporter, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.Reporters.Attach(reporter);
            _context.Entry(reporter).Collection(includeProperty).Load();
        }

        public IEnumerable<Reporter> GetMany(Expression<Func<Reporter, bool>> where)
        {
            return _context.Reporters.Where(where).ToList();
        }

        public void InsertOrUpdate(Reporter reporter)
        {
            if (reporter.Id == default(int)) {
                // New entity
                _context.Reporters.Add(reporter);
            } else {
                // Existing entity
                _context.Entry(reporter).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var reporter = _context.Reporters.Find(id);
            _context.Reporters.Remove(reporter);
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

    public interface IReporterRepository : IDisposable
    {
        IQueryable<Reporter> All { get; }
        IQueryable<Reporter> AllIncluding(params Expression<Func<Reporter, object>>[] includeProperties);
        Reporter Find(int id);
        Reporter Find(string title);
        IEnumerable<Reporter> GetMany(Expression<Func<Reporter, bool>> where);
        void Load<TElement>(Reporter reporter, Expression<Func<Reporter, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(Reporter reporter);
        void Delete(int id);
        void Save();
    }
}