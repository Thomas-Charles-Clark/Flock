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
        public ActionResult OrderStatuses(string targetLanguage)
        {
            var Model = new TranslateOrderStatusViewModel();

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

            return View("OrderStatuses/TranslateOrderStatus", Model);
        }

        public ActionResult TranslateOrderStatusTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateOrderStatusGridView(byte? languageId, string commandType, byte? orderStatusId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.OrderStatus_Language>().GetAll().Where(p => p.OrderStatusID == orderStatusId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<OrderStatus_Language_SelectList_OrderStatusResult> OrderStatuses = new List<OrderStatus_Language_SelectList_OrderStatusResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.OrderStatus_Language_SelectList(languageId.Value);

                    OrderStatuses = Results.GetResult<OrderStatus_Language_SelectList_OrderStatusResult>().ToList();
                }

                TranslateOrderStatusGridViewModel Model = new TranslateOrderStatusGridViewModel(OrderStatuses, languageId);

                return PartialView("OrderStatuses/_TranslateOrderStatusGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateOrderStatusGridViewUpdate(OrderStatus_Language_SelectList_OrderStatusResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateOrderStatusGridViewModel();
                    saveModel.OrderStatuses.Add(item);
                    UnitOfWork.Context.OrderStatus_Language_Save(languageId, saveModel);

                    List<OrderStatus_Language_SelectList_OrderStatusResult> OrderStatuses = new List<OrderStatus_Language_SelectList_OrderStatusResult>();
                    IMultipleResults Results = UnitOfWork.Context.OrderStatus_Language_SelectList(languageId);
                    OrderStatuses = Results.GetResult<OrderStatus_Language_SelectList_OrderStatusResult>().ToList();
                    var Model = new TranslateOrderStatusGridViewModel(OrderStatuses, languageId);

                    return PartialView("OrderStatuses/_TranslateOrderStatusGridView", Model);
                }
                else
                {
                    List<OrderStatus_Language_SelectList_OrderStatusResult> Attributes = new List<OrderStatus_Language_SelectList_OrderStatusResult>();
                    IMultipleResults Results = UnitOfWork.Context.Attribute_Language_SelectList(languageId);
                    Attributes = Results.GetResult<OrderStatus_Language_SelectList_OrderStatusResult>().ToList();
                    var Model = new TranslateOrderStatusGridViewModel(Attributes, languageId);

                    return PartialView("OrderStatuses/_TranslateOrderStatusGridView", Model);
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