using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;
using Flock.Models;
using System.Web.Script.Serialization;
using DevExpress.Web.Mvc;
using Flock.DAL;
using Flock.ViewModels;
//using Flock.Classes.Auth;
using System.Data.Linq;
using Newtonsoft.Json;
using System.Net;
using NLog.Internal;
using DevExpress.XtraPrinting.Native;

namespace Flock.Controllers
{
    public partial class CMSController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        //[CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        //[CustomAuthorize(Roles="Admin")]
        public ActionResult PageSections()
        {
            return View();
        }

        public ActionResult PageSectionOverviewGridViewPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult PageSectionGridViewCustomCommand(string commandType, string keyString, string dateString)
        {
            bool hasError = false;
            string errorString = "";
            List<long> keys = new List<long>();
            if (keyString != null && keyString != "")
                keys = keyString.Split(',').ToList().ConvertAll(d => Convert.ToInt64(d));

            switch (commandType.ToLower())
            {
                case "enable":
                    try
                    {
                        DateTime date = DateTime.Parse(dateString);
                        List<PageSection> toEnable = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => keys.Contains(p.PageSectionID)).ToList();
                        foreach (PageSection c in toEnable)
                        {
                            if (c.OffDate != null)
                            {
                                if (date < c.OffDate || c.OffDate == null)
                                    c.OnDate = date;
                                else
                                {
                                    errorString += "Page Section " + c.PageSectionID.ToString() + " was not updated because the provided On Date falls after the page's assigned Off Date.\r\n\r\n";
                                    hasError = true;
                                }
                            }
                            else
                            {
                                c.OnDate = date;
                            }
                        }
                        UnitOfWork.Save();
                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] += e.Message + "\r\n\r\n";
                        hasError = true;
                    }
                    break;
                case "disable":
                    try
                    {
                        DateTime date = DateTime.Parse(dateString);
                        List<PageSection> toEnable = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => keys.Contains(p.PageSectionID)).ToList();
                        foreach (PageSection c in toEnable)
                        {

                            if (date >= c.OnDate || c.OnDate == null)
                                c.OffDate = date;
                            else
                            {
                                errorString += "Page Section " + c.PageSectionID.ToString() + " was not updated because the provided Off Date preceeds the page's assigned On Date.\r\n\r\n";
                                hasError = true;
                            }
                        }
                        UnitOfWork.Save();
                    }
                    catch (Exception e)
                    {
                        errorString += e.Message + "\r\n\r\n";
                        hasError = true;
                    }
                    break;
                case "delete":
                    try
                    {
                        List<PageSection> toDelete = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => keys.Contains(p.PageSectionID)).ToList();
                        foreach (PageSection c in toDelete)
                            c.DeletedDT = DateTime.Now;
                        UnitOfWork.Save();
                    }
                    catch (Exception e)
                    {
                        errorString += e.Message + "\r\n\r\n";
                        hasError = true;
                    }
                    break;
                default:
                    break;
            }

            if (hasError)
            {
                Response.StatusCode = 400;
                return Content(errorString);
            }

            return PartialView("PageSectionOverviewGridViewPartial");
        }

        [HttpPost]
        public ActionResult PageSectionProductPageMerge(int PageID, int TargetPageSectionID, int SourcePageSectionID, string keyString, bool replace)
        {
            ViewBag.IsUserShuffling = false;
            try
            {
                var t = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == TargetPageSectionID && p.PageSectionTypeID == 4).SingleOrDefault();

                if (t == null)
                {
                    Response.StatusCode = 400;
                    return Content("Invalid Page Section ID (are you sure it's a Product Page Grid?)");
                }

                if (SourcePageSectionID == TargetPageSectionID)
                {
                    Response.StatusCode = 400;
                    return Content("Source was the same as Target.");
                }

                UnitOfWork.Context.PageSection_ProductPage_Merge(TargetPageSectionID, SourcePageSectionID, keyString, replace);
                UnitOfWork.Save();
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
            
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

            var Model = Builder.Build(PageID, countryID, languageId);

            return PartialView("PageSectionsPartial", Model.PageSections);
        }

        [HttpPost]
        public ActionResult UpdateRelatedPages(int BasePageID, string Pages)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> PagesList = ser.Deserialize<List<SortDatum>>(Pages);

            foreach (SortDatum d in PagesList)
            {
                Page_RelatedPage pprpp = UnitOfWork.GetRepository<Page_RelatedPage>().GetAll().Where(p => p.PageID == BasePageID && p.RelatedPageID == d.ID).SingleOrDefault();
                if (pprpp == null)
                {
                    pprpp = new Page_RelatedPage() { PageID = BasePageID, RelatedPageID = Convert.ToInt32(d.ID), SortOrder = Convert.ToByte(d.SortOrder) };
                    UnitOfWork.GetRepository<Page_RelatedPage>().Add(pprpp);
                }
                else
                {
                    pprpp.SortOrder = Convert.ToByte(d.SortOrder);
                }
            }

            UnitOfWork.Save();

            List<vwPage_RelatedPage> Model = UnitOfWork.GetRepository<vwPage_RelatedPage>().GetAll().Where(p => p.PageID == BasePageID).OrderBy(p => p.SortOrder).ToList();
            return PartialView("PageRelatedPagesPartial", Model);
        }

        [HttpPost]
        public ActionResult RemoveRelatedPage(int BasePageID, int PageID)
        {
            try
            {
                Page_RelatedPage pprpp = UnitOfWork.GetRepository<Page_RelatedPage>().GetAll().Where(p => p.PageID == BasePageID && p.RelatedPageID == PageID).Single();
                UnitOfWork.GetRepository<Page_RelatedPage>().Delete(pprpp);
                UnitOfWork.Save();

                List<vwPage_RelatedPage> Model = UnitOfWork.GetRepository<vwPage_RelatedPage>().GetAll().Where(p => p.PageID == BasePageID).OrderBy(p => p.SortOrder).ToList();
                return PartialView("PageRelatedPagesPartial", Model);
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                if (e.Message.ToLower().Contains("sequence"))
                    return Content("That page was already removed from the list.");
                return Content(e.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult Compose(int? id)
        {
            ViewBag.IsUserShuffling = false;
            if (id != null)
            {
                PageComposeViewModel PVM = null;
                if (UnitOfWork.GetRepository<Page>().GetAll().Where(p => p.PageID == id && p.DeletedDT == null).SingleOrDefault() != null)
                { 
                    PageViewModelBuilder PageBuilder = new PageViewModelBuilder(UnitOfWork);

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

                    PVM = PageBuilder.Build(id, countryID, languageId);
                }
                else
                {
                    PVM = new PageComposeViewModel();
                    ViewData["InvalidPageIDError"] = "Invalid Page ID, or the page is marked as deleted. Deleted pages can be retrieved."; 
                }
                return View("ComposePage", PVM);
            }
            else
            {
                PageComposeViewModel PCVM = new PageComposeViewModel();
                return View("ComposePage", PCVM);
            }
        }

        public ActionResult ComposePageHeaderPageGridSelectorPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ChangePage(int ID)
        {
            ViewBag.IsUserShuffling = false;
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

            PageComposeViewModel Model = Builder.Build(ID, countryID, languageId);

            return PartialView("PageComposePartial", Model);
        }

        [HttpPost]
        public ActionResult getProductTypeURLKeywordsById(byte? productTypeId)
        {
            var obj = LargeDatabaseDataProvider.GetProductTypeByIDForComboBox(new ListEditItemRequestedByValueEventArgs(productTypeId));

            if (obj == null)
            {
                return Content(null);
            }
            else
            {
                return Content(((List<ProductTypeDatum>)obj).FirstOrDefault()?.URLKeywords);
            }
        }

        [AllowAnonymous]
        public ActionResult CMSOverviewSiteSectionDropDownPartial()
        {
            var Model = UnitOfWork.GetRepository<SiteSection>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/SiteSectionDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposePageHeaderSiteSectionDropDownPartial(byte? SelectedSiteSectionID)
        {
            ViewBag.ComposePageHeaderSelectedSiteSectionID = SelectedSiteSectionID;
            var Model = UnitOfWork.GetRepository<SiteSection>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/SiteSectionDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposePageHeaderParentPageDropDownPartial(int? SelectedParentPageID)
        {
            ViewBag.ComposePageHeaderSelectedParentPageID = SelectedParentPageID;
            var Model = UnitOfWork.GetRepository<Page>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/PageDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposePageHeaderPageDropDownPartial(int? SelectedPageID, int? SelectedParentPageID, byte? SelectedSiteSectionID)
        {
            ViewBag.IsUserShuffling = false;

            ViewBag.ComposePageHeaderSelectedPageID = SelectedPageID;
            if (SelectedParentPageID != null && SelectedSiteSectionID != null)
            {
                var Model = UnitOfWork.GetRepository<Page>().GetAll().Where(p => p.ParentID == SelectedParentPageID && p.SiteSectionID == SelectedSiteSectionID).ToList();
                return PartialView("~/Views/Shared/SearchFilters/PageDropDownPartial.cshtml", Model);
            }
            else if (SelectedParentPageID == null && SelectedSiteSectionID != null)
            {
                var Model = UnitOfWork.GetRepository<Page>().GetAll().Where(p => p.SiteSectionID == SelectedSiteSectionID).ToList();
                return PartialView("~/Views/Shared/SearchFilters/PageDropDownPartial.cshtml", Model);
            }
            else if (SelectedParentPageID != null && SelectedSiteSectionID == null)
            {
                var Model = UnitOfWork.GetRepository<Page>().GetAll().Where(p => p.ParentID == SelectedParentPageID).ToList();
                return PartialView("~/Views/Shared/SearchFilters/PageDropDownPartial.cshtml", Model);
            }
            else
            {
                var Model = UnitOfWork.GetRepository<Page>().GetAll().ToList();
                return PartialView("~/Views/Shared/SearchFilters/PageDropDownPartial.cshtml", Model);
            }
        }


        public ActionResult HtmlPageSectionImageUpload()
        {
            try
            {
                var v = new DevExpress.Web.UploadControlValidationSettings
                {
                    AllowedFileExtensions = new string[] { ".jpg", ".jpeg" },
                    MaxFileSize = 4194304
                };

                if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\images\\htmlsectionimages\\"))
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\images\\htmlsectionimages\\");

                HtmlEditorExtension.SaveUploadedFile("HtmlEditor", v, "~\\images\\htmlsectionimages\\");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }

            return null;
        }
        //[HttpPost]
        //public ActionResult LoadSlideshowImageText(long SlideshowID, long ImageID)
        //{
        //    return PartialView("SlideshowImageDialogInnerPartial", new SlideshowImageOverlayViewModelBuilder(UnitOfWork).Build(SlideshowID, ImageID));
        //}

        //[HttpPost]
        //public ActionResult SaveSlideshowImageTextDetails(SlideshowImageViewModel item)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        var db = UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == item.SlideshowID && p.ImageID == item.ImageID).Single();
        //        db.Heading = item.Heading;
        //        db.Text = item.Text;
        //        db.PageID = item.PageID;
        //        db.ProductPageID = item.ProductPageID;
        //        db.CollectionID = item.CollectionID;
        //        db.ExternalURL = item.ExternalURL;
        //        db.NewWindow = item.NewWindow;

        //        UnitOfWork.Save();

        //        return PartialView("SlideshowImageDialogInnerPartial", item);
        //    }
        //    else
        //    {
        //        Response.StatusCode = 400;
        //        return PartialView("SlideshowImageDialogInnerPartial", item);
        //    }
        //}
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateSlideshowImages(long PageSectionID, int SlideshowID, string Images)
        {
            Slideshow_PageSection Model = new Slideshow_PageSection();

            ViewBag.IsUserShuffling = false;

            try
            {
                UnitOfWork.Context.Slideshow_Image_Merge(SlideshowID, Images);
                UnitOfWork.Context.SlideShow_Image_Language_Initialise();
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }

            try
            {
                PageSection ps = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Single();
                
                Model.PageSectionID = PageSectionID;
                Model.PageSectionTypeID = 2;
                Model.SlideshowID = SlideshowID;
                Model.AspectRatio = UnitOfWork.GetRepository<SlideShow>().GetAll().Where(p => p.SlideShowID == SlideshowID).Select(p => p.AspectRatio).Single();
                AspectRatio ar = UnitOfWork.GetRepository<AspectRatio>().GetAll().Where(p => p.AspectRatio1 == Convert.ToDecimal(Model.AspectRatio)).Single();
                Model.ARID = "ar" + ar.AspectRatioID.ToString();
                Model.ARDescription = ar.Name;
                List<SlideShow_Image> imgs = UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == SlideshowID).OrderBy(p => p.SortOrder).ToList();
                Model.Images = new List<SlideshowImage>();
                foreach (SlideShow_Image img in imgs)
                {
                    SlideshowImage ssi = new SlideshowImage();
                    ssi.Record = img;
                    ssi.Hotspots = UnitOfWork.Context.ImageHotspot_Select(ssi.Record.ImageID, 1, DateTime.Now, true).ToList();
                    Model.Images.Add(ssi);
                }
                
                Model.OnDate = ps.OnDate;
                Model.OffDate = ps.OffDate;
                Model.Status = UnitOfWork.GetRepository<Status>().GetAll().Where(p => p.StatusID == UnitOfWork.Context.fnc_Status(Model.OnDate, Model.OffDate)).Select(p => p.Name).Single();
            }
            catch(Exception e)
            {
                Response.StatusDescription = e.Message;
                //log e
            }
            Model.CountryLanguageRestrictions = UnitOfWork.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();

            if (Model.ARID == "ar9" || Model.ARID == "ar10")
            {
                return PartialView("PageSection_HeaderPartial", Model);
            }

            return PartialView("PageSection_SlideshowPartial", Model);
        }

        /// <summary>
        /// Allows addition and removal of Product Page items from a Product Page Grid page section.
        /// </summary>
        /// <param name="PageSectionID">The Page Section to act upon.</param>
        /// <param name="ProductPages">JSON string of ProductPages [{ ID: , SortOrder: },...]</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateProductPageGrid(long PageSectionID, byte? productTypeId, string ProductPages)
        {
            ViewBag.IsUserShuffling = false;

            //  insertion point: whether or not to prepend the products to be added. this can be enacted by comparing extant with input, etc...
            //  how...
            //

            List<PageSection_ProductPage> Current = UnitOfWork.GetRepository<PageSection_ProductPage>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();
            List<long> CurrentPPs = new List<long>();
            List<long> InputPPs = new List<long>();

            foreach (PageSection_ProductPage pspp in Current)
            {
                CurrentPPs.Add(pspp.ProductPageID);
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ProductPagesList = ser.Deserialize<List<SortDatum>>(ProductPages);

            foreach (SortDatum d in ProductPagesList)
            {
                InputPPs.Add(d.ID);
            }
            List<long> Removals = CurrentPPs.Except(InputPPs).ToList();

            foreach (long id in Removals)
            {
                PageSection_ProductPage remove = UnitOfWork.GetRepository<PageSection_ProductPage>().GetAll().Where(p => p.ProductPageID == id && p.PageSectionID == PageSectionID).Single();
                UnitOfWork.GetRepository<PageSection_ProductPage>().Delete(remove);
            }
            UnitOfWork.Save();

            foreach (SortDatum datum in ProductPagesList)
            {
                PageSection_ProductPage pspp = UnitOfWork.GetRepository<PageSection_ProductPage>().GetAll().Where(p => p.PageSectionID == PageSectionID && p.ProductPageID == datum.ID).SingleOrDefault();
                if (pspp == null)
                {
                    pspp = new PageSection_ProductPage() { PageSectionID = PageSectionID, ProductPageID = Convert.ToInt32(datum.ID), SortOrder = (short)datum.SortOrder };
                    UnitOfWork.GetRepository<PageSection_ProductPage>().Add(pspp);
                }
                else
                {
                    pspp.SortOrder = datum.SortOrder;
                }
            }
            UnitOfWork.Save();
            byte? priceListID = 1;
            string currencySymbol = "£";
            if (Session["GlobalPriceListID"] != null)
            {
                priceListID = Convert.ToByte(Session["GlobalPriceListID"]);
                if (priceListID == 0)
                    priceListID = 1;
                var currencyID = UnitOfWork.GetRepository<PriceList>().GetAll().Where(p => p.PriceListID == priceListID).Single().CurrencyID;
                currencySymbol = UnitOfWork.GetRepository<Currency>().GetAll().Where(p => p.CurrencyID == currencyID).Select(p => p.Symbol).Single();
            }
            short? countryID = null;
            if (Session["GlobalCountryID"] != null)
            {
                countryID = Convert.ToInt16(Session["GlobalCountryID"]);
                if (countryID == 0)
                    countryID = null;
            }

            IMultipleResults results = UnitOfWork.Context.PageSection_ProductPage_Select(PageSectionID, priceListID, countryID);

            PageSection_ProductPageViewModel Model = new PageSection_ProductPageViewModel();
            Model.PageSectionID = PageSectionID;
            Model.CurrencySymbol = currencySymbol;
            Model.CoreResults = results.GetResult<PageSection_ProductPage_Select_CoreResult>().OrderBy(p => p.SortOrder).ToList();
            Model.RelatedProductPages = results.GetResult<PageSection_ProductPage_Select_RelatedProductPageResult>().ToList();
            Model.SecondaryProducts = results.GetResult<PageSection_ProductPage_Select_SecondaryProductResult>().ToList();
            Model.PriceAndStock = results.GetResult<PageSection_ProductPage_Select_PriceStockResult>().ToList();
            Model.Colours = results.GetResult<PageSection_ProductPage_Select_ColourResult>().ToList();
            Model.Launches = results.GetResult<PageSection_ProductPage_Select_LaunchResult>().ToList();
            Model.Statuses = results.GetResult<PageSection_ProductPage_Select_StatusResult>().ToList();

            ViewBag.IsUserShuffling = false;

            return PartialView("PageSection_ProductPageGrid", Model);
        }


        /// <summary>
        /// Allows addition and removal of Collection items from a Collection Grid page section.
        /// </summary>
        /// <param name="PageSectionID">The Page Section to act upon.</param>
        /// <param name="ProductPages">JSON string of Collections [{ ID: , SortOrder: },...]</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateCollectionGrid(long PageSectionID, string Collections)
        {
            ViewBag.IsUserShuffling = false;

            //  insertion point: whether or not to prepend the products to be added. this can be enacted by comparing extant with input, etc...
            //  how...
            //

            List<PageSection_Collection> Current = UnitOfWork.GetRepository<PageSection_Collection>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();
            List<long> CurrentCs = new List<long>();
            List<long> InputCs = new List<long>();

            foreach (PageSection_Collection psc in Current)
            {
                CurrentCs.Add(psc.CollectionID);
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> CollectionsList = ser.Deserialize<List<SortDatum>>(Collections);

            foreach (SortDatum c in CollectionsList)
            {
                InputCs.Add(c.ID);
            }
            List<long> Removals = CurrentCs.Except(InputCs).ToList();

            foreach (long id in Removals)
            {
                PageSection_Collection remove = UnitOfWork.GetRepository<PageSection_Collection>().GetAll().Where(p => p.CollectionID == id && p.PageSectionID == PageSectionID).Single();
                UnitOfWork.GetRepository<PageSection_Collection>().Delete(remove);
            }
            UnitOfWork.Save();

            foreach (SortDatum datum in CollectionsList)
            {
                PageSection_Collection psc = UnitOfWork.GetRepository<PageSection_Collection>().GetAll().Where(p => p.PageSectionID == PageSectionID && p.CollectionID == datum.ID).SingleOrDefault();
                if (psc == null)
                {
                    psc = new PageSection_Collection() { PageSectionID = PageSectionID, CollectionID = Convert.ToInt16(datum.ID), SortOrder = datum.SortOrder };
                    UnitOfWork.GetRepository<PageSection_Collection>().Add(psc);
                }
                else
                {
                    psc.SortOrder = datum.SortOrder;
                }
            }
            UnitOfWork.Save();

            //var Model = UnitOfWork.GetRepository<PageSection_ProductPage>().GetAll().Where(p => p.PageSectionID == PageSectionID).OrderBy(p => p.SortOrder).ToList();


            List<PageSection_Collection_SelectResult> Model = new List<PageSection_Collection_SelectResult>();
            System.Data.Linq.ISingleResult<PageSection_Collection_SelectResult> Results = null;

            Results = UnitOfWork.Context.PageSection_Collection_Select(PageSectionID);

            if (Results != null)
            {
                foreach (PageSection_Collection_SelectResult result in Results)
                {
                    Model.Add(result);
                }
            }

            Model = Model.OrderBy(p => p.SortOrder).ToList();

            ViewBag.IsUserShuffling = false;

            return PartialView("PageSection_CollectionGrid", Model);
        }

        /// <summary>
        /// Allows addition and removal of Page items from a Collection Grid page section.
        /// </summary>
        /// <param name="PageSectionID">The Page Section to act upon.</param>
        /// <param name="ProductPages">JSON string of Pages [{ ID: , SortOrder: },...]</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdatePageGrid(long PageSectionID, string Pages)
        {
            ViewBag.IsUserShuffling = false;
            int ThisPageID = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Select(p => p.PageID).Single();
            List<PageSection_Page> Current = UnitOfWork.GetRepository<PageSection_Page>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();
            List<long> CurrentPs = new List<long>();
            List<long> InputPs = new List<long>();

            foreach (PageSection_Page psp in Current)
            {
                CurrentPs.Add(psp.PageID);
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> PagesList = ser.Deserialize<List<SortDatum>>(Pages);

            foreach (SortDatum p in PagesList)
            {
                InputPs.Add(p.ID);
            }
            List<long> Removals = CurrentPs.Except(InputPs).ToList();

            foreach (long id in Removals)
            {
                PageSection_Page remove = UnitOfWork.GetRepository<PageSection_Page>().GetAll().Where(p => p.PageID == id && p.PageSectionID == PageSectionID).Single();
                UnitOfWork.GetRepository<PageSection_Page>().Delete(remove);
            }
            UnitOfWork.Save();

            foreach (SortDatum datum in PagesList)
            {
                PageSection_Page psp = UnitOfWork.GetRepository<PageSection_Page>().GetAll().Where(p => p.PageSectionID == PageSectionID && p.PageID == datum.ID).SingleOrDefault();
                if (psp == null)
                {
                    if (datum.ID != ThisPageID)
                    {
                        psp = new PageSection_Page() { PageSectionID = PageSectionID, PageID = Convert.ToInt32(datum.ID), SortOrder = (byte)datum.SortOrder };
                        UnitOfWork.GetRepository<PageSection_Page>().Add(psp);
                    }
                }
                else
                {
                    psp.SortOrder = (byte)datum.SortOrder;
                }
            }
            UnitOfWork.Save();

            //var Model = UnitOfWork.GetRepository<PageSection_ProductPage>().GetAll().Where(p => p.PageSectionID == PageSectionID).OrderBy(p => p.SortOrder).ToList();


            List<PageSection_Page_SelectResult> Model = new List<PageSection_Page_SelectResult>();
            System.Data.Linq.ISingleResult<PageSection_Page_SelectResult> Results = null;

            Results = UnitOfWork.Context.PageSection_Page_Select(PageSectionID);

            if (Results != null)
            {
                foreach (PageSection_Page_SelectResult result in Results)
                {
                    Model.Add(result);
                }
            }

            Model = Model.OrderBy(p => p.SortOrder).ToList();

            ViewBag.IsUserShuffling = false;

            return PartialView("PageSection_PageGrid", Model);
        }


        /// <summary>
        /// Allows addition and removal of Page items from a Collection Grid page section.
        /// </summary>
        /// <param name="PageSectionID">The Page Section to act upon.</param>
        /// <param name="ProductPages">JSON string of Pages [{ ID: , SortOrder: },...]</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateArticleGrid(long PageSectionID, string Articles)
        {
            ViewBag.IsUserShuffling = false;
            int ThisPageID = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Select(p => p.PageID).Single();
            List<PageSection_Article> Current = UnitOfWork.GetRepository<PageSection_Article>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();
            List<long> CurrentAs = new List<long>();
            List<long> InputAs = new List<long>();

            foreach (PageSection_Article psa in Current)
            {
                CurrentAs.Add(psa.ArticleID);
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ArticlesList = new List<SortDatum>();

            if (Articles != null && Articles != "")
            {
                ArticlesList = ser.Deserialize<List<SortDatum>>(Articles);
            }

            foreach (SortDatum a in ArticlesList)
            {
                InputAs.Add(a.ID);
            }
            List<long> Removals = CurrentAs.Except(InputAs).ToList();

            foreach (long id in Removals)
            {
                PageSection_Article remove = UnitOfWork.GetRepository<PageSection_Article>().GetAll().Where(p => p.ArticleID == id && p.PageSectionID == PageSectionID).Single();
                UnitOfWork.GetRepository<PageSection_Article>().Delete(remove);
            }
            UnitOfWork.Save();

            foreach (SortDatum datum in ArticlesList)
            {
                PageSection_Article psa = UnitOfWork.GetRepository<PageSection_Article>().GetAll().Where(p => p.PageSectionID == PageSectionID && p.ArticleID == datum.ID).SingleOrDefault();
                if (psa == null)
                {
                    if (datum.ID != ThisPageID)
                    {
                        psa = new PageSection_Article() { PageSectionID = PageSectionID, ArticleID = Convert.ToInt32(datum.ID), SortOrder = (byte)datum.SortOrder };
                        UnitOfWork.GetRepository<PageSection_Article>().Add(psa);
                    }
                }
                else
                {
                    psa.SortOrder = (byte)datum.SortOrder;
                }
            }
            UnitOfWork.Save();

            List<PageSection_Article_SelectResult> Model = new List<PageSection_Article_SelectResult>();
            System.Data.Linq.ISingleResult<PageSection_Article_SelectResult> Results = null;

            Results = UnitOfWork.Context.PageSection_Article_Select(PageSectionID);

            if (Results != null)
            {
                foreach (PageSection_Article_SelectResult result in Results)
                {
                    Model.Add(result);
                }
            }

            Model = Model.OrderBy(p => p.SortOrder).ToList();

            ViewBag.IsUserShuffling = false;

            return PartialView("PageSection_ArticleGrid", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdatePageBasicDetails(PageComposeViewModel model)
        {
            ViewBag.IsUserShuffling = false;

            bool hasError = false;
            // test various stuff in addition to 

            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        Page page = UnitOfWork.GetRepository<Page>().GetAll().Where(p => p.PageID == model.PageID).Single();
                        var originalCalculateDiscountValue = page.CalculateDiscount;
                        page.Name = model.Name;
                        page.ModifiedDT = DateTime.Now;
                        page.New = model.New.HasValue ? model.New.Value : false;
                        page.RelatedFilterBold = model.RelatedFilterBold;
                        page.CalculateDiscount = model.CalculateDiscount;
                        page.RelatedFilterFontColour = model.RelatedFilterFontColour;
                        page.SiteSectionID = model.SiteSectionID;
                        page.ProductTypeID = model.ProductTypeID;
                        page.EnableFilterIndexing = model.EnableFilterIndexing;
                        page.Clearance = model.Clearance;
                        page.ShowRecentlyViewed = model.ShowRecentlyViewed;
                        page.NoIndex = model.NoIndex;
                        page.NoFollow = model.NoFollow;
                        page.CanonicalPageID = model.CanonicalPageID;
                        if (model.CanonicalPageID == null)
                        {
                            page.CanonicalAttributeList = null;
                        }
                        UnitOfWork.Save();

                        if (originalCalculateDiscountValue != page.CalculateDiscount)
                        {
                            UnitOfWork.Context.Page_PriceList_Initialise();
                            UnitOfWork.Context.Page_PriceList_Merge();
                        }

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

                        PageComposeViewModel Model = Builder.Build(page.PageID, countryID, languageId);

                        return PartialView("~/Views/CMS/PageBasicInfoPartial.cshtml", Model);
                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] += e.Message;
                        return PartialView("~/Views/CMS/PageBasicInfoPartial.cshtml", model);
                    }
                }
                else {
                    Response.StatusCode = 400;
                    return PartialView("~/Views/CMS/PageBasicInfoPartial.cshtml", model);
                }
            }
            else
                return PartialView("~/Views/CMS/PageBasicInfoPartial.cshtml", model);
        }

        [AllowAnonymous]
        public ActionResult TreeViewPartial(byte? SiteSectionID)
        {
            PageTreeViewModelBuilder Builder = new PageTreeViewModelBuilder(UnitOfWork);

            var Model = Builder.Build(SiteSectionID);

            return PartialView(Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddPageSection(int PageID, byte PageSectionTypeID, decimal SlideshowHeight, decimal AnimationHeight, byte Position, byte BannerLocationGroup)
        {
            ViewBag.IsUserShuffling = false;

            int SortOrder = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageID == PageID).ToList().Count;

            byte InsertedOrder = 0;

            if (Position != 0)
                InsertedOrder = Convert.ToByte(UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageID == PageID && p.Deleted == false).Count());
            else
            {
                var currentSections = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageID == PageID).ToList();
                for (int i = 0; i < currentSections.Count; i++)
                    currentSections[i].SortOrder++;
                UnitOfWork.Save();
            }

            bool isHeader = false;

            if (AnimationHeight == 3.0M || AnimationHeight == 1.869M)
            {
                isHeader = true;
            }

            PageSection newSection = new PageSection() { PageID = PageID, CreatedDT = DateTime.Now, PageSectionTypeID = PageSectionTypeID, SortOrder = InsertedOrder, IsHeader = isHeader };

            UnitOfWork.GetRepository<PageSection>().Add(newSection);
            UnitOfWork.Save();

            switch (PageSectionTypeID)
            {
                case 1:
                    HTMLSection newHtmlSection = new HTMLSection() { Contents = "" };
                    UnitOfWork.GetRepository<HTMLSection>().Add(newHtmlSection);
                    UnitOfWork.Save();
                    UnitOfWork.GetRepository<PageSection_HTMLSection>().Add(new PageSection_HTMLSection() { PageSectionID = newSection.PageSectionID, HtmlSectionID = newHtmlSection.HTMLSectionID });
                    UnitOfWork.Save();
                    UnitOfWork.Context.HTMLSection_Language_Initialise();
                    break;
                case 2:
                    SlideShow newSlideshow = new SlideShow() { Name = "PageSection" + newSection.PageSectionID.ToString() + "Slideshow", Settings = "{}", AspectRatio = SlideshowHeight };
                    UnitOfWork.GetRepository<SlideShow>().Add(newSlideshow);
                    UnitOfWork.Save();
                    UnitOfWork.GetRepository<PageSection_SlideShow>().Add(new PageSection_SlideShow() { PageSectionID = newSection.PageSectionID, SlideShowID = newSlideshow.SlideShowID });
                    UnitOfWork.Save();
                    //UnitOfWork.GetRepository<SlideShow_Image>().Add(new SlideShow_Image() { SlideShowID = newSlideshow.SlideShowID, ImageID = 1 });
                    //UnitOfWork.Save();
                    break;
                case 3: // Form
                        Form newForm = new Form() { Name = "", Heading = "", ConfirmEmail = true, AddressRequired = false, CaptchaRequired = true };
                        UnitOfWork.GetRepository<Form>().Add(newForm);
                        UnitOfWork.Save();
                        UnitOfWork.Context.Form_Language_Initialise();
                        UnitOfWork.GetRepository<PageSection_Form>().Add(new PageSection_Form() { PageSectionID = newSection.PageSectionID, FormID = newForm.FormID });
                        UnitOfWork.Save();
                    break;
                case 4: // Product Page Grid
                    break;
                case 5: // Collection Grid
                    break;
                case 6: // Page Grid
                    break;
                case 7: // Brochure PageSection
                    break;
                case 8: // Banner Location PageSection
                    PageSection_BannerLocationGroup PSBLP = new PageSection_BannerLocationGroup();
                    PSBLP.PageSectionID = newSection.PageSectionID;
                    PSBLP.BannerLocationGroupID = BannerLocationGroup;
                    UnitOfWork.GetRepository<PageSection_BannerLocationGroup>().Add(PSBLP);
                    UnitOfWork.Save();
                    break;
                case 9: // Article Grid
                    break;
                case 10: // Video
                    break;
                case 11: // Masonry
                    break;
                case 12: // Animation
                    UnitOfWork.GetRepository<Animation>().Add(new Animation() { 
                        Name = "PageSection " + newSection.PageSectionID + " Animation",
                        PageSectionID = newSection.PageSectionID,
                        AspectRatioID = UnitOfWork.GetRepository<AspectRatio>().GetAll().Where(p => p.AspectRatio1 == AnimationHeight).Select(p => p.AspectRatioID).Single()
                    });
                    UnitOfWork.Save();
                    break;
                default: break;
            }

            UnitOfWork.Context.PageSection_Language_Initialise();

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

            var Model = Builder.Build(PageID, countryID, languageId);

            return PartialView("PageSectionsPartial", Model.PageSections);
        }

        [HttpPost]
        public ActionResult OverridePageImage(long pageSectionID, int pageID, long imageID)
        {
            PageSection_Page psp = UnitOfWork.GetRepository<PageSection_Page>().GetAll().Where(p => p.PageSectionID == pageSectionID && p.PageID == pageID).Single();
            psp.ImageID = imageID;

            UnitOfWork.Save();

            List<PageSection_Page_SelectResult> Model = new List<PageSection_Page_SelectResult>();
            var Results = UnitOfWork.Context.PageSection_Page_Select(pageSectionID).ToList();

            if (Results != null)
            {
                foreach (PageSection_Page_SelectResult result in Results)
                {
                    Model.Add(result);
                }
            }

            Model = Model.OrderBy(p => p.SortOrder).ToList();

            ViewBag.IsUserShuffling = false;

            return PartialView("PageSection_PageGrid", Model);
        }

        [HttpPost]
        public ActionResult RemovePageImageOverride(long pageSectionID, int pageID)
        {
            PageSection_Page psp = UnitOfWork.GetRepository<PageSection_Page>().GetAll().Where(p => p.PageSectionID == pageSectionID && p.PageID == pageID).Single();
            psp.ImageID = null;

            UnitOfWork.Save();

            List<PageSection_Page_SelectResult> Model = new List<PageSection_Page_SelectResult>();
            var Results = UnitOfWork.Context.PageSection_Page_Select(pageSectionID).ToList();

            if (Results != null)
            {
                foreach (PageSection_Page_SelectResult result in Results)
                {
                    Model.Add(result);
                }
            }

            Model = Model.OrderBy(p => p.SortOrder).ToList();

            ViewBag.IsUserShuffling = false;

            return PartialView("PageSection_PageGrid", Model);
        }

        public ActionResult PageSectionBrochureSelectPartial(long PageSectionID, short? BrochureID)
        {
            var PageSection = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Single();
            
            var LinkerModel = UnitOfWork.GetRepository<PageSection_Brochure>().GetAll().Where(p => p.PageSectionID == PageSectionID).SingleOrDefault();

            if (LinkerModel != null)
            {
                var PreModel = UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == LinkerModel.BrochureID).SingleOrDefault();
                var Model = new BrochurePageSection() { PageSectionID = PageSectionID, Brochure = PreModel };

                if (Model.Brochure != null)
                {
                    Model.BrochureID = Model.Brochure.BrochureID;
                }
                return PartialView("BrochureComboBoxPartial", Model);
            }
            else
            {
                var Model = new BrochurePageSection() { Brochure = null, PageSectionID = PageSectionID, BrochureID = null };
                return PartialView("BrochureComboBoxPartial", Model);
            }
        }

        //public ActionResult PageSectionVideoSelectPartial(long PageSectionID, short? VideoID)
        //{
        //    var PageSection = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Single();

        //    var LinkerModel = UnitOfWork.GetRepository<PageSection_Video>().GetAll().Where(p => p.PageSectionID == PageSectionID).SingleOrDefault();

        //    if (LinkerModel != null)
        //    {
        //        var PreModel = UnitOfWork.GetRepository<Video>().GetAll().Where(p => p.VideoID == LinkerModel.VideoID).SingleOrDefault();
        //        var Model = new Video_PageSection() { PageSectionID = PageSectionID, VideoID = VideoID };

        //        return PartialView("VideoComboBoxPartial", Model);
        //    }
        //    else
        //    {
        //        var Model = new Video_PageSection() { PageSectionID = PageSectionID, VideoID = null };
        //        return PartialView("VideoComboBoxPartial", Model);
        //    }
        //}

        [HttpPost]
        public ActionResult SaveBrochurePageSection(long PageSectionID, short BrochureID)
        {
            ViewBag.IsUserShuffling = false;

            PageSection_Brochure psb = UnitOfWork.GetRepository<PageSection_Brochure>().GetAll().Where(p => p.PageSectionID == PageSectionID).SingleOrDefault();

            if(psb == null)
            {
                psb = new PageSection_Brochure() { PageSectionID = PageSectionID, BrochureID = BrochureID };
                UnitOfWork.GetRepository<PageSection_Brochure>().Add(psb);
                UnitOfWork.Save();
            }
            else if(psb.BrochureID != BrochureID)
            {
                UnitOfWork.GetRepository<PageSection_Brochure>().Delete(psb);
                UnitOfWork.Save();
                psb = new PageSection_Brochure() { PageSectionID = PageSectionID, BrochureID = BrochureID };
                UnitOfWork.GetRepository<PageSection_Brochure>().Add(psb);
                UnitOfWork.Save();
            }

            Brochure PreModel = null;

            UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == psb.BrochureID).SingleOrDefault();
            var Model = new BrochurePageSection() { PageSectionID = PageSectionID, Brochure = PreModel };
            if (Model.Brochure != null)
            {
                Model.BrochureID = Model.Brochure.BrochureID;
                //ViewBag["PageSection" + PageSectionID.ToString() + "SelectedBrochureID"] = Model.Brochure.BrochureID.ToString();
            }
            // log e
            return Content("");// PartialView("PageSection_Brochure", Model);
        }

        public ActionResult SaveAnimationPageSection(long PageSectionID, short AnimationID)
        {
            ViewBag.IsUserShuffling = false;

            try
            {
                var a = UnitOfWork.GetRepository<Animation>().GetAll().Where(p => p.PageSectionID == PageSectionID).Single();

                var Model = new Animation_PageSection()
                {
                    PageSectionID = a.PageSectionID,
                    Animation = a,
                    Status = UnitOfWork.GetRepository<Status>().GetAll().Where(p => p.StatusID == UnitOfWork.Context.fnc_Status(a.PageSection.OnDate, a.PageSection.OffDate)).Select(p => p.Name).Single()
                };

                Model.CountryLanguageRestrictions = UnitOfWork.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();

                return PartialView("PageSection_Animation", Model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = ex.Message;
                return Content("");
            }
        }

        public ActionResult SaveVideoPageSection(long PageSectionID, short? VideoID)
        {
            ViewBag.IsUserShuffling = false;

            if (!VideoID.HasValue)
                throw new Exception("VideoID was null");
            try
            {
                PageSection_Video psv = UnitOfWork.GetRepository<PageSection_Video>().GetAll().Where(p => p.PageSectionID == PageSectionID).SingleOrDefault();

                if(psv == null)
                {
                    psv = new PageSection_Video() { PageSectionID = PageSectionID, VideoID = VideoID.Value };
                    UnitOfWork.GetRepository<PageSection_Video>().Add(psv);
                    UnitOfWork.Save();
                }
                else if(psv.VideoID != VideoID)
                {
                    UnitOfWork.GetRepository<PageSection_Video>().Delete(psv);
                    UnitOfWork.Save();
                    psv = new PageSection_Video() { PageSectionID = PageSectionID, VideoID = VideoID.Value };
                    UnitOfWork.GetRepository<PageSection_Video>().Add(psv);
                    UnitOfWork.Save();
                }

                var PageSection = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Single();

                var Model = new Video_PageSection()
                {
                    PageSectionID = PageSection.PageSectionID,
                    VideoID = VideoID.Value,
                    VideoEmbedCode = psv.Video.EmbedCode,
                    Status = UnitOfWork.GetRepository<Status>().GetAll().Where(p => p.StatusID == UnitOfWork.Context.fnc_Status(psv.PageSection.OnDate, psv.PageSection.OffDate)).Select(p => p.Name).Single()
                };
                return PartialView("PageSection_Video", Model);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = ex.Message;
                return Content("");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateForm(Form_PageSection item, DateTime? OnDate, DateTime? OffDate, long PageSectionID)
        {
            ViewBag.IsUserShuffling = false;
            bool hasError = false;
            if (OffDate != null && OnDate == null)
            {
                hasError = true;
                ModelState.AddModelError("OnDate", "Please set an On Date for this form");
            }
            if (OnDate != null && OffDate != null)
            {
                if (OffDate <= OnDate)
                {
                    hasError = true;
                    ModelState.AddModelError("OnDate", "On Date must precede Off Date.");
                    ModelState.AddModelError("OffDate", "Off Date date must not precede On Date.");
                }
            }
            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    PageSection Section = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Single();
                    PageSection_Form PSF = UnitOfWork.GetRepository<PageSection_Form>().GetAll().Where(p => p.PageSectionID == PageSectionID).Single();
                    Form formsection = UnitOfWork.GetRepository<Form>().GetAll().Where(p => p.FormID == PSF.FormID).Single();
                    Form_PageSection fps = new Form_PageSection();

                    Section.Name = item.PageSectionName;
                    Section.OnDate = item.OnDate;
                    Section.OffDate = item.OffDate;

                    formsection.Heading = item.Heading;
                    formsection.SubHeading = item.SubHeading;
                    formsection.CompetitionID = item.CompetitionID;
                    
                    fps.PageID = item.PageID;
                    fps.PageSectionName = item.PageSectionName;
                    fps.PageSectionID = PageSectionID;
                    fps.OnDate = item.OnDate;
                    fps.OffDate = item.OffDate;
                    fps.PageSectionTypeID = 3;
                    fps.FormID = formsection.FormID;
                    fps.SortOrder = item.SortOrder;
                    fps.SubHeading = formsection.SubHeading;
                    fps.Heading = formsection.Heading;
                    fps.CompetitionID = formsection.CompetitionID;
                    fps.ThirdPartyID = formsection.ThirdPartyID;
                    fps.UseCaptcha = formsection.CaptchaRequired;

                    UnitOfWork.Save();

                    return PartialView("PageSection_FormPartial", fps);
                }
                else
                {
                    Response.StatusCode = 400;
                    return PartialView("PageSection_FormPartial", item);
                }
            }
            else
            {
                Response.StatusCode = 400;
                return PartialView("PageSection_FormPartial", item);
            }
        }

        [AllowAnonymous]
        public ActionResult LoadProductPageGrid(long PageSectionID)
        {
            byte? priceListID = 1;
            string currencySymbol = "£";
            if (Session["GlobalPriceListID"] != null)
            {
                priceListID = Convert.ToByte(Session["GlobalPriceListID"]);
                if (priceListID == 0)
                    priceListID = 1;
                var currencyID = UnitOfWork.GetRepository<PriceList>().GetAll().Where(p => p.PriceListID == priceListID).Single().CurrencyID;
                currencySymbol = UnitOfWork.GetRepository<Currency>().GetAll().Where(p => p.CurrencyID == currencyID).Select(p => p.Symbol).Single();
            }
            short? countryID = null;
            if (Session["GlobalCountryID"] != null)
            {
                countryID = Convert.ToInt16(Session["GlobalCountryID"]);
                if (countryID == 0)
                    countryID = null;
            }

            IMultipleResults results = UnitOfWork.Context.PageSection_ProductPage_Select(PageSectionID, priceListID, countryID);

            PageSection_ProductPageViewModel Model = new PageSection_ProductPageViewModel();
            Model.PageSectionID = PageSectionID;
            Model.CurrencySymbol = currencySymbol;
            Model.CoreResults = results.GetResult<PageSection_ProductPage_Select_CoreResult>().OrderBy(p => p.SortOrder).ToList();
            Model.RelatedProductPages = results.GetResult<PageSection_ProductPage_Select_RelatedProductPageResult>().ToList();
            Model.SecondaryProducts = results.GetResult<PageSection_ProductPage_Select_SecondaryProductResult>().ToList();
            Model.PriceAndStock = results.GetResult<PageSection_ProductPage_Select_PriceStockResult>().ToList();
            Model.Colours = results.GetResult<PageSection_ProductPage_Select_ColourResult>().ToList();
            Model.Launches = results.GetResult<PageSection_ProductPage_Select_LaunchResult>().ToList();
            Model.Statuses = results.GetResult<PageSection_ProductPage_Select_StatusResult>().ToList();

            ViewBag.IsUserShuffling = false;

            return PartialView("PageSection_ProductPageGrid", Model);
        }

        [AllowAnonymous]
        public PartialViewResult LoadCollectionGrid(long PageSectionID)
        {
            List<PageSection_Collection_SelectResult> Model = new List<PageSection_Collection_SelectResult>();
            var Results = UnitOfWork.Context.PageSection_Collection_Select(PageSectionID).ToList();

            if (Results != null)
            {
                foreach (PageSection_Collection_SelectResult result in Results)
                {
                    Model.Add(result);
                }
            }

            Model = Model.OrderBy(p => p.SortOrder).ToList();

            ViewBag.IsUserShuffling = false;

            return PartialView("PageSection_CollectionGrid", Model);
        }

        [AllowAnonymous]
        public PartialViewResult LoadPageGrid(long PageSectionID)
        {
            List<PageSection_Page_SelectResult> Model = new List<PageSection_Page_SelectResult>();
            var Results = UnitOfWork.Context.PageSection_Page_Select(PageSectionID).ToList();

            if (Results != null)
            {
                foreach (PageSection_Page_SelectResult result in Results)
                {
                    Model.Add(result);
                }
            }

            Model = Model.OrderBy(p => p.SortOrder).ToList();

            ViewBag.IsUserShuffling = false;

            return PartialView("PageSection_PageGrid", Model);
        }

        [AllowAnonymous]
        public PartialViewResult LoadArticleGrid(long PageSectionID)
        {
            bool AutoPopulate = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Select(p => p.ArticleAutoPopulate).Single();
            if(!AutoPopulate)
            { 
                List<PageSection_Article_SelectResult> Model = new List<PageSection_Article_SelectResult>();
                var Results = UnitOfWork.Context.PageSection_Article_Select(PageSectionID).ToList();

                if (Results != null)
                {
                    foreach (PageSection_Article_SelectResult result in Results)
                    {
                        Model.Add(result);
                    }
                }

                Model = Model.OrderBy(p => p.SortOrder).ToList();

                ViewBag.IsUserShuffling = false;

                return PartialView("PageSection_ArticleGrid", Model);
            }
            else
            {
                var Model = UnitOfWork.GetRepository<PageSection_ArticleType>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();
                ViewData["PageSectionID"] = PageSectionID;
                return PartialView("PageSection_AutoArticleGrid", Model);
            }
        }

        public PartialViewResult AutoArticleSubTypeGrid(long PageSectionID, byte ArticleTypeID)
        {
            List<PageSection_ArticleSubType> Model = UnitOfWork.GetRepository<PageSection_ArticleSubType>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();

            ViewBag.IsUserShuffling = false;

            ViewData["PageSectionID"] = PageSectionID;
            ViewData["ArticleTypeID"] = ArticleTypeID;
            return PartialView("PageSection_AutoArticleSubTypeGrid", Model);
        }

        [HttpPost]
        public ActionResult UpdatePageSectionArticleSubTypeAutoPopulate(long PageSectionID, string ArticleSubTypes)
        {
            ViewBag.IsUserShuffling = false;
            int ThisPageID = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Select(p => p.PageID).Single();

            var ArticleTypeAssociations = UnitOfWork.GetRepository<PageSection_ArticleType>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();
            foreach(var at in ArticleTypeAssociations)
            {
                UnitOfWork.GetRepository<PageSection_ArticleType>().Delete(at);
            }
            UnitOfWork.Save();

            List<PageSection_ArticleSubType> CurrentSub = UnitOfWork.GetRepository<PageSection_ArticleSubType>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();
            List<long> CurrentSubAs = new List<long>();
            List<long> InputSubAs = new List<long>();

            foreach (PageSection_ArticleSubType psa in CurrentSub)
            {
                CurrentSubAs.Add(psa.ArticleSubTypeID);
            }

            List<byte> SubArticlesList = new List<byte>();

            if (ArticleSubTypes != null && ArticleSubTypes != "")
            {
                SubArticlesList = ArticleSubTypes.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToList();
            }

            foreach (byte a in SubArticlesList)
            {
                InputSubAs.Add(a);
            }
            List<long> SubRemovals = CurrentSubAs.Except(InputSubAs).ToList();

            foreach (long id in SubRemovals)
            {
                PageSection_ArticleSubType remove = UnitOfWork.GetRepository<PageSection_ArticleSubType>().GetAll().Where(p => p.ArticleSubTypeID == id && p.PageSectionID == PageSectionID).Single();
                UnitOfWork.GetRepository<PageSection_ArticleSubType>().Delete(remove);
            }
            UnitOfWork.Save();

            foreach (byte datum in SubArticlesList)
            {
                PageSection_ArticleSubType psa = UnitOfWork.GetRepository<PageSection_ArticleSubType>().GetAll().Where(p => p.PageSectionID == PageSectionID && p.ArticleSubTypeID == datum).SingleOrDefault();
                if (psa == null)
                {
                    if (datum != ThisPageID)
                    {
                        psa = new PageSection_ArticleSubType() { PageSectionID = PageSectionID, ArticleSubTypeID = datum };
                        UnitOfWork.GetRepository<PageSection_ArticleSubType>().Add(psa);
                    }
                }
                else
                {

                }
            }
            UnitOfWork.Save();

            return Content("");
        }

        [HttpPost]
        public ActionResult UpdatePageSectionArticleAutoPopulate(long PageSectionID, string ArticleTypes, string ArticleSubTypes)
        {
            ViewBag.IsUserShuffling = false;
            int ThisPageID = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Select(p => p.PageID).Single();

            var ArticleSubTypeAssociations = UnitOfWork.GetRepository<PageSection_ArticleSubType>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();
            foreach (var at in ArticleSubTypeAssociations)
            {
                UnitOfWork.GetRepository<PageSection_ArticleSubType>().Delete(at);
            }
            UnitOfWork.Save();

            List<PageSection_ArticleType> Current = UnitOfWork.GetRepository<PageSection_ArticleType>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();
            List<long> CurrentAs = new List<long>();
            List<long> InputAs = new List<long>();

            foreach (PageSection_ArticleType psa in Current)
            {
                CurrentAs.Add(psa.ArticleTypeID);
            }
            
            List<byte> ArticlesList = new List<byte>();

            if (ArticleTypes != null && ArticleTypes != "")
            {
                ArticlesList = ArticleTypes.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToList();
            }

            foreach (byte a in ArticlesList)
            {
                InputAs.Add(a);
            }
            List<long> Removals = CurrentAs.Except(InputAs).ToList();

            foreach (long id in Removals)
            {
                PageSection_ArticleType remove = UnitOfWork.GetRepository<PageSection_ArticleType>().GetAll().Where(p => p.ArticleTypeID == id && p.PageSectionID == PageSectionID).Single();
                UnitOfWork.GetRepository<PageSection_ArticleType>().Delete(remove);
            }
            UnitOfWork.Save();

            foreach (byte datum in ArticlesList)
            {
                PageSection_ArticleType psa = UnitOfWork.GetRepository<PageSection_ArticleType>().GetAll().Where(p => p.PageSectionID == PageSectionID && p.ArticleTypeID == datum).SingleOrDefault();
                if (psa == null)
                {
                    if (datum != ThisPageID)
                    {
                        psa = new PageSection_ArticleType() { PageSectionID = PageSectionID, ArticleTypeID = datum };
                        UnitOfWork.GetRepository<PageSection_ArticleType>().Add(psa);
                    }
                }
                else
                {

                }
            }
            UnitOfWork.Save();

            return Content("");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemovePageSection(int PageID, long PageSectionID)
        {
            ViewBag.IsUserShuffling = false;

            PageSection ps = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Single();

            ps.DeletedDT = DateTime.Now;

            UnitOfWork.Save();

            UnitOfWork.Context.PageSection_Sort(PageID);

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

            var Model = Builder.Build(PageID, countryID, languageId);

            return PartialView("PageSectionsPartial", Model.PageSections);
        }

        //[HttpPost]
        //public ActionResult SavePageBasicDetails(int PageID, string PageName)
        //{
        //    Page p = UnitOfWork.GetRepository<Page>().GetAll().Where(p => p.PageID == PageID).SingleOrDefault();
        //    if(p == null)
        //    {
        //        Response.StatusCode = 400;
        //        return Content("Fatal Error: Could not locate PageID " + PageID.ToString() + " in the database. Please contact the system administrator.");
        //    }
        //    else
        //    {
        //        p.Name = PageName;
        //    }
        //    return PartialView("PageBasicInfoPartial");
        //}

        [AllowAnonymous]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult HTMLPageSectionSave(long HTMLSectionID, string Contents)
        {
            HTMLSection section = UnitOfWork.GetRepository<HTMLSection>().GetAll().Where(p => p.HTMLSectionID == HTMLSectionID).Single();
            section.Contents = Contents;
            if (section.Contents == "[No contents]")
                section.Contents = "";
            section.ModifiedDT = DateTime.Now;
            UnitOfWork.Save();
            UnitOfWork.Context.ReplaceLocalURLPlaceholders_HTMLSection(null, HTMLSectionID, null, null, null, null, null, false, false);
            
            return Json(new { });// PartialView("PageSectionHtmlEditor", Model);
        }


        [AllowAnonymous]
        [HttpPost]
        public void SortPageSections(long PageID, string PageSectionInfo)
        {
            ViewBag.IsUserShuffling = true;

            PageViewModelBuilder Builder = new PageViewModelBuilder(UnitOfWork);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<PageSectionInformation> PageSectionList = ser.Deserialize<List<PageSectionInformation>>(PageSectionInfo);

            Builder.ReOrderSections(PageSectionList);
        }

        [AllowAnonymous]
        [HttpPost]
        public ContentResult SortSectionContents(long PageSectionID, string PageSectionContents)
        {
            string result = "";

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<PageSectionContentsInformation> PageSectionContentsList = ser.Deserialize<List<PageSectionContentsInformation>>(PageSectionContents);

            foreach (PageSectionContentsInformation psci in PageSectionContentsList)
            {
                switch (psci.DataType)
                {
                    case "html":
                        HTMLSection sect = UnitOfWork.GetRepository<HTMLSection>().GetAll().Where(p => p.HTMLSectionID == psci.ContentID).Single();
                        UnitOfWork.Save();
                        break;
                    case "slideshow":
                        break;

                    default: break;
                }
            }
            result = "done";

            return Content(result);
        }

        public ActionResult PageSectionBrochure(long PageSectionID)
        {
            var LinkerModel = UnitOfWork.GetRepository<PageSection_Brochure>().GetAll().Where(p => p.PageSectionID == PageSectionID).SingleOrDefault();
            Brochure PreModel = null;
            if(LinkerModel != null)
                UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == LinkerModel.BrochureID).SingleOrDefault();
            var Model = new BrochurePageSection() { PageSectionID = PageSectionID, Brochure = PreModel }; 
            if(Model.Brochure != null) 
            {
                Model.BrochureID = Model.Brochure.BrochureID;
            }
            return PartialView("PageSection_Brochure", Model);
        }

        public class BrochurePageSection
        {
            public long PageSectionID { get; set; }
            public short? BrochureID { get; set; }
            public Brochure Brochure { get; set; }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddNewPage(PageTreeViewModel item, byte siteSectionID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Page p = new Page();
                    p.CreatedDT = DateTime.Now;
                    p.Name = item.Name;
                    p.TitleTag = item.TitleTag;
                    p.ParentID = item.ParentID == null ? 0 : item.ParentID;
                    p.MetaDescription = item.MetaDescription;
                    p.Heading = item.Heading;
                    p.URLKeywords = item.URLKeywords == null ? "" : item.URLKeywords;
                    p.SiteSectionID = siteSectionID;
                    UnitOfWork.GetRepository<Page>().Add(p);
                    UnitOfWork.Save();
                    UnitOfWork.Context.Page_Language_Initialise();
                    //UnitOfWork.Context.ReplaceLocalURLPlaceholders_Page(p.PageID, null, null, null, null, null, false, false);
                    UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, p.PageID, null, null, null, false, false);
                }
                catch (Exception e)
                {
                    ViewData["EditNodeError"] = e.Message;
                }
            }
            else
                ViewData["EditNodeError"] = "Please, correct all errors.";

            PageTreeViewModelBuilder Builder = new PageTreeViewModelBuilder(UnitOfWork);
            var Model = Builder.Build(siteSectionID);

            return PartialView("TreeViewPartial", Model);
        }

        public ActionResult AddNewPageFromGrid(PageTreeViewModel item, byte siteSectionID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Page p = new Page();
                    p.CreatedDT = DateTime.Now;
                    p.Name = item.Name;
                    p.TitleTag = item.TitleTag;
                    p.ParentID = item.ParentID == null ? 0 : item.ParentID;
                    p.MetaDescription = item.MetaDescription;
                    p.Heading = item.Heading;
                    p.URLKeywords = item.URLKeywords == null ? "" : item.URLKeywords;
                    p.SiteSectionID = siteSectionID;
                    UnitOfWork.GetRepository<Page>().Add(p);
                    UnitOfWork.Save();
                    UnitOfWork.Context.Page_Language_Initialise();
                    //UnitOfWork.Context.ReplaceLocalURLPlaceholders_Page(p.PageID, null, null, null, null, null, false, false);
                    UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, p.PageID, null, null, null, false, false);
                }
                catch (Exception e)
                {
                    ViewData["EditNodeError"] = e.Message;
                }
            }
            else
                ViewData["EditNodeError"] = "Please, correct all errors.";

            PageComposeViewModel Model = new PageComposeViewModel();

            return PartialView("ComposePageHeaderPageGridSelectorPartial", Model);
        }

        [HttpPost]
        public ActionResult MovePage(PageTreeViewModel item, byte siteSectionID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Page x = UnitOfWork.GetRepository<Page>().GetAll().Where(p => p.PageID == item.PageID).Single();
                    x.ParentID = item.ParentID == null ? 0 : item.ParentID;
                    UnitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditNodeError"] = e.Message;
                }
            }
            else
                ViewData["EditNodeError"] = "Please, correct all errors.";

            PageTreeViewModelBuilder Builder = new PageTreeViewModelBuilder(UnitOfWork);
            var Model = Builder.Build(siteSectionID);

            return PartialView("TreeViewPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdatePage(string marketID, string siteSectionID)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult DeletePage(Page page, byte? siteSectionID)
        {
            var pa = UnitOfWork.GetRepository<Page>().GetAll().Where(p => p.PageID == page.PageID).Single();
            pa.DeletedDT = DateTime.Now;
            UnitOfWork.Save();

            PageTreeViewModelBuilder Builder = new PageTreeViewModelBuilder(UnitOfWork);
            var Model = Builder.Build(siteSectionID);

            return PartialView("TreeViewPartial", Model);
        }


        [AllowAnonymous]
        public ActionResult DataBindingToXMLPartial()
        {
            return PartialView("~/Views/CMS/_CMSTreeViewPartial.cshtml");
        }


        [AllowAnonymous]
        public ActionResult PageTreeListDataBindingPartial(byte? SiteSectionID)
        {
            PageTreeViewModelBuilder Builder = new PageTreeViewModelBuilder(UnitOfWork);

            var Model = Builder.Build(SiteSectionID);

            return PartialView("~/Views/CMS/TreeViewPartial.cshtml", Model);
        }

        /// <summary>
        /// This can stay.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="pageSectionID"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public PartialViewResult PageSectionSaveBasicSettings(PageSection section, string pageSectionID)
        {
            // must update the target page section with the new data in the input object.
            using (FlockDataContext db = new FlockDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString))
            {
                PageSection toUpdate = db.PageSections.Single(p => p.PageSectionID == section.PageSectionID);
                toUpdate = section;
                db.SubmitChanges();
            }

            return PartialView("~/Views/CMS/Compose/PageSectionBasicSettings.cshtml", section);
        }

        [HttpPost]
        public ActionResult SavePageGridAdvancedSettings(PageSection PS)
        {
            try
            {
                var ps = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PS.PageSectionID).Single();
                ps.MultiplePodImages = PS.MultiplePodImages;
                ps.Paging = PS.Paging;
                ps.ModifiedDT = DateTime.Now;
                UnitOfWork.Save();
                return Content("");
            }
            catch(Exception e)
            {
                Response.StatusCode = 400;
                return Content(e.Message);
            }
        }

        [HttpPost]
        public ActionResult SaveArticleGridAdvancedSettings(PageSection PS)
        {
            try
            {
                var ps = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PS.PageSectionID).Single();
                ps.MultiplePodImages = PS.MultiplePodImages;
                ps.ArticleAutoPopulate = PS.ArticleAutoPopulate;
                ps.Paging = PS.Paging;
                ps.ModifiedDT = DateTime.Now;
                UnitOfWork.Save();
                return Content("");
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Content(e.Message);
            }
        }

        [HttpPost]
        public ActionResult SaveProductPageGridAdvancedSettings(PageSection PS)
        {
            try
            {
                var ps = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PS.PageSectionID).Single();
                ps.MultiplePodImages = PS.MultiplePodImages;
                ps.IsLifestyleGallery = PS.IsLifestyleGallery;
                ps.Filter = PS.Filter;
                ps.Paging = PS.Paging;
                ps.PreviewColourways = PS.PreviewColourways;
                ps.PreviewColourwaysCircularClip = PS.PreviewColourwaysCircularClip;
                ps.ModifiedDT = DateTime.Now;
                UnitOfWork.Save();
                return Content("");
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Content(e.Message);
            }
        }

        [HttpPost]
        public ActionResult SaveCollectionGridAdvancedSettings(PageSection PS)
        {
            try
            {
                var ps = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PS.PageSectionID).Single();
                ps.MultiplePodImages = PS.MultiplePodImages;
                ps.ModifiedDT = DateTime.Now;
                ps.Paging = PS.Paging;
                UnitOfWork.Save();
                return Content("");
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Content(e.Message);
            }
        }

        [HttpPost]
        public ActionResult SaveSlideshowImageOverlaySettings(long PageSectionID, SlideshowImageOverlayViewModel slide)
        {
            try
            {
                Dictionary<string, string> adjustedModelErrors = new Dictionary<string, string>();

                foreach(var p in ModelState)
                {
                    if (p.Value.Errors.Count > 0)
                    {
                        foreach(var k in p.Value.Errors)
                        {
                            adjustedModelErrors.Add("SlideShow" + PageSectionID.ToString() + p.Key, k.ErrorMessage);
                        }
                    }
                }

                foreach (KeyValuePair<string, string> kvp in adjustedModelErrors)
                    ModelState.AddModelError(kvp.Key, kvp.Value);

                if (ModelState.IsValid)
                {
                    var i = UnitOfWork.GetRepository<PageSection_SlideShow>().GetAll().Where(p => p.PageSectionID == PageSectionID).Select(p => p.SlideShowID).Single();
                    var s = UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == i && p.ImageID == slide.ImageID).Single();

                    s.ShowHotspots = slide.ShowHotspots;
                    s.OverlayTop = slide.OverlayTop;
                    s.OverlayLeft = slide.OverlayLeft;
                    s.OverlayWidth = slide.OverlayWidth;
                    s.OverlayHeight = slide.OverlayHeight;
                    s.OverlayFontColour = slide.OverlayFontColour;
                    s.OverlayBackgroundColour = slide.OverlayBackgroundColour;
                    s.OverlayBackgroundOpacity = slide.OverlayBackgroundOpacity;
                    s.SlideshowOverlayPositionID = slide.SlideshowOverlayPositionID;
                    s.AutoHideOverlay = slide.AutoHideOverlay;
                    s.OverlayHeading = slide.OverlayHeading;
                    s.OverlayText = slide.OverlayText;
                    s.DisplayOverlay = slide.DisplayOverlay;
                    s.PageID = slide.PageID;
                    s.CollectionID = slide.CollectionID;
                    s.ProductPageID = slide.ProductPageID;
                    s.ExternalURL = slide.ExternalURL == null ? null : slide.ExternalURL.ToLowerInvariant().TrimEnd(new[] { '/' });
                    s.NewWindow = slide.NewWindow;
                    s.OverlayFade = slide.OverlayFade;
                    slide.Index = (byte)(s.SortOrder + 1);

                    UnitOfWork.Save();
                }

                var arID = "ar" + UnitOfWork.GetRepository<Image>().GetAll().Where(p => p.ImageID == slide.ImageID).Select(p => p.AspectRatioID).Single();

                slide.ARID = arID;

                slide.ImagesTotal = (byte)UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slide.SlideshowID).Count();

                return PartialView("SlideshowImageOverlayPartial", slide);
            }   
            catch(Exception e)
            {
                Response.StatusCode = 400;
                //log e
                return Content(e.Message);
            }
        }

        public ActionResult SlideshowImageOverlayPartial(long pageSectionID, long imageID)
        {
            return PartialView(new SlideshowImageOverlayViewModelBuilder(UnitOfWork).Build(pageSectionID, imageID));
        }

        [HttpPost]
        public ActionResult MergeSlideshowOverlaySetting(long pageSectionID, SlideshowImageOverlayViewModel slide)
        {
            try
            {
                Dictionary<string, string> adjustedModelErrors = new Dictionary<string, string>();

                foreach (var p in ModelState)
                {
                    if (p.Value.Errors.Count > 0)
                    {
                        foreach (var k in p.Value.Errors)
                        {
                            adjustedModelErrors.Add("SlideShow" + pageSectionID.ToString() + p.Key, k.ErrorMessage);
                        }
                    }
                }

                foreach (KeyValuePair<string, string> kvp in adjustedModelErrors)
                    ModelState.AddModelError(kvp.Key, kvp.Value);

                if (ModelState.IsValid)
                {
                    var i = UnitOfWork.GetRepository<PageSection_SlideShow>().GetAll().Where(p => p.PageSectionID == pageSectionID).Select(p => p.SlideShowID).Single();
                    var s = UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == i && p.ImageID == slide.ImageID).Single();

                    s.ShowHotspots = slide.ShowHotspots;
                    s.OverlayTop = slide.OverlayTop;
                    s.OverlayLeft = slide.OverlayLeft;
                    s.OverlayWidth = slide.OverlayWidth;
                    s.OverlayHeight = slide.OverlayHeight;
                    s.OverlayFontColour = slide.OverlayFontColour;
                    s.OverlayBackgroundColour = slide.OverlayBackgroundColour;
                    s.OverlayBackgroundOpacity = slide.OverlayBackgroundOpacity;
                    s.SlideshowOverlayPositionID = slide.SlideshowOverlayPositionID;
                    s.AutoHideOverlay = slide.AutoHideOverlay;
                    s.OverlayHeading = slide.OverlayHeading;
                    s.OverlayText = slide.OverlayText;
                    s.DisplayOverlay = slide.DisplayOverlay;
                    s.PageID = slide.PageID;
                    s.CollectionID = slide.CollectionID;
                    s.ProductPageID = slide.ProductPageID;
                    s.ExternalURL = slide.ExternalURL == null ? null : slide.ExternalURL.ToLowerInvariant().TrimEnd(new[] { '/' });
                    s.NewWindow = slide.NewWindow;
                    s.OverlayFade = slide.OverlayFade;

                    slide.Index = (byte)(s.SortOrder + 1);

                    UnitOfWork.Save();

                    UnitOfWork.Context.Slideshow_Image_MergeOverlays(s.SlideShowID, slide.ImageID);
                }
                else
                {
                    Response.StatusCode = 400;
                }

                var arID = "ar" + UnitOfWork.GetRepository<Image>().GetAll().Where(p => p.ImageID == slide.ImageID).Select(p => p.AspectRatioID).Single();

                slide.ARID = arID;

                slide.ImagesTotal = (byte)UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slide.SlideshowID).Count();

                long SlideShowID = UnitOfWork.GetRepository<PageSection_SlideShow>().GetAll().Where(p => p.PageSectionID == pageSectionID).Select(p => p.SlideShowID).Single();

                return PartialView("SlideshowImageOverlayPartial", slide);
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                //log e
                return Content(e.Message);
            }
        }

        [HttpPost]
        public ActionResult UpdatePageSectionCountryRestrictions(long pageSectionID, string countryLanguageRestrictions, byte pageSectionTypeId)
        {
            var incomingRestrictions = JsonConvert.DeserializeObject<List<CountryLanguageSortDatum>>(countryLanguageRestrictions ?? "[]");

            var currentRestrictions = UnitOfWork.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == pageSectionID).ToList();

            var countryLanguages = new List<CountryLanguageSortDatum>();

            foreach (var cl in currentRestrictions)
            {
                countryLanguages.Add(new CountryLanguageSortDatum() { CountryID = cl.CountryID, LanguageID = cl.LanguageID, SortOrder = 0 });
            }

            var removals = countryLanguages.Except(incomingRestrictions).ToList();

            foreach (var datum in removals)
            {
                var remove = UnitOfWork.GetRepository<PageSection_Country_Language>().GetAll().Single(p => p.PageSectionID == pageSectionID && p.CountryID == datum.CountryID && p.LanguageID == datum.LanguageID);
                UnitOfWork.GetRepository<PageSection_Country_Language>().Delete(remove);
            }
            UnitOfWork.Save();

            //could probably do deletion from here too: if ProductPage_Country table holds record which isn't in Countries passed to this function, then delete those.

            foreach (var datum in incomingRestrictions)
            {
                var bc = UnitOfWork.GetRepository<PageSection_Country_Language>().GetAll().SingleOrDefault(p => p.PageSectionID == pageSectionID && p.CountryID == datum.CountryID && p.LanguageID == datum.LanguageID);

                if (bc == null)
                {
                    bc = new PageSection_Country_Language() { PageSectionID = pageSectionID, CountryID = datum.CountryID, LanguageID = datum.LanguageID };
                    UnitOfWork.GetRepository<PageSection_Country_Language>().Add(bc);
                }
            }

            UnitOfWork.Save();

            var model = UnitOfWork.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == pageSectionID).ToList();

            switch (pageSectionTypeId)
            {
                case 1:
                    var htmlSectionId = UnitOfWork.GetRepository<PageSection_HTMLSection>().GetAll().Single(p => p.PageSectionID == pageSectionID).HtmlSectionID;
                    UnitOfWork.Context.HTMLSection_Country_Language_Initialise(htmlSectionId);
                    break;
                // Do we actually want to do this... 
                //case 9:
                //    var articleIds = UnitOfWork.GetRepository<PageSection_Article>().GetAll().Where(p => p.PageSectionID == pageSectionID).Select(a => a.ArticleID);
                //    articleIds.ForEach(aid =>
                //    {
                //        UnitOfWork.Context.Article_Country_Language_Initialise(aid);
                //    });
                //    break;
                default: break;
            }
            
            return PartialView("CountryLanguageRestrictionPartial", model);
        }

        ///// <summary>
        ///// Should be handled by UpdateSlideshowImages
        ///// </summary>
        ///// <param name="slideshowID"></param>
        ///// <param name="imageID"></param>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult RemoveSlideShowImage(long slideshowID, long imageID)
        //{
        //    SlideShow_Image ssi = UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slideshowID && p.ImageID == imageID).Single();
        //    UnitOfWork.GetRepository<SlideShow_Image>().Delete(ssi);
        //    UnitOfWork.Save();

        //    return Content("");
        //}

        //[HttpPost]
        //public ActionResult RemovePodImage(int PageID, long ImageID)
        //{
        //    Page_Image pi = UnitOfWork.GetRepository<Page_Image>().GetAll().Where(p => p.PageID == PageID && p.ImageID == ImageID).Single();
        //    UnitOfWork.GetRepository<Page_Image>().Delete(pi);
        //    UnitOfWork.Save();
        //    return Content("");
        //}

        ///// <summary>
        ///// Should be handled by UpdateSlideshowImages
        ///// </summary>
        ///// <param name="slideshowID"></param>
        ///// <param name="imagesSort"></param>
        ///// <returns></returns>
        //public ActionResult UpdateSlideShowOrder(long slideshowID, string imagesSort)
        //{
        //    List<SlideShowImageSort> images = new JavaScriptSerializer().Deserialize<List<SlideShowImageSort>>(imagesSort);

        //    for (int i = 0; i < images.Count; i++)
        //    {
        //        SlideShow_Image image = UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slideshowID && p.ImageID == images[i].ImageID).Single();
        //        image.SortOrder = images[i].SortOrder;

        //        UnitOfWork.Save();
        //    }

        //    return Content("Success");
        //}

        ///// <summary>
        ///// the SlideShow table needs a more specific name...
        ///// </summary>
        ///// <param name="slideshowID">ID of the slideshow to upd...a...te...</param>
        ///// <param name="imagesInput">JSON string of 'Datum'</param>
        ///// <returns></returns>
        ///// 
        ///// could this not be replaced with a simple 'UpdateSlideshowImages' as is found elsewhere 
        ///// 
        //public ActionResult AddImagesToSlideShow(long slideshowID, string imagesInput)
        //{
        //    List<SlideShowImageSort> images = new JavaScriptSerializer().Deserialize<List<SlideShowImageSort>>(imagesInput);
        //    int lastSort = 0;
        //    List<SlideShow_Image> imagesInSlideshow = UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slideshowID).ToList();
        //    List<SlideShowImageSort> iIS = imagesInSlideshow.Select(s => new SlideShowImageSort() { ImageID = s.ImageID, SortOrder = s.SortOrder }).ToList();

        //    List<long> validAdditions = images.Select(p => p.ImageID).ToList().Except(iIS.Select(o => o.ImageID).ToList()).ToList();

        //    if (imagesInSlideshow.Count > 0)
        //        lastSort = imagesInSlideshow.Max(p => p.SortOrder);

        //    for (int i = 0; i < validAdditions.Count; i++)
        //    {
        //        SlideShow_Image image = new SlideShow_Image();
        //        image.ImageID = validAdditions[i];

        //        image.SortOrder = lastSort + (i + 1);

        //        image.SlideShowID = slideshowID;

        //        UnitOfWork.GetRepository<SlideShow_Image>().Add(image);
        //    }

        //    UnitOfWork.Save();

        //    return Content("");
        //}

        /// <summary>
        /// Should be named "Datum", actually should find out if previously defined Datum struct is accessible here.
        /// </summary>
        public struct SlideShowImageSort
        {
            public long ImageID;
            public int SortOrder;
        };

        /// <summary>
        /// Required anymore?
        /// </summary>
        /// <param name="marketID"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SiteSectionComboBoxPartial(long marketID)
        {
            //long id = DevExpress.Web.Mvc.ComboBoxExtension.GetValue<long>("MarketListComboBox");

            ViewData["SiteSections"] = UnitOfWork.GetRepository<SiteSection>().GetAll().Where(p => UnitOfWork.GetRepository<SiteSection_Market>().GetAll().Where(x => x.MarketID == marketID).Select(x => x.SiteSectionID).ToList().Contains(p.SiteSectionID)).ToList();

            return PartialView("_CMSSiteSectionComboPartial");
        }

        /// <summary>
        /// ??????????
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public PartialViewResult PagesPartial()
        {
            return PartialView("~/Views/CMS/_CMSTreeViewPartial.cshtml");
        }


        /*
        PageSectionID: PageSectionID,
        PageSectionName: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "PageSectionName']").val(),
        OnDate: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "OnDate']").val(),
        OffDate: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "OffDate']").val(),
         */
        [HttpPost]
        public ActionResult SaveFormDetails(Form_PageSection item, long PageSectionID, string PageSectionName, DateTime? OnDate, DateTime? OffDate)
        {
            if (item.OffDate != null && item.OnDate == null)
            {
                ModelState.AddModelError("OnDate", "Please set an On Date.");
            }
            if (item.OnDate != null && item.OffDate != null)
            {
                if (item.OffDate <= item.OnDate)
                {
                    ModelState.AddModelError("OnDate", "On Date must precede Off Date.");
                    ModelState.AddModelError("OffDate", "Off Date date must not precede On Date.");
                }
            }
            if (item.SourceID == 1 && item.CompetitionID == null)
            {
                ModelState.AddModelError("CompetitionID", "You must specify a Competition");
            }
            if (item.SourceID == 2 && item.BrochureID == null)
            {
                ModelState.AddModelError("BrochureID", "You must specify a Brochure");
            }
            if (item.SourceID == 72 && item.StoreEventID == null)
            {
                ModelState.AddModelError("StoreEventID", "You must specify a Store Event");
            }
            Dictionary<string, string> adjustedModelErrors = new Dictionary<string, string>();

            foreach (var p in ModelState)
            {
                if (p.Value.Errors.Count > 0)
                {
                    foreach (var k in p.Value.Errors)
                    {
                        adjustedModelErrors.Add("Form" + item.FormID.ToString() + p.Key, k.ErrorMessage);
                    }
                }
            }

            foreach (KeyValuePair<string, string> kvp in adjustedModelErrors)
                ModelState.AddModelError(kvp.Key, kvp.Value);

            Form f = UnitOfWork.GetRepository<Form>().GetAll().Where(p => p.FormID == item.FormID).Single();
            PageSection ps = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == item.PageSectionID).Single();
            if(ModelState.IsValid)
            {
                f.SourceID = item.SourceID;
                f.SubHeading = item.SubHeading;
                f.Heading = item.Heading;
                f.CompetitionID = item.CompetitionID;
                f.ThirdPartyID = item.ThirdPartyID;
                f.CaptchaRequired = item.UseCaptcha;
                f.Name = item.Name;
                f.BrochureID = item.BrochureID;
                f.AddressRequired = item.DisplayAddressBlock;
                f.ConfirmEmail = item.ConfirmEmail;
                ps.OnDate = item.OnDate;
                ps.OffDate = item.OffDate;
                ps.Name = item.PageSectionName;
                f.StoreEventID = item.StoreEventID;
                f.RedirectPageID = item.RedirectPageID;
                f.PromotionCodeID = item.PromotionCodeID;
                f.PhoneNumberRequired = item.PhoneNumberRequired;
                UnitOfWork.Save();
            }
            else
            {
                Response.StatusCode = 400;
            }
            
            item.Status = UnitOfWork.GetRepository<Status>().GetAll().Where(p => p.StatusID == UnitOfWork.Context.fnc_Status(item.OnDate, item.OffDate)).Select(p => p.Name).Single();

            item.CountryLanguageRestrictions = UnitOfWork.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PageSectionID).ToList();
           
            ViewBag.IsUserShuffling = false;

            if (item.SourceID == 2 && item.BrochureID != null) 
            {
                var psbLinker = UnitOfWork.GetRepository<PageSection_Brochure>().GetAll().Where(p => p.PageSectionID == item.PageSectionID).ToList();

                UnitOfWork.GetRepository<PageSection_Brochure>().DeleteAll(psbLinker);

                UnitOfWork.Save();

                UnitOfWork.GetRepository<PageSection_Brochure>().Add(new PageSection_Brochure() { PageSectionID = item.PageSectionID, BrochureID = item.BrochureID.Value });

                UnitOfWork.Save();

            }

            return PartialView("PageSection_FormPartial", item);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SavePageSectionHeaderDetails(BasePageSection item)
        {
            bool hasError = false;
            if (item.OffDate != null && item.OnDate == null)
            {
                hasError = true;
                ModelState.AddModelError("OnDate", "Please set an On Date.");
            }
            if (item.OnDate != null && item.OffDate != null)
            {
                if (item.OffDate <= item.OnDate)
                {
                    hasError = true;
                    ModelState.AddModelError("OnDate", "On Date must precede Off Date.");
                    ModelState.AddModelError("OffDate", "Off Date date must not precede On Date.");
                }
            }
            //if (item.PageSectionName.Length > 50 || item.PageSectionName.Length < 3)
            //{
            //    hasError = true;
            //    ModelState.AddModelError("PageSectionName", "The field PageSectionName must be a string with a minimum length of 3 and a maximum length of 50.");
            //}

            if (!hasError)
            {
                if(ModelState.IsValid)
                { 
                    // must resurrect a basepagesection object from the details given... must update the correct pagesection 
                    BasePageSection Model = new BasePageSection();
                    PageSection section = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == item.PageSectionID).Single();

                    section.Name = item.PageSectionName;
                    section.Heading = item.Heading;
                    section.OnDate = item.OnDate;
                    section.OffDate = item.OffDate;
                    section.IsHeader = item.IsHeader;
                    section.PlatformSwitch = item.PlatformSwitch;
                    section.TradeSwitch = item.TradeSwitch;
                    section.HalfWidth = item.HalfWidth;
                    section.ViewportWidth = item.ViewportWidth;
                    section.Paging = item.Paging;
                    section.ModifiedDT = DateTime.Now;

                    Model.PageSectionTypeID = item.PageSectionTypeID;
                    Model.PageSectionID = item.PageSectionID;
                    Model.PageSectionName = item.PageSectionName;
                    Model.Heading = item.Heading;
                    Model.IsHeader = item.IsHeader;
                    Model.PlatformSwitch = item.PlatformSwitch;
                    Model.TradeSwitch = item.TradeSwitch;
                    Model.TradePartnerAvailability = item.TradePartnerAvailability;
                    Model.HalfWidth = item.HalfWidth;
                    Model.ViewportWidth = item.ViewportWidth;
                    Model.OnDate = item.OnDate;
                    Model.OffDate = item.OffDate;
                    Model.Status = UnitOfWork.GetRepository<Status>().GetAll().Where(p => p.StatusID == UnitOfWork.Context.fnc_Status(Model.OnDate, Model.OffDate)).Select(p => p.Name).Single();

                    UnitOfWork.Save();

                    return PartialView("PageSectionHeaderDetails", Model);
                }
                else
                {
                    Response.StatusCode = 400;
                    return PartialView("PageSectionHeaderDetails", item);
                }
            }
            else
            {
                BasePageSection Model = new BasePageSection();
                Model.PageSectionTypeID = item.PageSectionTypeID;
                Model.PageSectionID = item.PageSectionID;
                Model.PageSectionName = item.PageSectionName;
                Model.OnDate = item.OnDate;
                Model.OffDate = item.OffDate;
                Model.Status = "Unsaved";//UnitOfWork.GetRepository<Status>().GetAll().Where(p => p.StatusID == UnitOfWork.Context.fnc_Status(Model.OnDate, Model.OffDate)).Select(p => p.Name).Single();

                Response.StatusCode = 400;
                return PartialView("PageSectionHeaderDetails", Model);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SavePageDetails(PageComposeViewModel item)
        {
            if (ModelState.IsValid)
            {
                Page p = UnitOfWork.GetRepository<Page>().GetAll().Where(x => x.PageID == item.PageID).Single();
                p.TitleTag = item.TitleTag;
                if (item.URLKeywords == null)
                    item.URLKeywords = "";
                p.URLKeywords = item.URLKeywords;
                p.Description = item.Description;
                p.MetaDescription = item.MetaDescription;
                p.Heading = item.Heading;
                p.SubHeading = item.SubHeading;
                p.ModifiedDT = DateTime.Now;
                UnitOfWork.Save();

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_Page(p.PageID, null, null, null, null, null, false, false);

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, p.PageID, null, null, null, false, false);

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

                var Model = Builder.Build(item.PageID, countryID, languageId);

                return PartialView("PageDetailsPartial", Model);
            }
            else
            {
                // rebuilding images because the details container holds the pod images slider... not ideal but whatever.
                Response.StatusCode = 400;

                item.Images = UnitOfWork.GetRepository<Page_Image>().GetAll().Where(p => p.PageID == item.PageID).ToList();

                return PartialView("PageDetailsPartial", item);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdatePagePodImages(int PageID, string Images, byte TemplateImageID)
        {
            try
            {
                UnitOfWork.Context.Page_Image_Merge(PageID, Images, TemplateImageID);
            }
            catch(Exception e)
            {
                Response.StatusDescription = e.Message;
                //log e
            }

            //JavaScriptSerializer ser = new JavaScriptSerializer();
            //List<SortDatum> ImagesList = ser.Deserialize<List<SortDatum>>(Images);



            //ViewBag.IsUserShuffling = false;

            //List<Page_Image> Current = UnitOfWork.GetRepository<Page_Image>().GetAll().Where(p => p.PageID == PageID).ToList();
            //List<long> CurrentImages = new List<long>();
            //List<long> InputImages = new List<long>();

            //foreach (Page_Image img in Current)
            //{
            //    CurrentImages.Add(img.ImageID);
            //}

            //foreach (SortDatum d in ImagesList)
            //{
            //    InputImages.Add(d.ID);
            //}
            //List<long> Removals = CurrentImages.Except(InputImages).ToList();

            //foreach (long id in Removals)
            //{
            //    Page_Image remove = UnitOfWork.GetRepository<Page_Image>().GetAll().Where(p => p.PageID == PageID && p.ImageID == id).Single();
            //    UnitOfWork.GetRepository<Page_Image>().Delete(remove);
            //}
            //UnitOfWork.Save();


            //foreach (SortDatum datum in ImagesList)
            //{
            //    Page_Image ppi = UnitOfWork.GetRepository<Page_Image>().GetAll().Where(p => p.PageID == PageID && p.ImageID == datum.ID && p.TemplateImageID == TemplateImageID).SingleOrDefault();
            //    if (ppi == null)
            //    {
            //        ppi = new Page_Image() { PageID = PageID, TemplateImageID = TemplateImageID, ImageID = datum.ID, SortOrder = (byte)datum.SortOrder };
            //        UnitOfWork.GetRepository<Page_Image>().Add(ppi);
            //    }
            //    else
            //    {
            //        ppi.SortOrder = (byte)datum.SortOrder;
            //    }
            //}
            //UnitOfWork.Save();

            var Model = UnitOfWork.GetRepository<Page_Image>().GetAll().Where(p => p.PageID == PageID && p.TemplateImageID == 1).OrderBy(p => p.SortOrder).ToList();

            return PartialView("PagePodImages", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdatePageCountryRestrictions(int PageID, string Countries)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> CountryRestrictions = ser.Deserialize<List<SortDatum>>(Countries);

            List<Page_Country> Current = UnitOfWork.GetRepository<Page_Country>().GetAll().Where(p => p.PageID == PageID).ToList();
            List<SortDatum> CurrentCountries = new List<SortDatum>();
            foreach (Page_Country country in Current)
            {
                CurrentCountries.Add(new SortDatum() { ID = country.CountryID, SortOrder = 0 });
            }
            List<SortDatum> Removals = CurrentCountries.Except(CountryRestrictions).ToList();

            foreach (SortDatum datum in Removals)
            {
                Page_Country remove = UnitOfWork.GetRepository<Page_Country>().GetAll().Where(p => p.PageID == PageID && p.CountryID == datum.ID).Single();
                UnitOfWork.GetRepository<Page_Country>().Delete(remove);
            }
            UnitOfWork.Save();

            //could probably do deletion from here too: if ProductPage_Country table holds record which isn't in Countries passed to this function, then delete those.

            foreach (SortDatum datum in CountryRestrictions)
            {
                Page_Country bc = UnitOfWork.GetRepository<Page_Country>().GetAll().Where(p => p.PageID == PageID && p.CountryID == datum.ID).SingleOrDefault();

                if (bc == null)
                {
                    bc = new Page_Country() { PageID = PageID, CountryID = (short)datum.ID };
                    UnitOfWork.GetRepository<Page_Country>().Add(bc);
                }
            }

            UnitOfWork.Save();

            List<short> Linker = UnitOfWork.GetRepository<Page_Country>().GetAll().Where(p => p.PageID == PageID).Select(p => p.CountryID).ToList();
            List<Country> Model = UnitOfWork.GetRepository<Country>().GetAll().Where(p => Linker.Contains(p.CountryID)).ToList();
            return PartialView("PageCountryRestrictionsPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ReloadSectionsForShuffle(int PageID, int? parentpageid, byte? sitesectionid)
        {
            PageViewModelBuilder PageBuilder = new PageViewModelBuilder(UnitOfWork);

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

            PageComposeViewModel PCVM = PageBuilder.Build(PageID, countryID, languageId);

            PCVM.PageSearchHeader = new PageSearchHeader() { PageID = PageID, ParentPageID = parentpageid, SiteSectionID = sitesectionid };

            ViewBag.IsUserShuffling = true;

            return PartialView("PageSectionsPartial", PCVM.PageSections);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ReloadSectionsForEditing(int PageID, int? parentpageid, byte? sitesectionid)
        {
            PageViewModelBuilder PageBuilder = new PageViewModelBuilder(UnitOfWork);

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

            PageComposeViewModel PCVM = PageBuilder.Build(PageID, countryID, languageId);

            PCVM.PageSearchHeader = new PageSearchHeader() { PageID = PageID, ParentPageID = parentpageid, SiteSectionID = sitesectionid };

            ViewBag.IsUserShuffling = false;

            return PartialView("PageSectionsPartial", PCVM.PageSections);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdatePageSectionSortOrder(int PageID, string PageSections)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> PageSectionsList = ser.Deserialize<List<SortDatum>>(PageSections);
            
            //List<PageSection> Current = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageID == PageID).ToList();

            foreach (SortDatum datum in PageSectionsList)
            {
                PageSection ps = UnitOfWork.GetRepository<PageSection>().GetAll().Where(p => p.PageID == PageID && p.PageSectionID == datum.ID).Single();

                ps.SortOrder = Convert.ToByte(datum.SortOrder);
            }

            UnitOfWork.Save();

            PageViewModelBuilder PageBuilder = new PageViewModelBuilder(UnitOfWork);

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

            PageComposeViewModel PCVM = PageBuilder.Build(PageID, countryID, languageId);

            PCVM.PageSearchHeader = new PageSearchHeader() { PageID = PageID };

            ViewBag.IsUserShuffling = true;

            return PartialView("PageSectionsPartial", PCVM.PageSections);
        }

        [AllowAnonymous]
        public ActionResult CompetitionDropDownList(byte? SelectedCompetitionID, long FormID)
        {
            ViewBag.SelectedCompetitionID = SelectedCompetitionID;
            ViewBag.FormID = FormID;

            var Model = UnitOfWork.GetRepository<Competition>().GetAll().ToList();
            return PartialView("~/Views/Shared/CompetitionDropDownPartial.cshtml", Model);
        }

        public ActionResult PageSectionProductPageSortBestSeller(int PageID, long PageSectionID, byte PriceListOverride)
        {
            try
            {
                ViewBag.IsUserShuffling = false;

                if (Session["GlobalPriceListID"] == null)
                    Session["GlobalPriceListID"] = 1;

                byte? priceListID = 1;
                string currencySymbol = "£";
                if (Session["GlobalPriceListID"] != null)
                {
                    priceListID = Convert.ToByte(Session["GlobalPriceListID"]);
                    if (priceListID == 0)
                        priceListID = 1;
                    var currencyID = UnitOfWork.GetRepository<PriceList>().GetAll().Where(p => p.PriceListID == priceListID).Single().CurrencyID;
                    currencySymbol = UnitOfWork.GetRepository<Currency>().GetAll().Where(p => p.CurrencyID == currencyID).Select(p => p.Symbol).Single();
                }

                UnitOfWork.Context.PageSection_ProductPage_Sort_Bestseller(PageSectionID, Convert.ToByte(Session["GlobalPriceListID"] ?? PriceListOverride));

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

                var Model = Builder.Build(PageID, countryID, languageId);

                return PartialView("PageSectionsPartial", Model.PageSections);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        public ActionResult PageSectionProductPageSortStock(int PageID, long PageSectionID)
        {
            try
            {
                ViewBag.IsUserShuffling = false;

                UnitOfWork.Context.PageSection_ProductPage_Sort_Stock(PageSectionID);

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

                var Model = Builder.Build(PageID, countryID, languageId);

                return PartialView("PageSectionsPartial", Model.PageSections);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        public ActionResult PageSectionProductPageSortClearance(int PageID, long PageSectionID)
        {
            try
            {
                ViewBag.IsUserShuffling = false;

                UnitOfWork.Context.PageSection_ProductPage_Sort_Clearance(PageSectionID);

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

                var Model = Builder.Build(PageID, countryID, languageId);

                return PartialView("PageSectionsPartial", Model.PageSections);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        public ActionResult PageSectionProductPageSortColour(int PageID, long PageSectionID, bool Direction)
        {
            try
            {
                ViewBag.IsUserShuffling = false;

                UnitOfWork.Context.PageSection_ProductPage_Sort_Colour(PageSectionID, Direction);

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

                var Model = Builder.Build(PageID, countryID, languageId);

                return PartialView("PageSectionsPartial", Model.PageSections);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        public ActionResult PageSectionProductPageSortLaunch(int PageID, long PageSectionID)
        {
            try
            {
                ViewBag.IsUserShuffling = false;

                UnitOfWork.Context.PageSection_ProductPage_Sort_Launch(PageSectionID);

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

                var Model = Builder.Build(PageID, countryID, languageId);

                return PartialView("PageSectionsPartial", Model.PageSections);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        public ActionResult PageSectionClonePageIDComboBoxPartial()
        {
            return PartialView();
        }

        public ActionResult CanonicalPageIDComboBoxPartial(int? CanonicalPageID)
        {
            ViewBag.CanonicalPageID = CanonicalPageID;

            return PartialView();
        }

        public ActionResult PageSection_ProductTypeIDComboBoxPartial(long PageSectionID, byte? ProductTypeID)
        {
            var model = new BasePageSection();

            model.PageSectionID = PageSectionID;
            model.ProductTypeID = ProductTypeID;

            return PartialView(model);
        }

        public ActionResult ClonePageSection(int PageID, int? TargetPageID, long PageSectionID, DateTime? OnDate, bool CopyContents)
        {
            try
            {
                ViewBag.IsUserShuffling = false;

                UnitOfWork.Context.PageSection_Copy(PageSectionID, OnDate, CopyContents, TargetPageID);

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

                var Model = Builder.Build(PageID, countryID, languageId);

                return PartialView("PageSectionsPartial", Model.PageSections);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        public ActionResult HtmlEditorTest()
        {
            return View();
        }

        public ActionResult HtmlEditorTestPartial()
        {
            string Model = "<h1>This is a test.</h1>";

            return PartialView("_HtmlEditorTestPartial", Model);
        }

        public ActionResult RedirectPageIDComboBoxPartial(long? pageId, short formId)
        {
            ViewBag.PageID = pageId;
            ViewBag.FormID = formId;
            return PartialView("RedirectPageIDComboBoxPartial");
        }

        public ActionResult PromotionCodeIDComboBoxPartial(short? promotionCodeId, short formId)
        {
            ViewBag.PromotionCodeID = promotionCodeId;
            ViewBag.FormID = formId;
            return PartialView("PromotionCodeIDComboBoxPartial");
        }


        //@Html.Action("PageComposePageSectionCountryDropDown", "CMS")
        //@Html.Action("PageComposePageSectionTypeDropDown", "CMS")
        //@Html.Action("PageComposePageSectionAudienceDropDown", "CMS")
        //@Html.Action("PageComposePageSectionPlatformDropDown", "CMS")
        //@Html.Action("PageComposePageSectionWebStatusDropDown", "CMS")

        public ActionResult PageSectionsPartial(int pageId, byte? countryLanguageId, byte? pageSectionTypeId, bool? tradeSwitch, byte? platformSwitch, byte? webStatus)
        {
            ViewBag.IsUserShuffling = false;

            PageComposeViewModel PVM = null;

            if (UnitOfWork.GetRepository<Page>().GetAll().Where(p => p.PageID == pageId && p.DeletedDT == null).SingleOrDefault() != null)
            {
                PageViewModelBuilder PageBuilder = new PageViewModelBuilder(UnitOfWork);

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

                PVM = PageBuilder.Build(pageId, countryID, languageId);
            }
            else
            {
                PVM = new PageComposeViewModel();
                ViewData["InvalidPageIDError"] = "Invalid Page ID, or the page is marked as deleted. Deleted pages can be retrieved.";
            }

            if (countryLanguageId != null && countryLanguageId != 0)
            {
                var countryLanguage = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(cl => cl.Country_Language_ID == countryLanguageId).Single();

                PVM.PageSections = PVM.PageSections.Where(p => p.CountryLanguageRestrictions == null ||
                    (p.CountryLanguageRestrictions.Any(q => q.CountryID == countryLanguage.CountryID && q.LanguageID == countryLanguage.LanguageID))).ToList();
            }

            if (pageSectionTypeId != null)
            {
                PVM.PageSections = PVM.PageSections.Where(p => p.PageSectionTypeID == pageSectionTypeId.Value).ToList();
            }

            if (tradeSwitch != null)
            {
                PVM.PageSections = PVM.PageSections.Where(p => p.TradeSwitch == null || p.TradeSwitch == tradeSwitch).ToList();
            }

            if (platformSwitch != null)
            {
                bool? plat = null;

                switch (platformSwitch)
                {
                    case 0:
                        break;
                    case 1:
                        plat = false;
                        break;
                    case 2:
                        plat = true;
                        break;
                }

                PVM.PageSections = PVM.PageSections.Where(p => p.PlatformSwitch == null || p.PlatformSwitch == plat).ToList();
            }


            if (webStatus != null)
            {
                PVM.PageSections = PVM.PageSections.Where(p => UnitOfWork.Context.fnc_Status(p.OnDate, p.OffDate) == webStatus.Value).ToList();
            }

            return PartialView("PageSectionsPartial", PVM.PageSections);
        }

        public ActionResult PageComposePageSectionCountryDropDown()
        {
            return PartialView("~/Views/Shared/SearchFilters/CountryDropDownPartial.cshtml", UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => p.Deleted == false).Select(p => p.Country).Distinct().OrderBy(p => p.Name).ToList());
        }

        public ActionResult PageComposePageSectionTypeDropDown()
        {
            return PartialView("PageSectionTypeDropDownPartial", UnitOfWork.GetRepository<PageSectionType>().GetAll().OrderBy(p => p.Name).ToList());
        }

        public ActionResult PageComposePageSectionAudienceDropDown()
        {
            return PartialView("AudienceDropDownPartial");
        }

        public ActionResult PageComposePageSectionPlatformDropDown()
        {
            return PartialView("PlatformDropDownPartial");
        }

        public ActionResult PageComposePageSectionWebStatusDropDown()
        {
            return PartialView("~/Views/Shared/SearchFilters/WebStatusDropDownPartial.cshtml", UnitOfWork.GetRepository<Status>().GetAll().OrderBy(p => p.Name).ToList());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoadPagePodAdvancedSettings(PassPagePodFetch objFetch)
        {
            var viewModel = new PagePodAdvancedSettingsViewModel(objFetch);

            return PartialView("PagePodAdvancedSettingsPartial", viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoadCanonicalPageAttributeSettings(PassPageFetch objFetch)
        {
            var viewModel = new CanonicalPageAttributeSettingsViewModel(objFetch);

            if (viewModel.FilterIndexingEnabled)
            {
                return PartialView("CanonicalPageAttributeSettingsPartial", viewModel);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Gone;
                return Json(new { });
            }
        }
    }

    public class NavigationItem
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public byte SortOrder { get; set; }
    }

    public class NavigationGroup
    {
        public string Name { get; set; }
        public List<NavigationItem> NavItems { get; set; }
        public byte SortOrder { get; set; }
    }
}
