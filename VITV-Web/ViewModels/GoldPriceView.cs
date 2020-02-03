using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV_Web.Models.StoreInfo;

namespace VITV_Web.ViewModels
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