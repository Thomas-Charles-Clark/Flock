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
        public ActionResult TechInfo_PatternMatch(string targetLanguage)
        {
            var Model = new TranslateTechInfo_PatternMatchViewModel();

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

            return View("TechInfo_PatternMatch/TranslateTechInfo_PatternMatch", Model);
        }

        public ActionResult TranslatePatternMatchTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslatePatternMatchGridView(byte? languageId, string commandType, short? patternMatchId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechInfo_Ref_PatternMatch_Language>().GetAll().Where(p => p.PatternMatchID == patternMatchId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult> TechInfo_PatternMatchs = new List<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechInfo_PatternMatch_Language_SelectList(languageId.Value);

                    TechInfo_PatternMatchs = Results.GetResult<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult>().ToList();
                }

                TranslateTechInfo_PatternMatchGridViewModel Model = new TranslateTechInfo_PatternMatchGridViewModel(TechInfo_PatternMatchs, languageId);

                return PartialView("TechInfo_PatternMatch/_TranslateTechInfo_PatternMatchGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslatePatternMatchGridViewUpdate(TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechInfo_PatternMatchGridViewModel saveModel = new TranslateTechInfo_PatternMatchGridViewModel();
                    saveModel.TechInfo_PatternMatchs.Add(item);
                    UnitOfWork.Context.TechInfo_PatternMatch_Language_Save(languageId, saveModel);

                    List<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult> TechInfo_PatternMatchs = new List<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_PatternMatch_Language_SelectList(languageId);

                    TechInfo_PatternMatchs = Results.GetResult<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult>().ToList();

                    TranslateTechInfo_PatternMatchGridViewModel Model = new TranslateTechInfo_PatternMatchGridViewModel(TechInfo_PatternMatchs, languageId);

                    return PartialView("TechInfo_PatternMatch/_TranslateTechInfo_PatternMatchGridView", Model);
                }
                else
                {
                    List<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult> TechInfo_PatternMatchs = new List<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_PatternMatch_Language_SelectList(languageId);

                    TechInfo_PatternMatchs = Results.GetResult<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult>().ToList();

                    TranslateTechInfo_PatternMatchGridViewModel Model = new TranslateTechInfo_PatternMatchGridViewModel(TechInfo_PatternMatchs, languageId);

                    return PartialView("TechInfo_PatternMatch/_TranslateTechInfo_PatternMatchGridView", Model);
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