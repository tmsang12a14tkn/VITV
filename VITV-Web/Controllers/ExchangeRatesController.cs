using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.ViewModels;
using VITV.Web.ViewModels;

namespace VITV.Web.Controllers
{
    public class ExchangeRatesController : Controller
    {
        [Route("ty-gia")]
        public ActionResult Index()
        {
            var context = new StoreInfoContext();
            ExchangeRateView info = new ExchangeRateView
            {
                usdExchangeRates = context.USDExchangeRates.ToList(),
                vndExchangeRates = context.VNDExchangeRates.ToList()
            };
            return View(info);
        }
        [Route("gia-vang")]
        public ActionResult GoldPrice()
        {
            var context = new StoreInfoContext();
            GoldPriceView info = new GoldPriceView
            {
                Banks = context.GoldPriceVietNams.Where(g=>g.Type == "BANK").ToList(),
                SJCs = context.GoldPriceVietNams.Where(g => g.Type == "SJC").ToList(),
                PNJs = context.GoldPriceVietNams.Where(g => g.Type == "PNJ").ToList(),
                DOJIs = context.GoldPriceVietNams.Where(g => g.Type == "DOJI").ToList(),
                Others = context.GoldPriceVietNams.Where(g => g.Type == "OTHERS").ToList(),
            };
            return View(info);
        }

        public JsonResult GoldPriceData()
        {
            var beginDate = new DateTime(1970, 1, 1, 0, 0, 0);
            var context = new StoreInfoContext();
            var sjchcm = context.GoldPriceVietNam_Days.Where(g => g.Name == "SJC TP HCM").Select( g => new 
            {
                x = DbFunctions.DiffSeconds(beginDate, g.VCreateDate),
                y = g.Buy 
            }).ToList();

            var pnj = context.GoldPriceVietNam_Days.Where(g => g.Name == "PNJ TP.HCM").Select(g => new
            {
                x = DbFunctions.DiffSeconds(beginDate, g.VCreateDate),
                y = g.Buy
            }).ToList();

            var doji = context.GoldPriceVietNam_Days.Where(g => g.Name == "DOJI SG").Select(g => new
            {
                x = DbFunctions.DiffSeconds(beginDate, g.VCreateDate),
                y = g.Buy
            }).ToList();

            return Json(new ArrayList() { new { key = "SJC HCM", values = sjchcm }, new { key = "PNJ HCM", values = pnj }, new { key = "DOJI HCM", values = doji }}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VNDExchangeRateData(string code)
        {
             var beginDate = new DateTime(1970, 1, 1, 0, 0, 0);
            var context = new StoreInfoContext();
            var exchangeRates = context.VNDExchangeRate_Days.Where(e => e.Code == code).Select(e => new {
                x = DbFunctions.DiffSeconds(beginDate, e.UpdatedTime),
                y = e.Buy  
            }).ToList();
            return Json(new ArrayList() { new { key = code, values = exchangeRates } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VNDChart()
        {
            return View();
        }

        [Route("chung-khoan/truc-tuyen/{stc}")]
        public ActionResult StockOnline(string stc="hose")
        {
            
            return View("StockOnline", null, stc);
        }

        [Route("chung-khoan/bieu-do/{mack}")]
        public ActionResult StockChart(string mack)
        {
            mack = mack.ToUpper();
            var context = new StoreInfoContext();
            var stockInfo = context.Stocks.Find(mack);
            if (stockInfo != null)
                return View(stockInfo);
            else
                return new HttpNotFoundResult("Không tìm thấy mã cổ phiếu");
        }
        [Route("chung-khoan/san/{marketName}")]
        public ActionResult MarketChart(string marketName)
        {
            marketName = marketName.ToUpper();
            var context = new StoreInfoContext();
            var market = context.StockMarkets.Find(marketName);
            if (market != null)
            {
                return View(market);
            }
            else
            {
                return new HttpNotFoundResult("Không tim thấy sàn");
            }
        }

        public JsonResult GetHnxIndexChart()
        {
            HnxIndexEntity hnx = new HnxIndexEntity();
            var client = new VITV_Web.SSIStockService.AjaxWebServiceSoapClient("AjaxWebServiceSoap");
            string result = client.GetHnxIndexChart();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHoseIndexChart()
        {
            HnxIndexEntity hnx = new HnxIndexEntity();
            var client = new VITV_Web.SSIStockService.AjaxWebServiceSoapClient("AjaxWebServiceSoap");
            string result = client.GetHoseIndexChart();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStockBy(string name)
        {
            var context = new StoreInfoContext();
            var stocks = context.Stock_Days.Where(r => r.Ticker == name).ToList();
            var result = Mapper.Map<IEnumerable<StockChartEntity>>(stocks);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMarketBy(string name)
        {
            var context = new StoreInfoContext();
            var stocks = context.StockMarket_Days.Where(r => r.MarketName == name).ToList();
            var result = Mapper.Map<IEnumerable<StockChartEntity>>(stocks);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}