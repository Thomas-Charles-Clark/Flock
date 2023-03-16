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
        public ActionResult AttributeGroups(string targetLanguage)
        {
            var Model = new TranslateAttributeGroupViewModel();

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

            return View("AttributeGroups/TranslateAttributeGroup", Model);
        }

        public ActionResult TranslateAttributeGroupTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateAttributeGroupGridView(string commandType, byte? attributeGroupId, byte? languageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var attributeGroup = UnitOfWork.GetRepository<Models.AttributeGroup_Language>().GetAll().Where(p => p.AttributeGroupID == attributeGroupId && p.LanguageID == languageId).Single();
                        attributeGroup.Heading = null;
                        attributeGroup.Stage = null;
                        attributeGroup.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }
                List<AttributeGroup_Language_SelectList_AttributeGroupResult> AttributeGroups = new List<AttributeGroup_Language_SelectList_AttributeGroupResult>();

                if (languageId != null)
                {
                    //UnitOfWork.Context.AttributeGroup_Language_GoogleTranslate(null, targetLanguageID);

                    IMultipleResults Results = UnitOfWork.Context.AttributeGroup_Language_SelectList(languageId.Value);

                    AttributeGroups = Results.GetResult<AttributeGroup_Language_SelectList_AttributeGroupResult>().ToList();
                }

                TranslateAttributeGroupGridViewModel Model = new TranslateAttributeGroupGridViewModel(AttributeGroups, languageId);

                return PartialView("AttributeGroups/_TranslateAttributeGroupGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateAttributeGroupGridViewUpdate(AttributeGroup_Language_SelectList_AttributeGroupResult item, byte languageId)
        {
            try
            {   
                if(ModelState.IsValid)
                {
                    var saveModel = new TranslateAttributeGroupGridViewModel();
                    saveModel.AttributeGroups.Add(item);
                    UnitOfWork.Context.AttributeGroup_Language_Save(languageId, saveModel);

                    List<AttributeGroup_Language_SelectList_AttributeGroupResult> AttributeGroups = new List<AttributeGroup_Language_SelectList_AttributeGroupResult>();
                    IMultipleResults Results = UnitOfWork.Context.AttributeGroup_Language_SelectList(languageId);
                    AttributeGroups = Results.GetResult<AttributeGroup_Language_SelectList_AttributeGroupResult>().ToList();
                    var Model = new TranslateAttributeGroupGridViewModel(AttributeGroups, languageId);

                    return PartialView("AttributeGroups/_TranslateAttributeGroupGridView", Model);
                }
                else
                {
                    List<AttributeGroup_Language_SelectList_AttributeGroupResult> AttributeGroups = new List<AttributeGroup_Language_SelectList_AttributeGroupResult>();
                    IMultipleResults Results = UnitOfWork.Context.AttributeGroup_Language_SelectList(languageId);
                    AttributeGroups = Results.GetResult<AttributeGroup_Language_SelectList_AttributeGroupResult>().ToList();
                    var Model = new TranslateAttributeGroupGridViewModel(AttributeGroups, languageId);

                    return PartialView("AttributeGroups/_TranslateAttributeGroupGridView", Model);
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }
    }
}