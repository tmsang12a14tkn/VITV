using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV_Web.DAL;
using VITV_Web.Models;

namespace VITV_Web.Repositories
{
    public class RecruitExtraInfoRepository : IRecruitExtraInfoRepository
    {
        private readonly VITVContext _context;

        public RecruitExtraInfoRepository(VITVContext context)
        {
            _context = context;
        }

        public RecruitExtraInfoRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<RecruitExtraInfo> All
        {
            get { return _context.RecruitExtraInfoes; }
        }

        public IQueryable<RecruitExtraInfo> AllIncluding(params Expression<Func<RecruitExtraInfo, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<RecruitExtraInfo, object>>, IQueryable<RecruitExtraInfo>>(_context.RecruitExtraInfoes, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Load<TElement>(RecruitExtraInfo recruitExtraInfo, Expression<Func<RecruitExtraInfo, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.RecruitExtraInfoes.Attach(recruitExtraInfo);
            _context.Entry(recruitExtraInfo).Collection(includeProperty).Load();
        }

        public RecruitExtraInfo Find(int id)
        {
            return _context.RecruitExtraInfoes.Find(id);
        }

        public IQueryable<RecruitExtraInfo> GetMany(Expression<Func<RecruitExtraInfo, bool>> where)
        {
            return _context.RecruitExtraInfoes.Where(where);
        }

        public void InsertOrUpdate(RecruitExtraInfo recruitExtraInfo)
        {
            if (recruitExtraInfo.Id == default(int))
            {
                // New entity
                _context.RecruitExtraInfoes.Add(recruitExtraInfo);
            }
            else
            {
                _context.Entry(recruitExtraInfo).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var recruitExtraInfo = _context.RecruitExtraInfoes.Find(id);
            _context.RecruitExtraInfoes.Remove(recruitExtraInfo);
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

    public interface IRecruitExtraInfoRepository : IDisposable
    {
        IQueryable<RecruitExtraInfo> All { get; }
        IQueryable<RecruitExtraInfo> AllIncluding(params Expression<Func<RecruitExtraInfo, object>>[] includeProperties);
        RecruitExtraInfo Find(int id);
        IQueryable<RecruitExtraInfo> GetMany(Expression<Func<RecruitExtraInfo, bool>> where);
        void Load<TElement>(RecruitExtraInfo recruitExtraInfo, Expression<Func<RecruitExtraInfo, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(RecruitExtraInfo recruitExtraInfo);
        void Delete(int id);
        void Save();
    }
}