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
        public ActionResult TechInfo_ContractFR(string targetLanguage)
        {
            var Model = new TranslateTechInfo_ContractFRViewModel();

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
            return View("TechInfo_ContractFR/TranslateTechInfo_ContractFR", Model);
        }

        public ActionResult TranslateContractFRTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateTechInfo_ContractFRGridView(byte? languageId, string commandType, short? contractFRId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechInfo_Ref_ContractFR_Language>().GetAll().Where(p => p.ContractFRID == contractFRId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult> TechInfo_ContractFRs = new List<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechInfo_ContractFR_Language_SelectList(languageId.Value);

                    TechInfo_ContractFRs = Results.GetResult<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult>().ToList();
                }

                TranslateTechInfo_ContractFRGridViewModel Model = new TranslateTechInfo_ContractFRGridViewModel(TechInfo_ContractFRs, languageId);

                return PartialView("TechInfo_ContractFR/_TranslateTechInfo_ContractFRGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateContractFRGridViewUpdate(TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechInfo_ContractFRGridViewModel saveModel = new TranslateTechInfo_ContractFRGridViewModel();
                    saveModel.TechInfo_ContractFRs.Add(item);
                    UnitOfWork.Context.TechInfo_ContractFR_Language_Save(languageId, saveModel);

                    List<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult> TechInfo_ContractFRs = new List<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_ContractFR_Language_SelectList(languageId);

                    TechInfo_ContractFRs = Results.GetResult<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult>().ToList();

                    TranslateTechInfo_ContractFRGridViewModel Model = new TranslateTechInfo_ContractFRGridViewModel(TechInfo_ContractFRs, languageId);

                    return PartialView("TechInfo_ContractFR/_TranslateTechInfo_ContractFRGridView", Model);
                }
                else
                {
                    List<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult> TechInfo_ContractFRs = new List<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_ContractFR_Language_SelectList(languageId);

                    TechInfo_ContractFRs = Results.GetResult<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult>().ToList();

                    TranslateTechInfo_ContractFRGridViewModel Model = new TranslateTechInfo_ContractFRGridViewModel(TechInfo_ContractFRs, languageId);

                    return PartialView("TechInfo_ContractFR/_TranslateTechInfo_ContractFRGridView", Model);
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