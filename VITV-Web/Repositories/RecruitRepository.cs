using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV_Web.DAL;
using VITV_Web.Models;

namespace VITV_Web.Repositories
{
    public class RecruitRepository : IRecruitRepository
    {
        private readonly VITVContext _context;

        public RecruitRepository(VITVContext context)
        {
            _context = context;
        }

        public RecruitRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Recruit> All
        {
            get { return _context.Recruits; }
        }

        public IQueryable<Recruit> AllIncluding(params Expression<Func<Recruit, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Recruit, object>>, IQueryable<Recruit>>(_context.Recruits, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(Recruit recruit, Expression<Func<Recruit, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.Recruits.Attach(recruit);
            _context.Entry(recruit).Collection(includeProperty).Load();

        }
        public Recruit Find(int id)
        {
            return _context.Recruits.Find(id);
        }

        public IQueryable<Recruit> GetMany(Expression<Func<Recruit, bool>> where)
        {
            return _context.Recruits.Where(where);
        }

        public void InsertOrUpdate(Recruit recruit)
        {
            if (recruit.Id == default(int))
            {
                // New entity
                _context.Recruits.Add(recruit);
            }
            else
            {
                _context.Entry(recruit).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var recruit = _context.Recruits.Find(id);
            _context.Recruits.Remove(recruit);
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

    public interface IRecruitRepository : IDisposable
    {
        IQueryable<Recruit> All { get; }
        IQueryable<Recruit> AllIncluding(params Expression<Func<Recruit, object>>[] includeProperties);
        Recruit Find(int id);
        IQueryable<Recruit> GetMany(Expression<Func<Recruit, bool>> where);
        void Load<TElement>(Recruit recruit, Expression<Func<Recruit, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(Recruit recruit);
        void Delete(int id);
        void Save();
    }
}