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
        public ActionResult ModuleOptionGroups(string targetLanguage)
        {
            var Model = new TranslateModuleOptionGroupViewModel();

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

            return View("ModuleOptionGroups/TranslateModuleOptionGroup", Model);
        }

        public ActionResult TranslateModuleOptionGroupTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateModuleOptionGroupGridView(byte? languageId, string commandType, short? moduleOptionGroupId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.ModuleOptionGroup_Language>().GetAll().Where(p => p.ModuleOptionGroupID == moduleOptionGroupId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Description = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult> ModuleOptionGroups = new List<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.ModuleOptionGroup_Language_SelectList(languageId.Value);

                    ModuleOptionGroups = Results.GetResult<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult>().ToList();
                }

                TranslateModuleOptionGroupGridViewModel Model = new TranslateModuleOptionGroupGridViewModel(ModuleOptionGroups, languageId);

                return PartialView("ModuleOptionGroups/_TranslateModuleOptionGroupGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateModuleOptionGroupGridViewUpdate(ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateModuleOptionGroupGridViewModel();
                    saveModel.ModuleOptionGroups.Add(item);
                    UnitOfWork.Context.ModuleOptionGroup_Language_Save(languageId, saveModel);

                    List<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult> ModuleOptionGroups = new List<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult>();
                    IMultipleResults Results = UnitOfWork.Context.ModuleOptionGroup_Language_SelectList(languageId);
                    ModuleOptionGroups = Results.GetResult<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult>().ToList();
                    var Model = new TranslateModuleOptionGroupGridViewModel(ModuleOptionGroups, languageId);

                    return PartialView("ModuleOptionGroups/_TranslateModuleOptionGroupGridView", Model);
                }
                else
                {
                    List<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult> ModuleOptionGroups = new List<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult>();
                    IMultipleResults Results = UnitOfWork.Context.ModuleOptionGroup_Language_SelectList(languageId);
                    ModuleOptionGroups = Results.GetResult<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult>().ToList();
                    var Model = new TranslateModuleOptionGroupGridViewModel(ModuleOptionGroups, languageId);

                    return PartialView("ModuleOptionGroups/_TranslateModuleOptionGroupGridView", Model);
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