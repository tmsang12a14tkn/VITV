using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class ArticleRevisionRepository : IArticleRevisionRepository
    {
        private readonly VITVContext _context;

        public ArticleRevisionRepository(VITVContext context)
        {
            _context = context;
        }

        public ArticleRevisionRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<ArticleRevision> All
        {
            get { return _context.ArticleRevisions; }
        }

        public IQueryable<ArticleRevision> AllIncluding(params Expression<Func<ArticleRevision, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<ArticleRevision, object>>, IQueryable<ArticleRevision>>(_context.ArticleRevisions, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(ArticleRevision articleRevision, Expression<Func<ArticleRevision, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.ArticleRevisions.Attach(articleRevision);
            _context.Entry(articleRevision).Collection(includeProperty).Load();            
        }

        public ArticleRevision Find(int id)
        {
            return _context.ArticleRevisions.Find(id);
        }

        public IEnumerable<ArticleRevision> GetMany<TKey>(Expression<Func<ArticleRevision, bool>> where, Expression<Func<ArticleRevision, TKey>> orderBy)
        {
            return _context.ArticleRevisions.Where(where).OrderBy(orderBy).ToList();
        }

        public void InsertOrUpdate(ArticleRevision articleRevision)
        {
            if (articleRevision.Id == default(int))
            {
                // New entity
                _context.ArticleRevisions.Add(articleRevision);
            } 
            else {
                // Existing entity
                _context.Entry(articleRevision).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var articleRevision = _context.ArticleRevisions.Find(id);
            _context.ArticleRevisions.Remove(articleRevision);
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

    public interface IArticleRevisionRepository : IDisposable
    {
        IQueryable<ArticleRevision> All { get; }
        IQueryable<ArticleRevision> AllIncluding(params Expression<Func<ArticleRevision, object>>[] includeProperties);
        ArticleRevision Find(int id);
        IEnumerable<ArticleRevision> GetMany<TKey>(Expression<Func<ArticleRevision, bool>> where, Expression<Func<ArticleRevision, TKey>> orderBy);
        void Load<TElement>(ArticleRevision articleRevision, Expression<Func<ArticleRevision, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(ArticleRevision articleRevision);
        void Delete(int id);
        void Save();
    }
}
