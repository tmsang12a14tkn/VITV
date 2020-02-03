using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class RecruitCategoryRepository : IRecruitCategoryRepository
    {
        private readonly VITVContext _context;

        public RecruitCategoryRepository(VITVContext context)
        {
            _context = context;
        }

        public RecruitCategoryRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<RecruitCategory> All
        {
            get { return _context.RecruitCategories; }
        }

        public IQueryable<RecruitCategory> AllIncluding(params Expression<Func<RecruitCategory, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<RecruitCategory, object>>, IQueryable<RecruitCategory>>(_context.RecruitCategories, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(RecruitCategory recruitCategory, Expression<Func<RecruitCategory, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.RecruitCategories.Attach(recruitCategory);
            _context.Entry(recruitCategory).Collection(includeProperty).Load();
        }

        public RecruitCategory Find(int id)
        {
            return _context.RecruitCategories.Find(id);
        }

        public IQueryable<RecruitCategory> GetMany(Expression<Func<RecruitCategory, bool>> where)
        {
            return _context.RecruitCategories.Where(where);
        }

        public void InsertOrUpdate(RecruitCategory recruitCategory)
        {
            if (recruitCategory.Id == default(int))
            {
                // New entity
                _context.RecruitCategories.Add(recruitCategory);
            }
            else
            {
                _context.Entry(recruitCategory).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var recruitCategory = _context.RecruitCategories.Find(id);
            _context.RecruitCategories.Remove(recruitCategory);
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

    public interface IRecruitCategoryRepository : IDisposable
    {
        IQueryable<RecruitCategory> All { get; }
        IQueryable<RecruitCategory> AllIncluding(params Expression<Func<RecruitCategory, object>>[] includeProperties);
        RecruitCategory Find(int id);
        IQueryable<RecruitCategory> GetMany(Expression<Func<RecruitCategory, bool>> where);
        void Load<TElement>(RecruitCategory recruitCategory, Expression<Func<RecruitCategory, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(RecruitCategory recruitCategory);
        void Delete(int id);
        void Save();
    }
}