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
        public ActionResult TechInfo_Aftercare(string targetLanguage)
        {
            var Model = new TranslateTechInfo_AftercareViewModel();

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
            return View("TechInfo_Aftercare/TranslateTechInfo_Aftercare", Model);
        }

        public ActionResult TranslateTechInfo_AftercareTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateTechInfo_AftercareGridView(byte? languageId, string commandType, short? aftercareId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechInfo_Ref_Aftercare_Language>().GetAll().Where(p => p.AftercareID == aftercareId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechInfo_Aftercare_Language_SelectList_AftercareResult> TechInfo_Aftercares = new List<TechInfo_Aftercare_Language_SelectList_AftercareResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Aftercare_Language_SelectList(languageId.Value);

                    TechInfo_Aftercares = Results.GetResult<TechInfo_Aftercare_Language_SelectList_AftercareResult>().ToList();
                }

                TranslateTechInfo_AftercareGridViewModel Model = new TranslateTechInfo_AftercareGridViewModel(TechInfo_Aftercares, languageId);

                return PartialView("TechInfo_Aftercare/_TranslateTechInfo_AftercareGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateTechInfo_AftercareGridViewUpdate(TechInfo_Aftercare_Language_SelectList_AftercareResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechInfo_AftercareGridViewModel saveModel = new TranslateTechInfo_AftercareGridViewModel();
                    saveModel.TechInfo_Aftercares.Add(item);
                    UnitOfWork.Context.TechInfo_Aftercare_Language_Save(languageId, saveModel);

                    List<TechInfo_Aftercare_Language_SelectList_AftercareResult> TechInfo_Aftercares = new List<TechInfo_Aftercare_Language_SelectList_AftercareResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Aftercare_Language_SelectList(languageId);

                    TechInfo_Aftercares = Results.GetResult<TechInfo_Aftercare_Language_SelectList_AftercareResult>().ToList();

                    TranslateTechInfo_AftercareGridViewModel Model = new TranslateTechInfo_AftercareGridViewModel(TechInfo_Aftercares, languageId);

                    return PartialView("TechInfo_Aftercare/_TranslateTechInfo_AftercareGridView", Model);
                }
                else
                {
                    List<TechInfo_Aftercare_Language_SelectList_AftercareResult> TechInfo_Aftercares = new List<TechInfo_Aftercare_Language_SelectList_AftercareResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Aftercare_Language_SelectList(languageId);

                    TechInfo_Aftercares = Results.GetResult<TechInfo_Aftercare_Language_SelectList_AftercareResult>().ToList();

                    TranslateTechInfo_AftercareGridViewModel Model = new TranslateTechInfo_AftercareGridViewModel(TechInfo_Aftercares, languageId);

                    return PartialView("TechInfo_Aftercare/_TranslateTechInfo_AftercareGridView", Model);
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