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
        public ActionResult SiteSections(string targetLanguage)
        {
            var Model = new TranslateSiteSectionViewModel();

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

            return View("SiteSections/TranslateSiteSection", Model);
        }

        public ActionResult TranslateSiteSectionTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateSiteSectionGridView(byte? languageId, string commandType, byte? siteSectionId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.SiteSection_Language>().GetAll().Where(p => p.SiteSectionID == siteSectionId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<SiteSection_Language_SelectList_SiteSectionResult> SiteSectiones = new List<SiteSection_Language_SelectList_SiteSectionResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.SiteSection_Language_SelectList(languageId.Value);

                    SiteSectiones = Results.GetResult<SiteSection_Language_SelectList_SiteSectionResult>().ToList();
                }

                TranslateSiteSectionGridViewModel Model = new TranslateSiteSectionGridViewModel(SiteSectiones, languageId);

                return PartialView("SiteSections/_TranslateSiteSectionGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateSiteSectionGridViewUpdate(SiteSection_Language_SelectList_SiteSectionResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateSiteSectionGridViewModel();
                    saveModel.SiteSections.Add(item);
                    UnitOfWork.Context.SiteSection_Language_Save(languageId, saveModel);

                    List<SiteSection_Language_SelectList_SiteSectionResult> SiteSectiones = new List<SiteSection_Language_SelectList_SiteSectionResult>();
                    IMultipleResults Results = UnitOfWork.Context.SiteSection_Language_SelectList(languageId);
                    SiteSectiones = Results.GetResult<SiteSection_Language_SelectList_SiteSectionResult>().ToList();
                    var Model = new TranslateSiteSectionGridViewModel(SiteSectiones, languageId);

                    return PartialView("SiteSections/_TranslateSiteSectionGridView", Model);
                }
                else
                {
                    List<SiteSection_Language_SelectList_SiteSectionResult> Attributes = new List<SiteSection_Language_SelectList_SiteSectionResult>();
                    IMultipleResults Results = UnitOfWork.Context.Attribute_Language_SelectList(languageId);
                    Attributes = Results.GetResult<SiteSection_Language_SelectList_SiteSectionResult>().ToList();
                    var Model = new TranslateSiteSectionGridViewModel(Attributes, languageId);

                    return PartialView("SiteSections/_TranslateSiteSectionGridView", Model);
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