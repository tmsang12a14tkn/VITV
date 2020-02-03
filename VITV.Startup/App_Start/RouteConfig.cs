using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VITV.Startup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "About",
                url: "gioi-thieu",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "Article",
                url: "tin-tuc",
                defaults: new { controller = "Article", action = "Index" }
            );

            routes.MapRoute(
                name: "ArticleCategoryDetails",
                url: "tin-tuc-bai-viet/{title}/{id}",
                defaults: new { controller = "Article", action = "CategoryDetails", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ArtileDetails",
                url: "tin-chu/{date}/{title}/{id}",
                defaults: new { controller = "Article", action = "Details", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GetArticleByKeyword",
                url: "tag/{id}",
                defaults: new { controller = "Article", action = "GetArticleByKeyword", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "VideoCategoryDetails",
                url: "chuong-trinh/{title}/{id}",
                defaults: new { controller = "Video", action = "CategoryDetails", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
