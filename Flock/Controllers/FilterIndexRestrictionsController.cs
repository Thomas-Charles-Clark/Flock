using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.ViewModels;
using Flock.Models;
using NLog;

namespace Flock.Controllers
{
    public class FilterIndexRestrictionsController : Controller
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
            return View("FilterIndexRestrictions");
        }

        public ActionResult LoadReviews(int ProductID, byte? MarketID)
        {
            var Model = UnitOfWork.GetRepository<ProductReview>().GetAll().Where(p => p.ProductID == ProductID && p.DeletedDT == null).ToList();

            if(MarketID != null)
                Model = Model.Where(p => p.PriceListID == MarketID).ToList();

            return PartialView("ReviewsPartial", Model);
        }

        public ActionResult ProductReviewsGridViewPartial(byte? MarketID)
        {
            return PartialView("_ProductReviewsGridViewPartial", new ReviewGridSettings() { MarketID = MarketID });
        }

        public ActionResult ProductReviewsCollectionDropDownPartial()
        {
            List<Collection> Model = UnitOfWork.GetRepository<Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        public ActionResult ProductReviewsPatternDropDownPartial()
        {
            List<Pattern> Model = UnitOfWork.GetRepository<Pattern>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PatternDropDownPartial.cshtml", Model);
        }

        public ActionResult ProductReviewsColourDropDownPartial()
        {
            List<Colour> Model = UnitOfWork.GetRepository<Colour>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }

        public ActionResult ProductReviewsMarketDropDownPartial()
        {
            List<PriceList> Model = UnitOfWork.GetRepository<PriceList>().GetAll().OrderBy(p => p.PriceListID).ToList();
            return PartialView("~/Views/Shared/SearchFilters/MarketDropDownPartial.cshtml", Model);

        }

        [HttpPost]
        public ActionResult ProductReviewsCollectionListBoxSetSessionVariable(string CollectionIDs)
        {
            Session["ProductReviewsCollectionIDs"] = CollectionIDs;
            return Content("");
        }

        [HttpPost]
        public ActionResult ProductReviewsPatternListBoxSetSessionVariable(string PatternIDs)
        {
            Session["ProductReviewsPatternIDs"] = PatternIDs;
            return Content("");
        }

        [HttpPost]
        public ActionResult ProductReviewsColourListBoxSetSessionVariable(string ColourIDs)
        {
            Session["ProductReviewsColourIDs"] = ColourIDs;
            return Content("");
        }

        [HttpPost]
        public ActionResult ProductReviewsMarketListBoxSetSessionVariable(string MarketIDs)
        {
            Session["ProductReviewsMarketIDs"] = MarketIDs;
            return Content("");
        }
	}
}