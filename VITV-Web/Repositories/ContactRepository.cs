using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using VITV_Web.DAL;
using VITV_Web.Models;

namespace VITV_Web.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly VITVContext _context;

        public ContactRepository(VITVContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            this._context = context;
        }

        public ContactRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Contact> All
        {
            get { return _context.Contacts; }
        }

        public IQueryable<Contact> AllIncluding(params Expression<Func<Contact, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Contact, object>>, IQueryable<Contact>>(_context.Contacts, (current, includeProperty) => current.Include(includeProperty));
        }

        public Contact GetContact(int id)
        {
            return _context.Contacts.Find(id);
        }

        public Contact Find(int id)
        {
            return _context.Contacts.Find(id);
        }


        public void InsertOrUpdate(Contact contact)
        {
            if (contact.Id == default(int))
            {
                // New entity
                _context.Contacts.Add(contact);
            }
            else
            {
                _context.Entry(contact).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var contact = _context.Contacts.Find(id);
            _context.Contacts.Remove(contact);
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (OptimisticConcurrencyException)
            {
                //context.Refresh(System.Data.Entity.Core.Objects.RefreshMode.ClientWins, context.Contacts);
                //context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public interface IContactRepository : IDisposable
    {
        IQueryable<Contact> All { get; }
        IQueryable<Contact> AllIncluding(params Expression<Func<Contact, object>>[] includeProperties);
        Contact GetContact(int id);
        Contact Find(int id);
        void InsertOrUpdate(Contact contact);
        void Delete(int id);
        void Save();
    }
}