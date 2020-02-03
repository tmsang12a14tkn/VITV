using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class ArticleHighlightCatRepository : IArticleHighlightCatRepository
    {
        private readonly VITVContext context;

        public ArticleHighlightCatRepository(VITVContext _context)
        {
            context = _context;
        }

        public ArticleHighlightCatRepository()
        {
            context = new VITVContext();
        }

        public IQueryable<ArticleHighlightCat> All
        {
            get { return context.ArticleHighlightCats.OrderBy(art => art.Order); }
        }

        public IQueryable<ArticleHighlightCat> AllIncluding(params Expression<Func<ArticleHighlightCat, object>>[] includeProperties)
        {
            IQueryable<ArticleHighlightCat> query = context.ArticleHighlightCats.OrderBy(art => art.Order);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ArticleHighlightCat Find(int id)
        {
            return context.ArticleHighlightCats.Find(id);
        }

        public IEnumerable<ArticleHighlightCat> GetMany(Expression<Func<ArticleHighlightCat, bool>> where)
        {
            return context.ArticleHighlightCats.Where(where).OrderBy(art => art.Order).ToList();
        }

        public int GetLastOrder()
        {
            var articleHLs = context.ArticleHighlightCats.OrderByDescending(art => art.Order).FirstOrDefault();
            return articleHLs != null ? articleHLs.Order + 1 : 1;
        }

        public void InsertOrUpdate(ArticleHighlightCat articleHighlightCat)
        {
            var model = context.ArticleHighlightCats.Find(articleHighlightCat.Id);
            if (model == null)
            {
                // New entity
                context.ArticleHighlightCats.Add(articleHighlightCat);
            }
            else
            {
                // Existing entity
                context.Entry(model).State = EntityState.Detached;
                context.Entry(articleHighlightCat).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var articleHighlightCat = context.ArticleHighlightCats.Find(id);
            context.ArticleHighlightCats.Remove(articleHighlightCat);
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

    public interface IArticleHighlightCatRepository : IDisposable
    {
        IQueryable<ArticleHighlightCat> All { get; }
        IQueryable<ArticleHighlightCat> AllIncluding(params Expression<Func<ArticleHighlightCat, object>>[] includeProperties);
        ArticleHighlightCat Find(int id);
        IEnumerable<ArticleHighlightCat> GetMany(Expression<Func<ArticleHighlightCat, bool>> where);
        void InsertOrUpdate(ArticleHighlightCat articleHighlightCat);
        int GetLastOrder();
        void Delete(int id);
        void Save();
    }
}