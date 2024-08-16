using DevExpress.Web.Mvc;
using Flock.Models;
using Flock.ViewModels;
using Google.Apis.Translate.v2;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public partial class TranslationsController : Controller
    {
        public ActionResult ModularProducts(string targetLanguage)
        {
            var Model = new TranslateModularProductViewModel();

            if (targetLanguage != null)
            {
                if (targetLanguage.Length > 2 || targetLanguage.Length < 2)
                {
                    ModelState.AddModelError("TargetLanguage", "Invalid language code detected");
                }
                else
                {
                    var lang = UnitOfWork.GetRepository<Language>().GetAll().Where(p => p.Code == targetLanguage).SingleOrDefault();

                    if (lang == null)
                    {
                        ModelState.AddModelError("TargetLanguage", "Invalid language code detected");
                    }
                    else
                    {
                        Model.TargetLanguageID = lang.LanguageID;
                    }
                }
            }

            return View("ModularProducts/TranslateModularProduct", Model);
        }

        public ActionResult TranslateModularProductsTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateModularProductsGridView(byte? languageId, string commandType, short? modularProductId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.ModularProduct_Language>().GetAll().Where(p => p.ModularProductID == modularProductId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<ModularProduct_Language_SelectList_ModularProductResult> ModularProducts = new List<ModularProduct_Language_SelectList_ModularProductResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.ModularProduct_Language_SelectList(languageId.Value);

                    ModularProducts = Results.GetResult<ModularProduct_Language_SelectList_ModularProductResult>().ToList();
                }

                TranslateModularProductGridViewModel Model = new TranslateModularProductGridViewModel(ModularProducts, languageId);

                return PartialView("ModularProducts/_TranslateModularProductGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateModularProductGridViewUpdate(ModularProduct_Language_SelectList_ModularProductResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateModularProductGridViewModel();
                    saveModel.ModularProducts.Add(item);
                    UnitOfWork.Context.ModularProduct_Language_Save(languageId, saveModel);

                    List<ModularProduct_Language_SelectList_ModularProductResult> ModularProducts = new List<ModularProduct_Language_SelectList_ModularProductResult>();
                    IMultipleResults Results = UnitOfWork.Context.ModularProduct_Language_SelectList(languageId);
                    ModularProducts = Results.GetResult<ModularProduct_Language_SelectList_ModularProductResult>().ToList();
                    var Model = new TranslateModularProductGridViewModel(ModularProducts, languageId);

                    return PartialView("ModularProducts/_TranslateModularProductGridView", Model);
                }
                else
                {
                    List<ModularProduct_Language_SelectList_ModularProductResult> ModularProducts = new List<ModularProduct_Language_SelectList_ModularProductResult>();
                    IMultipleResults Results = UnitOfWork.Context.ModularProduct_Language_SelectList(languageId);
                    ModularProducts = Results.GetResult<ModularProduct_Language_SelectList_ModularProductResult>().ToList();
                    var Model = new TranslateModularProductGridViewModel(ModularProducts, languageId);

                    return PartialView("ModularProducts/_TranslateModularProductGridView", Model);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }
    }
}