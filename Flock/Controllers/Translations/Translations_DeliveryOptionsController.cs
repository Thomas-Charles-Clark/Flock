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
        public ActionResult DeliveryOptions(string targetLanguage)
        {
            var Model = new TranslateDeliveryOptionViewModel();

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


            return View("DeliveryOptions/TranslateDeliveryOption", Model);
        }

        public ActionResult TranslateDeliveryOptionTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateDeliveryOptionGridView(byte? languageId, string commandType, byte? deliveryOptionId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.DeliveryOption_Language>().GetAll().Where(p => p.DeliveryOptionID == deliveryOptionId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<DeliveryOption_Language_SelectList_DeliveryOptionResult> DeliveryOptions = new List<DeliveryOption_Language_SelectList_DeliveryOptionResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.DeliveryOption_Language_SelectList(languageId.Value);

                    DeliveryOptions = Results.GetResult<DeliveryOption_Language_SelectList_DeliveryOptionResult>().ToList();
                }

                TranslateDeliveryOptionGridViewModel Model = new TranslateDeliveryOptionGridViewModel(DeliveryOptions, languageId);

                return PartialView("DeliveryOptions/_TranslateDeliveryOptionGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }


        [ValidateInput(false)]
        public ActionResult TranslateDeliveryOptionGridViewUpdate(DeliveryOption_Language_SelectList_DeliveryOptionResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateDeliveryOptionGridViewModel();
                    saveModel.DeliveryOptions.Add(item);
                    UnitOfWork.Context.DeliveryOption_Language_Save(languageId, saveModel);

                    List<DeliveryOption_Language_SelectList_DeliveryOptionResult> DeliveryOptions = new List<DeliveryOption_Language_SelectList_DeliveryOptionResult>();
                    IMultipleResults Results = UnitOfWork.Context.DeliveryOption_Language_SelectList(languageId);
                    DeliveryOptions = Results.GetResult<DeliveryOption_Language_SelectList_DeliveryOptionResult>().ToList();
                    var Model = new TranslateDeliveryOptionGridViewModel(DeliveryOptions, languageId);

                    return PartialView("DeliveryOptions/_TranslateDeliveryOptionGridView", Model);
                }
                else
                {
                    List<DeliveryOption_Language_SelectList_DeliveryOptionResult> DeliveryOptions = new List<DeliveryOption_Language_SelectList_DeliveryOptionResult>();
                    IMultipleResults Results = UnitOfWork.Context.DeliveryOption_Language_SelectList(languageId);
                    DeliveryOptions = Results.GetResult<DeliveryOption_Language_SelectList_DeliveryOptionResult>().ToList();
                    var Model = new TranslateDeliveryOptionGridViewModel(DeliveryOptions, languageId);

                    return PartialView("DeliveryOptions/_TranslateDeliveryOptionGridView", Model);
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