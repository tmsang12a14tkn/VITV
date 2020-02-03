using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using VITV_Web.DAL;
using VITV_Web.Models;

namespace VITV_Web.Repositories
{
    public class InterestRateRepository : IInterestRateRepository
    {
        private readonly VITVContext _context;

        public InterestRateRepository(VITVContext context)
        {
            _context = context;
        }

        public InterestRateRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<InterestRate> All
        {
            get { return _context.InterestRates; }
        }

        public IQueryable<InterestRate> AllIncluding(params Expression<Func<InterestRate, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<InterestRate, object>>, IQueryable<InterestRate>>(_context.InterestRates, (current, includeProperty) => current.Include(includeProperty));
        }

        public InterestRate Find(int id)
        {
            return _context.InterestRates.Find(id);
        }

        public void Load<TElement>(InterestRate interestRate, Expression<Func<InterestRate, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.InterestRates.Attach(interestRate);
            _context.Entry(interestRate).Collection(includeProperty).Load();
        }

        public IEnumerable<InterestRate> GetMany(Expression<Func<InterestRate, bool>> where)
        {
            return _context.InterestRates.Where(where).ToList();
        }

        public void InsertOrUpdate(InterestRate interestRate)
        {
            if (interestRate.Id == default(int))
            {
                // New entity
                _context.InterestRates.Add(interestRate);
            } else {
                // Existing entity
                _context.Entry(interestRate).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var interestRate = _context.InterestRates.Find(id);
            _context.InterestRates.Remove(interestRate);
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

    public interface IInterestRateRepository : IDisposable
    {
        IQueryable<InterestRate> All { get; }
        IQueryable<InterestRate> AllIncluding(params Expression<Func<InterestRate, object>>[] includeProperties);
        InterestRate Find(int id);
        IEnumerable<InterestRate> GetMany(Expression<Func<InterestRate, bool>> where);
        void Load<TElement>(InterestRate interestRate, Expression<Func<InterestRate, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(InterestRate interestRate);
        void Delete(int id);
        void Save();
    }
}