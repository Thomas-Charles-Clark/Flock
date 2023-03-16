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
        public ActionResult TechInfo_Flamcode(string targetLanguage)
        {
            var Model = new TranslateTechInfo_FlamcodeViewModel();

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
            return View("TechInfo_Flamcode/TranslateTechInfo_Flamcode", Model);
        }

        public ActionResult TranslateFlamcodeTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateFlamcodeGridView(byte? languageId, string commandType, short? flamcodeId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechInfo_Ref_Flamcode_Language>().GetAll().Where(p => p.FlamcodeID == flamcodeId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult> TechInfo_Flamcodes = new List<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Flamcode_Language_SelectList(languageId.Value);

                    TechInfo_Flamcodes = Results.GetResult<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult>().ToList();
                }

                TranslateTechInfo_FlamcodeGridViewModel Model = new TranslateTechInfo_FlamcodeGridViewModel(TechInfo_Flamcodes, languageId);

                return PartialView("TechInfo_Flamcode/_TranslateTechInfo_FlamcodeGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateFlamcodeGridViewUpdate(TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechInfo_FlamcodeGridViewModel saveModel = new TranslateTechInfo_FlamcodeGridViewModel();
                    saveModel.TechInfo_Flamcodes.Add(item);
                    UnitOfWork.Context.TechInfo_Flamcode_Language_Save(languageId, saveModel);

                    List<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult> TechInfo_Flamcodes = new List<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Flamcode_Language_SelectList(languageId);

                    TechInfo_Flamcodes = Results.GetResult<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult>().ToList();

                    TranslateTechInfo_FlamcodeGridViewModel Model = new TranslateTechInfo_FlamcodeGridViewModel(TechInfo_Flamcodes, languageId);

                    return PartialView("TechInfo_Flamcode/_TranslateTechInfo_FlamcodeGridView", Model);
                }
                else
                {
                    List<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult> TechInfo_Flamcodes = new List<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Flamcode_Language_SelectList(languageId);

                    TechInfo_Flamcodes = Results.GetResult<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult>().ToList();

                    TranslateTechInfo_FlamcodeGridViewModel Model = new TranslateTechInfo_FlamcodeGridViewModel(TechInfo_Flamcodes, languageId);

                    return PartialView("TechInfo_Flamcode/_TranslateTechInfo_FlamcodeGridView", Model);
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