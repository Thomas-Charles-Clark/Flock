using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Flock.DAL;
using Flock.Util;
using NLog;
using Flock.Models;
using Flock.ViewModels;
using System.Data.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Flock.Controllers
{
    public class ImagesController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            ImagesOverviewViewModelBuilder Builder = new ImagesOverviewViewModelBuilder(UnitOfWork);

            ImagesOverviewViewModel Model = Builder.Build(null);

            return View(Model);
        }

        [ValidateInput(false)]
        public ActionResult ImageSearch(
            byte? ProductType, byte? Brand, short? Launch, short? Collection, 
            byte? AspectRatio, byte? ImageType, byte? Perspective, bool? HotspotStatus, 
            byte? Colour, byte? Room, byte? Style, string NameOrID, bool? SearchWithinHotspots, string Resolution)
        {
            if (NameOrID == "" || NameOrID == "Name, Code or ID")
                NameOrID = null;
            if (Resolution == "")
                Resolution = null;
            System.Data.Linq.ISingleResult<Image_SearchResult> Results = null;

            List<Image_SearchResult> Model = new List<Image_SearchResult>();
            using (FlockDataContext fdc = new FlockDataContext())
            {
                Results = fdc.Image_Search(AspectRatio, ImageType, Perspective, HotspotStatus, Style, Room, ProductType, Collection, Brand, Launch, Colour, NameOrID, Resolution, SearchWithinHotspots);

                foreach (Image_SearchResult result in Results)
                {
                    Model.Add(result);
                }
            }
            return PartialView("_ImageSearchDataViewPartial", Model);
        }

        public ActionResult ImageSearchProductTypeDropDownPartial()
        {
            List<Flock.Models.ProductType> Model = UnitOfWork.GetRepository<ProductType>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        public ActionResult ImageSearchBrandDropDownPartial()
        {
            List<Flock.Models.Brand> Model = UnitOfWork.GetRepository<Brand>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        public ActionResult ImageSearchLaunchDropDownPartial()
        {
            List<Flock.Models.Launch> Model = UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        public ActionResult ImageSearchNameOrIDTextBoxPartial()
        {
            return PartialView("~/Views/Shared/SearchFilters/NameOrIDTextBoxPartial.cshtml");
        }

        public ActionResult ImageSearchCollectionDropDownPartial()
        {
            List<Flock.Models.Collection> Model = UnitOfWork.GetRepository<Flock.Models.Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        public ActionResult ImageSearchAspectRatioDropDownPartial()
        {
            List<Flock.Models.AspectRatio> Model = UnitOfWork.GetRepository<Flock.Models.AspectRatio>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/AspectRatioDropDownPartial.cshtml", Model);
        }

        public ActionResult ImageSearchImageTypeDropDownPartial()
        {
            List<Flock.Models.ImageType> Model = UnitOfWork.GetRepository<Flock.Models.ImageType>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ImageTypeDropDownPartial.cshtml", Model);
        }

        public ActionResult ImageSearchPerspectiveDropDownPartial()
        {
            List<Flock.Models.Perspective> Model = UnitOfWork.GetRepository<Flock.Models.Perspective>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/PerspectiveDropDownPartial.cshtml", Model);
        }

        public ActionResult ImageSearchHotspotStatusDropDownPartial()
        {
            return PartialView("~/Views/Shared/SearchFilters/HotspotStatusDropDownPartial.cshtml");
        }

        public ActionResult ImageSearchColourDropDownPartial()
        {
            List<Flock.Models.Colour> Model = UnitOfWork.GetRepository<Flock.Models.Colour>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ColourDropDownPartial.cshtml", Model);
        }

        public ActionResult ImageSearchRoomDropDownPartial()
        {
            List<Flock.Models.Room> Model = UnitOfWork.GetRepository<Flock.Models.Room>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/RoomDropDownPartial.cshtml", Model);
        }

        public ActionResult ImageSearchStyleDropDownPartial()
        {
            List<Flock.Models.Style> Model = UnitOfWork.GetRepository<Flock.Models.Style>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/StyleDropDownPartial.cshtml", Model);
        }

        [HttpPost]
        public ActionResult SwapImage(long OriginalImage, long ReplacementImage)
        {
            try 
            {
                UnitOfWork.Context.Image_Swap(OriginalImage, ReplacementImage, false);
            }
            catch(Exception e)
            { 
                Response.StatusCode = 400;
                Logger.Error(e, "Swap Image\r\nOriginalImage = " + OriginalImage.ToString() + "\r\nReplacement Image = " + ReplacementImage.ToString());
                return Content(e.ToString());
            }
            return Content("");
        }

        public ActionResult VersionChecker()
        {
            // Get a list of pairs of images from stored procedure
            // 
            var Model = new ImageVersionCheckerViewModel();
            Model.Images = UnitOfWork.Context.Image_MultipleVersions_Select().ToList();
            
            return View(Model);
        }

        [HttpPost]
        public ActionResult ConfirmReplacement(long CandidateID, long IncumbentID)
        {
            try
            {
                UnitOfWork.Context.Image_Swap(IncumbentID, CandidateID, true);
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                Logger.Error(e, "Swap Image\r\nOriginalImage = " + CandidateID.ToString() + "\r\nReplacement Image = " + IncumbentID.ToString());
                return Content(e.ToString());
            }

            return Content("ok");
        }

        public ActionResult ImageExporter()
        {
            return View();
        }

        public ActionResult ImageExporterLaunchDropDownPartial()
        {
            var Model = UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();

            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateLaunchExport(string LaunchIDs)
        {
            try
            {
                // Detect extant folder structure specific to this launch.
                // Delete folder contents.
                // Create folder structure.
                // Copy files from ImageLibrary folder to correct locations in created folder structure.

                // There's a stored procedure to execute...
                /* 
                 *  [ResultType(typeof(ProductImage_Export_ProductImageResult))]
                    [ResultType(typeof(ProductImage_Export_LaunchResult))]
                    [ResultType(typeof(ProductImage_Export_BrandResult))]
                    [ResultType(typeof(ProductImage_Export_ProductTypeResult))]
                    [ResultType(typeof(ProductImage_Export_CollectionResult))]
                 * 
                 * copy from 1024
                 */

                string ExportDirectory = "C:\\FlockImageExports\\Exports";

                if (Directory.Exists(ExportDirectory))
                    Directory.Delete(ExportDirectory, true);
                Directory.CreateDirectory(ExportDirectory);

                List<short> _launchIDs = LaunchIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToList();

                foreach (short LaunchID in _launchIDs)
                {
                    IMultipleResults r = UnitOfWork.Context.ProductImage_Export(LaunchID);

                    List<ProductImage_Export_ProductImageResult> Images = r.GetResult<ProductImage_Export_ProductImageResult>().ToList();
                    List<ProductImage_Export_LaunchResult> Launches = r.GetResult<ProductImage_Export_LaunchResult>().ToList();
                    List<ProductImage_Export_BrandResult> Brands = r.GetResult<ProductImage_Export_BrandResult>().ToList();
                    List<ProductImage_Export_ProductTypeResult> ProductTypes = r.GetResult<ProductImage_Export_ProductTypeResult>().ToList();
                    List<ProductImage_Export_CollectionResult> Collections = r.GetResult<ProductImage_Export_CollectionResult>().ToList();

                    foreach (var l in Launches)
                    {
                        var LaunchDirectory = ExportDirectory + "\\" + l.Launch;
                        var LaunchSpreadsheetName = l.Launch + ".xlsx";

                        Directory.CreateDirectory(LaunchDirectory);

                        OpenXmlHelper.CreateProductImageExportSpreadsheet(LaunchDirectory + "\\" + LaunchSpreadsheetName, LaunchSpreadsheetName, Images);

                        foreach (var b in Brands.Where(p => p.LaunchID == l.LaunchID))
                        {
                            var BrandDirectory = ExportDirectory + "\\" + l.Launch + "\\" + b.Brand;

                            Directory.CreateDirectory(BrandDirectory);

                            foreach (var pt in ProductTypes.Where(p => p.LaunchID == l.LaunchID && p.BrandID == b.BrandID))
                            {
                                var ProductTypeDirectory = ExportDirectory + "\\" + l.Launch + "\\" + b.Brand + "\\" + pt.ProductType;

                                Directory.CreateDirectory(ProductTypeDirectory);

                                foreach (var c in Collections.Where(p => p.LaunchID == l.LaunchID && p.BrandID == b.BrandID && p.ProductTypeID == pt.ProductTypeID))
                                {
                                    var CollectionDirectory = ExportDirectory + "\\" + l.Launch + "\\" + b.Brand + "\\" + pt.ProductType + "\\" + c.Collection;

                                    Directory.CreateDirectory(CollectionDirectory);
                                }
                            }
                        }
                    }

                    await CopyImagesForExport(Images);
                }


                return Content("Done.");
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }


        private async System.Threading.Tasks.Task CopyImagesForExport(List<ProductImage_Export_ProductImageResult> images)
        {
            string StartDirectory = AppDomain.CurrentDomain.BaseDirectory + "//imagelibrary//1024//";
            string EndDirectory = @"C:\FlockImageExports\Exports";

            foreach (var i in images)
            {
                if(i.ImageID != null)
                {
                    using (FileStream SourceStream = System.IO.File.Open(StartDirectory + i.ImageID.ToString() + ".jpeg", FileMode.Open))
                    {
                        using (FileStream DestinationStream = System.IO.File.Create(EndDirectory + i.Path + "\\" + i.ImageFileName + ".jpeg"))
                        {
                            await SourceStream.CopyToAsync(DestinationStream);
                        }
                    }
                }
            }
        }
    }
}
