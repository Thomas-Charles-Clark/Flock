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
        public ActionResult PageSections(string targetLanguage)
        {
            var Model = new TranslatePageSectionViewModel();

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
            return View("PageSections/TranslatePageSection", Model);
        }

        public ActionResult TranslatePageSectionTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslatePageSectionGridView(byte? languageId, string commandType, long? pageSectionId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.PageSection_Language>().GetAll().Where(p => p.PageSectionID == pageSectionId && p.LanguageID == languageId).Single();
                        item.Heading = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<PageSection_Language_SelectList_PageSectionResult> PageSections = new List<PageSection_Language_SelectList_PageSectionResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.PageSection_Language_SelectList(languageId.Value);

                    PageSections = Results.GetResult<PageSection_Language_SelectList_PageSectionResult>().ToList();
                }

                TranslatePageSectionGridViewModel Model = new TranslatePageSectionGridViewModel(PageSections, languageId);

                return PartialView("PageSections/_TranslatePageSectionGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslatePageSectionGridViewUpdate(PageSection_Language_SelectList_PageSectionResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslatePageSectionGridViewModel saveModel = new TranslatePageSectionGridViewModel();
                    saveModel.PageSections.Add(item);
                    UnitOfWork.Context.PageSection_Language_Save(languageId, saveModel);

                    List<PageSection_Language_SelectList_PageSectionResult> PageSections = new List<PageSection_Language_SelectList_PageSectionResult>();

                    IMultipleResults Results = UnitOfWork.Context.PageSection_Language_SelectList(languageId);

                    PageSections = Results.GetResult<PageSection_Language_SelectList_PageSectionResult>().ToList();

                    TranslatePageSectionGridViewModel Model = new TranslatePageSectionGridViewModel(PageSections, languageId);

                    return PartialView("PageSections/_TranslatePageSectionGridView", Model);
                }
                else
                {
                    List<PageSection_Language_SelectList_PageSectionResult> PageSections = new List<PageSection_Language_SelectList_PageSectionResult>();

                    IMultipleResults Results = UnitOfWork.Context.PageSection_Language_SelectList(languageId);

                    PageSections = Results.GetResult<PageSection_Language_SelectList_PageSectionResult>().ToList();

                    TranslatePageSectionGridViewModel Model = new TranslatePageSectionGridViewModel(PageSections, languageId);

                    return PartialView("PageSections/_TranslatePageSectionGridView", Model);
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