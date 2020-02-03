using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{ 
    public class ProgramScheduleRepository : IProgramScheduleRepository
    {
        private readonly VITVContext _context;

        public ProgramScheduleRepository(VITVContext context)
        {
            _context = context;
        }

        public ProgramScheduleRepository()
        {
            _context = new VITVContext();
        }
        public IQueryable<ProgramSchedule> All
        {
            get { return _context.ProgramSchedules; }
        }

        public IQueryable<ProgramSchedule> AllIncluding(params Expression<Func<ProgramSchedule, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<ProgramSchedule, object>>, IQueryable<ProgramSchedule>>(_context.ProgramSchedules, (current, includeProperty) => current.Include(includeProperty));
        }

        public ProgramSchedule Find(int id)
        {
            return _context.ProgramSchedules.Find(id);
        }
        public IEnumerable<ProgramSchedule> GetMany<TKey>(Expression<Func<ProgramSchedule, bool>> where, Expression<Func<ProgramSchedule, TKey>> orderBy)
        {
            return _context.ProgramSchedules.Where(where).OrderBy(orderBy).ToList();
        }
        public void InsertOrUpdate(ProgramSchedule programSchedule)
        {
            if (programSchedule.Id == default(int)) {
                // New entity
                _context.ProgramSchedules.Add(programSchedule);
            } else {
                // Existing entity
                _context.Entry(programSchedule).State = EntityState.Modified;
            }
        }

        public ProgramSchedule Delete(int id)
        {
            var programSchedule = _context.ProgramSchedules.Find(id);
            _context.ProgramSchedules.Remove(programSchedule);
            return programSchedule;
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

    public interface IProgramScheduleRepository : IDisposable
    {
        IQueryable<ProgramSchedule> All { get; }
        IQueryable<ProgramSchedule> AllIncluding(params Expression<Func<ProgramSchedule, object>>[] includeProperties);
        ProgramSchedule Find(int id);
        IEnumerable<ProgramSchedule> GetMany<TKey>(Expression<Func<ProgramSchedule, bool>> where, Expression<Func<ProgramSchedule, TKey>> orderBy);
        void InsertOrUpdate(ProgramSchedule programSchedule);
        ProgramSchedule Delete(int id);
        void Save();
    }
}