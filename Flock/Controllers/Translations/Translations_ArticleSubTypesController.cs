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
        public ActionResult ArticleSubTypes(string targetLanguage)
        {
            var Model = new TranslateArticleSubTypeViewModel();

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

            return View("ArticleSubTypes/TranslateArticleSubType", Model);
        }

        public ActionResult TranslateArticleSubTypeTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateArticleSubTypeGridView(string commandType, byte? articleSubTypeId, byte? languageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var articleSubType = UnitOfWork.GetRepository<Models.ArticleSubType_Language>().GetAll().Where(p => p.ArticleSubTypeID == articleSubTypeId && p.LanguageID == languageId).Single();
                        articleSubType.Name = null;
                        articleSubType.Stage = null;
                        articleSubType.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<ArticleSubType_Language_SelectList_ArticleSubTypeResult> ArticleSubTypes = new List<ArticleSubType_Language_SelectList_ArticleSubTypeResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.ArticleSubType_Language_SelectList(languageId.Value);

                    ArticleSubTypes = Results.GetResult<ArticleSubType_Language_SelectList_ArticleSubTypeResult>().ToList();
                }

                TranslateArticleSubTypeGridViewModel Model = new TranslateArticleSubTypeGridViewModel(ArticleSubTypes, languageId);

                return PartialView("ArticleSubTypes/_TranslateArticleSubTypeGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateArticleSubTypeGridViewUpdate(ArticleSubType_Language_SelectList_ArticleSubTypeResult item, byte languageId)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateArticleSubTypeGridViewModel();
                    saveModel.ArticleSubTypes.Add(item);
                    UnitOfWork.Context.ArticleSubType_Language_Save(languageId, saveModel);

                    List<ArticleSubType_Language_SelectList_ArticleSubTypeResult> ArticleSubTypes = new List<ArticleSubType_Language_SelectList_ArticleSubTypeResult>();
                    IMultipleResults Results = UnitOfWork.Context.ArticleSubType_Language_SelectList(languageId);
                    ArticleSubTypes = Results.GetResult<ArticleSubType_Language_SelectList_ArticleSubTypeResult>().ToList();
                    var Model = new TranslateArticleSubTypeGridViewModel(ArticleSubTypes, languageId);

                    return PartialView("ArticleSubTypes/_TranslateArticleSubTypeGridView", Model);
                }
                else
                {
                    List<ArticleSubType_Language_SelectList_ArticleSubTypeResult> ArticleSubTypes = new List<ArticleSubType_Language_SelectList_ArticleSubTypeResult>();
                    IMultipleResults Results = UnitOfWork.Context.ArticleSubType_Language_SelectList(languageId);
                    ArticleSubTypes = Results.GetResult<ArticleSubType_Language_SelectList_ArticleSubTypeResult>().ToList();
                    var Model = new TranslateArticleSubTypeGridViewModel(ArticleSubTypes, languageId);

                    return PartialView("ArticleSubTypes/_TranslateArticleSubTypeGridView", Model);
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