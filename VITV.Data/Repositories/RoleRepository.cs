using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
    readonly VITVContext _context;

        public RoleRepository()
        {
            _context = new VITVContext();
        }

        public RoleRepository(VITVContext context)
        {
            _context = context;
        }

        public IQueryable<Role> All
        {
            get { return _context.ReporterRoles; }
        }

        public Role Find(int id)
        {
            return _context.ReporterRoles.Find(id);
        }

        public void InsertOrUpdate(Role Role)
        {
            if (Role.Id == default(int))
            {
                // New entity
                _context.ReporterRoles.Add(Role);
            }
            else
            {
                // Existing entity
                _context.Entry(Role).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var Role = _context.ReporterRoles.Find(id);
            _context.ReporterRoles.Remove(Role);
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

    public interface IRoleRepository : IDisposable
    {
        IQueryable<Role> All { get; }
        Role Find(int id);
        void InsertOrUpdate(Role Role);
        void Delete(int id);
        void Save();
    }
}