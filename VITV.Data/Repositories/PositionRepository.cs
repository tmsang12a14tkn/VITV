using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class PositionRepository : IPositionRepository
    {
    private readonly VITVContext _context;

        public PositionRepository(VITVContext context)
        {
            _context = context;
        }

        public PositionRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Position> All
        {
            get { return _context.Positions; }
        }

        public IQueryable<Position> AllIncluding(params Expression<Func<Position, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Position, object>>, IQueryable<Position>>(_context.Positions, (current, includeProperty) => current.Include(includeProperty));
        }

        public Position Find(int id)
        {
            return _context.Positions.Find(id);
        }

        public IEnumerable<Position> GetMany(Expression<Func<Position, bool>> where)
        {
            return _context.Positions.Where(where).ToList();
        }

        public void InsertOrUpdate(Position Position)
        {
            if (Position.Id == default(int)) {
                // New entity
                _context.Positions.Add(Position);
            } else {
                // Existing entity
                _context.Entry(Position).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var Position = _context.Positions.Find(id);
            _context.Positions.Remove(Position);
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

    public interface IPositionRepository : IDisposable
    {
        IQueryable<Position> All { get; }
        IQueryable<Position> AllIncluding(params Expression<Func<Position, object>>[] includeProperties);
        Position Find(int id);
        IEnumerable<Position> GetMany(Expression<Func<Position, bool>> where);
        void InsertOrUpdate(Position Position);
        void Delete(int id);
        void Save();
    }
}