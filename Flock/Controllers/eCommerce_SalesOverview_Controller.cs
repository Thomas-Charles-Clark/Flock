using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.Models;
using Flock.DAL;
using Flock.ViewModels;
using System.Data.Linq;

namespace Flock.Controllers
{
    public partial class eCommerceController : Controller
    {
        // GET: eCommerce_SalesOverview
        public ActionResult SalesOverview()
        {
            DateTime from = Convert.ToDateTime("01-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString());
            DateTime to = Convert.ToDateTime("01-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString()).AddMonths(1);
            byte priceList = 1;

            IMultipleResults results = UnitOfWork.Context.Sales_Overview_Generate(from, to, priceList);

            var Model = new SalesOverviewResultsViewModel();
            Model.Summary = results.GetResult<SalesOverviewSummary>().SingleOrDefault();
            Model.Transactions = results.GetResult<SalesOverviewTransaction>().ToList();

            return View("SalesOverview/Index", Model);
        }

        public ActionResult SalesOverviewPriceListDropDownPartial()
        {
            var Model = UnitOfWork.GetRepository<PriceList>().GetAll().OrderBy(p => p.PriceListID).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PriceListDropDownPartial.cshtml", Model);
        }

        public ActionResult SalesOverviewResultsGridView(string From, string To, byte PriceListID)
        {
            var fromDate = Convert.ToDateTime(From);
            var toDate = Convert.ToDateTime(To);

            if (fromDate == toDate)
            {
                toDate = toDate.AddDays(1);
            }

            IMultipleResults results = UnitOfWork.Context.Sales_Overview_Generate(fromDate, toDate, PriceListID);

            var Model = new SalesOverviewResultsViewModel();
            Model.Summary = results.GetResult<SalesOverviewSummary>().SingleOrDefault();
            Model.Transactions = results.GetResult<SalesOverviewTransaction>().ToList();

            return PartialView("SalesOverview/_SalesOverviewResultsGridViewPartial", Model.Transactions);
        }

        [HttpPost]
        public ActionResult UpdateSummary(string From, string To, byte PriceListID)
        {
            var fromDate = Convert.ToDateTime(From);
            var toDate = Convert.ToDateTime(To);

            if (fromDate == toDate)
            {
                toDate = toDate.AddDays(1);
            }

            IMultipleResults results = UnitOfWork.Context.Sales_Overview_Generate(fromDate, toDate, PriceListID);

            var Model = new SalesOverviewResultsViewModel();
            Model.Summary = results.GetResult<SalesOverviewSummary>().SingleOrDefault();
            
            return PartialView("SalesOverview/Results", Model.Summary);
        }
    }
}