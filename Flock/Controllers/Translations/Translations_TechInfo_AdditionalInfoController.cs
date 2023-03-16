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
        public ActionResult TechInfo_AdditionalInfo(string targetLanguage)
        {
            var Model = new TranslateTechInfo_AdditionalInfoViewModel();

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

            return View("TechInfo_AdditionalInfo/TranslateTechInfo_AdditionalInfo", Model);
        }

        public ActionResult TranslateAdditionalInfoTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateAdditionalInfoGridView(byte? languageId, string commandType, short? additionalInfoId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechInfo_Ref_AdditionalInfo_Language>().GetAll().Where(p => p.AdditionalInfoID == additionalInfoId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult> TechInfo_AdditionalInfos = new List<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechInfo_AdditionalInfo_Language_SelectList(languageId.Value);

                    TechInfo_AdditionalInfos = Results.GetResult<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult>().ToList();
                }

                TranslateTechInfo_AdditionalInfoGridViewModel Model = new TranslateTechInfo_AdditionalInfoGridViewModel(TechInfo_AdditionalInfos, languageId);

                return PartialView("TechInfo_AdditionalInfo/_TranslateTechInfo_AdditionalInfoGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateAdditionalInfoGridViewUpdate(TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechInfo_AdditionalInfoGridViewModel saveModel = new TranslateTechInfo_AdditionalInfoGridViewModel();
                    saveModel.TechInfo_AdditionalInfos.Add(item);
                    UnitOfWork.Context.TechInfo_AdditionalInfo_Language_Save(languageId, saveModel);

                    List<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult> TechInfo_AdditionalInfos = new List<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_AdditionalInfo_Language_SelectList(languageId);

                    TechInfo_AdditionalInfos = Results.GetResult<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult>().ToList();

                    TranslateTechInfo_AdditionalInfoGridViewModel Model = new TranslateTechInfo_AdditionalInfoGridViewModel(TechInfo_AdditionalInfos, languageId);

                    return PartialView("TechInfo_AdditionalInfo/_TranslateTechInfo_AdditionalInfoGridView", Model);
                }
                else
                {
                    List<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult> TechInfo_AdditionalInfos = new List<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_AdditionalInfo_Language_SelectList(languageId);

                    TechInfo_AdditionalInfos = Results.GetResult<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult>().ToList();

                    TranslateTechInfo_AdditionalInfoGridViewModel Model = new TranslateTechInfo_AdditionalInfoGridViewModel(TechInfo_AdditionalInfos, languageId);

                    return PartialView("TechInfo_AdditionalInfo/_TranslateTechInfo_AdditionalInfoGridView", Model);
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