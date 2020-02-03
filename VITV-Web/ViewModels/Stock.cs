using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Web.ViewModels
{
    public class RealtimeStock
    {
        // 0 stockname
        public string Symbol{get;set;}
        // 1 gm3
        public string Gm3 {get;set;}
        // 2 klm3
        public string Klm3{get;set;}
        // 3 gm2
        public string Gm2 {get;set;}
        // 4 klm2
        public string Klm2 { get; set; }
        // 5 gm1
        public string Gm1 { get; set; }
        // 6 klm1
        public string Klm1 { get; set; }
        // 7 close
        public string Close { get; set; }
        // 8 volume
        public string Volumn { get; set; }
        // 9 price_change
        public string PriceChange { get; set; }
        // 10 percent_change
        public string PercentChange { get; set; }
        // 11 gb1
        public string Gb1 { get; set; }
        // 12 klb1
        public string Klb1 { get; set; }
        // 13 gb2
        public string Gb2 { get; set; }
        // 14 klb2
        public string Klb2 { get; set; }
        // 15 gb3
        public string Gb3 { get; set; }
        // 16 klb3
        public string Klb3 { get; set; }
        // 17 price_open
        public string PriceOpen { get; set; }
        // 18 price_highest
        public string PriceHighest { get; set; }
        // 19 price_lowest
        public string PriceLowest { get; set; }
        // 20 current_volume
        public string CurrentVolumn { get; set; }
        // 21 foreigner_buy_volume
        public string ForeignerBuyVolume { get; set; }
        // 22 foreigner_sell_volume
        public string ForeignerSellVolume { get; set; }
        // 23 volume_deal
        public string VolumnDeal { get; set; }
        // 24 ceiling
        public string Ceiling { get; set; }
        // 25 floor
        public string Floor { get; set; }
        // 26 open
        public string Open { get; set; }
        // 27 value
        public string Value { get; set; }
        // 28 price_fixed
        public string PriceFixed { get; set; }

        public RealtimeStock(string[] stockParams)
        {
            if (stockParams.Length == 29)
            {
                Symbol = stockParams[0];
                Gm3 = stockParams[1];
                Klm3 = stockParams[2];
                Gm2 = stockParams[3];
                Klm2 = stockParams[4];
                Gm1 = stockParams[5];
                Klm1 = stockParams[6];
                Close = stockParams[7];
                Volumn = stockParams[8];
                PriceChange = stockParams[9];
                PercentChange = stockParams[10];
                Gb1 = stockParams[11];
                Klb1 = stockParams[12];
                Gb2 = stockParams[13];
                Klb2 = stockParams[14];
                Gb3 = stockParams[15];
                Klb3 = stockParams[16];
                PriceOpen = stockParams[17];
                PriceHighest = stockParams[18];
                PriceLowest = stockParams[19];
                CurrentVolumn = stockParams[20];
                ForeignerBuyVolume = stockParams[21];
                ForeignerSellVolume = stockParams[22];
                VolumnDeal = stockParams[23];
                Ceiling = stockParams[24];
                Floor = stockParams[25];
                Open = stockParams[26];
                Value = stockParams[27];
                PriceFixed = stockParams[28];
            }
        }
        public override bool Equals(object obj)
        {
            if (this != null && obj != null)
            {
                Type type = this.GetType();
                foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    object selfValue = type.GetProperty(pi.Name).GetValue(this, null);
                    object toValue = type.GetProperty(pi.Name).GetValue(obj, null);

                    if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                    {
                        return false;
                    }
                    
                }
                return true;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}