using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.Models;
using NLog;
using Flock.DAL;

namespace Flock.Controllers
{
    public class CountrySearchController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        /*
        @Html.Action("CountrySearchMarketDropDownPartial", "CountrySearch")
        @Html.Action("CountrySearchContinentDropDownPartial", "CountrySearch")
        @Html.Action("CountrySearchRegionDropDownPartial", "CountrySearch")
        @Html.Action("CountrySearchLanguageDropDownPartial", "CountrySearch")
        @Html.Action("CountrySearchNameTextBox", "CountrySearch")
         */
        [AllowAnonymous]
        public ActionResult CountrySearchTestPage()
        {
            return View("~/Views/Shared/CountrySearchTestPage.cshtml");
        }

        [AllowAnonymous]
        public ActionResult CountrySearchMarketDropDownPartial()
        {
            List<Market> Model = UnitOfWork.GetRepository<Market>().GetAll().OrderBy(p => p.Name).ToList();

            return PartialView("~/Views/Shared/SearchFilters/MarketDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult CountrySearchContinentDropDownPartial()
        {
            List<Continent> Model = UnitOfWork.GetRepository<Continent>().GetAll().OrderBy(p => p.Name).ToList();

            return PartialView("~/Views/Shared/SearchFilters/ContinentDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult CountrySearchRegionDropDownPartial()
        {
            List<Region> Model = UnitOfWork.GetRepository<Region>().GetAll().OrderBy(p => p.Name).ToList();

            return PartialView("~/Views/Shared/SearchFilters/RegionDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult CountrySearchLanguageDropDownPartial()
        {
            List<Language> Model = UnitOfWork.GetRepository<Language>().GetAll().OrderBy(p => p.Name).ToList();

            return PartialView("~/Views/Shared/SearchFilters/LanguageDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult CountrySearchNameTextBox()
        {
            return PartialView("~/Views/Shared/SearchFilters/NameOrIDTextBoxPartial.cshtml");
        }

        [AllowAnonymous]
        public ActionResult Search(byte? MarketID, byte? ContinentID, byte? RegionID, byte? LanguageID, string SearchString, Boolean? TableLock, int? ColumnCount)
        {
            ViewBag.ColumnCount = ColumnCount;

            if (TableLock == null)
                TableLock = true;
            if (SearchString == "Country Name" || SearchString == "")
                SearchString = null;
            System.Data.Linq.ISingleResult<Country_Language_SearchResult> Results = null;
            var Model = new List<Country_Language_SearchResult>();
            using(var fdc = new FlockDataContext())
            {
                Results = fdc.Country_Language_Search(MarketID, ContinentID, RegionID, LanguageID, SearchString, TableLock);

                foreach (var csr in Results)
                {
                    Model.Add(csr);
                }
            }
            return PartialView("~/Views/Shared/CountrySearchDataViewPartial.cshtml", Model);
        }
    }
}
