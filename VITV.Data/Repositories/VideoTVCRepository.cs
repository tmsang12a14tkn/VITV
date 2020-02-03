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
    public class VideoTVCRepository: IVideoTVCRepository
    {
        private readonly VITVContext _context;

        public VideoTVCRepository(VITVContext context)
        {
            _context = context;
        }

        public VideoTVCRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<VideoTVC> All
        {
            get { return _context.VideoTVCs; }
        }

        public IQueryable<TVCProduct> AllProduct
        {
            get { return _context.TVCProducts; }
        }
        public IQueryable<TVCCompany> AllCompany
        {
            get { return _context.TVCCompanies; }
        }
        public IQueryable<TVCProductGroup> AllProductGroup
        {
            get { return _context.TVCProductGroups; }
        }
        public IQueryable<TVCCompanyGroup> AllCompanyGroup
        {
            get { return _context.TVCCompanyGroups; }
        }
        public IQueryable<VideoTVC> AllIncluding(params Expression<Func<VideoTVC, object>>[] includeProperties)
        {
            IQueryable<VideoTVC> query = _context.VideoTVCs;
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(VideoTVC videoTVC, Expression<Func<VideoTVC, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.VideoTVCs.Attach(videoTVC);
            _context.Entry(videoTVC).Collection(includeProperty).Load();
        }

        public DbPropertyEntry GetProperty<TProperty>(VideoTVC videoTVC, Expression<Func<VideoTVC, TProperty>> property)
        {
            return _context.Entry(videoTVC).Property(property);
        }

        public VideoTVC Find(int id)
        {
            return _context.VideoTVCs.Find(id);
        }

        public IQueryable<VideoTVC> GetMany(Expression<Func<VideoTVC, bool>> where)
        {
            return _context.VideoTVCs.Where(where);
        }

        public void InsertOrUpdate(VideoTVC videoTVC)
        {
            var model = _context.VideoTVCs.Find(videoTVC.Id);
            if (videoTVC.Id == default(int))
            {
                // New entity
                _context.VideoTVCs.Add(videoTVC);
            }
            else
            {
                // Existing entity
                _context.Entry(model).State = EntityState.Detached;
                _context.Entry(videoTVC).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var videoTVC = _context.VideoTVCs.Find(id);
            _context.VideoTVCs.Remove(videoTVC);
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

    public interface IVideoTVCRepository : IDisposable
    {
        IQueryable<VideoTVC> All { get; }
        IQueryable<TVCProduct> AllProduct { get; }
        IQueryable<TVCCompany> AllCompany { get; }
        IQueryable<TVCProductGroup> AllProductGroup { get; }
        IQueryable<TVCCompanyGroup> AllCompanyGroup { get; }
        IQueryable<VideoTVC> AllIncluding(params Expression<Func<VideoTVC, object>>[] includeProperties);
        void Load<TElement>(VideoTVC videoTVC, Expression<Func<VideoTVC, ICollection<TElement>>> includeProperty) where TElement : class;
        VideoTVC Find(int id);
        IQueryable<VideoTVC> GetMany(Expression<Func<VideoTVC, bool>> where);
        void InsertOrUpdate(VideoTVC videoTVC);
        void Delete(int id);
        DbPropertyEntry GetProperty<TProperty>(VideoTVC videoTVC, Expression<Func<VideoTVC, TProperty>> property);
        void Save();
    }
}
