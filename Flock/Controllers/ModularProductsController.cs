using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;
using System.Data.Linq;

namespace Flock.Controllers
{
    public class ModularProductsController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        // GET: ModularProduct
        public ActionResult Index()
        {
            return View();
        }

        Flock.Models.FlockDataContext db = new Flock.Models.FlockDataContext();

        #region Modular Products

        #region ModularProduct GridView
        [ValidateInput(false)]
        public ActionResult ModularProductGridViewPartial()
        {
            var model = db.ModularProducts;
            return PartialView("_ModularProductGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ModularProductGridViewPartialAddNew(Flock.Models.ModularProduct item)
        {
            var model = db.ModularProducts;
            if (ModelState.IsValid)
            {
                try
                {
                    model.InsertOnSubmit(item);
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ModularProductGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ModularProductGridViewPartialUpdate(Flock.Models.ModularProduct item)
        {
            var model = db.ModularProducts;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ModularProductID == item.ModularProductID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SubmitChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ModularProductGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ModularProductGridViewPartialDelete(System.Int16? ModularProductID)
        {
            var model = db.ModularProducts;
            if (ModularProductID != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ModularProductID == ModularProductID);
                    if (item != null)
                        model.DeleteOnSubmit(item);
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_ModularProductGridViewPartial", model);
        }

        [HttpPost]
        public ActionResult ModularProduct_ModuleOptionsPartial(Int16 ModularProductID)
        {
            var model = new ModularProduct_ModuleOptionViewModel();

            model.ModularProductID = ModularProductID;

            model.AvailableOptions = UnitOfWork.GetRepository<ModuleOption>().GetAll().ToList();

            model.SelectedOptions = UnitOfWork.GetRepository<ModularProduct_ModuleOption>().GetAll().Where(p => p.ModularProductID == ModularProductID).Select(p => p.ModuleOption).ToList();
            
            return PartialView("_ModularProduct_ModuleOptionsPartial", model);
        }
        #endregion

        #region Add Module Options to Modular Product
        [HttpPost]
        public ActionResult addModulesOptionsToModularProduct(Int16 ModularProductID, Int16 ModuleID)
        {
            var thisModulesOptions = UnitOfWork.GetRepository<ModuleOption>().GetAll().Where(p => p.ModuleID == ModuleID).ToList();

            var currentModuleOptions = UnitOfWork.GetRepository<ModularProduct_ModuleOption>().GetAll().Where(p => p.ModularProductID == ModularProductID).Select(p => p.ModuleOption).ToList();

            var toAdd = thisModulesOptions.Except(currentModuleOptions).ToList().ConvertAll(p => new ModularProduct_ModuleOption() { ModuleOptionID = p.ModuleOptionID, ModularProductID = ModularProductID });

            db.ModularProduct_ModuleOptions.InsertAllOnSubmit(toAdd);

            db.SubmitChanges();

            var model = new ModularProduct_ModuleOptionViewModel();

            model.ModularProductID = ModularProductID;

            model.AvailableOptions = UnitOfWork.GetRepository<ModuleOption>().GetAll().ToList();

            model.SelectedOptions = UnitOfWork.GetRepository<ModularProduct_ModuleOption>().GetAll().Where(p => p.ModularProductID == ModularProductID).Select(p => p.ModuleOption).ToList();
            
            return PartialView("_SelectedModuleOptionsPartial", model);
        }

        [HttpPost]
        public ActionResult addModuleOptionGroupOptionsToModularProduct(Int16 ModularProductID, Int16 ModuleOptionGroupID)
        {
            var thisModulesOptions = UnitOfWork.GetRepository<ModuleOption>().GetAll().Where(p => p.ModuleOptionGroupID == ModuleOptionGroupID).ToList();

            var currentModuleOptions = UnitOfWork.GetRepository<ModularProduct_ModuleOption>().GetAll().Where(p => p.ModularProductID == ModularProductID).Select(p => p.ModuleOption).ToList();

            var toAdd = thisModulesOptions.Except(currentModuleOptions).ToList().ConvertAll(p => new ModularProduct_ModuleOption() { ModuleOptionID = p.ModuleOptionID, ModularProductID = ModularProductID });

            db.ModularProduct_ModuleOptions.InsertAllOnSubmit(toAdd);

            db.SubmitChanges();

            var model = new ModularProduct_ModuleOptionViewModel();

            model.ModularProductID = ModularProductID;

            model.AvailableOptions = UnitOfWork.GetRepository<ModuleOption>().GetAll().ToList();

            model.SelectedOptions = UnitOfWork.GetRepository<ModularProduct_ModuleOption>().GetAll().Where(p => p.ModularProductID == ModularProductID).Select(p => p.ModuleOption).ToList();

            return PartialView("_SelectedModuleOptionsPartial", model);
        }

        [HttpPost]
        public ActionResult addModuleOptionToModularProduct(Int16 ModularProductID, Int32 ModuleOptionID)
        {
            var thisModulesOptions = UnitOfWork.GetRepository<ModuleOption>().GetAll().Where(p => p.ModuleOptionID == ModuleOptionID).ToList();

            var currentModuleOptions = UnitOfWork.GetRepository<ModularProduct_ModuleOption>().GetAll().Where(p => p.ModularProductID == ModularProductID).Select(p => p.ModuleOption).ToList();

            var toAdd = thisModulesOptions.Except(currentModuleOptions).ToList().ConvertAll(p => new ModularProduct_ModuleOption() { ModuleOptionID = p.ModuleOptionID, ModularProductID = ModularProductID });

            db.ModularProduct_ModuleOptions.InsertAllOnSubmit(toAdd);

            db.SubmitChanges();

            var model = new ModularProduct_ModuleOptionViewModel();

            model.ModularProductID = ModularProductID;

            model.AvailableOptions = UnitOfWork.GetRepository<ModuleOption>().GetAll().ToList();

            model.SelectedOptions = UnitOfWork.GetRepository<ModularProduct_ModuleOption>().GetAll().Where(p => p.ModularProductID == ModularProductID).Select(p => p.ModuleOption).ToList();

            return PartialView("_SelectedModuleOptionsPartial", model);
        }
        #endregion

        #region Remove Module Options from Modular Product

        [HttpPost]
        public ActionResult removeModulesOptionsFromModularProduct(Int16 ModularProductID, Int16 ModuleID)
        {
            var thisModulesOptions = db.ModularProduct_ModuleOptions.Where(p => p.ModuleOption.ModuleID == ModuleID).ToList();

            db.ModularProduct_ModuleOptions.DeleteAllOnSubmit(thisModulesOptions);

            db.SubmitChanges();

            var model = new ModularProduct_ModuleOptionViewModel();

            model.ModularProductID = ModularProductID;

            model.AvailableOptions = UnitOfWork.GetRepository<ModuleOption>().GetAll().ToList();

            model.SelectedOptions = UnitOfWork.GetRepository<ModularProduct_ModuleOption>().GetAll().Where(p => p.ModularProductID == ModularProductID).Select(p => p.ModuleOption).ToList();

            return PartialView("_SelectedModuleOptionsPartial", model);
        }

        [HttpPost]
        public ActionResult removeModuleOptionGroupOptionsFromModularProduct(Int16 ModularProductID, Int16 ModuleOptionGroupID)
        {
            var thisModuleOptionGroupsOptions = db.ModularProduct_ModuleOptions.Where(p => p.ModuleOption.ModuleOptionGroupID == ModuleOptionGroupID);

            db.ModularProduct_ModuleOptions.DeleteAllOnSubmit(thisModuleOptionGroupsOptions);

            db.SubmitChanges();

            var model = new ModularProduct_ModuleOptionViewModel();

            model.ModularProductID = ModularProductID;

            model.AvailableOptions = UnitOfWork.GetRepository<ModuleOption>().GetAll().ToList();

            model.SelectedOptions = UnitOfWork.GetRepository<ModularProduct_ModuleOption>().GetAll().Where(p => p.ModularProductID == ModularProductID).Select(p => p.ModuleOption).ToList();

            return PartialView("_SelectedModuleOptionsPartial", model);
        }

        [HttpPost]
        public ActionResult removeModuleOptionFromModularProduct(Int16 ModularProductID, Int32 ModuleOptionID)
        {
            var thisModuleOption = db.ModularProduct_ModuleOptions.Where(p => p.ModularProductID == ModularProductID && p.ModuleOptionID == ModuleOptionID).Single();

            db.ModularProduct_ModuleOptions.DeleteOnSubmit(thisModuleOption);

            db.SubmitChanges();

            var model = new ModularProduct_ModuleOptionViewModel();

            model.ModularProductID = ModularProductID;

            model.AvailableOptions = UnitOfWork.GetRepository<ModuleOption>().GetAll().ToList();

            model.SelectedOptions = UnitOfWork.GetRepository<ModularProduct_ModuleOption>().GetAll().Where(p => p.ModularProductID == ModularProductID).Select(p => p.ModuleOption).ToList();

            return PartialView("_SelectedModuleOptionsPartial", model);
        }
        #endregion

        #endregion

        #region Modules

        public ActionResult Modules()
        {
            return View("Modules/Modules");
        }

        #endregion

        [ValidateInput(false)]
        public ActionResult ModuleGridViewPartial()
        {
            var model = db.Modules;
            return PartialView("Modules/_ModuleGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ModuleGridViewPartialAddNew(Flock.Models.Module item)
        {
            var model = db.Modules;
            if (ModelState.IsValid)
            {
                try
                {
                    model.InsertOnSubmit(item);
                    db.SubmitChanges();
                    db.Module_Language_Initialise();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("Modules/_ModuleGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ModuleGridViewPartialUpdate(Flock.Models.Module item)
        {
            var model = db.Modules;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ModuleID == item.ModuleID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SubmitChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("Modules/_ModuleGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ModuleGridViewPartialDelete(System.Int16 ModuleID)
        {
            var model = db.Modules;
            
            try
            {
                var item = model.FirstOrDefault(it => it.ModuleID == ModuleID);
                if (item != null)
                {
                    db.Module_Languages.DeleteAllOnSubmit(item.Module_Languages);
                    model.DeleteOnSubmit(item);
                }
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("Modules/_ModuleGridViewPartial", model);
        }

        [HttpPost]
        public ActionResult Module_ModuleOptionsPartial(Int16 ModuleID)
        {
            var model = new Module_ModuleOptionsViewModel();

            IMultipleResults results = db.ModuleOption_SortOrder_Select(ModuleID);
            model.ModuleID = ModuleID;
            model.ModuleOptions = results.GetResult<ModuleOptionSortDisplay>().ToList();
            model.ModuleOptionGroups = results.GetResult<ModuleOptionGroup>().ToList();

            return PartialView("Modules/_Module_ModuleOptionsPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult ModuleOptionGridViewPartial(Int16 ModuleID)
        {
            ViewData["ModuleID"] = ModuleID;
            return PartialView("Modules/_ModuleOptionGridViewPartial");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ModuleOptionGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]ModuleOptionViewModel item)
        {
            ViewData["ModuleID"] = item.ModuleID;
            var model = db.ModuleOptions;
            if (ModelState.IsValid)
            {
                try
                {
                    db.ModuleOptions.InsertOnSubmit(new ModuleOption() { 
                        ModuleID = item.ModuleID,
                        ModuleOptionGroupID = item.ModuleOptionGroupID,
                        Name = item.Name,
                        ProductID = item.ProductID,
                        ImageID = item.ImageID
                    });
                    db.SubmitChanges();

                    db.ModuleOption_Language_Initialise();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("Modules/_ModuleOptionGridViewPartial", item);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ModuleOptionGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))]ModuleOptionViewModel item)
        {
            ViewData["ModuleID"] = item.ModuleID;
            var model = db.ModuleOptions;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ModuleOptionID == item.ModuleOptionID);
                    if (modelItem != null)
                    {
                        modelItem.ModuleID = item.ModuleID;
                        modelItem.ModuleOptionGroupID = item.ModuleOptionGroupID;
                        modelItem.Name = item.Name;
                        modelItem.ProductID = item.ProductID;
                        modelItem.ImageID = item.ImageID;

                        db.SubmitChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("Modules/_ModuleOptionGridViewPartial", item);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ModuleOptionGridViewPartialDelete(System.Int16 ModuleID, System.Int32 ModuleOptionID)
        {
            ViewData["ModuleID"] = ModuleID;
            var model = db.ModuleOptions;
            if (ModuleOptionID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ModuleOptionID == ModuleOptionID);
                    if (item != null)
                    {
                        db.ModuleOption_Languages.DeleteAllOnSubmit(item.ModuleOption_Languages);
                        db.ModularProduct_ModuleOptions.DeleteAllOnSubmit(item.ModularProduct_ModuleOptions);
                        model.DeleteOnSubmit(item);
                    }
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("Modules/_ModuleOptionGridViewPartial");
        }

        public ActionResult ModuleOptionEditFormPartial(Int32? ModuleOptionID)
        {
            ModuleOptionViewModel Model = null;

            if(ModuleOptionID.HasValue)
            {
                Model = new ModuleOptionViewModel();

                var item = db.ModuleOptions.Where(p => p.ModuleOptionID == ModuleOptionID).Single();

                Model.ModuleOptionID = ModuleOptionID;
                Model.ModuleID = item.ModuleID;
                Model.ModuleOptionGroupID = item.ModuleOptionGroupID;
                Model.Name = item.Name;
                Model.ProductID = item.ProductID;
                Model.ImageID = item.ImageID;
            }

            return PartialView("Modules/_ModuleOptionEditFormPartial", Model);
        }

        public ActionResult ModuleOptionGroupComboBoxPartial(Int16? ModuleOptionGroupID)
        {
            return PartialView("Modules/_ModuleOptionGroupComboBoxPartial", ModuleOptionGroupID);
        }

        public ActionResult ModuleOptionProductIDComboBoxPartial(Int32? ProductID)
        {
            return PartialView("Modules/_ModuleOptionProductIDComboBoxPartial", ProductID);
        }

        [ValidateInput(false)]
        public ActionResult ModuleOptionGroupGridViewPartial()
        {
            var model = db.ModuleOptionGroups;
            return PartialView("Modules/_ModuleOptionGroupGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ModuleOptionGroupGridViewPartialAddNew(Flock.Models.ModuleOptionGroup item)
        {
            var model = db.ModuleOptionGroups;
            if (ModelState.IsValid)
            {
                try
                {
                    model.InsertOnSubmit(item);
                    db.SubmitChanges();
                    db.ModuleOptionGroup_Language_Initialise();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("Modules/_ModuleOptionGroupGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ModuleOptionGroupGridViewPartialUpdate(Flock.Models.ModuleOptionGroup item)
        {
            var model = db.ModuleOptionGroups;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ModuleOptionGroupID == item.ModuleOptionGroupID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SubmitChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("Modules/_ModuleOptionGroupGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ModuleOptionGroupGridViewPartialDelete(System.Int16 ModuleOptionGroupID)
        {
            var model = db.ModuleOptionGroups;

            try
            {
                var item = model.FirstOrDefault(it => it.ModuleOptionGroupID == ModuleOptionGroupID);
                if (item != null)
                {
                    db.ModuleOptionGroup_Languages.DeleteAllOnSubmit(item.ModuleOptionGroup_Languages);
                    model.DeleteOnSubmit(item);

                }
                    
                var affected = db.ModuleOptions.Where(p => p.ModuleOptionGroupID == ModuleOptionGroupID);
                foreach (var a in affected)
                    a.ModuleOptionGroupID = null;

                db.SubmitChanges();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("Modules/_ModuleOptionGroupGridViewPartial", model);
        }

        public ActionResult ModuleOptionSorterPartial(Int16 ModuleID)
        {
            var model = new Module_ModuleOptionsViewModel();

            IMultipleResults results = db.ModuleOption_SortOrder_Select(ModuleID);
            model.ModuleID = ModuleID;
            model.ModuleOptions = results.GetResult<ModuleOptionSortDisplay>().ToList();
            model.ModuleOptionGroups = results.GetResult<ModuleOptionGroup>().ToList();


            return PartialView("Modules/_ModuleOptionSorterPartial", model);
        }

        [HttpPost]
        public JsonResult ModuleOptionSort(List<ModuleOptionSortDatum> items)
        {
            var moduleOptionIDs = items.Select(q => q.ModuleOptionID).ToList();
            var modelItems = db.ModuleOptions.Where(p => moduleOptionIDs.Contains(p.ModuleOptionID));

            foreach(var mi in modelItems)
            {
                mi.SortOrder = items.Where(p => p.ModuleOptionID == mi.ModuleOptionID).Select(p => p.SortOrder).Single();
            }

            db.SubmitChanges();

            return Json(new { result = "done" });
        }

        [HttpPost]
        public JsonResult ModuleOptionGroupSort(List<ModuleOptionGroupSortDatum> items)
        {
            var moduleOptionGroupIDs = items.Select(q => q.ModuleOptionGroupID).ToList();
            var modelItems = db.ModuleOptionGroups.Where(p => moduleOptionGroupIDs.Contains(p.ModuleOptionGroupID));

            foreach (var mi in modelItems)
            {
                mi.SortOrder = items.Where(p => p.ModuleOptionGroupID == mi.ModuleOptionGroupID).Select(p => p.SortOrder).Single();
            }

            db.SubmitChanges();

            return Json(new { result = "done" });
        }
    }

    public class ModuleOptionSortDatum
    {
        public Int32 ModuleOptionID { get; set; }

        public Int16 SortOrder { get; set; }
    }

    public class ModuleOptionGroupSortDatum
    {
        public Int16 ModuleOptionGroupID { get; set; }

        public Byte SortOrder { get; set; }
    }
}