using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using NLog;
using System.Drawing;
using System.Web.UI;
using System.IO;
using Flock.DAL;
using Flock.Models;
using Flock.Util;
using Flock.ViewModels;
using System.Web.Script.Serialization;
using System.Printing;
using Microsoft.Office.Interop.Word;
using DevExpress.Utils;
using OpenXmlPowerTools;

namespace Flock.Controllers
{
    public partial class AssetsController : Controller
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
        // GET: /Assets/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Brochures(short? id)
        {
            if (id != null)
            {
                BrochureViewModel BVM = null;
                if (UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == id && p.DeletedDT == null).SingleOrDefault() != null)
                {
                    BrochureViewModelBuilder Builder = new BrochureViewModelBuilder(UnitOfWork);
                    BVM = Builder.Build(id);
                }
                else
                {
                    BVM = new BrochureViewModel();
                    ViewData["InvalidBrochureIDError"] = "Invalid Brochure ID, or the brochure is marked as deleted. Deleted brochures can be retrieved.";
                }
                return View("BrochureManager", BVM);
            }
            else
            {
                BrochureViewModel BVM = new BrochureViewModel();
                return View("BrochureManager", BVM);
            }
        }

        public ActionResult BrochureRequests()
        {
            return View();
        }

        public ActionResult BrochureRequestComboBoxPartial()
        {
            return PartialView("_BrochureRequestComboBoxPartial");
        }

        [ValidateInput(false)]
        public ActionResult BrochureRequestGridViewPartial(short BrochureID)
        {
            return PartialView("_BrochureRequestGridViewPartial", BrochureID);
        }

        public ActionResult GenerateBrochureRequestLabels(string keys, short brochureID)
        {
            try
            {
                Brochure b = UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == brochureID).Single();

                string mergedFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Brochures\\LabelTemplate\\" + b.Name.Replace(" ", "-") + "-" 
                    + DateTime.Now.ToString("dd-MM-yyyy") + ".docx";

                List<int> requests = keys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

                int countRequests = requests.Count();

                int countPages = (countRequests / 21) + ((countRequests % 21) == 0 ? 0 : 1);

                List<string> fileNames = new List<string>();

                for (int i = 0; i < countPages; i++)
                {
                    string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\Brochures\\LabelTemplate\\" + b.Name.Replace(" ", "-") + "-" 
                        + DateTime.Now.ToString("dd-MM-yyyy") + "-page-" + (i + 1).ToString() + ".docx";

                    fileNames.Add(fileName);

                    var thisSet = requests.Skip(i * 21).Take(21).ToList();

                    CreateDocument(labelTemplateFile, fileName, thisSet);
                }

                var sources = new List<OpenXmlPowerTools.Source>();
                //Document Streams (File Streams) of the documents to be merged.
                foreach (var f in fileNames)
                {
                    sources.Add(new OpenXmlPowerTools.Source(new OpenXmlPowerTools.WmlDocument(f), true));
                }

                var mergedDoc = DocumentBuilder.BuildDocument(sources);
                mergedDoc.SaveAs(mergedFileName);

                FileStream file = new FileStream(mergedFileName, FileMode.Open, FileAccess.Read);

                return File(mergedFileName, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", b.Name.Replace(" ", "-") + "-" + DateTime.Now.ToString("dd-MM-yyyy") + ".docx");
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        public ActionResult GenerateBrochureRequestXlsx(string keys, short brochureID)
        {
            try
            {
                Brochure b = UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == brochureID).Single();

                string resultantFileName = b.Name.Replace(" ", "-") + "-"
                    + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";

                string resultantFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Brochures\\LabelTemplate\\" + resultantFileName;

                List<int> keyList = keys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

                List<Member_Brochure> requests = UnitOfWork.GetRepository<Member_Brochure>().GetAll().InRange(p => p.Member_Brochure_ID, 2000, keyList).ToList();


                List<Country> countries = UnitOfWork.GetRepository<Country>().GetAll().Where(p => requests.Select(r => r.CountryID).Distinct().Contains(p.CountryID)).Distinct().ToList();

                OpenXmlHelper.CreateBrochureRequestSpreadsheet(resultantFilePath, resultantFileName, requests, countries);

                FileStream file = new FileStream(resultantFilePath, FileMode.Open, FileAccess.Read);

                foreach (var m in requests)
                {
                    if (m.ProcessedDT == null)
                        m.ProcessedDT = DateTime.Now;
                }

                UnitOfWork.Save();

                return File(resultantFilePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", resultantFileName);
            
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        public ActionResult Images(long? id)
        {
            if (id != null)
            {
                if (UnitOfWork.GetRepository<Flock.Models.Image>().GetAll().Where(p => p.ImageID == id && p.DeletedDT == null).SingleOrDefault() != null)
                {
                    return View("ImageManager", new ImageManagerViewModel() { ImageID = id });
                }
                else
                {
                    ViewData["InvalidImageIDError"] = "Invalid Image ID, or the page is marked as deleted. Deleted pages can be retrieved.";
                    return View("ImageManager", new ImageManagerViewModel());
                }
            }
            else
            {
                return View("ImageManager", new ImageManagerViewModel());
            }
        }

        public ActionResult LoadImageSwap(long? ImageID)
        {
            vwImage Model = null;
            try
            {
                Model = UnitOfWork.GetRepository<vwImage>().GetAll().Where(p => p.ImageID == ImageID).Single();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
            return PartialView("ImageSwapPartial", Model);
        }

        [HttpPost]
        public ActionResult ImageSwapPartial(long ImageID)
        {
            vwImage Model = null;
            try {
                Model = UnitOfWork.GetRepository<vwImage>().GetAll().Where(p => p.ImageID == ImageID).Single();
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
            return PartialView(Model);
        }

        public ActionResult Videos()
        {
            return View("VideoManager");
        }

        [ValidateInput(false)]
        public ActionResult VideoFileManagerPartial()
        {
            return PartialView("_VideoFileManagerPartial", AssetsControllerVideoFileManagerSettings.Model);
        }

        public FileStreamResult VideoFileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles("VideoFileManager", AssetsControllerVideoFileManagerSettings.Model);
        }

        [ValidateInput(false)]
        public ActionResult BrochureFileManagerPartial()
        {
            return PartialView("_BrochureFileManagerPartial", AssetsControllerBrochureFileManagerSettings.Model);
        }

        public FileStreamResult BrochureFileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles("BrochureFileManager", AssetsControllerBrochureFileManagerSettings.Model);
        }

        //[HttpPost]
        //public ActionResult BrochureDetailsPartial(short? BrochureID)
        //{
        //    return PartialView("BrochureDetailsPartial");
        //}

        [HttpPost]
        public ActionResult BrochureUploaded(string FileName)
        {
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Brochures/" + FileName))
            {
                try
                {
                    FileStream stream = System.IO.File.Open(AppDomain.CurrentDomain.BaseDirectory + "Brochures\\" + FileName, FileMode.Open);
                    Brochure brochure = new Brochure()
                    {
                        Name = FileName,
                        Description = "",
                        CoverImagePath = "",
                        FileSizeMB = (short)((stream.Length / 1024) / 1024),
                        BrandID = 2,
                        LaunchID = 1,
                        CreatedDT = DateTime.Now,
                        ModifiedDT = DateTime.Now
                    };
                    stream.Close();
                    stream.Dispose();
                    UnitOfWork.GetRepository<Brochure>().Add(brochure);
                    UnitOfWork.Save();

                    return Content("");
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 400;
                    Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                    return Content("Error: " + ex.Message);
                }
            }
            else
            {
                Response.StatusCode = 400;
                return Content("Error: File does not exist.");
            }
        }

        [HttpPost]
        public ActionResult BrochureDetailsPartial(short ID)
        {
            try
            {
                BrochureDetailsViewModelBuilder Builder = new BrochureDetailsViewModelBuilder(UnitOfWork);
                var Model = Builder.Build(ID);
                return PartialView("BrochureDetailsPartial", Model);
            }
            catch(Exception ex)
            {
                Response.StatusDescription = ex.Message;
                return PartialView("BrochureDetailsPartial");
            }
        }

        public ActionResult BrochureDetailsBrandDropDownPartial(byte? SelectedBrandID)
        {
            var Model = UnitOfWork.GetRepository<Brand>().GetAll().ToList();

            ViewBag.BrochureDetailsSelectedBrandID = SelectedBrandID;

            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        public ActionResult BrochureDetailsLaunchDropDownPartial(short? SelectedLaunchID)
        {
            var Model = UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();

            ViewBag.BrochureDetailsSelectedLaunchID = SelectedLaunchID;

            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }
        /*
                @Html.DevExpress().LabelFor(m => Model.BrandID, settings => { settings.Text = "Brand ID"; }).GetHtml()
                @Html.Action("BrochureDetailsBrandDropDownPartial", new { SelectedBrandID = Model.BrandID })

                @Html.DevExpress().LabelFor(m => Model.LaunchID, settings => { settings.Text = "Launch ID"; }).GetHtml()
                @Html.Action("BrochureDetailsLaunchDropDownPartial", new { SelectedLaunchID = Model.LaunchID })
         */

        [AllowAnonymous]
        [HttpPost]
        public JsonResult BrochureUpload(HttpPostedFileBase file)
        {
            List<string> allowedExtensions = new List<string>() { ".pdf" };
            if (file == null)
            {
                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();
                jsresult.files[0].name = "error";
                jsresult.files[0].size = 0;
                jsresult.files[0].url = "";
                jsresult.files[0].thumbnailUrl = "";
                jsresult.files[0].deleteUrl = "";
                jsresult.files[0].deleteType = "delete";
                jsresult.files[0].error = "No file uploaded";
                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
            else if (!allowedExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                JSResult jsresult = new JSResult();
                jsresult.files = new JSFileResult[1];
                jsresult.files[0] = new JSFileResult();
                jsresult.files[0].name = "error";
                jsresult.files[0].size = 0;
                jsresult.files[0].url = "";
                jsresult.files[0].thumbnailUrl = "";
                jsresult.files[0].deleteUrl = "";
                jsresult.files[0].deleteType = "delete";
                jsresult.files[0].error = "Invalid file type";
                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.Name == file.FileName).SingleOrDefault() == null)
                {
                    Brochure brochure = new Brochure()
                    {
                        Name = file.FileName,
                        Description = "",
                        CoverImagePath = "",
                        FileSizeMB = (short)((file.ContentLength / 1024) / 1024),
                        BrandID = 2,
                        LaunchID = 1,
                        CreatedDT = DateTime.Now,
                        ModifiedDT = DateTime.Now
                    };
                    try { 
                        UnitOfWork.GetRepository<Brochure>().Add(brochure);
                        UnitOfWork.Save();
                    }
                    catch(System.Data.SqlClient.SqlException e)
                    {
                        JSResult err = new JSResult();
                        err.files = new JSFileResult[1];
                        err.files[0] = new JSFileResult();
                        err.files[0].name = "error";
                        err.files[0].error = e.Message;
                        err.files[0].size = 0;
                        err.files[0].url = "";
                        err.files[0].thumbnailUrl = "";
                        err.files[0].deleteUrl = "";
                        err.files[0].deleteType = "DELETE";
                        return Json(err, JsonRequestBehavior.AllowGet);
                    }

                    using (var fileStream = System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Brochures\\" + brochure.BrochureID + System.IO.Path.GetExtension(file.FileName)))
                    {
                        file.InputStream.CopyTo(fileStream);
                    }

                    JSResult jsresult = new JSResult();
                    jsresult.files = new JSFileResult[1];
                    jsresult.files[0] = new JSFileResult();
                    jsresult.files[0].name = file.FileName;
                    jsresult.files[0].size = file.InputStream.Length;
                    jsresult.files[0].url = "/Brochures/" + file.FileName;
                    jsresult.files[0].thumbnailUrl = "/Brochures/" + file.FileName;
                    jsresult.files[0].deleteUrl = "/deletetest";
                    jsresult.files[0].deleteType = "DELETE";
                    return Json(jsresult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    JSResult jsresult = new JSResult();
                    jsresult.files = new JSFileResult[1];
                    jsresult.files[0] = new JSFileResult();
                    jsresult.files[0].name = "error";
                    jsresult.files[0].size = 0;
                    jsresult.files[0].url = "";
                    jsresult.files[0].thumbnailUrl = "";
                    jsresult.files[0].deleteUrl = "";
                    jsresult.files[0].deleteType = "delete";
                    jsresult.files[0].error = "File already exists. To replace the brochure, update the file's record.";
                    return Json(jsresult, JsonRequestBehavior.AllowGet);
                }
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


        [ValidateInput(false)]
        public ActionResult BrochureGridViewPartial()
        {
            var Model = UnitOfWork.GetRepository<vwBrochureOverview>().GetAll().OrderByDescending(p => p.BrochureID).ToList();
            return PartialView("_BrochureGridViewPartial", Model);
        }
        
        [HttpPost]
        public ActionResult ChangeBrochure(short BrochureID)
        {
            var Builder = new BrochureViewModelBuilder(UnitOfWork);
            var Model = Builder.Build(BrochureID);

            return PartialView("BrochureSettingsPartial", Model);
        }

        public ActionResult BrochureGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]BrochureViewModel item)
        {
            if (item.OffDate != null && item.OnDate == null)
            {
                ModelState.AddModelError("OnDate", "Please set an On Date for this form");
            }
            if (item.OnDate != null && item.OffDate != null)
            {
                if (item.OffDate <= item.OnDate)
                {
                    ModelState.AddModelError("OnDate", "On Date must precede Off Date.");
                    ModelState.AddModelError("OffDate", "Off Date date must not precede On Date.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Brochure brochure = new Brochure();
                    brochure.Name = item.Name;
                    brochure.BrandID = item.BrandID;
                    brochure.LaunchID = item.LaunchID;
                    brochure.CreatedDT = DateTime.Now;
                    brochure.ModifiedDT = DateTime.Now;
                    brochure.OnDate = item.OnDate;
                    brochure.OffDate = item.OffDate;
                    brochure.CoverImagePath = "";
                    brochure.PDFPath = "";
                    brochure.Code = "";
                    brochure.InteractiveBrochureURL = "";
                    UnitOfWork.GetRepository<Brochure>().Add(brochure);
                    UnitOfWork.Save();
                    UnitOfWork.Context.Brochure_Language_Initialise();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else 
            {
                ViewData["EditError"] = "Please, correct all errors.";
            }

            var Model = UnitOfWork.GetRepository<vwBrochureOverview>().GetAll().OrderByDescending(p => p.BrochureID).ToList();

            return PartialView("_BrochureGridViewPartial", Model);
        }

        public ActionResult CoverImageFileManagerPartial()
        {
            return PartialView();
        }

        public ActionResult InteractiveBrochureFileManagerPartial()
        {
            return PartialView();
        }

        public ActionResult PDFFileManagerPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult SaveBrochureDetails([ModelBinder(typeof(DevExpressEditorsBinder))]BrochureViewModel item, string ProductTypeIDs, string LanguageIDs)
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
            if(!hasError)
            { 
                if (ModelState.IsValid)
                {
                    try
                    {
                        Brochure brochure = UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == item.BrochureID).Single();
                        brochure.Name = item.Name;
                        brochure.InteractiveBrochureURL = item.InteractiveBrochureURL == null ? "" : item.InteractiveBrochureURL;
                        brochure.Description = item.Description == null ? "" : item.Description;
                        brochure.CoverImagePath = item.CoverImagePath == null ? "" : item.CoverImagePath;
                        brochure.PDFPath = item.PDFPath == null ? "" : item.PDFPath;
                        brochure.Code = item.Code;
                        //FileSizeMB = (short)((file.ContentLength / 1024) / 1024),
                        brochure.BrandID = item.BrandID;
                        brochure.LaunchID = item.LaunchID;
                        brochure.ModifiedDT = DateTime.Now;
                        brochure.OnDate = item.OnDate;
                        brochure.OffDate = item.OffDate;

                        UnitOfWork.Save();

                        if(ProductTypeIDs != null && ProductTypeIDs != "")
                        {
                            List<Brochure_ProductType> Current = UnitOfWork.GetRepository<Brochure_ProductType>().GetAll().Where(p => p.BrochureID == item.BrochureID).ToList();
                            foreach(Brochure_ProductType bpt in Current)
                            {
                                UnitOfWork.GetRepository<Brochure_ProductType>().Delete(bpt);
                            }
                            UnitOfWork.Save();

                            List<byte> ProductTypes = ProductTypeIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToList();
                            foreach(byte pt in ProductTypes)
                            {
                                UnitOfWork.GetRepository<Brochure_ProductType>().Add(new Brochure_ProductType() { BrochureID = item.BrochureID.Value, ProductTypeID = pt });
                            }
                            UnitOfWork.Save();
                        }

                        if (LanguageIDs != null && LanguageIDs != "")
                        {
                            List<Brochure_Language> Current = UnitOfWork.GetRepository<Brochure_Language>().GetAll().Where(p => p.BrochureID == item.BrochureID).ToList();
                            foreach (Brochure_Language bpt in Current)
                            {
                                UnitOfWork.GetRepository<Brochure_Language>().Delete(bpt);
                            }
                            UnitOfWork.Save();

                            List<byte> Languages = LanguageIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToList();
                            foreach (byte l in Languages)
                            {
                                UnitOfWork.GetRepository<Brochure_Language>().Add(new Brochure_Language() { BrochureID = item.BrochureID.Value, LanguageID = l });
                            }
                            UnitOfWork.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                        ViewData["EditError"] = ex.Message;
                        Response.StatusCode = 400;
                    }
                }
                else
                {
                    Response.StatusCode = 400;
                    ViewData["EditError"] = "Please, correct all errors.";

                    return PartialView("BrochureSettingsPartial", item);
                }
            }
            else 
            {
                Response.StatusCode = 400;
                ViewData["EditError"] = "Please, correct all errors.";
            }
            BrochureViewModelBuilder Builder = new BrochureViewModelBuilder(UnitOfWork);
            BrochureViewModel Model = Builder.Build(item.BrochureID);

            return PartialView("BrochureSettingsPartial", Model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BrochureGridViewPartialDelete(System.Int16 BrochureID)
        {
            var item = UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == BrochureID).SingleOrDefault();

            try
            {
                item.DeletedDT = DateTime.Now;

                UnitOfWork.Save();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            var Model = UnitOfWork.GetRepository<vwBrochureOverview>().GetAll().OrderByDescending(p => p.BrochureID).ToList();

            return PartialView("_BrochureGridViewPartial", Model);
        }

        public FileResult DownloadBrochure(short BrochureID)
        {
            string filename = "/Brochures/"+BrochureID.ToString()+".pdf";
            string contentType = "application/pdf";
            return File(filename, contentType, filename);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateBrochureCountryRestrictions(short BrochureID, string Countries)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> CountryRestrictions = ser.Deserialize<List<SortDatum>>(Countries);

            List<Brochure_Country> Current = UnitOfWork.GetRepository<Brochure_Country>().GetAll().Where(p => p.BrochureID == BrochureID).ToList();
            List<SortDatum> CurrentCountries = new List<SortDatum>();
            foreach (Brochure_Country country in Current)
            {
                CurrentCountries.Add(new SortDatum() { ID = country.CountryID, SortOrder = 0 });
            }
            List<SortDatum> Removals = CurrentCountries.Except(CountryRestrictions).ToList();

            foreach (SortDatum datum in Removals)
            {
                Brochure_Country remove = UnitOfWork.GetRepository<Brochure_Country>().GetAll().Where(p => p.BrochureID == BrochureID && p.CountryID == datum.ID).Single();
                UnitOfWork.GetRepository<Brochure_Country>().Delete(remove);
            }
            UnitOfWork.Save();

            //could probably do deletion from here too: if ProductPage_Country table holds record which isn't in Countries passed to this function, then delete those.

            foreach (SortDatum datum in CountryRestrictions)
            {
                Brochure_Country ppc = UnitOfWork.GetRepository<Brochure_Country>().GetAll().Where(p => p.BrochureID == BrochureID && p.CountryID == datum.ID).SingleOrDefault();

                if (ppc == null)
                {
                    ppc = new Brochure_Country() { BrochureID = BrochureID, CountryID = (short)datum.ID };
                    UnitOfWork.GetRepository<Brochure_Country>().Add(ppc);
                }
            }

            UnitOfWork.Save();

            List<short> Linker = UnitOfWork.GetRepository<Brochure_Country>().GetAll().Where(p => p.BrochureID == BrochureID).Select(p => p.CountryID).ToList();
            List<Country> Model = UnitOfWork.GetRepository<Country>().GetAll().Where(p => Linker.Contains(p.CountryID)).ToList();
            return PartialView("BrochureCountryRestrictionsPartial", Model);
        }

        public ActionResult ImageInfo()
        {
            return View("ImageInfoHub");
        }

        public ActionResult ImageInfoHubGridPartial()
        {
            return PartialView();
        }

        public ActionResult ImageInfoGridViewBatchUpdate(MVCxGridViewBatchUpdateValues<vwImage, long> updateValues)
        {
            foreach (var image in updateValues.Update)
            {
                if (updateValues.IsValid(image))
                {
                    Flock.Models.Image i = UnitOfWork.GetRepository<Flock.Models.Image>().GetAll().Where(q => q.ImageID == image.ImageID).Single();
                    if (image.TitleText != null)
                        i.TitleText = image.TitleText;
                    if (image.AltText != null)
                        i.AltText = image.AltText;
                }

                UnitOfWork.Save();
            }
            
            foreach (var image in updateValues.DeleteKeys)
            {
                Flock.Models.Image i = UnitOfWork.GetRepository<Flock.Models.Image>().GetAll().Where(p => p.ImageID == image).Single();

                IMultipleResults mResults = UnitOfWork.Context.Image_Usage(image);
                var Products = mResults.GetResult<ImageUsage_Product>().ToList();
                var ProductPages = mResults.GetResult<ImageUsage_ProductPage>().ToList();
                var Pages = mResults.GetResult<ImageUsage_Page>().ToList();
                var Slideshows = mResults.GetResult<ImageUsage_Slideshow>().ToList();
                var Collections = mResults.GetResult<ImageUsage_Collection>().ToList();
                var Articles = mResults.GetResult<ImageUsage_Article>().ToList();
                var Animations = mResults.GetResult<ImageUsage_Animation>().ToList();

                if (Products.Count == 0 && 
                    ProductPages.Count == 0 && 
                    Pages.Count == 0 && 
                    Slideshows.Count == 0 && 
                    Collections.Count == 0 && 
                    Articles.Count == 0 &&
                    Animations.Count == 0)
                {
                    UnitOfWork.Context.Image_Delete(image);
                    return PartialView("ImageInfoHubGridPartial");
                }
                else
                {
                    updateValues.SetErrorText(image, "Could not delete ImageID: " + image.ToString() + " because it is currently associated to one or more entities.\r\nPlease inspect the Image Usage tab.");
                }
            }

            return PartialView("ImageInfoHubGridPartial");
        }

        public ActionResult LoadImageUsage(long? ImageID)
        {
            IMultipleResults Results = UnitOfWork.Context.Image_Usage(ImageID.Value);

            return PartialView("ImageUsagePartial", Results);
        }

        [HttpPost]
        public ActionResult ImageUsagePartial(long ImageID)
        {
            IMultipleResults Results = UnitOfWork.Context.Image_Usage(ImageID);

            return PartialView("ImageUsagePartial", Results);
        }

        public ActionResult LoadImageInspector(long? ImageID)
        {
            IMultipleResults Results = UnitOfWork.Context.Image_Inspect(ImageID.Value);

            return PartialView("ImageInspectPartial", Results);
        }

        [HttpPost]
        public ActionResult InspectImage(long ImageID)
        {
            IMultipleResults Results = UnitOfWork.Context.Image_Inspect(ImageID);

            return PartialView("ImageInspectPartial", Results);
        }

        public ActionResult ImageInfoGridView()
        {
            return PartialView("ImageInfoHubGridPartial");
        }

        [HttpPost]
        public ActionResult ImageInfoGridViewDelete(vwImage item)
        {
            Flock.Models.Image i = UnitOfWork.GetRepository<Flock.Models.Image>().GetAll().Where(p => p.ImageID == item.ImageID).Single();

            IMultipleResults mResults = UnitOfWork.Context.Image_Usage(item.ImageID);
            var Products = mResults.GetResult<ImageUsage_Product>().ToList();
            var ProductPages = mResults.GetResult<ImageUsage_ProductPage>().ToList();
            var Pages = mResults.GetResult<ImageUsage_Page>().ToList();
            var Slideshows = mResults.GetResult<ImageUsage_Slideshow>().ToList();
            var Collections = mResults.GetResult<ImageUsage_Collection>().ToList();
            var Articles = mResults.GetResult<ImageUsage_Article>().ToList();
            
            if(Products.Count == 0 && ProductPages.Count == 0 && Pages.Count == 0 && Slideshows.Count == 0 && Collections.Count == 0 && Articles.Count == 0)
            {
                UnitOfWork.Context.Image_Delete(item.ImageID);
                return PartialView("ImageInfoHubGridPartial");
            }
            else
            {
                Response.StatusCode = 403;
                return Content("Could not delete ImageID: " + item.ImageID.ToString() + " because it is currently associated to one or more entities.\r\nPlease inspect the Image Usage tab.");
            }
            //if (UnitOfWork.Context.Image_Delete(item.ImageID).Single().Column1 == true)
            //    return PartialView("ImageInfoHubGridPartial");
            //else
            //{
            //}
        }


        /// <summary>
        /// Messages section
        /// </summary>
        /// <returns></returns>

        public ActionResult Messages(short? id)
        {
            if (id != null)
            {
                MessageViewModel MVM = null;
                if (UnitOfWork.GetRepository<Message>().GetAll().Where(p => p.MessageID == id && p.DeletedDT == null).SingleOrDefault() != null)
                {
                    MessageViewModelBuilder Builder = new MessageViewModelBuilder(UnitOfWork);
                    MVM = Builder.Build(id);
                }
                else
                {
                    MVM = new MessageViewModel();
                    ViewData["InvalidMessageIDError"] = "Invalid Message ID, or the message is marked as deleted. Deleted messages can be retrieved.";
                }
                return View("Messages", MVM);
            }
            else
            {
                MessageViewModel MVM = new MessageViewModel();
                return View("Messages", MVM);
            }
        }

        public ActionResult MessagesGridViewPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ChangeMessage(short? MessageID)
        {
            var Model = new MessageViewModelBuilder(UnitOfWork).Build(MessageID);

            return PartialView("MessageComposePartial", Model);
        }

        public ActionResult MessagesGridViewPartialAddNew(MessageViewModel item)
        {
            item.CreatedDT = DateTime.Now;
           
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

            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    Message m = new Message();
                    m.PriceListID = item.PriceListID;
                    m.LanguageID = item.LanguageID;
                    m.Threshold = item.Threshold;
                    m.BelowThresholdMessage = item.BelowThresholdMessage;
                    m.AboveThresholdMessage = item.AboveThresholdMessage;
                    m.NoThresholdMessage = item.NoThresholdMessage;
                    m.OnDate = item.OnDate;
                    m.OffDate = item.OffDate;
                    m.CreatedDT = item.CreatedDT;
                    m.TradeSwitch = item.TradeSwitch;
                    UnitOfWork.GetRepository<Message>().Add(m);
                    UnitOfWork.Save();
                }
            }

            return PartialView("MessagesGridViewPartial");
        }

        public ActionResult MessagesGridViewPartialUpdate(MessageViewModel item)
        {
            return PartialView("MessagesGridViewPartial");
        }

        public ActionResult MessagesGridViewPartialDelete(MessageViewModel item)
        {
            Message m = UnitOfWork.GetRepository<Message>().GetAll().Where(p => p.MessageID == item.MessageID).Single();
            m.DeletedDT = DateTime.Now;
            UnitOfWork.Save();
            return PartialView("MessagesGridViewPartial");
        }

        public ActionResult MessagesGridViewPartialCustomCommand(string commandType, string keyString, string dateString, bool? newState)
        {
            string errorString = "";
            bool hasError = false;
            List<short> keys = new List<short>();
            if(keyString != null && keyString != "")
                keys = keyString.Split(',').ToList().ConvertAll(d => Convert.ToInt16(d));

            DateTime date = DateTime.Now;

            if(dateString != null && dateString != "")
                date = DateTime.Parse(dateString);

            switch (commandType.ToLower())
            {
                case "setondate":
                    try
                    {
                        List<Message> toEnable = UnitOfWork.GetRepository<Message>().GetAll().Where(p => keys.Contains(p.MessageID)).ToList();
                        foreach (Message c in toEnable)
                        {
                            c.OnDate = DateTime.Parse(dateString);

                            if(c.OffDate != null)
                            {
                                if (date < c.OffDate || c.OffDate == null)
                                    c.OnDate = date;
                                else
                                {
                                    errorString += "Message " + c.MessageID.ToString() + " was not updated because the provided On Date falls after the message's assigned Off Date.\r\n\r\n";
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
                        //log e
                        hasError = true;
                    }
                    break;
                case "setoffdate":
                    try
                    {
                        List<Message> toDisable = UnitOfWork.GetRepository<Message>().GetAll().Where(p => keys.Contains(p.MessageID)).ToList();
                        foreach (Message c in toDisable)
                        {
                            if (date >= c.OnDate || c.OnDate == null)
                                c.OffDate = date;
                            else
                            {
                                errorString += "Message " + c.MessageID.ToString() + " was not updated because the provided Off Date preceeds the message's assigned On Date.\r\n\r\n";
                                hasError = true;
                            }
                        }
                        
                        UnitOfWork.Save();
                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] = e.Message;
                        //log e
                        hasError = true;
                    }
                    break;
                case "delete":
                    try
                    {
                        List<Message> toDelete = UnitOfWork.GetRepository<Message>().GetAll().Where(p => keys.Contains(p.MessageID)).ToList();
                        foreach (Message c in toDelete)
                            c.DeletedDT = DateTime.Now;
                        UnitOfWork.Save();
                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] = e.Message;
                        // log e
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

            return PartialView("MessagesGridViewPartial");
        }

        [HttpPost]
        public ActionResult UpdateMessageCountryRestrictions(short MessageID, string Countries)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> CountryRestrictions = ser.Deserialize<List<SortDatum>>(Countries);

            List<Message_Country> Current = UnitOfWork.GetRepository<Message_Country>().GetAll().Where(p => p.MessageID == MessageID).ToList();
            List<SortDatum> CurrentCountries = new List<SortDatum>();
            foreach (Message_Country country in Current)
            {
                CurrentCountries.Add(new SortDatum() { ID = country.CountryID, SortOrder = 0 });
            }
            List<SortDatum> Removals = CurrentCountries.Except(CountryRestrictions).ToList();

            foreach (SortDatum datum in Removals)
            {
                Message_Country remove = UnitOfWork.GetRepository<Message_Country>().GetAll().Where(p => p.MessageID == MessageID && p.CountryID == datum.ID).Single();
                UnitOfWork.GetRepository<Message_Country>().Delete(remove);
            }
            UnitOfWork.Save();

            //could probably do deletion from here too: if ProductPage_Country table holds record which isn't in Countries passed to this function, then delete those.

            foreach (SortDatum datum in CountryRestrictions)
            {
                Message_Country ppc = UnitOfWork.GetRepository<Message_Country>().GetAll().Where(p => p.MessageID == MessageID && p.CountryID == datum.ID).SingleOrDefault();

                if (ppc == null)
                {
                    ppc = new Message_Country() { MessageID = MessageID, CountryID = (short)datum.ID };
                    UnitOfWork.GetRepository<Message_Country>().Add(ppc);
                }
            }

            UnitOfWork.Save();

            List<short> Linker = UnitOfWork.GetRepository<Message_Country>().GetAll().Where(p => p.MessageID == MessageID).Select(p => p.CountryID).ToList();
            List<Country> Model = UnitOfWork.GetRepository<Country>().GetAll().Where(p => Linker.Contains(p.CountryID)).ToList();
            return PartialView("MessageCountryRestrictionsPartial", Model);
        }

        [HttpPost]
        public ActionResult SaveMessageDetails(MessageViewModel item)
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
            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Message message = UnitOfWork.GetRepository<Message>().GetAll().Where(p => p.MessageID == item.MessageID).Single();
                        message.PriceListID = item.PriceListID;
                        message.LanguageID = item.LanguageID;
                        message.Threshold = item.Threshold;
                        message.BelowThresholdMessage = item.BelowThresholdMessage;
                        message.AboveThresholdMessage = item.AboveThresholdMessage;
                        message.NoThresholdMessage = item.NoThresholdMessage;
                        message.OnDate = item.OnDate;
                        message.OffDate = item.OffDate;
                        message.TradeSwitch = item.TradeSwitch;

                        UnitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                        ViewData["EditError"] = ex.Message;
                        Response.StatusCode = 400;
                    }
                }
            }
            else
            {
                Response.StatusCode = 400;
                ViewData["EditError"] = "Please, correct all errors.";
            }

            //MessageViewModelBuilder Builder = new MessageViewModelBuilder(UnitOfWork);
            //MessageViewModel Model = Builder.Build(item.MessageID);

            return PartialView("MessageComposePartial", item);
        }

        /*
          public ActionResult SaveBrochureDetails(Flock.Models.vwBrochureOverview item, string ProductTypeIDs, string LanguageIDs)
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
            if(!hasError)
            { 
                if (ModelState.IsValid)
                {
                    try
                    {
                        Brochure brochure = UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == item.BrochureID).Single();
                        brochure.Name = item.Name;
                        brochure.InteractiveBrochureURL = item.InteractiveBrochureURL == null ? "" : item.InteractiveBrochureURL;
                        brochure.Description = item.Description == null ? "" : item.Description;
                        brochure.CoverImagePath = item.CoverImagePath == null ? "" : item.CoverImagePath;
                        brochure.PDFPath = item.PDFPath == null ? "" : item.PDFPath;
                        //FileSizeMB = (short)((file.ContentLength / 1024) / 1024),
                        brochure.BrandID = item.BrandID;
                        brochure.LaunchID = item.LaunchID;
                        brochure.ModifiedDT = DateTime.Now;
                        brochure.OnDate = item.OnDate;
                        brochure.OffDate = item.OffDate;

                        UnitOfWork.Save();

                        if(ProductTypeIDs != null && ProductTypeIDs != "")
                        {
                            List<Brochure_ProductType> Current = UnitOfWork.GetRepository<Brochure_ProductType>().GetAll().Where(p => p.BrochureID == item.BrochureID).ToList();
                            foreach(Brochure_ProductType bpt in Current)
                            {
                                UnitOfWork.GetRepository<Brochure_ProductType>().Delete(bpt);
                            }
                            UnitOfWork.Save();

                            List<byte> ProductTypes = ProductTypeIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToList();
                            foreach(byte pt in ProductTypes)
                            {
                                UnitOfWork.GetRepository<Brochure_ProductType>().Add(new Brochure_ProductType() { BrochureID = item.BrochureID, ProductTypeID = pt });
                            }
                            UnitOfWork.Save();
                        }

                        if (LanguageIDs != null && LanguageIDs != "")
                        {
                            List<Brochure_Language> Current = UnitOfWork.GetRepository<Brochure_Language>().GetAll().Where(p => p.BrochureID == item.BrochureID).ToList();
                            foreach (Brochure_Language bpt in Current)
                            {
                                UnitOfWork.GetRepository<Brochure_Language>().Delete(bpt);
                            }
                            UnitOfWork.Save();

                            List<byte> Languages = LanguageIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToList();
                            foreach (byte l in Languages)
                            {
                                UnitOfWork.GetRepository<Brochure_Language>().Add(new Brochure_Language() { BrochureID = item.BrochureID, LanguageID = l });
                            }
                            UnitOfWork.Save();
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.ErrorException("", e);
                        ViewData["EditError"] = e.Message;
                        Response.StatusCode = 400;
                    }
                }
            }
            else 
            {
                Response.StatusCode = 400;
                ViewData["EditError"] = "Please, correct all errors.";
            }
            BrochureViewModelBuilder Builder = new BrochureViewModelBuilder(UnitOfWork);
            BrochureViewModel Model = Builder.Build(item.BrochureID);

            return PartialView("BrochureSettingsPartial", Model);
        }

         */

        [ValidateInput(false)]
        public ActionResult VideoGridViewPartial()
        {
            var Model = UnitOfWork.GetRepository<vwVideoOverview>().GetAll().ToList();
            return PartialView("_VideoGridViewPartial", Model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult VideoGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]Flock.Models.vwVideoOverview item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime dt = DateTime.Today + item.RunningTime.TimeOfDay;

                    UnitOfWork.GetRepository<Video>().Add(new Video() { 
                        Name = item.Name,
                        Description = item.Description,
                        EmbedCode = item.EmbedCode,
                        RunningTime = dt,
                        HD = item.HD,
                        BrandID = item.BrandID,
                        LaunchID = item.LaunchID,
                        LanguageID = item.LanguageID,
                        CreatedDT = DateTime.Now,
                        ModifiedDT = DateTime.Now,
                        OnDate = item.OnDate,
                        OffDate = item.OffDate,
                        DeletedDT = null
                    });
                    UnitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var Model = UnitOfWork.GetRepository<vwVideoOverview>().GetAll().ToList();

            return PartialView("_VideoGridViewPartial", Model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult VideoGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))]Flock.Models.Video item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = UnitOfWork.GetRepository<Video>().GetAll().Where(p => p.VideoID == item.VideoID).SingleOrDefault();

                    if(modelItem != null)
                    {
                        modelItem.Name = item.Name;
                        modelItem.Description = item.Description;
                        modelItem.EmbedCode = item.EmbedCode;
                        modelItem.RunningTime = item.RunningTime;
                        modelItem.HD = item.HD;
                        modelItem.BrandID = item.BrandID;
                        modelItem.LaunchID = item.LaunchID;
                        modelItem.LanguageID = item.LanguageID;
                        modelItem.ModifiedDT = DateTime.Now;
                        modelItem.OnDate = item.OnDate;
                        modelItem.OffDate = item.OffDate;

                        UnitOfWork.Save();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var Model = UnitOfWork.GetRepository<vwVideoOverview>().GetAll().ToList();

            return PartialView("_VideoGridViewPartial", Model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult VideoGridViewPartialDelete(System.Int16 VideoID)
        {
            try
            {
                var modelItem = UnitOfWork.GetRepository<Video>().GetAll().Where(p => p.VideoID == VideoID).SingleOrDefault();
                if(modelItem != null)
                {
                    modelItem.DeletedDT = DateTime.Now;
                }
                UnitOfWork.Save();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            var Model = UnitOfWork.GetRepository<vwVideoOverview>().GetAll().ToList();

            return PartialView("_VideoGridViewPartial", Model);
        }
    }

    public class AssetsControllerBrochureFileManagerSettings
    {
        public const string RootFolder = @"~\Brochures";

        public static string Model { get { return RootFolder; } }
    }

    public class AssetsControllerVideoFileManagerSettings
    {
        public const string RootFolder = @"~\Videos";

        public static string Model { get { return RootFolder; } }
    }
}