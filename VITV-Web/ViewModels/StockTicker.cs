using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR.Hubs;
using VITV.Web.Hubs;
using Microsoft.AspNet.SignalR;
using System.Net;
using System.IO;
using System.Linq;
using VITV.Data.Models.StoreInfo;
using VITV.Data.DAL;
using System.Data.Entity;
namespace VITV.Web.ViewModels
{
    public class StockTicker
    {
        private readonly static Lazy<StockTicker> _instance = new Lazy<StockTicker>(
               () => new StockTicker(GlobalHost.ConnectionManager.GetHubContext<HoseStockHub>().Clients,
                                    GlobalHost.ConnectionManager.GetHubContext<HnxStockHub>().Clients,
                                    GlobalHost.ConnectionManager.GetHubContext<UpcomStockHub>().Clients));

        private readonly object _marketStateLock = new object();
        private readonly object _updateStockPricesLock = new object();

        private readonly ConcurrentDictionary<string, RealtimeStock> _hoseStocks = new ConcurrentDictionary<string, RealtimeStock>();
        private readonly ConcurrentDictionary<string, RealtimeStock> _hnxStocks = new ConcurrentDictionary<string, RealtimeStock>();
        private readonly ConcurrentDictionary<string, RealtimeStock> _upcomStocks = new ConcurrentDictionary<string, RealtimeStock>();

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(5000);

        private Timer _timer;
        private volatile bool _updatingStockPrices;
        private volatile MarketState _marketState;

        private StockTicker(IHubConnectionContext<dynamic> hoseClients, IHubConnectionContext<dynamic> hnxClients, IHubConnectionContext<dynamic> upcomClients)
        {
            //_marketState = ViewModels.MarketState.Open;
            HoseClients = hoseClients;
            HnxClients = hnxClients;
            UpcomClients = upcomClients;

            UpdateStocks();
        }

        public static StockTicker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> HoseClients
        {
            get;
            set;
        }
        private IHubConnectionContext<dynamic> HnxClients
        {
            get;
            set;
        }
        private IHubConnectionContext<dynamic> UpcomClients
        {
            get;
            set;
        }

        public MarketState MarketState
        {
            get { return _marketState; }
            private set { _marketState = value; }
        }

        public IEnumerable<RealtimeStock> GetAllHoseStocks()
        {
            return _hoseStocks.OrderBy(s=>s.Key).Select(s=>s.Value).ToList();
        }

        public IEnumerable<RealtimeStock> GetAllHnxStocks()
        {
            return _hnxStocks.OrderBy(s => s.Key).Select(s => s.Value).ToList();
        }

        public IEnumerable<RealtimeStock> GetAllUpcomStocks()
        {
            return _upcomStocks.OrderBy(s => s.Key).Select(s => s.Value).ToList();
        }

