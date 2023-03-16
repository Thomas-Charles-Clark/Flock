using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace Flock.Controllers
{
    public class OrdersController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("overview");
        }

        public ActionResult Overview()
        {
            return View();
        }
        
        public ActionResult OrderSearchResultPartial(string OrderRef)
        {
            return PartialView();
        }

        public ActionResult OrderGridPartial()
        {
            return PartialView();
        }

        public ActionResult OrderDetailGridPartial()
        {
            return PartialView();
        }
    }
}