using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Flock.Models;
using System.Xml;
using Flock.DAL;
using Flock.ViewModels;
using Flock.Classes.PayPal;

namespace Flock.Controllers 
{
    public class PayPalTestController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return new RedirectResult("Overview");
        }

        public ActionResult Overview()
        {
            PayPalContext PPM = new PayPalContext();

            string token = PPM.FetchToken().Result;

            ViewData["token"] = token;

            return View();
        }
    }
}