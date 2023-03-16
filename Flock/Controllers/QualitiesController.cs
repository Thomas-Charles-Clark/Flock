using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;

namespace Flock.Controllers
{
    public class QualitiesController : Controller
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
            return RedirectToAction("Overview");
        }

        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult QualityOverviewGridViewPartial()
        {
            var Model = new QualityOverviewViewModel();

            var results = UnitOfWork.Context.Quality_Overview_Generate();

            Model.ProductTypes = results.GetResult<Quality_Overview_Generate_ProductTypeResult>().ToList();
            Model.Launches = results.GetResult<Quality_Overview_Generate_LaunchResult>().ToList();
            Model.Brands = results.GetResult<Quality_Overview_Generate_BrandResult>().ToList();
            var qs = results.GetResult<Quality_Overview_Generate_QualityResult>().ToList();
            Model.QualitiesCollections = results.GetResult<Quality_Overview_Generate_QualityCollectionResult>().ToList();
            Model.Records = new List<Quality_Overview_Grid_Record>();

            foreach(var q in qs)
            {
                Model.Records.Add(new Quality_Overview_Grid_Record()
                {
                    QualityID = q.QualityID,
                    Name = q.Name,
                    ProductTypeID = q.ProductTypeID,
                    ProductTypeName = Model.ProductTypes.Where(p => p.ProductTypeID == q.ProductTypeID).Select(p => p.Name).Single(),
                    BrandID = q.BrandID,
                    BrandName = Model.Brands.Where(p => p.BrandID == q.BrandID).Select(p => p.Name).Single(),
                    FirstLaunchID = q.FirstLaunchID,
                    FirstLaunchName = Model.Launches.Where(p => p.LaunchID == q.FirstLaunchID).Select(p => p.Name).Single(),
                    LastLaunchID = q.LastLaunchID,
                    LastLaunchName = Model.Launches.Where(p => p.LaunchID == q.LastLaunchID).Select(p => p.Name).Single(),
                    ProductCount = q.ProductCount,
                    CollectionCount = q.CollectionCount,
                    Code = q.Code,
                    Width = q.Width,
                    Weight = q.Weight,
                    HorizontalPatternRepeat = q.HorizontalPatternRepeat,
                    VerticalPatternRepeat = q.VerticalPatternRepeat,
                    Composition = q.Composition,                    
                    Martindale = q.Martindale
                });
            }

            return PartialView(Model);
        }

        public ActionResult QualityComposeCollectionDropDownPartial()
        {
            var Model = UnitOfWork.GetRepository<Collection>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        public ActionResult QualityOverviewGridPartialUpdate(short QualityID, byte ProductTypeID, string MetaDescription, string SubHeading, string Description)
        {

            var m = new QualityComposeViewModelBuilder(UnitOfWork).Build(QualityID, ProductTypeID);

            string ProductPageIDs = String.Join(",", m.ProductPages.Select(p => p.ProductPageID.ToString()).ToArray());

            UnitOfWork.Context.Quality_Overview_Detail_Save(ProductPageIDs, Description, MetaDescription, SubHeading);

            var Model = new QualityOverviewViewModel();

            var results = UnitOfWork.Context.Quality_Overview_Generate();

            Model.ProductTypes = results.GetResult<Quality_Overview_Generate_ProductTypeResult>().ToList();
            Model.Launches = results.GetResult<Quality_Overview_Generate_LaunchResult>().ToList();
            Model.Brands = results.GetResult<Quality_Overview_Generate_BrandResult>().ToList();
            var qs = results.GetResult<Quality_Overview_Generate_QualityResult>().ToList();
            Model.QualitiesCollections = results.GetResult<Quality_Overview_Generate_QualityCollectionResult>().ToList();
            Model.Records = new List<Quality_Overview_Grid_Record>();

            foreach (var q in qs)
            {
                Model.Records.Add(new Quality_Overview_Grid_Record()
                {
                    QualityID = q.QualityID,
                    Name = q.Name,
                    ProductTypeID = q.ProductTypeID,
                    ProductTypeName = Model.ProductTypes.Where(p => p.ProductTypeID == q.ProductTypeID).Select(p => p.Name).Single(),
                    BrandID = q.BrandID,
                    BrandName = Model.Brands.Where(p => p.BrandID == q.BrandID).Select(p => p.Name).Single(),
                    FirstLaunchID = q.FirstLaunchID,
                    FirstLaunchName = Model.Launches.Where(p => p.LaunchID == q.FirstLaunchID).Select(p => p.Name).Single(),
                    LastLaunchID = q.LastLaunchID,
                    LastLaunchName = Model.Launches.Where(p => p.LaunchID == q.LastLaunchID).Select(p => p.Name).Single(),
                    ProductCount = q.ProductCount,
                    CollectionCount = q.CollectionCount,
                    Code = q.Code,
                    Width = q.Width,
                    Weight = q.Weight,
                    HorizontalPatternRepeat = q.HorizontalPatternRepeat,
                    VerticalPatternRepeat = q.VerticalPatternRepeat,
                    Composition = q.Composition,
                    Martindale = q.Martindale
                });
            }

            return PartialView("QualityOverviewGridViewPartial", Model);
        }

        [HttpPost]
        public ActionResult ChangeQuality(short QualityID, byte ProductTypeID)
        {
            var Model = new QualityComposeViewModelBuilder(UnitOfWork).Build(QualityID, ProductTypeID);

            return PartialView("QualityComposePartial", Model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult QualityHtmlEditorSavePartial(short QualityID, string Html)
        {
            var quality = UnitOfWork.GetRepository<Quality>().GetAll().Where(p => p.QualityID == QualityID).Single();
            quality.Description = Html;
            UnitOfWork.Save();
            return PartialView("QualityComposeHTMLEditorPartial", Html);
        }

        [HttpPost]
        public ActionResult LoadQualityDescription(short ID)
        {
            return Content(UnitOfWork.GetRepository<Quality>().GetAll().Where(p => p.QualityID == ID).Select(p => p.Description).Single());
        }
	}
}