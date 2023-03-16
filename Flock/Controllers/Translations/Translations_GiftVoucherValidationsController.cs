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
        public ActionResult GiftVoucherValidations(string targetLanguage)
        {
            var Model = new TranslateGiftVoucherValidationViewModel();

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

            return View("GiftVoucherValidations/TranslateGiftVoucherValidation", Model);
        }

        public ActionResult TranslateGiftVoucherValidationTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateGiftVoucherValidationGridView(byte? languageId, string commandType, byte? giftVoucherValidationId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.GiftVoucherValidation_Language>().GetAll().Where(p => p.GiftVoucherValidationID == giftVoucherValidationId && p.LanguageID == languageId).Single();
                        item.Message = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult> GiftVoucherValidations = new List<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.GiftVoucherValidation_Language_SelectList(languageId.Value);

                    GiftVoucherValidations = Results.GetResult<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult>().ToList();
                }

                TranslateGiftVoucherValidationGridViewModel Model = new TranslateGiftVoucherValidationGridViewModel(GiftVoucherValidations, languageId);

                return PartialView("GiftVoucherValidations/_TranslateGiftVoucherValidationGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateGiftVoucherValidationGridViewUpdate(GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateGiftVoucherValidationGridViewModel();
                    saveModel.GiftVoucherValidations.Add(item);
                    UnitOfWork.Context.GiftVoucherValidation_Language_Save(languageId, saveModel);

                    List<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult> GiftVoucherValidations = new List<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult>();
                    IMultipleResults Results = UnitOfWork.Context.GiftVoucherValidation_Language_SelectList(languageId);
                    GiftVoucherValidations = Results.GetResult<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult>().ToList();
                    var Model = new TranslateGiftVoucherValidationGridViewModel(GiftVoucherValidations, languageId);

                    return PartialView("GiftVoucherValidations/_TranslateGiftVoucherValidationGridView", Model);
                }
                else
                {
                    List<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult> GiftVoucherValidations = new List<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult>();
                    IMultipleResults Results = UnitOfWork.Context.GiftVoucherValidation_Language_SelectList(languageId);
                    GiftVoucherValidations = Results.GetResult<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult>().ToList();
                    var Model = new TranslateGiftVoucherValidationGridViewModel(GiftVoucherValidations, languageId);

                    return PartialView("GiftVoucherValidations/_TranslateGiftVoucherValidationGridView", Model);
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