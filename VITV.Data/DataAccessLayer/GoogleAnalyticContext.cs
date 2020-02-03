using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VITV.Data.Models.GoogleAnalytics;

namespace VITV.Data.DAL
{
    public class GoogleAnalyticContext : DbContext
    {
        public GoogleAnalyticContext()
            : base("GoogleAnalyticContext")
        {
        }

        public DbSet<Age> Ages { get; set; }
        public DbSet<AgeOverview> AgeOverviews { get; set; }
        public DbSet<Browser> Browsers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Continent> Continents { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryIsoCode> CountryIsoCodes { get; set; }
        public DbSet<DeviceBranding> DeviceBrandings { get; set; }
        public DbSet<DeviceCategory> DeviceCategories { get; set; }
        public DbSet<DeviceInfo> DeviceInfoes { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<GenderOverview> GenderOverviews { get; set; }
        public DbSet<OS> OSs { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }
        public DbSet<SiteContent> SiteContents { get; set; }
        public DbSet<SiteSearch> SiteSearchs { get; set; }
        public DbSet<SocialNetworkDetail> SocialNetworkDetails { get; set; }
        public DbSet<SocialNetworkGrp> SocialNetworkGrps { get; set; }
        public DbSet<SubContinent> SubContinents { get; set; }
        public DbSet<UserType> UserTypes { get; set; }

    }
}