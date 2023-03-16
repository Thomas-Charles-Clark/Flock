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
        public ActionResult SlideShowImages(string targetLanguage)
        {
            var Model = new TranslateSlideShowImageViewModel();

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

            return View("SlideShowImages/TranslateSlideShowImage", Model);
        }

        public ActionResult TranslateSlideShowImageTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateSlideShowImageGridView(byte? languageId, string commandType, long? slideShowId, long? imageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.SlideShow_Image_Language>().GetAll().Where(p => p.SlideShowID == slideShowId && p.ImageID == imageId && p.LanguageID == languageId).Single();
                        item.OverlayHeading = null;
                        item.OverlayText = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<SlideShowImage_Language_SelectList_SlideShowImageResult> SlideShowImagees = new List<SlideShowImage_Language_SelectList_SlideShowImageResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.SlideShowImage_Language_SelectList(languageId.Value);

                    SlideShowImagees = Results.GetResult<SlideShowImage_Language_SelectList_SlideShowImageResult>().ToList();
                }

                TranslateSlideShowImageGridViewModel Model = new TranslateSlideShowImageGridViewModel(SlideShowImagees, languageId);

                return PartialView("SlideShowImages/_TranslateSlideShowImageGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateSlideShowImageGridViewUpdate(SlideShowImage_Language_SelectList_SlideShowImageResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateSlideShowImageGridViewModel();
                    saveModel.SlideShowImages.Add(item);
                    UnitOfWork.Context.SlideShowImage_Language_Save(languageId, saveModel);

                    List<SlideShowImage_Language_SelectList_SlideShowImageResult> SlideShowImagees = new List<SlideShowImage_Language_SelectList_SlideShowImageResult>();
                    IMultipleResults Results = UnitOfWork.Context.SlideShowImage_Language_SelectList(languageId);
                    SlideShowImagees = Results.GetResult<SlideShowImage_Language_SelectList_SlideShowImageResult>().ToList();
                    var Model = new TranslateSlideShowImageGridViewModel(SlideShowImagees, languageId);

                    return PartialView("SlideShowImages/_TranslateSlideShowImageGridView", Model);
                }
                else
                {
                    List<SlideShowImage_Language_SelectList_SlideShowImageResult> Attributes = new List<SlideShowImage_Language_SelectList_SlideShowImageResult>();
                    IMultipleResults Results = UnitOfWork.Context.Attribute_Language_SelectList(languageId);
                    Attributes = Results.GetResult<SlideShowImage_Language_SelectList_SlideShowImageResult>().ToList();
                    var Model = new TranslateSlideShowImageGridViewModel(Attributes, languageId);

                    return PartialView("SlideShowImages/_TranslateSlideShowImageGridView", Model);
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