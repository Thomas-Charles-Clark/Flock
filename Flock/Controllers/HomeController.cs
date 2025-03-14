using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;

namespace Flock.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public ActionResult GlobalCountryLanguageSelector()
        {
            var Model = UnitOfWork.GetRepository<Country_Language>().GetAll().OrderBy(p => p.SortOrder).ToList();

            return PartialView("GlobalCountryLanguageDropDownPartial", Model);
        }
        
        public ActionResult Index()
        {
            var Model = new BaseViewModel();
            return View(Model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult SetGlobalCountryLanguageID(byte? CountryLanguageID)
        {
            Session["GlobalCountryLanguageID"] = CountryLanguageID;

            var Model = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => p.Country_Language_ID == CountryLanguageID).SingleOrDefault();

            if (Model != null)
            {
                Session["GlobalPriceListID"] = Model.Country.PriceListID;
                Session["GlobalCountryID"] = Model.Country.CountryID;
            }
            else
            {
                Session["GlobalPriceListID"] = 0;
                Session["GlobalCountryID"] = 0;
            }

            return Content("");
        }
    }
}
