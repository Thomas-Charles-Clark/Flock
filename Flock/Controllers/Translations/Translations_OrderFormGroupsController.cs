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
        public ActionResult OrderFormGroups(string targetLanguage)
        {
            var Model = new TranslateOrderFormGroupViewModel();

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

            return View("OrderFormGroups/TranslateOrderFormGroup", Model);
        }

        public ActionResult TranslateOrderFormGroupTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateOrderFormGroupGridView(byte? languageId, string commandType, short? orderFormGroupId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.OrderFormGroup_Language>().GetAll().Where(p => p.OrderFormGroupID == orderFormGroupId && p.LanguageID == languageId).Single();
                        item.OrderFormGroup = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<OrderFormGroup_Language_SelectList_OrderFormGroupResult> OrderFormGroups = new List<OrderFormGroup_Language_SelectList_OrderFormGroupResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.OrderFormGroup_Language_SelectList(languageId.Value);

                    OrderFormGroups = Results.GetResult<OrderFormGroup_Language_SelectList_OrderFormGroupResult>().ToList();
                }

                TranslateOrderFormGroupGridViewModel Model = new TranslateOrderFormGroupGridViewModel(OrderFormGroups, languageId);

                return PartialView("OrderFormGroups/_TranslateOrderFormGroupGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateOrderFormGroupGridViewUpdate(OrderFormGroup_Language_SelectList_OrderFormGroupResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateOrderFormGroupGridViewModel();
                    saveModel.OrderFormGroups.Add(item);
                    UnitOfWork.Context.OrderFormGroup_Language_Save(languageId, saveModel);

                    List<OrderFormGroup_Language_SelectList_OrderFormGroupResult> OrderFormGroups = new List<OrderFormGroup_Language_SelectList_OrderFormGroupResult>();
                    IMultipleResults Results = UnitOfWork.Context.OrderFormGroup_Language_SelectList(languageId);
                    OrderFormGroups = Results.GetResult<OrderFormGroup_Language_SelectList_OrderFormGroupResult>().ToList();
                    var Model = new TranslateOrderFormGroupGridViewModel(OrderFormGroups, languageId);

                    return PartialView("OrderFormGroups/_TranslateOrderFormGroupGridView", Model);
                }
                else
                {
                    List<OrderFormGroup_Language_SelectList_OrderFormGroupResult> OrderFormGroups = new List<OrderFormGroup_Language_SelectList_OrderFormGroupResult>();
                    IMultipleResults Results = UnitOfWork.Context.OrderFormGroup_Language_SelectList(languageId);
                    OrderFormGroups = Results.GetResult<OrderFormGroup_Language_SelectList_OrderFormGroupResult>().ToList();
                    var Model = new TranslateOrderFormGroupGridViewModel(OrderFormGroups, languageId);

                    return PartialView("OrderFormGroups/_TranslateOrderFormGroupGridView", Model);
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