        public void OpenMarket()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Open)
                {
                    _timer = new Timer(UpdateStockPrices, null, TimeSpan.FromSeconds(1), _updateInterval);

                    MarketState = MarketState.Open;

                    BroadcastMarketStateChange(MarketState.Open);
                }
            }
        }

        public void CloseMarket()
        {
            lock (_marketStateLock)
            {
                if (MarketState == MarketState.Open)
                {
                    if (_timer != null)
                    {
                        _timer.Dispose();
                    }

                    MarketState = MarketState.Closed;

                    BroadcastMarketStateChange(MarketState.Closed);
                }
            }
        }

        public void Reset()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Closed)
                {
                    throw new InvalidOperationException("Market must be closed before it can be reset.");
                }

                UpdateStocks();
                BroadcastMarketReset();
            }
        }

        private void UpdateStocks()
        {
            if (_marketState == ViewModels.MarketState.Closed && DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 15)
            {
                OpenMarket();
            }
            else if (_marketState == ViewModels.MarketState.Open && (DateTime.Now.Hour >= 15 || DateTime.Now.Hour < 9))
            {
                CloseMarket();
            }

            _hoseStocks.Clear();
            var hoseStocks = ParseStockOnline("http://cophieu68.vn/datax123456/xml_stockonline_ho.txt");
            hoseStocks.ForEach(stock => _hoseStocks.TryAdd(stock.Symbol, stock));

            _hnxStocks.Clear();
            var hnxStocks = ParseStockOnline("http://cophieu68.vn/datax123456/xml_stockonline_ha.txt");
            hnxStocks.ForEach(stock => _hnxStocks.TryAdd(stock.Symbol, stock));

            _upcomStocks.Clear();
            var upcomStocks = ParseStockOnline("http://cophieu68.vn/datax123456/xml_stockonline_upcom.txt");
            upcomStocks.ForEach(stock => _upcomStocks.TryAdd(stock.Symbol, stock));
        }

        private void UpdateStockPrices(object state)
        {
            if(_marketState == ViewModels.MarketState.Closed && DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 15)
            {
                OpenMarket();
            }
            else if(_marketState == ViewModels.MarketState.Open && (DateTime.Now.Hour >= 15 || DateTime.Now.Hour < 9))
            {
                CloseMarket();

                //Lưu lại giá trị ngày hôm nay
                using (var context = new StoreInfoContext())
                {
                    SaveTodayStock(_hoseStocks.Values, context);
                    SaveTodayStock(_hnxStocks.Values, context);
                    SaveTodayStock(_upcomStocks.Values, context);
                    context.SaveChanges();
                }
            }

            // This function must be re-entrant as it's running as a timer interval handler
            lock (_updateStockPricesLock)
            {
                if (!_updatingStockPrices)
                {
                    _updatingStockPrices = true;

                    var hoseStocks = ParseStockOnline("http://cophieu68.vn/datax123456/xml_stockonline_ho.txt");
                    SaveRealtimeStock(hoseStocks);
                    foreach (var updatedStock in hoseStocks)
                    {
                        if (_hoseStocks.ContainsKey(updatedStock.Symbol))
                        {
                            var stock = _hoseStocks[updatedStock.Symbol];
                            if (!updatedStock.Equals(stock))
                            {
                                _hoseStocks[stock.Symbol] = updatedStock;
                                BroadcastHoseStockPrice(updatedStock);
                            }
                        }
                    }

                    var hnxStocks = ParseStockOnline("http://cophieu68.vn/datax123456/xml_stockonline_ha.txt");
                    SaveRealtimeStock(hnxStocks);
                    foreach (var updatedStock in hnxStocks)
                    {
                        if (_hnxStocks.ContainsKey(updatedStock.Symbol))
                        {
                            var stock = _hnxStocks[updatedStock.Symbol];
                            if (!updatedStock.Equals(stock))
                            {
                                _hnxStocks[stock.Symbol] = updatedStock;
                                BroadcastHnxStockPrice(updatedStock);
                            }
                        }
                    }

                    var upcomStocks = ParseStockOnline("http://cophieu68.vn/datax123456/xml_stockonline_upcom.txt");
                    SaveRealtimeStock(upcomStocks);
                    foreach (var updatedStock in upcomStocks)
                    {
                        if (_upcomStocks.ContainsKey(updatedStock.Symbol))
                        {
                            var stock = _upcomStocks[updatedStock.Symbol];
                            if (!updatedStock.Equals(stock))
                            {
                                _upcomStocks[stock.Symbol] = updatedStock;
                                BroadcastUpcomStockPrice(updatedStock);
                            }
                        }
                    }

                    _updatingStockPrices = false;
                }
            }
        }

        private void SaveTodayStock(ICollection<RealtimeStock> stocks, StoreInfoContext context)
        {
            var dictMarkets = new Dictionary<string, string> { { "^HASTC", "HNX" }, { "^VNINDEX", "HOSE" } };
            float open, high, low, close;
            int volumn;
            DateTime date = DateTime.Now.Date;
            foreach (var stock in stocks)
            {
                //Save market index
                if (stock.Symbol.Contains("^"))
                {
                    if (dictMarkets.ContainsKey(stock.Symbol.ToUpper()))
                    {
                        var marketTicker = stock.Symbol.ToUpper();
                        if (float.TryParse(stock.Open, out open)
                                && float.TryParse(stock.PriceHighest, out high)
                                && float.TryParse(stock.PriceLowest, out low)
                                && float.TryParse(stock.Close, out close)
                                && int.TryParse(stock.Volumn, out volumn)
                                && dictMarkets.ContainsKey(marketTicker))
                        {
                            var marketName = dictMarkets[marketTicker];
                            if (context.StockMarket_Days.Find(marketName, date) == null)
                            {
                                var stockMarketDay = new StockMarket_Day
                                {
                                    Date = date,
                                    Close = close,
                                    High = high,
                                    Low = low,
                                    Open = open,
                                    MarketName = marketName,
                                    Volumn = volumn
                                };
                                context.StockMarket_Days.Add(stockMarketDay);
                            }
                        }
                    }
                }
                    //Save company index
                else
                {
                    if (float.TryParse(stock.Open, out open)
                            && float.TryParse(stock.PriceHighest, out high)
                            && float.TryParse(stock.PriceLowest, out low)
                            && float.TryParse(stock.Close, out close)
                            && int.TryParse(stock.Volumn, out volumn)
                            && context.Stock_Days.Find(stock.Symbol, date) == null)
                    {
                        var stockDay = new Stock_Day
                        {
                            Date = date,
                            Close = close,
                            High = high,
                            Low = low,
                            Open = open,
                            Ticker = stock.Symbol,
                            Volumn = volumn
                        };
                        context.Stock_Days.Add(stockDay);
                    }
                }
            }
        }

        private void SaveRealtimeStock(List<RealtimeStock> stocks)
        {
            var context = new StoreInfoContext();
            try
            {
                foreach (var stock in stocks)
                {
                    if (stock.Symbol.Contains("^"))
                    {
                        var rt_StockMarket = context.StockMarket_RealTimes.Find(stock.Symbol);
                        if (rt_StockMarket == null)
                        {
                            rt_StockMarket = new StockMarket_RealTime()
                            {
                                Close = float.Parse(stock.Close),
                                LastUpdate = DateTime.Now,
                                PercentChange = float.Parse(stock.PercentChange),
                                PriceChange = float.Parse(stock.PriceChange),
                                Ticker = stock.Symbol,
                                Volumn = int.Parse(stock.Volumn)
                            };
                            context.Entry(rt_StockMarket).State = EntityState.Added;
                        }
                        else
                        {
                            rt_StockMarket.LastUpdate = DateTime.Now;
                            rt_StockMarket.PercentChange = float.Parse(stock.PercentChange);
                            rt_StockMarket.PriceChange = float.Parse(stock.PriceChange);
                            rt_StockMarket.Volumn = int.Parse(stock.Volumn);
                            rt_StockMarket.Close = float.Parse(stock.Close);
                        }
                       
                    }
                }
                context.SaveChanges();
            }
            catch
            {
            }
        }
        private void BroadcastMarketStateChange(MarketState marketState)
        {
            switch (marketState)
            {
                case MarketState.Open:
                    HoseClients.All.marketOpened();
                    HnxClients.All.marketOpened();
                    UpcomClients.All.marketOpened();
                    break;
                case MarketState.Closed:
                    HoseClients.All.marketClosed();
                    HnxClients.All.marketClosed();
                    UpcomClients.All.marketClosed();
                    break;
                default:
                    break;
            }
        }

        private void BroadcastMarketReset()
        {
            HoseClients.All.marketReset();
            HnxClients.All.marketReset();
            UpcomClients.All.marketReset();
        }

        private void BroadcastHoseStockPrice(RealtimeStock stock)
        {
            HoseClients.All.updateStockPrice(stock);
        }
        private void BroadcastHnxStockPrice(RealtimeStock stock)
        {
            HnxClients.All.updateStockPrice(stock);
        }
        private void BroadcastUpcomStockPrice(RealtimeStock stock)
        {
            UpcomClients.All.updateStockPrice(stock);
        }

        private List<RealtimeStock> ParseStockOnline(string url)
        {
            var listStock = new List<RealtimeStock>();
            try
            {
                var webRequest = WebRequest.Create(url);

                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(content))
                    {
                        int i = 0;
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            string[] stockParams = line.Split('|');

                            listStock.Add(new RealtimeStock(stockParams));
                            i++;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return listStock;
        }
    }

    public enum MarketState
    {
        Closed,
        Open
    }
}