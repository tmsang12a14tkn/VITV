using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.ViewModels;

namespace VITV.Data.Repositories
{
    public class ReporterRepository : IReporterRepository
    {
        private readonly VITVContext _context;

        public ReporterRepository(VITVContext context)
        {
            _context = context;
        }

        public ReporterRepository()
        {
            _context = new VITVContext();
        }

        public List<Employee> All
        {
            get { return _context.Employees.ToList(); }
        }

        public List<Employee> SP_GetEmployees(string name)
        {
            return _context.Database.SqlQuery<Employee>("exec sp_GetEmployees @Query ", new SqlParameter("@Query", name)).ToList();
        }

        public IQueryable<Employee> AllIncluding(params Expression<Func<Employee, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<Employee, object>>, IQueryable<Employee>>(_context.Employees, (current, includeProperty) => current.Include(includeProperty));
        }

        public Employee Find(int id)
        {
            return _context.Employees.Find(id);
        }

        public Employee Find(string title)
        {
            return _context.Employees.FirstOrDefault(r => r.UniqueTitle == title);
        }


        public void Load<TElement>(Employee reporter, Expression<Func<Employee, ICollection<TElement>>> includeProperty) where TElement : class
        {
            _context.Employees.Attach(reporter);
            _context.Entry(reporter).Collection(includeProperty).Load();
        }

        public IEnumerable<Employee> GetMany(Expression<Func<Employee, bool>> where)
        {
            return _context.Employees.Where(where).ToList();
        }

        public void InsertOrUpdate(Employee reporter)
        {
            if (reporter.Id == default(int)) {
                // New entity
                _context.Employees.Add(reporter);
            } else {
                // Existing entity
                _context.Entry(reporter).State = EntityState.Modified;
            }
        }
       public void InsertOrUpdate(EmployeePersonalInfo personalInfo)
        {

            if (_context.EmployeePersonalInfos.Find(personalInfo.Id) == null)
            {
                // New entity
                _context.Entry(personalInfo).State = EntityState.Added;
            }
            else
            {
                // Existing entity
                _context.Entry(personalInfo).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var reporter = _context.Employees.Find(id);
            if (reporter.EmployeePersonalInfo != null)
                _context.EmployeePersonalInfos.Remove(reporter.EmployeePersonalInfo);
            _context.Employees.Remove(reporter);
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

    public interface IReporterRepository : IDisposable
    {
        List<Employee> SP_GetEmployees(string name);
        List<Employee> All { get; }
        IQueryable<Employee> AllIncluding(params Expression<Func<Employee, object>>[] includeProperties);
        Employee Find(int id);
        Employee Find(string title);
        IEnumerable<Employee> GetMany(Expression<Func<Employee, bool>> where);
        void Load<TElement>(Employee reporter, Expression<Func<Employee, ICollection<TElement>>> includeProperty) where TElement : class;
        void InsertOrUpdate(Employee reporter);
        void InsertOrUpdate(EmployeePersonalInfo personalInfo);
        void Delete(int id);
        void Save();
    }
}