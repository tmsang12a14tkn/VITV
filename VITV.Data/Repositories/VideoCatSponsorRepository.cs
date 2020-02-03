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
    public class VideoCatSponsorRepository : IVideoCatSponsorRepository
    {
        private readonly VITVContext _context;

        public VideoCatSponsorRepository(VITVContext context)
        {
            _context = context;
        }

        public VideoCatSponsorRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<VideoCategorySponsor> All
        {
            get { return _context.VideoCategorySponsors; }
        }

        public IQueryable<VideoCategorySponsor> AllIncluding(params Expression<Func<VideoCategorySponsor, object>>[] includeProperties)
        {
            IQueryable<VideoCategorySponsor> query = _context.VideoCategorySponsors;
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(VideoCategorySponsor videoCategorySponsor, Expression<Func<VideoCategorySponsor, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.VideoCategorySponsors.Attach(videoCategorySponsor);
            _context.Entry(videoCategorySponsor).Collection(includeProperty).Load();
        }

        public DbPropertyEntry GetProperty<TProperty>(VideoCategorySponsor videoCategorySponsor, Expression<Func<VideoCategorySponsor, TProperty>> property)
        {
            return _context.Entry(videoCategorySponsor).Property(property);
        }

        public VideoCategorySponsor Find(int id)
        {
            return _context.VideoCategorySponsors.Find(id);
        }

        public IQueryable<VideoCategorySponsor> GetMany(Expression<Func<VideoCategorySponsor, bool>> where)
        {
            return _context.VideoCategorySponsors.Where(where);
        }

        public void InsertOrUpdate(VideoCategorySponsor videoCategorySponsor)
        {
            var model = _context.VideoCategorySponsors.Find(videoCategorySponsor.Id);
            if (videoCategorySponsor.Id == default(int))
            {
                // New entity
                _context.VideoCategorySponsors.Add(videoCategorySponsor);
            }
            else
            {
                // Existing entity
                _context.Entry(model).State = EntityState.Detached;
                _context.Entry(videoCategorySponsor).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var videoCategorySponsor = _context.VideoCategorySponsors.Find(id);
            _context.VideoCategorySponsors.Remove(videoCategorySponsor);
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

    public interface IVideoCatSponsorRepository : IDisposable
    {
        IQueryable<VideoCategorySponsor> All { get; }
        IQueryable<VideoCategorySponsor> AllIncluding(params Expression<Func<VideoCategorySponsor, object>>[] includeProperties);
        void Load<TElement>(VideoCategorySponsor videoCategorySponsor, Expression<Func<VideoCategorySponsor, ICollection<TElement>>> includeProperty) where TElement : class;
        VideoCategorySponsor Find(int id);
        IQueryable<VideoCategorySponsor> GetMany(Expression<Func<VideoCategorySponsor, bool>> where);
        void InsertOrUpdate(VideoCategorySponsor videoCategorySponsor);
        void Delete(int id);
        DbPropertyEntry GetProperty<TProperty>(VideoCategorySponsor videoCategorySponsor, Expression<Func<VideoCategorySponsor, TProperty>> property);
        void Save();
    }
}
