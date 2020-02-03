using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV_Web.DAL;
using VITV_Web.Models;

namespace VITV_Web.Repositories
{ 
    public class ArticleCategoryRepository : IArticleCategoryRepository
    {
        private readonly VITVContext context;

        public ArticleCategoryRepository(VITVContext _context)
        {
            context = _context;
        }

        public ArticleCategoryRepository()
        {
            context = new VITVContext();
        }

        public IQueryable<ArticleCategory> All
        {
            get { return context.ArticleCategories.OrderBy(art => art.Order); }
        }

        public IQueryable<ArticleCategory> AllIncluding(params Expression<Func<ArticleCategory, object>>[] includeProperties)
        {
            IQueryable<ArticleCategory> query = context.ArticleCategories.OrderBy(art => art.Order);
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ArticleCategory Find(int id)
        {
            return context.ArticleCategories.Find(id);
        }
        public IEnumerable<ArticleCategory> GetMany(Expression<Func<ArticleCategory, bool>> where)
        {
            return context.ArticleCategories.Where(where).ToList();
        }
        public void InsertOrUpdate(ArticleCategory articlecategory)
        {
            if (articlecategory.Id == default(int)) {
                // New entity
                context.ArticleCategories.Add(articlecategory);
            } else {
                // Existing entity
                context.Entry(articlecategory).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var articlecategory = context.ArticleCategories.Find(id);
            context.ArticleCategories.Remove(articlecategory);
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

    public interface IArticleCategoryRepository : IDisposable
    {
        IQueryable<ArticleCategory> All { get; }
        IQueryable<ArticleCategory> AllIncluding(params Expression<Func<ArticleCategory, object>>[] includeProperties);
        ArticleCategory Find(int id);
        IEnumerable<ArticleCategory> GetMany(Expression<Func<ArticleCategory, bool>> where);
        void InsertOrUpdate(ArticleCategory articlecategory);
        void Delete(int id);
        void Save();
    }
}