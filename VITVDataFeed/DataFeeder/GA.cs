using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VITV.Data.DAL;
using VITV.Data.Models.GoogleAnalytics;

namespace VITVDataFeed.DataFeeder
{
    public static class GA
    {
        private static GoogleAnalytics ga = new GoogleAnalytics(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"VITV-33322adfc7de.p12"), "471958393900-p85uhou0d63gem618letjpf7ico3u36u@developer.gserviceaccount.com");
        
        public static void GetAgeData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:userAgeBracket", "ga:date", "ga:landingPagePath" },
                                                            from, to);
               

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                        Age age = new Age
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.Ages.Add(age);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetAgeData: " + ex.Message);
            }
        }

        public static void GetAgeOverviewData(DateTime to)
        {
            var from = new DateTime(2015, 2, 25);
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext(); VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:userAgeBracket", "ga:landingPagePath" },
                                                            from, to);


                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var pageUrl = r[1];

                    if (pageUrl.Length <= 128)
                    {
                        var entry = context.AgeOverviews.Find(name, pageUrl);
                        if (entry != null)
                        {
                            entry.SessionCount = Convert.ToDouble(r[2]);
                            entry.NewSessionRate = Convert.ToDouble(r[3]);
                            entry.NewUser = Convert.ToDouble(r[4]);
                            entry.BounceRate = Convert.ToDouble(r[5]);
                            entry.PageViewPerSession = Convert.ToDouble(r[6]);
                            entry.AvgSessionDurration = Convert.ToDouble(r[7]);
                        }
                        else
                        {
                            var page = vitvContext.Pages.Find(pageUrl);
                            AgeOverview ageOverview = new AgeOverview
                            {
                                Name = name,
                                PageUrl = pageUrl,
                                SessionCount = Convert.ToDouble(r[2]),
                                NewSessionRate = Convert.ToDouble(r[3]),
                                NewUser = Convert.ToDouble(r[4]),
                                BounceRate = Convert.ToDouble(r[5]),
                                PageViewPerSession = Convert.ToDouble(r[6]),
                                AvgSessionDurration = Convert.ToDouble(r[7]),
                                PageView = Convert.ToInt32(r[8]),
                                VideoId = page != null ? page.VideoId : null,
                                VideoCatId = page != null ? page.VideoCatId : null,
                            };

                            context.AgeOverviews.Add(ageOverview);
                        }
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetAgeData: " + ex.Message);
            }
        }

        public static void GetGenderData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:userGender", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                        Gender gender = new Gender
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.Genders.Add(gender);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetAgeData: " + ex.Message);
            }
        }

        public static void GetGenderOverviewData(DateTime to)
        {
            var from = new DateTime(2015, 2, 25);
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext(); VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:userGender", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var pageUrl = r[1];
                   
                    if (pageUrl.Length <= 128)
                    {
                        var entry = context.GenderOverviews.Find(name, pageUrl);
                        if (entry != null)
                        {
                            entry.SessionCount = Convert.ToDouble(r[2]);
                            entry.NewSessionRate = Convert.ToDouble(r[3]);
                            entry.NewUser = Convert.ToDouble(r[4]);
                            entry.BounceRate = Convert.ToDouble(r[5]);
                            entry.PageViewPerSession = Convert.ToDouble(r[6]);
                            entry.AvgSessionDurration = Convert.ToDouble(r[7]);
                            entry.PageView = Convert.ToInt32(r[8]);
                        }
                        else
                        {
                            var page = vitvContext.Pages.Find(pageUrl);
                            GenderOverview genderOverview = new GenderOverview
                            {
                                Name = name,
                                PageUrl = pageUrl,
                                SessionCount = Convert.ToDouble(r[2]),
                                NewSessionRate = Convert.ToDouble(r[3]),
                                NewUser = Convert.ToDouble(r[4]),
                                BounceRate = Convert.ToDouble(r[5]),
                                PageViewPerSession = Convert.ToDouble(r[6]),
                                AvgSessionDurration = Convert.ToDouble(r[7]),
                                PageView = Convert.ToInt32(r[8]),
                                VideoId = page != null ? page.VideoId : null,
                                VideoCatId = page != null ? page.VideoCatId : null,
                            };
                            context.GenderOverviews.Add(genderOverview);
                        }
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetAgeData: " + ex.Message);
            }
        }

        public static void GetCountryData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:country", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                        Country country = new Country
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.Countries.Add(country);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetCountryData: " + ex.Message);
            }
        }
        public static void GetCountryIsoCodeData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext(); VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:country", "ga:countryIsoCode", "ga:landingPagePath" },
                                                            from, to);
                var test = result.Rows.Where(r => r[1] == "VN" && r[2] == "/ExchangeRates/StockOnline").ToList();
                foreach (var r in result.Rows)
                {
                    var name = r[1];
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                       
                            CountryIsoCode country = new CountryIsoCode
                            {
                                Name = name,
                                PageUrl = pageUrl,
                                FullName = r[0],
                                SessionCount = Convert.ToDouble(r[3]),
                                NewSessionRate = Convert.ToDouble(r[4]),
                                NewUser = Convert.ToDouble(r[5]),
                                BounceRate = Convert.ToDouble(r[6]),
                                PageViewPerSession = Convert.ToDouble(r[7]),
                                AvgSessionDurration = Convert.ToDouble(r[8]),
                                PageView = Convert.ToInt32(r[9]),
                                VideoId = page != null ? page.VideoId : null,
                                VideoCatId = page != null ? page.VideoCatId : null,
                            };
                            context.CountryIsoCodes.Add(country);
                            
                        
                       
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetCountryData: " + ex.Message);
            }
            
        }
        public static void GetCityData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:city", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                        City city = new City
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.Cities.Add(city);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetCityData: " + ex.Message);
            }
        }

        public static void GetContinentData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:continent", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128 && context.Continents.Find(date, name, pageUrl) == null)
                    {
                        Continent continent = new Continent
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.Continents.Add(continent);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetContinentData: " + ex.Message);
            }
        }

        public static void GetSubContinentData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:subContinent", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                        SubContinent subContinent = new SubContinent
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.SubContinents.Add(subContinent);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetSubContinentData: " + ex.Message);
            }
        }

        public static void GetUserTypeData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:userType", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128 && context.UserTypes.Find(date, name, pageUrl) == null)
                    {
                        UserType userType = new UserType
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.UserTypes.Add(userType);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetUserTypeData: " + ex.Message);
            }
        }

        public static void GetBrowserData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:browser", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128 && context.Browsers.Find(date, name, pageUrl) == null)
                    {
                        Browser browser = new Browser
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.Browsers.Add(browser);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetBrowserData: " + ex.Message);
            }
        }

        public static void GetOSData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:operatingSystem", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);
                    if (pageUrl.Length <= 128)
                    {
                        OS os = new OS
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.OSs.Add(os);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetOSData: " + ex.Message);
            }
        }

        public static void GetScreenResolutionData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:screenResolution", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                        Resolution resolution = new Resolution
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.Resolutions.Add(resolution);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetScreenResolutionData: " + ex.Message);
            }
        }

        public static void GetDeviceCategoryData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:deviceCategory", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];
                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                        DeviceCategory deviceCat = new DeviceCategory
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.DeviceCategories.Add(deviceCat);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetDeviceCategoryData: " + ex.Message);
            }
        }

        public static void GetDeviceInfoData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:mobileDeviceInfo", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];

                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                        DeviceInfo deviceInfo = new DeviceInfo
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.DeviceInfoes.Add(deviceInfo);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetDeviceInfoData: " + ex.Message);
            }
        }

        public static void GetDeviceBrandingData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:percentNewSessions", "ga:newUsers", "ga:bounceRate", "ga:pageviewsPerSession", "ga:avgSessionDuration", "ga:pageviews" },
                                                            new string[] { "ga:mobileDeviceBranding", "ga:date", "ga:landingPagePath" },
                                                            from, to);

                foreach (var r in result.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[2];

                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                        DeviceBranding deviceBranding = new DeviceBranding
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            SessionCount = Convert.ToDouble(r[3]),
                            NewSessionRate = Convert.ToDouble(r[4]),
                            NewUser = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            PageViewPerSession = Convert.ToDouble(r[7]),
                            AvgSessionDurration = Convert.ToDouble(r[8]),
                            PageView = Convert.ToInt32(r[9]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.DeviceBrandings.Add(deviceBranding);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetDeviceBrandingData: " + ex.Message);
            }
        }

        public static void GetSocialNetworkData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

                var result_social = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:pageviews", "ga:avgSessionDuration", "ga:pageviewsPerSession" },
                                                            new string[] { "ga:socialNetwork", "ga:date" },
                                                            from, to);

                foreach (var r in result_social.Rows)
                {
                    var name = r[0];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));

                    //if (pageUrl.Length <= 128)
                    {
                        SocialNetworkGrp socialNetworkGrp = new SocialNetworkGrp
                        {
                            Name = name,
                            CreateDate = date,
                            SessionCount = Convert.ToDouble(r[2]),
                            PageView = Convert.ToDouble(r[3]),
                            AvgSessionDurration = Convert.ToDouble(r[4]),
                            PageViewPerSession = Convert.ToDouble(r[5])
                        };
                        context.SocialNetworkGrps.Add(socialNetworkGrp);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetSocialNetworkData: " + ex.Message);
            }
        }

        public static void GetSocialActivityContentUrlData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();
                VITVContext vitvContext = new VITVContext();

                var result_social = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:sessions", "ga:pageviews", "ga:avgSessionDuration", "ga:pageviewsPerSession" },
                                                            new string[] { "ga:socialActivityContentUrl", "ga:date", "ga:socialNetwork" },
                                                            from, to);

                foreach (var r in result_social.Rows)
                {
                    var name = r[2];
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[0];

                    var url = pageUrl.Replace("www.vitv.vn","").Replace("vitv.vn","");
                    var page = vitvContext.Pages.Find(url);

                    if (pageUrl.Length <= 128 && name.Length <= 128)
                    {
                        SocialNetworkDetail socialNetworkDetail = new SocialNetworkDetail
                        {
                            Name = name,
                            CreateDate = date,
                            PageUrl = pageUrl,
                            Url = url,
                            SessionCount = Convert.ToDouble(r[3]),
                            PageView = Convert.ToDouble(r[4]),
                            AvgSessionDurration = Convert.ToDouble(r[5]),
                            PageViewPerSession = Convert.ToDouble(r[6]),
                            VideoId = page!=null? page.VideoId: null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };

                        context.SocialNetworkDetails.Add(socialNetworkDetail);
                        
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetSocialActivityContentUrlData: " + ex.Message);
            }
        }

        public static void GetSearchContentData(DateTime from, DateTime to)
        {
            try
            {
                GoogleAnalyticContext context = new GoogleAnalyticContext();
                VITVContext vitvContext = new VITVContext();

                //search term - ga:searchKeyword
                var result_search = ga.GetAnalyticsData("ga:97823092",
                                                            new string[] { "ga:searchUniques", "ga:avgSearchResultViews", "ga:searchExitRate", "ga:percentSearchRefinements", "ga:searchDuration", "ga:avgSearchDepth", "ga:pageviews" },
                                                            new string[] { "ga:searchKeyword", "ga:date" },
                                                            from, to);

                foreach (var r in result_search.Rows)
                {
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[0];

                    var page = vitvContext.Pages.Find(pageUrl);
                    if (pageUrl.Length <= 128)
                    {
                        SiteSearch siteSearch = new SiteSearch
                        {
                            PageUrl = pageUrl,
                            CreateDate = date,
                            TotalUniqueSearch = Convert.ToDouble(r[2]),
                            AvgSearchResultViews = Convert.ToDouble(r[3]),
                            SearchExitRate = Convert.ToDouble(r[4]),
                            PercentSearchRefinements = Convert.ToDouble(r[5]),
                            SearchDuration = Convert.ToDouble(r[6]),
                            AvgSearchDepth = Convert.ToDouble(r[7]),
                            PageView = Convert.ToInt32(r[8]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                        context.SiteSearchs.Add(siteSearch);
                       
                    }
                } 
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Write("GoogleAnalytics.GetSearchContentData: " + ex.Message);
            }
        }

        public static void GetSiteContentData(DateTime from, DateTime to)
        {
           
            GoogleAnalyticContext context = new GoogleAnalyticContext();VITVContext vitvContext = new VITVContext();

            var result = ga.GetAnalyticsData("ga:97823092",
                                            new string[] { "ga:pageviews", "ga:uniquePageviews", "ga:avgTimeOnPage", "ga:entrances", "ga:bounceRate", "ga:exitRate" },
                                            new string[] { "ga:pagePath", "ga:date" },
                                            from, to);

            foreach (var r in result.Rows)
            {
                try
                {
                    var date = new DateTime(Convert.ToInt32(r[1].Substring(0, 4)), Convert.ToInt32(r[1].Substring(4, 2)), Convert.ToInt32(r[1].Substring(6, 2)));
                    var pageUrl = r[0];

                    var page = vitvContext.Pages.Find(pageUrl);

                    if (pageUrl.Length <= 128)
                    {
                        SiteContent siteContent = new SiteContent
                        {
                            PageUrl = pageUrl,
                            CreateDate = date,
                            PageViews = Convert.ToDouble(r[2]),
                            UniquePageViews = Convert.ToDouble(r[3]),
                            AvgTimeOnPage = Convert.ToDouble(r[4]),
                            Entrances = Convert.ToDouble(r[5]),
                            BounceRate = Convert.ToDouble(r[6]),
                            ExitRate = Convert.ToDouble(r[7]),
                            VideoId = page != null ? page.VideoId : null,
                            VideoCatId = page != null ? page.VideoCatId : null,
                        };
                       
                        context.SiteContents.Add(siteContent);
                        
                    }
                }
                catch (Exception ex)
                {
                    Logger.Write("GoogleAnalytics.GetSiteContentData: " + ex.Message);
                }
            }
            //safe code
            //foreach(var error in context.GetValidationErrors())
            //{
            //    context.Entry(error.Entry).State = EntityState.Deleted;
            //}
            context.SaveChanges();
            
        }

        public static DateTime GetLastUpdate()
        {

            TextReader tr = new StreamReader(Path.Combine(System.Reflection.Assembly.GetEntryAssembly().Location, "/GA.txt"));

            // write lines of text to the file

            var dateStr = tr.ReadLine();

            var date = DateTime.ParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            // close the stream     
            tr.Close();

            return date;
        }
        public static void SetLastUpdate(DateTime date)
        {
            TextWriter tw = new StreamWriter(Path.Combine(System.Reflection.Assembly.GetEntryAssembly().Location, "/GA.txt"));

            // write lines of text to the file

            tw.WriteLine(date.ToString("dd/MM/yyyy"));
            
            // close the stream     
            tw.Close();
            
        }
    }
}
