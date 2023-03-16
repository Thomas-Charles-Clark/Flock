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
        public ActionResult StockStatuses(string targetLanguage)
        {
            var Model = new TranslateStockStatusViewModel();

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

            return View("StockStatuses/TranslateStockStatus", Model);
        }

        public ActionResult TranslateStockStatusTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateStockStatusGridView(byte? languageId, string commandType, byte? stockStatusId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.StockStatus_Language>().GetAll().Where(p => p.StockStatusID == stockStatusId && p.LanguageID == languageId).Single();
                        item.StockStatus = null;
                        item.ProductFeedStockStatus = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<StockStatus_Language_SelectList_StockStatusResult> StockStatuses = new List<StockStatus_Language_SelectList_StockStatusResult>();

                if (languageId != null)
                {

                    IMultipleResults Results = UnitOfWork.Context.StockStatus_Language_SelectList(languageId.Value);

                    StockStatuses = Results.GetResult<StockStatus_Language_SelectList_StockStatusResult>().ToList();
                }

                TranslateStockStatusGridViewModel Model = new TranslateStockStatusGridViewModel(StockStatuses, languageId);

                return PartialView("StockStatuses/_TranslateStockStatusGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateStockStatusGridViewUpdate(StockStatus_Language_SelectList_StockStatusResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateStockStatusGridViewModel();
                    saveModel.StockStatuses.Add(item);
                    UnitOfWork.Context.StockStatus_Language_Save(languageId, saveModel);

                    List<StockStatus_Language_SelectList_StockStatusResult> StockStatuses = new List<StockStatus_Language_SelectList_StockStatusResult>();
                    IMultipleResults Results = UnitOfWork.Context.StockStatus_Language_SelectList(languageId);
                    StockStatuses = Results.GetResult<StockStatus_Language_SelectList_StockStatusResult>().ToList();
                    var Model = new TranslateStockStatusGridViewModel(StockStatuses, languageId);

                    return PartialView("StockStatuses/_TranslateStockStatusGridView", Model);
                }
                else
                {
                    List<StockStatus_Language_SelectList_StockStatusResult> StockStatuses = new List<StockStatus_Language_SelectList_StockStatusResult>();
                    IMultipleResults Results = UnitOfWork.Context.StockStatus_Language_SelectList(languageId);
                    StockStatuses = Results.GetResult<StockStatus_Language_SelectList_StockStatusResult>().ToList();
                    var Model = new TranslateStockStatusGridViewModel(StockStatuses, languageId);

                    return PartialView("StockStatuses/_TranslateStockStatusGridView", Model);
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