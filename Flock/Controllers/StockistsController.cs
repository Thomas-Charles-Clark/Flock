using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;

namespace Flock.Controllers
{
    public class StockistsController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        public ActionResult Index()
        { 
            return RedirectToAction("overview"); 
        }

        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult StockistGridViewPartial(string CountryCode)
        {
            return PartialView("StockistGridViewPartial", CountryCode);
        }
	}
}