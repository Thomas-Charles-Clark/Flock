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
        public ActionResult Patterns(string targetLanguage)
        {
            var Model = new TranslatePatternViewModel();

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
            return View("Patterns/TranslatePattern", Model);
        }

        public ActionResult TranslatePatternTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslatePatternGridView(byte? languageId, string commandType, byte? patternId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Pattern_Language>().GetAll().Where(p => p.PatternID == patternId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Pattern_Language_SelectList_PatternResult> Patterns = new List<Pattern_Language_SelectList_PatternResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Pattern_Language_SelectList(languageId.Value);

                    Patterns = Results.GetResult<Pattern_Language_SelectList_PatternResult>().ToList();
                }

                TranslatePatternGridViewModel Model = new TranslatePatternGridViewModel(Patterns, languageId);

                return PartialView("Patterns/_TranslatePatternGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslatePatternGridViewUpdate(Pattern_Language_SelectList_PatternResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslatePatternGridViewModel saveModel = new TranslatePatternGridViewModel();
                    saveModel.Patterns.Add(item);
                    UnitOfWork.Context.Pattern_Language_Save(languageId, saveModel);

                    List<Pattern_Language_SelectList_PatternResult> Patterns = new List<Pattern_Language_SelectList_PatternResult>();

                    IMultipleResults Results = UnitOfWork.Context.Pattern_Language_SelectList(languageId);

                    Patterns = Results.GetResult<Pattern_Language_SelectList_PatternResult>().ToList();

                    TranslatePatternGridViewModel Model = new TranslatePatternGridViewModel(Patterns, languageId);

                    return PartialView("Patterns/_TranslatePatternGridView", Model);
                }
                else
                {
                    List<Pattern_Language_SelectList_PatternResult> Patterns = new List<Pattern_Language_SelectList_PatternResult>();

                    IMultipleResults Results = UnitOfWork.Context.Pattern_Language_SelectList(languageId);

                    Patterns = Results.GetResult<Pattern_Language_SelectList_PatternResult>().ToList();

                    TranslatePatternGridViewModel Model = new TranslatePatternGridViewModel(Patterns, languageId);

                    return PartialView("Patterns/_TranslatePatternGridView", Model);
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