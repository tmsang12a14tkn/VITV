using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Repositories
{
    public class VNDExchangeRateRepository:IVNDExchangeRateRepository
    {
        private readonly VITVContext _context;
        public VNDExchangeRateRepository()
        {
            _context = new VITVContext();
        }
        public VNDExchangeRateRepository(VITVContext context)
        {
            _context = context;
        }

        public DateTime GetLastUpdateTime()
        {
            var last = _context.VNDExchangeRates.OrderByDescending(r => r.Id).FirstOrDefault();
            if (last == null)
                return DateTime.Now.AddDays(-10);
            else 
                return last.UpdatedTime;
        }

        

        public void Insert(string Code, string Name, double Buy, double Transfer, double Sell, DateTime UpdateTime)
        {
            var currency = _context.Currencies.FirstOrDefault(c => c.CurrencyCode == Code);
            if (currency == null)
            {
                currency = new Models.Currency
                {
                    CurrencyCode = Code,
                    CurrencyName = Name
                };
            }
            var rate = new VNDExchangeRate
            {
                Currency = currency,
                Buy = Buy,
                Transfer = Transfer,
                Sell = Sell,
                UpdatedTime = UpdateTime
            };
            _context.VNDExchangeRates.Add(rate);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }


        public List<VNDExchangeRate> GetLastRates()
        {
            return _context.Currencies.Select(c => c.VNDExchangeRates.OrderByDescending(rate => rate.Id).FirstOrDefault()).ToList();
        }
    }

    public interface IVNDExchangeRateRepository : IDisposable
    {
        DateTime GetLastUpdateTime();

        List<VNDExchangeRate> GetLastRates();
        void Insert(string Code, string Name, double Buy, double Transfer, double Sell, DateTime UpdateTime);
        void Save();
    }
}