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
        public ActionResult CallsToAction(string targetLanguage)
        {
            var Model = new TranslateCallToActionViewModel();

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
            return View("CallsToAction/TranslateCallToAction", Model);
        }

        public ActionResult TranslateCallToActionTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateCallToActionGridView(byte? languageId, string commandType, int? callToActionId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.CallToAction_Language>().GetAll().Where(p => p.CallToActionID == callToActionId && p.LanguageID == languageId).Single();
                        item.Text = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<CallToAction_Language_SelectList_CallToActionResult> CallsToAction = new List<CallToAction_Language_SelectList_CallToActionResult>();
                
                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.CallToAction_Language_SelectList(languageId.Value);

                    CallsToAction = Results.GetResult<CallToAction_Language_SelectList_CallToActionResult>().ToList();
                }

                TranslateCallToActionGridViewModel Model = new TranslateCallToActionGridViewModel(CallsToAction, languageId);

                return PartialView("CallsToAction/_TranslateCallToActionGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateCallToActionGridViewUpdate(CallToAction_Language_SelectList_CallToActionResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TranslateCallToActionGridViewModel saveModel = new TranslateCallToActionGridViewModel();
                    saveModel.CallsToAction.Add(item);
                    UnitOfWork.Context.CallToAction_Language_Save(languageId, saveModel);

                    List<CallToAction_Language_SelectList_CallToActionResult> CallsToAction = new List<CallToAction_Language_SelectList_CallToActionResult>();

                    IMultipleResults Results = UnitOfWork.Context.CallToAction_Language_SelectList(languageId);

                    CallsToAction = Results.GetResult<CallToAction_Language_SelectList_CallToActionResult>().ToList();

                    TranslateCallToActionGridViewModel Model = new TranslateCallToActionGridViewModel(CallsToAction, languageId);

                    return PartialView("CallsToAction/_TranslateCallToActionGridView", Model);
                }
                else
                {
                    List<CallToAction_Language_SelectList_CallToActionResult> CallsToAction = new List<CallToAction_Language_SelectList_CallToActionResult>();

                    IMultipleResults Results = UnitOfWork.Context.CallToAction_Language_SelectList(languageId);

                    CallsToAction = Results.GetResult<CallToAction_Language_SelectList_CallToActionResult>().ToList();

                    TranslateCallToActionGridViewModel Model = new TranslateCallToActionGridViewModel(CallsToAction, languageId);

                    return PartialView("CallsToAction/_TranslateCallToActionGridView", Model);
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