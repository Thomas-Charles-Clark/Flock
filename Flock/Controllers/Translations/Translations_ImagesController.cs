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
        public ActionResult Images(string targetLanguage)
        {
            var Model = new TranslateImageViewModel();

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
            return View("Images/TranslateImage", Model);
        }

        public ActionResult TranslateImageTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateImageGridView(byte? languageId, string commandType, long? imageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Image_Language>().GetAll().Where(p => p.ImageID == imageId && p.LanguageID == languageId).Single();
                        item.AltText = null;
                        item.TitleText = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Image_Language_SelectList_ImageResult> Images = new List<Image_Language_SelectList_ImageResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Image_Language_SelectList(languageId.Value);

                    Images = Results.GetResult<Image_Language_SelectList_ImageResult>().ToList();
                }

                TranslateImageGridViewModel Model = new TranslateImageGridViewModel(Images, languageId);

                return PartialView("Images/_TranslateImageGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateImageGridViewUpdate(Image_Language_SelectList_ImageResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateImageGridViewModel saveModel = new TranslateImageGridViewModel();
                    saveModel.Images.Add(item);
                    UnitOfWork.Context.Image_Language_Save(languageId, saveModel);

                    List<Image_Language_SelectList_ImageResult> Images = new List<Image_Language_SelectList_ImageResult>();

                    IMultipleResults Results = UnitOfWork.Context.Image_Language_SelectList(languageId);

                    Images = Results.GetResult<Image_Language_SelectList_ImageResult>().ToList();

                    TranslateImageGridViewModel Model = new TranslateImageGridViewModel(Images, languageId);

                    return PartialView("Images/_TranslateImageGridView", Model);
                }
                else
                {
                    List<Image_Language_SelectList_ImageResult> Images = new List<Image_Language_SelectList_ImageResult>();

                    IMultipleResults Results = UnitOfWork.Context.Image_Language_SelectList(languageId);

                    Images = Results.GetResult<Image_Language_SelectList_ImageResult>().ToList();

                    TranslateImageGridViewModel Model = new TranslateImageGridViewModel(Images, languageId);

                    return PartialView("Images/_TranslateImageGridView", Model);
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