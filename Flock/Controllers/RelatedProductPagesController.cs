using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;
using System.Data.Linq;
using System.Web.Script.Serialization;

namespace Flock.Controllers
{
    public class RelatedProductPagesController : Controller
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
            return RedirectToAction("overview");
        }

        public ActionResult Overview()
        {
            RelatedProductPageViewModelBuilder Builder = new RelatedProductPageViewModelBuilder(UnitOfWork);
            var Model = Builder.Build();
            return View(Model);
        }

        [HttpPost]
        public ActionResult RelatedProductPagePartial(
            string PP_NameOrID,
            byte? PP_ProductTypeID,
            short? PP_CollectionID,
            byte? PP_BrandID,
            short? PP_LaunchID,
            byte? PP_TemplateID,
            byte? PP_StatusID,
            byte? RPP_ProductTypeID,
            short? RPP_CollectionID,
            byte? RPP_BrandID,
            short? RPP_LaunchID,
            byte? RPP_TemplateID,
            byte? RPP_StatusID,
            byte? RPP_Count,
            string RPP_CountOperator,
            int PageLimit,
            int PageOffset)
        {
            if (PP_NameOrID == "")
                PP_NameOrID = null;

            int From = 0;
            int To = 0;

            To = (PageOffset + 1) * PageLimit;
            From = (To - PageLimit) + 1;

            // should be passing pageLimit and pageOffset into this procedure.
            // ... even when limiting the result sets it's slow.
            IMultipleResults Results = UnitOfWork.Context.ProductPage_RelatedProductPage_Select(
                PP_NameOrID,
                PP_ProductTypeID,
                PP_CollectionID,
                PP_BrandID,
                PP_LaunchID,
                PP_TemplateID,
                PP_StatusID,
                RPP_ProductTypeID,
                RPP_CollectionID,
                RPP_BrandID,
                RPP_LaunchID,
                RPP_TemplateID,
                RPP_StatusID,
                RPP_Count,
                RPP_CountOperator,
                From,
                To
            );

            RelatedProductPagePartialViewModel Model = new RelatedProductPagePartialViewModel();

            Model.PrimaryResultSetTotal = Results.GetResult<PPRPPS_TotalCount>().ToList();
            Model.PrimaryResultSet = Results.GetResult<PPRPPS_One>().ToList();
            Model.SecondaryResultSet = Results.GetResult<PPRPPS_Two>().ToList();

            Model.CurrentPage = PageOffset;

            if (PageLimit != 0)
                Model.NumPages = ((Model.PrimaryResultSetTotal[0].TotalCount - 1) / (PageLimit)) + 1;
            else
                Model.NumPages = 1;

            return PartialView(Model);
        }

        [HttpPost]
        public ActionResult UpdateRelatedProductPages(int BaseProductPageID, string ProductPages)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ProductPagesList = ser.Deserialize<List<SortDatum>>(ProductPages);

            foreach (SortDatum d in ProductPagesList)
            {
                if(d.ID != BaseProductPageID)
                { 
                    ProductPage_RelatedProductPage pprpp = UnitOfWork.GetRepository<ProductPage_RelatedProductPage>().GetAll().Where(p => p.ProductPageID == BaseProductPageID && p.RelatedProductPageID == d.ID).SingleOrDefault();
                    if (pprpp == null)
                    {
                        pprpp = new ProductPage_RelatedProductPage() { ProductPageID = BaseProductPageID, RelatedProductPageID = Convert.ToInt32(d.ID), SortOrder = Convert.ToByte(d.SortOrder) };
                        UnitOfWork.GetRepository<ProductPage_RelatedProductPage>().Add(pprpp);
                    }
                    else
                    {
                        pprpp.SortOrder = Convert.ToByte(d.SortOrder);
                    }
                }
            }

            UnitOfWork.Save();

            return Content("");
        }

        [HttpPost]
        public ActionResult SwapProductPage(int OriginalProductPage, int ReplacementProductPage)
        {
            string result = "";
            if(OriginalProductPage != ReplacementProductPage)
            {
                UnitOfWork.Context.ProductPage_RelatedProductPage_Swap(OriginalProductPage, ReplacementProductPage);
                result = "Swap performed successfully.";
            }
            else
            {
                Response.StatusCode = 400;
                result = "Futile swap detected.";
            }
            return Content(result);
        }

        [HttpPost]
        public ActionResult LoadSuggestions(int ProductPageID)
        {
            var Model = new Object();
            return PartialView("SuggestedRelations", Model);
        }

        [HttpPost]
        public ActionResult ShareRelated(int ProductPageID)
        {
            UnitOfWork.Context.ProductPage_RelatedProductPage_CopyMultiple(ProductPageID, false);
            return Content("");
        }

        [HttpPost]
        public ActionResult DeleteAllOff_Count()
        {
            return Content("");//Convert.ToInt32(UnitOfWork.Context.ProductPage_RelatedProductPage_RemoveAllOff(false).ReturnValue).ToString());
        }

        [HttpPost]
        public ActionResult DeleteAllOff_Delete()
        {
            return Content("");//UnitOfWork.Context.ProductPage_RelatedProductPage_RemoveAllOff(true).ReturnValue.ToString());
        }

        /// <summary>
        /// PRIMARY PRODUCTPAGE FILTERS FETCH SECTION
        /// </summary>
        /// <param name="Filters"></param>
        /// <returns></returns>
        #region Primary Filters
        public PartialViewResult ProductPage_ProductTypeDropDown(RelatedProductPage_PPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<ProductType>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }
        public PartialViewResult ProductPage_CollectionDropDown(RelatedProductPage_PPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        public PartialViewResult ProductPage_BrandDropDown(RelatedProductPage_PPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<Brand>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        public PartialViewResult ProductPage_LaunchDropDown(RelatedProductPage_PPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        public PartialViewResult ProductPage_TemplateDropDown(RelatedProductPage_PPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<Flock.Models.Template>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/TemplateDropDownPartial.cshtml", Model);
        }

        public PartialViewResult ProductPage_StatusDropDown(RelatedProductPage_PPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<Flock.Models.Status>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/WebStatusDropDownPartial.cshtml", Model);
        }

        public PartialViewResult ProductPage_NameOrIDTextBox(RelatedProductPage_PPFilters Filters)
        {
            return PartialView("~/Views/Shared/SearchFilters/NameOrIDTextBoxPartial.cshtml", Filters);
        }
        #endregion

        /// <summary>
        /// RELATED PRODUCTPAGE FILTERS FETCH SECTION
        /// </summary>
        /// <param name="Filters"></param>
        /// <returns></returns>
        #region Secondary Filters
        public PartialViewResult RelatedProductPage_ProductTypeDropDown(RelatedProductPage_RPPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<ProductType>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }
        public PartialViewResult RelatedProductPage_CollectionDropDown(RelatedProductPage_RPPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        public PartialViewResult RelatedProductPage_BrandDropDown(RelatedProductPage_RPPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<Brand>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        public PartialViewResult RelatedProductPage_LaunchDropDown(RelatedProductPage_RPPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        public PartialViewResult RelatedProductPage_TemplateDropDown(RelatedProductPage_RPPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<Flock.Models.Template>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/TemplateDropDownPartial.cshtml", Model);
        }

        public PartialViewResult RelatedProductPage_StatusDropDown(RelatedProductPage_RPPFilters Filters)
        {
            var Model = UnitOfWork.GetRepository<Flock.Models.Status>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/WebStatusDropDownPartial.cshtml", Model);
        }
        #endregion
    }
}