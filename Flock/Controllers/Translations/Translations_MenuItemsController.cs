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
        public ActionResult MenuItems(string targetLanguage)
        {
            var Model = new TranslateMenuItemViewModel();

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
            return View("MenuItems/TranslateMenuItem", Model);
        }

        public ActionResult TranslateMenuItemTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateMenuItemGridView(byte? languageId, string commandType, short? menuItemId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.MenuItem_Language>().GetAll().Where(p => p.MenuItemID == menuItemId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.ExternalURL = null;
                        item.QueryStringParameters = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<MenuItem_Language_SelectList_MenuItemResult> MenuItems = new List<MenuItem_Language_SelectList_MenuItemResult>();
                List<Menu_SelectResult> Menus = new List<Menu_SelectResult>();
                
                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.MenuItem_Language_SelectList(languageId.Value);

                    MenuItems = Results.GetResult<MenuItem_Language_SelectList_MenuItemResult>().ToList();
                    Menus = Results.GetResult<Menu_SelectResult>().ToList();
                }

                TranslateMenuItemGridViewModel Model = new TranslateMenuItemGridViewModel(MenuItems, Menus, languageId);

                return PartialView("MenuItems/_TranslateMenuItemGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateMenuItemGridViewUpdate(MenuItem_Language_SelectList_MenuItemResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TranslateMenuItemGridViewModel saveModel = new TranslateMenuItemGridViewModel();
                    saveModel.MenuItems.Add(item);
                    UnitOfWork.Context.MenuItem_Language_Save(languageId, saveModel);

                    List<MenuItem_Language_SelectList_MenuItemResult> MenuItems = new List<MenuItem_Language_SelectList_MenuItemResult>();
                    List<Menu_SelectResult> Menus = new List<Menu_SelectResult>();

                    IMultipleResults Results = UnitOfWork.Context.MenuItem_Language_SelectList(languageId);

                    MenuItems = Results.GetResult<MenuItem_Language_SelectList_MenuItemResult>().ToList();
                    Menus = Results.GetResult<Menu_SelectResult>().ToList();

                    TranslateMenuItemGridViewModel Model = new TranslateMenuItemGridViewModel(MenuItems, Menus, languageId);

                    return PartialView("MenuItems/_TranslateMenuItemGridView", Model);
                }
                else
                {
                    List<MenuItem_Language_SelectList_MenuItemResult> MenuItems = new List<MenuItem_Language_SelectList_MenuItemResult>();
                    List<Menu_SelectResult> Menus = new List<Menu_SelectResult>();

                    IMultipleResults Results = UnitOfWork.Context.MenuItem_Language_SelectList(languageId);

                    MenuItems = Results.GetResult<MenuItem_Language_SelectList_MenuItemResult>().ToList();
                    Menus = Results.GetResult<Menu_SelectResult>().ToList();

                    TranslateMenuItemGridViewModel Model = new TranslateMenuItemGridViewModel(MenuItems, Menus, languageId);

                    return PartialView("MenuItems/_TranslateMenuItemGridView", Model);
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