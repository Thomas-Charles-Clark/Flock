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
        public ActionResult TechInfo_Usage(string targetLanguage)
        {
            var Model = new TranslateTechInfo_UsageViewModel();

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

            return View("TechInfo_Usage/TranslateTechInfo_Usage", Model);
        }

        public ActionResult TranslateTechInfo_UsageTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateTechInfo_UsageGridView(byte? languageId, string commandType, short? usageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechInfo_Ref_Usage_Language>().GetAll().Where(p => p.UsageID == usageId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult> TechInfo_Usages = new List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Usage_Language_SelectList(languageId.Value);

                    TechInfo_Usages = Results.GetResult<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult>().ToList();
                }

                TranslateTechInfo_UsageGridViewModel Model = new TranslateTechInfo_UsageGridViewModel(TechInfo_Usages, languageId);

                return PartialView("TechInfo_Usage/_TranslateTechInfo_UsageGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateTechInfo_UsageGridViewUpdate(TechInfo_Usage_Language_SelectList_TechInfo_UsageResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechInfo_UsageGridViewModel saveModel = new TranslateTechInfo_UsageGridViewModel();
                    saveModel.TechInfo_Usages.Add(item);
                    UnitOfWork.Context.TechInfo_Usage_Language_Save(languageId, saveModel);

                    List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult> TechInfo_Usages = new List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Usage_Language_SelectList(languageId);

                    TechInfo_Usages = Results.GetResult<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult>().ToList();

                    TranslateTechInfo_UsageGridViewModel Model = new TranslateTechInfo_UsageGridViewModel(TechInfo_Usages, languageId);

                    return PartialView("TechInfo_Usage/_TranslateTechInfo_UsageGridView", Model);
                }
                else
                {
                    List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult> TechInfo_Usages = new List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Usage_Language_SelectList(languageId);

                    TechInfo_Usages = Results.GetResult<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult>().ToList();

                    TranslateTechInfo_UsageGridViewModel Model = new TranslateTechInfo_UsageGridViewModel(TechInfo_Usages, languageId);

                    return PartialView("TechInfo_Usage/_TranslateTechInfo_UsageGridView", Model);
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