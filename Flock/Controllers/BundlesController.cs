using DevExpress.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public class BundlesController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

        public BundlesController() { }

        public BundlesController(UserManager userManager)
        {
            try
            {
                UserManager = userManager;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }

        private UserManager _userManager;
        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Bundles
        public ActionResult Index()
        {
            BundlesViewModel Model = new BundlesViewModel(UoW);
            return View(Model);
        }

        public ActionResult BundlesPriceListDropDown()
        {
            List<Flock.Models.PriceList> PriceLists = UoW.GetRepository<PriceList>().GetAll().OrderBy(p => p.PriceListID).ToList();

            return PartialView("~/Views/Shared/SearchFilters/PriceListDropDownPartial.cshtml", PriceLists);
        }

        [HttpPost]
        public ActionResult ProductCodeAutocomplete(PassAutocomplete objAutocomplete)
        {
            var results = UoW.Context.ProductCode_Bundle_Autocomplete(objAutocomplete.term).ToList();

            return Json(results, JsonRequestBehavior.DenyGet);
        }

        public ActionResult BundleGridViewPartial(int? bundleId)
        {
            BundleEditModel Model = new BundleEditModel(bundleId, UoW);

            return PartialView("_BundleGridViewPartial", Model);
        }

        public ActionResult BundleGridViewAddNew([ModelBinderAttribute(typeof(DevExpressEditorsBinder))]BundleEditModel item)
        {
            if (item.BundledProducts == null || item.BundledProducts.Count == 0)
            {
                ModelState.AddModelError("BundledProducts", "Empty bundles are not permitted.");
            }
            else
            {
                ModelState.Remove(ModelState.Single(m => m.Key == "ProductBundle"));
            }

            var bundleProductId = (int?)null;

            if (ModelState.IsValid)
            {
                var sameCodeCheck = UoW.GetRepository<Product>().GetAll().Where(p => p.Code == item.Code).SingleOrDefault();

                if (sameCodeCheck == null)
                {
                    var p = new Models.Product()
                    {
                      CreatedDT = DateTime.Now,
                      ModifiedDT = DateTime.Now,
                      Code = item.Code,
                      Status = 'C',
                      Enabled = true,
                      BrandID = item.BrandID.Value,
                      LaunchID = item.LaunchID.Value,
                      ProductTypeID = item.ProductTypeID.Value,
                      OrderFormName = item.OrderFormName,
                      BasketName = item.BasketName,
                      UKVATCode = 'S'
                    };

                    UoW.GetRepository<Product>().Add(p);

                    UoW.Save();

                    UoW.Context.Product_Language_Initialise();

                    bundleProductId = p.ProductID;
                }
                else
                {
                    ModelState.AddModelError("Code", "Code already exists!");
                }

                foreach (var b in item.BundledProducts)
                {
                    UoW.GetRepository<ProductBundle>().Add(new ProductBundle()
                    {
                        BundleProductID = bundleProductId.Value,
                        ProductID = b.ProductID,
                        Quantity = b.Quantity
                    });
                }

                UoW.Save();
            }

            item.RebuildRequirements(UoW);

            return PartialView("_BundleGridViewPartial", item);
        }

        public ActionResult BundleGridViewUpdate([ModelBinderAttribute(typeof(DevExpressEditorsBinder))]BundleEditModel item)
        {
            if (ModelState.IsValid)
            {
                var extant = UoW.GetRepository<Product>().GetAll().Where(p => p.ProductID == item.BundleID).SingleOrDefault();

                if (extant != null)
                {
                    extant.Code = item.Code;
                    extant.BrandID = item.BrandID.Value;
                    extant.LaunchID = item.LaunchID.Value;
                    extant.ProductTypeID = item.ProductTypeID.Value;
                    extant.OrderFormName = item.OrderFormName;
                    extant.BasketName = item.BasketName;

                    // do remember to call Update with the selected priceListId.

                    // ideally we can just delete all these records and add new ones.
                    var toDelete = UoW.GetRepository<ProductBundle>().GetAll().Where(p => p.BundleProductID == item.BundleID).ToList();
                    UoW.GetRepository<ProductBundle>().DeleteAll(toDelete);

                    UoW.Save();

                    if (item.BundledProducts == null)
                    {

                    }
                    else
                    {
                        foreach (var b in item.BundledProducts)
                        {
                            UoW.GetRepository<ProductBundle>().Add(new ProductBundle()
                            {
                                BundleProductID = b.BundleProductID,
                                ProductID = b.ProductID,
                                Quantity = b.Quantity
                            });
                        }

                        UoW.Save();
                    }

                    UoW.Context.ProductBundle_Price_Update(null, item.BundleID);
                }

                BundleEditModel Model = new BundleEditModel(null, UoW);

                return PartialView("_BundleGridViewPartial", Model);
            }
            else
            {
                return PartialView("_BundleGridViewPartial", item);
            }
        }

        public ActionResult BundleGridViewDelete()
        {
            return PartialView("_BundleGridViewPartial");
        }


        [HttpPost]
        //[CustomAuthorize(Roles = "Admin")]
        public ActionResult BundleGridViewCustomAction(string commandType, List<int> bundleIds, List<byte> priceListIds)
        {
            switch(commandType)
            {
                case "AssignMarkets":
                    foreach(var i in priceListIds)
                    {
                        UoW.Context.ProductBundle_Price_Initialise(null, bundleIds, i);
                    }
                    break;
                case "DeleteMarkets":
                    foreach (var i in priceListIds)
                    {
                        UoW.Context.ProductBundle_Price_Delete(bundleIds, i);
                    }
                    break;
                default: break;
            }

            BundleEditModel Model = new BundleEditModel(null, UoW);

            return PartialView("_BundleGridViewPartial", Model);
        }
    }
}