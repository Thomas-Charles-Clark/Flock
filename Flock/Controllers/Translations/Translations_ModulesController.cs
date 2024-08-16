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
        public ActionResult Modules(string targetLanguage)
        {
            var Model = new TranslateModuleViewModel();

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

            return View("Modules/TranslateModule", Model);
        }

        public ActionResult TranslateModuleTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateModuleGridView(byte? languageId, string commandType, short? moduleId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Module_Language>().GetAll().Where(p => p.ModuleID == moduleId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Description = null;
                        item.ConfirmText = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Module_Language_SelectList_ModuleResult> Modules = new List<Module_Language_SelectList_ModuleResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Module_Language_SelectList(languageId.Value);

                    Modules = Results.GetResult<Module_Language_SelectList_ModuleResult>().ToList();
                }

                TranslateModuleGridViewModel Model = new TranslateModuleGridViewModel(Modules, languageId);

                return PartialView("Modules/_TranslateModuleGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateModuleGridViewUpdate(Module_Language_SelectList_ModuleResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateModuleGridViewModel();
                    saveModel.Modules.Add(item);
                    UnitOfWork.Context.Module_Language_Save(languageId, saveModel);

                    List<Module_Language_SelectList_ModuleResult> Modules = new List<Module_Language_SelectList_ModuleResult>();
                    IMultipleResults Results = UnitOfWork.Context.Module_Language_SelectList(languageId);
                    Modules = Results.GetResult<Module_Language_SelectList_ModuleResult>().ToList();
                    var Model = new TranslateModuleGridViewModel(Modules, languageId);

                    return PartialView("Modules/_TranslateModuleGridView", Model);
                }
                else
                {
                    List<Module_Language_SelectList_ModuleResult> Modules = new List<Module_Language_SelectList_ModuleResult>();
                    IMultipleResults Results = UnitOfWork.Context.Module_Language_SelectList(languageId);
                    Modules = Results.GetResult<Module_Language_SelectList_ModuleResult>().ToList();
                    var Model = new TranslateModuleGridViewModel(Modules, languageId);

                    return PartialView("Modules/_TranslateModuleGridView", Model);
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