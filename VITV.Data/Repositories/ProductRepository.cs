using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{ 
    public class ProductRepository : IProductRepository
    {
        readonly VITVContext _context = new VITVContext();
    
        public IQueryable<Product> All
        {
            get { return _context.Products; }
        }

        public IQueryable<Product> AllIncluding(params Expression<Func<Product, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Product, object>>, IQueryable<Product>>(_context.Products, (current, includeProperty) => current.Include(includeProperty));
        }

        public Product Find(int id)
        {
            return _context.Products.Find(id);
        }

        public void InsertOrUpdate(Product product)
        {
            if (product.Id == default(int)) {
                // New entity
                _context.Products.Add(product);
            } else {
                // Existing entity
                _context.Entry(product).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
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

    public interface IProductRepository : IDisposable
    {
        IQueryable<Product> All { get; }
        IQueryable<Product> AllIncluding(params Expression<Func<Product, object>>[] includeProperties);
        Product Find(int id);
        void InsertOrUpdate(Product product);
        void Delete(int id);
        void Save();
    }
}