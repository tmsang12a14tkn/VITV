using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class ArticleHighlightAllRepository : IArticleHighlightAllRepository
    {
        private readonly VITVContext context;

        public ArticleHighlightAllRepository(VITVContext _context)
        {
            context = _context;
        }

        public ArticleHighlightAllRepository()
        {
            context = new VITVContext();
        }

        public IQueryable<ArticleHighlightAll> All
        {
            get { return context.ArticleHighlightAlls.OrderBy(art => art.Order); }
        }

        public IQueryable<ArticleHighlightAll> AllIncluding(params Expression<Func<ArticleHighlightAll, object>>[] includeProperties)
        {
            IQueryable<ArticleHighlightAll> query = context.ArticleHighlightAlls.OrderBy(art => art.Order);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ArticleHighlightAll Find(int id)
        {
            return context.ArticleHighlightAlls.Find(id);
        }

        public IEnumerable<ArticleHighlightAll> GetMany(Expression<Func<ArticleHighlightAll, bool>> where)
        {
            return context.ArticleHighlightAlls.Where(where).OrderBy(art => art.Order).ToList();
        }

        public int GetLastOrder()
        {
            var articleHLs = context.ArticleHighlightAlls.OrderByDescending(art => art.Order).FirstOrDefault();
            return articleHLs != null ? articleHLs.Order + 1 : 1;
        }

        public void InsertOrUpdate(ArticleHighlightAll articleHighlightAll)
        {
            var model = context.ArticleHighlightAlls.Find(articleHighlightAll.Id);
            if (model == null)
            {
                // New entity
                context.ArticleHighlightAlls.Add(articleHighlightAll);
            }
            else
            {
                // Existing entity
                context.Entry(model).State = EntityState.Detached;
                context.Entry(articleHighlightAll).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var articleHighlightAll = context.ArticleHighlightAlls.Find(id);
            context.ArticleHighlightAlls.Remove(articleHighlightAll);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }

    public interface IArticleHighlightAllRepository : IDisposable
    {
        IQueryable<ArticleHighlightAll> All { get; }
        IQueryable<ArticleHighlightAll> AllIncluding(params Expression<Func<ArticleHighlightAll, object>>[] includeProperties);
        ArticleHighlightAll Find(int id);
        IEnumerable<ArticleHighlightAll> GetMany(Expression<Func<ArticleHighlightAll, bool>> where);
        void InsertOrUpdate(ArticleHighlightAll articleHighlightAll);
        int GetLastOrder();
        void Delete(int id);
        void Save();
    }
}