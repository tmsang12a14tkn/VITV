using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{ 
    public class KeywordRepository : IKeywordRepository
    {
        private readonly VITVContext _context;

        public KeywordRepository(VITVContext context)
        {
            _context = context;
        }

        public KeywordRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Keyword> All
        {
            get { return _context.Keywords; }
        }

        public IQueryable<Keyword> AllIncluding(params Expression<Func<Keyword, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Keyword, object>>, IQueryable<Keyword>>(_context.Keywords, (current, includeProperty) => current.Include(includeProperty));
        }

        public Keyword Find(int id)
        {
            return _context.Keywords.Find(id);
        }
        public Keyword FindValue(string value)
        {
            return _context.Keywords.FirstOrDefault(kw => kw.Value == value);
        }

        public void InsertOrUpdate(Keyword keyword)
        {
            if (keyword.Id == default(int)) {
                // New entity
                _context.Keywords.Attach(keyword);
                _context.Keywords.Add(keyword);
            } else {
                // Existing entity
                _context.Entry(keyword).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var keyword = _context.Keywords.Find(id);
            _context.Keywords.Remove(keyword);
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

    public interface IKeywordRepository : IDisposable
    {
        IQueryable<Keyword> All { get; }
        IQueryable<Keyword> AllIncluding(params Expression<Func<Keyword, object>>[] includeProperties);
        Keyword Find(int id);
        Keyword FindValue(string value);
        void InsertOrUpdate(Keyword keyword);
        void Delete(int id);
        void Save();
    }
}