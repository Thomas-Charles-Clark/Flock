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
        public ActionResult ProductTypes(string targetLanguage)
        {
            var Model = new TranslateProductTypeViewModel();

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

            return View("ProductTypes/TranslateProductType", Model);
        }

        public ActionResult TranslateProductTypeTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateProductTypeGridView(byte? languageId, string commandType, byte? productTypeId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.ProductType_Language>().GetAll().Where(p => p.ProductTypeID == productTypeId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.SearchKeywords = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<ProductType_Language_SelectList_ProductTypeResult> ProductTypes = new List<ProductType_Language_SelectList_ProductTypeResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.ProductType_Language_SelectList(languageId.Value);

                    ProductTypes = Results.GetResult<ProductType_Language_SelectList_ProductTypeResult>().ToList();
                }

                TranslateProductTypeGridViewModel Model = new TranslateProductTypeGridViewModel(ProductTypes, languageId);

                return PartialView("ProductTypes/_TranslateProductTypeGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateProductTypeGridViewUpdate(ProductType_Language_SelectList_ProductTypeResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateProductTypeGridViewModel saveModel = new TranslateProductTypeGridViewModel();
                    saveModel.ProductTypes.Add(item);
                    UnitOfWork.Context.ProductType_Language_Save(languageId, saveModel);

                    List<ProductType_Language_SelectList_ProductTypeResult> ProductTypes = new List<ProductType_Language_SelectList_ProductTypeResult>();

                    IMultipleResults Results = UnitOfWork.Context.ProductType_Language_SelectList(languageId);

                    ProductTypes = Results.GetResult<ProductType_Language_SelectList_ProductTypeResult>().ToList();

                    TranslateProductTypeGridViewModel Model = new TranslateProductTypeGridViewModel(ProductTypes, languageId);

                    return PartialView("ProductTypes/_TranslateProductTypeGridView", Model);
                }
                else
                {
                    List<ProductType_Language_SelectList_ProductTypeResult> ProductTypes = new List<ProductType_Language_SelectList_ProductTypeResult>();

                    IMultipleResults Results = UnitOfWork.Context.ProductType_Language_SelectList(languageId);

                    ProductTypes = Results.GetResult<ProductType_Language_SelectList_ProductTypeResult>().ToList();

                    TranslateProductTypeGridViewModel Model = new TranslateProductTypeGridViewModel(ProductTypes, languageId);

                    return PartialView("ProductTypes/_TranslateProductTypeGridView", Model);
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