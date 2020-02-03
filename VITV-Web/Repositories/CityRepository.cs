using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using VITV_Web.DAL;
using VITV_Web.Models;
using VITV_Web.ViewModels;

namespace VITV_Web.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly VITVContext _context;

        public CityRepository(VITVContext _context)
        {
            if (_context == null) throw new ArgumentNullException("_context");
            this._context = _context;
        }

        public CityRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<City> All
        {
            get { return _context.Cities; }
        }

        public IQueryable<City> AllIncluding(params Expression<Func<City, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<City, object>>, IQueryable<City>>(_context.Cities, (current, includeProperty) => current.Include(includeProperty));
        }

        public IEnumerable<CityView> GetCityForRecruit()
        {
            var cities = (from c in _context.Cities
                          join r in _context.Recruits on c.Id equals r.CityId
                          where r.IsRecruiting && (r.CityId == 1 || r.CityId == 3)
                          select new CityView() { Id = c.Id == 3 ? 1 : c.Id, Name = c.Id == 3 ? "Hà Nội" : c.Name })
                    .Union(from c in _context.Cities
                           join r in _context.Recruits on c.Id equals r.CityId
                           where r.IsRecruiting && (r.CityId == 2 || r.CityId == 3)
                           select new CityView() { Id = c.Id == 3 ? 2 : c.Id, Name = c.Id == 3 ? "TP. Hồ Chí Minh" : c.Name });

            return cities.GroupBy(c => c.Name).Select(g => g.FirstOrDefault());
        }

        public City Find(string id)
        {
            return _context.Cities.Find(id);
        }

        public void InsertOrUpdate(City city)
        {
            if (city.Id == default(int))
            {
                _context.Cities.Add(city);
            }
            else
            {
                _context.Entry(city).State = EntityState.Modified;
            }
        }

        public void Delete(string id)
        {
            var city = _context.Cities.Find(id);
            _context.Cities.Remove(city);
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

    public interface ICityRepository : IDisposable
    {
        IQueryable<City> All { get; }
        IQueryable<City> AllIncluding(params Expression<Func<City, object>>[] includeProperties);
        IEnumerable<CityView> GetCityForRecruit();
        City Find(string id);
        void InsertOrUpdate(City city);
        void Delete(string id);
        void Save();
    }
}