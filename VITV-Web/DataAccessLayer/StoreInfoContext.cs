using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VITV_Web.Models.StoreInfo;

namespace VITV_Web.DAL
{
    public class StoreInfoContext : DbContext
    {
        public StoreInfoContext()
            : base("StoreInfoContext")
        {
        }

        public DbSet<USDExchangeRate> USDExchangeRates { get; set; }
        public DbSet<USDExchangeRate_Day> USDExchangeRate_Days { get; set; }
        public DbSet<VNDExchangeRate> VNDExchangeRates { get; set; }
        public DbSet<VNDExchangeRate_Day> VNDExchangeRate_Days { get; set; }
        public DbSet<OilPrice> OilPrices { get; set; }
        public DbSet<OilPrice_Day> OilPrice_Days { get; set; }
        public DbSet<MetalPrice> MetalPrices { get; set; }
        public DbSet<MetalPrice_Day> MetalPrice_Days { get; set; }
        public DbSet<GoldPriceVietNam> GoldPriceVietNams { get; set; }
        public DbSet<GoldPriceVietNam_Day> GoldPriceVietNam_Days { get; set; }
        public DbSet<GoldPriceWorld> GoldPriceWorlds { get; set; }
        public DbSet<GoldPriceWorld_Day> GoldPriceWorld_Days { get; set; }
        public DbSet<StockWorld> StockWorlds { get; set; }
        public DbSet<StockWorld_Day> StockWorld_Days { get; set; }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Stock_Day> Stock_Days { get; set; }
        public DbSet<StockMarket> StockMarkets { get; set; }
        public DbSet<StockMarket_Day> StockMarket_Days { get; set; }
        public DbSet<StockMarket_RealTime> StockMarket_RealTimes { get; set; }

    }
}