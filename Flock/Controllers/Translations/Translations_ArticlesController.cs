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
        public ActionResult Articles(string targetLanguage)
        {
            var Model = new TranslateArticleViewModel();

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
            //    if (id != null)
            //    {
            //        try
            //        {
            //            short collId = Convert.ToInt16(id.Value);
            //            var coll = UnitOfWork.GetRepository<Article>().GetAll().Where(p => p.ArticleID == collId && p.DeletedDT == null).SingleOrDefault();
            //            if (coll == null)
            //            {
            //                ModelState.AddModelError("ArticleID", "Invalid ArticleID, or that Article has been deleted. Deleted Articles can be retrieved.");
            //            }
            //            else
            //            {
            //                Model.ArticleID = coll.ArticleID;
            //            }
            //        }
            //        catch(Exception ex)
            //        {
            //            ModelState.AddModelError("ArticleID", "Invalid ArticleID, or that Article has been deleted. Deleted Articles can be retrieved.\r\n\r\n" + ex.Message);
            //        }
            //    }
            //}

            return View("Articles/TranslateArticle", Model);
        }

        public ActionResult TranslateArticleTargetLanguageDropDown(byte? TargetLanguageID)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = TargetLanguageID;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateArticleGridView(string commandType, int? articleId, byte? languageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var article = UnitOfWork.GetRepository<Models.Article_Language>().GetAll().Where(p => p.ArticleID == articleId && p.LanguageID == languageId).Single();
                        article.Contents = null;
                        article.Heading = null;
                        article.MetaDescription = null;
                        article.TitleTag = null;
                        article.SubHeading = null;
                        article.URLKeywords = null;
                        article.Stage = null;
                        article.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Article_Language_SelectList_ArticleResult> Articles = new List<Article_Language_SelectList_ArticleResult>();
                List<Brand_SelectResult> Brands = new List<Brand_SelectResult>();
                List<ArticleSubType_SelectResult> ArticleSubTypes = new List<ArticleSubType_SelectResult>();
                List<Status_SelectResult> Statuses = new List<Status_SelectResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Article_Language_SelectList(languageId.Value);

                    Articles = Results.GetResult<Article_Language_SelectList_ArticleResult>().ToList();
                    ArticleSubTypes = Results.GetResult<ArticleSubType_SelectResult>().ToList();
                    Brands = Results.GetResult<Brand_SelectResult>().ToList();
                    Statuses = Results.GetResult<Status_SelectResult>().ToList();
                }

                int articlesToTranslateCount = Articles.Where(p => p.FieldsToReview > 0).Count();

                TranslateArticleGridViewModel Model = new TranslateArticleGridViewModel(Articles, Brands, ArticleSubTypes, Statuses, articlesToTranslateCount, languageId);

                if(articleId != null)
                {
                    ViewBag.SelectedArticleID = articleId.Value;
                }

                return PartialView("Articles/_TranslateArticleGridView", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult LoadArticleTranslator(short ArticleID, string TargetLanguage, byte targetLanguageId)
        {
            try
            {
                IMultipleResults Results = UnitOfWork.Context.Article_Language_SelectDetail(ArticleID, targetLanguageId);

                TranslateArticleDetailViewModel Model = new TranslateArticleDetailViewModel(
                    ArticleID,
                    targetLanguageId,
                    TargetLanguage,
                    Results.GetResult<Article_Language_SelectDetail_ArticleResult>().Single(),
                    Results.GetResult<Article_Language_SelectDetail_TranslationResult>().Single(),
                    Results.GetResult<Article_Language_SelectDetail_ReviewRequiredResult>().Single(),
                    Results.GetResult<Article_Language_SelectDetail_ImageResult>().Single()
                );

                return PartialView("Articles/_TranslateArticlePartial", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Next_Article(short ArticleID)
        {
            return Content("");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save_Article(
            short ArticleID, 
            byte LanguageID,
            string Heading, 
            string SubHeading,
            string Contents,
            string TitleTag,
            string MetaDescription,
            string TranslatedHeading,
            string TranslatedSubHeading,
            string TranslatedContents,
            string TranslatedTitleTag,
            string TranslatedMetaDescription)
        {
            try
            {
                UnitOfWork.Context.Article_Language_Save(ArticleID, LanguageID,
                    new Article_Language_SelectDetail_ArticleResult { 
                        Heading = Heading, 
                        SubHeading = SubHeading, 
                        Contents = Contents, 
                        TitleTag = TitleTag, 
                        MetaDescription = MetaDescription
                    }, 
                    new Article_Language_SelectDetail_TranslationResult { 
                        Heading = TranslatedHeading, 
                        SubHeading = TranslatedSubHeading, 
                        Contents = TranslatedContents, 
                        TitleTag = TranslatedTitleTag, 
                        MetaDescription = TranslatedMetaDescription
                    }
                );

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_Article(ArticleID, LanguageID, null, null, null, null, false, false);

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(LanguageID, null, null, null, ArticleID, false, false);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }

            return Content("ok");
        }
    }
}