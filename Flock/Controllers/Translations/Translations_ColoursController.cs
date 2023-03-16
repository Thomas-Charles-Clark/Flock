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
        public ActionResult Colours(string targetLanguage)
        {
            var Model = new TranslateColourViewModel();

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

            return View("Colours/TranslateColour", Model);
        }

        public ActionResult TranslateColourTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateColourGridView(byte? languageId, string commandType, byte? colourId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Colour_Language>().GetAll().Where(p => p.ColourID == colourId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Colour_Language_SelectList_ColourResult> Colours = new List<Colour_Language_SelectList_ColourResult>();

                if (languageId != null)
                {
                    //UnitOfWork.Context.Colour_Language_GoogleTranslate(null, targetLanguageID);

                    IMultipleResults Results = UnitOfWork.Context.Colour_Language_SelectList(languageId.Value);

                    Colours = Results.GetResult<Colour_Language_SelectList_ColourResult>().ToList();
                }

                TranslateColourGridViewModel Model = new TranslateColourGridViewModel(Colours, languageId);

                return PartialView("Colours/_TranslateColourGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateColourGridViewUpdate(Colour_Language_SelectList_ColourResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateColourGridViewModel saveModel = new TranslateColourGridViewModel();
                    saveModel.Colours.Add(item);
                    UnitOfWork.Context.Colour_Language_Save(languageId, saveModel);

                    List<Colour_Language_SelectList_ColourResult> Colours = new List<Colour_Language_SelectList_ColourResult>();

                    IMultipleResults Results = UnitOfWork.Context.Colour_Language_SelectList(languageId);

                    Colours = Results.GetResult<Colour_Language_SelectList_ColourResult>().ToList();

                    TranslateColourGridViewModel Model = new TranslateColourGridViewModel(Colours, languageId);

                    return PartialView("Colours/_TranslateColourGridView", Model);
                }
                else
                {
                    List<Colour_Language_SelectList_ColourResult> Colours = new List<Colour_Language_SelectList_ColourResult>();

                    IMultipleResults Results = UnitOfWork.Context.Colour_Language_SelectList(languageId);

                    Colours = Results.GetResult<Colour_Language_SelectList_ColourResult>().ToList();

                    TranslateColourGridViewModel Model = new TranslateColourGridViewModel(Colours, languageId);

                    return PartialView("Colours/_TranslateColourGridView", Model);
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