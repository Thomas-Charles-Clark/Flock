using Flock.Models;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Flock.Classes.Auth
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
      
            bool AllowAnonymous = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
              || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (AllowAnonymous)
            {
                return;
            }

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (filterContext.Result is HttpUnauthorizedResult)
                {
                    filterContext.Result = new RedirectResult("~/account/login?returnUrl="+HttpUtility.UrlEncode(String.Join("", filterContext.HttpContext.Request.Url.Segments)));
                    return;
                }
                //foreach (string segment in filterContext.HttpContext.Request.Url.Segments)
                //{
                //    if (segment.ToLower().Contains("admin"))
                //    {
                //        if (HttpContext.Current.User.IsInRole("Admin"))
                //        {
                //            filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.Url.ToString());
                //            return;
                //        }
                //        else
                //        {
                //            filterContext.Result = new RedirectToRouteResult(
                //                new RouteValueDictionary { { "controller", "account" }, { "action", "accessdenied" } });
                //            return;
                //        }
                //    }
                //}
            }
            else // we are authenticated
            {
                var roles = Roles.Split(',');

                // sanity check
                if (roles.Length == 0)
                    throw new ArgumentNullException("roles");

                // if the user is trying to access the admin area
                if (filterContext.HttpContext.Request.Url.Segments[1].ToLower().Contains("admin"))
                {
                    foreach (string s in roles)
                    {
                        if (HttpContext.Current.GetOwinContext().GetUserManager<UserManager>().GetRoles(HttpContext.Current.User.Identity.GetUserId()).Contains(s))//(HttpContext.Current.User.IsInRole(s))
                        {
                            return;
                        }
                    }
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary() { { "controller", "account" }, { "action", "accessdenied" }, { "attempted", String.Join("", filterContext.HttpContext.Request.Url.Segments)} });
                    //}
                }
                else // user is not trying to access admin area, go ahead.
                {
                    return;
                }
            }

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary() { { "controller", "account" }, { "action", "accessdenied" }, { "attempted", String.Join("", filterContext.HttpContext.Request.Url.Segments) } });
                return;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new Ajax401Response();
            }
            else
                base.HandleUnauthorizedRequest(filterContext);
        }

        private class Ajax401Response : ActionResult
        {
            // Called by the MVC framework to run the action result using the specified controller context
            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.StatusCode = 401; // The request requires user authentication
                context.HttpContext.Response.Write("You don't have permission to perform that action"); // HTTP response
                context.HttpContext.Response.End();
            }
        }
    }
}