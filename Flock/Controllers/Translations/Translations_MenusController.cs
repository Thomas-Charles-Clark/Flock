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
using Microsoft.AspNet.Identity;

namespace Flock.Controllers
{
    public partial class TranslationsController : Controller
    {
        public ActionResult Menus(string targetLanguage)
        {
            var Model = new TranslateMenuViewModel();

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


            return View("Menus/TranslateMenu", Model);
        }

        public ActionResult TranslateMenuTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateMenuGridView(byte? languageId, string commandType, byte? menuId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Menu_Language>().GetAll().Where(p => p.MenuID == menuId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Menu_Language_SelectList_MenuResult> Menus = new List<Menu_Language_SelectList_MenuResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Menu_Language_SelectList(languageId.Value);

                    Menus = Results.GetResult<Menu_Language_SelectList_MenuResult>().ToList();
                }

                TranslateMenuGridViewModel Model = new TranslateMenuGridViewModel(Menus, languageId);

                return PartialView("Menus/_TranslateMenuGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateMenuGridViewUpdate(Menu_Language_SelectList_MenuResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    TranslateMenuGridViewModel saveModel = new TranslateMenuGridViewModel();
                    saveModel.Menus.Add(item);
                    UnitOfWork.Context.Menu_Language_Save(languageId, saveModel);

                    List<Menu_Language_SelectList_MenuResult> Menus = new List<Menu_Language_SelectList_MenuResult>();

                    IMultipleResults Results = UnitOfWork.Context.Menu_Language_SelectList(languageId);

                    Menus = Results.GetResult<Menu_Language_SelectList_MenuResult>().ToList();

                    TranslateMenuGridViewModel Model = new TranslateMenuGridViewModel(Menus, languageId);

                    return PartialView("Menus/_TranslateMenuGridView", Model);
                }
                else
                {
                    List<Menu_Language_SelectList_MenuResult> Menus = new List<Menu_Language_SelectList_MenuResult>();

                    IMultipleResults Results = UnitOfWork.Context.Menu_Language_SelectList(languageId);

                    Menus = Results.GetResult<Menu_Language_SelectList_MenuResult>().ToList();

                    TranslateMenuGridViewModel Model = new TranslateMenuGridViewModel(Menus, languageId);

                    return PartialView("Menus/_TranslateMenuGridView", Model);
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