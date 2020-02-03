using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using VITV_Web.DAL;
using VITV_Web.Models;

namespace VITV_Web.Repositories
{ 
    public class InfoRepository : IInfoRepository
    {
        private readonly VITVContext _context;

 
        public InfoRepository(VITVContext _context)
        {
            if (_context == null) throw new ArgumentNullException("_context");
            this._context = _context;
        }

        public InfoRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Info> All
        {
            get { return _context.Infoes; }
        }

        public IQueryable<Info> AllIncluding(params Expression<Func<Info, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Info, object>>, IQueryable<Info>>(_context.Infoes, (current, includeProperty) => current.Include(includeProperty));
        }

        public Info GetInfo(string id)
        {
            return _context.Infoes.Find(id);
        }

        public Info Find(string id)
        {
            return _context.Infoes.Find(id);
        }

        
        public void InsertOrUpdate(Info info)
        {
            var uInfo = Find(info.Id);
            if (uInfo == null)
            {
                // New entity
                _context.Infoes.Add(info);
            } 
            else {
                uInfo.Title = info.Title;
                uInfo.Description = info.Description;
            }
        }

        public void Delete(string id)
        {
            var info = _context.Infoes.Find(id);
            _context.Infoes.Remove(info);
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (OptimisticConcurrencyException)
            {
                //context.Refresh(System.Data.Entity.Core.Objects.RefreshMode.ClientWins, context.Infos);
                //context.SaveChanges();
            }
        }

        public void Dispose() 
        {
            _context.Dispose();
        }
    }

    public interface IInfoRepository : IDisposable
    {
        IQueryable<Info> All { get; }
        IQueryable<Info> AllIncluding(params Expression<Func<Info, object>>[] includeProperties);
        Info GetInfo(string id);
        Info Find(string id);
        void InsertOrUpdate(Info info);
        void Delete(string id);
        void Save();
    }
}