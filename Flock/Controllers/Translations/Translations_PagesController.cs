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
        public ActionResult Pages(byte? targetCountryLanguageID)
        {
            var Model = new TranslatePageViewModel();

            if (targetCountryLanguageID != null)
            {
                var lang = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => p.Country_Language_ID == targetCountryLanguageID).SingleOrDefault();

                if (lang == null)
                {
                    ModelState.AddModelError("TargetLanguage", "Invalid CountryLanguageID detected.");
                }
                else
                {
                    Model.TargetLanguageID = lang.LanguageID;
                }
            }
            return View("Pages/TranslatePage", Model);
        }

        public ActionResult TranslatePageTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslatePageGridView(byte? languageId, string commandType, int? pageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Page_Language>().GetAll().Where(p => p.PageID == pageId && p.LanguageID == languageId).Single();
                        item.TitleTag = null;
                        item.Heading = null;
                        item.SubHeading = null;
                        item.MetaDescription = null;
                        item.Description = null;
                        item.URLKeywords = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Page_Language_SelectList_PageResult> Pages = new List<Page_Language_SelectList_PageResult>();
                List<SiteSection_SelectResult> SiteSections = new List<SiteSection_SelectResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Page_Language_SelectList(languageId.Value);

                    Pages = Results.GetResult<Page_Language_SelectList_PageResult>().ToList();
                    SiteSections = Results.GetResult<SiteSection_SelectResult>().ToList();
                }

                int PagesToTranslateCount = Pages.Where(p => p.FieldsToReview > 0).Count();

                TranslatePageGridViewModel Model = new TranslatePageGridViewModel(Pages, SiteSections, PagesToTranslateCount, languageId);

                if(pageId != null)
                {
                    ViewBag.SelectedPageID = pageId.Value;
                }

                return PartialView("Pages/_TranslatePageGridView", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult LoadPageTranslator(int PageID, string TargetLanguage, byte TargetLanguageID)
        {
            try
            {
                IMultipleResults Results = UnitOfWork.Context.Page_Language_SelectDetail(PageID, TargetLanguageID);

                TranslatePageDetailViewModel Model = new TranslatePageDetailViewModel(
                    PageID,
                    TargetLanguageID,
                    TargetLanguage,
                    Results.GetResult<Page_Language_SelectDetail_PageResult>().Single(),
                    Results.GetResult<Page_Language_SelectDetail_TranslationResult>().Single(),
                    Results.GetResult<Page_Language_SelectDetail_ReviewRequiredResult>().Single(),
                    Results.GetResult<Page_Language_SelectDetail_ImageResult>().SingleOrDefault()
                );

                return PartialView("Pages/_TranslatePagePartial", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Next_Page(int PageID)
        {
            return Content("");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save_Page(
            int PageID, 
            byte languageId,
            string Heading, 
            string SubHeading,
            string URLKeywords,
            string Description,
            string TitleTag,
            string MetaDescription,
            string TranslatedHeading,
            string TranslatedSubHeading,
            string TranslatedURLKeywords,
            string TranslatedDescription,
            string TranslatedTitleTag,
            string TranslatedMetaDescription)
        {
            try
            {
                UnitOfWork.Context.Page_Language_Save(PageID, languageId,
                    new Page_Language_SelectDetail_PageResult { 
                        Heading = Heading, 
                        SubHeading = SubHeading, 
                        URLKeywords = URLKeywords,
                        TitleTag = TitleTag, 
                        MetaDescription = MetaDescription, 
                        Description = Description
                    }, 
                    new Page_Language_SelectDetail_TranslationResult { 
                        Heading = TranslatedHeading, 
                        SubHeading = TranslatedSubHeading, 
                        URLKeywords = TranslatedURLKeywords,
                        TitleTag = TranslatedTitleTag, 
                        MetaDescription = TranslatedMetaDescription, 
                        Description = TranslatedDescription
                 });

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_Page(PageID, languageId, null, null, null, null, false, false);

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(languageId, PageID, null, null, null, false, false);
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