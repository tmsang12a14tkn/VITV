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
    public class SettingRepository : ISettingRepository
    {
        private readonly VITVContext _context;

        public SettingRepository(VITVContext context)
        {
            _context = context;
        }

        public SettingRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Setting> All
        {
            get { return _context.Settings; }
        }

        public IQueryable<Setting> AllIncluding(params Expression<Func<Setting, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Setting, object>>, IQueryable<Setting>>(_context.Settings, (current, includeProperty) => current.Include(includeProperty));
        }

        public Setting Find(string name)
        {
            var setting = _context.Settings.Find(name);
            if (setting == null)
            {
                setting = new Setting
                {
                    Name = name,
                    Value = ""
                };
                Save();
            }
            return setting; 
        }
        public string GetValue(string name)
        {
            var setting = Find(name);
            return setting.Value;
        }
        public void Load<TElement>(Setting Setting, Expression<Func<Setting, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.Settings.Attach(Setting);
            _context.Entry(Setting).Collection(includeProperty).Load();
        }

        public IEnumerable<Setting> GetMany(Expression<Func<Setting, bool>> where)
        {
            return _context.Settings.Where(where).ToList();
        }

        public void InsertOrUpdate(Setting setting)
        {
            if (_context.Settings.AsNoTracking().All(s=>s.Name != setting.Name))
            {
                // New entity
                _context.Settings.Add(setting);
            } else {
                // Existing entity
                _context.Entry(setting).State = EntityState.Modified;
            }
        }

        public void Delete(string name)
        {
            var Setting = _context.Settings.Find(name);
            _context.Settings.Remove(Setting);
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

    public interface ISettingRepository : IDisposable
    {
        IQueryable<Setting> All { get; }
        IQueryable<Setting> AllIncluding(params Expression<Func<Setting, object>>[] includeProperties);
        Setting Find(string name);
        string GetValue(string name);
        IEnumerable<Setting> GetMany(Expression<Func<Setting, bool>> where);
        void Load<TElement>(Setting Setting, Expression<Func<Setting, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(Setting Setting);
        void Delete(string name);
        void Save();
    }
}