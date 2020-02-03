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
    public class BankRepository : IBankRepository
    {
        private readonly VITVContext _context;

        public BankRepository(VITVContext context)
        {
            _context = context;
        }

        public BankRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Bank> All
        {
            get { return _context.Banks; }
        }

        public IQueryable<Bank> AllIncluding(params Expression<Func<Bank, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Bank, object>>, IQueryable<Bank>>(_context.Banks, (current, includeProperty) => current.Include(includeProperty));
        }

        public Bank Find(int id)
        {
            return _context.Banks.Find(id);
        }

        public Bank Find(string name)
        {
            return _context.Banks.FirstOrDefault(r => r.Name == name);
        }

        public void Load<TElement>(Bank bank, Expression<Func<Bank, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.Banks.Attach(bank);
            _context.Entry(bank).Collection(includeProperty).Load();
        }

        public IEnumerable<Bank> GetMany(Expression<Func<Bank, bool>> where)
        {
            return _context.Banks.Where(where).ToList();
        }

        public void InsertOrUpdate(Bank bank)
        {
            if (bank.Id == default(int)) {
                // New entity
                _context.Banks.Add(bank);
            } else {
                // Existing entity
                _context.Entry(bank).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var bank = _context.Banks.Find(id);
            _context.Banks.Remove(bank);
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

    public interface IBankRepository : IDisposable
    {
        IQueryable<Bank> All { get; }
        IQueryable<Bank> AllIncluding(params Expression<Func<Bank, object>>[] includeProperties);
        Bank Find(int id);
        Bank Find(string title);
        IEnumerable<Bank> GetMany(Expression<Func<Bank, bool>> where);
        void Load<TElement>(Bank bank, Expression<Func<Bank, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(Bank bank);
        void Delete(int id);
        void Save();
    }
}