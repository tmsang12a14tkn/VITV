using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class ArticleSeriesRepository : IArticleSeriesRepository
    {
        private readonly VITVContext context;

        public ArticleSeriesRepository(VITVContext _context)
        {
            context = _context;
        }

        public ArticleSeriesRepository()
        {
            context = new VITVContext();
        }

        public IQueryable<ArticleSeries> All
        {
            get { return context.ArticleSeries.OrderByDescending(art => art.Order); }
        }

        public IQueryable<ArticleSeries> AllIncluding(params Expression<Func<ArticleSeries, object>>[] includeProperties)
        {
            IQueryable<ArticleSeries> query = context.ArticleSeries.OrderByDescending(art => art.Order);
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ArticleSeries Find(int id)
        {
            return context.ArticleSeries.Find(id);
        }

        public IEnumerable<ArticleSeries> GetMany(Expression<Func<ArticleSeries, bool>> where)
        {
            return context.ArticleSeries.Where(where).OrderBy(art => art.Order).ToList();
        }

        public void InsertOrUpdate(ArticleSeries articleSeries)
        {
            if (articleSeries.Id == default(int))
            {
                // New entity
                context.ArticleSeries.Add(articleSeries);
            } else {
                // Existing entity
                context.Entry(articleSeries).State = EntityState.Modified;
            }
        }

        public int GetLastOrder()
        {      
            var articleSeries = context.ArticleSeries.OrderByDescending(art => art.Order).FirstOrDefault();
            return articleSeries != null ? articleSeries.Order + 1 : 1;
        }

        public void Delete(int id)
        {
            var articleSeries = context.ArticleSeries.Find(id);
            context.ArticleSeries.Remove(articleSeries);
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

    public interface IArticleSeriesRepository : IDisposable
    {
        IQueryable<ArticleSeries> All { get; }
        IQueryable<ArticleSeries> AllIncluding(params Expression<Func<ArticleSeries, object>>[] includeProperties);
        ArticleSeries Find(int id);
        IEnumerable<ArticleSeries> GetMany(Expression<Func<ArticleSeries, bool>> where);
        int GetLastOrder();
        void InsertOrUpdate(ArticleSeries articleSeries);
        void Delete(int id);
        void Save();
    }
}
