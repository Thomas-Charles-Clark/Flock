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
        public ActionResult Forms(string targetLanguage)
        {
            var Model = new TranslateFormViewModel();

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
            
            return View("Forms/TranslateForm", Model);
        }

        public ActionResult TranslateFormTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateFormGridView(byte? languageId, string commandType, short? formId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Form_Language>().GetAll().Where(p => p.FormID == formId && p.LanguageID == languageId).Single();
                        item.Heading = null;
                        item.SubHeading = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Form_Language_SelectList_FormResult> Forms = new List<Form_Language_SelectList_FormResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Form_Language_SelectList(languageId.Value);

                    Forms = Results.GetResult<Form_Language_SelectList_FormResult>().ToList();
                }

                TranslateFormGridViewModel Model = new TranslateFormGridViewModel(Forms, languageId);

                return PartialView("Forms/_TranslateFormGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateFormGridViewUpdate(Form_Language_SelectList_FormResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateFormGridViewModel saveModel = new TranslateFormGridViewModel();
                    saveModel.Forms.Add(item);
                    UnitOfWork.Context.Form_Language_Save(languageId, saveModel);

                    List<Form_Language_SelectList_FormResult> Forms = new List<Form_Language_SelectList_FormResult>();

                    IMultipleResults Results = UnitOfWork.Context.Form_Language_SelectList(languageId);

                    Forms = Results.GetResult<Form_Language_SelectList_FormResult>().ToList();

                    TranslateFormGridViewModel Model = new TranslateFormGridViewModel(Forms, languageId);

                    return PartialView("Forms/_TranslateFormGridView", Model);
                }
                else
                {
                    List<Form_Language_SelectList_FormResult> Forms = new List<Form_Language_SelectList_FormResult>();

                    IMultipleResults Results = UnitOfWork.Context.Form_Language_SelectList(languageId);

                    Forms = Results.GetResult<Form_Language_SelectList_FormResult>().ToList();

                    TranslateFormGridViewModel Model = new TranslateFormGridViewModel(Forms, languageId);

                    return PartialView("Forms/_TranslateFormGridView", Model);
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