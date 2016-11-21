using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoGallery
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "Profile/{uniqueUserName}/Photo/{photoName}",
                defaults: new { controller = "Photo", action = "ViewPhoto" }
            );

            routes.MapRoute(
              name: null,
              url: "Profile/{uniqueUserName}/Update/{photoName}",
              defaults: new { controller = "Photo", action = "UpdatePhoto" }
           );

            routes.MapRoute(
               name: null,
               url: "Profile/{uniqueUserName}/Edit/{photoName}",
               defaults: new { controller = "Photo", action = "EditPhotoPage" }
            );

            routes.MapRoute(
                name: null,
                url: "Profile/{uniqueUserName}/{offset}",
                defaults: new { controller = "Photo", action = "ViewWall", uniqueUserName = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: null,
                url: "Profile/{uniqueUserName}",
                defaults: new { controller = "Photo", action = "ViewWall", uniqueUserName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: null,
                url: "Albums/{uniqueUserName}",
                defaults: new { controller = "Album", action = "UserAlbums", uniqueUserName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: null,
                url: "Albums/{uniqueUserName}/{albumName}",
                defaults: new { controller = "Album", action = "ViewAlbum", userName = UrlParameter.Optional, albumName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: null,
                url: "Albums/{uniqueUserName}/{albumName}/{offset}",
                defaults: new { controller = "Album", action = "ViewAlbum", userName = UrlParameter.Optional, albumName = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: null,
               url: "Profile/{uniqueUserName}/Full/{photoName}",
               defaults: new { controller = "Photo", action = "ViewFullSize" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
