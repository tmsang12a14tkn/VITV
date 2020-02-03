using System.Collections.Generic;
using VITV.Data.Models.StoreInfo;

namespace VITV.Data.ViewModels
{
    public class RateAndPrices
    {
        //public List<ReuterIndex> LocalGolds { get; set; }
        //public List<ReuterIndex> GlobalGolds { get; set; }
        public List<GoldPriceVietNam> GoldVNs { get; set; }
        public List<GoldPriceWorld> GoldWorlds { get; set; }
        public List<MetalPrice> Metals { get; set; }
        public List<OilPrice> Oils { get; set; }
        //public List<ReuterIndex> Metals { get; set; }
        public List<USDExchangeRate> USDRates { get; set; }
        public List<VNDExchangeRate> VNDRates { get; set; }
        public List<StockWorld> WorldStocks { get; set; }
        public List<StockMarket_RealTime> VNStocks { get; set; }

        //public List<ReuterIndex> GlobalStockList { get; set; }
        //public List<VnMarketIndex> VnMarketIndices { get; set; } 

    }
}