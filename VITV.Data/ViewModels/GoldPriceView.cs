using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models.StoreInfo;

namespace VITV.Data.ViewModels
{
    public class GoldPriceView
    {
        public List<GoldPriceVietNam> SJCs { get; set; }
        public List<GoldPriceVietNam> PNJs { get; set; }
        public List<GoldPriceVietNam> DOJIs { get; set; }
        public List<GoldPriceVietNam> Banks { get; set; }
        public List<GoldPriceVietNam> Others { get; set; }
    }
}