using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NLog;
using DevExpress.Web;
using System.Web.UI;
using System.IO;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using WebPWrapper;

namespace Flock.Controllers
{
    public class BannersController : Controller
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
            return View("BannersOverview");
        }

        public ActionResult Compose(long? id)
        {
            var Model = new BannerComposeViewModel();
            if (id != null)
            {
                if (UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == id && p.DeletedDT == null).SingleOrDefault() != null)
                { 
                    Model = new BannerComposeViewModelBuilder(UnitOfWork).Build(id);
                }
                else
                {
                    ViewData["InvalidBannerIDError"] = "Invalid Banner ID, or the banner is marked as deleted. Deleted banners can be retrieved.";
                }
            }
            return View("BannersCompose", Model);
        }

        public ActionResult ChangeBanner(int ID)
        {
            return PartialView("BannerComposePartial", new BannerComposeViewModelBuilder(UnitOfWork).Build(ID));
        }

        public ActionResult BannerComposeGridSelectorPartial(string BannerLocationIDs, string SiteSectionIDs, string CountryIDs, string PageIDs)
        {
            short[] Countries = null;

            if (CountryIDs != null && CountryIDs != "")
                Countries = CountryIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToArray();

            byte[] Locations = null;

            if (BannerLocationIDs != null && BannerLocationIDs != "")
                Locations = BannerLocationIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            byte[] SiteSections = null;

            if (SiteSectionIDs != null && SiteSectionIDs != "")
                SiteSections = SiteSectionIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();

            int[] Pages = null;

            if (PageIDs != null && PageIDs != "")
                Pages = PageIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt32(p)).ToArray();

            return PartialView("BannerSelectGridViewPartial", new BannerSelectGridSettings() { BannerLocationIDs = Locations, CountryIDs = Countries, SiteSectionIDs = SiteSections, PageIDs = Pages });
        }

        [HttpPost]
        public ActionResult SaveBannerDetails(BannerComposeViewModel model)
        {
            bool hasError = false;
            if (model.OnDate == null && model.OffDate != null)
            {
                hasError = true;
                ModelState.AddModelError("OnDate", "Please set an On Date.");
            }
            if (model.OnDate != null && model.OffDate != null)
            {
                if (model.OffDate <= model.OnDate)
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
                    Banner b = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == model.BannerID).Single();
                    b.Name = model.Name;
                    b.FontClassID = model.FontClassID;
                    b.OnDate = model.OnDate;
                    b.OffDate = model.OffDate;
                    b.LanguageID = model.LanguageID;
                    b.TitleText = model.TitleText;
                    b.AltText = model.AltText;
                    b.OverlayText = model.OverlayText == null ? "" : model.OverlayText;
                    b.ModifiedDT = DateTime.Now;
                    b.ForegroundColour = model.ForegroundColour;
                    b.BackgroundColour = model.BackgroundColour;
                    b.TradeSwitch = model.TradeSwitch;

                    UnitOfWork.GetRepository<Banner_FontClass>().DeleteAll(UnitOfWork.GetRepository<Banner_FontClass>().GetAll().Where(p => p.BannerID == model.BannerID).ToList());

                    if (model.CssClassesToSave != null)
                    {
                        foreach (var i in model.CssClassesToSave)
                        {
                            UnitOfWork.GetRepository<Banner_FontClass>().Add(new Banner_FontClass() { BannerID = model.BannerID.Value, FontClassID = i });
                        }
                    }

                    UnitOfWork.Save();

                    model.CssClasses = b.Banner_FontClasses.ToList();

                    return PartialView("BannerDetailsPartial", model);
                }
                else
                {
                    Response.StatusCode = 400;
                    return PartialView("BannerDetailsPartial", model);
                }
            }
            else
            {
                Response.StatusCode = 400;
                return PartialView("BannerDetailsPartial", model);
            }
        }

        public ActionResult BannerOverviewBannerLocationDropDownPartial(byte? SelectedBannerLocationID)
        {
            List<BannerLocation> Model = UnitOfWork.GetRepository<BannerLocation>().GetAll().OrderBy(p => p.Name).ToList();
            List<BannerLocationGroup> Groups = UnitOfWork.GetRepository<BannerLocationGroup>().GetAll().ToList();
            foreach(var bl in Model)
            {
                var grp = Groups.Where(p => p.BannerLocationGroupID == bl.BannerLocationGroupID).SingleOrDefault();

                if(grp != null)
                {
                    bl.Name = grp.Name + " - " + bl.Name; 
                }
            }

            ViewBag.BannerOverviewSelectedBannerLocationID = SelectedBannerLocationID;
            return PartialView("~/Views/Shared/SearchFilters/BannerLocationDropDownPartial.cshtml", Model);
        }

        public ActionResult BannerOverviewSiteSectionDropDownPartial(byte? SelectedSiteSectionID)
        {
            List<SiteSection> Model = UnitOfWork.GetRepository<SiteSection>().GetAll().OrderBy(p => p.Name).ToList();
            ViewBag.BannerOverviewSelectedSiteSectionID = SelectedSiteSectionID;
            return PartialView("~/Views/Shared/SearchFilters/SiteSectionDropDownPartial.cshtml", Model);
        }

        public ActionResult BannerOverviewCountryDropDownPartial(short? SelectedCountryID)
        {
            List<Country> Model = UnitOfWork.GetRepository<Country>().GetAll().OrderBy(p => p.Name).ToList();
            ViewBag.BannerOverviewSelectedCountryID = SelectedCountryID;
            return PartialView("~/Views/Shared/SearchFilters/CountryDropDownPartial.cshtml", Model);
        }

        public ActionResult BannerOverviewLanguageDropDownPartial(short? SelectedLanguageID)
        {
            List<Language> Model = UnitOfWork.GetRepository<Language>().GetAll().OrderBy(p => p.Name).ToList();
            ViewBag.BannerOverviewSelectedLanguageID = SelectedLanguageID;
            return PartialView("~/Views/Shared/SearchFilters/LanguageDropDownPartial.cshtml", Model);
        }

        [ValidateInput(false)]
        public ActionResult BannerOverviewGridViewPartial(byte? BannerLocationID, byte? SiteSectionID, short? CountryID, bool? TradeSwitch) // languageID
        {
            List<vwBannerOverview> Model = UnitOfWork.GetRepository<vwBannerOverview>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            
            // build unique list of BannerIDs from the linker tables
            List<int> BannerLocations = new List<int>();
            List<int> BannerSiteSections = new List<int>();
            List<int> BannerCountries = new List<int>();

            if (BannerLocationID != null)
                BannerLocations = UnitOfWork.GetRepository<Banner_BannerLocation>().GetAll().Where(p => p.BannerLocationID == BannerLocationID).Select(p => p.BannerID).ToList();
            if (SiteSectionID != null)
                BannerSiteSections = UnitOfWork.GetRepository<Banner_SiteSection>().GetAll().Where(p => p.SiteSectionID == SiteSectionID).Select(p => p.BannerID).ToList();
            if (CountryID != null)
                BannerCountries = UnitOfWork.GetRepository<Banner_Country>().GetAll().Where(p => p.CountryID == CountryID).Select(p => p.BannerID).ToList();

            if (BannerLocationID != null)
                Model = Model.Where(p => BannerLocations.Contains(p.BannerID)).ToList();
            if(SiteSectionID != null)
                Model = Model.Where(p => BannerSiteSections.Contains(p.BannerID)).ToList();
            if(CountryID != null)
                Model = Model.Where(p => BannerCountries.Contains(p.BannerID)).ToList();
            if (TradeSwitch != null)
                Model = Model.Where(p => p.TradeSwitch == TradeSwitch).ToList();

            return PartialView("_BannerOverviewGridViewPartial", Model);
        }

        [AllowAnonymous]
        public ActionResult AddNew()
        {
            Banner banner = new Banner();
            banner.Name = "New Banner";
            banner.CreatedDT = DateTime.Now;
            banner.FontClassID = 1;
            banner.BackgroundColour = "#ffffff";
            banner.ForegroundColour = "#000000";
            banner.TradeSwitch = false;
            UnitOfWork.GetRepository<Banner>().Add(banner);
            UnitOfWork.Save();
            //bool hasError = false;
            //if (item.Name == null || item.Name == "")
            //{
            //    hasError = true;
            //    ViewData["EditError"] += "Please set a Name for this banner.<br/>";
            //}
            //if (item.LinkURL == null || item.LinkURL == "")
            //{
            //    hasError = true;
            //    ViewData["EditError"] += "Please set a Link for this banner.<br/>";
            //}
            //if (item.OnDate == null)
            //{
            //    hasError = true;
            //    ViewData["EditError"] += "Please set a Start Date for this banner.<br/>";
            //}
            //if (item.OffDate <= item.OnDate)
            //{
            //    hasError = true;
            //    ViewData["EditError"] += "Start Date must be before End Date.<br/>";
            //}
            //if (!hasError)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        try
            //        {
            //            Banner banner = new Banner();
            //            banner.Name = item.Name;
            //            banner.ExternalURL = item.LinkURL;
            //            banner.OnDate = item.OnDate;
            //            banner.OffDate = item.OffDate;
            //            banner.NewWindow = item.NewWindow;
            //            banner.CreatedDT = DateTime.Now;
            //            UnitOfWork.GetRepository<Banner>().Add(banner);
            //            UnitOfWork.Save();
            //        }
            //        catch (Exception e)
            //        {
            //            ViewData["EditError"] += e.Message;
            //        }
            //    }
            //    else
            //        ViewData["EditError"] += "Please, correct all errors.";
            //    List<vwBannerOverview> Model = UnitOfWork.GetRepository<vwBannerOverview>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            //    return PartialView("_BannerOverviewGridViewPartial", Model);
            //}
            //else
            //{
            //    List<vwBannerOverview> Model = UnitOfWork.GetRepository<vwBannerOverview>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            //    return PartialView("_BannerOverviewGridViewPartial", Model);
            //}

            return RedirectToAction("compose", new { id = banner.BannerID });
        }

        public ActionResult AddNewFromSelectGrid()
        {
            Banner banner = new Banner();
            banner.Name = "New Banner";
            banner.CreatedDT = DateTime.Now;
            banner.FontClassID = 1;
            banner.BackgroundColour = "#ffffff";
            banner.ForegroundColour = "#000000";
            banner.TradeSwitch = null;
            UnitOfWork.GetRepository<Banner>().Add(banner);
            UnitOfWork.Save();

            return Content("");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult BannerOverviewGridViewCustomCommand(string commandType, string keyString)
        {
            List<long> keys = keyString.Split(',').ToList().ConvertAll(d => Convert.ToInt64(d));

            switch (commandType)
            {
                case "Delete":
                    List<Banner> toDelete = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => keys.Contains(p.BannerID)).ToList();
                    for (int i = 0; i < toDelete.Count; i++)
                    {
                        Banner b = toDelete[i];
                        b.DeletedDT = DateTime.Now;
                        UnitOfWork.Save();
                    }
                    break;

                default: break;
            }

            List<vwBannerOverview> Model = UnitOfWork.GetRepository<vwBannerOverview>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            return PartialView("_BannerOverviewGridViewPartial", Model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BannerOverviewGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.Models.vwBannerOverview item)
        {
            bool hasError = false;
            if (item.Name == null || item.Name == "")
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Name for this banner.<br/>";
            }
            if (item.LinkURL == null || item.LinkURL == "")
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Link for this banner.<br/>";
            }
            if (item.OnDate == null)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Start Date for this banner.<br/>";
            }
            if (item.OffDate <= item.OnDate)
            {
                hasError = true;
                ViewData["EditError"] += "Start Date must be before End Date.<br/>";
            }
            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == item.BannerID).Single();
                        banner.Name = item.Name;
                        banner.ExternalURL = item.LinkURL == null ? null : item.LinkURL.ToLowerInvariant().TrimEnd(new[] { '/' });
                        banner.OnDate = item.OnDate;
                        banner.OffDate = item.OffDate;
                        banner.NewWindow = item.NewWindow;

                        UnitOfWork.Save();
                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] += e.Message;
                    }
                }
                else
                    ViewData["EditError"] += "Please, correct all errors.";

                List<vwBannerOverview> Model = UnitOfWork.GetRepository<vwBannerOverview>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
                return PartialView("_BannerOverviewGridViewPartial", Model);
            }
            else
            {
                List<vwBannerOverview> Model = UnitOfWork.GetRepository<vwBannerOverview>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
                return PartialView("_BannerOverviewGridViewPartial", Model);
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BannerOverviewGridViewPartialDelete(System.Int32 BannerID)
        {
            try
            {
                Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();
                banner.DeletedDT = DateTime.Now;
                
                UnitOfWork.Save();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            List<vwBannerOverview> Model = UnitOfWork.GetRepository<vwBannerOverview>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            return PartialView("_BannerOverviewGridViewPartial", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposeBannerHeaderBannerLocationDropDownPartial(byte? SelectedBannerLocationID)
        {
            List<BannerLocation> Model = UnitOfWork.GetRepository<BannerLocation>().GetAll().OrderBy(p => p.BannerLocationID).ToList();
            List<BannerLocationGroup> Groups = UnitOfWork.GetRepository<BannerLocationGroup>().GetAll().ToList();
            foreach (var bl in Model)
            {
                var grp = Groups.Where(p => p.BannerLocationGroupID == bl.BannerLocationGroupID).SingleOrDefault();

                if (grp != null)
                {
                    bl.Name = bl.BannerLocationID.ToString() + " - " + grp.Name + " - " + bl.Name;
                }
            }

            ViewBag.BannerSelectHeaderSelectedBannerLocationID = SelectedBannerLocationID;
            return PartialView("~/Views/Shared/SearchFilters/BannerLocationDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposeBannerHeaderBannerLocationComboBoxPartial(byte? SelectedBannerLocationID)
        {
            List<vwBannerLocation> Model = UnitOfWork.GetRepository<vwBannerLocation>().GetAll().OrderBy(p => p.BannerLocationGroupSortOrder).ToList();
            ViewBag.BannerSelectHeaderSelectedBannerLocationID = SelectedBannerLocationID;
            return PartialView("~/Views/Shared/SearchFilters/BannerLocationComboBoxPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposeBannerHeaderSiteSectionDropDownPartial(byte? SelectedSiteSectionID)
        {
            List<SiteSection> Model = UnitOfWork.GetRepository<SiteSection>().GetAll().OrderBy(p => p.Name).ToList();
            ViewBag.BannerSelectHeaderSelectedSiteSectionID = SelectedSiteSectionID;
            return PartialView("~/Views/Shared/SearchFilters/SiteSectionDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposeBannerHeaderCountryDropDownPartial(byte? SelectedCountryID)
        {
            List<Country> Model = UnitOfWork.GetRepository<Country>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CountryDropDownPartial.cshtml", Model);
        }

        public ActionResult ComposeBannerHeaderPageComboBoxPartial()
        {
            return PartialView("BannerComposeHeaderPageComboBoxPartial");
        }

        public ActionResult ComposeBannerHeaderTradeComboBoxPartial()
        {
            return PartialView("BannerComposeHeaderTradeComboBoxPartial");
        }

        public ActionResult ComposeBannerHeaderPageDropDownPartial()
        {
            List<Flock.Models.Page> Model = UnitOfWork.GetRepository<Flock.Models.Page>().GetAll().OrderBy(p => p.PageID).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PageDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ComposeBannerHeaderBannerDropDownPartial(short? SelectedBannerID, byte? SelectedBannerLocationID, byte? SelectedSiteSectionID, byte? SelectedCountryID, short? SelectedLanguageID, byte? SelectedStatusID)
        {
            List<vwBannerOverview> Model = UnitOfWork.GetRepository<vwBannerOverview>().GetAll().Where(p => p.DeletedDT == null).ToList();
            ViewBag.BannerSelectHeaderSelectedBannerID = SelectedBannerID;
            ViewBag.BannerSelectHeaderSelectedBannerLocationID = SelectedBannerLocationID;
            ViewBag.BannerSelectHeaderSelectedSiteSectionID = SelectedSiteSectionID;
            ViewBag.BannerSelectHeaderSelectedCountryID = SelectedCountryID;
            ViewBag.BannerSelectHeaderSelectedLanguageID = SelectedLanguageID;
            ViewBag.BannerSelectHeaderSelectedStatusID = SelectedStatusID;

            if (SelectedBannerLocationID != null)
            {
                List<int> BannersByLocation = UnitOfWork.GetRepository<Banner_BannerLocation>().GetAll().Where(p => p.BannerLocationID == SelectedBannerID).Select(p => p.BannerID).ToList();
                Model = Model.Where(p => BannersByLocation.Contains(p.BannerID)).ToList();
            }

            if (SelectedSiteSectionID != null)
            {
                List<int> BannersBySiteSection = UnitOfWork.GetRepository<Banner_SiteSection>().GetAll().Where(p => p.SiteSectionID == SelectedSiteSectionID).Select(p => p.BannerID).ToList();
                Model = Model.Where(p => BannersBySiteSection.Contains(p.BannerID)).ToList();
            }

            if (SelectedCountryID != null)
            {
                List<int> BannersByCountry = UnitOfWork.GetRepository<Banner_Country>().GetAll().Where(p => p.CountryID == SelectedCountryID).Select(p => p.BannerID).ToList();
                Model = Model.Where(p => BannersByCountry.Contains(p.BannerID)).ToList();
            }

            if (SelectedLanguageID != null)
            {
                Model = Model.Where(p => p.LanguageID == SelectedLanguageID).ToList();
            }

            if (SelectedStatusID != null)
            {
                Model = Model.Where(p => p.StatusID == SelectedStatusID).ToList();
            }

            return PartialView("~/Views/Shared/SearchFilters/BannerDropDownPartial.cshtml", Model);
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
        public JsonResult BannerImageAreaPartial(HttpPostedFileBase file, int BannerID, byte? source, int? ImageLibraryID, bool isImageB)
        {

            List<string> allowedExtensions = new List<string>() { ".jpeg", ".jpg", ".gif" };

            string fileExtension = source == 0 ? Path.GetExtension(file.FileName) : ".jpeg";

            if (file == null && source == 0)
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
            else if ((file != null ? !allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) : false) && source == 0)
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
                //Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages");

                Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();

                System.Drawing.Image img = null;

                //Aurigma.GraphicsMill.Bitmap auBmp = null;
                
                switch (source)
                {
                    case 0:
                        img = System.Drawing.Image.FromStream(file.InputStream, true, true);
                        //auBmp = new Aurigma.GraphicsMill.Bitmap(file.InputStream);
                        break;
                    case 1:
                        img = System.Drawing.Image.FromFile("D://wwwroot//flock//imagelibrary//1024//" + ImageLibraryID.ToString() + ".jpeg");
                        //auBmp = new Aurigma.GraphicsMill.Bitmap("C://wwwroot//flock//imagelibrary//1024//" + ImageLibraryID.ToString() + ".jpeg");
                        break;
                    default: break;
                }
                
                short newHeight = 0;
                var targetDimensions = new List<short[]>();

                if(img.Width >= 156)
                {
                    decimal d = 156.0M / (decimal)img.Width;
                    newHeight = Convert.ToInt16(img.Height * d);
                    targetDimensions.Add(new short[2] { 156, newHeight });
                }
                if (img.Width >= 256)
                {
                    decimal d = 256.0M / (decimal)img.Width;
                    newHeight = Convert.ToInt16(img.Height * d);
                    targetDimensions.Add(new short[2] { 256, newHeight });
                }
                if (img.Width >= 342)
                {
                    decimal d = 342.0M / (decimal)img.Width;
                    newHeight = Convert.ToInt16(img.Height * d);
                    targetDimensions.Add(new short[2] { 342, newHeight });
                }
                if (img.Width >= 512)
                {
                    decimal d = 512.0M / (decimal)img.Width;
                    newHeight = Convert.ToInt16(img.Height * d);
                    targetDimensions.Add(new short[2] { 512, newHeight });
                }
                if (img.Width >= 1024)
                {
                    decimal d = 1024.0M / (decimal)img.Width;
                    newHeight = Convert.ToInt16(img.Height * d);
                    targetDimensions.Add(new short[2] { 1024, newHeight });
                }

                // based on the width of the original image, we are able to process - maintaining the same aspect ratio - into:
                // 156 - 256 - 342 - 512 - 1024

                // we want an array of the widths and heights we must process the image to...

                foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//1024//").GetFiles(BannerID.ToString() + (isImageB ? "-B" : "") + ".*"))
                {
                    f.Delete();
                }
                foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//512//").GetFiles(BannerID.ToString() + (isImageB ? "-B" : "") + ".*"))
                {
                    f.Delete();
                }
                foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//342//").GetFiles(BannerID.ToString() + (isImageB ? "-B" : "") + ".*"))
                {
                    f.Delete();
                }
                foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//256//").GetFiles(BannerID.ToString() + (isImageB ? "-B" : "") + ".*"))
                {
                    f.Delete();
                }
                foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//156//").GetFiles(BannerID.ToString() + (isImageB ? "-B" : "") + ".*"))
                {
                    f.Delete();
                }

                foreach(var i in targetDimensions) 
                {
                    if (fileExtension.Contains("jp"))
                    {
                        fileExtension = ".jpeg";

                        var bitmap = new Bitmap(i[0], i[1]);

                        try
                        {
                            using (Graphics g = Graphics.FromImage(bitmap))
                            {
                                g.SmoothingMode = SmoothingMode.HighQuality;
                                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                g.CompositingQuality = CompositingQuality.HighQuality;
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                                g.DrawImage(img,
                                    new Rectangle(0, 0, i[0], i[1]),
                                    new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                            }

                            // at this point the object "bitmap" contains the image, resized to the target directory size. Convert to webp and save that too.

                            var fileName = AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//" + i[0].ToString() + "//" + BannerID.ToString() + (isImageB ? "-B" : "");

                            using (var webp = new WebP())
                            {
                                var webpFilename = fileName + ".webp";

                                if (bitmap.PixelFormat != PixelFormat.Format24bppRgb &&
                                    bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                                {
                                    Bitmap clone = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);

                                    using (Graphics gr = Graphics.FromImage(clone))
                                    {
                                        gr.DrawImage(clone, new Rectangle(0, 0, clone.Width, clone.Height));
                                    }

                                    webp.Save(clone, webpFilename, 75);
                                }
                                else
                                {
                                    webp.Save(bitmap, webpFilename, 75);
                                }
                            }

                            using (var fileStream = System.IO.File.Create(fileName + ".jpeg"))
                            {
                                var eps = new EncoderParameters(2);
                                eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                                eps.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);

                                var ici = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().Where(p => p.MimeType == ("image/jpeg")).FirstOrDefault();

                                bitmap.Save(fileStream, GetEncoderInfo("image/jpeg"), eps);
                            }


                            //var jpegSettings = new Aurigma.GraphicsMill.Codecs.JpegSettings();
                            //jpegSettings.Quality = 100;
                            //jpegSettings.UseSubsampling = false;
                            //jpegSettings.IsProgressive = false;

                            //auBmp.ApplyTransform(new Aurigma.GraphicsMill.Transforms.Resize() {
                            //    Width = i[0],
                            //    Height = i[1],
                            //    Id = "",
                            //    InterpolationMode = Aurigma.GraphicsMill.Transforms.ResizeInterpolationMode.High,
                            //    ResizeMode = Aurigma.GraphicsMill.Transforms.ResizeMode.Resize
                            //});
                            //auBmp.Save(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//" + i[0].ToString() + "//" + BannerID.ToString() + (isImageB ? "-B" : "") + ".jpeg"); //, jpegSettings

                        }
                        catch
                        {
                            if (bitmap != null) bitmap.Dispose();
                            throw;
                        }
                    }
                    else
                    {
                        using (var fileStream = System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//" + i[0].ToString() + "//" + BannerID.ToString() + (isImageB ? "-B" : "") + ".gif"))
                        {
                            using (var reader = new Aurigma.GraphicsMill.Codecs.GifReader(file.InputStream))//AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//" + i[0].ToString() + "//" + BannerID.ToString() + ".gif"))
                            using (var writer = new Aurigma.GraphicsMill.Codecs.GifWriter(fileStream))//AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//" + i[0].ToString() + "//" + BannerID.ToString() + ".gif"))
                            {
                                //Copy general properties of the source file
                                writer.BackgroundIndex = reader.BackgroundEntryIndex;
                                writer.Palette = reader.Palette;
                                writer.PlaybackCount = reader.PlaybackCount;
                            
                                for (int j = 0; j < reader.Frames.Count; j++)
                                {

                                    //Read a frame
                                    using (var frame = (Aurigma.GraphicsMill.Codecs.GifFrame)reader.Frames[j])
                                    using (var bitmap = frame.GetBitmap())
                                    {
                                        decimal widthPercentage = Math.Round(((decimal)i[0] / (decimal)img.Width), 10, MidpointRounding.AwayFromZero);// frame.Width;
                                        decimal heightPercentage = Math.Round(((decimal)i[1] / (decimal)img.Height), 10, MidpointRounding.AwayFromZero);// frame.Height;

                                        writer.FrameOptions.Left = (ushort)Convert.ToInt16(Math.Round(frame.Left * widthPercentage, 10, MidpointRounding.ToEven));
                                        writer.FrameOptions.Top = (ushort)Convert.ToInt16(Math.Round(frame.Top * heightPercentage, 10, MidpointRounding.ToEven));

                                        //Preserve the original palette
                                        Aurigma.GraphicsMill.ColorPalette palette = bitmap.Palette;
                                        //Preserve the original pixel format
                                        Aurigma.GraphicsMill.PixelFormat pixelFormat = bitmap.PixelFormat;

                                        //Convert the bitmap to a non-indexed format
                                        bitmap.ColorManagement.Convert(Aurigma.GraphicsMill.ColorSpace.Rgb, true, false);

                                        int w = (int)(Math.Round(frame.Width * widthPercentage));//, 10, MidpointRounding.ToEven));
                                        int h = (int)(Math.Round(frame.Height * heightPercentage));//, 10, MidpointRounding.ToEven));
                                        if (w == 0)
                                            w = 1;
                                        if (h == 0)
                                            h = 1;
                                        bitmap.Transforms.Resize(w, h,
                                            Aurigma.GraphicsMill.Transforms.ResizeInterpolationMode.Anisotropic9);

                                        bitmap.Palette = palette;
                                        bitmap.ColorManagement.Convert(pixelFormat);

                                        //Copy frame delay
                                        writer.FrameOptions.Delay = frame.Delay;

                                        writer.FrameOptions.DisposalMethod = frame.DisposalMethod;// Aurigma.GraphicsMill.Codecs.DisposalMethod.Background;
                                        //Add the frame
                                        Aurigma.GraphicsMill.Pipeline.Run(bitmap + writer);
                                    }
                                }
                            }
                        }
                    }
                }
                if (!isImageB)
                {
                    banner.Version++;
                    if (banner.Version == 255)
                        banner.Version = 0;
                }
                else
                {
                    banner.VersionB++;
                    if (banner.VersionB == 255)
                        banner.VersionB = 0;

                    if (banner.VersionB == null)
                        banner.VersionB = 0;
                }

                short[] last = targetDimensions.Last();

                banner.Width = last[0];
                banner.Height = last[1];
                banner.AspectRatio = (decimal)((decimal)last[0] / (decimal)last[1]);

                List<byte> AllowedLocations = null;

                if (last[0] >= 1024)
                {
                    ;
                }
                else if (last[0] >= 512)
                {
                    AllowedLocations = new List<byte>() {
                        12, 13, 14, 15, 16, 17, 8, 9, 10, 11, 50, 51, 52, 53, 2, 3, 4, 5, 6, 7, 40, 41, 42, 43, 44, 45, 54, 55 // 14/04/2020 - Removed 512px wide locations; Tom
                        //12, 13, 14, 15, 16, 17, 8, 9, 10, 11, 50, 51, 52, 53, 2, 3, 4, 5, 6, 7, 40, 41, 42, 43, 44, 45, 30, 31, 32, 33, 54, 55
                    };
                    List<Banner_BannerLocation> toDelete = UnitOfWork.GetRepository<Banner_BannerLocation>().GetAll()
                        .Where(p => p.BannerID == BannerID && !AllowedLocations.Contains(p.BannerLocationID)).ToList();
                    foreach(var bbl in toDelete) { 
                        UnitOfWork.GetRepository<Banner_BannerLocation>().Delete(bbl);
                    }
                    UnitOfWork.Save();
                }
                else if (last[0] >= 342)
                {
                    AllowedLocations = new List<byte>() { 
                        12, 13, 14, 15, 16, 17, 8, 9, 10, 11, 50, 51, 52, 53, 2, 3, 4, 5, 6, 7, 40, 41, 42, 43, 44, 45, 54, 55
                    };
                    List<Banner_BannerLocation> toDelete = UnitOfWork.GetRepository<Banner_BannerLocation>().GetAll()
                        .Where(p => p.BannerID == BannerID && !AllowedLocations.Contains(p.BannerLocationID)).ToList();
                    foreach (var bbl in toDelete)
                    {
                        UnitOfWork.GetRepository<Banner_BannerLocation>().Delete(bbl);
                    }
                    UnitOfWork.Save();
                }
                else if (last[0] >= 256)
                {
                    AllowedLocations = new List<byte>() { 
                       12, 13, 14, 15, 16, 17, 8, 9, 10, 11, 50, 51, 52, 53
                    };
                    List<Banner_BannerLocation> toDelete = UnitOfWork.GetRepository<Banner_BannerLocation>().GetAll()
                        .Where(p => p.BannerID == BannerID && !AllowedLocations.Contains(p.BannerLocationID)).ToList();
                    foreach (var bbl in toDelete)
                    {
                        UnitOfWork.GetRepository<Banner_BannerLocation>().Delete(bbl);
                    }
                    UnitOfWork.Save();
                }

                img.Dispose();

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
                jsresult.files[0].url = "/BannerImages/" + last[0] + "/" + BannerID.ToString() + (isImageB ? "-B" : "") + fileExtension + "?v=" + (isImageB ? banner.VersionB.ToString() : banner.Version.ToString());
                jsresult.files[0].thumbnailUrl = "/BannerImages/" + last[0] + "/" + BannerID.ToString() + (isImageB ? "-B" : "") + fileExtension + "?v=" + (isImageB ? banner.VersionB.ToString() : banner.Version.ToString());
                jsresult.files[0].deleteUrl = "/deletetest";
                jsresult.files[0].deleteType = "DELETE";
                return Json(jsresult, JsonRequestBehavior.AllowGet);
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
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

        public ActionResult ComposeBannerBannerLocationListBoxPartial(string BannerLocationAssignmentSelectedLocationIDs)
        {
            List<BannerLocation> Model = UnitOfWork.GetRepository<BannerLocation>().GetAll().OrderBy(p => p.Name).ToList();
            ViewBag.BannerLocationAssignmentSelectedLocationIDs = BannerLocationAssignmentSelectedLocationIDs;
            return PartialView("~/Views/Shared/SearchFilters/BannerLocationListBoxPartial.cshtml", Model);
        }

        public ActionResult ComposeBannerSiteSectionListBoxPartial(string SiteSectionAssignmentSelectedSiteSectionIDs)
        {
            List<SiteSection> Model = UnitOfWork.GetRepository<SiteSection>().GetAll().OrderBy(p => p.Name).ToList();
            ViewBag.SiteSectionAssignmentSelectedSiteSectionIDs = SiteSectionAssignmentSelectedSiteSectionIDs;
            return PartialView("~/Views/Shared/SearchFilters/SiteSectionListBoxPartial.cshtml", Model);
        }

        public ActionResult ComposeBannerMenuListBoxPartial(string MenuAssignmentSelectedMenuIDs)
        {
            List<Menu> Model = UnitOfWork.GetRepository<Menu>().GetAll().OrderBy(p => p.LocalName).ToList();
            ViewBag.MenuAssignmentSelectedMenuIDs = MenuAssignmentSelectedMenuIDs;
            return PartialView("MenuListBoxPartial", Model);
        }

        public ActionResult ComposeBannerLanguageDropDownPartial(byte? SelectedLanguageID)
        {
            List<Language> Model = UnitOfWork.GetRepository<Language>().GetAll().OrderBy(p => p.Name).ToList();
            ViewBag.BannerDetailsLanguageID = SelectedLanguageID;
            return PartialView("~/Views/Shared/SearchFilters/LanguageDropDownPartial.cshtml", Model);
        }

        [HttpPost]
        public ActionResult SaveBannerSiteSections(int ID, string SiteSectionIDs)
        {
            try
            {
                if (SiteSectionIDs != null && SiteSectionIDs != "")
                {
                    byte[] Sections = SiteSectionIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();
                    List<Banner_SiteSection> Current = UnitOfWork.GetRepository<Banner_SiteSection>().GetAll().Where(p => p.BannerID == ID).ToList();
                    foreach (var i in Current)
                    {
                        UnitOfWork.GetRepository<Banner_SiteSection>().Delete(i);
                    }
                    UnitOfWork.Save();
                    foreach (byte i in Sections)
                    {
                        UnitOfWork.GetRepository<Banner_SiteSection>().Add(new Banner_SiteSection() { BannerID = ID, SiteSectionID = i });
                    }
                    UnitOfWork.Save();
                }
                else
                {
                    List<Banner_SiteSection> Current = UnitOfWork.GetRepository<Banner_SiteSection>().GetAll().Where(p => p.BannerID == ID).ToList();
                    foreach (var i in Current)
                    {
                        UnitOfWork.GetRepository<Banner_SiteSection>().Delete(i);
                    }
                    UnitOfWork.Save();
                }
            }
            catch (SqlException e)
            {
                Response.StatusCode = 400;
                return Content(e.Message);
            }
            return Content("");
        }

        [HttpPost]
        public ActionResult SaveBannerMenus(int ID, string MenuIDs)
        {
            try
            {
                if (MenuIDs != null && MenuIDs != "")
                {
                    byte[] Menus = MenuIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();
                    List<Banner_Menu> Current = UnitOfWork.GetRepository<Banner_Menu>().GetAll().Where(p => p.BannerID == ID).ToList();
                    foreach (var i in Current)
                    {
                        UnitOfWork.GetRepository<Banner_Menu>().Delete(i);
                    }
                    UnitOfWork.Save();
                    foreach (byte i in Menus)
                    {
                        UnitOfWork.GetRepository<Banner_Menu>().Add(new Banner_Menu() { BannerID = ID, MenuID = i });
                    }
                    UnitOfWork.Save();
                }
                else
                {
                    List<Banner_Menu> Current = UnitOfWork.GetRepository<Banner_Menu>().GetAll().Where(p => p.BannerID == ID).ToList();
                    foreach (var i in Current)
                    {
                        UnitOfWork.GetRepository<Banner_Menu>().Delete(i);
                    }
                    UnitOfWork.Save();
                }
            }
            catch (SqlException e)
            {
                Response.StatusCode = 400;
                return Content(e.Message);
            }
            return Content("");
        }


        public class bannerLocationSave
        {
            public int ID { get; set; }
            public string BannerLocationIDs { get; set; }
        }

        public class bannerLocationSaveReturn
        {
            public Boolean reload { get; set; }
        }

        [HttpPost]
        public ActionResult SaveBannerLocations(bannerLocationSave objSave)
        {
            var result = new bannerLocationSaveReturn();

            result.reload = false;

            try
            {
                if (objSave.BannerLocationIDs != null && objSave.BannerLocationIDs != "")
                {
                    byte[] Locations = objSave.BannerLocationIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToArray();
                    List<Banner_BannerLocation> Current = UnitOfWork.GetRepository<Banner_BannerLocation>().GetAll().Where(p => p.BannerID == objSave.ID).ToList();
                    foreach (var i in Current)
                    {
                        UnitOfWork.GetRepository<Banner_BannerLocation>().Delete(i);
                    }
                    UnitOfWork.Save();


                    if(Locations.ToList().Contains(55))
                    {
                        // this is mutex with all other banner locations and vice versa
                        result.reload = true;
                        UnitOfWork.GetRepository<Banner_BannerLocation>().Add(new Banner_BannerLocation() { BannerID = objSave.ID, BannerLocationID = 55 });
                    }
                    else
                    {
                        var b = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == objSave.ID).Single();

                        b.VersionB = null;

                        // If we're nullifying VersionB we should delete the files, force the user to upload a new image.
                        // If you have got BannerLocationID 55 selected and VersionB is null, we will not serve the banner.
                        
                        foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//1024//").GetFiles(objSave.ID.ToString() + "-B" + ".*"))
                        {
                            f.Delete();
                        }
                        foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//512//").GetFiles(objSave.ID.ToString() + "-B" + ".*"))
                        {
                            f.Delete();
                        }
                        foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//342//").GetFiles(objSave.ID.ToString() + "-B" + ".*"))
                        {
                            f.Delete();
                        }
                        foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//256//").GetFiles(objSave.ID.ToString() + "-B" + ".*"))
                        {
                            f.Delete();
                        }
                        foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//156//").GetFiles(objSave.ID.ToString() + "-B" + ".*"))
                        {
                            f.Delete();
                        }

                        foreach (byte i in Locations)
                        {
                            UnitOfWork.GetRepository<Banner_BannerLocation>().Add(new Banner_BannerLocation() { BannerID = objSave.ID, BannerLocationID = i });
                        }
                    }

                    UnitOfWork.Save();
                }
                else
                {
                    List<Banner_BannerLocation> Current = UnitOfWork.GetRepository<Banner_BannerLocation>().GetAll().Where(p => p.BannerID == objSave.ID).ToList();

                    if (Current.Select(p => p.BannerLocationID).ToList().Contains(55))
                    {
                        result.reload = true;
                    }

                    var b = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == objSave.ID).Single();

                    b.VersionB = null;

                    // If we're nullifying VersionB we should delete the files, force the user to upload a new image.
                    // If you have got BannerLocationID 55 selected and VersionB is null, we will not serve the banner.

                    foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//1024//").GetFiles(objSave.ID.ToString() + "-B" + ".*"))
                    {
                        f.Delete();
                    }
                    foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//512//").GetFiles(objSave.ID.ToString() + "-B" + ".*"))
                    {
                        f.Delete();
                    }
                    foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//342//").GetFiles(objSave.ID.ToString() + "-B" + ".*"))
                    {
                        f.Delete();
                    }
                    foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//256//").GetFiles(objSave.ID.ToString() + "-B" + ".*"))
                    {
                        f.Delete();
                    }
                    foreach (FileInfo f in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//156//").GetFiles(objSave.ID.ToString() + "-B" + ".*"))
                    {
                        f.Delete();
                    }

                    foreach (var i in Current)
                    {
                        UnitOfWork.GetRepository<Banner_BannerLocation>().Delete(i);
                    }

                    UnitOfWork.Save();
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Content(e.Message);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateBannerCountryRestrictions(int BannerID, string Countries)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> CountryRestrictions = ser.Deserialize<List<SortDatum>>(Countries);

            List<Banner_Country> Current = UnitOfWork.GetRepository<Banner_Country>().GetAll().Where(p => p.BannerID == BannerID).ToList();
            List<SortDatum> CurrentCountries = new List<SortDatum>();
            foreach (Banner_Country country in Current)
            {
                CurrentCountries.Add(new SortDatum() { ID = country.CountryID, SortOrder = 0 });
            }
            List<SortDatum> Removals = CurrentCountries.Except(CountryRestrictions).ToList();

            foreach (SortDatum datum in Removals)
            {
                Banner_Country remove = UnitOfWork.GetRepository<Banner_Country>().GetAll().Where(p => p.BannerID == BannerID && p.CountryID == datum.ID).Single();
                UnitOfWork.GetRepository<Banner_Country>().Delete(remove);
            }
            UnitOfWork.Save();

            //could probably do deletion from here too: if ProductPage_Country table holds record which isn't in Countries passed to this function, then delete those.

            foreach (SortDatum datum in CountryRestrictions)
            {
                Banner_Country bc = UnitOfWork.GetRepository<Banner_Country>().GetAll().Where(p => p.BannerID == BannerID && p.CountryID == datum.ID).SingleOrDefault();

                if (bc == null)
                {
                    bc = new Banner_Country() { BannerID = BannerID, CountryID = (short)datum.ID };
                    UnitOfWork.GetRepository<Banner_Country>().Add(bc);
                }
            }

            UnitOfWork.Save();

            List<short> Linker = UnitOfWork.GetRepository<Banner_Country>().GetAll().Where(p => p.BannerID == BannerID).Select(p => p.CountryID).ToList();
            List<Country> Model = UnitOfWork.GetRepository<Country>().GetAll().Where(p => Linker.Contains(p.CountryID)).ToList();
            return PartialView("BannerCountryRestrictionsPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateBannerPageRestrictions(int BannerID, string Pages)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> PageRestrictions = ser.Deserialize<List<SortDatum>>(Pages);

            List<Banner_Page> Current = UnitOfWork.GetRepository<Banner_Page>().GetAll().Where(p => p.BannerID == BannerID).ToList();
            List<SortDatum> CurrentPages = new List<SortDatum>();
            foreach (Banner_Page page in Current)
            {
                CurrentPages.Add(new SortDatum() { ID = page.PageID, SortOrder = 0 });
            }
            List<SortDatum> Removals = CurrentPages.Except(PageRestrictions).ToList();

            foreach (SortDatum datum in Removals)
            {
                Banner_Page remove = UnitOfWork.GetRepository<Banner_Page>().GetAll().Where(p => p.BannerID == BannerID && p.PageID == datum.ID).Single();
                UnitOfWork.GetRepository<Banner_Page>().Delete(remove);
            }
            UnitOfWork.Save();

            //could probably do deletion from here too: if ProductPage_Country table holds record which isn't in Countries passed to this function, then delete those.

            foreach (SortDatum datum in PageRestrictions)
            {
                Banner_Page bc = UnitOfWork.GetRepository<Banner_Page>().GetAll().Where(p => p.BannerID == BannerID && p.PageID == datum.ID).SingleOrDefault();

                if (bc == null)
                {
                    bc = new Banner_Page() { BannerID = BannerID, PageID = (short)datum.ID };
                    UnitOfWork.GetRepository<Banner_Page>().Add(bc);
                }
            }

            UnitOfWork.Save();

            List<int> Linker = UnitOfWork.GetRepository<Banner_Page>().GetAll().Where(p => p.BannerID == BannerID).Select(p => p.PageID).ToList();
            List<Flock.Models.Page> Model = UnitOfWork.GetRepository<Flock.Models.Page>().GetAll().Where(p => Linker.Contains(p.PageID)).ToList();
            return PartialView("BannerPageRestrictionsPartial", Model);
        }

        [HttpPost]
        public ActionResult UpdateBannerLink(int BannerID, byte LinkType, bool NewWindow, string Data)
        {
            Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();

            switch (LinkType)
            {
                case 1: // page
                    banner.BannerLinkTypeID = 1;
                    banner.CollectionID = null;
                    banner.PageID = Convert.ToInt32(Data);
                    banner.ProductPageID = null;
                    banner.ExternalURL = null;
                    banner.ArticleID = null;
                    break;
                case 2: // productpage
                    banner.BannerLinkTypeID = 2;
                    banner.ExternalURL = null;
                    banner.PageID = null;
                    banner.ProductPageID = Convert.ToInt32(Data);
                    banner.CollectionID = null;
                    banner.ArticleID = null;
                    break;
                case 3: // collection
                    banner.BannerLinkTypeID = 3;
                    banner.ExternalURL = null;
                    banner.PageID = null;
                    banner.ProductPageID = null;
                    banner.CollectionID = Convert.ToInt16(Data);
                    banner.ArticleID = null;
                    break;
                case 4: // external
                    banner.BannerLinkTypeID = 4;
                    banner.ExternalURL = Data == null ? null : Data.TrimEnd(new[] { '/' });
                    banner.PageID = null;
                    banner.ProductPageID = null;
                    banner.CollectionID = null;
                    banner.ArticleID = null;
                    break;
                case 5:
                    banner.BannerLinkTypeID = 5;
                    banner.ExternalURL = null;
                    banner.PageID = null;
                    banner.ProductPageID = null;
                    banner.CollectionID = null;
                    banner.ArticleID = null;
                    break;
                case 6:
                    banner.BannerLinkTypeID = 6;
                    banner.ExternalURL = null;
                    banner.PageID = null;
                    banner.ProductPageID = null;
                    banner.CollectionID = null;
                    banner.ArticleID = Convert.ToInt32(Data);
                    break;
                default: break;
            }

            banner.NewWindow = NewWindow;
            UnitOfWork.Save();

            BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
            BannerComposeViewModel Model = Builder.Build(BannerID);
            return PartialView("BannerLinkPartial", Model);
        }

        [HttpPost]
        public ActionResult RemoveBannerLink(int BannerID)
        {
            Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();

            banner.CollectionID = null;
            banner.PageID = null;
            banner.ProductPageID = null;
            banner.ExternalURL = null;
                   
            banner.NewWindow = false;
            UnitOfWork.Save();

            BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
            BannerComposeViewModel Model = Builder.Build(BannerID);
            return PartialView("BannerLinkPartial", Model);
        }

        [HttpPost]
        public ActionResult UpdateBannerSiteSections(int BannerID, string SiteSections)
        {
            Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();

            JavaScriptSerializer ser = new JavaScriptSerializer();

            List<byte> SiteSectionList = new List<byte>();
            if(SiteSections != "")
                SiteSectionList = SiteSections.Split(',').ToList().ConvertAll(Convert.ToByte).ToList();

            List<byte> Current = UnitOfWork.GetRepository<Banner_SiteSection>().GetAll().Where(p => p.BannerID == BannerID).Select(p => p.SiteSectionID).ToList();

            List<byte> Removals = Current.Except(SiteSectionList).ToList();

            foreach (byte b in Removals)
            {
                Banner_SiteSection bss = UnitOfWork.GetRepository<Banner_SiteSection>().GetAll().Where(p => p.BannerID == BannerID && p.SiteSectionID == b).Single();
                UnitOfWork.GetRepository<Banner_SiteSection>().Delete(bss);
            }
            UnitOfWork.Save();

            foreach (byte bs in SiteSectionList)
            {
                Banner_SiteSection bss = UnitOfWork.GetRepository<Banner_SiteSection>().GetAll().Where(p => p.BannerID == BannerID && p.SiteSectionID == bs).SingleOrDefault();
                if (bss == null)
                {
                    bss = new Banner_SiteSection() { BannerID = BannerID, SiteSectionID = bs };
                    UnitOfWork.GetRepository<Banner_SiteSection>().Add(bss);
                }
                UnitOfWork.Save();
            }

            BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
            BannerComposeViewModel Model = Builder.Build(BannerID);
            return PartialView("BannerLocationPartial", Model);
        }

        [HttpPost]
        public ActionResult BannerLocationPartial(int BannerID)
        {
            BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
            BannerComposeViewModel Model = Builder.Build(BannerID);
            return PartialView("BannerLocationPartial", Model);
        }

        [HttpPost]
        public ActionResult UpdateBannerLocations(int BannerID, string BannerLocations)
        {
            Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<byte> LocationList = new List<byte>();
            if(BannerLocations != "")
                LocationList = BannerLocations.Split(',').ToList().ConvertAll(Convert.ToByte).ToList();

            List<byte> Current = UnitOfWork.GetRepository<Banner_BannerLocation>().GetAll().Where(p => p.BannerID == BannerID).Select(p => p.BannerLocationID).ToList();

            List<byte> Removals = Current.Except(LocationList).ToList();

            foreach (byte b in Removals)
            {
                Banner_BannerLocation bbl = UnitOfWork.GetRepository<Banner_BannerLocation>().GetAll().Where(p => p.BannerID == BannerID && p.BannerLocationID == b).Single();
                UnitOfWork.GetRepository<Banner_BannerLocation>().Delete(bbl);
            }
            UnitOfWork.Save();

            foreach (byte bs in LocationList)
            {
                Banner_BannerLocation bbl = UnitOfWork.GetRepository<Banner_BannerLocation>().GetAll().Where(p => p.BannerID == BannerID && p.BannerLocationID == bs).SingleOrDefault();
                if (bbl == null)
                {
                    bbl = new Banner_BannerLocation() { BannerID = BannerID, BannerLocationID = bs };
                    UnitOfWork.GetRepository<Banner_BannerLocation>().Add(bbl);
                }
                UnitOfWork.Save();
            }
            BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
            BannerComposeViewModel Model = Builder.Build(BannerID);
            return PartialView("BannerLocationPartial", Model);
        }

        [HttpPost]
        public ActionResult BannerUpdateLanguage(int BannerID, byte? LanguageID)
        {
            Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();

            banner.LanguageID = LanguageID;

            UnitOfWork.Save();
            
            BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
            BannerComposeViewModel Model = Builder.Build(BannerID);

            return PartialView("BannerDetailsPartial", Model);
        }

        [HttpPost]
        public ActionResult BannerUpdateName(int BannerID, string Text)
        {
            if (ModelState.IsValid)
            {
                Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();

                banner.Name = Text;

                UnitOfWork.Save();

                BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
                BannerComposeViewModel Model = Builder.Build(BannerID);
                return PartialView("BannerDetailsPartial", Model);
            }
            else
            {
                ViewData["EditError"] += "Please, correct all errors";

                BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
                BannerComposeViewModel Model = Builder.Build(BannerID);
                return PartialView("BannerDetailsPartial", Model);
            }
        }

        [HttpPost]
        public ActionResult BannerUpdateDate(int BannerID, string Date, byte OnOrOff)
        {
            if (Date != "")
            {
                DateTime dt = Convert.ToDateTime(Date);

                Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();

                switch (OnOrOff)
                {
                    case 0:
                        break;

                    case 1:
                        banner.OffDate = dt;
                        break;

                    default: break;
                }
            }
            else
            {
                Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();

                switch (OnOrOff)
                {
                    case 0:
                        banner.OnDate = null;
                        break;

                    case 1:
                        banner.OffDate = null;
                        break;

                    default: break;
                }
            }
            UnitOfWork.Save();

            BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
            BannerComposeViewModel Model = Builder.Build(BannerID);

            return PartialView("BannerDetailsPartial", Model);
        }

        [HttpPost]
        public ActionResult BannerUpdateTitleText(int BannerID, string TitleText)
        {
            UnitOfWork.Save();

            BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
            BannerComposeViewModel Model = Builder.Build(BannerID);

            return PartialView("BannerDetailsPartial", Model);
        }

        [HttpPost]
        public ActionResult BannerUpdateAltText(int BannerID, string AltText)
        {
            Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();

            UnitOfWork.Save();

            BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
            BannerComposeViewModel Model = Builder.Build(BannerID);

            return PartialView("BannerDetailsPartial", Model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult UpdateBannerDetails(BannerComposeViewModel model)
        {
            bool hasError = false;
            if (model.OnDate == null && model.OffDate != null)
            {
                hasError = true;
                ViewData["EditError"] += "Please specify an On Date.";
            }
            if (model.OnDate != null && model.OffDate != null)
            {
                if (model.OffDate <= model.OnDate)
                {
                    hasError = true;
                    ViewData["EditError"] += "On Date must be before Off Date.";
                }
            }
            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    Banner banner = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == model.BannerID).Single();

                    banner.Name = model.Name;
                    banner.TitleText = model.TitleText;
                    banner.AltText = model.AltText;
                    banner.OnDate = model.OnDate;
                    banner.OffDate = model.OffDate;
                    banner.LanguageID = model.LanguageID;

                    UnitOfWork.Save();

                    BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
                    BannerComposeViewModel Model = Builder.Build(model.BannerID);

                    return PartialView("BannerDetailsPartial", Model);
                }
                else
                {
                    ViewData["EditError"] += "Please correct all errors.";
                    Response.StatusCode = 400;

                    return PartialView("BannerDetailsPartial", model);
                }
            }
            else
            {
                Response.StatusCode = 400;

                BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
                BannerComposeViewModel Model = Builder.Build(model.BannerID);

                return PartialView("BannerDetailsPartial", model);
            }
        }

        [HttpPost]
        public ActionResult BannerSetNewWindowFlag(int BannerID, bool NewWindow)
        {
            Banner b = UnitOfWork.GetRepository<Banner>().GetAll().Where(p => p.BannerID == BannerID).Single();
            b.NewWindow = NewWindow;
            UnitOfWork.Save();
            BannerComposeViewModelBuilder Builder = new BannerComposeViewModelBuilder(UnitOfWork);
            BannerComposeViewModel Model = Builder.Build(BannerID);
            return PartialView("BannerLinkPartial", Model);
        }

        [HttpPost]
        public ActionResult cloneBanner(int BannerID)
        {
            int? newBannerId = null;

            try
            {
                newBannerId = UnitOfWork.Context.Banner_Copy(BannerID);

                if (newBannerId != null)
                {

                    var folders = new List<int>
                    {
                        156,
                        256,
                        342,
                        512,
                        1024
                    };
                    
                    foreach(var f in folders)
                    {
                        if(System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//" + f + "//" + BannerID + ".jpeg"))
                        {
                            System.IO.File.Copy(
                                AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//" + f + "//" + BannerID + ".jpeg",
                                AppDomain.CurrentDomain.BaseDirectory + "//BannerImages//" + f + "//" + newBannerId + ".jpeg"
                            );
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return Content(ex.ToString());
            }

            return Content(newBannerId.ToString());
        }
    }
}