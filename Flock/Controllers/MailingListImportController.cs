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
    public class MailingListImportController : Controller
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
        // GET: /MailingListImport/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("UploadControl", 
                MailingListImportControllerUploadControlSettings.UploadControlValidationSettings,
                MailingListImportControllerUploadControlSettings.FileUploadComplete);

            return null;
        }

        public ActionResult Merge(string FileName, short CountryID, byte LanguageID, byte DotDigitalAccountID, short? ThirdPartyID, bool SendWelcomeEmail)
        {
            var excel = new ExcelQueryFactory();

            excel.FileName = FileName;

            Flock.Util.RegexUtilities reg = new Flock.Util.RegexUtilities();

            var allRecords = (from x in excel.Worksheet<MailingListExcelRow>()
                             select x).ToList();

            var records = (from x in excel.Worksheet<MailingListExcelRow>()
                          select x).ToList();

            records = records.Where(p => reg.IsValidEmail(p.EMAIL) == true).ToList();

            var count = excel.Worksheet<MailingListExcelRow>().Count();

            UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

            var Model = new Flock.ViewModels.MailingListMergeResultViewModel();

            Model.InsertedRecords = records;
            Model.AllRecords = allRecords;
            var ret = UoW.Context.Member_Import(records, CountryID, LanguageID, DotDigitalAccountID, ThirdPartyID, SendWelcomeEmail);

            List<MailingListExcelRow> Returned = ret.AsEnumerable().Select(p => new MailingListExcelRow { EMAIL = p.Field<string>("Email") }).ToList();
            Model.Returned = Returned;

            return PartialView("MergeResultsPanelPartial", Model);
        }

        public ActionResult CountryComboBoxPartial()
        {
            return PartialView();
        }
        public ActionResult LanguageComboBoxPartial()
        {
            return PartialView();
        }
        public ActionResult DotDigitalAccountComboBoxPartial()
        {
            return PartialView();
        }
        public ActionResult ThirdPartyComboBoxPartial()
        {
            return PartialView();
        }
	}

    public class MailingListImportControllerUploadControlSettings
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
                    string fileName = System.Web.HttpRuntime.AppDomainAppPath + "//UploadedMailingLists//" + e.UploadedFile.FileName + "-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";

                    e.UploadedFile.SaveAs(fileName);

                    var excel = new ExcelQueryFactory();

                    excel.FileName = fileName;

                    var records = from x in excel.Worksheet<MailingListExcelRow>()
                                  select x;

                    var Model = new Flock.ViewModels.MailingListImportResultViewModel();
                    Model.ErrorTexts = null;
                    Model.UploadedRows = records.ToList();
                    Model.FileName = fileName;

                    e.CallbackData = RenderViewToString(CreateController<EmptyController>().ControllerContext, "~/Views/MailingListImport/UploadResultsPanelPartial.cshtml", Model, true);
                }
            }
            catch (Exception ex)
            {
                var Model = new Flock.ViewModels.MailingListImportResultViewModel();
                e.ErrorText = ex.ToString();
                Model.ErrorTexts = new string[1] { e.ErrorText };
                Model.UploadedRows = null;
                e.CallbackData = RenderViewToString(CreateController<EmptyController>().ControllerContext, "~/Views/MailingListImport/UploadResultsPanelPartial.cshtml", Model, true);
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

    public class MailingListExcelRow
    {
        public string EMAIL { get; set; }
        public string FULLNAME { get; set; }
    }
}