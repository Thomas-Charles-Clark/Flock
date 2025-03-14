using DevExpress.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using System;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
//using Flock.Classes.Auth;
using System.Data.Linq;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using DevExpress.CodeParser;

namespace Flock.Controllers
{
    public partial class ProductPagesController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        //[CustomAuthorize(Roles="Admin, Peon")]
        public ActionResult Index()
        {
            return RedirectToAction("Overview");
        }

        //[CustomAuthorize(Roles = "Admin, Peon")]
        public ActionResult Overview()
        {
            return PartialView("ProductPagesOverview");
        }

        /// <summary>
        /// Recieves upload of file, 
        /// </summary>
        /// <param name="file">posted file</param>
        /// <param name="BannerID">id</param>
        /// <param name="source">POST upload / ImageLibrary drag drop: 0 is POST, 1 is ImageLibrary</param>
        /// <returns>Json describing result of upload attempt</returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult FurnitureDimensionsImage(HttpPostedFileBase file, int ProductPageID)
        {
            List<string> allowedExtensions = new List<string>() { ".jpeg", ".jpg" };

            if (file == null)
            {
                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();
                jsresult.files[0].name = "ERROR";
                jsresult.files[0].size = 0;
                jsresult.files[0].url = "/test/";
                jsresult.files[0].thumbnailUrl = "/thumbtest/";
                jsresult.files[0].deleteUrl = "/deletetest";
                jsresult.files[0].deleteType = "DELETE";
                jsresult.files[0].error = "No file provided";
                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
            else if (file != null ? !allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) : false)
            {
                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();
                jsresult.files[0].name = "error";
                jsresult.files[0].size = 0;
                jsresult.files[0].url = "";
                jsresult.files[0].thumbnailUrl = "";
                jsresult.files[0].deleteUrl = "";
                jsresult.files[0].deleteType = "DELETE";
                jsresult.files[0].error = "Invalid file type.";
                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var guid = Guid.NewGuid().ToString();

                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\FurnitureImages\\Dimensions\\"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\FurnitureImages\\Dimensions\\");

                using (var fileStream = System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\FurnitureImages\\Dimensions\\" + guid.ToString() + ".jpeg"))
                {
                    file.InputStream.CopyTo(fileStream);
                }

                ProductPage ppi = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID).Single();

                ppi.FurnitureDimensions = guid;

                UnitOfWork.Save();

                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();

                if (file != null)
                {
                    jsresult.files[0].name = file.FileName;
                    jsresult.files[0].size = file.InputStream.Length;
                }
                else
                {
                    jsresult.files[0].name = "test";
                    jsresult.files[0].size = 0;
                }

                jsresult.files[0].url = "/FurnitureImages/Dimensions/" + guid + ".jpeg";
                jsresult.files[0].thumbnailUrl = "/FurnitureImages/Dimensions/" + guid + ".jpeg";
                jsresult.files[0].deleteUrl = "";
                jsresult.files[0].deleteType = "";

                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Recieves upload of file, 
        /// </summary>
        /// <param name="file">posted file</param>
        /// <param name="BannerID">id</param>
        /// <param name="source">POST upload / ImageLibrary drag drop: 0 is POST, 1 is ImageLibrary</param>
        /// <returns>Json describing result of upload attempt</returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult FurniturePDF(HttpPostedFileBase file, int ProductPageID)
        {
            List<string> allowedExtensions = new List<string>() { ".pdf" };

            if (file == null)
            {
                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();
                jsresult.files[0].name = "ERROR";
                jsresult.files[0].size = 0;
                jsresult.files[0].url = "/test/";
                jsresult.files[0].thumbnailUrl = "/thumbtest/";
                jsresult.files[0].deleteUrl = "/deletetest";
                jsresult.files[0].deleteType = "DELETE";
                jsresult.files[0].error = "No file provided";
                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
            else if (file != null ? !allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) : false)
            {
                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();
                jsresult.files[0].name = "error";
                jsresult.files[0].size = 0;
                jsresult.files[0].url = "";
                jsresult.files[0].thumbnailUrl = "";
                jsresult.files[0].deleteUrl = "";
                jsresult.files[0].deleteType = "DELETE";
                jsresult.files[0].error = "Invalid file type.";
                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var fileName = System.Text.RegularExpressions.Regex.Replace(file.FileName, @"[^\w\.@-]", "_", RegexOptions.None, TimeSpan.FromSeconds(1.5));

                var guid = Guid.NewGuid().ToString();

                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\FurniturePDFs\\"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\FurniturePDFs\\");

                using (var fileStream = System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\FurniturePDFs\\" + fileName))
                {
                    file.InputStream.CopyTo(fileStream);
                }

                ProductPage ppi = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID).Single();

                ppi.FurniturePDF = fileName;

                UnitOfWork.Save();

                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();

                if (file != null)
                {
                    jsresult.files[0].name = fileName;
                    jsresult.files[0].size = file.InputStream.Length;
                }
                else
                {
                    jsresult.files[0].name = "test";
                    jsresult.files[0].size = 0;
                }

                jsresult.files[0].url = "/FurniturePDFs/" + fileName;
                jsresult.files[0].thumbnailUrl = "";
                jsresult.files[0].deleteUrl = "";
                jsresult.files[0].deleteType = "";

                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Recieves upload of file, 
        /// </summary>
        /// <param name="file">posted file</param>
        /// <param name="BannerID">id</param>
        /// <param name="source">POST upload / ImageLibrary drag drop: 0 is POST, 1 is ImageLibrary</param>
        /// <returns>Json describing result of upload attempt</returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult FurnitureCutOutImage(HttpPostedFileBase file, int ProductPageID)
        {
            List<string> allowedExtensions = new List<string>() { ".jpeg", ".jpg" };

            if (file == null)
            {
                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();
                jsresult.files[0].name = "ERROR";
                jsresult.files[0].size = 0;
                jsresult.files[0].url = "/test/";
                jsresult.files[0].thumbnailUrl = "/thumbtest/";
                jsresult.files[0].deleteUrl = "/deletetest";
                jsresult.files[0].deleteType = "DELETE";
                jsresult.files[0].error = "No file provided";
                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
            else if (file != null ? !allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) : false)
            {
                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();
                jsresult.files[0].name = "error";
                jsresult.files[0].size = 0;
                jsresult.files[0].url = "";
                jsresult.files[0].thumbnailUrl = "";
                jsresult.files[0].deleteUrl = "";
                jsresult.files[0].deleteType = "DELETE";
                jsresult.files[0].error = "Invalid file type.";
                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var guid = Guid.NewGuid().ToString();

                if(!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\FurnitureImages\\CutOut\\"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\FurnitureImages\\CutOut\\");

                using (var fileStream = System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\FurnitureImages\\CutOut\\" + guid.ToString() + ".jpeg"))
                {
                    file.InputStream.CopyTo(fileStream);
                }

                ProductPage ppi = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID).Single();

                ppi.FurnitureCutOut = guid;

                UnitOfWork.Save();

                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();

                if (file != null)
                {
                    jsresult.files[0].name = file.FileName;
                    jsresult.files[0].size = file.InputStream.Length;
                }
                else
                {
                    jsresult.files[0].name = "test";
                    jsresult.files[0].size = 0;
                }

                jsresult.files[0].url = "/FurnitureImages/CutOut/" + guid + ".jpeg";
                jsresult.files[0].thumbnailUrl = "/FurnitureImages/CutOut/" + guid + ".jpeg";
                jsresult.files[0].deleteUrl = "";
                jsresult.files[0].deleteType = "";

                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
        }


        public class JSResult
        {
            public JSFileResult[] files;
        }

        public class JSFileResult
        {
            public string name { get; set; }
            public long size { get; set; }
            public string url { get; set; }
            public string thumbnailUrl { get; set; }
            public string deleteUrl { get; set; }
            public string deleteType { get; set; }
            public string error { get; set; }
        }

        [HttpPost]
        public ActionResult RemoveDimensionImage(long productPageID)
        {
            ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == productPageID).Single();
            pp.FurnitureDimensions = null;
            UnitOfWork.Save();

            ProductPageComposeViewModelBuilder Builder = new ProductPageComposeViewModelBuilder(UnitOfWork);
            ProductPageComposeViewModel Model = Builder.Build(productPageID);

            return PartialView("ProductPageFurnitureCutOutImagePartial", Model);
        }

        [HttpPost]
        public ActionResult RemoveCutOutImage(long productPageID)
        {
            ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == productPageID).Single();
            pp.FurnitureCutOut = null;
            UnitOfWork.Save();

            ProductPageComposeViewModelBuilder Builder = new ProductPageComposeViewModelBuilder(UnitOfWork);
            ProductPageComposeViewModel Model = Builder.Build(productPageID);

            return PartialView("ProductPageFurnitureCutOutImagePartial", Model);
        }


        [HttpPost]
        public ActionResult ChangeProductPage(long ID)
        {
            ProductPageComposeViewModelBuilder Builder = new ProductPageComposeViewModelBuilder(UnitOfWork);
            ProductPageComposeViewModel Model = Builder.Build(ID);

            return PartialView("ProductPageComposePartial", Model);
        }

        public ActionResult Compose(int? id) // probably needs ProductType or something... 
        {
            ProductPageComposeViewModel Model = new ProductPageComposeViewModel();
            if (id != null)
            {
                if (UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == id && p.DeletedDT == null).SingleOrDefault() != null)
                {
                    ProductPageComposeViewModelBuilder Builder = new ProductPageComposeViewModelBuilder(UnitOfWork);
                    Model = Builder.Build(id);
                }
                else
                {
                    ViewData["InvalidProductPageIDError"] = "Invalid Product Page ID, or the page is marked as deleted. Deleted pages can be retrieved.";
                }
            }
            return View("ProductPageCompose", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageSearch(byte? ProductTypeID, byte? BrandID, short? LaunchID, byte? TemplateID, short? CollectionID, byte? PatternID, byte? StatusID, byte? ColourID, string SearchString)
        {
            if (SearchString == "" || SearchString == "Product Page Name or ID")
                SearchString = null;

            List<ProductPage_SearchResult> Model = new List<ProductPage_SearchResult>();
            System.Data.Linq.ISingleResult<ProductPage_SearchResult> Results = null;

            Results = UnitOfWork.Context.ProductPage_Search(ProductTypeID, BrandID, LaunchID, CollectionID, TemplateID, StatusID, ColourID, PatternID, SearchString);

           foreach (ProductPage_SearchResult result in Results)
            {
                Model.Add(result);
            }

            return PartialView("ProductPageSearchDataViewPartial", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageSearchProductTypeDropDownPartial()
        {
            List<ProductType> Model = UnitOfWork.GetRepository<ProductType>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageSearchBrandDropDownPartial()
        {
            List<Brand> Model = UnitOfWork.GetRepository<Brand>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageSearchLaunchDropDownPartial()
        {
            List<Launch> Model = UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageSearchCollectionDropDownPartial()
        {
            List<Collection> Model = UnitOfWork.GetRepository<Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageSearchPatternDropDownPartial()
        {
            List<Pattern> Model = UnitOfWork.GetRepository<Pattern>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PatternDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageSearchStatusDropDownPartial()
        {
            List<Status> Model = UnitOfWork.GetRepository<Status>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/WebStatusDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageSearchColourDropDownPartial()
        {
            List<Colour> Model = UnitOfWork.GetRepository<Colour>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageSearchTemplateDropDownPartial()
        {
            List<Flock.Models.Template> Model = UnitOfWork.GetRepository<Flock.Models.Template>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/TemplateDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageSearchNameTextBox()
        {
            return PartialView("~/Views/Shared/SearchFilters/NameOrIDTextBoxPartial.cshtml");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateProductPageCountryRestrictions(int ProductPageID, string Countries)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> CountryRestrictions = ser.Deserialize<List<SortDatum>>(Countries);

            List<ProductPage_Country> Current = UnitOfWork.GetRepository<ProductPage_Country>().GetAll().Where(p => p.ProductPageID == ProductPageID).ToList();
            List<SortDatum> CurrentCountries = new List<SortDatum>();
            foreach (ProductPage_Country country in Current)
            {
                CurrentCountries.Add(new SortDatum() { ID = country.CountryID, SortOrder = 0 });
            }
            List<SortDatum> Removals = CurrentCountries.Except(CountryRestrictions).ToList();

            foreach (SortDatum datum in Removals)
            {
                ProductPage_Country remove = UnitOfWork.GetRepository<ProductPage_Country>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.CountryID == datum.ID).Single();
                UnitOfWork.GetRepository<ProductPage_Country>().Delete(remove);
            }
            UnitOfWork.Save();

            //could probably do deletion from here too: if ProductPage_Country table holds record which isn't in Countries passed to this function, then delete those.

            foreach (SortDatum datum in CountryRestrictions)
            {
                ProductPage_Country ppc = UnitOfWork.GetRepository<ProductPage_Country>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.CountryID == datum.ID).SingleOrDefault();

                if (ppc == null)
                {
                    ppc = new ProductPage_Country() { ProductPageID = ProductPageID, CountryID = (short)datum.ID };
                    UnitOfWork.GetRepository<ProductPage_Country>().Add(ppc);
                }
            }

            UnitOfWork.Save();

            List<short> Linker = UnitOfWork.GetRepository<ProductPage_Country>().GetAll().Where(p => p.ProductPageID == ProductPageID).Select(p => p.CountryID).ToList();
            List<Country> Model = UnitOfWork.GetRepository<Country>().GetAll().Where(p => Linker.Contains(p.CountryID)).ToList();
            return PartialView("ProductPageCountryRestrictionPartial", Model);
        }

        [AllowAnonymous]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ProductPageEditorSavePartial(int? ProductPageID, string Html)
        {
            ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID).Single();
            pp.ModifiedDT = DateTime.Now;
            pp.Description = Html;
            UnitOfWork.Save();
            
            UnitOfWork.Context.ReplaceLocalURLPlaceholders_ProductPage(ProductPageID, null, null, null, null, null, false, false);

            return PartialView("ProductPageHTMLEditorPartial", Html);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ProductPageSecondaryProductsPartial(int ProductPageID)
        {
            List<vwProductPage_SecondaryProduct> Model = UnitOfWork.GetRepository<vwProductPage_SecondaryProduct>().GetAll().Where(p => p.ProductPageID == ProductPageID).OrderBy(p => p.SortOrder).ToList();
            return PartialView("ProductPageSecondaryProductsPartial", Model);
        }

        [AllowAnonymous]
        public ActionResult OrderFormGroupManagerPartial()
        {
            List<OrderFormGroup> Model = UnitOfWork.GetRepository<OrderFormGroup>().GetAll().ToList();
            return PartialView("OrderFormGroupManagePartial", Model);
        }

        [HttpPost]
        public ActionResult UpdateProductOrderFormName(int ProductID, string OrderFormName)
        {
            if(OrderFormName == null || OrderFormName == "")
            {
                Response.StatusCode = 400;
                return Content("You cannot have a blank order form name.");
            }
            else {
                Product pr = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.ProductID == ProductID).Single();
                pr.OrderFormName = OrderFormName;
                UnitOfWork.Save();
            }

            return Content("");
        }

        //UpdateProductOrderFormName", "ProductPages")+@"',
        //                                        type: 'POST',
        //                                        traditional: true,
        //                                        data: { 
        //                                            ProductID: '"+i.ProductID+@"',
        //                                            OrderFormName: s.GetValue()
        //                                        },

        [AllowAnonymous]
        public ActionResult PrimaryOrderFormGroupDropDownPartial(int ProductPageID, int ProductID, short? SelectedOrderFormGroupID)
        {
            ViewBag.OrderFormGroupProductPageID = ProductPageID;
            ViewBag.OrderFormGroupProductID = ProductID;
            ViewBag.SelectedOrderFormGroupID = SelectedOrderFormGroupID;
            return PartialView("~/Views/Shared/SearchFilters/OrderFormGroupDropDownPartial.cshtml", UnitOfWork.GetRepository<OrderFormGroup>().GetAll().OrderBy(p => p.Name).ToList());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SetPrimaryProductOrderFormGroup(int ProductPageID, int ProductID, byte? OrderFormGroupID)
        {
            Product pr = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.ProductID == ProductID).Single();
            pr.OrderFormGroupID = OrderFormGroupID;
            UnitOfWork.Save();

            List<vwProductPage_PrimaryProduct> Model = UnitOfWork.GetRepository<vwProductPage_PrimaryProduct>().GetAll().Where(p => p.ProductPageID == ProductPageID).OrderBy(p => p.SortOrder).ToList();
            return PartialView("ProductPagePrimaryProductsPartial", Model);
        }

        [AllowAnonymous]
        public ActionResult SecondaryOrderFormGroupDropDownPartial(int ProductPageID, int ProductID, short? SelectedOrderFormGroupID)
        {
            ViewBag.OrderFormGroupProductPageID = ProductPageID;
            ViewBag.OrderFormGroupProductID = ProductID;
            ViewBag.SelectedOrderFormGroupID = SelectedOrderFormGroupID;
            return PartialView("~/Views/Shared/SearchFilters/OrderFormGroupDropDownPartial.cshtml", UnitOfWork.GetRepository<OrderFormGroup>().GetAll().OrderBy(p => p.Name).ToList());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SetSecondaryProductOrderFormGroup(int ProductPageID, int ProductID, byte? OrderFormGroupID)
        {
            Product_ProductPage ppp = UnitOfWork.GetRepository<Product_ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.ProductID == ProductID).Single();
            ppp.OrderFormGroupID = OrderFormGroupID;
            UnitOfWork.Save();


            List<vwProductPage_SecondaryProduct> Model = UnitOfWork.GetRepository<vwProductPage_SecondaryProduct>().GetAll().Where(p => p.ProductPageID == ProductPageID).OrderBy(p => p.SortOrder).ToList();
            return PartialView("ProductPageSecondaryProductsPartial", Model);
        }

        [HttpPost]
        public ActionResult ProductPagesOverviewGridViewPartialCustomCommand(
            short? CollectionID, 
            byte? ColourID, 
            string CountryIDs, 
            string commandType, 
            string keyString, 
            string dateString, 
            string countryKeys,
            bool? newState, 
            bool? tradeSwitch)
        {
            bool hasError = false;
            string errorString = "";
            List<int> keys = new List<int>();
            if(keyString != null && keyString != "")
                keys = keyString.Split(',').ToList().ConvertAll(d => Convert.ToInt32(d));
            if (countryKeys == "")
                countryKeys = null;
            List<short> countries = new List<short>();
            if (countryKeys != null)
                countries = countryKeys.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(d => Convert.ToInt16(d));

            switch (commandType.ToLower())
            {
                case "setcountryrestrictions":
                try
                {
                    UnitOfWork.Context.ProductPage_Country_BulkUpdate(keys, countries);
                }
                catch(Exception e){
                    ViewData["EditError"] += e.Message + "\r\n\r\n";
                    hasError = true;
                }
                break;
                case "enable":
                    try
                    {
                        DateTime? date = null;
                        if(dateString != "null")
                            date = DateTime.Parse(dateString);
                        List<ProductPage> toEnable = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => keys.Contains(p.ProductPageID)).ToList();
                        foreach (ProductPage c in toEnable)
                        {
                            if (date != null)
                            {
                                if (date < c.OffDate || c.OffDate == null)
                                    c.OnDate = date;
                                else
                                {
                                    errorString += "Product Page " + c.ProductPageID.ToString() + " was not updated because the provided On Date falls after the page's assigned Off Date.\r\n\r\n";
                                    hasError = true;
                                }
                            }
                            else
                            {
                                if(c.OffDate != null)
                                {
                                    errorString += "Product Page " + c.ProductPageID.ToString() + " was not updated because it has an Off Date set.\r\n\r\n";
                                    hasError = true;
                                }
                                else
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
                        DateTime? date = null;
                        if (dateString != "null")
                            date = DateTime.Parse(dateString);
                        List<ProductPage> toEnable = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => keys.Contains(p.ProductPageID)).ToList();
                        foreach (ProductPage c in toEnable)
                        {
                            if (date != null)
                            {
                                if (date >= c.OnDate || c.OnDate == null)
                                    c.OffDate = date;
                                else
                                {
                                    errorString += "Product Page " + c.ProductPageID.ToString() + " was not updated because the provided Off Date preceeds the page's assigned On Date.\r\n\r\n";
                                    hasError = true;
                                }
                            }
                            else
                                c.OffDate = date;
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
                        List<ProductPage> toDelete = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => keys.Contains(p.ProductPageID)).ToList();
                        foreach (ProductPage c in toDelete)
                            c.DeletedDT = DateTime.Now;
                        UnitOfWork.Save();
                    }
                    catch (Exception e)
                    {
                        errorString += e.Message + "\r\n\r\n";
                        hasError = true;
                    }
                    break;
                case "setnewstate":
                    try
                    {
                        List<ProductPage> toSet = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => keys.Contains(p.ProductPageID)).ToList();
                        foreach (ProductPage c in toSet) {
                            c.New = newState.HasValue ? newState.Value : false;
                        }
                        UnitOfWork.Save();
                    }
                    catch(Exception e)
                    {
                        errorString += e.Message + "\r\n\r\n";
                        hasError = true;
                    }
                    break;
                case "copypodtoslideshow":

                    List<ProductPage> copyAB = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => keys.Contains(p.ProductPageID)).ToList();
                    foreach (ProductPage c in copyAB)
                    {
                        var src = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == c.ProductPageID && p.TemplateImageID == 1).ToList();
                        var dest = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == c.ProductPageID && p.TemplateImageID == 2).ToList();
                        UnitOfWork.GetRepository<ProductPage_Image>().DeleteAll(dest);
                        UnitOfWork.Save();

                        foreach (var i in src)
                        {
                            UnitOfWork.GetRepository<ProductPage_Image>().Add(new ProductPage_Image()
                            {
                                ProductPageID = c.ProductPageID,
                                TemplateImageID = 2,
                                ImageID = i.ImageID,
                                AltText = i.AltText,
                                TitleText = i.TitleText,
                                SortOrder = i.SortOrder
                            });
                        }
                        UnitOfWork.Save();
                    }
                    break;
                case "copyslideshowtopod":
                    List<ProductPage> copyBA = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => keys.Contains(p.ProductPageID)).ToList();
                    foreach (ProductPage c in copyBA)
                    {
                        var src2 = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == c.ProductPageID && p.TemplateImageID == 2).ToList();
                        var dest2 = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == c.ProductPageID && p.TemplateImageID == 1).ToList();
                        UnitOfWork.GetRepository<ProductPage_Image>().DeleteAll(dest2);
                        UnitOfWork.Save();

                        foreach (var i in src2)
                        {
                            UnitOfWork.GetRepository<ProductPage_Image>().Add(new ProductPage_Image()
                            {
                                ProductPageID = c.ProductPageID,
                                TemplateImageID = 1,
                                ImageID = i.ImageID,
                                AltText = i.AltText,
                                TitleText = i.TitleText,
                                SortOrder = i.SortOrder
                            });
                        }
                        UnitOfWork.Save();
                    }
                    break;
                case "setaudience":
                    try
                    {
                        List<ProductPage> toSet = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => keys.Contains(p.ProductPageID)).ToList();
                        foreach (ProductPage c in toSet)
                        {
                            c.TradeSwitch = tradeSwitch;
                        }
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

            if(hasError)
            {
                Response.StatusCode = 400;
                return Content(errorString);
            }
           
           List<int> ProductsByCollection = UnitOfWork.GetRepository<Product_Collection>().GetAll().Where(x => x.CollectionID == CollectionID).Select(y => y.ProductID).ToList();
            List<int?> ProductPagesByCollection = UnitOfWork.GetRepository<Product>().GetAll().InRange(x => x.ProductID, 2000, ProductsByCollection).Select(y => y.ProductPageID).ToList();

            List<int> ProductsByColour = UnitOfWork.GetRepository<Product_Colour>().GetAll().Where(x => x.ColourID == ColourID).Select(y => y.ProductID).ToList();
            List<int?> ProductPagesByColour = UnitOfWork.GetRepository<Product>().GetAll().InRange(x => x.ProductID, 2000, ProductsByColour).Select(y => y.ProductPageID).ToList();

            if (CountryIDs == "")
                CountryIDs = null;

            countries = new List<short>();

            if(CountryIDs != null)
            {
                countries = CountryIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(p => Convert.ToInt16(p)).ToList();
            }

            List<int> ByCountry = UnitOfWork.GetRepository<ProductPage_Country>().GetAll().Where(x => countries.Contains(x.CountryID)).Select(y => y.ProductPageID).ToList();

            List<vwProductPageOverview> Model = UnitOfWork.GetRepository<vwProductPageOverview>().GetAll()
                .Where(p => CollectionID == null ? true : ProductPagesByCollection.Contains(p.ProductPageID))
                .Where(p => ColourID == null ? true : ProductPagesByColour.Contains(p.ProductPageID))
                .Where(p => CountryIDs == null ? true : ByCountry.Contains(p.ProductPageID))
                .ToList();

            return PartialView("_ProductPagesOverviewGridViewPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddNewOrderFormGroup([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.Models.OrderFormGroup item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UnitOfWork.GetRepository<OrderFormGroup>().Add(item);
                    UnitOfWork.Save();

                    using (FlockDataContext db = new FlockDataContext())
                    {
                        db.OrderFormGroup_Language_Initialise();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            List<OrderFormGroup> Model = UnitOfWork.GetRepository<OrderFormGroup>().GetAll().ToList();
            return PartialView("OrderFormGroupManagePartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateOrderFormGroup([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.Models.OrderFormGroup item)
        {
            try
            {
                OrderFormGroup ofg = UnitOfWork.GetRepository<OrderFormGroup>().GetAll().Where(p => p.OrderFormGroupID == item.OrderFormGroupID).Single();
                ofg.Name = item.Name;
                UnitOfWork.Save();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            List<OrderFormGroup> Model = UnitOfWork.GetRepository<OrderFormGroup>().GetAll().ToList();
            return PartialView("OrderFormGroupManagePartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateProductPagePrimaryProducts(int ProductPageID, string Products)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ProductsList = ser.Deserialize<List<SortDatum>>(Products);

            foreach (SortDatum d in ProductsList)
            {
                bool alreadyInSecondary = UnitOfWork.GetRepository<Product_ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.ProductID == d.ID).SingleOrDefault() != null ? true : false;

                if (alreadyInSecondary)
                {
                    continue;
                }

                Product ppsp = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.ProductID == d.ID).SingleOrDefault();
                if (ppsp == null)
                {
                    ppsp = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.ProductID == d.ID).Single();
                    ppsp.ProductPageID = ProductPageID;
                    ppsp.SortOrder = Convert.ToByte(d.SortOrder);
                }
                else
                    ppsp.SortOrder = Convert.ToByte(d.SortOrder);
            }

            UnitOfWork.Save();

            List<vwProductPage_PrimaryProduct> Model = UnitOfWork.GetRepository<vwProductPage_PrimaryProduct>().GetAll().Where(p => p.ProductPageID == ProductPageID).OrderBy(p => p.SortOrder).ToList();
            return PartialView("ProductPagePrimaryProductsPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveProductPagePrimaryProduct(int ProductPageID, int ProductID)
        {
            try
            {
                Product product = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.ProductID == ProductID).Single();
                product.ProductPageID = null;
                UnitOfWork.Save();

                return Content("success");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateProductPageSecondaryProducts(int ProductPageID, string Products)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ProductsList = ser.Deserialize<List<SortDatum>>(Products);

            // okay, so if... the product you're trying to add has this productPageID as it's Product.ProductPageID, then it's already in the primary products
            // and you're not allowed to list it as a secondary product.
            // same holds for Primary.

            foreach (SortDatum d in ProductsList)
            {
                bool alreadyInPrimary = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.ProductID == d.ID && p.ProductPageID == ProductPageID).SingleOrDefault() != null ? true : false;

                if(alreadyInPrimary)
                {
                    continue;
                }

                Product_ProductPage ppp = UnitOfWork.GetRepository<Product_ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.ProductID == d.ID).SingleOrDefault();
                if (ppp == null)
                {
                    ppp = new Product_ProductPage() { ProductPageID = ProductPageID, ProductID = Convert.ToInt32(d.ID), SortOrder = Convert.ToByte(d.SortOrder), OrderFormGroupID = null };
                    UnitOfWork.GetRepository<Product_ProductPage>().Add(ppp);
                }
                else
                {
                    ppp.SortOrder = Convert.ToByte(d.SortOrder);
                }
            }

            UnitOfWork.Save();

            List<vwProductPage_SecondaryProduct> Model = UnitOfWork.GetRepository<vwProductPage_SecondaryProduct>().GetAll().Where(p => p.ProductPageID == ProductPageID).OrderBy(p => p.SortOrder).ToList();
            return PartialView("ProductPageSecondaryProductsPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveProductPageSecondaryProduct(int ProductPageID, int ProductID)
        {
            try
            {
                Product_ProductPage product = UnitOfWork.GetRepository<Product_ProductPage>().GetAll().Where(p => p.ProductID == ProductID && p.ProductPageID == ProductPageID).Single();
                UnitOfWork.GetRepository<Product_ProductPage>().Delete(product);
                UnitOfWork.Save();

                return Content("success");
            }
            catch (Exception e)
            {
                if (e.Message.ToLower().Contains("sequence"))
                    return Content("That product was already removed from the list.");
                return Content(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateRelatedProductPages(int BaseProductPageID, string ProductPages)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ProductPagesList = ser.Deserialize<List<SortDatum>>(ProductPages);

            foreach (SortDatum d in ProductPagesList)
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

            UnitOfWork.Save();

            List<vwProductPage_RelatedProductPage> Model = UnitOfWork.GetRepository<vwProductPage_RelatedProductPage>().GetAll().Where(p => p.ProductPageID == BaseProductPageID).OrderBy(p => p.SortOrder).ToList();
            return PartialView("ProductPageRelatedProductPagesPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveRelatedProductPage(int BaseProductPageID, int ProductPageID)
        {
            try
            {
                ProductPage_RelatedProductPage pprpp = UnitOfWork.GetRepository<ProductPage_RelatedProductPage>().GetAll().Where(p => p.ProductPageID == BaseProductPageID && p.RelatedProductPageID == ProductPageID).Single();
                UnitOfWork.GetRepository<ProductPage_RelatedProductPage>().Delete(pprpp);
                UnitOfWork.Save();

                return Content("success");
            }
            catch (Exception e)
            {
                if (e.Message.ToLower().Contains("sequence"))
                    return Content("That product page was already removed from the list.");
                return Content(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateProductPageImageArea(int ProductPageID, string Images, byte TemplateImageID)
        {
            byte TemplateID = 0; 

            try
            {
                if (TemplateImageID != 6)
                {
                    TemplateID = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID).Single().TemplateID;

                    UnitOfWork.Context.ProductPage_Image_Merge(ProductPageID, Images, TemplateImageID);
                }
                else
                {
                    var pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Single(p => p.ProductPageID == ProductPageID);
                    var imagesList = JsonConvert.DeserializeObject<List<SortDatum>>(Images).ToList();
                    pp.AlternateColourwayImageID = imagesList?.Any() == true ? imagesList[0].ID : (long?)null;
                    UnitOfWork.Save();
                }
            }
            catch (Exception e)
            {
                Response.StatusDescription = e.Message;
                // log e
            }
            List<ProductPage_Image> Model = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.TemplateImageID == TemplateImageID).OrderBy(p => p.SortOrder).ToList();
            switch (TemplateImageID)
            {
                case 1:
                    return PartialView("ProductPagePodImagesPartial", Model);
                case 2:
                    switch (TemplateID)
                    {
                        case 4:
                            return PartialView("ProductPageFurnitureSlideshowImagesPartial", Model);
                        default:
                            return PartialView("ProductPageSlideshowImagesPartial", Model);
                    }
                case 3: 
                case 4:
                    if (TemplateID == 4)
                        return PartialView("FurnitureCutOutImagePartial", Model);
                    else
                        return Content("");
                case 5:
                    return PartialView("ProductPageSlideshowPartial", new ProductPageComposeViewModelBuilder(UnitOfWork).Build(ProductPageID));
                case 6:
                    return PartialView("ProductPageInfoPartial", new ProductPageComposeViewModelBuilder(UnitOfWork).Build(ProductPageID));
                default:
                    return Content("");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveImageFromProductPageArea(int ProductPageID, short TemplateImageID, long ImageID)
        {
            byte templateID = 0;
            try
            {
                templateID = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID).Single().TemplateID;

                if (TemplateImageID != 6)
                {
                    ProductPage_Image CI = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.ImageID == ImageID && p.TemplateImageID == TemplateImageID).Single();
                    UnitOfWork.GetRepository<ProductPage_Image>().Delete(CI);
                    UnitOfWork.Save();
                }
                else
                {
                    var pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Single(p => p.ProductPageID == ProductPageID);
                    pp.AlternateColourwayImageID = null;
                    UnitOfWork.Save();
                }

            }
            catch (Exception e)
            {
                Response.StatusDescription = e.Message;
                // log e
            }

            List<ProductPage_Image> Model = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.TemplateImageID == TemplateImageID).OrderBy(p => p.SortOrder).ToList();
            switch (TemplateImageID)
            {
                case 1:
                    return PartialView("ProductPagePodImagesPartial", Model);
                case 2:
                    switch (templateID)
                    {
                        case 4:
                            return PartialView("ProductPageFurnitureSlideshowImagesPartial", Model);
                        default:
                            return PartialView("ProductPageSlideshowImagesPartial", Model);
                    }
                case 3:
                case 4:
                    return PartialView("FurnitureCutOutImagePartial", Model);
                case 5:
                    return PartialView("ProductPageSlideshowPartial", new ProductPageComposeViewModelBuilder(UnitOfWork).Build(ProductPageID));
                case 6:
                    return PartialView("ProductPageInfoPartial", new ProductPageComposeViewModelBuilder(UnitOfWork).Build(ProductPageID));
                default:
                    return Content("");
            }
        }

        
        public ActionResult ComposeProductPageGridSelectorPartial()
        {
            return PartialView();
        }

        public class ProductPageCreationModel
        {
            [Required]
            [StringLength(100, MinimumLength = 5)]
            public string Name { get; set; }

            [Required]
            public byte TemplateID { get; set; }

            [Required]
            [StringLength(70, MinimumLength = 5)]
            public string Heading { get; set; }

            [Required]
            public byte BrandID { get; set; }

            [Required]
            public short LaunchID { get; set; }
        }

        [HttpPost]
        public ActionResult ComposeProductPageGridSelectorPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]ProductPageCreationModel item)
        {
            if(ModelState.IsValid)
            {
                UnitOfWork.GetRepository<ProductPage>().Add(new ProductPage() { 
                    Name = item.Name, 
                    TemplateID = item.TemplateID, 
                    Heading = item.Heading, 
                    URLKeywords = "",
                    ProductTypeID = 255,
                    BrandID = item.BrandID,
                    LaunchID = item.LaunchID,
                    CreatedDT = DateTime.Now,
                    TradeSwitch = null
                });
                UnitOfWork.Save();
                UnitOfWork.Context.ProductPage_Language_Initialise();
            }
            
            return PartialView("ComposeProductPageGridSelectorPartial", item);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ComposeProductPageGridSelectorPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.Models.vwProductPageOverview item)
        {
            try
            {
                ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == item.ProductPageID).Single();
                pp.DeletedDT = DateTime.Now;

                UnitOfWork.Save();

                return PartialView("ComposeProductPageGridSelectorPartial");
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("ComposeProductPageGridSelectorPartial");
        }

        [AllowAnonymous]
        public ActionResult ComposeProductPageHeaderProductTypeDropDownPartial(ProductSearchHeader Filters)
        {
            ViewBag.ProductHeaderSelectedProductTypeID = Filters.ProductTypeID;

            List<ProductPageComposeHeader_ProductType_SelectListResult> PreModel =
                UnitOfWork.Context.ProductPageComposeHeader_ProductType_SelectList(Filters.LaunchID, Filters.BrandID, Filters.TemplateID, Filters.WebStatusID)
                .OrderBy(p => p.Name).ToList();
            var Model = PreModel
              .Select(x => new ProductType() { ProductTypeID = x.ProductTypeID, Name = x.Name })
              .ToList();
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposeProductPageHeaderBrandDropDownPartial(ProductSearchHeader Filters)
        {
            ViewBag.ProductHeaderSelectedBrandID = Filters.BrandID; 
            List<ProductPageComposeHeader_Brand_SelectListResult> PreModel =
                 UnitOfWork.Context.ProductPageComposeHeader_Brand_SelectList(Filters.ProductTypeID, Filters.LaunchID, Filters.TemplateID, Filters.WebStatusID)
                 .OrderBy(p => p.Name).ToList();
            var Model = PreModel
                .Select(x => new Brand() { BrandID = x.BrandID, Name = x.Name })
                .ToList();
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposeProductPageHeaderLaunchDropDownPartial(ProductSearchHeader Filters)
        {
            ViewBag.ProductHeaderSelectedLaunchID = Filters.LaunchID;
            List<ProductPageComposeHeader_Launch_SelectListResult> PreModel =
                UnitOfWork.Context.ProductPageComposeHeader_Launch_SelectList(Filters.ProductTypeID, Filters.BrandID, Filters.TemplateID, Filters.WebStatusID)
                .OrderByDescending(p => p.LaunchDate)
                .ToList();
            var Model = PreModel
                .Select(x => new Launch() { LaunchID = x.LaunchID, LaunchDate = x.LaunchDate, Name = x.Name, SeasonID = x.SeasonID })
                .ToList();
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposeProductPageHeaderWebStatusDropDownPartial(ProductSearchHeader Filters)
        {
            ViewBag.ProductHeaderSelectedWebStatusID = Filters.WebStatusID;
            List<ProductPageComposeHeader_WebStatus_SelectListResult> PreModel = 
                UnitOfWork.Context.ProductPageComposeHeader_WebStatus_SelectList(Filters.ProductTypeID, Filters.BrandID, Filters.TemplateID, Filters.LaunchID)
                .OrderBy(p => p.Name)
                .ToList();
            var Model = PreModel
                .Select(x => new Status() { Name = x.Name, StatusID = x.StatusID })
                .ToList();
            return PartialView("~/Views/Shared/SearchFilters/WebStatusDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposeProductPageHeaderTemplateDropDownPartial(ProductSearchHeader Filters)
        {
            ViewBag.ProductHeaderSelectedTemplateID = Filters.TemplateID;
            List<ProductPageComposeHeader_Template_SelectListResult> PreModel = 
                UnitOfWork.Context.ProductPageComposeHeader_Template_SelectList(Filters.ProductTypeID, Filters.BrandID, Filters.WebStatusID, Filters.LaunchID)
                .OrderBy(p => p.Name)
                .ToList();

            var Model = PreModel
                .Select(x => new Template() { Name = x.Name, TemplateID = x.TemplateID })
                .ToList();
            return PartialView("~/Views/Shared/SearchFilters/TemplateDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ComposeProductPageHeader(ProductSearchHeader Filters)
        {
            Session["ComposeProductPageProductTypeID"] = Filters.ProductTypeID;
            Session["ComposeProductPageBrandID"] = Filters.BrandID;
            Session["ComposeProductPageLaunchID"] = Filters.LaunchID;
            Session["ComposeProductPageWebStatusID"] = Filters.WebStatusID;
            Session["ComposeProductPageTemplateID"] = Filters.TemplateID;

            ViewBag.ProductHeaderSelectedProductPageID = Filters.ID;
            ViewBag.ProductHeaderSelectedProductTypeID = Filters.ProductTypeID;
            ViewBag.ProductHeaderSelectedBrandID = Filters.BrandID;
            ViewBag.ProductHeaderSelectedLaunchID = Filters.LaunchID;
            ViewBag.ProductHeaderSelectedWebStatusID = Filters.WebStatusID;
            ViewBag.ProductHeaderSelectedTemplateID = Filters.TemplateID;

            return PartialView("ProductPageComposeProductSelectHeaderPartial", Filters);
        }

        [AllowAnonymous]
        public ActionResult ComposeProductPageHeaderProductPageDropDownPartial(ProductSearchHeader Filters)// byte? ProductTypeID, byte? BrandID, short? LaunchID, byte? WebStatusID, byte? TemplateID)
        {
            Session["ComposeProductPageProductTypeID"] = Filters.ProductTypeID;
            Session["ComposeProductPageBrandID"] = Filters.BrandID;
            Session["ComposeProductPageLaunchID"] = Filters.LaunchID;
            Session["ComposeProductPageWebStatusID"] = Filters.WebStatusID;
            Session["ComposeProductPageTemplateID"] = Filters.TemplateID;

            ViewBag.ProductHeaderSelectedProductPageID = Filters.ID;
            ViewBag.ProductHeaderSelectedProductTypeID = Filters.ProductTypeID;
            ViewBag.ProductHeaderSelectedBrandID = Filters.BrandID;
            ViewBag.ProductHeaderSelectedLaunchID = Filters.LaunchID;
            ViewBag.ProductHeaderSelectedWebStatusID = Filters.WebStatusID;
            ViewBag.ProductHeaderSelectedTemplateID = Filters.TemplateID;

            return PartialView("ProductPageComboboxPartial", Filters);
        }

        [AllowAnonymous]
        public ActionResult ProductPagesOverviewGridViewPartial(short? CollectionID, byte? ColourID, string CountryIDs)
        {
           List<int> ProductsByCollection = UnitOfWork.GetRepository<Product_Collection>().GetAll().Where(x => x.CollectionID == CollectionID).Select(y => y.ProductID).ToList();
            List<int?> ProductPagesByCollection = UnitOfWork.GetRepository<Product>().GetAll().InRange(x => x.ProductID, 2000, ProductsByCollection).Select(y => y.ProductPageID).ToList();

            List<int> ProductsByColour = UnitOfWork.GetRepository<Product_Colour>().GetAll().Where(x => x.ColourID == ColourID).Select(y => y.ProductID).ToList();
            List<int?> ProductPagesByColour = UnitOfWork.GetRepository<Product>().GetAll().InRange(x => x.ProductID, 2000, ProductsByColour).Select(y => y.ProductPageID).ToList();

            if (CountryIDs == "")
                CountryIDs = null;

            List<short> countries = new List<short>();

            if(CountryIDs != null)
            {
                countries = CountryIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(p => Convert.ToInt16(p)).ToList();
            }

            List<int> ByCountry = UnitOfWork.GetRepository<ProductPage_Country>().GetAll().Where(x => countries.Contains(x.CountryID)).Select(y => y.ProductPageID).ToList();

            var Model = UnitOfWork.GetRepository<vwProductPageOverview>().GetAll()
                .Where(p => CollectionID == null ? true : ProductPagesByCollection.Contains(p.ProductPageID))
                .Where(p => ColourID == null ? true : ProductPagesByColour.Contains(p.ProductPageID))
                .Where(p => CountryIDs == null ? true : ByCountry.Contains(p.ProductPageID))
                .ToList();

            return PartialView("_ProductPagesOverviewGridViewPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ProductPagesOverviewGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.ViewModels.ProductPageOverviewViewModel item)
        {
            if(ModelState.IsValid)
            {
                if(item.Name != "")
                {
                    ProductPage productpage = new ProductPage();
                    productpage.TemplateID = item.TemplateID;
                    productpage.Name = item.Name;
                    productpage.CreatedDT = DateTime.Now;
                    productpage.URLKeywords = "";
                    productpage.ProductTypeID = 255;
                    UnitOfWork.GetRepository<ProductPage>().Add(productpage);
                    UnitOfWork.Save();
                    UnitOfWork.Context.ProductPage_Language_Initialise();
                    List<vwProductPageOverview> Model = UnitOfWork.GetRepository<vwProductPageOverview>().GetAll().OrderBy(p => p.ProductPageID).ToList();
                    return PartialView("_ProductPagesOverviewGridViewPartial", Model);
                }
                else
                {
                    ModelState.AddModelError("Name", "The Name field is required.");
                    List<vwProductPageOverview> Model = UnitOfWork.GetRepository<vwProductPageOverview>().GetAll().OrderBy(p => p.ProductPageID).ToList();
                    return PartialView("_ProductPagesOverviewGridViewPartial", Model);
                }
            }
            else
            {
                if (item.Name == "" || item.Name == null)
                {
                    ModelState.AddModelError("Name", "The Name field is required.");
                }
                ViewData["EditError"] = "Please, correct all errors.";
                List<vwProductPageOverview> Model = UnitOfWork.GetRepository<vwProductPageOverview>().GetAll().OrderBy(p => p.ProductPageID).ToList();
                return PartialView("_ProductPagesOverviewGridViewPartial", Model);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ProductPagesOverviewGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.Models.vwProductPageOverview item)
        {
            bool hasError = false;
            if (item.Name == null || item.Name == "")
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Name for this product page.<br/>";
            }
            if (item.TemplateID == 0 || item.TemplateID == 1)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Template for this product page<br/>";
            }
            if (item.BrandID == 0)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Brand for this product page.<br/>";
            }
            if (item.LaunchID == 0)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Launch for this product page.<br/>";
            }
            if (item.ProductTypeID == 0)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Product Type for this product page<br/>";
            }
            if (item.OffDate != null && item.OnDate == null)
            {
                hasError = true;
                ViewData["EditError"] += "Please set an On date for this product page<br/>";
            }
            if (item.OnDate != null && item.OffDate != null)
            {
                if (item.OffDate <= item.OnDate)
                {
                    hasError = true;
                    ViewData["EditError"] += "On date must be before Off date<br/>";
                }
            }
            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == item.ProductPageID).Single();
                        pp.Name = item.Name;
                        pp.TemplateID = item.TemplateID;
                        pp.BrandID = item.BrandID;
                        pp.ProductTypeID = item.ProductTypeID;
                        pp.LaunchID = item.LaunchID;
                        pp.OnDate = item.OnDate;
                        pp.OffDate = item.OffDate;

                        UnitOfWork.Save();

                        List<vwProductPageOverview> model = UnitOfWork.GetRepository<vwProductPageOverview>().GetAll().OrderBy(p => p.ProductPageID).ToList();

                        return PartialView("_ProductPagesOverviewGridViewPartial", model);
                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] = e.Message;
                    }
                }
                else
                    ViewData["EditError"] = "Please, correct all errors.";
            }

            List<vwProductPageOverview> Model = UnitOfWork.GetRepository<vwProductPageOverview>().GetAll().OrderBy(p => p.ProductPageID).ToList();

            return PartialView("_ProductPagesOverviewGridViewPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ProductPagesOverviewGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.Models.vwProductPageOverview item)
        {
            try
            {
                ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == item.ProductPageID).Single();
                pp.DeletedDT = DateTime.Now;

                UnitOfWork.Save();

                List<vwProductPageOverview> model = UnitOfWork.GetRepository<vwProductPageOverview>().GetAll().OrderBy(p => p.ProductPageID).ToList();

                return PartialView("_ProductPagesOverviewGridViewPartial", model);
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            List<vwProductPageOverview> Model = UnitOfWork.GetRepository<vwProductPageOverview>().GetAll().OrderBy(p => p.ProductPageID).ToList();

            return PartialView("_ProductPagesOverviewGridViewPartial", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPagesOverviewNewCountryRestrictionDropDownPartial()
        {
            List<Country> Model = UnitOfWork.GetRepository<Country_Language>().GetAll().OrderBy(p => p.Name).Select(p => p.Country).Distinct().ToList();
            return PartialView("~/Views/Shared/SearchFilters/CountryDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPagesOverviewCollectionDropDownPartial()
        {
            List<Collection> Model = UnitOfWork.GetRepository<Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPagesOverviewColourDropDownPartial()
        {
            List<Colour> Model = UnitOfWork.GetRepository<Colour>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPagesOverviewCountryRestrictionsDropDownPartial()
        {
            List<Country> Model = UnitOfWork.GetRepository<Country_Language>().GetAll().OrderBy(p => p.Name).Select(p => p.Country).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CountryDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateProductPageTextArea(int ProductPageID, byte AreaID, string Text)
        {
            ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID).Single();
            switch (AreaID)
            {
                case 1: // TitleTag
                    pp.TitleTag = Text;
                    break;
                case 2: // URLKeywords
                    pp.URLKeywords = Text == null ? "" : Text;
                    break;
                case 3: // MetaDescription
                    pp.MetaDescription = Text;
                    break;
                case 4: // Heading
                    pp.Heading = Text;
                    break;
                case 5: // SubHeading
                    pp.SubHeading = Text;
                    break;
                default: break;
            }
            if (pp.URLKeywords == null)
                pp.URLKeywords = "";
            pp.ModifiedDT = DateTime.Now;
            UnitOfWork.Save();

            UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, null, ProductPageID, null, null, false, false);

            return Content("");
        }

        [AllowAnonymous]
        public ActionResult ProductPageSave(ProductPageComposeViewModel item)
        {
            ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == item.ProductPageID).Single();

            pp.TitleTag = item.TitleTag;
            pp.URLKeywords = item.URLKeywords == null ? "" : item.URLKeywords;
            pp.MetaDescription = item.MetaDescription;
            pp.Heading = item.Heading;
            pp.SubHeading = item.SubHeading;
            pp.ModifiedDT = DateTime.Now;

            UnitOfWork.Save();

            return RedirectToAction("compose", "products", new { id = item.ProductPageID });
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveProductPageInfo(ProductPageComposeViewModel model)
        {
            bool hasError = false;
            if (model.OnDate == null && model.OffDate != null)
            {
                hasError = true;
                ViewData["EditError"] += "Please specify an On Date.";
                ModelState.AddModelError("OnDate", "Please specify an On Date");
            }
            if (model.OnDate != null && model.OffDate != null)
            {
                if (model.OffDate <= model.OnDate)
                {
                    hasError = true;
                    ViewData["EditError"] += "On Date must be before Off Date.";
                    ModelState.AddModelError("OnDate", "On Date must be before Off Date");
                    ModelState.AddModelError("OffDate", "Off Date must be after On Date");
                }
            }
            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == model.ProductPageID).Single();
                    pp.BrochureID = model.BrochureID;
                    pp.Name = model.Name;
                    pp.OnDate = model.OnDate;
                    pp.OffDate = model.OffDate;
                    pp.New = model.New;
                    pp.TradeSwitch = model.TradeSwitch;
                    UnitOfWork.Save();

                    ProductPageComposeViewModelBuilder Builder = new ProductPageComposeViewModelBuilder(UnitOfWork);
                    ProductPageComposeViewModel Model = Builder.Build(model.ProductPageID);

                    return PartialView("ProductPageInfoPartial", Model);
                }
                else
                {
                    ViewData["EditError"] += "Please correct all errors.";
                    Response.StatusCode = 400;

                    return PartialView("ProductPageInfoPartial", model);
                }
            }
            else
            {
                Response.StatusCode = 400;

                ProductPageComposeViewModelBuilder Builder = new ProductPageComposeViewModelBuilder(UnitOfWork);
                ProductPageComposeViewModel Model = Builder.Build(model.ProductPageID);

                return PartialView("ProductPageInfoPartial", Model);
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveProductPageDetails(ProductPageComposeViewModel model)
        {
            if (ModelState.IsValid)
            {
                ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == model.ProductPageID).Single();

                pp.TitleTag = model.TitleTag;
                pp.URLKeywords = model.URLKeywords == null ? "" : model.URLKeywords;
                pp.MetaDescription = model.MetaDescription;
                pp.ModifiedDT = DateTime.Now;
                UnitOfWork.Save();

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, null, model.ProductPageID, null, null, false, false);

                ProductPageComposeViewModelBuilder Builder = new ProductPageComposeViewModelBuilder(UnitOfWork);
                ProductPageComposeViewModel Model = Builder.Build(model.ProductPageID);

                return PartialView("ProductPageDetailsPartial", Model);
            }
            else
            {
                ViewData["EditError"] += "Please correct all errors.";
                Response.StatusCode = 400;

                ProductPageComposeViewModelBuilder Builder = new ProductPageComposeViewModelBuilder(UnitOfWork);
                ProductPageComposeViewModel Model = Builder.Build(model.ProductPageID);

                model.Images = Model.Images;

                return PartialView("ProductPageDetailsPartial", Model);
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveProductPageText(ProductPageComposeViewModel model)
        {
            if(ModelState.IsValid)
            {
                ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == model.ProductPageID).Single();
                pp.Heading = model.Heading;
                pp.SubHeading = model.SubHeading;
                pp.Description = model.Description;
                pp.FurnitureFabricOptions = model.FurnitureFabricOptions.HasValue? model.FurnitureFabricOptions.Value : false;
                pp.FurnitureLeatherOptions = model.FurnitureLeatherOptions.HasValue ? model.FurnitureLeatherOptions.Value : false;
                pp.FurnitureLegStyles = model.FurnitureLegStyles.HasValue ? model.FurnitureLegStyles.Value : false; ;
                pp.FurnitureLegFinishes = model.FurnitureLegFinishes.HasValue ? model.FurnitureLegFinishes.Value : false;
                pp.FurnitureStitchingOptions = model.FurnitureStitchingOptions.HasValue ? model.FurnitureStitchingOptions.Value : false;
                pp.FurniturePDF = model.FurniturePDF;
                pp.VideoURL = model.VideoURL;

                if (pp.ProductPage_Images.FirstOrDefault(i => i.TemplateImageID == 5) == null)
                {
                    var img = pp.ProductPage_Images.Where(i => i.TemplateImageID == 2).OrderBy(i => i.SortOrder).FirstOrDefault();

                    if (img != null)
                    {
                        pp.ProductPage_Images.Add(new ProductPage_Image()
                        {
                            ImageID = img.ImageID,
                            TemplateImageID = 5,
                            ProductPageID = model.ProductPageID.Value
                        });
                    }
                }

                pp.ModifiedDT = DateTime.Now;

                UnitOfWork.Save();

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_ProductPage(model.ProductPageID, null, null, null, null, null, false, false);

                ProductPageComposeViewModelBuilder Builder = new ProductPageComposeViewModelBuilder(UnitOfWork);
                ProductPageComposeViewModel Model = Builder.Build(model.ProductPageID);
                if (pp.TemplateID != 4)
                    return PartialView("ProductPageSlideshowPartial", Model);
                else
                    return PartialView("ProductPageFurnitureCutOutImagePartial", Model);
            }
            else
            {
                ViewData["EditError"] += "Please, correct all errors.";
                Response.StatusCode = 400;
                ProductPage pp = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == model.ProductPageID).Single();

                model.Images = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == model.ProductPageID).OrderBy(p => p.SortOrder).ToList();


                if (pp.TemplateID != 4)
                    return PartialView("ProductPageSlideshowPartial", model);
                else
                    return PartialView("ProductPageFurnitureCutOutImagePartial", model);
            }
        }

        [AllowAnonymous]
        public ActionResult ProductPageComposeTemplateDropDownPartial(byte? SelectedTemplateID)
        {
            ViewBag.ProductPageComposeSelectedTemplateID = SelectedTemplateID;
            List<Template> Model = UnitOfWork.GetRepository<Template>().GetAll().OrderBy(p => p.Name).Where(p => p.TemplateID != 1).ToList();
            return PartialView("~/Views/Shared/SearchFilters/TemplateDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageComposeBrandDropDownPartial(byte? SelectedBrandID)
        {
            ViewBag.ProductPageComposeSelectedBrandID = SelectedBrandID;
            List<Brand> Model = UnitOfWork.GetRepository<Brand>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageComposeProductTypeDropDownPartial(byte? SelectedProductTypeID)
        {
            ViewBag.ProductPageComposeSelectedProductTypeID = SelectedProductTypeID;
            List<ProductType> Model = UnitOfWork.GetRepository<ProductType>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageComposeLaunchDropDownPartial(byte? SelectedLaunchID)
        {
            ViewBag.ProductPageComposeSelectedLaunchID = SelectedLaunchID;
            List<Launch> Model = UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageComposePatternDropDownPartial(byte? SelectedPatternID)
        {
            ViewBag.ProductPageComposeSelectedPatternID = SelectedPatternID;
            List<Pattern> Model = UnitOfWork.GetRepository<Pattern>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PatternDropDownPartial.cshtml", Model);
        }

        [HttpPost]
        public ActionResult CopyImages(int ProductPageID, string Direction)
        {
            try
            {
                switch(Direction)
                {
                    case "PodSlideshow":
                        var src = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.TemplateImageID == 1).ToList();
                        var dest = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.TemplateImageID == 2).ToList();
                        UnitOfWork.GetRepository<ProductPage_Image>().DeleteAll(dest);
                        UnitOfWork.Save();

                        foreach (var i in src)
                        {
                            UnitOfWork.GetRepository<ProductPage_Image>().Add(new ProductPage_Image()
                            {
                                ProductPageID = ProductPageID,
                                TemplateImageID = 2,
                                ImageID = i.ImageID,
                                AltText = i.AltText,
                                TitleText = i.TitleText,
                                SortOrder = i.SortOrder
                            });
                        }
                        UnitOfWork.Save();
                        var Model = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.TemplateImageID == 2).OrderBy(p => p.SortOrder).ToList();
                        return PartialView("ProductPageSlideshowImagesPartial", Model);
                    case "SlideshowPod":
                        var src2 = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.TemplateImageID == 2).ToList();
                        var dest2 = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.TemplateImageID == 1).ToList();
                        UnitOfWork.GetRepository<ProductPage_Image>().DeleteAll(dest2);
                        UnitOfWork.Save();

                        foreach (var i in src2)
                        {
                            UnitOfWork.GetRepository<ProductPage_Image>().Add(new ProductPage_Image()
                            {
                                ProductPageID = ProductPageID,
                                TemplateImageID = 1,
                                ImageID = i.ImageID,
                                AltText = i.AltText,
                                TitleText = i.TitleText,
                                SortOrder = i.SortOrder
                            });
                        }
                        UnitOfWork.Save();
                        var Model2 = UnitOfWork.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.TemplateImageID == 1).OrderBy(p => p.SortOrder).ToList();
                        return PartialView("ProductPagePodImagesPartial", Model2);
                    default:
                        return Content("");
                }
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return Content(ex.ToString());
            }
        }


        /////////////////
        //
        //  Attributes
        //
        /////////////////


        public ActionResult Attributes(int? ID)
        {
            AttributeAssignmentViewModel Model = new AttributeAssignmentViewModel();

            if (ID != null)
            {
                if (UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == ID && p.DeletedDT == null).SingleOrDefault() != null)
                {
                    Model = new AttributeAssignmentViewModelBuilder(UnitOfWork).Build(ID);

                    return View(Model);
                }
                else
                {
                    ViewData["InvalidProductPageIDError"] = "Invalid Product Page ID, or the page is marked as deleted. Deleted pages can be retrieved.";
                }
            }
            return View(Model);
        }

        [AllowAnonymous]
        public ActionResult ProductPageAttributeGridSelectorPartial()
        {
            return PartialView();
        }
    
        public ActionResult AttributeProductTypeDropDownPartial(byte? SelectedProductTypeID)
        {
            var Model = UnitOfWork.GetRepository<ProductType>().GetAll().Where(p => p.Deleted == false).OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        [HttpPost]
        public ActionResult ReloadProductPageAttributes(int ID)
        {
            AttributeAssignmentViewModel Model = new AttributeAssignmentViewModelBuilder(UnitOfWork).Build(ID);
            
            return PartialView(Model);
        }

        [HttpPost]
        public ActionResult AttributeGroupsPartial(int ID, byte? ProductTypeID)
        {
            AttributeAssignmentViewModel Model = new AttributeAssignmentViewModelBuilder(UnitOfWork).Build(ID);

            Model.AttributeGroups.Clear();

            var groups = UnitOfWork.Context.AttributeGroup_Select(ProductTypeID).OrderBy(p => p.SortOrder).ToList();

            foreach (var group in groups)
            {
                var available = UnitOfWork.Context.Attribute_Select(ProductTypeID, group.AttributeGroupID).OrderBy(p => p.SortOrder).ToList();
                var assigned = UnitOfWork.Context.ProductPage_Attribute_Select(ID).ToList();
                Model.AttributeGroups.Add(new AttributeGroupViewModel()
                {
                    AttributeGroupID = group.AttributeGroupID,
                    Name = group.Name,
                    IsMultipleSelect = group.MultipleSelect,
                    SortOrder = group.SortOrder,
                    Options = available,
                    SelectedValues = assigned.Select(p => p.AttributeID).ToList(),
                    AutoSync = group.AutoSync
                });
            }

            Model.AttributeGroups = Model.AttributeGroups.OrderBy(p => p.SortOrder).ToList();

            return PartialView(Model);
        }

        [HttpPost]
        public ActionResult SuggestAttributes(int ID)
        {
            IMultipleResults Suggestions = UnitOfWork.Context.ProductPage_Attribute_Suggest(ID, false);
            var PTID = Suggestions.GetResult<ProductTypeSuggestionResult>().Single();
            var BID = Suggestions.GetResult<BrandSuggestionResult>().Single();
            var LID = Suggestions.GetResult<LaunchSuggestionResult>().Single();
            var Attributes = Suggestions.GetResult<AttributeResultTable>().ToList();

            // right, the multiple results set thing works nicely...
            return Json(new { 
                ProductTypeID = PTID.ProductTypeID,
                BrandID = BID.BrandID,
                LaunchID = LID.LaunchID,
                Attributes = Json(Attributes)
            });
        }

        [HttpPost]
        public ActionResult UpdateAttributes(int ID, byte? ProductTypeID, byte? BrandID, byte? LaunchID, byte? DesignID, byte? ColourID, byte? PatternID, string AttributeIDs)
        {
            try
            {
                List<short> AttributesList = new JavaScriptSerializer().Deserialize<List<short>>(AttributeIDs);
                UnitOfWork.Context.ProductPage_Attribute_Update(ID, ProductTypeID, BrandID, LaunchID, ColourID, PatternID, AttributesList);
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
        public ActionResult AttributeSyncAssociatedProducts(int ID, bool IsProductPage)
        {
            if (IsProductPage)
            {
                var Model = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.ProductPageID == ID && p.DeletedDT == null).ToList();
                return PartialView(Model);
            }
            else
            {
                var PreModel = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.ProductID == ID && p.DeletedDT == null).Select(p => p.ProductPageID).Single();
                var Model = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.ProductPageID == PreModel).ToList();
                return PartialView(Model);
            }
        }

        //[HttpPost]
        //public ActionResult SynchroniseProductPagePrimaries(int ID, byte ProductTypeID, byte? BrandID, byte? LaunchID, byte? DesignID, string AttributeIDs, string ProductIDs)
        //{
        //    try
        //    {
        //        List<short> AttributesList = new JavaScriptSerializer().Deserialize<List<short>>(AttributeIDs);
        //        List<int> ProductsList = new JavaScriptSerializer().Deserialize<List<int>>(ProductIDs);
        //        UnitOfWork.Context.ProductPage_Attribute_Update(ID, ProductTypeID, BrandID, LaunchID, AttributesList);
        //        UnitOfWork.Context.Product_Attribute_UpdateFromProductPage(ID, ProductTypeID, BrandID, LaunchID, AttributesList, ProductsList);
        //        return Content("");
        //    }
        //    catch(Exception e)
        //    {
        //        //log e
        //        Response.StatusCode = 400;
        //        return Content(e.Message);
        //    }
        //}

        public ActionResult AttributeProductTypeDropDown(byte? SelectedProductTypeID)
        {
            var Model = UnitOfWork.GetRepository<ProductType>().GetAll().ToList();
            ViewBag.AttributesSelectedProductTypeID = SelectedProductTypeID;
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        public ActionResult AttributeBrandDropDown(byte? SelectedBrandID)
        {
            var Model = UnitOfWork.GetRepository<Brand>().GetAll().ToList();
            ViewBag.AttributesSelectedBrandID = SelectedBrandID;
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        public ActionResult AttributeLaunchDropDown(short? SelectedLaunchID)
        {
            var Model = UnitOfWork.GetRepository<Launch>().GetAll().ToList();
            ViewBag.AttributesSelectedLaunchID = SelectedLaunchID;
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        public ActionResult AttributeColourDropDown(byte? SelectedColourID)
        {
            var Model = UnitOfWork.GetRepository<Colour>().GetAll().ToList();
            ViewBag.AttributesSelectedColourID = SelectedColourID;
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }
        
        public ActionResult AttributePatternDropDown(byte? SelectedPatternID)
        {
            var Model = UnitOfWork.GetRepository<Pattern>().GetAll().ToList();
            ViewBag.AttributesSelectedPatternID = SelectedPatternID;
            return PartialView("~/Views/Shared/SearchFilters/PatternDropDownPartial.cshtml", Model);
        }

        [HttpPost]
        public ActionResult AttributeMainPartial(int? ID)
        {
            if (ID != null)
            {
                AttributeAssignmentViewModel Model = new AttributeAssignmentViewModelBuilder(UnitOfWork).Build(ID);

                return PartialView(Model);
            }
            else
            {
                return PartialView();
            }
        }


        public ActionResult Colourways()
        {
            return View();
        }

        public ActionResult AlternateColourwayGridPartial()
        {
            var Model = new List<ProductPageColourwayRow>();

            IMultipleResults ProductPages = UnitOfWork.Context.ProductPage_AlternateColourway_SortOrder();
            var PPs = ProductPages.GetResult<ProductPageAlternateColourwaySelect_First>().ToList();
            var ACs = ProductPages.GetResult<ProductPageAlternateColourwaySelect_Second>().ToList();

            foreach(var PP in PPs)
            {
                var colourways = ACs.Where(p => p.DesignID == PP.DesignID && p.BrandID == PP.BrandID && p.ProductTypeID == PP.ProductTypeID).ToList();
                Model.Add(new ProductPageColourwayRow() { 
                    DesignID = PP.DesignID,
                    LaunchID = PP.LaunchID,
                    Name = PP.Name,
                    ProductPageCount = PP.ProductPageCount,
                    ProductTypeID = PP.ProductTypeID,
                    BrandID = PP.BrandID,
                    Colourways = colourways,
                    ColourwayCount = colourways.Count()
                });
            }
            
            return PartialView("AlternateColourwayGridPartial", Model);
        }

        [HttpPost]
        public ActionResult UpdateColourwaysSortOrder(string Colourways)
        {
            try { 
                var ColourwaysList = new JavaScriptSerializer().Deserialize<List<SortDatum>>(Colourways);
                foreach(var pp in ColourwaysList)
                {
                    var x = UnitOfWork.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == pp.ID).Single();
                    x.AlternateColourwaySortOrder = Convert.ToByte(pp.SortOrder);
                }
                UnitOfWork.Save();
                return Content("");
            }
            catch(Exception e)
            {
                Response.StatusCode = 400;
                return Content(e.ToString());
            }
        }
    }
}
