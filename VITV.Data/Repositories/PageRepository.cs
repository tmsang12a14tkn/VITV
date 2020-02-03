using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.ViewModels;

namespace VITV.Data.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly VITVContext _context;

        public PageRepository(VITVContext context)
        {
            _context = context;
        }

        public PageRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<Page> All
        {
            get { return _context.Pages; }
        }


        public Page Find(int videoid)
        {
            return _context.Pages.Where(p => p.VideoId == videoid).FirstOrDefault();
        }

        public IEnumerable<Page> GetMany(Expression<Func<Page, bool>> where)
        {
            return _context.Pages.Where(where).ToList();
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

    public interface IPageRepository : IDisposable
    {
        IQueryable<Page> All { get; }
        Page Find(int videoid);
        IEnumerable<Page> GetMany(Expression<Func<Page, bool>> where);
        void Save();
    }
}
