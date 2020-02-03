using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.ViewModels.Portal;

namespace VITV.Data.Repositories
{ 
    public class ArticleRepository : IArticleRepository
    {
        private readonly VITVContext _context;

        public ArticleRepository(VITVContext context)
        {
            _context = context;
        }

        public ArticleRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Article> All
        {
            get { return _context.Articles; }
        }

        public IQueryable<Article> AllIncluding(params Expression<Func<Article, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Article, object>>, IQueryable<Article>>(_context.Articles, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(Article article, Expression<Func<Article, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.Articles.Attach(article);
            _context.Entry(article).Collection(includeProperty).Load();
            
        }
        public Article Find(int id)
        {
            return _context.Articles.Find(id);
        }

        public IEnumerable<Article> GetMany<TKey>(Expression<Func<Article, bool>> where, Expression<Func<Article, TKey>> orderBy)
        {
            return _context.Articles.Where(where).OrderBy(orderBy).ToList();
        }

        public void InsertOrUpdate(Article article)
        {
            //var model = _context.Articles.Find(article.Id);
            if (article.Id == default(int)) {
                // New entity
                _context.Articles.Add(article);
            } else {
                // Existing entity
                //_context.Entry(model).State = EntityState.Detached;
                _context.Entry(article).State = EntityState.Modified;
                
                
            }
        }
        public void InsertReview(ArticleReview articleReview)
        {
            //_context.ArticleReviews.Add(articleReview);
        }

        public void Delete(int id)
        {
            var article = _context.Articles.Find(id);
            var pages = _context.Pages.Where(p => p.ArticleId == id);
            _context.Pages.RemoveRange(pages);
            _context.Articles.Remove(article);
        }
        public ArticleFirstWeekReport GetFirstWeekReport(int articleId)
        {
            var article = _context.Articles.Find(articleId);
            if (article.PublishedTime.HasValue)
            {
                var fromDate = article.PublishedTime.Value.Date;
                var next7Days = fromDate.AddDays(6);

                var all = _context.PageAccesses.Where(pa => pa.Page.ArticleId == articleId && pa.Date >= fromDate && pa.Date <= next7Days).ToList();
                var groupByDate = all.GroupBy(pa => pa.Date).OrderByDescending(g => g.Count());
                var highest = groupByDate.FirstOrDefault();
                var lowest = groupByDate.LastOrDefault();
                var result = new ArticleFirstWeekReport
                {
                    PageView = all.Count(),
                    Highest = highest != null && highest.Count() > 0 ? highest.Count() : 0,
                    Lowest = lowest != null && lowest.Count() > 0 ? lowest.Count() : 0,
                    ViewDateCount = groupByDate.Count()
                };
                return result;
            }
            else
                return new ArticleFirstWeekReport();

           
        }

        //testsang
        public ArticleAllTimeReport GetAllTimeReport(int articleId)
        {
            var all = _context.PageAccesses.Where(pa => pa.Page.ArticleId == articleId).ToList();
            var groupByDate = all.GroupBy(pa => pa.Date).OrderByDescending(g => g.Count());
            var highest = groupByDate.FirstOrDefault();
            var lowest = groupByDate.LastOrDefault();
            var result = new ArticleAllTimeReport
            {
                PageView = all.Count(),
                Highest = highest != null && highest.Count() > 0 ? highest.Count() : 0,
                Lowest = lowest != null && lowest.Count() > 0 ? lowest.Count() : 0,
                ViewDateCount = groupByDate.Count()
            };

            return result;
        }

        public int GetLastOrder(int seriesId)
        {
            var articleSeries = _context.ArticleSeries.Find(seriesId);
            return articleSeries == null ? 0 : articleSeries.Articles.Count + 1;
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

    public interface IArticleRepository : IDisposable
    {
        IQueryable<Article> All { get; }
        IQueryable<Article> AllIncluding(params Expression<Func<Article, object>>[] includeProperties);
        Article Find(int id);
        IEnumerable<Article> GetMany<TKey>(Expression<Func<Article, bool>> where, Expression<Func<Article, TKey>> orderBy);
        void Load<TElement>(Article article, Expression<Func<Article, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(Article article);
        void InsertReview(ArticleReview articleReview);
        void Delete(int id);
        ArticleFirstWeekReport GetFirstWeekReport(int articleId);
        ArticleAllTimeReport GetAllTimeReport(int articleId);
        int GetLastOrder(int seriesId);
        void Save();
    }
}