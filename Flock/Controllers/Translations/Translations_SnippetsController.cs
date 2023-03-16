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
        public ActionResult Snippets()
        {
            var Model = new TranslateSnippetViewModel();

            //if(targetLanguage != null)
            //{
            //if(targetLanguage.Length > 2 || targetLanguage.Length < 2)
            //{
            //    ModelState.AddModelError("TargetLanguage", "Invalid language code detected");
            //}
            //else
            //{
            //    var lang = UnitOfWork.GetRepository<Language>().GetAll().Where(p => p.Code == targetLanguage).SingleOrDefault();

            //    if (lang == null)
            //    {
            //        ModelState.AddModelError("TargetLanguage", "Invalid language code detected");
            //    }
            //    else
            //    {
            //        Model.TargetLanguageID = lang.LanguageID;
            //    }
            //}

            //if (id != null)
            //{
            //    try
            //    {
            //        var pp = UnitOfWork.GetRepository<HTMLSection>().GetAll().Where(p => p.SnippetID == id.Value && p.DeletedDT == null).SingleOrDefault();

            //        if (pp == null)
            //        {
            //            ModelState.AddModelError("SnippetID", "Invalid SnippetID, or that Snippet has been deleted. Deleted Snippets can be retrieved.");
            //        }
            //        else
            //        {
            //            Model.SnippetID = pp.SnippetID;
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        ModelState.AddModelError("SnippetID", "Invalid SnippetID, or that Snippet has been deleted. Deleted Snippets can be retrieved.");
            //    }
            //}
            //}

            return View("Snippets/TranslateSnippet", Model);
        }

        public ActionResult TranslateSnippetTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateSnippetGridView(byte? languageId, string commandType, short? snippetId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Snippet_Language>().GetAll().Where(p => p.SnippetID == snippetId && p.LanguageID == languageId).Single();
                        item.HTMLContent = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Snippet_Language_SelectList_SnippetResult> Snippets = new List<Snippet_Language_SelectList_SnippetResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Snippet_Language_SelectList(languageId.Value);

                    Snippets = Results.GetResult<Snippet_Language_SelectList_SnippetResult>().ToList();
                }

                int SnippetsToTranslateCount = Snippets.Where(p => p.FieldsToReview > 0).Count();

                TranslateSnippetGridViewModel Model = new TranslateSnippetGridViewModel(Snippets, SnippetsToTranslateCount, languageId);

                if (snippetId != null)
                {
                    ViewBag.SelectedSnippetID = snippetId.Value;
                }

                return PartialView("Snippets/_TranslateSnippetGridView", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult LoadSnippetTranslator(short SnippetID, string TargetLanguage, byte TargetLanguageID)
        {
            try
            {
                IMultipleResults Results = UnitOfWork.Context.Snippet_Language_SelectDetail(SnippetID, TargetLanguageID);

                TranslateSnippetDetailViewModel Model = new TranslateSnippetDetailViewModel(
                    SnippetID,
                    TargetLanguageID,
                    TargetLanguage,
                    Results.GetResult<Snippet_Language_SelectDetail_SnippetResult>().Single(),
                    Results.GetResult<Snippet_Language_SelectDetail_TranslationResult>().Single(),
                    Results.GetResult<Snippet_Language_SelectDetail_ReviewRequiredResult>().Single()
                );

                return PartialView("Snippets/_TranslateSnippetPartial", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Next_Snippet(int SnippetID)
        {
            return Content("");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save_Snippet(
            short SnippetID, 
            byte languageId,
            string OriginalContents,
            string TranslatedContents)
        {
            try
            {
                UnitOfWork.Context.Snippet_Language_Save(languageId, SnippetID, OriginalContents, TranslatedContents);
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