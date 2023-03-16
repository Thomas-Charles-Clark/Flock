using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using NLog;
using Flock.Models;
using Flock.ViewModels;

namespace Flock.Controllers
{
    public class PublicController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Public/

        [ValidateInput(false)]
        public ActionResult Index(long? PageID)
        {
            // how do we get from being a Product string to an ID...

            if (PageID != null)
            {
                SearchPageViewModelBuilder Builder = new SearchPageViewModelBuilder(UnitOfWork);

                short? countryID = null;

                if (Session["GlobalCountryID"] != null)
                {
                    countryID = Convert.ToInt16(Session["GlobalCountryID"]);
                    if (countryID == 0)
                        countryID = null;
                }

                byte? languageId = null;

                if (Session["GlobalLanguageID"] != null)
                {
                    languageId = Convert.ToByte(Session["GlobalLanguageID"]);
                    if (languageId == 0)
                        languageId = null;
                }

                SearchPageViewModel ViewModel = Builder.Build((long)PageID, countryID, languageId);
                return View(ViewModel);
            }
            else
            {
                // We should be serving the global home page: www.designersguild.com
                // make a page for it...

                PageViewModelBuilder Builder = new PageViewModelBuilder(UnitOfWork);

                short? countryID = null;

                if (Session["GlobalCountryID"] != null)
                {
                    countryID = Convert.ToInt16(Session["GlobalCountryID"]);
                    if (countryID == 0)
                        countryID = null;
                }

                byte? languageId = null;

                if (Session["GlobalLanguageID"] != null)
                {
                    languageId = Convert.ToByte(Session["GlobalLanguageID"]);
                    if (languageId == 0)
                        languageId = null;
                }

                PageComposeViewModel ViewModel = Builder.Build(Convert.ToInt32(27), countryID, languageId);

                return View(ViewModel);
            }
        }

        public ActionResult ProductPage(long ProductID)
        {
            ProductPageViewModelBuilder Builder = new ProductPageViewModelBuilder(UnitOfWork);

            ProductPageViewModel ViewModel = Builder.Build(ProductID, 1);

            return View(ViewModel);
        }

        public ActionResult Collection(long? id)
        {
            CollectionViewModelBuilder Builder = new CollectionViewModelBuilder(UnitOfWork);
            CollectionViewModel Model = Builder.Build(id);

            return View("~/Views/Collections/Index.cshtml", Model);
        }
    }
}
