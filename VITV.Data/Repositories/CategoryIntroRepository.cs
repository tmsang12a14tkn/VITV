using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.ViewModels;

namespace VITV.Data.Repositories
{
    public class CategoryIntroRepository : ICategoryIntroRepository
    {
        private readonly VITVContext _context;

        public CategoryIntroRepository(VITVContext context)
        {
            _context = context;
        }

        public CategoryIntroRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<CategoryIntro> All
        {
            get { return _context.CategoryIntroes; }
        }

        public IQueryable<CategoryIntro> AllIncluding(params Expression<Func<CategoryIntro, object>>[] includeProperties)
        {
            IQueryable<CategoryIntro> query = _context.CategoryIntroes;
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(CategoryIntro categoryIntro, Expression<Func<CategoryIntro, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.CategoryIntroes.Attach(categoryIntro);
            _context.Entry(categoryIntro).Collection(includeProperty).Load();
        }

        public DbPropertyEntry GetProperty<TProperty>(CategoryIntro categoryIntro, Expression<Func<CategoryIntro, TProperty>> property)
        {
            return _context.Entry(categoryIntro).Property(property);
        }

        public CategoryIntro Find(int id)
        {
            return _context.CategoryIntroes.Find(id);
        }

        public IQueryable<CategoryIntro> GetMany(Expression<Func<CategoryIntro, bool>> where)
        {
            return _context.CategoryIntroes.Where(where);
        }

        public void InsertOrUpdate(CategoryIntro categoryIntro)
        {
            var model = _context.CategoryIntroes.Find(categoryIntro.Id);
            if (categoryIntro.Id == default(int))
            {
                // New entity
                _context.CategoryIntroes.Add(categoryIntro);
            }
            else
            {
                // Existing entity
                _context.Entry(model).State = EntityState.Detached;
                _context.Entry(categoryIntro).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var categoryIntro = _context.CategoryIntroes.Find(id);
            _context.CategoryIntroes.Remove(categoryIntro);
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

    public interface ICategoryIntroRepository : IDisposable
    {
        IQueryable<CategoryIntro> All { get; }
        IQueryable<CategoryIntro> AllIncluding(params Expression<Func<CategoryIntro, object>>[] includeProperties);
        void Load<TElement>(CategoryIntro categoryIntro, Expression<Func<CategoryIntro, ICollection<TElement>>> includeProperty) where TElement : class;
        CategoryIntro Find(int id);
        IQueryable<CategoryIntro> GetMany(Expression<Func<CategoryIntro, bool>> where);
        void InsertOrUpdate(CategoryIntro categoryIntro);
        void Delete(int id);
        DbPropertyEntry GetProperty<TProperty>(CategoryIntro categoryIntro, Expression<Func<CategoryIntro, TProperty>> property);
        void Save();
    }
}
