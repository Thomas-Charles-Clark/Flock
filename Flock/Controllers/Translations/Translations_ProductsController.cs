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
        public ActionResult Products(string targetLanguage)
        {
            var Model = new TranslateProductViewModel();

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

            return View("Products/TranslateProduct", Model);
        }

        public ActionResult TranslateProductTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateProductGridView(byte? languageId, string commandType, int? productId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Product_Language>().GetAll().Where(p => p.ProductID == productId && p.LanguageID == languageId).Single();
                        item.BasketName = null;
                        item.OrderFormName = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Product_Language_SelectList_ProductResult> Products = new List<Product_Language_SelectList_ProductResult>();
                List<Launch_SelectResult> Launches = new List<Launch_SelectResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Product_Language_SelectList(languageId.Value);

                    Products = Results.GetResult<Product_Language_SelectList_ProductResult>().ToList();
                    Launches = Results.GetResult<Launch_SelectResult>().ToList();
                }

                TranslateProductGridViewModel Model = new TranslateProductGridViewModel(Products, Launches, languageId);

                return PartialView("Products/_TranslateProductGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateProductGridViewUpdate(Product_Language_SelectList_ProductResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateProductGridViewModel saveModel = new TranslateProductGridViewModel();
                    saveModel.Products.Add(item);
                    UnitOfWork.Context.Product_Language_Save(languageId, saveModel);

                    List<Product_Language_SelectList_ProductResult> Products = new List<Product_Language_SelectList_ProductResult>();
                    List<Launch_SelectResult> Launches = new List<Launch_SelectResult>();

                    IMultipleResults Results = UnitOfWork.Context.Product_Language_SelectList(languageId);

                    Products = Results.GetResult<Product_Language_SelectList_ProductResult>().ToList();
                    Launches = Results.GetResult<Launch_SelectResult>().ToList();
                    
                    TranslateProductGridViewModel Model = new TranslateProductGridViewModel(Products, Launches, languageId);

                    return PartialView("Products/_TranslateProductGridView", Model);
                }
                else
                {
                    List<Product_Language_SelectList_ProductResult> Products = new List<Product_Language_SelectList_ProductResult>();
                    List<Launch_SelectResult> Launches = new List<Launch_SelectResult>();

                    IMultipleResults Results = UnitOfWork.Context.Product_Language_SelectList(languageId);

                    Products = Results.GetResult<Product_Language_SelectList_ProductResult>().ToList();
                    Launches = Results.GetResult<Launch_SelectResult>().ToList();

                    TranslateProductGridViewModel Model = new TranslateProductGridViewModel(Products, Launches, languageId);

                    return PartialView("Products/_TranslateProductGridView", Model);
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