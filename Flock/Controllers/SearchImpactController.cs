using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public class SearchImpactController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UoW.Disposed)
                UoW.Dispose();
            
            base.Dispose(disposing);
        }

        // GET: SearchImpact
        public ActionResult Index()
        {
            var Model = new SearchImpactViewModel(UoW);

            UoW.Context.ProductPage_SearchSortOrder_Initialise(false, null, null);

            return View(Model);
        }

        [HttpPost]
        public ActionResult Reset(searchImpactSearchTerms objReset)
        {
            UoW.Context.ProductPage_SearchSortOrder_Initialise(true, objReset.selectedLaunchID, objReset.selectedProductTypeID);

            var objReturn = new searchImpactSearchReturn();

            objReturn.results = UoW.Context.ProductPage_SearchSortOrder_Select(objReset.selectedLaunchID, objReset.selectedProductTypeID).ToList();

            return Json(objReturn, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult PerformSearch(searchImpactSearchTerms objSearch)
        {
            var objReturn = new searchImpactSearchReturn();

            objReturn.results = UoW.Context.ProductPage_SearchSortOrder_Select(objSearch.selectedLaunchID, objSearch.selectedProductTypeID).ToList();

            return Json(objReturn, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult Update(List<ProductPageSortOrder> objUpdate, byte selectedProductTypeID, short selectedLaunchID)
        {
            UoW.Context.ProductPage_SearchSortOrder_Update(objUpdate);

            var objReturn = new searchImpactSearchReturn();

            objReturn.results = UoW.Context.ProductPage_SearchSortOrder_Select(selectedLaunchID, selectedProductTypeID).ToList();

            return Json(objReturn, JsonRequestBehavior.DenyGet);
        }
    }
}