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
        public ActionResult HtmlSections()
        {
            var Model = new TranslateHtmlSectionViewModel();

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
            //        var pp = UnitOfWork.GetRepository<HTMLSection>().GetAll().Where(p => p.HtmlSectionID == id.Value && p.DeletedDT == null).SingleOrDefault();

            //        if (pp == null)
            //        {
            //            ModelState.AddModelError("HtmlSectionID", "Invalid HtmlSectionID, or that HtmlSection has been deleted. Deleted HtmlSections can be retrieved.");
            //        }
            //        else
            //        {
            //            Model.HtmlSectionID = pp.HtmlSectionID;
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        ModelState.AddModelError("HtmlSectionID", "Invalid HtmlSectionID, or that HtmlSection has been deleted. Deleted HtmlSections can be retrieved.");
            //    }
            //}
            //}

            return View("HtmlSections/TranslateHtmlSection", Model);
        }

        public ActionResult TranslateHtmlSectionTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateHtmlSectionGridView(byte? languageId, string commandType, long? htmlSectionId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.HTMLSection_Language>().GetAll().Where(p => p.HTMLSectionID == htmlSectionId && p.LanguageID == languageId).Single();
                        item.Contents = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<HtmlSection_Language_SelectList_HtmlSectionResult> HtmlSections = new List<HtmlSection_Language_SelectList_HtmlSectionResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.HtmlSection_Language_SelectList(languageId.Value);

                    HtmlSections = Results.GetResult<HtmlSection_Language_SelectList_HtmlSectionResult>().ToList();
                }

                int HtmlSectionsToTranslateCount = HtmlSections.Where(p => p.FieldsToReview > 0).Count();

                TranslateHtmlSectionGridViewModel Model = new TranslateHtmlSectionGridViewModel(HtmlSections, UnitOfWork.GetRepository<Status>().GetAll().ToList(), HtmlSectionsToTranslateCount, languageId);

                if (htmlSectionId != null)
                {
                    ViewBag.SelectedHtmlSectionID = htmlSectionId.Value;
                }

                return PartialView("HtmlSections/_TranslateHtmlSectionGridView", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult LoadHtmlSectionTranslator(int HTMLSectionID, string TargetLanguage, byte TargetLanguageID)
        {
            try
            {
                IMultipleResults Results = UnitOfWork.Context.HtmlSection_Language_SelectDetail(HTMLSectionID, TargetLanguageID);

                TranslateHtmlSectionDetailViewModel Model = new TranslateHtmlSectionDetailViewModel(
                    HTMLSectionID,
                    TargetLanguageID,
                    TargetLanguage,
                    Results.GetResult<HtmlSection_Language_SelectDetail_HtmlSectionResult>().Single(),
                    Results.GetResult<HtmlSection_Language_SelectDetail_TranslationResult>().Single(),
                    Results.GetResult<HtmlSection_Language_SelectDetail_ReviewRequiredResult>().Single()
                );

                return PartialView("HtmlSections/_TranslateHtmlSectionPartial", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Next_HtmlSection(int HTMLSectionID)
        {
            return Content("");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save_HtmlSection(
            int HTMLSectionID, 
            byte languageId,
            string OriginalContents,
            string TranslatedContents)
        {
            try
            {
                UnitOfWork.Context.HtmlSection_Language_Save(languageId, HTMLSectionID, OriginalContents, TranslatedContents);
                UnitOfWork.Context.ReplaceLocalURLPlaceholders_HTMLSection(null, HTMLSectionID, languageId, null, null, null, null, false, false);
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