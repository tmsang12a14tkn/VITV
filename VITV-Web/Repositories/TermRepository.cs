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
    public class TermRepository : ITermRepository
    {
        private readonly VITVContext _context;

        public TermRepository(VITVContext context)
        {
            _context = context;
        }

        public TermRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Term> All
        {
            get { return _context.Terms; }
        }

        public IQueryable<Term> AllIncluding(params Expression<Func<Term, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Term, object>>, IQueryable<Term>>(_context.Terms, (current, includeProperty) => current.Include(includeProperty));
        }

        public Term Find(int id)
        {
            return _context.Terms.Find(id);
        }

        public Term Find(string name)
        {
            return _context.Terms.FirstOrDefault(r => r.Name == name);
        }

        public void Load<TElement>(Term term, Expression<Func<Term, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.Terms.Attach(term);
            _context.Entry(term).Collection(includeProperty).Load();
        }

        public IEnumerable<Term> GetMany(Expression<Func<Term, bool>> where)
        {
            return _context.Terms.Where(where).ToList();
        }

        public void InsertOrUpdate(Term term)
        {
            if (term.Id == default(int))
            {
                // New entity
                _context.Terms.Add(term);
            } else {
                // Existing entity
                _context.Entry(term).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var term = _context.Terms.Find(id);
            _context.Terms.Remove(term);
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

    public interface ITermRepository : IDisposable
    {
        IQueryable<Term> All { get; }
        IQueryable<Term> AllIncluding(params Expression<Func<Term, object>>[] includeProperties);
        Term Find(int id);
        Term Find(string name);
        IEnumerable<Term> GetMany(Expression<Func<Term, bool>> where);
        void Load<TElement>(Term term, Expression<Func<Term, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(Term term);
        void Delete(int id);
        void Save();
    }
}