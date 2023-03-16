using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Flock
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MarketActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);


            if (filterContext.HttpContext.Request.Url.Segments.Length > 1)
            {
                string market = filterContext.HttpContext.Request.Url.Segments[1];

                List<string> ValidMarkets = new List<string> { "en-uk/", "en-us/", "fr-eu/" };

                if (!ValidMarkets.Contains(market) && market != "" && market != null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" }, { "market", "" } });
                    return;
                }
                else
                {
                    filterContext.Controller.ViewBag.Market = market;
                }
            }
        }
    }

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // we aren't using webforms: by default two viewengines process each view
            ViewEngines.Engines.Clear(); ViewEngines.Engines.Add(new RazorViewEngine());

            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();
            
            //AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["GlobalCountryLanguageID"] == null)
                    HttpContext.Current.Session["GlobalCountryLanguageID"] = 0;

                if (HttpContext.Current.Session["GlobalPriceListID"] == null)
                    HttpContext.Current.Session["GlobalCountryID"] = 0;

                if (HttpContext.Current.Session["GlobalPriceListID"] == null)
                    HttpContext.Current.Session["GlobalCountryID"] = 0;
            }
        }

        protected void Application_End()
        {

        }
    }
}