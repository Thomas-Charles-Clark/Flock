using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using NLog;
using DevExpress.Web.Mvc;
using LinqToExcel;
using DevExpress.Web;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Web.Routing;

namespace Flock.Controllers
{
    public class BundlePriceImportController : Controller
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
        // GET: /BundlePriceImport/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("UploadControl", 
                BundlePriceImportControllerUploadControlSettings.UploadControlValidationSettings,
                BundlePriceImportControllerUploadControlSettings.FileUploadComplete);

            return null;
        }
        
        public ActionResult CountryComboBoxPartial()
        {
            return PartialView();
        }
        public ActionResult LanguageComboBoxPartial()
        {
            return PartialView();
        }
        public ActionResult ThirdPartyEntryBoxPartial()
        {
            return PartialView();
        }
	}

    public class BundlePriceImportControllerUploadControlSettings
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadControlValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
            MaxFileSize = 4194304
        };

        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            try
            {
                if (e.UploadedFile.IsValid)
                {
                    string fileName = System.Web.HttpRuntime.AppDomainAppPath + "//UploadedBundlePrices//" + e.UploadedFile.FileName + "-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";

                    if (!System.IO.Directory.Exists(System.Web.HttpRuntime.AppDomainAppPath + "//UploadedBundlePrices//"))
                    {
                        System.IO.Directory.CreateDirectory(System.Web.HttpRuntime.AppDomainAppPath + "//UploadedBundlePrices//");
                    }

                    e.UploadedFile.SaveAs(fileName);

                    var excel = new ExcelQueryFactory();

                    excel.FileName = fileName;

                    var records = (from x in excel.Worksheet<BundlePriceImportExcelRow>()
                                  select x).ToList();

                    var Model = new Flock.ViewModels.BundlePriceMergeResultViewModel();

                    var UoW = new UnitOfWork<FlockDataContext>();
                    
                    Model.AllRecords = records;

                    Model.Rejected = Model.AllRecords.Where(p => p.IsValid() == false).ToList();
                    Model.Accepted = Model.AllRecords.Except(Model.Rejected).ToList();

                    UoW.Context.BundlePriceImports.DeleteAllOnSubmit(UoW.Context.BundlePriceImports);

                    UoW.Context.SubmitChanges();

                    //var ret = UoW.Context.Member_Import(records, CountryID, LanguageID, ThirdPartyID, SendWelcomeEmail);

                    UoW.Context.BundlePriceImports.InsertAllOnSubmit(Model.BundlePrices);

                    UoW.Context.SubmitChanges();

                    UoW.Context.BundlePriceImport_Process();

                    e.CallbackData = RenderViewToString(CreateController<EmptyController>().ControllerContext, "~/Views/BundlePriceImport/UploadResultsPanelPartial.cshtml", Model, true);
                }
            }
            catch (Exception ex)
            {
                var Model = new Flock.ViewModels.MailingListImportResultViewModel();
                e.ErrorText = ex.ToString();
                Model.ErrorTexts = new string[1] { e.ErrorText };
                Model.UploadedRows = null;
                e.CallbackData = RenderViewToString(CreateController<EmptyController>().ControllerContext, "~/Views/BundlePriceImport/UploadResultsPanelPartial.cshtml", Model, true);
            }
        }

        static string RenderViewToString(ControllerContext context,
                                    string viewPath,
                                    object model = null,
                                    bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }

        public static T CreateController<T>(RouteData routeData = null)
            where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper;
            if (System.Web.HttpContext.Current != null)
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            else
                throw new InvalidOperationException(
                    "Can't create Controller Context if no " +
                    "active HttpContext instance is available.");

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") &&
                !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller",
                                     controller.GetType()
                                               .Name.ToLower().Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }

        public class EmptyController : Controller { }
    }

    public class BundlePriceImportExcelRow
    {
        public string NewBundleGrouping { get; set; }
        public string BundleProductCode { get; set; }
        public string BundleName { get; set; }
        public string ComponentProductCode { get; set; }
        public decimal? ComponentQuantity { get; set; }
        public string PriceListCode { get; set; }
        public string PriceListName { get; set; }
        public decimal? OriginalBundlePrice { get; set; }
        public decimal? DiscountedBundlePrice { get; set; }
        public decimal? OriginalComponentPrice { get; set; }
        public decimal? DiscountedComponentPrice { get; set; }

        public bool IsValid()
        {
            var result = true;

            if (!String.IsNullOrEmpty(NewBundleGrouping) && !String.IsNullOrEmpty(BundleProductCode))
                result = false;

            if (String.IsNullOrEmpty(NewBundleGrouping) && String.IsNullOrEmpty(BundleProductCode))
                result = false;

            if (OriginalBundlePrice.HasValue)
                if (OriginalBundlePrice < 0)
                    result = false;

            if (DiscountedBundlePrice.HasValue)
            {
                if (DiscountedBundlePrice < 0)
                    result = false;

                if (DiscountedBundlePrice >= OriginalBundlePrice)
                    result = false;
            }

            if (OriginalComponentPrice.HasValue)
                if (OriginalComponentPrice < 0)
                        return false;

            if (DiscountedComponentPrice.HasValue)
            {
                if (DiscountedComponentPrice < 0)
                    return false;

                if (DiscountedComponentPrice >= OriginalComponentPrice)
                    return false;
            }

            return result;
        }
    }
}