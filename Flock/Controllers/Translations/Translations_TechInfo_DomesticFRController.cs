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
        public ActionResult TechInfo_DomesticFR(string targetLanguage)
        {
            var Model = new TranslateTechInfo_DomesticFRViewModel();

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
            return View("TechInfo_DomesticFR/TranslateTechInfo_DomesticFR", Model);
        }

        public ActionResult TranslateDomesticFRTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateDomesticFRGridView(byte? languageId, string commandType, short? domesticFRId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechInfo_Ref_DomesticFR_Language>().GetAll().Where(p => p.DomesticFRID == domesticFRId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult> TechInfo_DomesticFRs = new List<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechInfo_DomesticFR_Language_SelectList(languageId.Value);

                    TechInfo_DomesticFRs = Results.GetResult<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult>().ToList();
                }

                TranslateTechInfo_DomesticFRGridViewModel Model = new TranslateTechInfo_DomesticFRGridViewModel(TechInfo_DomesticFRs, languageId);

                return PartialView("TechInfo_DomesticFR/_TranslateTechInfo_DomesticFRGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateDomesticFRGridViewUpdate(TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechInfo_DomesticFRGridViewModel saveModel = new TranslateTechInfo_DomesticFRGridViewModel();
                    saveModel.TechInfo_DomesticFRs.Add(item);
                    UnitOfWork.Context.TechInfo_DomesticFR_Language_Save(languageId, saveModel);

                    List<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult> TechInfo_DomesticFRs = new List<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_DomesticFR_Language_SelectList(languageId);

                    TechInfo_DomesticFRs = Results.GetResult<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult>().ToList();

                    TranslateTechInfo_DomesticFRGridViewModel Model = new TranslateTechInfo_DomesticFRGridViewModel(TechInfo_DomesticFRs, languageId);

                    return PartialView("TechInfo_DomesticFR/_TranslateTechInfo_DomesticFRGridView", Model);
                }
                else
                {
                    List<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult> TechInfo_DomesticFRs = new List<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_DomesticFR_Language_SelectList(languageId);

                    TechInfo_DomesticFRs = Results.GetResult<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult>().ToList();

                    TranslateTechInfo_DomesticFRGridViewModel Model = new TranslateTechInfo_DomesticFRGridViewModel(TechInfo_DomesticFRs, languageId);

                    return PartialView("TechInfo_DomesticFR/_TranslateTechInfo_DomesticFRGridView", Model);
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