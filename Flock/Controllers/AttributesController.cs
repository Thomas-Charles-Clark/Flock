using Flock.Classes.Auth;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public class AttributesController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        #region Manage

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return RedirectToAction("manage");
        }

        //[CustomAuthorize(Roles = "Admin")]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Manage()
        {
            return View(new AttributeManageViewModel());
        }

        /// <summary>
        /// MASTER
        /// </summary>
        /// <returns></returns>
        public ActionResult AttributeGroupGridViewPartial()
        {
            return PartialView();
        }

        public ActionResult AttributeGroupGridViewAddNew(AttributeGroup item)
        {
            if (item.Name == "" || item.Name == null)
                ModelState.AddModelError("Name", "Name must not be blank.");
                
            if (item.Heading == "" || item.Heading == null)
                ModelState.AddModelError("Heading", "Heading must not be blank.");

            item.AttributeGroupID = (byte)(UnitOfWork.GetRepository<AttributeGroup>().GetAll().Select(p => p.AttributeGroupID).Max() + (byte)1);
            if (item.AttributeGroupID > 255)
                ModelState.AddModelError("Name", "Sorry, Maximum Number of Attribute Groups Created!");

            if(ModelState.IsValid)
            {
                item.SortOrder = (byte)(UnitOfWork.GetRepository<AttributeGroup>().GetAll().Select(p => p.SortOrder).Max() + (byte)1);
                UnitOfWork.GetRepository<AttributeGroup>().Add(item);
                UnitOfWork.Save();
                UnitOfWork.Context.AttributeGroup_Language_Initialise();
            }
            return PartialView("AttributeGroupGridViewPartial");
        }

        public ActionResult AttributeGroupGridViewUpdate(AttributeGroup item)
        {
            if (item.Name == "" || item.Name == null)
                ModelState.AddModelError("Name", "Name must not be blank.");

            if (item.Heading == "" || item.Heading == null)
                ModelState.AddModelError("Heading", "Heading must not be blank.");

            if (ModelState.IsValid)
            {
                var ag = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => p.AttributeGroupID == item.AttributeGroupID).Single();
                ag.Name = item.Name;
                ag.Heading = item.Heading;
                ag.MultipleSelect = item.MultipleSelect;
                ag.UseIcons = item.UseIcons;
                UnitOfWork.Save();
            }
            return PartialView("AttributeGroupGridViewPartial");
        }

        public ActionResult AttributeGridViewPartial(byte AttributeGroupID)
        {
            return PartialView(AttributeGroupID);
        }

        public ActionResult AttributeGroupGridViewCustomCommand(string CommandType, byte AttributeGroupID)
        {
            switch(CommandType)
            {
                case "MoveUp":
                    AttributeGroup moveUp = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID).Single();
                    if(moveUp.SortOrder > 1)
                    {
                        AttributeGroup switchGroup = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => p.SortOrder == (byte)(moveUp.SortOrder - 1)).Single();
                        moveUp.SortOrder = switchGroup.SortOrder;
                        switchGroup.SortOrder = (byte)(moveUp.SortOrder + 1);
                        UnitOfWork.Save();
                    }
                    break;

                case "MoveDown":
                    AttributeGroup moveDown = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID).Single();
                    if (moveDown.SortOrder < UnitOfWork.GetRepository<AttributeGroup>().GetAll().Select(p => p.SortOrder).Max())
                    {
                        AttributeGroup switchGroup = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => p.SortOrder == (byte)(moveDown.SortOrder + 1)).Single();
                        moveDown.SortOrder = switchGroup.SortOrder;
                        switchGroup.SortOrder = (byte)(moveDown.SortOrder - 1);
                        UnitOfWork.Save();
                    }
                    break;
            }

            return PartialView("AttributeGroupGridViewPartial");
        }


        /// <summary>
        /// DETAIL
        /// </summary>
        /// <returns></returns>

        public ActionResult AttributeGridViewAddNew(Flock.Models.Attribute item)
        {
            if (item.Name == "" || item.Name == null)
                ModelState.AddModelError("Name", "Name must not be blank.");

            if (ModelState.IsValid)
            {
                if (UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == item.AttributeGroupID).Count() >= 1)
                    item.SortOrder = (byte)(UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == item.AttributeGroupID).Select(p => p.SortOrder).Max() + (byte)1);
                else
                    item.SortOrder = 1;
                UnitOfWork.GetRepository<Flock.Models.Attribute>().Add(item);
                UnitOfWork.Save();
                UnitOfWork.Context.Attribute_Language_Initialise();
            }
            return PartialView("AttributeGridViewPartial", item.AttributeGroupID);
        }

        public ActionResult AttributeGridViewUpdate(Flock.Models.Attribute item)
        {
            if (item.Name == "" || item.Name == null)
                ModelState.AddModelError("Name", "Name must not be blank.");

            if (ModelState.IsValid)
            {
                var ag = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == item.AttributeGroupID && p.AttributeID == item.AttributeID).Single();
                ag.Name = item.Name;
                UnitOfWork.Save();
            }
            return PartialView("AttributeGridViewPartial", item.AttributeGroupID);
        }

        public ActionResult AttributeGridViewCustomCommand(string CommandType, byte AttributeGroupID, short AttributeID)
        {
            switch (CommandType)
            {
                case "MoveUp":
                    Flock.Models.Attribute moveUp = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID && p.AttributeID == AttributeID).Single();
                    if (moveUp.SortOrder > 1)
                    {
                        Flock.Models.Attribute switchAttr = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID && p.SortOrder == (byte)(moveUp.SortOrder - 1)).Single();
                        moveUp.SortOrder = switchAttr.SortOrder;
                        switchAttr.SortOrder = (byte)(moveUp.SortOrder + 1);
                        UnitOfWork.Save();
                    }
                    break;

                case "MoveDown":
                    Flock.Models.Attribute moveDown = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID && p.AttributeID == AttributeID).Single();
                    if (moveDown.SortOrder < UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID).Select(p => p.SortOrder).Max())
                    {
                        Flock.Models.Attribute switchAttr = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID && p.SortOrder == (byte)(moveDown.SortOrder + 1)).Single();
                        moveDown.SortOrder = switchAttr.SortOrder;
                        switchAttr.SortOrder = (byte)(moveDown.SortOrder - 1);
                        UnitOfWork.Save();
                    }
                    break;
            }

            return PartialView("AttributeGridViewPartial", AttributeGroupID);
        }

        #endregion


        #region Map

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Map()
        {
            return View();
        }

        public ActionResult MapAttributesProductTypeDropDown()
        {
            var Model = UnitOfWork.GetRepository<ProductType>().GetAll().ToList();
            return PartialView("~/Views/Shared/SearchFilters/ProductTypeDropDownPartial.cshtml", Model);
        }

        [HttpPost]
        public ActionResult ProductTypeAttributeMappingPartial(byte ProductTypeID)
        {
            var Model = new ProductTypeAttributeGroupViewModel(); 
            Model.AttributeGroups = UnitOfWork.GetRepository<AttributeGroup_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).OrderBy(p => p.SortOrder).ToList();
            Model.Attributes = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).ToList();
            Model.AvailableGroups = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => !Model.AttributeGroups.Select(x => x.AttributeGroupID).Contains(p.AttributeGroupID)).ToList();
            return PartialView(Model);
        }

        [HttpPost]
        public ActionResult SaveAttributeGroupProductTypeFlags(byte AttributeGroupID, byte ProductTypeID, bool Filter, bool Info, bool Mandatory, bool? AutoSync, byte? SortOrder)
        {
            try
            { 
                AttributeGroup_ProductType agpt = UnitOfWork.GetRepository<AttributeGroup_ProductType>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID && p.ProductTypeID == ProductTypeID).Single();
                agpt.Filter = Filter;
                agpt.Info = Info;
                agpt.Mandatory = Mandatory;
                agpt.AutoSync = AutoSync;
                agpt.SortOrder = SortOrder;
                UnitOfWork.Save();
                return Content("");
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content("");
            }
        }

        [HttpPost]
        public ActionResult UpdateProductTypeAttributeGroupAttributes(byte ProductTypeID, byte AttributeGroupID, string AttributeIDs)
        {
            
            List<short> Current = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).Select(p => p.AttributeID).ToList();
            List<short> InputAttributes = new List<short>();
            if(AttributeIDs != "")
                InputAttributes = AttributeIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToList();

            List<short> Removals = Current.Except(InputAttributes).ToList();

            foreach(var i in Removals)
            {
                Attribute_ProductType Removal = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID && p.AttributeID == i).Single();
                UnitOfWork.GetRepository<Attribute_ProductType>().Delete(Removal);
            }
            UnitOfWork.Save();

            foreach (var i in InputAttributes)
            {
                Attribute_ProductType apt = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID && p.AttributeID == i).SingleOrDefault();

                if (apt == null)
                {
                    apt = new Attribute_ProductType() { ProductTypeID = ProductTypeID, AttributeID = i };
                    UnitOfWork.GetRepository<Attribute_ProductType>().Add(apt);
                }
            }
            UnitOfWork.Save();

            var Model = new ProductTypeAttributeGroupViewModel();
            Model.AttributeGroups = UnitOfWork.GetRepository<AttributeGroup_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID && p.AttributeGroupID == AttributeGroupID).OrderBy(p => p.SortOrder).ToList();
            Model.Attributes = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).ToList();
            Model.AvailableGroups = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => !Model.AttributeGroups.Select(x => x.AttributeGroupID).Contains(p.AttributeGroupID)).ToList();
            return PartialView("AttributeGroupMapPartial", Model);
        }

        [HttpPost]
        public ActionResult RemoveAttributeGroupFromProductType(byte AttributeGroupID, byte ProductTypeID)
        {
            try { 
                AttributeGroup_ProductType AGPT = UnitOfWork.GetRepository<AttributeGroup_ProductType>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID && p.ProductTypeID == ProductTypeID).Single();
                UnitOfWork.GetRepository<AttributeGroup_ProductType>().Delete(AGPT);

                List<short> Attributes = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID).Select(p => p.AttributeID).ToList();

                List<Attribute_ProductType> AttributePTs = UnitOfWork.GetRepository<Flock.Models.Attribute_ProductType>().GetAll().Where(p => Attributes.Contains(p.AttributeID) && p.ProductTypeID == ProductTypeID).ToList();

                foreach (var s in AttributePTs)
                {
                    UnitOfWork.GetRepository<Attribute_ProductType>().Delete(s);
                }
                UnitOfWork.Save();

                var Model = new ProductTypeAttributeGroupViewModel();
                Model.AttributeGroups = UnitOfWork.GetRepository<AttributeGroup_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).OrderBy(p => p.SortOrder).ToList();
                Model.Attributes = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).ToList();
                Model.AvailableGroups = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => !Model.AttributeGroups.Select(x => x.AttributeGroupID).Contains(p.AttributeGroupID)).ToList();
                return PartialView("ProductTypeAttributeMappingPartial", Model);

            }
            catch(Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult AddAttributeGroupToProductType(byte AttributeGroupID, byte ProductTypeID)
        {
            try
            {
                AttributeGroup_ProductType AGPT = new AttributeGroup_ProductType() { AttributeGroupID = AttributeGroupID, ProductTypeID = ProductTypeID };
                UnitOfWork.GetRepository<AttributeGroup_ProductType>().Add(AGPT);
                UnitOfWork.Save();

                var Model = new ProductTypeAttributeGroupViewModel();
                Model.AttributeGroups = UnitOfWork.GetRepository<AttributeGroup_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).OrderBy(p => p.SortOrder).ToList();
                Model.Attributes = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).ToList();
                Model.AvailableGroups = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => !Model.AttributeGroups.Select(x => x.AttributeGroupID).Contains(p.AttributeGroupID)).ToList();
                return PartialView("ProductTypeAttributeMappingPartial", Model);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult AddAllAttributesToGroup(byte AttributeGroupID, byte ProductTypeID)
        {
            List<short> Attributes = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID).Select(p => p.AttributeID).ToList();

            foreach(short s in Attributes)
            {
                var t = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.AttributeID == s && p.ProductTypeID == ProductTypeID).SingleOrDefault();
                if(t == null)
                {
                    t = new Attribute_ProductType() { ProductTypeID = ProductTypeID, AttributeID = s };
                    UnitOfWork.GetRepository<Attribute_ProductType>().Add(t);
                }
            }
            UnitOfWork.Save();

            var Model = new ProductTypeAttributeGroupViewModel();
            Model.AttributeGroups = UnitOfWork.GetRepository<AttributeGroup_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID && p.AttributeGroupID == AttributeGroupID).OrderBy(p => p.SortOrder).ToList();
            Model.Attributes = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).ToList();
            Model.AvailableGroups = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => !Model.AttributeGroups.Select(x => x.AttributeGroupID).Contains(p.AttributeGroupID)).ToList();
            return PartialView("AttributeGroupMapPartial", Model);
        }

        [HttpPost]
        public ActionResult RemoveAllAttributesFromGroup(byte AttributeGroupID, byte ProductTypeID)
        {
            List<short> Attributes = UnitOfWork.GetRepository<Flock.Models.Attribute>().GetAll().Where(p => p.AttributeGroupID == AttributeGroupID).Select(p => p.AttributeID).ToList();

            List<Attribute_ProductType> AttributePTs = UnitOfWork.GetRepository<Flock.Models.Attribute_ProductType>().GetAll().Where(p => Attributes.Contains(p.AttributeID) && p.ProductTypeID == ProductTypeID).ToList();

            foreach (var s in AttributePTs)
            {
                UnitOfWork.GetRepository<Attribute_ProductType>().Delete(s);
            }
            UnitOfWork.Save();

            var Model = new ProductTypeAttributeGroupViewModel();
            Model.AttributeGroups = UnitOfWork.GetRepository<AttributeGroup_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID && p.AttributeGroupID == AttributeGroupID).OrderBy(p => p.SortOrder).ToList();
            Model.Attributes = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).ToList();
            Model.AvailableGroups = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => !Model.AttributeGroups.Select(x => x.AttributeGroupID).Contains(p.AttributeGroupID)).ToList();
            return PartialView("AttributeGroupMapPartial", Model);
        }

        [HttpPost]
        public ActionResult RemoveAttributeFromProductType(short AttributeID, byte AttributeGroupID, byte ProductTypeID)
        {
            Attribute_ProductType APT = UnitOfWork.GetRepository<Flock.Models.Attribute_ProductType>().GetAll().Where(p => p.AttributeID == AttributeID && p.ProductTypeID == ProductTypeID).Single();
            UnitOfWork.GetRepository<Attribute_ProductType>().Delete(APT);
            UnitOfWork.Save();
            
            var Model = new ProductTypeAttributeGroupViewModel();
            Model.AttributeGroups = UnitOfWork.GetRepository<AttributeGroup_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID && p.AttributeGroupID == AttributeGroupID).OrderBy(p => p.SortOrder).ToList();
            Model.Attributes = UnitOfWork.GetRepository<Attribute_ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).ToList();
            Model.AvailableGroups = UnitOfWork.GetRepository<AttributeGroup>().GetAll().Where(p => !Model.AttributeGroups.Select(x => x.AttributeGroupID).Contains(p.AttributeGroupID)).ToList();
            return PartialView("AttributeGroupMapPartial", Model);
        }

        #endregion
        #region Product Type
        [Authorize(Roles = "Admin")]
        public ActionResult ProductTypes()
        {
            return View();
        }

        public ActionResult ProductTypeGridViewPartial()
        {
            return PartialView();
        }

        public ActionResult ProductTypeGridViewAddNew(ProductType item)
        {
            if (item.Name == "" || item.Name == null)
                ModelState.AddModelError("Name", "Name must not be blank.");

            item.ProductTypeID = (byte)(UnitOfWork.GetRepository<ProductType>().GetAll().Select(p => p.ProductTypeID).Max() + (byte)1);
            if (item.ProductTypeID > 255)
                ModelState.AddModelError("Name", "Sorry, Maximum Number of Product Types Created!");

            if (ModelState.IsValid)
            {
                item.SortOrder = (byte)(UnitOfWork.GetRepository<ProductType>().GetAll().Select(p => p.SortOrder).Max() + (byte)1);
                UnitOfWork.GetRepository<ProductType>().Add(item);
                UnitOfWork.Save();
                UnitOfWork.Context.ProductType_Language_Initialise();
            }
            return PartialView("ProductTypeGridViewPartial");
        }

        public ActionResult ProductTypeGridViewUpdate(ProductType item)
        {
            if (item.Name == "" || item.Name == null)
                ModelState.AddModelError("Name", "Name must not be blank.");

            if (ModelState.IsValid)
            {
                var ag = UnitOfWork.GetRepository<ProductType>().GetAll().Where(p => p.ProductTypeID == item.ProductTypeID).Single();
                ag.Name = item.Name;
                ag.AlternateColourways = item.AlternateColourways;
                ag.GiftWrappable = item.GiftWrappable;
                ag.StockLocationID = item.StockLocationID;
                ag.AllowReturns = item.AllowReturns;
                UnitOfWork.Save();
            }
            return PartialView("ProductTypeGridViewPartial");
        }

        public ActionResult ProductTypeGridViewCustomCommand(string CommandType, byte ProductTypeID)
        {
            switch (CommandType)
            {
                case "MoveUp":
                    ProductType moveUp = UnitOfWork.GetRepository<ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).Single();
                    if (moveUp.SortOrder > 1)
                    {
                        ProductType switchGroup = UnitOfWork.GetRepository<ProductType>().GetAll().Where(p => p.SortOrder == (byte)(moveUp.SortOrder - 1)).Single();
                        moveUp.SortOrder = switchGroup.SortOrder;
                        switchGroup.SortOrder = (byte)(moveUp.SortOrder + 1);
                        UnitOfWork.Save();
                    }
                    break;

                case "MoveDown":
                    ProductType moveDown = UnitOfWork.GetRepository<ProductType>().GetAll().Where(p => p.ProductTypeID == ProductTypeID).Single();
                    if (moveDown.SortOrder < UnitOfWork.GetRepository<ProductType>().GetAll().Select(p => p.SortOrder).Max())
                    {
                        ProductType switchGroup = UnitOfWork.GetRepository<ProductType>().GetAll().Where(p => p.SortOrder == (byte)(moveDown.SortOrder + 1)).Single();
                        moveDown.SortOrder = switchGroup.SortOrder;
                        switchGroup.SortOrder = (byte)(moveDown.SortOrder - 1);
                        UnitOfWork.Save();
                    }
                    break;
            }

            return PartialView("ProductTypeGridViewPartial");
        }

        #endregion
    }
}