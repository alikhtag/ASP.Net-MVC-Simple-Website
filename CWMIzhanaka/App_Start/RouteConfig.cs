using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CWMIzhanaka.Controllers;

namespace CWMIzhanaka
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              "User",
               "User/{action}/{id}",
               new { controller = "User", action = "Index", id = UrlParameter.Optional },
               new[] { "CWMIzhanaka.Controllers" }
               );

            routes.MapRoute(
                "ShoppingCart",
                 "ShoppingCart/{action}/{id}",
                 new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
                 new[] { "CWMIzhanaka.Controllers" }
                 );

            routes.MapRoute(
                 "Default",
                 "",
                 new { controller = "Paging", action = "Index" },
                 new[] { "CWMIzhanaka.Controllers" }
            );

            routes.MapRoute(
                 "Paging",
                 "{pg}",
                 new { controller = "Paging", action = "Index" },
                 new[] { "CWMIzhanaka.Controllers" }
            );

            routes.MapRoute(
                 "PgMenuView",
                 "Paging/PgMenuView",
                 new { controller = "Paging", action = "PgMenuView" },
                 new[] { "CWMIzhanaka.Controllers" }
            );

            routes.MapRoute(
                 "SideBarView",
                 "Paging/SideBarView",
                 new { controller = "Paging", action = "SideBarView" },
                 new[] { "CWMIzhanaka.Controllers" }
            );

            routes.MapRoute(
                 "Store",
                 "Store/{action}/{name}",
                 new { controller = "Store", action = "Index", name = UrlParameter.Optional },
                 new[] { "CWMIzhanaka.Controllers" }
            );


        }
    }
}
