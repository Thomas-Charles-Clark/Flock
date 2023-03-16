using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.Models;
using Flock.ViewModels;
using DevExpress.Web.Mvc;

namespace Flock.Controllers
{
    public partial class ProductPagesController : Controller
    {
        /*
            [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ProductPage_Generate_Select")]
            [ResultType(typeof(ProductPage_Generate_Select_CoreResult))]
            [ResultType(typeof(ProductPage_Generate_Select_ProductResult))]
            [ResultType(typeof(Brand_SelectResult))]
            [ResultType(typeof(ProductType_SelectResult))]
            [ResultType(typeof(Launch_SelectResult))]
            public IMultipleResults ProductPage_Generate_Select()
         */
        // GET: ProductPages_Merchandiser
        [Authorize(Roles = "Admin, FlockUser")]
        public ActionResult Generate()
        {
            return View("generator/generate");
        }

        public ActionResult GeneratorGridViewPartial()
        {
            string _selectedIDs = Request.Params["selectedIDs"];
            ViewData["_selectedIDs"] = _selectedIDs;

            var Model = new ProductPageGenerateGridViewModel();

            Model.Merchandisers = new List<ProductPage_Generate_Select_CoreResult>();
            Model.Products = new List<ProductPage_Generate_Select_ProductResult>();
            Model.Brands = new List<Brand_SelectResult>();
            Model.ProductTypes = new List<ProductType_SelectResult>();
            Model.Launches = new List<Launch_SelectResult>();
            Model.NamingConventions = UnitOfWork.GetRepository<NamingConvention>().GetAll().OrderBy(p => p.NamingConventionID).ToList();

            return PartialView("generator/_gridviewpartial", Model);
        }

        public ActionResult GeneratorGridViewPartialBind(Int16? launchID)
        {
            string _selectedIDs = Request.Params["selectedIDs"];
            ViewData["_selectedIDs"] = _selectedIDs;

            var Model = new ProductPageGenerateGridViewModel();

            IMultipleResults results = new FlockDataContext().ProductPage_Generate_Select(launchID);

            Model.Merchandisers = results.GetResult<ProductPage_Generate_Select_CoreResult>().ToList();
            Model.Products = results.GetResult<ProductPage_Generate_Select_ProductResult>().ToList();
            Model.Brands = results.GetResult<Brand_SelectResult>().ToList();
            Model.ProductTypes = results.GetResult<ProductType_SelectResult>().ToList();
            Model.Launches = results.GetResult<Launch_SelectResult>().ToList();
            Model.NamingConventions = UnitOfWork.GetRepository<NamingConvention>().GetAll().OrderBy(p => p.NamingConventionID).ToList();
            
            return PartialView("generator/_gridviewpartial", Model);
        }

        public ActionResult ProductPage_Generate_LaunchPreFilter()
        {
            var Model = UnitOfWork.GetRepository<Launch>().GetAll().Where(p => p.LaunchDate > DateTime.Now.AddYears(-2)).OrderByDescending(p => p.LaunchDate).ToList();

            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        [HttpPost]
        public ActionResult FireGeneratorPreflightCheck(String merchandiserIDs, Int16? launchID)
        {
            try
            {
                bool? goodToGo = UnitOfWork.Context.ProductPage_Generate_PreflightCheck(merchandiserIDs, launchID);

                return Content(goodToGo.GetValueOrDefault() ? "Preflight check passed" : "Preflight check did not pass");
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult GenerateSelected(string IDsToGenerate, DateTime? OnDate)
        {
            try
            {
                if (new FlockDataContext().ProductPage_Generate(IDsToGenerate, OnDate))
                    return Content("OK");
                else
                {
                    Response.StatusCode = 500;
                    return Content("FAIL");
                }   
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Response.StatusCode = 500;
                return Content("FAIL");
            }
        }



        [ValidateInput(false)]
        public ActionResult CategorySettingsGridViewPartial()
        {
            var model = UnitOfWork.GetRepository<Mertex_ProductCategory>().GetAll().Where(p => p.Exclude == false);
            return PartialView("generator/_CategorySettingsGridViewPartial", model);
        }

        public ActionResult CategorySettingsGridViewBatchEditingUpdate(MVCxGridViewBatchUpdateValues<Mertex_ProductCategory, short> updateValues)
        {
            string updateErrors = null;
            MVCxGridViewBatchUpdateValues<Mertex_ProductCategory, short> validUpdates = new MVCxGridViewBatchUpdateValues<Mertex_ProductCategory, short>();
            foreach (var category in updateValues.Update)
            {
                if (updateValues.IsValid(category))
                {
                    validUpdates.Update.Add(category);
                }
                else
                {
                    updateErrors += "Row: " + category.ProductCategoryID.ToString() + " was not updated. \r\n" + updateValues.EditorErrors[category].Count.ToString() + " errors were present for this record.\r\n";
                    //log e
                }
            }
            if (validUpdates.Update.Count > 0)
            {
                foreach(var v in validUpdates.Update)
                {
                    var modelItem = UnitOfWork.GetRepository<Mertex_ProductCategory>().GetAll().Where(p => p.ProductCategoryID == v.ProductCategoryID).Single();
                    modelItem.ProductTypeID = v.ProductTypeID;
                    modelItem.NamingConventionID = v.NamingConventionID;
                    modelItem.TemplateID = v.TemplateID;
                    UnitOfWork.Save();
                }
            }
            ViewData["UpdateErrors"] = updateErrors;

            var model = UnitOfWork.GetRepository<Mertex_ProductCategory>().GetAll().Where(p => p.Exclude == false);

            return PartialView("generator/_CategorySettingsGridViewPartial", model);
        }

        public ActionResult ErrorInspectorGridViewPartial(Int16? LaunchID)
        {
            var model = UnitOfWork.Context.ProductPage_Generate_ErrorInspector_GenerateMultiple(LaunchID).GetResult<Product_ReasonNotImportedResult>().ToList();
            return PartialView("Generator/_ErrorInspectorGridViewPartial", model);
        }
    }
}