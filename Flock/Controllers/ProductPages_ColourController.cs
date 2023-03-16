using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.Models;
using Flock.ViewModels;
using DevExpress.Web.Mvc;
using Flock.DAL;
using Newtonsoft.Json;

namespace Flock.Controllers
{
    public partial class ProductPagesController : Controller
    {
        public ActionResult Colour()
        {
            return View("colour/colour");
        }

        [HttpPost]
        public ActionResult loadProductPagesOfColour(byte? colourId)
        {
            if (colourId == 0)
                colourId = null;

            var results = new UnitOfWork<FlockDataContext>().GetRepository<ProductPage>().GetAll().Where(p => p.ColourID == colourId).ToList();

            return Content(JsonConvert.SerializeObject(results, new JsonSerializerSettings() {
                PreserveReferencesHandling = PreserveReferencesHandling.None, ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        public ActionResult ColourGridViewPartial()
        {
            //string _selectedIDs = Request.Params["selectedIDs"];
            //ViewData["_selectedIDs"] = _selectedIDs;

            //var Model = new ProductPageGenerateGridViewModel();

            //Model.Merchandisers = new List<ProductPage_Generate_Select_CoreResult>();
            //Model.Products = new List<ProductPage_Generate_Select_ProductResult>();
            //Model.Brands = new List<Brand_SelectResult>();
            //Model.ProductTypes = new List<ProductType_SelectResult>();
            //Model.Launches = new List<Launch_SelectResult>();
            //Model.NamingConventions = UnitOfWork.GetRepository<NamingConvention>().GetAll().OrderBy(p => p.NamingConventionID).ToList();

            return PartialView("colour/_colourgridviewpartial");
        }

        public ActionResult ColourGridViewBatchEditingUpdate(MVCxGridViewBatchUpdateValues<ProductPage, int> updateValues)
        {
            foreach (var productPage in updateValues.Update)
            {
                var pp = UnitOfWork.Context.ProductPages.Where(p => p.ProductPageID == productPage.ProductPageID).Single();

                pp.ColourID = productPage.ColourID;
            }

            UnitOfWork.Context.SubmitChanges();
            
            return PartialView("colour/_colourgridviewpartial");
        }
    }
}

namespace Flock.Models
{
    public class ProductPageColourRecord
    {
        public int ProductPageID { get; set; }
        public byte ProductTypeID { get; set; }
        public ProductType ProductType { get; set; }
        public short LaunchID { get; set; }
        public Launch Launch { get; set; }
        public byte BrandID { get; set; }
        public Brand Brand { get; set; }
        public byte? ColourID { get; set; }
        public Colour Colour { get; set; }
        public Image Image { get; set; }
    }
}