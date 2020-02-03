using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{ 
    public class ProgramScheduleDetailRepository : IProgramScheduleDetailRepository
    {
        private readonly VITVContext _context;

        public ProgramScheduleDetailRepository(VITVContext context)
        {
            _context = context;
        }

        public ProgramScheduleDetailRepository()
        {
            _context = new VITVContext();
        }
        public IQueryable<ProgramScheduleDetail> All
        {
            get { return _context.ProgramScheduleDetails; }
        }

        public IQueryable<ProgramScheduleDetail> AllIncluding(params Expression<Func<ProgramScheduleDetail, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<ProgramScheduleDetail, object>>, IQueryable<ProgramScheduleDetail>>(_context.ProgramScheduleDetails, (current, includeProperty) => current.Include(includeProperty));
        }

        public ProgramScheduleDetail Find(int id)
        {
            return _context.ProgramScheduleDetails.Find(id);
        }
        public IEnumerable<ProgramScheduleDetail> GetMany<TKey>(Expression<Func<ProgramScheduleDetail, bool>> where, Expression<Func<ProgramScheduleDetail, TKey>> orderBy)
        {
            return _context.ProgramScheduleDetails.Where(where).OrderBy(orderBy).ToList();
        }

        public void UpdateVideo(int id, int videoId)
        {
            var programScheduleDetail = _context.ProgramScheduleDetails.Find(id);
            if (programScheduleDetail != null)
                programScheduleDetail.VideoId = videoId;
        }

        public void InsertOrUpdate(ProgramScheduleDetail programScheduleDetail)
        {
            if (programScheduleDetail.Id == default(int)) {
                // New entity
                _context.ProgramScheduleDetails.Add(programScheduleDetail);
            } else {
                // Existing entity
                _context.Entry(programScheduleDetail).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var programScheduleDetail = _context.ProgramScheduleDetails.Find(id);
            _context.ProgramScheduleDetails.Remove(programScheduleDetail);
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

    public interface IProgramScheduleDetailRepository : IDisposable
    {
        IQueryable<ProgramScheduleDetail> All { get; }
        IQueryable<ProgramScheduleDetail> AllIncluding(params Expression<Func<ProgramScheduleDetail, object>>[] includeProperties);
        ProgramScheduleDetail Find(int id);
        IEnumerable<ProgramScheduleDetail> GetMany<TKey>(Expression<Func<ProgramScheduleDetail, bool>> where, Expression<Func<ProgramScheduleDetail, TKey>> orderBy);
        void InsertOrUpdate(ProgramScheduleDetail programScheduleDetail);
        void UpdateVideo(int id, int videoId);
        void Delete(int id);
        void Save();
    }
}