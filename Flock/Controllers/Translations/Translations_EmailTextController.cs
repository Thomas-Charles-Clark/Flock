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
        public ActionResult EmailTexts(string targetLanguage)
        {
            var Model = new TranslateEmailTextViewModel();

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

            return View("EmailTexts/TranslateEmailText", Model);
        }

        public ActionResult TranslateEmailTextTargetLanguageDropDown(byte? TargetLanguageId)
        {
            List<Language> Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => p.DeletedDT == null).ToList();
            
            ViewBag.TargetCountryLanguageID = TargetLanguageId;

            return PartialView("EmailTexts/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateEmailTextGridView(byte? languageId, string commandType, short? emailTextId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.EmailText_Language>().GetAll().Where(p => p.EmailTextID == emailTextId && p.LanguageID == languageId).Single();
                        item.Text = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<EmailText_Language_SelectList_EmailTextResult> EmailTexts = new List<EmailText_Language_SelectList_EmailTextResult>();
                List<EmailText_Language_SelectList_EmailTemplateResult> EmailTemplates = new List<EmailText_Language_SelectList_EmailTemplateResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.EmailText_Language_SelectList(languageId.Value);

                    EmailTexts = Results.GetResult<EmailText_Language_SelectList_EmailTextResult>().ToList();

                    EmailTemplates = Results.GetResult<EmailText_Language_SelectList_EmailTemplateResult>().ToList();
                }

                TranslateEmailTextGridViewModel Model = new TranslateEmailTextGridViewModel(EmailTexts, EmailTemplates, languageId);

                return PartialView("EmailTexts/_TranslateEmailTextGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateEmailTextGridViewUpdate(EmailText_Language_SelectList_EmailTextResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateEmailTextGridViewModel saveModel = new TranslateEmailTextGridViewModel();
                    saveModel.EmailTexts.Add(item);
                    UnitOfWork.Context.EmailText_Language_Save(languageId, saveModel);

                    List<EmailText_Language_SelectList_EmailTextResult> EmailTexts = new List<EmailText_Language_SelectList_EmailTextResult>();
                    List<EmailText_Language_SelectList_EmailTemplateResult> EmailTemplates = new List<EmailText_Language_SelectList_EmailTemplateResult>();
                    
                    IMultipleResults Results = UnitOfWork.Context.EmailText_Language_SelectList(languageId);

                    EmailTexts = Results.GetResult<EmailText_Language_SelectList_EmailTextResult>().ToList();
                    EmailTemplates = Results.GetResult<EmailText_Language_SelectList_EmailTemplateResult>().ToList();

                    TranslateEmailTextGridViewModel Model = new TranslateEmailTextGridViewModel(EmailTexts, EmailTemplates, languageId);

                    return PartialView("EmailTexts/_TranslateEmailTextGridView", Model);
                }
                else
                {
                    List<EmailText_Language_SelectList_EmailTextResult> EmailTexts = new List<EmailText_Language_SelectList_EmailTextResult>();
                    List<EmailText_Language_SelectList_EmailTemplateResult> EmailTemplates = new List<EmailText_Language_SelectList_EmailTemplateResult>();
                    
                    IMultipleResults Results = UnitOfWork.Context.EmailText_Language_SelectList(languageId);

                    EmailTexts = Results.GetResult<EmailText_Language_SelectList_EmailTextResult>().ToList();
                    EmailTemplates = Results.GetResult<EmailText_Language_SelectList_EmailTemplateResult>().ToList();

                    TranslateEmailTextGridViewModel Model = new TranslateEmailTextGridViewModel(EmailTexts, EmailTemplates, languageId);

                    return PartialView("EmailTexts/_TranslateEmailTextGridView", Model);
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