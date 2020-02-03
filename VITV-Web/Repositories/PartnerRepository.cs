using System;
using VITV_Web.DAL;
using VITV_Web.Models;
using System.Linq;

namespace VITV_Web.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        readonly VITVContext _context;

        public PartnerRepository()
        {
            _context = new VITVContext();
        }

        public PartnerRepository(VITVContext context)
        {
            _context = context;
        }

        public IQueryable<Partner> All
        {
            get { return _context.Partners; }
        }

        public Partner Find(int id)
        {
            return _context.Partners.Find(id);
        }

        public void InsertOrUpdate(Partner Partner)
        {
            if (Partner.Id == default(int))
            {
                // New entity
                _context.Partners.Add(Partner);
            }
            else
            {
                // Existing entity
                _context.Entry(Partner).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var Partner = _context.Partners.Find(id);
            _context.Partners.Remove(Partner);
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

    public interface IPartnerRepository : IDisposable
    {
        IQueryable<Partner> All { get; }
        Partner Find(int id);
        void InsertOrUpdate(Partner Partner);
        void Delete(int id);
        void Save();
    }
}