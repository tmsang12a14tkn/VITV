using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class VideoCatTypeRepository : IVideoCatTypeRepository
    {
        private readonly VITVContext _context;

        public VideoCatTypeRepository(VITVContext context)
        {
            _context = context;
        }

        public VideoCatTypeRepository()
        {
            _context = new VITVContext();
        }
        public IQueryable<VideoCatType> All
        {
            get { return _context.VideoCatTypes.OrderBy(group => group.Id); }
        }


        public VideoCatType Find(int id)
        {
            return _context.VideoCatTypes.Find(id);
        }

        public VideoCatType Find(string name)
        {
            return _context.VideoCatTypes.FirstOrDefault(v => v.Name == name);
        }
        public IEnumerable<VideoCatType> GetMany<TKey>(Expression<Func<VideoCatType, bool>> where, Expression<Func<VideoCatType, TKey>> orderBy)
        {
            return _context.VideoCatTypes.Where(where).OrderBy(orderBy).ToList();
        }
        public void InsertOrUpdate(VideoCatType videoCatType)
        {
            if (videoCatType.Id == default(int))
            {
                // New entity
                _context.VideoCatTypes.Add(videoCatType);
            }
            else
            {
                // Existing entity
                _context.Entry(videoCatType).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var videoCatGroup = _context.VideoCatTypes.Find(id);
            _context.VideoCatTypes.Remove(videoCatGroup);
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

    public interface IVideoCatTypeRepository : IDisposable
    {
        IQueryable<VideoCatType> All { get; }
        VideoCatType Find(int id);
        VideoCatType Find(string unique);
        IEnumerable<VideoCatType> GetMany<TKey>(Expression<Func<VideoCatType, bool>> where, Expression<Func<VideoCatType, TKey>> orderBy);
        void InsertOrUpdate(VideoCatType videoCatGroup);
        void Delete(int id);
        void Save();
    }
}