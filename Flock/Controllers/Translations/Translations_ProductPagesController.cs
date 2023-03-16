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
        public ActionResult ProductPages(string targetLanguage)
        {
            var Model = new TranslateProductPageViewModel();

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

            return View("ProductPages/TranslateProductPage", Model);
        }

        public ActionResult TranslateProductPageTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateProductPageGridView(byte? languageId, string commandType, int? productPageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.ProductPage_Language>().GetAll().Where(p => p.ProductPageID == productPageId && p.LanguageID == languageId).Single();
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

                List<ProductPage_Language_SelectList_ProductPageResult> ProductPages = new List<ProductPage_Language_SelectList_ProductPageResult>();
                List<Brand_SelectResult> Brands = new List<Brand_SelectResult>();
                List<Launch_SelectResult> Launches = new List<Launch_SelectResult>();
                List<ProductType_SelectResult> ProductTypes = new List<ProductType_SelectResult>();
                List<Status_SelectResult> Statuses = new List<Status_SelectResult>();
                List<ProductPage_TemplateSelectResult> Templates = new List<ProductPage_TemplateSelectResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.ProductPage_Language_SelectList(languageId.Value);

                    ProductPages = Results.GetResult<ProductPage_Language_SelectList_ProductPageResult>().ToList();
                    Brands = Results.GetResult<Brand_SelectResult>().ToList();
                    Launches = Results.GetResult<Launch_SelectResult>().ToList();
                    ProductTypes = Results.GetResult<ProductType_SelectResult>().ToList();
                    Statuses = Results.GetResult<Status_SelectResult>().ToList();
                    Templates = Results.GetResult<ProductPage_TemplateSelectResult>().ToList();
                }

                int ProductPagesToTranslateCount = ProductPages.Where(p => p.FieldsToReview > 0).Count();

                TranslateProductPageGridViewModel Model = new TranslateProductPageGridViewModel(ProductPages, Brands, Launches, ProductTypes, Statuses, Templates, ProductPagesToTranslateCount, languageId);

                if(productPageId != null)
                {
                    ViewBag.SelectedProductPageID = productPageId.Value;
                }

                return PartialView("ProductPages/_TranslateProductPageGridView", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult LoadProductPageTranslator(int ProductPageID, string TargetLanguage, byte TargetLanguageID)
        {
            try
            {
                IMultipleResults Results = UnitOfWork.Context.ProductPage_Language_SelectDetail(ProductPageID, TargetLanguageID);

                TranslateProductPageDetailViewModel Model = new TranslateProductPageDetailViewModel(
                    ProductPageID,
                    TargetLanguageID,
                    TargetLanguage,
                    Results.GetResult<ProductPage_Language_SelectDetail_ProductPageResult>().Single(),
                    Results.GetResult<ProductPage_Language_SelectDetail_TranslationResult>().Single(),
                    Results.GetResult<ProductPage_Language_SelectDetail_ReviewRequiredResult>().Single(),
                    Results.GetResult<ProductPage_Language_SelectDetail_ImageResult>().SingleOrDefault()
                );

                return PartialView("ProductPages/_TranslateProductPagePartial", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Next_ProductPage(int ProductPageID)
        {
            return Content("");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save_ProductPage(
            int ProductPageID, 
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
                UnitOfWork.Context.ProductPage_Language_Save(ProductPageID, languageId,
                    new ProductPage_Language_SelectDetail_ProductPageResult { 
                        Heading = Heading, 
                        SubHeading = SubHeading, 
                        Description = Description, 
                        TitleTag = TitleTag, 
                        MetaDescription = MetaDescription
                    }, 
                    new ProductPage_Language_SelectDetail_TranslationResult { 
                        Heading = TranslatedHeading, 
                        SubHeading = TranslatedSubHeading, 
                        Description = TranslatedDescription, 
                        TitleTag = TranslatedTitleTag, 
                        MetaDescription = TranslatedMetaDescription
                    }
                );

                UnitOfWork.Context.ProductPage_Language_CloneTranslations(ProductPageID, languageId);

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(languageId, null, ProductPageID, null, null, false, false);
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