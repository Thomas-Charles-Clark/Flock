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
        public ActionResult ReturnReasons(string targetLanguage)
        {
            var Model = new TranslateReturnReasonViewModel();

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

            return View("ReturnReasons/TranslateReturnReason", Model);
        }

        public ActionResult TranslateReturnReasonTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateReturnReasonGridView(byte? languageId, string commandType, byte? returnReasonId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.ReturnReason_Language>().GetAll().Where(p => p.ReturnReasonID == returnReasonId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<ReturnReason_Language_SelectList_ReturnReasonResult> ReturnReasons = new List<ReturnReason_Language_SelectList_ReturnReasonResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.ReturnReason_Language_SelectList(languageId.Value);

                    ReturnReasons = Results.GetResult<ReturnReason_Language_SelectList_ReturnReasonResult>().ToList();
                }

                TranslateReturnReasonGridViewModel Model = new TranslateReturnReasonGridViewModel(ReturnReasons, languageId);

                return PartialView("ReturnReasons/_TranslateReturnReasonGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateReturnReasonGridViewUpdate(ReturnReason_Language_SelectList_ReturnReasonResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateReturnReasonGridViewModel();
                    saveModel.ReturnReasons.Add(item);
                    UnitOfWork.Context.ReturnReason_Language_Save(languageId, saveModel);

                    List<ReturnReason_Language_SelectList_ReturnReasonResult> ReturnReasons = new List<ReturnReason_Language_SelectList_ReturnReasonResult>();
                    IMultipleResults Results = UnitOfWork.Context.ReturnReason_Language_SelectList(languageId);
                    ReturnReasons = Results.GetResult<ReturnReason_Language_SelectList_ReturnReasonResult>().ToList();
                    var Model = new TranslateReturnReasonGridViewModel(ReturnReasons, languageId);

                    return PartialView("ReturnReasons/_TranslateReturnReasonGridView", Model);
                }
                else
                {
                    List<ReturnReason_Language_SelectList_ReturnReasonResult> ReturnReasons = new List<ReturnReason_Language_SelectList_ReturnReasonResult>();
                    IMultipleResults Results = UnitOfWork.Context.ReturnReason_Language_SelectList(languageId);
                    ReturnReasons = Results.GetResult<ReturnReason_Language_SelectList_ReturnReasonResult>().ToList();
                    var Model = new TranslateReturnReasonGridViewModel(ReturnReasons, languageId);

                    return PartialView("ReturnReasons/_TranslateReturnReasonGridView", Model);
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