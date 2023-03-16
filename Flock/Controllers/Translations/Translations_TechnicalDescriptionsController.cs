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
        public ActionResult TechnicalDescriptions(string targetLanguage)
        {
            var Model = new TranslateTechnicalDescriptionViewModel();

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

            return View("TechnicalDescriptions/TranslateTechnicalDescription", Model);
        }

        public ActionResult TranslateTechnicalDescriptionTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateTechnicalDescriptionGridView(byte? languageId, string commandType, short? technicalDescriptionId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechnicalDescription_Language>().GetAll().Where(p => p.TechnicalDescriptionID == technicalDescriptionId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult> TechnicalDescriptions = new List<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechnicalDescription_Language_SelectList(languageId.Value);

                    TechnicalDescriptions = Results.GetResult<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult>().ToList();
                }

                TranslateTechnicalDescriptionGridViewModel Model = new TranslateTechnicalDescriptionGridViewModel(TechnicalDescriptions, languageId);

                return PartialView("TechnicalDescriptions/_TranslateTechnicalDescriptionGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateTechnicalDescriptionGridViewUpdate(TechnicalDescription_Language_SelectList_TechnicalDescriptionResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechnicalDescriptionGridViewModel saveModel = new TranslateTechnicalDescriptionGridViewModel();
                    saveModel.TechnicalDescriptions.Add(item);
                    UnitOfWork.Context.TechnicalDescription_Language_Save(languageId, saveModel);

                    List<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult> TechnicalDescriptions = new List<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechnicalDescription_Language_SelectList(languageId);

                    TechnicalDescriptions = Results.GetResult<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult>().ToList();

                    TranslateTechnicalDescriptionGridViewModel Model = new TranslateTechnicalDescriptionGridViewModel(TechnicalDescriptions, languageId);

                    return PartialView("TechnicalDescriptions/_TranslateTechnicalDescriptionGridView", Model);
                }
                else
                {
                    List<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult> TechnicalDescriptions = new List<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechnicalDescription_Language_SelectList(languageId);

                    TechnicalDescriptions = Results.GetResult<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult>().ToList();

                    TranslateTechnicalDescriptionGridViewModel Model = new TranslateTechnicalDescriptionGridViewModel(TechnicalDescriptions, languageId);

                    return PartialView("TechnicalDescriptions/_TranslateTechnicalDescriptionGridView", Model);
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