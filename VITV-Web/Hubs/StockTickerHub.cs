using System;
using Microsoft.AspNet.SignalR;
using VITV.Web.ViewModels;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;

namespace VITV.Web.Hubs
{
    [HubName("hoseStockHub")]
    public class HoseStockHub : Hub
    {
        private readonly StockTicker _stockTicker;

        public HoseStockHub() :
            this(StockTicker.Instance)
        {

        }

        public HoseStockHub(StockTicker stockTicker)
        {
            _stockTicker = stockTicker;
        }

        public IEnumerable<RealtimeStock> GetAllStocks()
        {
            return _stockTicker.GetAllHoseStocks();
        }

        public string GetMarketState()
        {
            return _stockTicker.MarketState.ToString();
        }
    }


    [HubName("hnxStockHub")]
    public class HnxStockHub : Hub
    {
        private readonly StockTicker _stockTicker;

        public HnxStockHub() :
            this(StockTicker.Instance)
        {

        }

        public HnxStockHub(StockTicker stockTicker)
        {
            _stockTicker = stockTicker;
        }

        public IEnumerable<RealtimeStock> GetAllStocks()
        {
            return _stockTicker.GetAllHnxStocks();
        }

        public string GetMarketState()
        {
            return _stockTicker.MarketState.ToString();
        }
    }

    [HubName("upcomStockHub")]
    public class UpcomStockHub : Hub
    {
        private readonly StockTicker _stockTicker;

        public UpcomStockHub() :
            this(StockTicker.Instance)
        {

        }

        public UpcomStockHub(StockTicker stockTicker)
        {
            _stockTicker = stockTicker;
        }

        public IEnumerable<RealtimeStock> GetAllStocks()
        {
            return _stockTicker.GetAllUpcomStocks();
        }

        public string GetMarketState()
        {
            return _stockTicker.MarketState.ToString();
        }
    }
}