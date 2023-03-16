using DevExpress.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.IO;
using System.Data.Linq;
using Flock.Util;

namespace Flock.Controllers
{
    public class CollectionsController : Controller
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
            return RedirectToAction("overview");
        }

        [AllowAnonymous]
        public ActionResult Overview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveCollectionInfo(CollectionInfoViewModel model)
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
                    ModelState.AddModelError("OnDate", "On Date must be before Off Date");
                    ModelState.AddModelError("OffDate", "Off Date must be after On Date");
                    ViewData["EditError"] += "On Date must be before Off Date.";
                }
            }
            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    Collection coll = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == model.CollectionID).Single();
                    coll.Name = model.Name;
                    coll.OnDate = model.OnDate;
                    coll.OffDate = model.OffDate;
                    coll.ModifiedDT = DateTime.Now;
                    coll.New = model.New;
                    coll.BrandID = model.BrandID;
                    coll.LaunchID = model.LaunchID;
                    coll.PBCode = model.PatternBookCode;
                    coll.Code = model.Code;
                    coll.PreviewColourways = model.PreviewColourways;
                    UnitOfWork.Save();

                    CollectionViewModelBuilder Builder = new CollectionViewModelBuilder(UnitOfWork);
                    CollectionViewModel Model = Builder.Build(model.CollectionID);
                    
                    return PartialView("CollectionInfoPartial", Model);
                }
                else
                {
                    ViewData["EditError"] += "Please correct all errors.";
                    Response.StatusCode = 400;

                    return PartialView("CollectionInfoPartial", model);
                }
            }
            else
            {
                Response.StatusCode = 400;

                CollectionViewModelBuilder Builder = new CollectionViewModelBuilder(UnitOfWork);
                CollectionViewModel Model = Builder.Build(model.CollectionID);

                return PartialView("CollectionInfoPartial", Model);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SaveCollectionTextDetails(CollectionDetailsAndPodsViewModel item)
        {
            if(ModelState.IsValid)
            {
                Collection coll = UnitOfWork.GetRepository<Collection>().GetAll().Where(c => c.CollectionID == item.CollectionID).Single();
                coll.TitleTag = item.TitleTag;
                coll.URLKeywords = item.URLKeywords;
                coll.MetaDescription = item.MetaDescription;
                //item.Name = coll.Name;
                //item.Code = coll.Code;
                UnitOfWork.Save();

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, null, null, item.CollectionID, null, false, false);

                item.CollectionImages = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == item.CollectionID && p.TemplateImageID == 1).OrderBy(p => p.SortOrder).ToList();
                CollectionViewModelBuilder CollectionBuilder = new CollectionViewModelBuilder(UnitOfWork);
                var CVM = CollectionBuilder.Build(item.CollectionID);
                return PartialView("CollectionDetailsPartial", CVM);
            }
            else
            {
                item.CollectionImages = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == item.CollectionID && p.TemplateImageID == 1).OrderBy(p => p.SortOrder).ToList();
                CollectionViewModelBuilder CollectionBuilder = new CollectionViewModelBuilder(UnitOfWork);
                var CVM = CollectionBuilder.Build(item.CollectionID);
                Response.StatusCode = 500;
                return PartialView("CollectionDetailsPartial", CVM);
            }
        }

        [ValidateInput(false)]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SaveCollectionDescriptionDetails(CollectionTextAndSlideshowViewModel item)
        {
            if(ModelState.IsValid)
            {
                Collection coll = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == item.CollectionID).Single();
                coll.Heading = item.Heading;
                coll.SubHeading = item.SubHeading;
                coll.Description = item.Description;

                UnitOfWork.Save();

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_Collection(item.CollectionID, null, null, null, null, null, false, false);

                item.CollectionImages = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == item.CollectionID && p.TemplateImageID == 2).OrderBy(p => p.SortOrder).ToList();
                CollectionViewModelBuilder CollectionBuilder = new CollectionViewModelBuilder(UnitOfWork);
                var CVM = CollectionBuilder.Build(item.CollectionID);

                return PartialView("CollectionDescriptionPartial", CVM);
            }
            else
            {
                item.CollectionImages = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == item.CollectionID && p.TemplateImageID == 1).OrderBy(p => p.SortOrder).ToList();

                CollectionViewModelBuilder CollectionBuilder = new CollectionViewModelBuilder(UnitOfWork);
                var CVM = CollectionBuilder.Build(item.CollectionID);
                Response.StatusCode = 500;
                return PartialView("CollectionDescriptionPartial", CVM);
            }
        }
        
        /// <summary>
        /// This action method looks up collections for autocomplete.
        /// </summary>
        /// <param name="q">The query that contains the user input.</param>
        /// <param name="limit">The number of collections to return.</param>
        public ActionResult LookupCollections(string q, int limit)
        {
            List<Collection> colls = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.Name.StartsWith(q, StringComparison.CurrentCultureIgnoreCase)).ToList();

            var retValue = colls.Take(limit).Select(r => new { Name = r.Name, ID = r.CollectionID});

            return Json(retValue);
        }

        public ActionResult CollectionComboBoxPartial()
        {
            var Model = UnitOfWork.GetRepository<Collection>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionComboBoxPartial.cshtml", Model);
        }

        /*
         
        public class CollectionHeaderFilters
        {
            public byte? ProductTypeID { get; set; }
            public byte? BrandID { get; set; }
            public short? LaunchID { get; set; }
        }
         
         */

        [AllowAnonymous]
        public ActionResult Compose(long? id, byte? ProductTypeid, byte? brandid, short? launchid)
        {
            if (id != null)
            {
                CollectionViewModel CVM = null;
                if (UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == id && p.DeletedDT == null).SingleOrDefault() != null)
                {
                    CollectionViewModelBuilder CollectionBuilder = new CollectionViewModelBuilder(UnitOfWork);
                    CVM = CollectionBuilder.Build(id);
                }
                else
                {
                    CVM = new CollectionViewModel();
                    CVM.CountryRestrictions = new List<Country>();
                    ViewData["InvalidCollectionIDError"] = "Invalid Collection ID, or the collection is marked as deleted. Deleted collections can be retrieved."; 
                }
                return View("ComposeCollection", CVM);
            }
            else
            {
                CollectionViewModel CVM = new CollectionViewModel();
                CVM.CountryRestrictions = new List<Country>();
                return View("ComposeCollection", CVM);
            }
        }

        public ActionResult CollectionComposeBrochureComboBoxPartial()
        {
            var Model = UnitOfWork.GetRepository<Brochure>().GetAll().Where(p => p.DeletedDT == null).ToList();
            return PartialView("~/Views/Shared/SearchFilters/BrochureComboBoxPartial.cshtml", Model);
        }

        public ActionResult CollectionComposeVideoComboBoxPartial()
        {
            var Model = UnitOfWork.GetRepository<Video>().GetAll().Where(p => p.DeletedDT == null).ToList();
            return PartialView("~/Views/Shared/SearchFilters/VideoComboBoxPartial.cshtml", Model);
        }

        [HttpPost]
        public ActionResult AssignCollectionBrochure(short CollectionID, short BrochureID)
        {
            var Coll = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == CollectionID).Single();
            Coll.BrochureID = BrochureID;

            UnitOfWork.Save();

            CollectionViewModelBuilder CollectionBuilder = new CollectionViewModelBuilder(UnitOfWork);
            var Model = CollectionBuilder.Build(CollectionID);

            return PartialView("CollectionImagesBrochurePartial", Model);
        }

        [HttpPost]
        public ActionResult AssignCollectionVideo(short CollectionID, short VideoID)
        {
            var Coll = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == CollectionID).Single();
            Coll.VideoID = VideoID;

            UnitOfWork.Save();

            CollectionViewModelBuilder CollectionBuilder = new CollectionViewModelBuilder(UnitOfWork);
            var Model = CollectionBuilder.Build(CollectionID);

            return PartialView("CollectionVideoPartial", Model);
        }

        [HttpPost]
        public ActionResult ChangeCollection(short id)
        {
            CollectionViewModelBuilder CollectionBuilder = new CollectionViewModelBuilder(UnitOfWork);

            CollectionViewModel CVM = CollectionBuilder.Build(id);

            return PartialView("CollectionComposePartial", CVM);
        }

        public ActionResult ComposeCollectionGridSelectorPartial()
        {
            return PartialView();
        }

        [AllowAnonymous]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CollectionUpdate(CollectionViewModel item)
        {
            CollectionViewModelBuilder CollBuilder = new CollectionViewModelBuilder(UnitOfWork);

            Flock.Models.Collection col = UnitOfWork.GetRepository<Flock.Models.Collection>().GetAll().Where(p => p.CollectionID == item.CollectionID).Single();
            col.BrandID = item.BrandID;
            col.Name = item.Name;

            col.TitleTag = item.TitleTag;
            col.URLKeywords = item.URLKeywords;
            col.MetaDescription = item.MetaDescription;

            col.Heading = item.Heading;
            col.SubHeading = item.SubHeading;
            col.Description = item.Description;
            col.LaunchID = item.LaunchID;
            col.ModifiedDT = DateTime.Now;
            col.VideoID = item.VideoID;
            col.BrochureID = item.BrochureID;

            UnitOfWork.Save();

            UnitOfWork.Context.ReplaceLocalURLPlaceholders_Collection(item.CollectionID, null, null, null, null, null, false, false);

            UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, null, null, item.CollectionID, null, false, false);

            return RedirectToAction("compose", "collections", new { id = item.CollectionID });
        }

        [AllowAnonymous]
        public ActionResult ProductAssociation(long? id, byte? ProductTypeid, byte? brandid, short? launchid)
        {
            ProductToCollectionViewModelBuilder Builder = new ProductToCollectionViewModelBuilder(UnitOfWork);
            ProductToCollectionViewModel Model = Builder.Build(null);

            CollectionHeaderFilters CHF = new CollectionHeaderFilters() { CollectionID = Convert.ToInt16(id), BrandID = brandid, ProductTypeID = ProductTypeid, LaunchID = launchid };

            Model.HeaderFilters = CHF;

            if (id != null)
                Model.CollectionID = id;
            return View("ProductAssociation", Model);
        }

        [HttpPost]
        public ActionResult ChangeCollectionAssociation(short? CollectionID)
        {
            var Model = new CollectionProductAssociationContentsViewModel();
            Model.Products = new List<CollectionProductAssociationViewModel>();
            if (CollectionID != null)
            {
                var PreModel = UnitOfWork.Context.Product_Collection_Select(CollectionID);
                foreach (var i in PreModel.OrderBy(p => p.SortOrder))
                {
                    Model.Products.Add(new CollectionProductAssociationViewModel()
                    {
                        Name = i.Name,
                        Code = i.Code,
                        ImageID = i.ImageID,
                        ProductID = i.ProductID,
                        ProductPageID = i.ProductPageID,
                        SortOrder = i.SortOrder,
                        Status = i.Status
                    });
                }
            }    
            return PartialView("CollectionContents", Model);
        }

        [ValidateInput(false)]
        public ActionResult CollectionSearch(byte? ProductType, byte? Brand, short? Launch, string NameOrID)
        {
            if (NameOrID == "" || NameOrID == "Name, Code or ID")
                NameOrID = null;
            System.Data.Linq.ISingleResult<Collection_SearchResult> Results = null;

            List<Collection_SearchResult> Model = new List<Collection_SearchResult>();
            using (FlockDataContext fdc = new FlockDataContext())
            {
                Results = fdc.Collection_Search(ProductType, Brand, Launch, NameOrID);

                foreach (Collection_SearchResult result in Results)
                {
                    Model.Add(result);
                }
            }
            return PartialView("_CollectionSearchDataViewPartial", Model);
        }

        public ActionResult CollectionSearchProductTypeDropDownPartial()
        {
            List<Flock.Models.ProductType> Model = UnitOfWork.GetRepository<ProductType>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        public ActionResult CollectionSearchBrandDropDownPartial()
        {
            List<Flock.Models.Brand> Model = UnitOfWork.GetRepository<Brand>().GetAll().Where(p => p.DeletedDT == null).ToList();
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        public ActionResult CollectionSearchLaunchDropDownPartial()
        {
            List<Flock.Models.Launch> Model = UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        public ActionResult CollectionSearchNameOrIDTextBoxPartial()
        {
            return PartialView("~/Views/Shared/SearchFilters/NameOrIDTextBoxPartial.cshtml");
        }

        public ActionResult CollectionSearchCollectionDropDownPartial()
        {
            List<Flock.Models.Collection> Model = UnitOfWork.GetRepository<Flock.Models.Collection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        [ValidateInput(false)]
        public ActionResult CollectionGridViewPartial()
        {
            //var model = UnitOfWork.GetRepository<vwCollectionOverview>().GetAll().OrderBy(p => p.Name).OrderByDescending(p => p.LaunchDate).ToList();
            //List<CollectionOverviewViewModel> Model = new List<CollectionOverviewViewModel>();
            //// somehow automagically bind model to Model... so we can utilise the validation rules...
            //for (int i = 0; i < model.Count; i++)
            //{
            //    CollectionOverviewViewModel COVM = new CollectionOverviewViewModel();
            //    COVM.CollectionID = model[i].CollectionID;
            //    COVM.ImageID = model[i].ImageID;
            //    COVM.Name = model[i].Name;
            //    COVM.Code = model[i].Code;
            //    COVM.PatternBookCode = model[i].PatternBookCode;
            //    COVM.OnDate = model[i].OnDate;
            //    COVM.OffDate = model[i].OffDate;
            //    COVM.StatusID = model[i].StatusID;
            //    COVM.BrandID = model[i].BrandID;
            //    COVM.LaunchID = model[i].LaunchID;
            //    COVM.ProductTypeID = model[i].ProductTypeID;
            //    COVM.AssociatedProducts = model[i].AssociatedProducts;
            //    COVM.RelatedCollections = model[i].RelatedCollections;
            //    COVM.VideoID = model[i].VideoID;
            //    COVM.VideoName = model[i].VideoName;
            //    COVM.BrochureID = model[i].BrochureID;
            //    COVM.BrochureName = model[i].BrochureName;
            //    COVM.EmbedCode = model[i].EmbedCode;
            //    COVM.BrandName = model[i].BrandName;
            //    COVM.LaunchName = model[i].LaunchName;
            //    COVM.ProductTypeName = model[i].ProductTypeName;
            //    COVM.URLKeywords = model[i].URLKeywords;

            //    //COV

            //    Model.Add(COVM);
            //}

            return PartialView("_CollectionGridViewPartial");//, Model);
        }

        [ValidateInput(false)]
        public ActionResult ProductToCollection_ProductDataViewPartial()
        {
            var model = UnitOfWork.GetRepository<vwProductSearch>().GetAll().Distinct().ToList();
            return PartialView("_ProductToCollection_ProductDataViewPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult ProductToCollection_ProductDataViewFilter(string Brands, string Launches, string ProductTypes, string TextSearch)
        {
            List<short> LaunchesList = new List<short>();
            List<byte> BrandsList = new List<byte>();
            List<byte> ProductTypesList = new List<byte>();


            List<PSR> Model = new List<PSR>();
            #region extremely long search stuff...
            if (Brands != null && Brands != "")
            {
                BrandsList = Brands.Split(',').ToList().ConvertAll(d => Convert.ToByte(d));
                Model = (
                    from s in UnitOfWork.GetRepository<vwProductSearch>().GetAll().Where(p => BrandsList.Contains(p.BrandID))
                    select new PSR()
                    {
                        ImageID = s.ImageID,
                        Name = s.Name,
                        Code = s.Code,
                        ProductID = s.ProductID,
                        SortOrder = s.SortOrder,
                        ProductTypeID = s.ProductTypeID,
                        BrandID = s.BrandID,
                        LaunchID = s.LaunchID
                    }).Distinct().ToList();
            }

            if (Launches != null && Launches != "")
            {
                LaunchesList = Launches.Split(',').ToList().ConvertAll(d => Convert.ToInt16(d));
                if (Model.Count > 0)
                {
                    Model = (
                        from s in Model.Where(p => LaunchesList.Contains(p.LaunchID))
                        select new PSR()
                        {
                            ImageID = s.ImageID,
                            Name = s.Name,
                            Code = s.Code,
                            ProductID = s.ProductID,
                            SortOrder = s.SortOrder,
                            ProductTypeID = s.ProductTypeID,
                            BrandID = s.BrandID,
                            LaunchID = s.LaunchID
                        }).Distinct().ToList();
                }
                else
                {
                    Model = (
                        from s in UnitOfWork.GetRepository<vwProductSearch>().GetAll().Where(p => LaunchesList.Contains(p.LaunchID))
                        select new PSR()
                        {
                            ImageID = s.ImageID,
                            Name = s.Name,
                            Code = s.Code,
                            ProductID = s.ProductID,
                            SortOrder = s.SortOrder,
                            ProductTypeID = s.ProductTypeID,
                            BrandID = s.BrandID,
                            LaunchID = s.LaunchID
                        }).Distinct().ToList();
                }
            }
            if (ProductTypes != null && ProductTypes != "")
            {
                ProductTypesList = ProductTypes.Split(',').ToList().ConvertAll(d => Convert.ToByte(d));
                if (Model.Count > 0)
                {
                    Model = (
                         from s in Model.Where(p => ProductTypesList.Contains(p.ProductTypeID))
                         select new PSR()
                         {
                             ImageID = s.ImageID,
                             Name = s.Name,
                             Code = s.Code,
                             ProductID = s.ProductID,
                             SortOrder = s.SortOrder,
                             ProductTypeID = s.ProductTypeID,
                             BrandID = s.BrandID,
                             LaunchID = s.LaunchID
                         }).Distinct().ToList();
                }
                else
                {
                    Model = (
                        from s in UnitOfWork.GetRepository<vwProductSearch>().GetAll().Where(p => ProductTypesList.Contains(p.ProductTypeID))
                        select new PSR()
                        {
                            ImageID = s.ImageID,
                            Name = s.Name,
                            Code = s.Code,
                            ProductID = s.ProductID,
                            SortOrder = s.SortOrder,
                            ProductTypeID = s.ProductTypeID,
                            BrandID = s.BrandID,
                            LaunchID = s.LaunchID
                        }).Distinct().ToList();
                }
            }
            if (TextSearch != null && TextSearch != "")
            {
                if (Model.Count > 0)
                {
                    Model = (
                        from s in Model.Where(p => p.Name.Contains(TextSearch))
                        select new PSR()
                        {
                            ImageID = s.ImageID,
                            Name = s.Name,
                            Code = s.Code,
                            ProductID = s.ProductID,
                            SortOrder = s.SortOrder,
                            ProductTypeID = s.ProductTypeID,
                            BrandID = s.BrandID,
                            LaunchID = s.LaunchID
                        }).Distinct().ToList();
                }
                else
                {
                    Model = (
                        from s in UnitOfWork.GetRepository<vwProductSearch>().GetAll().Where(p => p.Name.Contains(TextSearch) || p.Code.Contains(TextSearch))
                        select new PSR()
                        {
                            ImageID = s.ImageID,
                            Name = s.Name,
                            Code = s.Code,
                            ProductID = s.ProductID,
                            SortOrder = s.SortOrder,
                            ProductTypeID = s.ProductTypeID,
                            BrandID = s.BrandID,
                            LaunchID = s.LaunchID
                        }).Distinct().ToList();
                }
            }
            #endregion
            return PartialView("_ProductToCollection_ProductDataViewPartial", Model);
        }

        [ValidateInput(false)]
        public ActionResult ProductToCollection_CollectionDataViewPartial(long? collectionID)
        {
            if (collectionID == null)
            {
                CollectionForEditingViewModel model = new CollectionForEditingViewModel();
                return PartialView("_ProductToCollection_CollectionDataViewPartial", model);
            }
            else
            {
                List<PSR> model = (from s in UnitOfWork.GetRepository<vwProductSearch>().GetAll().Where(p => p.CollectionID == collectionID) select new PSR() { ImageID = s.ImageID, Name = s.Name, Code = s.Code, ProductID = s.ProductID, SortOrder = s.SortOrder }).Distinct().ToList();
                List<PSR> sortedList = model.OrderBy(p => p.SortOrder).ToList();
                CollectionForEditingViewModel CEVM = new CollectionForEditingViewModel();
                CEVM.CollectionID = (long)collectionID;
                CEVM.Items = sortedList;
                return PartialView("_ProductToCollection_CollectionDataViewPartial", CEVM);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateCollectionSortOrder(short CollectionID, List<SortDatum> Products)
        {
            // how do we update sort order here...
            // for each in Products, find Product_Collection record, update.
            var list = UnitOfWork.GetRepository<Product_Collection>().GetAll().Where(p => p.CollectionID == CollectionID).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                short so = Products.Where(p => p.ID == list[i].ProductID).Select(p => p.SortOrder).Single();
                list[i].SortOrder = (byte)so;
            }
            UnitOfWork.Save();

            var model = (from s in UnitOfWork.GetRepository<vwProductSearch>().GetAll().Where(p => p.CollectionID == CollectionID).OrderBy(p => p.SortOrder) select new vwProductSearch() { CollectionID = s.CollectionID, ImageID = s.ImageID, Name = s.Name, Code = s.Code, ProductID = s.ProductID }).Distinct();
            return PartialView("_ProductToCollection_CollectionDataViewPartial", model);
            //        traditional: true,
            //        data: { CollectionID: s.GetText(), Products: products },
            //        success: function(data) {
            //}
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateCollection(short CollectionID, string Products)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> products = ser.Deserialize<List<SortDatum>>(Products);

            foreach (SortDatum pd in products)
            {
                Product_Collection pc = UnitOfWork.GetRepository<Product_Collection>().GetAll().Where(p => p.CollectionID == Convert.ToInt16(CollectionID) && p.ProductID == pd.ID).SingleOrDefault();
                if (pc == null)
                {
                    pc = new Product_Collection() { ProductID = (int)pd.ID, CollectionID = Convert.ToInt16(CollectionID), SortOrder = (byte)pd.SortOrder };
                    UnitOfWork.GetRepository<Product_Collection>().Add(pc);
                }
                else
                    pc.SortOrder = (byte)pd.SortOrder;
            }

            UnitOfWork.Save();

            var Model = new CollectionProductAssociationContentsViewModel();
            Model.Products = new List<CollectionProductAssociationViewModel>();
            try
            {
                var PreModel = UnitOfWork.Context.Product_Collection_Select(CollectionID);
                foreach (var i in PreModel.OrderBy(p => p.SortOrder))
                {
                    Model.Products.Add(new CollectionProductAssociationViewModel()
                    {
                        Name = i.Name,
                        Code = i.Code,
                        ImageID = i.ImageID,
                        ProductID = i.ProductID,
                        ProductPageID = i.ProductPageID,
                        SortOrder = i.SortOrder,
                        Status = i.Status
                    });
                }
            }
            catch(Exception ex)
            {
                Response.StatusDescription = ex.Message;
            }
            return PartialView("CollectionContents", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveProductFromCollection(short CollectionID, long ProductID)
        {
            Product_Collection entry = UnitOfWork.GetRepository<Product_Collection>().GetAll().Where(p => p.CollectionID == CollectionID && p.ProductID == ProductID).SingleOrDefault();
            if(entry != null)
            {
                UnitOfWork.GetRepository<Product_Collection>().Delete(entry);
                UnitOfWork.Save();
            }
            
            var Model = new CollectionProductAssociationContentsViewModel();
            Model.Products = new List<CollectionProductAssociationViewModel>();
            var PreModel = UnitOfWork.Context.Product_Collection_Select(CollectionID);

            foreach (var i in PreModel)
            {
                Model.Products.Add(new CollectionProductAssociationViewModel()
                {
                    Name = i.Name,
                    Code = i.Code,
                    ImageID = i.ImageID,
                    ProductID = i.ProductID,
                    ProductPageID = i.ProductPageID,
                    SortOrder = i.SortOrder,
                    Status = i.Status
                });
            }
            
            return PartialView("CollectionContents", Model);
        }

        [HttpPost]
        public ActionResult RemoveAllProductsFromCollection(short CollectionID)
        {
            List<Product_Collection> entries = UnitOfWork.GetRepository<Product_Collection>().GetAll().Where(p => p.CollectionID == CollectionID).ToList();
            UnitOfWork.GetRepository<Product_Collection>().DeleteAll(entries);
            UnitOfWork.Save();
            
            var Model = new CollectionProductAssociationContentsViewModel();
            Model.Products = new List<CollectionProductAssociationViewModel>();
            var PreModel = UnitOfWork.Context.Product_Collection_Select(CollectionID);

            foreach (var i in PreModel)
            {
                Model.Products.Add(new CollectionProductAssociationViewModel()
                {
                    Name = i.Name,
                    Code = i.Code,
                    ImageID = i.ImageID,
                    ProductID = i.ProductID,
                    ProductPageID = i.ProductPageID,
                    SortOrder = i.SortOrder,
                    Status = i.Status
                });
            }

            return PartialView("CollectionContents", Model);
        }

        [HttpPost]
        public ActionResult SaveCollectionProducts(short CollectionID, string Products)
        {
            List<SortDatum> ProductsList = new JavaScriptSerializer().Deserialize<List<SortDatum>>(Products);

            UnitOfWork.GetRepository<Product_Collection>().DeleteAll(UnitOfWork.GetRepository<Product_Collection>().GetAll().Where(p => p.CollectionID == CollectionID));
            UnitOfWork.Save();

            foreach(var s in ProductsList)
            {
                Product_Collection pc = new Product_Collection() { CollectionID = CollectionID, ProductID = Convert.ToInt32(s.ID), SortOrder = Convert.ToByte(s.SortOrder) };
                
                UnitOfWork.GetRepository<Product_Collection>().Add(pc);
            }

            UnitOfWork.Save();

            var Model = new CollectionProductAssociationContentsViewModel();
            Model.Products = new List<CollectionProductAssociationViewModel>();
            var PreModel = UnitOfWork.Context.Product_Collection_Select(CollectionID);

            foreach (var i in PreModel)
            {
                Model.Products.Add(new CollectionProductAssociationViewModel()
                {
                    Name = i.Name,
                    Code = i.Code,
                    ImageID = i.ImageID,
                    ProductID = i.ProductID,
                    ProductPageID = i.ProductPageID,
                    SortOrder = i.SortOrder,
                    Status = i.Status
                });
            }
            return PartialView("CollectionContents", Model);
        }

        [AllowAnonymous]
        public ActionResult CollectionComposeHeaderProductTypeDropDownPartial(byte? SelectedProductTypeID)
        {
            ViewBag.CollectionHeaderSelectedProductTypeID = SelectedProductTypeID;

            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", UnitOfWork.GetRepository<ProductType>().GetAll().OrderBy(p => p.Name).ToList());
        }

        [AllowAnonymous]
        public ActionResult CollectionComposeHeaderBrandDropDownPartial(byte? SelectedBrandID)
        {
            ViewBag.CollectionHeaderSelectedBrandID = SelectedBrandID;

            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", UnitOfWork.GetRepository<Brand>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList());
        }

        [AllowAnonymous]
        public ActionResult CollectionComposeHeaderLaunchDropDownPartial(short? SelectedLaunchID)
        {
            ViewBag.CollectionHeaderSelectedLaunchID = SelectedLaunchID;

            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList());
        }

        [AllowAnonymous]
        public ActionResult CollectionComposeHeaderCollectionDropDownPartial(short? CollectionID, byte? ProductTypeID, byte? BrandID, short? LaunchID)
        {
            List<Collection> Model = UnitOfWork.GetRepository<Collection>().GetAll().OrderBy(p => p.Name).ToList();
            if (ProductTypeID != null)
                Model = Model.Where(p => p.ProductTypeID == ProductTypeID).ToList();
            if (BrandID != null)
                Model = Model.Where(p => p.BrandID == BrandID).ToList();
            if (LaunchID != null)
                Model = Model.Where(p => p.LaunchID == LaunchID).ToList();

            ViewBag.CollectionComposeHeaderSelectedCollectionID = CollectionID;

            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }


        [AllowAnonymous]
        public ActionResult CollectionContents(short? CollectionID)
        {
            //System.Data.Linq.ISingleResult<Product_SearchResult> Results = null;
            var Model = new CollectionProductAssociationContentsViewModel();

            Model.Products = new List<CollectionProductAssociationViewModel>();
            if (CollectionID != null)
            {
                var PreModel = UnitOfWork.Context.Product_Collection_Select(CollectionID);
                foreach(var i in PreModel)
                {
                    Model.Products.Add(new CollectionProductAssociationViewModel()
                    {
                        Name = i.Name,
                        Code = i.Code,
                        ImageID = i.ImageID,
                        ProductID = i.ProductID,
                        ProductPageID = i.ProductPageID,
                        SortOrder = i.SortOrder,
                        Status = i.Status
                    });
                }
            }
            return PartialView("CollectionContents", Model);
        }

        [HttpPost]
        public ActionResult SuggestCollectionContents(short? CollectionID)
        {
            var Model = new CollectionProductAssociationContentsViewModel();
            Model.Products = new List<CollectionProductAssociationViewModel>();
            if (CollectionID != null)
            {
                var PreModel = UnitOfWork.Context.Product_Collection_Suggest(CollectionID);// GetAll().Where(p => p.CollectionID == CollectionID).OrderBy(p => p.SortOrder).ToList();
                foreach (var i in PreModel)
                {
                    Model.Products.Add(new CollectionProductAssociationViewModel()
                    {
                        Name = i.Name,
                        Code = i.Code,
                        ImageID = i.ImageID,
                        ProductID = i.ProductID,
                        ProductPageID = i.ProductPageID,
                        SortOrder = i.SortOrder,
                        Status = i.Status
                    });
                }
            }
            return PartialView("CollectionContents", Model);
        }

        [AllowAnonymous]
        public ActionResult CollectionOverviewBrandDropDownPartial(byte? SelectedBrandID)
        {
            List<Brand> Model = UnitOfWork.GetRepository<Brand>().GetAll().Where(p => p.DeletedDT == null).ToList();
            ViewBag.SelectedBrandID = SelectedBrandID;
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult CollectionOverviewLaunchDropDownPartial(short? SelectedLaunchID)
        {
            List<Launch> Model = UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();
            ViewBag.SelectedLaunchID = SelectedLaunchID;
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult CollectionOverviewProductTypeDropDownPartial(short? SelectedProductTypeID)
        {
            List<ProductType> Model = UnitOfWork.GetRepository<ProductType>().GetAll().OrderBy(p => p.Name).ToList();
            ViewBag.SelectedProductTypeID = SelectedProductTypeID;
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult CollectionOverviewEnabledDropDownPartial(bool? EnabledState)
        {
            return PartialView("~/Views/Shared/SearchFilters/EnabledDropDownPartial.cshtml", EnabledState);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CollectionGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.ViewModels.CollectionOverviewViewModel item)
        {
            bool hasError = false;
            if (item.Name == null || item.Name == "")
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Name for this collection.<br/>";
            }
            if (item.Code == null || item.Code == "")
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Code for this collection.<br/>";
            }
            if (item.BrandID == 0)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Brand for this collection.<br/>";
            }
            if (item.LaunchID == 0)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Launch for this collection.<br/>";
            }
            if (item.ProductTypeID == 0)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Product Type for this collection<br/>";
            }
            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Collection c = new Collection();
                        c.URLKeywords = "";
                        c.BrandID = item.BrandID;
                        c.Code = item.Code;
                        c.PBCode = item.PatternBookCode;
                        c.OnDate = item.OnDate;
                        c.OffDate = item.OffDate;
                        c.LaunchID = item.LaunchID;
                        c.Name = item.Name;
                        c.ProductTypeID = item.ProductTypeID;
                        UnitOfWork.GetRepository<Collection>().Add(c);
                        UnitOfWork.Save();
                        UnitOfWork.Context.Collection_Language_Initialise();

                        UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, null, null, c.CollectionID, null, false, false);
                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] += e.Message;
                    }
                }
                else
                    ViewData["EditError"] += "Please, correct all errors.";
            }

            List<vwCollectionOverview> Model = UnitOfWork.GetRepository<vwCollectionOverview>().GetAll().ToList();
            return PartialView("_CollectionGridViewPartial", Model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CollectionGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.Models.vwCollectionOverview item)
        {
            try
            {
                Collection c = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == item.CollectionID).Single();
                c.DeletedDT = DateTime.Now;
                UnitOfWork.Save();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            List<vwCollectionOverview> Model = UnitOfWork.GetRepository<vwCollectionOverview>().GetAll().ToList();

            return PartialView("_CollectionGridViewPartial", Model);
        }

        [HttpPost]
        public ActionResult CollectionGridViewPartialCustomCommand(string commandType, string keyString, string dateString, bool? newState)
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
                        List<Collection> toEnable = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => keys.Contains(p.CollectionID)).ToList();
                        foreach (Collection c in toEnable)
                        {
                            c.OnDate = DateTime.Parse(dateString);

                            if(c.OffDate != null)
                            {
                                if (date < c.OffDate || c.OffDate == null)
                                    c.OnDate = date;
                                else
                                {
                                    errorString += "Collection " + c.CollectionID.ToString() + " was not updated because the provided On Date falls after the collection's assigned Off Date.\r\n\r\n";
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
                        List<Collection> toDisable = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => keys.Contains(p.CollectionID)).ToList();
                        foreach (Collection c in toDisable)
                        {
                            if (date >= c.OnDate || c.OnDate == null)
                                c.OffDate = date;
                            else
                            {
                                errorString += "Collection " + c.CollectionID.ToString() + " was not updated because the provided Off Date preceeds the collection's assigned On Date.\r\n\r\n";
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
                        List<Collection> toDelete = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => keys.Contains(p.CollectionID)).ToList();
                        foreach (Collection c in toDelete)
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

                case "copypodtoslideshow":
                    List<Collection> copyAB = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => keys.Contains(p.CollectionID)).ToList();
                    foreach (Collection c in copyAB)
                    {
                        var src = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == c.CollectionID && p.TemplateImageID == 1).ToList();
                        var dest = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == c.CollectionID && p.TemplateImageID == 2).ToList();
                        UnitOfWork.GetRepository<Collection_Image>().DeleteAll(dest);
                        UnitOfWork.Save();

                        foreach (var i in src)
                        {
                            UnitOfWork.GetRepository<Collection_Image>().Add(new Collection_Image()
                            {
                                CollectionID = c.CollectionID,
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
                    List<Collection> copyBA = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => keys.Contains(p.CollectionID)).ToList();
                    foreach (Collection c in copyBA)
                    {
                        var src2 = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == c.CollectionID && p.TemplateImageID == 2).ToList();
                        var dest2 = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == c.CollectionID && p.TemplateImageID == 1).ToList();
                        UnitOfWork.GetRepository<Collection_Image>().DeleteAll(dest2);
                        UnitOfWork.Save();

                        foreach (var i in src2)
                        {
                            UnitOfWork.GetRepository<Collection_Image>().Add(new Collection_Image()
                            {
                                CollectionID = c.CollectionID,
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
                case "setnewstate":
                    try
                    {
                        List<Collection> toSet = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => keys.Contains(p.CollectionID)).ToList();
                        foreach (Collection c in toSet)
                        {
                            c.New = newState.HasValue ? newState.Value : false;
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

            if (hasError)
            {
                Response.StatusCode = 400;
                return Content(errorString);
            }

            List<vwCollectionOverview> Model = UnitOfWork.GetRepository<vwCollectionOverview>().GetAll().ToList();

            return PartialView("_CollectionGridViewPartial", Model);
        }

        [AllowAnonymous]
        public ActionResult CollectionGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.ViewModels.CollectionOverviewViewModel item)
        {
            bool hasError = false;
            if (item.Name == null || item.Name == "")
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Name for this collection.<br/>";
            }
            if (item.Code == null || item.Code == "")
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Code for this collection.<br/>";
            }
            if (item.BrandID == 0)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Brand for this collection.<br/>";
            }
            if (item.LaunchID == 0)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Launch for this collection.<br/>";
            }
            if (item.ProductTypeID == 0)
            {
                hasError = true;
                ViewData["EditError"] += "Please set a Product Type for this collection<br/>";
            }
            if (!hasError)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var thisCollection = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == item.CollectionID).SingleOrDefault();
                        if (thisCollection != null)
                        {
                            thisCollection.BrandID = item.BrandID;
                            thisCollection.PBCode = item.PatternBookCode;
                            thisCollection.OnDate = item.OnDate;
                            thisCollection.OffDate = item.OffDate;
                            thisCollection.Code = item.Code;
                            thisCollection.Name = item.Name;
                            thisCollection.LaunchID = item.LaunchID;
                            thisCollection.ProductTypeID = item.ProductTypeID;

                            UnitOfWork.Save();

                            var model = UnitOfWork.GetRepository<vwCollectionOverview>().GetAll().ToList();
                            return PartialView("_CollectionGridViewPartial", model);
                        }
                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] = e.Message;
                    }
                }
                else
                    ViewData["EditError"] = "Please, correct all errors.";
            }

            var Model = UnitOfWork.GetRepository<vwCollectionOverview>().GetAll().ToList();

            return PartialView("_CollectionGridViewPartial", Model);
        }

        [AllowAnonymous]
        public ActionResult CollectionComposeCollectionSelectHeader(short? collectionid, byte? ProductTypeid, byte? brandid, short? launchid)
        {
            CollectionHeaderFilters Model = new CollectionHeaderFilters() { CollectionID = collectionid, LaunchID = launchid, ProductTypeID = ProductTypeid, BrandID = brandid };

            return PartialView("CollectionSelectHeader", Model);
        }
        [AllowAnonymous]
        public ActionResult ProductAssociationHeaderProductTypeDropDownPartial(byte? SelectedProductTypeID)
        {
            ViewBag.ProductAssociationSelectedProductTypeID = SelectedProductTypeID;

            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", UnitOfWork.GetRepository<ProductType>().GetAll().OrderBy(p => p.Name).ToList());
        }

        [AllowAnonymous]
        public ActionResult ProductAssociationHeaderBrandDropDownPartial(byte? SelectedBrandID)
        {
            ViewBag.ProductAssociationSelectedBrandID = SelectedBrandID;

            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", UnitOfWork.GetRepository<Brand>().GetAll().Where(p => p.DeletedDT == null).ToList());
        }

        [AllowAnonymous]
        public ActionResult ProductAssociationHeaderLaunchDropDownPartial(short? SelectedLaunchID)
        {
            ViewBag.ProductAssociationSelectedLaunchID = SelectedLaunchID;

            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", UnitOfWork.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList());
        }

        [AllowAnonymous]
        public ActionResult ProductAssociationHeaderCollectionDropDownPartial(short? CollectionID, byte? ProductTypeID, byte? BrandID, short? LaunchID)
        {
            List<Collection> Model = UnitOfWork.GetRepository<Collection>().GetAll().OrderBy(p => p.Name).ToList();
            if (ProductTypeID != null)
                Model = Model.Where(p => p.ProductTypeID == ProductTypeID).ToList();
            if (BrandID != null)
                Model = Model.Where(p => p.BrandID == BrandID).ToList();
            if (LaunchID != null)
                Model = Model.Where(p => p.LaunchID == LaunchID).ToList();

            ViewBag.ProductAssociationHeaderSelectedCollectionID = CollectionID;

            return PartialView("~/Views/Shared/SearchFilters/CollectionDropDownPartial.cshtml", Model);
        }

        [AllowAnonymous]
        public ActionResult ProductAssociationCollectionSelectHeader(short? collectionid, byte? ProductTypeid, byte? brandid, short? launchid)
        {
            CollectionHeaderFilters Model = new CollectionHeaderFilters() { CollectionID = collectionid, LaunchID = launchid, ProductTypeID = ProductTypeid, BrandID = brandid };

            return PartialView("CollectionSelectHeader", Model);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateCollectionRelatedCollections(short CollectionID, string Collections)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                List<SortDatum> CollectionList = ser.Deserialize<List<SortDatum>>(Collections);

                foreach (SortDatum datum in CollectionList.Where(p => p.ID != CollectionID))
                {
                    Collection_RelatedCollection crc = UnitOfWork.GetRepository<Collection_RelatedCollection>().GetAll().Where(p => p.CollectionID == CollectionID && p.RelatedCollectionID == datum.ID).SingleOrDefault();
                    if (crc == null)
                    {
                        crc = new Collection_RelatedCollection() { CollectionID = CollectionID, RelatedCollectionID = (short)datum.ID, SortOrder = (byte)datum.SortOrder };
                        UnitOfWork.GetRepository<Collection_RelatedCollection>().Add(crc);
                    }
                    else
                        crc.SortOrder = (byte)datum.SortOrder;

                    UnitOfWork.Save();
                }
            }
            catch (Exception e)
            {
                Response.StatusDescription = e.Message;
                // log e
            }

            List<vwCollection_RelatedCollection> Model = UnitOfWork.GetRepository<vwCollection_RelatedCollection>().GetAll().Where(p => p.CollectionID == CollectionID).OrderBy(p => p.SortOrder).ToList();

            return PartialView("RelatedCollections", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateCollectionImageArea(short CollectionID, string Images, byte TemplateImageID)
        {
            try
            {
                UnitOfWork.Context.Collection_Image_Merge(CollectionID, Images, TemplateImageID);
            }
            catch (Exception e)
            {
                Response.StatusDescription = e.Message;
                // log e
            }
            List<Collection_Image> Model = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == TemplateImageID).OrderBy(p => p.SortOrder).ToList();
            switch (TemplateImageID)
            {
                case 1:
                    return PartialView("CollectionPodImages", Model);
                case 2:
                    return PartialView("CollectionSlideShowImagesPartial", Model);
                case 3:
                    return PartialView("CollectionSP1Image", Model);
                case 4:
                    return PartialView("CollectionSP2Image", Model);
                case 5:
                    return PartialView("CollectionViewAllImage", Model);
                case 6:
                    return PartialView("CollectionBrochureImage", Model);
                case 7:
                    return PartialView("CollectionSP3Image", Model);
                case 8:
                    return PartialView("CollectionVideoImage", Model);
                default:
                    return Content("");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveImageFromCollectionArea(short CollectionID, short TemplateImageID, long ImageID)
        {
            try
            {
                Collection_Image CI = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.ImageID == ImageID && p.TemplateImageID == TemplateImageID).Single();
                UnitOfWork.GetRepository<Collection_Image>().Delete(CI);
                UnitOfWork.Save();

                List<Collection_Image> SortOrderFix = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == TemplateImageID).OrderBy(p => p.SortOrder).ToList();

                for (byte i = 0; i < SortOrderFix.Count; i++)
                {
                    SortOrderFix[i].SortOrder = i;
                }

                UnitOfWork.Save();
            }
            catch (Exception e)
            {
                Response.StatusDescription = e.Message;
                // log e
            }

            List<Collection_Image> Model = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == TemplateImageID).OrderBy(p => p.SortOrder).ToList();
            switch (TemplateImageID)
            {
                case 1:
                    return PartialView("CollectionPodImages", Model);
                case 2:
                    return PartialView("CollectionSlideShowImagesPartial", Model);
                case 3:
                    return PartialView("CollectionSP1Image", Model);
                case 4:
                    return PartialView("CollectionSP2Image", Model);
                case 5:
                    return PartialView("CollectionViewAllImage", Model);
                case 6:
                    return PartialView("CollectionBrochureImage", Model);
                case 7:
                    return PartialView("CollectionSP3Image", Model);
                case 8:
                    return PartialView("CollectionVideoImage", Model);
                default:
                    return Content("");
            }
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult UpdateCollectionSlideshow(short CollectionID, string Images)
        //{
        //    try
        //    {
        //        JavaScriptSerializer ser = new JavaScriptSerializer();
        //        List<ImageDatum> ImagesList = ser.Deserialize<List<ImageDatum>>(Images);
        //        foreach (ImageDatum id in ImagesList)
        //        {
        //            Collection_Image cd = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.ImageID == id.ImageID && p.TemplateImageID == 1).SingleOrDefault();
        //            if (cd == null)
        //            {
        //                cd = new Collection_Image() { CollectionID = CollectionID, TemplateImageID = 1, ImageID = id.ImageID, SortOrder = (byte)id.SortOrder };
        //                UnitOfWork.GetRepository<Collection_Image>().Add(cd);
        //            }
        //            else
        //                cd.SortOrder = (byte)id.SortOrder;
        //            UnitOfWork.Save();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        // log e
        //    }
        //    List<Collection_Image> Model = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 1).OrderBy(p => p.SortOrder).ToList();
        //    //List<Collection_Image> SortedModel = Model.OrderBy(p => p.SortOrder).ToList();
        //    return PartialView("~/Views/CMS/CollectionSlideShowImagesPartial.cshtml", Model);
        //}
        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult RemoveImageFromCollectionArea(short CollectionID, long ImageID, byte TemplateImageID)
        //{
        //    try
        //    {
        //        Collection_Image CI = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.ImageID == ImageID && p.TemplateImageID == TemplateImageID).Single();
        //        UnitOfWork.GetRepository<Collection_Image>().Delete(CI);
        //        UnitOfWork.Save();
        //    }
        //    catch (Exception e)
        //    {
        //        // log e
        //    }
        //    List<Collection_Image> Model = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == TemplateImageID).ToList();
        //    if(TemplateImageID == 1)
        //        return PartialView("~/Views/CMS/CollectionPodImages.cshtml", Model);
        //    else
        //        return PartialView("~/Views/CMS/CollectionSlideshowImagesPartial.cshtml", Model);
        //}
        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult RemoveImageFromCollectionPod(short CollectionID)
        //{
        //    try
        //    {
        //        Collection_Image CI = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 2).Single();
        //        UnitOfWork.GetRepository<Collection_Image>().Delete(CI);
        //        UnitOfWork.Save();
        //    }
        //    catch (Exception e)
        //    {
        //        // log e
        //    }
        //    long? Model = null;
        //    return PartialView("~/Views/CMS/CollectionPodImage.cshtml", Model);
        //}

        public struct SortDatum
        {
            public long ID;
            public short SortOrder;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveRelatedCollectionFromCollection(short CollectionID, short RelatedCollectionID)
        {
            Collection_RelatedCollection CRC = UnitOfWork.GetRepository<Collection_RelatedCollection>().GetAll().Where(p => p.CollectionID == CollectionID && p.RelatedCollectionID == RelatedCollectionID).Single();
            UnitOfWork.GetRepository<Collection_RelatedCollection>().Delete(CRC);
            UnitOfWork.Save();

            List<vwCollection_RelatedCollection> Model = UnitOfWork.GetRepository<vwCollection_RelatedCollection>().GetAll().Where(p => p.CollectionID == CollectionID).ToList();
            
            return PartialView("RelatedCollections", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CollectionHtmlEditorSavePartial(short CollectionID, string Html)
        {
            // save collection description HTML.
            Flock.Models.Collection c = UnitOfWork.GetRepository<Flock.Models.Collection>().GetAll().Where(p => p.CollectionID == CollectionID).Single();

            c.Description = Html;

            UnitOfWork.Save();

            UnitOfWork.Context.ReplaceLocalURLPlaceholders_Collection(CollectionID, null, null, null, null, null, false, false);

            return PartialView("CollectionHTMLEditorPartial", Html);
        }


        [HttpPost]
        public ActionResult CopyImages(short CollectionID, string Direction)
        {
            try
            {
                switch (Direction)
                {
                    case "PodSlideshow":
                        var src = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 1).ToList();
                        var dest = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 2).ToList();
                        UnitOfWork.GetRepository<Collection_Image>().DeleteAll(dest);
                        UnitOfWork.Save();

                        foreach (var i in src)
                        {
                            UnitOfWork.GetRepository<Collection_Image>().Add(new Collection_Image()
                            {
                                CollectionID = CollectionID,
                                TemplateImageID = 2,
                                ImageID = i.ImageID,
                                AltText = i.AltText,
                                TitleText = i.TitleText,
                                SortOrder = i.SortOrder
                            });
                        }
                        UnitOfWork.Save();
                        

                        List<Collection_Image> SortOrderFix = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 2).OrderBy(p => p.SortOrder).ToList();

                        for (byte i = 0; i < SortOrderFix.Count; i++)
                        {
                            SortOrderFix[i].SortOrder = i;
                        }

                        UnitOfWork.Save();

                        var Model = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 2).OrderBy(p => p.SortOrder).ToList();
                        return PartialView("CollectionSlideshowImagesPartial", Model);
                    case "SlideshowPod":
                        var src2 = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 2).ToList();
                        var dest2 = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 1).ToList();
                        UnitOfWork.GetRepository<Collection_Image>().DeleteAll(dest2);
                        UnitOfWork.Save();

                        foreach (var i in src2)
                        {
                            UnitOfWork.GetRepository<Collection_Image>().Add(new Collection_Image()
                            {
                                CollectionID = CollectionID,
                                TemplateImageID = 1,
                                ImageID = i.ImageID,
                                AltText = i.AltText,
                                TitleText = i.TitleText,
                                SortOrder = i.SortOrder
                            });
                        }
                        UnitOfWork.Save();
                        

                        List<Collection_Image> SortOrderFix2 = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 1).OrderBy(p => p.SortOrder).ToList();

                        for (byte i = 0; i < SortOrderFix2.Count; i++)
                        {
                            SortOrderFix2[i].SortOrder = i;
                        }

                        UnitOfWork.Save();

                        var Model2 = UnitOfWork.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 1).OrderBy(p => p.SortOrder).ToList();
                        return PartialView("CollectionPodImages", Model2);
                    default:
                        return Content("");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return Content(ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult RemoveCollectionVideo(short CollectionID)
        {
            Collection c = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == CollectionID).Single();
            c.VideoID = null;
            UnitOfWork.Save();

            CollectionViewModelBuilder CollectionBuilder = new CollectionViewModelBuilder(UnitOfWork);

            CollectionViewModel CVM = CollectionBuilder.Build(CollectionID);

            return PartialView("CollectionComposePartial", CVM);

        }

        [HttpPost]
        public ActionResult RemoveCollectionBrochure(short CollectionID)
        {
            Collection c = UnitOfWork.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == CollectionID).Single();
            c.BrochureID = null;
            UnitOfWork.Save();

            CollectionViewModelBuilder CollectionBuilder = new CollectionViewModelBuilder(UnitOfWork);

            CollectionViewModel CVM = CollectionBuilder.Build(CollectionID);

            return PartialView("CollectionComposePartial", CVM);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateCollectionCountryRestrictions(short CollectionID, string Countries)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> CountryRestrictions = ser.Deserialize<List<SortDatum>>(Countries);

            List<Collection_Country> Current = UnitOfWork.GetRepository<Collection_Country>().GetAll().Where(p => p.CollectionID == CollectionID).ToList();
            List<SortDatum> CurrentCountries = new List<SortDatum>();
            foreach (Collection_Country country in Current)
            {
                CurrentCountries.Add(new SortDatum() { ID = country.CountryID, SortOrder = 0 });
            }
            List<SortDatum> Removals = CurrentCountries.Except(CountryRestrictions).ToList();

            foreach (SortDatum datum in Removals)
            {
                Collection_Country remove = UnitOfWork.GetRepository<Collection_Country>().GetAll().Where(p => p.CollectionID == CollectionID && p.CountryID == datum.ID).Single();
                UnitOfWork.GetRepository<Collection_Country>().Delete(remove);
            }
            UnitOfWork.Save();

            foreach (SortDatum datum in CountryRestrictions)
            {
                Collection_Country bc = UnitOfWork.GetRepository<Collection_Country>().GetAll().Where(p => p.CollectionID == CollectionID && p.CountryID == datum.ID).SingleOrDefault();

                if (bc == null)
                {
                    bc = new Collection_Country() { CollectionID = CollectionID, CountryID = (short)datum.ID };
                    UnitOfWork.GetRepository<Collection_Country>().Add(bc);
                }
            }

            UnitOfWork.Save();

            List<Country> Model = UnitOfWork.GetRepository<Collection_Country>().GetAll().Where(p => p.CollectionID == CollectionID).Select(p => p.Country).ToList();

            return PartialView("CollectionCountryRestrictionsPartial", Model);
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
                string ExportDirectory = "C:\\FlockCollectionExports\\Exports";

                if (Directory.Exists(ExportDirectory))
                    Directory.Delete(ExportDirectory, true);
                Directory.CreateDirectory(ExportDirectory);

                List<short> _launchIDs = LaunchIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToList();

                foreach (short LaunchID in _launchIDs)
                {
                    IMultipleResults r = UnitOfWork.Context.CollectionImage_Export(LaunchID);

                    List<CollectionImage_Export_CollectionImageResult> Images = r.GetResult<CollectionImage_Export_CollectionImageResult>().ToList();
                    List<CollectionImage_Export_LaunchResult> Launches = r.GetResult<CollectionImage_Export_LaunchResult>().ToList();
                    List<CollectionImage_Export_BrandResult> Brands = r.GetResult<CollectionImage_Export_BrandResult>().ToList();
                    List<CollectionImage_Export_ProductTypeResult> ProductTypes = r.GetResult<CollectionImage_Export_ProductTypeResult>().ToList();
                    List<CollectionImage_Export_CollectionResult> Collections = r.GetResult<CollectionImage_Export_CollectionResult>().ToList();

                    foreach (var l in Launches)
                    {
                        var LaunchDirectory = ExportDirectory + "\\" + l.Launch;
                        var LaunchSpreadsheetName = l.Launch + ".xlsx";

                        Directory.CreateDirectory(LaunchDirectory);

                        OpenXmlHelper.CreateCollectionImageExportSpreadsheet(LaunchDirectory + "\\" + LaunchSpreadsheetName, LaunchSpreadsheetName, Images);

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
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }


        private async System.Threading.Tasks.Task CopyImagesForExport(List<CollectionImage_Export_CollectionImageResult> images)
        {
            string StartDirectory = AppDomain.CurrentDomain.BaseDirectory + "//imagelibrary//1024//";
            string EndDirectory = @"C:\FlockCollectionExports\Exports";

            foreach (var i in images)
            {
                if (i.ImageID != null)
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
