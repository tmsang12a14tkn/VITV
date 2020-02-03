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
    public class PageGroupRepository : IPageGroupRepository
    {
        private readonly VITVContext _context;

        public PageGroupRepository(VITVContext context)
        {
            _context = context;
        }

        public PageGroupRepository()
        {
            _context = new VITVContext();
        }

        public IQueryable<PageGroup> All
        {
            get { return _context.PageGroups; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public interface IPageGroupRepository : IDisposable
    {
        IQueryable<PageGroup> All { get; }
    }
}
