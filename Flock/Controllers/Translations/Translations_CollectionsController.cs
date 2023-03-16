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
        public ActionResult Collections(string targetLanguage)
        {
            var Model = new TranslateCollectionViewModel();

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

            return View("Collections/TranslateCollection", Model);
        }

        public ActionResult TranslateCollectionTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateCollectionGridView(byte? languageId, short? collectionId, string commandType)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Collection_Language>().GetAll().Where(p => p.CollectionID == collectionId && p.LanguageID == languageId).Single();
                        item.TitleTag = null;
                        item.Heading = null;
                        item.SubHeading = null;
                        item.MetaDescription = null;
                        item.URLKeywords = null;
                        item.Description = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                //var targetLanguageID = DevExpress.Web.Mvc.ListBoxExtension.GetValue<byte>("TranslationTargetLanguageListBox");
                List<Collection_Language_SelectList_CollectionResult> Collections = new List<Collection_Language_SelectList_CollectionResult>();
                List<Brand_SelectResult> Brands = new List<Brand_SelectResult>();
                List<Launch_SelectResult> Launches = new List<Launch_SelectResult>();
                List<ProductType_SelectResult> ProductTypes = new List<ProductType_SelectResult>();
                List<Status_SelectResult> Statuses = new List<Status_SelectResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Collection_Language_SelectList(languageId.Value);

                    Collections = Results.GetResult<Collection_Language_SelectList_CollectionResult>().ToList();
                    Brands = Results.GetResult<Brand_SelectResult>().ToList();
                    Launches = Results.GetResult<Launch_SelectResult>().ToList();
                    ProductTypes = Results.GetResult<ProductType_SelectResult>().ToList();
                    Statuses = Results.GetResult<Status_SelectResult>().ToList();
                }

                int collectionsToTranslateCount = Collections.Where(p => p.FieldsToReview > 0).Count();

                TranslateCollectionGridViewModel Model = new TranslateCollectionGridViewModel(Collections, Brands, Launches, ProductTypes, Statuses, collectionsToTranslateCount, languageId);

                if(collectionId != null)
                {
                    ViewBag.SelectedCollectionID = collectionId.Value;
                }

                return PartialView("Collections/_TranslateCollectionGridView", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult LoadCollectionTranslator(short CollectionID, string TargetLanguage, byte TargetLanguageID)
        {
            try
            {
                IMultipleResults Results = UnitOfWork.Context.Collection_Language_SelectDetail(CollectionID, TargetLanguageID);

                TranslateCollectionDetailViewModel Model = new TranslateCollectionDetailViewModel(
                    CollectionID,
                    TargetLanguageID,
                    TargetLanguage,
                    Results.GetResult<Collection_Language_SelectDetail_CollectionResult>().Single(),
                    Results.GetResult<Collection_Language_SelectDetail_TranslationResult>().Single(),
                    Results.GetResult<Collection_Language_SelectDetail_ReviewRequiredResult>().Single(),
                    Results.GetResult<Collection_Language_SelectDetail_ImageResult>().SingleOrDefault()
                );

                return PartialView("Collections/_TranslateCollectionPartial", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Next_Collection(short CollectionID)
        {
            return Content("");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save_Collection(
            short CollectionID, 
            byte languageId,
            string Heading, 
            string SubHeading,
            string Description,
            string TitleTag,
            string MetaDescription,
            string TranslatedHeading,
            string TranslatedSubHeading,
            string TranslatedDescription,
            string TranslatedTitleTag,
            string TranslatedMetaDescription)
        {
            try
            {
                UnitOfWork.Context.Collection_Language_Save(CollectionID, languageId,
                    new Collection_Language_SelectDetail_CollectionResult { 
                        Heading = Heading, 
                        SubHeading = SubHeading, 
                        Description = Description, 
                        TitleTag = TitleTag, 
                        MetaDescription = MetaDescription, 
                    },
                    new Collection_Language_SelectDetail_TranslationResult { 
                        Heading = TranslatedHeading,
                        SubHeading = TranslatedSubHeading, 
                        Description = TranslatedDescription, 
                        TitleTag = TranslatedTitleTag, 
                        MetaDescription = TranslatedMetaDescription, 
                    }
                );

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(languageId, null, null, CollectionID, null, false, false);
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