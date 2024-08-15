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
            var Model = new TranslateModularProductsViewModel();

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

            return View("ModuleOptions/TranslateModuleOption", Model);
        }

        public ActionResult TranslateModuleOptionTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateModuleOptionGridView(byte? languageId, string commandType, int? moduleOptionId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.ModuleOption_Language>().GetAll().Where(p => p.ModuleOptionID == moduleOptionId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<ModuleOption_Language_SelectList_ModuleOptionResult> ModuleOptions = new List<ModuleOption_Language_SelectList_ModuleOptionResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.ModuleOption_Language_SelectList(languageId.Value);

                    ModuleOptions = Results.GetResult<ModuleOption_Language_SelectList_ModuleOptionResult>().ToList();
                }

                TranslateModuleOptionGridViewModel Model = new TranslateModuleOptionGridViewModel(ModuleOptions, languageId);

                return PartialView("ModuleOptions/_TranslateModuleOptionGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateModuleOptionGridViewUpdate(ModuleOption_Language_SelectList_ModuleOptionResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateModuleOptionGridViewModel();
                    saveModel.ModuleOptions.Add(item);
                    UnitOfWork.Context.ModuleOption_Language_Save(languageId, saveModel);

                    List<ModuleOption_Language_SelectList_ModuleOptionResult> ModuleOptions = new List<ModuleOption_Language_SelectList_ModuleOptionResult>();
                    IMultipleResults Results = UnitOfWork.Context.ModuleOption_Language_SelectList(languageId);
                    ModuleOptions = Results.GetResult<ModuleOption_Language_SelectList_ModuleOptionResult>().ToList();
                    var Model = new TranslateModuleOptionGridViewModel(ModuleOptions, languageId);

                    return PartialView("ModuleOptions/_TranslateModuleOptionGridView", Model);
                }
                else
                {
                    List<ModuleOption_Language_SelectList_ModuleOptionResult> ModuleOptions = new List<ModuleOption_Language_SelectList_ModuleOptionResult>();
                    IMultipleResults Results = UnitOfWork.Context.ModuleOption_Language_SelectList(languageId);
                    ModuleOptions = Results.GetResult<ModuleOption_Language_SelectList_ModuleOptionResult>().ToList();
                    var Model = new TranslateModuleOptionGridViewModel(ModuleOptions, languageId);

                    return PartialView("ModuleOptions/_TranslateModuleOptionGridView", Model);
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