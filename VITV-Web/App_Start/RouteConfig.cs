using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VITV.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "About",
                url: "gioi-thieu",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index" }
            );

            routes.MapRoute(
                name: "Reporter",
                url: "cong-dong-vitv",
                defaults: new { controller = "Reporter", action = "Index" }
            );

            routes.MapRoute(
               name: "ReporterDetails",
               url: "btv/{title}",
               defaults: new { controller = "Reporter", action = "Details" }
           );

            routes.MapRoute(
                name: "ProgramScheduleDetails",
                url: "lich-phat-song",
                defaults: new { controller = "ProgramScheduleDetails", action = "Index" }
            );


            routes.MapRoute(
                name: "VideoCategoryCreate",
                url: "tao-moi-chuong-trinh/{groupId}",
                defaults: new { controller = "VideoCategory", action = "Create", groupId = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "VideoCategoryIndex",
                url: "tat-ca-chuong-trinh",
                defaults: new { controller = "VideoCategory", action = "Index" }
            );

            routes.MapRoute(
                name: "VideoCategoryDetails",
                url: "chuong-trinh/{title}",
                defaults: new { controller = "VideoCategory", action = "Details" }
            );

            routes.MapRoute(
                name: "VideoIndex",
                url: "tat-ca-video",
                defaults: new { controller = "Video", action = "Index" }
            );

            routes.MapRoute(
                name: "VideoDetails",
                url: "tin-video/{date}/{title}/{id}",
                defaults: new { controller = "Video", action = "Details", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CreateVideo",
                url: "them-video/{scheduleDetailId}",
                defaults: new { controller = "Video", action = "Create", scheduleDetailId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "InterestRateIndex",
                url: "lai-suat",
                defaults: new { controller = "InterestRate", action = "Index" }
            );

            routes.MapRoute(
                name: "InterestRateFilter",
                url: "lai-suat/f/{_target}",
                defaults: new { controller = "InterestRate", action = "IndexFilter", _target = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ArticleIndex",
                url: "tat-ca-bai-viet",
                defaults: new { controller = "Article", action = "Index" }
            );

            routes.MapRoute(
                name: "ArticleDetails",
                url: "tin-chu/{date}/{title}/{id}",
                defaults: new { controller = "Article", action = "Details", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MoreArticle",
                url: "hien-them-tin-tuc",
                defaults: new { controller = "Article", action = "MoreArticle" }
            );

            routes.MapRoute(
                name: "CreateArticle",
                url: "them-tin-tuc",
                defaults: new { controller = "Article", action = "Create" }
            );

            routes.MapRoute(
                name: "InfoCreate",
                url: "Info/Create",
                defaults: new { controller = "Info", action = "Create" }
            );
            
            routes.MapRoute(
                name: "InfoManagement",
                url: "Info/Management",
                defaults: new { controller = "Info", action = "Management"}
            );

            routes.MapRoute(
              name: "admin",
              url: "admin",
              defaults: new { controller = "Admin", action = "Index" }
          );
            routes.MapRoute(
             name: "quan-ly-nhom-chuyen-muc",
             url: "quan-ly-nhom-chuyen-muc",
             defaults: new { controller = "VideoCatGroups", action = "Index" }
         );
            routes.MapRoute(
              name: "ProgramSchedules",
              url: "ProgramSchedules",
              defaults: new { controller = "ProgramSchedules", action = "Index" }
          );
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
