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
        public ActionResult Attributes(string targetLanguage)
        {
            var Model = new TranslateAttributeViewModel();

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

            return View("Attributes/TranslateAttribute", Model);
        }

        public ActionResult TranslateAttributeTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;
            
            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateAttributeGridView(string commandType, short? attributeId, byte? languageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var announcement = UnitOfWork.GetRepository<Models.Attribute_Language>().GetAll().Where(p => p.AttributeID == attributeId && p.LanguageID == languageId).Single();
                        announcement.Name = null;
                        announcement.Stage = null;
                        announcement.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Attribute_Language_SelectList_AttributeResult> Attributes = new List<Attribute_Language_SelectList_AttributeResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Attribute_Language_SelectList(languageId.Value);

                    Attributes = Results.GetResult<Attribute_Language_SelectList_AttributeResult>().ToList();
                }

                TranslateAttributeGridViewModel Model = new TranslateAttributeGridViewModel(Attributes, languageId);

                return PartialView("Attributes/_TranslateAttributeGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateAttributeGridViewUpdate(Attribute_Language_SelectList_AttributeResult item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateAttributeGridViewModel();
                    saveModel.Attributes.Add(item);
                    UnitOfWork.Context.Attribute_Language_Save(item.LanguageID, saveModel);

                    List<Attribute_Language_SelectList_AttributeResult> Attributes = new List<Attribute_Language_SelectList_AttributeResult>();
                    IMultipleResults Results = UnitOfWork.Context.Attribute_Language_SelectList(item.LanguageID);
                    Attributes = Results.GetResult<Attribute_Language_SelectList_AttributeResult>().ToList();
                    var Model = new TranslateAttributeGridViewModel(Attributes, item.LanguageID);

                    return PartialView("Attributes/_TranslateAttributeGridView", Model);
                }
                else
                {
                    List<Attribute_Language_SelectList_AttributeResult> Attributes = new List<Attribute_Language_SelectList_AttributeResult>();
                    IMultipleResults Results = UnitOfWork.Context.Attribute_Language_SelectList(item.LanguageID);
                    Attributes = Results.GetResult<Attribute_Language_SelectList_AttributeResult>().ToList();
                    var Model = new TranslateAttributeGridViewModel(Attributes, item.LanguageID);

                    return PartialView("Attributes/_TranslateAttributeGridView", Model);
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