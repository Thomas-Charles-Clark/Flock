using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.ViewModels;
using System.Data.Linq;
using System.Web.Script.Serialization;

namespace Flock.Controllers
{
    public partial class ProductPagesController : Controller
    {
        //
        // GET: /ProductPages/Classifier/
        public ActionResult Classifier()
        {
            return View("classifier/classifier");
        }

        public ActionResult LoadClassifierHeader()
        {
            var Model = new ProductPageClassifierHeaderViewModel()
            {
                SelectedProductTypeID = null,
                SelectedAttributeGroupID = null,
                SelectedAttributeID = null,
                SelectedToggleState = 1
            };

            return PartialView("Classifier/ClassifierHeader", Model);
        }


        public ActionResult ClassifierHeader(byte? ProductTypeID, byte? AttributeGroupID, short? AttributeID, byte? Toggle)
        {
            var Model = new ProductPageClassifierHeaderViewModel() {
                SelectedProductTypeID = ProductTypeID,
                SelectedAttributeGroupID = AttributeGroupID,
                SelectedAttributeID = AttributeID,
                SelectedToggleState = Toggle
            };

            return PartialView("Classifier/ClassifierHeader", Model);
        }

        public ActionResult ClassifierProductTypeDropDown(byte? SelectedProductTypeID)
        {
            var Model = UnitOfWork.GetRepository<ProductType>().GetAll().ToList();
            ViewBag.SelectedProductTypeID = SelectedProductTypeID;
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        public ActionResult ClassifierAttributeGroupDropDownPartial(byte? ProductTypeID, byte? SelectedAttributeGroupID)
        {
            var Model = UnitOfWork.GetRepository<AttributeGroup_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).ToList();
            ViewBag.SelectedAttributeGroupID = SelectedAttributeGroupID;
            return PartialView("Classifier/AttributeGroupDropDownPartial", Model);
        }

        public ActionResult ClassifierToggleDropDownPartial(byte? ToggleState)
        {
            ViewBag.SelectedToggleState = ToggleState;
            return PartialView("Classifier/ToggleDropDownPartial", ToggleState);
        }

        public ActionResult ClassifierAttributeDropDownPartial(byte? ProductTypeID, byte? AttributeGroupID, short? SelectedAttributeID, byte? SelectedToggleState)
        {
            var Linker = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).Select(p => p.AttributeID).ToList();
            var Model = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID && Linker.Contains(p.AttributeID)).ToList();
            ViewBag.SelectedAttributeID = SelectedAttributeID;
            ViewBag.IsAttributeUsable = SelectedToggleState.Value != 2 ? true : false;
            return PartialView("Classifier/AttributeDropDownPartial", Model);
        }
        
        [HttpPost]
        public ActionResult ClassificationResultsPartial(byte ProductTypeID, byte AttributeGroupID, short? AttributeID, byte? Toggle, string SearchString)
        {
            var Model = new ProductPageClassifierViewModel()
            {
                AttributeGroupID = AttributeGroupID
            };

            IMultipleResults res = null;

            if (SearchString != null)
            {
                Model.IsFullConfiguration = true;

                Model.IsAttributeGroupMultiSelect = UnitOfWork.GetRepository<Flock.Models.AttributeGroup>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID).Select(p => p.MultipleSelect).Single();

                var Linker = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).Select(p => p.AttributeID).ToList();

                Model.AllAttributes = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID && Linker.Contains(p.AttributeID)).ToList();

                res = UnitOfWork.Context.ProductPage_Attribute_Generate(ProductTypeID, AttributeGroupID, null, null, SearchString);
            }
            else
            {
                if (Toggle.HasValue && AttributeID.HasValue)
                {
                    if (Toggle == 1)
                    {
                        Model.IsFullConfiguration = false;
                        Model.ToggleButtonText = "IS NOT " + UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeID == AttributeID).Select(p => p.Name).Single();
                    }
                    else if (Toggle == 0)
                    {
                        Model.IsFullConfiguration = false;
                        Model.ToggleButtonText = "IS " + UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeID == AttributeID).Select(p => p.Name).Single();
                    }

                    res = UnitOfWork.Context.ProductPage_Attribute_Generate(ProductTypeID, AttributeGroupID, AttributeID.Value, Toggle.Value, null);
                }
                else if (Toggle.Value == 2)
                {
                    Model.IsFullConfiguration = true;
                    Model.IsAttributeGroupMultiSelect = UnitOfWork.GetRepository<Flock.Models.AttributeGroup>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID).Select(p => p.MultipleSelect).Single();

                    var Linker = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).Select(p => p.AttributeID).ToList();

                    Model.AllAttributes = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID && Linker.Contains(p.AttributeID)).ToList();
                    res = UnitOfWork.Context.ProductPage_Attribute_Generate(ProductTypeID, AttributeGroupID);
                }
                else
                {
                    Model.IsFullConfiguration = false;
                    res = UnitOfWork.Context.ProductPage_Attribute_Generate(ProductTypeID, AttributeGroupID);
                }
            }
            //Model.Attributes = res.GetResult<ProductPage_Attribute_Generate_AttributeResult>().ToList();
            Model.ProductPages = res.GetResult<ProductPage_Attribute_Generate_ProductPageResult>().OrderByDescending(p => p.ImageID).ToList();
            Model.Mapping = res.GetResult<ProductPage_Attribute_Generate_AttributeMapResult>().ToList();

            return PartialView("classifier/resultpartial", Model);
        }

        [HttpPost]
        public ActionResult ToggleProductPageAttribute(int ProductPageID, short AttributeID)
        {
            try
            {
                UnitOfWork.Context.ProductPage_Attribute_Toggle(ProductPageID, AttributeID);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
            return Content("");
        }

        [HttpPost]
        public ActionResult SaveProductPageAttributes(int ProductPageID, byte AttributeGroupID, string AttributeIDs)
        {
            try 
            {
                UnitOfWork.Context.ProductPage_Attribute_Save(ProductPageID, AttributeGroupID, AttributeIDs);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }

            return Content("");
        }

        [HttpPost]
        public ActionResult SaveMultipleProductPageAttributes(byte AttributeGroupID, string Data)
        {
            try 
            {
                List<ProductPageAttributesSave> toSave = new JavaScriptSerializer().Deserialize<List<ProductPageAttributesSave>>(Data).ToList();
                foreach(var i in toSave)
                {
                    UnitOfWork.Context.ProductPage_Attribute_Save(i.ProductPageID, AttributeGroupID, String.Join(",", i.Attributes));
                }

                return Content("");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public struct ProductPageAttributesSave
        {
            public int ProductPageID { get; set; }
            public List<short> Attributes { get; set; }
        }
	}
}