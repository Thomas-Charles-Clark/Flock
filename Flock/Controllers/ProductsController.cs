using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using DevExpress.Web.Mvc;
using Flock.Classes;
using System.Data.Linq.SqlClient;
using System.Web.Script.Serialization;
//using Flock.Classes.Auth;
using System.Data.SqlClient;
using DevExpress.XtraPrinting;


namespace Flock.Controllers
{
    public class ProductsController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if(!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return RedirectToAction("overview");
        }

        public ActionResult Overview()
        {
            ProductOverviewViewModelBuilder Builder = new ProductOverviewViewModelBuilder(UnitOfWork);
            var Model = Builder.Build(null);
            return View(Model);
        }

        public ActionResult ProductOverviewCollectionDropDownPartial()
        {
            List<Collection> Model = UnitOfWork.GetRepository<Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        public ActionResult ProductOverviewPatternDropDownPartial()
        {
            List<Pattern> Model = UnitOfWork.GetRepository<Pattern>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PatternDropDownPartial.cshtml", Model);
        }

        public ActionResult ProductOverviewColourDropDownPartial()
        {
            List<Colour> Model = UnitOfWork.GetRepository<Colour>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }

        public ActionResult ProductOverviewMarketDropDownPartial()
        {
            List<PriceList> Model = UnitOfWork.GetRepository<PriceList>().GetAll().OrderBy(p => p.PriceListID).ToList();
            return PartialView("~/Views/Shared/SearchFilters/MarketDropDownPartial.cshtml", Model);

        }

        //[CustomAuthorize]
        public ActionResult Reviews()
        {
            ProductReviewsViewModelBuilder Builder = new ProductReviewsViewModelBuilder(UnitOfWork);
            ProductReviewsViewModel Model = Builder.Build(null);
            return View(Model);
        }

        public ActionResult ProductOverviewGridDetailPartial(long ProductID)
        {
            ProductDirectoryGridDetailViewModelBuilder Builder = new ProductDirectoryGridDetailViewModelBuilder(UnitOfWork);

            ProductDirectoryGridDetailViewModel Model = Builder.Build(ProductID);

            return View(Model);
        }

        [HttpPost]
        //[CustomAuthorize(Roles = "Admin")]
        public ActionResult ProductOverviewGridViewCustomCommand(string commandType, string keyString, string priceListKeyString, string CollectionIDs, string PatternIDs, string ColourIDs, string MarketIDs)
        {
            List<int> keys = keyString.Split(',').ToList().ConvertAll(d => Convert.ToInt32(d));

            switch (commandType)
            {
                case "Enable":
                    List<Product> toEnable = UnitOfWork.GetRepository<Product>().GetAll().Where(p => keys.Contains(p.ProductID)).ToList();
                    for (int i = 0; i < toEnable.Count; i++)
                    {
                        Product p = toEnable[i];
                        p.Enabled = true;
                        UnitOfWork.Save();
                    }
                    break;

                case "Disable":
                    List<Product> toDisable = UnitOfWork.GetRepository<Product>().GetAll().Where(p => keys.Contains(p.ProductID)).ToList();
                    for (int i = 0; i < toDisable.Count; i++)
                    {
                        Product p = toDisable[i];
                        p.Enabled = false;
                        UnitOfWork.Save();
                    }
                    break;
                case "Delete":
                    List<Product> toDelete = UnitOfWork.GetRepository<Product>().GetAll().Where(p => keys.Contains(p.ProductID)).ToList();
                    for (int i = 0; i < toDelete.Count; i++)
                    {
                        Product p = toDelete[i];
                        p.DeletedDT = DateTime.Now;
                        UnitOfWork.Save();
                    }
                    break;
                case "AssignMarkets":
                    List<byte> AssignPriceLists = priceListKeyString.Split(',').ToList().ConvertAll(d => Convert.ToByte(d));
                    UnitOfWork.Context.Price_Insert(keys, AssignPriceLists);
                    break;
                case "DeleteMarkets":
                    List<byte> DeletePriceLists = priceListKeyString.Split(',').ToList().ConvertAll(d => Convert.ToByte(d));
                    UnitOfWork.Context.Price_Delete(keys, DeletePriceLists);
                    break;
                default: break;
            }

            short[] Collections = null;

            if (CollectionIDs != null && CollectionIDs != "")
                Collections = CollectionIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToArray();

            byte[] Patterns = null;

            if (PatternIDs != null && PatternIDs != "")
                Patterns = PatternIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            byte[] Colours = null;

            if (ColourIDs != null && ColourIDs != "")
                Colours = ColourIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            byte[] Markets = null;

            if (MarketIDs != null && MarketIDs != "")
                Markets = MarketIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            return PartialView("_ProductOverviewGridViewPartial", new ProductOverviewSettings() { CollectionIDs = Collections, ColourIDs = Colours, PatternIDs = Patterns, MarketIDs = Markets });
        }

        [ValidateInput(false)]
        public ActionResult ProductOverviewGridViewPartial(string CollectionIDs, string PatternIDs, string ColourIDs, string MarketIDs)
        {
            short[] Collections = null;

            if (CollectionIDs != null && CollectionIDs != "")
                Collections = CollectionIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToArray();

            byte[] Patterns = null;

            if (PatternIDs != null && PatternIDs != "")
                Patterns = PatternIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            byte[] Colours = null;

            if (ColourIDs != null && ColourIDs != "")
                Colours = ColourIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            byte[] Markets = null;

            if (MarketIDs != null && MarketIDs != "")
                Markets = MarketIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            return PartialView("_ProductOverviewGridViewPartial", new ProductOverviewSettings() { CollectionIDs = Collections, ColourIDs = Colours, PatternIDs = Patterns, MarketIDs = Markets });
        }

        //[CustomAuthorize]
        public ActionResult Images()
        {
            ProductImagesViewModelBuilder Builder = new ProductImagesViewModelBuilder(UnitOfWork);
            ProductImagesViewModel Model = Builder.Build(null);
            return View(Model);
        }

        [HttpPost]
        public ActionResult RemoveImageFromPerspective(long productID, byte perspectiveID, long imageID)
        {
            Product_Image pi = UnitOfWork.GetRepository<Product_Image>().GetAll().Where(p => p.ProductID == productID && p.ImageID == imageID).Single();

            UnitOfWork.GetRepository<Product_Image>().Delete(pi);

            UnitOfWork.Save();

            return Content("");
        }

        [HttpPost]
        //[CustomAuthorize]
        public ActionResult UpdatePerspectiveImage(int ProductID, byte PerspectiveID, long ImageID)
        {
            //RIGHT.
            // Allow cross-perspective. Prevent dual

            try
            {
                Product_Image PI = UnitOfWork.GetRepository<Product_Image>().GetAll().Where(p => p.ProductID == ProductID && p.PerspectiveID == PerspectiveID).SingleOrDefault();

                if (PI == null)
                {
                    PI = new Product_Image() { ProductID = ProductID, ImageID = ImageID, PerspectiveID = PerspectiveID };
                    UnitOfWork.GetRepository<Product_Image>().Add(PI);
                    UnitOfWork.Save();
                    return Content("");
                }
                else
                {
                    UnitOfWork.GetRepository<Product_Image>().Delete(PI);
                    UnitOfWork.Save();

                    if (ImageID != -1)
                    {
                        PI = new Product_Image() { ProductID = ProductID, ImageID = ImageID, PerspectiveID = PerspectiveID };
                        UnitOfWork.GetRepository<Product_Image>().Add(PI);
                        UnitOfWork.Save();
                    }
                }
                return Content("");
            }
            catch(SqlException se)
            {
                Response.StatusCode = 400;
                if (se.Message.Contains("PRIMARY"))
                    return Content("You may not use the same Image in two Perspectives on the same Product.");
                else if (se.Message.Contains("FOREIGN"))
                    return Content("You may not list the same Image twice in the same Perspective on a Product.");
                else
                    return Content(se.Message);
            }
            catch (Exception e)
            {
                // log e
                Logger.Error(e, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(e.Message);
            }
        }

        [HttpPost]
        public ActionResult AddImageToPerspective(int ProductID, byte PerspectiveID, long ImageID)
        {
            try
            {
                Product_Image PI = UnitOfWork.GetRepository<Product_Image>().GetAll().Where(p => p.ProductID == ProductID && p.PerspectiveID == PerspectiveID).SingleOrDefault();

                if (PI == null)
                {
                    PI = new Product_Image() { ProductID = ProductID, ImageID = ImageID, PerspectiveID = PerspectiveID };
                    UnitOfWork.GetRepository<Product_Image>().Add(PI);
                    UnitOfWork.Save();
                    return Content("");
                }
                else
                {
                    UnitOfWork.GetRepository<Product_Image>().Delete(PI);
                    UnitOfWork.Save();

                    if (ImageID != -1)
                    {
                        PI = new Product_Image() { ProductID = ProductID, ImageID = ImageID, PerspectiveID = PerspectiveID };
                        UnitOfWork.GetRepository<Product_Image>().Add(PI);
                        UnitOfWork.Save();
                    }
                }
                return Content("");
            }
            catch (SqlException se)
            {
                Response.StatusCode = 400;
                if (se.Message.Contains("PRIMARY"))
                    return Content("You may not use the same Image in two Perspectives on the same Product.");
                else if (se.Message.Contains("FOREIGN"))
                    return Content("You may not list the same Image twice in the same Perspective on a Product.");
                else
                    return Content(se.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }
        }


        public ActionResult ProductImagesGridViewExportToXlsx()
        {
            XlsxExportOptionsEx exportOptions = new XlsxExportOptionsEx();
            exportOptions.PageRange = "1";
            exportOptions.ExportMode = XlsxExportMode.SingleFile;
            return GridViewExtension.ExportToXlsx(Flock.Models.ProductImagesGridViewSettings.GetProductImagesGridViewSettings,
                UnitOfWork.GetRepository<vwProductImage>().GetAll(),
                false,
                exportOptions);
            //return GridViewExportDemoHelper.ExportTypes[typeName].Method(GridViewExportDemoHelper.ExportGridViewSettings, NorthwindDataProvider.GetInvoices());
            
            //return RedirectToAction("ProductImagesGridView");
        }

        [HttpPost]
        //[CustomAuthorize]
        public ActionResult ProductImagesGridViewCustomCommand(string commandType, string keyString)
        {
            List<long> keys = keyString.Split(',').ToList().ConvertAll(d => Convert.ToInt64(d));

            switch (commandType)
            {
                case "Enable":
                    List<Product> toEnable = new List<Product>();
                    if (keys.Count > 2000)
                    {
                        List<Product> all = UnitOfWork.GetRepository<Product>().GetAll().ToList();
                        toEnable = all.Where(p => keys.Contains(p.ProductID)).ToList();
                    }
                    else
                    {
                        toEnable = UnitOfWork.GetRepository<Product>().GetAll().Where(p => keys.Contains(p.ProductID)).ToList();
                    }
                    for (int i = 0; i < toEnable.Count; i++)
                    {
                        Product p = toEnable[i];
                        p.Enabled = true;
                        //p.Enabled = true;
                    }
                    UnitOfWork.Save();
                    break;

                case "Disable":
                    List<Product> toDisable = new List<Product>();

                    if (keys.Count > 2000)
                    {
                        List<Product> all = UnitOfWork.GetRepository<Product>().GetAll().ToList();
                        toDisable = all.Where(p => keys.Contains(p.ProductID)).ToList();
                    }
                    else
                    {
                        toDisable = UnitOfWork.GetRepository<Product>().GetAll().Where(p => keys.Contains(p.ProductID)).ToList();
                    }

                    for (int i = 0; i < toDisable.Count; i++)
                    {
                        Product p = toDisable[i];
                        p.Enabled = false;
                    }
                    UnitOfWork.Save();
                    break;


                default: break;
            }

            return PartialView("_ProductImagesGridViewPartial");
        }

        //[CustomAuthorize]
        [ValidateInput(false)]
        public ActionResult ProductImagesGridViewPartial(string CollectionIDs, string ColourIDs)
        {
            short[] Collections = null;

            if (CollectionIDs != null && CollectionIDs != "")
                Collections = CollectionIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToArray();

            byte[] Colours = null;

            if (ColourIDs != null && ColourIDs != "")
                Colours = ColourIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            return PartialView("_ProductImagesGridViewPartial", new ProductImagesOverviewSettings { CollectionIDs = Collections, ColourIDs = Colours });
        }

        //[CustomAuthorize]
        public ActionResult ProductImagesGridDetailPartial(long ProductID)
        {
            ProductDirectoryGridDetailViewModelBuilder Builder = new ProductDirectoryGridDetailViewModelBuilder(UnitOfWork);

            ProductDirectoryGridDetailViewModel Model = Builder.Build(ProductID);

            return View(Model);
        }

        //[CustomAuthorize]
        public ActionResult CollectionsListBoxPartial(string FilterText)
        {
            var Model = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => SqlMethods.Like(p.Name, "%" + FilterText + "%")).ToList();
            return PartialView(Model);
        }

        //[CustomAuthorize]
        public ActionResult Prices()
        {
            ProductPricesViewModelBuilder Builder = new ProductPricesViewModelBuilder(UnitOfWork);
            ProductPricesViewModel Model = Builder.Build(null);
            Session["ProductPriceOverviewPriceListID"] = 1;
            return View(Model);
        }

        //[CustomAuthorize]
        public ActionResult PriceListsListBoxPartial()
        {
            var Model = UnitOfWork.GetRepository<PriceList>().GetAll().ToList();
            return PartialView(Model);
        }

        //[HttpPost]
        ////[CustomAuthorize]
        //public ActionResult ProductPriceOverviewPriceListBoxSetSessionVariable(byte? ProductPriceListID)
        //{
        //    Session["ProductPriceOverviewPriceListID"] = ProductPriceListID;
        //    return Content("");
        //}

        //[HttpPost]
        ////[CustomAuthorize]
        //public ActionResult ProductPriceOverviewCollectionListBoxSetSessionVariable(short? CollectionID)
        //{
        //    Session["ProductPriceOverviewCollectionID"] = CollectionID;
        //    return Content("");
        //}

        //[HttpPost]
        ////[CustomAuthorize]
        //public ActionResult ProductPriceOverviewColourListBoxSetSessionVariable(short? ColourID)
        //{
        //    Session["ProductPriceOverviewColourID"] = ColourID;
        //    return Content("");
        //}

        [ValidateInput(false)]
        //[CustomAuthorize]
        public ActionResult ProductPricesGridViewPartial(byte? PriceListID, string CollectionIDs, string ColourIDs)
        {
            short[] Collections = null;

            if (CollectionIDs != null && CollectionIDs != "")
                Collections = CollectionIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToArray();

            byte[] Colours = null;

            if (ColourIDs != null && ColourIDs != "")
                Colours = ColourIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            return PartialView("_ProductPricesGridViewPartial", new PriceOverviewSettings() { PriceListID = PriceListID, CollectionIDs = Collections, ColourIDs = Colours });
        }

        //[CustomAuthorize]
        public ActionResult ProductPricesGridDetailPartial(long ProductID) //", "Products", new { ProductID = c.KeyValue }));
        {

            return View();
        }

        [HttpPost] 
        //[CustomAuthorize]
        [ValidateInput(false)]
        public ActionResult ProductPricesGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.Models.vwProductPrice item, byte PriceListID, string CollectionIDs, string ColourIDs)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var priceItem = UnitOfWork.GetRepository<Price>().GetAll().Where(p => p.ProductID == item.ProductID && p.PriceListID == PriceListID).SingleOrDefault();
                    if (priceItem != null)
                    {
                        priceItem.PriceEx = (decimal)item.PriceEx;
                        priceItem.PriceInc = (decimal)item.PriceInc;
                        UnitOfWork.Save();
                    }
                    else
                    {
                        ViewData["EditError"] = "Failure to update. Please contact administrator.";
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            short[] Collections = null;

            if (CollectionIDs != null && CollectionIDs != "")
                Collections = CollectionIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToArray();

            byte[] Colours = null;

            if (ColourIDs != null && ColourIDs != "")
                Colours = ColourIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            return PartialView("_ProductPricesGridViewPartial", new PriceOverviewSettings() { PriceListID = PriceListID, CollectionIDs = Collections, ColourIDs = Colours });
        }

        [HttpPost]
        //[CustomAuthorize]
        public ActionResult ProductPricesGridViewCustomCommand(string commandType, string keyString, byte PriceListID, string CollectionIDs, string ColourIDs)
        {
            List<long> keys = keyString.Split(',').ToList().ConvertAll(d => Convert.ToInt64(d));

            short[] Collections = null;

            if (CollectionIDs != null && CollectionIDs != "")
                Collections = CollectionIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToArray();

            byte[] Colours = null;

            if (ColourIDs != null && ColourIDs != "")
                Colours = ColourIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            switch (commandType)
            {
                case "MertexSync":

                    DataTable dt = new DataTable();

                    using (SqlConnection connection = new SqlConnection(UnitOfWork.Context.Connection.ConnectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "adm.Price_Merge";
                            command.CommandType = CommandType.StoredProcedure;

                            DataTable table = new DataTable();
                            table.Columns.Add("ID", typeof(long));
                            foreach (long id in keys)
                            {
                                table.Rows.Add(id);
                            }

                            SqlParameter Param_ProductIDs = command.Parameters.AddWithValue("@ProductIDs", table);
                            Param_ProductIDs.SqlDbType = SqlDbType.Structured;
                            Param_ProductIDs.TypeName = "dbo.ProductIDsTableType";

                            SqlParameter Param_PriceListID = command.Parameters.AddWithValue("@PriceListID", PriceListID);
                            Param_PriceListID.SqlDbType = SqlDbType.TinyInt;

                            command.ExecuteNonQuery();
                        }
                    }
                    break;

                default: break;
            }

            //List<vwBannerOverview> Model = UoW.GetRepository<vwBannerOverview>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            return PartialView("_ProductPricesGridViewPartial", new PriceOverviewSettings() { PriceListID = PriceListID, CollectionIDs = Collections, ColourIDs = Colours });
        }

        [HttpPost]
        //[CustomAuthorize]
        public ActionResult SynchroniseAllProductPricesWithMertex(byte PriceListID)
        {
            try
            {
                UnitOfWork.Context.Price_MergeAll(PriceListID);

                return Content("");
            }
            catch (Exception e)
            {
                //log e
                Response.StatusCode = 400;
                return Content(e.Message);
            }
        }

        [HttpPost]
        public ActionResult ToggleProductPages(int ProductPageID, int ProductID)
        {
            UnitOfWork.Context.ProductPage_PublishingDate_Toggle(ProductPageID, ProductID);

            return Content("Success");
        }

        //[CustomAuthorize]
        public ActionResult Stock()
        {
            ProductStockViewModelBuilder Builder = new ProductStockViewModelBuilder(UnitOfWork);
            ProductStockViewModel Model = Builder.Build(null);
            return View(Model);
        }

        //[CustomAuthorize]
        public ActionResult StockOverviewCollectionDropDownPartial()
        {
            List<Collection> Model = UnitOfWork.GetRepository<Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult StockOverviewColourDropDownPartial()
        {
            List<Colour> Model = UnitOfWork.GetRepository<Colour>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        [ValidateInput(false)]
        public ActionResult ProductStockGridViewPartial()
        {
            return PartialView("_ProductStockGridViewPartial");
        }

        [HttpPost]
        //[CustomAuthorize]
        public ActionResult ProductStockGridViewCustomCommand(string commandType, string keyString)
        {
            try
            {
                List<long> keys = keyString.Split(',').ToList().ConvertAll(d => Convert.ToInt64(d));

                switch (commandType)
                {
                    case "Enable":
                        List<Product> toEnable = new List<Product>();
                        if (keys.Count > 2000)
                        {
                            List<Product> all = UnitOfWork.GetRepository<Product>().GetAll().ToList();
                            toEnable = all.Where(p => keys.Contains(p.ProductID)).ToList();
                        }
                        else
                        {
                            toEnable = UnitOfWork.GetRepository<Product>().GetAll().Where(p => keys.Contains(p.ProductID)).ToList();
                        }
                        for (int i = 0; i < toEnable.Count; i++)
                        {
                            Product p = toEnable[i];
                            p.Enabled = true;
                            //p.Enabled = true;
                        }
                        UnitOfWork.Save();
                        break;

                    case "Disable":
                        List<Product> toDisable = new List<Product>();

                        if (keys.Count > 2000)
                        {
                            List<Product> all = UnitOfWork.GetRepository<Product>().GetAll().ToList();
                            toDisable = all.Where(p => keys.Contains(p.ProductID)).ToList();
                        }
                        else
                        {
                            toDisable = UnitOfWork.GetRepository<Product>().GetAll().Where(p => keys.Contains(p.ProductID)).ToList();
                        }

                        for (int i = 0; i < toDisable.Count; i++)
                        {
                            Product p = toDisable[i];
                            p.Enabled = false;
                        }
                        UnitOfWork.Save();
                        break;


                    default: break;
                }
            }
            catch(Exception ex)
            {
                ViewData["EditError"] = ex.ToString();
            }
            
            return PartialView("_ProductStockGridViewPartial");
        }

        //[CustomAuthorize]
        public ActionResult ProductStockGridDetailPartial(long ProductID)//", "Products", new { ProductID = c.KeyValue 
        {
            return View(ProductID);
        }

        //[CustomAuthorize]
        [ValidateInput(false)]
        public ActionResult ProductStockDetailGridViewPartial(long ProductID)
        {
            var prod = UnitOfWork.GetRepository<vwProductStock>().GetAll().Where(p => p.ProductID == ProductID).Single();
            var model = UnitOfWork.GetRepository<vwProductStock>().GetAll().Where(p => p.ProductPageID == prod.ProductPageID && p.ProductID != ProductID).ToList();
            return PartialView("_ProductStockDetailGridViewPartial", model);
        }





        //[CustomAuthorize]
        public ActionResult ProductSearch(byte? ProductTypeID, byte? BrandID, short? LaunchID, int? DesignID, byte? PatternID, char? Status,
            byte? ProductPageAssociation, byte? StatusID, byte? StockStatusID, short? CollectionID, byte? ColourID, string SearchString, byte? PriceListID)
        {
            if (SearchString == "" || SearchString == "Name, Code or ID")
                SearchString = null;
            System.Data.Linq.ISingleResult<Product_SearchResult> Results = null;

            List<Product_SearchResult> Model = new List<Product_SearchResult>();
            bool? PPA = null;
            if (ProductPageAssociation.HasValue)
                PPA = Convert.ToBoolean(ProductPageAssociation);
            Results = UnitOfWork.Context.Product_Search(ProductTypeID, BrandID, LaunchID, DesignID, Status, PPA, StatusID, StockStatusID, CollectionID, ColourID, PatternID, PriceListID, SearchString, null); // clearance...

                foreach (Product_SearchResult result in Results)
                {
                    Model.Add(result);
                }
            
            //Model = Model.OrderBy(p => p.Name).ToList();

            return PartialView("ProductSearchDataViewPartial", Model);
        }


        //[CustomAuthorize]
        public ActionResult ProductSearchSmall(byte? ProductTypeID, byte? BrandID, short? LaunchID, short? DesignID, byte? PatternID, char? Status,
            byte? ProductPageAssociation, byte? StatusID, byte? StockStatusID, short? CollectionID, byte? ColourID, string SearchString, byte? PriceListID)
        {
            if (SearchString == "" || SearchString == "Name, Code or ID")
                SearchString = null;
            System.Data.Linq.ISingleResult<Product_SearchResult> Results = null;

            List<Product_SearchResult> Model = new List<Product_SearchResult>();
            bool? PPA = null;
            if (ProductPageAssociation.HasValue)
                PPA = Convert.ToBoolean(ProductPageAssociation);
            Results = UnitOfWork.Context.Product_Search(ProductTypeID, BrandID, LaunchID, DesignID, Status, PPA, StatusID, StockStatusID, CollectionID, ColourID, PatternID, PriceListID, SearchString, null); // clearance

            foreach (Product_SearchResult result in Results)
            {
                Model.Add(result);
            }

            //Model = Model.OrderBy(p => p.Name).ToList();

            return PartialView("ProductSearchDataViewPartialSmall", Model);
        }

        public ActionResult ProductSearchAssociationDropDownPartial()
        {
            return PartialView();
        }

        public ActionResult ProductSearchPriceListDropDownPartial()
        {
            var Model = UnitOfWork.GetRepository<PriceList>().GetAll().OrderBy(p => p.PriceListID).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PriceListDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult ProductSearchProductTypeDropDownPartial()
        {
            List<ProductType> Model = UnitOfWork.GetRepository<ProductType>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult ProductSearchBrandDropDownPartial()
        {
            List<Brand> Model = UnitOfWork.GetRepository<Brand>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult ProductSearchLaunchDropDownPartial()
        {
            List<Launch> Model = UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult ProductSearchCollectionDropDownPartial()
        {
            List<Flock.Models.Collection> Model = UnitOfWork.GetRepository<Flock.Models.Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult ProductSearchNameOrIDTextPartial()
        {
            return PartialView("~/Views/Shared/SearchFilters/NameOrIDTextBoxPartial.cshtml");
        }

        //[CustomAuthorize]
        public ActionResult ProductSearchPatternDropDownPartial()
        {
            List<Flock.Models.Pattern> Model = UnitOfWork.GetRepository<Flock.Models.Pattern>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PatternDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult ProductSearchColourDropDownPartial()
        {
            List<Flock.Models.Colour> Model = UnitOfWork.GetRepository<Flock.Models.Colour>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult ProductSearchStatusDropDownPartial()
        {
            return PartialView("~/Views/Shared/SearchFilters/StatusDropDownPartial.cshtml");
        }

        //[CustomAuthorize]
        public ActionResult ProductSearchWebStatusDropDownPartial()
        {
            List<Flock.Models.Status> Model = UnitOfWork.GetRepository<Flock.Models.Status>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/WebStatusDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult ProductSearchStockStatusDropDownPartial()
        {
            List<Flock.Models.StockStatus> Model = UnitOfWork.GetRepository<Flock.Models.StockStatus>().GetAll().OrderBy(p => p.StockStatus1).ToList();
            return PartialView("~/Views/Shared/SearchFilters/StockStatusDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult ShopAvailabilityDropDownPartial()
        {
            return PartialView("~/Views/Shared/SearchFilters/ShopAvailabilityDropDownPartial.cshtml");
        }

        //[CustomAuthorize]
        public ActionResult ProductImagesOverviewCollectionDropDownPartial()
        {
            List<Collection> Model = UnitOfWork.GetRepository<Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult ProductImagesOverviewColourDropDownPartial()
        {
            List<Colour> Model = UnitOfWork.GetRepository<Colour>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult PricesOverviewPriceListDropDownPartial()
        {
            List<PriceList> Model = UnitOfWork.GetRepository<PriceList>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PriceListDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult PricesOverviewCollectionDropDownPartial()
        {
            List<Collection> Model = UnitOfWork.GetRepository<Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        //[CustomAuthorize]
        public ActionResult PricesOverviewColourDropDownPartial()
        {
            List<Colour> Model = UnitOfWork.GetRepository<Colour>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }


        /////////////////////////
        /// Dimensions
        /////////////////////////
    
        public ActionResult Dimensions()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ProductDimensionsGridViewPartial(string CollectionIDs, string PatternIDs, string ColourIDs, string MarketIDs)
        {
            short[] Collections = null;

            if (CollectionIDs != null && CollectionIDs != "")
                Collections = CollectionIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToArray();

            byte[] Patterns = null;

            if (PatternIDs != null && PatternIDs != "")
                Patterns = PatternIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            byte[] Colours = null;

            if (ColourIDs != null && ColourIDs != "")
                Colours = ColourIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            byte[] Markets = null;

            if (MarketIDs != null && MarketIDs != "")
                Markets = MarketIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            return PartialView("_ProductDimensionsGridViewPartial", new ProductOverviewSettings() { CollectionIDs = Collections, ColourIDs = Colours, PatternIDs = Patterns, MarketIDs = Markets });
        }

        [ValidateInput(false)]
        public ActionResult ProductDimensionsGridBatchUpdate(MVCxGridViewBatchUpdateValues<vwProductText, int> updateValues,
            string CollectionIDs, string PatternIDs, string ColourIDs, string MarketIDs)
        {
            foreach (var product in updateValues.Update)
            {
                if (updateValues.IsValid(product))
                {
                    Product p = UnitOfWork.GetRepository<Product>().GetAll().Where(q => q.ProductID == product.ProductID).Single();
                    if(product.BasketName != null)
                        p.BasketName = product.BasketName;
                    //if(product.SizeMetric != null)
                    p.SizeMetric = product.SizeMetric;
                    //if(product.SizeImperial != null)
                    p.SizeImperial = product.SizeImperial;
                    
                    p.OrderFormGroupID = product.OrderFormGroupID;
                    
                    if (product.OrderFormName != null)
                        p.OrderFormName = product.OrderFormName;
                }

                UnitOfWork.Save();
            }

            short[] Collections = null;

            if (CollectionIDs != null && CollectionIDs != "")
                Collections = CollectionIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToArray();

            byte[] Patterns = null;

            if (PatternIDs != null && PatternIDs != "")
                Patterns = PatternIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            byte[] Colours = null;

            if (ColourIDs != null && ColourIDs != "")
                Colours = ColourIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            byte[] Markets = null;

            if (MarketIDs != null && MarketIDs != "")
                Markets = MarketIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();


            return PartialView("_ProductDimensionsGridViewPartial", new ProductOverviewSettings() { CollectionIDs = Collections, ColourIDs = Colours, PatternIDs = Patterns, MarketIDs = Markets });
        }

        public ActionResult ProductDimensionsCollectionDropDownPartial()
        {
            List<Collection> Model = UnitOfWork.GetRepository<Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        public ActionResult ProductDimensionsPatternDropDownPartial()
        {
            List<Pattern> Model = UnitOfWork.GetRepository<Pattern>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PatternDropDownPartial.cshtml", Model);
        }

        public ActionResult ProductDimensionsColourDropDownPartial()
        {
            List<Colour> Model = UnitOfWork.GetRepository<Colour>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }

        public ActionResult ProductDimensionsMarketDropDownPartial()
        {
            List<PriceList> Model = UnitOfWork.GetRepository<PriceList>().GetAll().OrderBy(p => p.PriceListID).ToList();
            return PartialView("~/Views/Shared/SearchFilters/MarketDropDownPartial.cshtml", Model);
        }

        [HttpPost]
        public ActionResult Undelete(String Code)
        {
            try
            {
                var product = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.Code == Code).Single();

                product.DeletedDT = null;

                UnitOfWork.Save();

                return Content("ok");
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.ToString());
            }
        }
    }
}

namespace Flock.Models 
{
    public struct OrderFormGroupCast
    {
        public string Name { get; set; }
        public short? OrderFormGroupID { get; set; }
    };
}