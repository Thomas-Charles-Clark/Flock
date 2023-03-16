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
        public ActionResult TechInfo_Type(string targetLanguage)
        {
            var Model = new TranslateTechInfo_TypeViewModel();

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

            return View("TechInfo_Type/TranslateTechInfo_Type", Model);
        }

        public ActionResult TranslateTypeTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateTypeGridView(byte? languageId, string commandType, short? typeId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechInfo_Ref_Type_Language>().GetAll().Where(p => p.TypeID == typeId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechInfo_Type_Language_SelectList_TechInfo_TypeResult> TechInfo_Types = new List<TechInfo_Type_Language_SelectList_TechInfo_TypeResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Type_Language_SelectList(languageId.Value);

                    TechInfo_Types = Results.GetResult<TechInfo_Type_Language_SelectList_TechInfo_TypeResult>().ToList();
                }

                TranslateTechInfo_TypeGridViewModel Model = new TranslateTechInfo_TypeGridViewModel(TechInfo_Types, languageId);

                return PartialView("TechInfo_Type/_TranslateTechInfo_TypeGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateTypeGridViewUpdate(TechInfo_Type_Language_SelectList_TechInfo_TypeResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechInfo_TypeGridViewModel saveModel = new TranslateTechInfo_TypeGridViewModel();
                    saveModel.TechInfo_Types.Add(item);
                    UnitOfWork.Context.TechInfo_Type_Language_Save(languageId, saveModel);

                    List<TechInfo_Type_Language_SelectList_TechInfo_TypeResult> TechInfo_Types = new List<TechInfo_Type_Language_SelectList_TechInfo_TypeResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Type_Language_SelectList(languageId);

                    TechInfo_Types = Results.GetResult<TechInfo_Type_Language_SelectList_TechInfo_TypeResult>().ToList();

                    TranslateTechInfo_TypeGridViewModel Model = new TranslateTechInfo_TypeGridViewModel(TechInfo_Types, languageId);

                    return PartialView("TechInfo_Type/_TranslateTechInfo_TypeGridView", Model);
                }
                else
                {
                    List<TechInfo_Type_Language_SelectList_TechInfo_TypeResult> TechInfo_Types = new List<TechInfo_Type_Language_SelectList_TechInfo_TypeResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Type_Language_SelectList(languageId);

                    TechInfo_Types = Results.GetResult<TechInfo_Type_Language_SelectList_TechInfo_TypeResult>().ToList();

                    TranslateTechInfo_TypeGridViewModel Model = new TranslateTechInfo_TypeGridViewModel(TechInfo_Types, languageId);

                    return PartialView("TechInfo_Type/_TranslateTechInfo_TypeGridView", Model);
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