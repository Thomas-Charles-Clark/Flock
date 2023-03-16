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
        public ActionResult TechInfo_Composition(string targetLanguage)
        {
            var Model = new TranslateTechInfo_CompositionViewModel();

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
            return View("TechInfo_Composition/TranslateTechInfo_Composition", Model);
        }

        public ActionResult TranslateCompositionTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateCompositionGridView(byte? languageId, string commandType, short? compositionId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechInfo_Ref_Composition_Language>().GetAll().Where(p => p.CompositionID == compositionId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult> TechInfo_Compositions = new List<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Composition_Language_SelectList(languageId.Value);

                    TechInfo_Compositions = Results.GetResult<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult>().ToList();
                }

                TranslateTechInfo_CompositionGridViewModel Model = new TranslateTechInfo_CompositionGridViewModel(TechInfo_Compositions, languageId);

                return PartialView("TechInfo_Composition/_TranslateTechInfo_CompositionGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateCompositionGridViewUpdate(TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechInfo_CompositionGridViewModel saveModel = new TranslateTechInfo_CompositionGridViewModel();
                    saveModel.TechInfo_Compositions.Add(item);
                    UnitOfWork.Context.TechInfo_Composition_Language_Save(languageId, saveModel);

                    List<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult> TechInfo_Compositions = new List<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Composition_Language_SelectList(languageId);

                    TechInfo_Compositions = Results.GetResult<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult>().ToList();

                    TranslateTechInfo_CompositionGridViewModel Model = new TranslateTechInfo_CompositionGridViewModel(TechInfo_Compositions, languageId);

                    return PartialView("TechInfo_Composition/_TranslateTechInfo_CompositionGridView", Model);
                }
                else
                {
                    List<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult> TechInfo_Compositions = new List<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Composition_Language_SelectList(languageId);

                    TechInfo_Compositions = Results.GetResult<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult>().ToList();

                    TranslateTechInfo_CompositionGridViewModel Model = new TranslateTechInfo_CompositionGridViewModel(TechInfo_Compositions, languageId);

                    return PartialView("TechInfo_Composition/_TranslateTechInfo_CompositionGridView", Model);
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