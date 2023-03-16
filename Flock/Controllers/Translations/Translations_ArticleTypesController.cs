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
        public ActionResult ArticleTypes(string targetLanguage)
        {
            var Model = new TranslateArticleTypeViewModel();

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

            return View("ArticleTypes/TranslateArticleType", Model);
        }

        public ActionResult TranslateArticleTypeTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateArticleTypeGridView(string commandType, byte? articleTypeId, byte? languageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var announcement = UnitOfWork.GetRepository<Models.ArticleType_Language>().GetAll().Where(p => p.ArticleTypeID == articleTypeId && p.LanguageID == languageId).Single();
                        announcement.Name = null;
                        announcement.Stage = null;
                        announcement.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<ArticleType_Language_SelectList_ArticleTypeResult> ArticleTypes = new List<ArticleType_Language_SelectList_ArticleTypeResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.ArticleType_Language_SelectList(languageId.Value);

                    ArticleTypes = Results.GetResult<ArticleType_Language_SelectList_ArticleTypeResult>().ToList();
                }

                TranslateArticleTypeGridViewModel Model = new TranslateArticleTypeGridViewModel(ArticleTypes, languageId);

                return PartialView("ArticleTypes/_TranslateArticleTypeGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateArticleTypeGridViewUpdate(ArticleType_Language_SelectList_ArticleTypeResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateArticleTypeGridViewModel();
                    saveModel.ArticleTypes.Add(item);
                    UnitOfWork.Context.ArticleType_Language_Save(languageId, saveModel);

                    List<ArticleType_Language_SelectList_ArticleTypeResult> ArticleTypes = new List<ArticleType_Language_SelectList_ArticleTypeResult>();
                    IMultipleResults Results = UnitOfWork.Context.ArticleType_Language_SelectList(languageId);
                    ArticleTypes = Results.GetResult<ArticleType_Language_SelectList_ArticleTypeResult>().ToList();
                    var Model = new TranslateArticleTypeGridViewModel(ArticleTypes, languageId);

                    return PartialView("ArticleTypes/_TranslateArticleTypeGridView", Model);
                }
                else
                {
                    List<ArticleType_Language_SelectList_ArticleTypeResult> ArticleTypes = new List<ArticleType_Language_SelectList_ArticleTypeResult>();
                    IMultipleResults Results = UnitOfWork.Context.ArticleType_Language_SelectList(languageId);
                    ArticleTypes = Results.GetResult<ArticleType_Language_SelectList_ArticleTypeResult>().ToList();
                    var Model = new TranslateArticleTypeGridViewModel(ArticleTypes, languageId);

                    return PartialView("ArticleTypes/_TranslateArticleTypeGridView", Model);
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