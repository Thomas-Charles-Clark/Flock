using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseDashedRouting;

namespace Flock
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            routes.MapRoute(
                name: "admin-controller-action-targetLanguage-id",
                url: "admin/{controller}/{action}/{targetLanguage}/{id}"
            );

            routes.MapRoute(
                name: "admin-controller-action-id",
                url: "admin/{controller}/{action}/{id}"
            );

            routes.MapRoute(
                name: "admin-controller-action-targetLanguage",
                url: "admin/{controller}/{action}/{targetLanguage}"
            );

            routes.MapRoute(
                name: "admin-controller-action-base",
                url: "admin/{controller}/{action}",
                defaults: new { action = "index" }
            );

            routes.MapRoute(
                name: "collections",
                url: "collections/{id}",
                defaults: new { controller = "public", action = "collections", id = 1 }
            );

            routes.MapRoute(
                name: "generic",
                url: "{controller}/{action}",
                defaults: new { controller = "home", action = "index" }
            );
            
            routes.MapRoute(
                name: "admin",
                url: "admin",
                defaults: new { controller = "admin", action = "index" }
            );

            routes.MapRoute(
                name: "home",
                url: "",
                defaults: new { controller = "admin", action = "index" }
            );


            //didn't get this to work yet - mapping subdomains on localhost wasn't playing ball
            //MapSubdomainRoute(routes, "stockists", "", new { controller = "stockists", action = "overview" });

        }

        public static void MapSubdomainRoute(this RouteCollection routes, string name, string url, object defaults = null, object constraints = null)
        {
            routes.Add(name, new SubdomainRoute(url)
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            });
        }
    }

    /// <summary>
    /// All credit: Edward Brey
    /// http://stackoverflow.com/questions/278668/is-it-possible-to-make-an-asp-net-mvc-route-based-on-a-subdomain
    /// </summary>
    public class SubdomainRoute : Route
    {
        public SubdomainRoute(string url) : base(url, new MvcRouteHandler()) { }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);
            if (routeData == null) return null; // Only look at the subdomain if this route matches in the first place.
            string subdomain = httpContext.Request.Params["subdomain"]; // A subdomain specified as a query parameter takes precedence over the hostname.
            if (subdomain == null)
            {
                string host = httpContext.Request.Headers["Host"];
                int index = host.IndexOf('.');
                if (index >= 0)
                    subdomain = host.Substring(0, index);
            }
            if (subdomain != null)
                routeData.Values["subdomain"] = subdomain;
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            object subdomainParam = requestContext.HttpContext.Request.Params["subdomain"];
            if (subdomainParam != null)
                values["subdomain"] = subdomainParam;
            return base.GetVirtualPath(requestContext, values);
        }
    }
}