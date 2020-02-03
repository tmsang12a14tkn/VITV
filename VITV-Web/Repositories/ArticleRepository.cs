using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV_Web.DAL;
using VITV_Web.Models;

namespace VITV_Web.Repositories
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

        public IQueryable<Article> GetMany(Expression<Func<Article, bool>> where)
        {
            return _context.Articles.Where(where);
        }

        public void InsertOrUpdate(Article article)
        {
            if (article.Id == default(int)) {
                // New entity
                _context.Articles.Add(article);
            } else {
                // Existing entity
                //Article updatedArticle = context.Articles.Include(s => s.Keywords).Single(a => a.Id == article.Id);
                //updatedArticle.Keywords = article.Keywords;
                //updatedArticle.ReporterId = article.ReporterId;
                //updatedArticle.Title = article.Title;
                //updatedArticle.ArticleContent = article.ArticleContent;
                //updatedArticle.CategoryId = article.CategoryId;
                _context.Entry(article).State = EntityState.Modified;
                
                
            }
        }

        public void Delete(int id)
        {
            var article = _context.Articles.Find(id);
            _context.Articles.Remove(article);
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
        IQueryable<Article> GetMany(Expression<Func<Article, bool>> where);
        void Load<TElement>(Article article, Expression<Func<Article, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(Article article);
        void Delete(int id);
        void Save();
    }
}