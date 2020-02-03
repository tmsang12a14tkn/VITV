using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Portal.ViewModel
{
    public class AgeReport
    {
        public string Name { get; set; }
        public double SessionCount { get; set; }
        public double NewUser { get; set; }
        public int PageView { get; set; }
    }

    public class GenderReport
    {
        public string Name { get; set; }
        public double SessionCount { get; set; }
        public double NewUser { get; set; }
        public int PageView { get; set; }
    }

    public class SocialNetworkReport
    {
        public string Name { get; set; }
        public double SessionCount { get; set; }
        public double PageView { get; set; }
    }

    public class CityReport
    {
        public string Name { get; set; }
        public double SessionCount { get; set; }
        public double NewUser { get; set; }
        public int PageView { get; set; }
    }

    public class CountryIsoCodeReport
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public double SessionCount { get; set; }
        public double NewUser { get; set; }
        public int PageView { get; set; }
    }
    public class UserTypeReport
    {
        public string Name { get; set; }
        public double SessionCount { get; set; }
        public int PageView { get; set; }
    }
    public class OSReport
    {
        public string Name { get; set; }
        public double SessionCount { get; set; }
        public double NewUser { get; set; }
        public int PageView { get; set; }
    }
}