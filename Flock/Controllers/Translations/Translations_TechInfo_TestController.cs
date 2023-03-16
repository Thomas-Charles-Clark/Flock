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
        public ActionResult TechInfo_Test(string targetLanguage)
        {
            var Model = new TranslateTechInfo_TestViewModel();

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
            return View("TechInfo_Test/TranslateTechInfo_Test", Model);
        }

        public ActionResult TranslateTestTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateTestGridView(byte? languageId, string commandType, short? testId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.TechInfo_Ref_Test_Language>().GetAll().Where(p => p.TestID == testId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<TechInfo_Test_Language_SelectList_TechInfo_TestResult> TechInfo_Tests = new List<TechInfo_Test_Language_SelectList_TechInfo_TestResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Test_Language_SelectList(languageId.Value);

                    TechInfo_Tests = Results.GetResult<TechInfo_Test_Language_SelectList_TechInfo_TestResult>().ToList();
                }

                TranslateTechInfo_TestGridViewModel Model = new TranslateTechInfo_TestGridViewModel(TechInfo_Tests, languageId);

                return PartialView("TechInfo_Test/_TranslateTechInfo_TestGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateTestGridViewUpdate(TechInfo_Test_Language_SelectList_TechInfo_TestResult item, byte languageId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateTechInfo_TestGridViewModel saveModel = new TranslateTechInfo_TestGridViewModel();
                    saveModel.TechInfo_Tests.Add(item);
                    UnitOfWork.Context.TechInfo_Test_Language_Save(languageId, saveModel);

                    List<TechInfo_Test_Language_SelectList_TechInfo_TestResult> TechInfo_Tests = new List<TechInfo_Test_Language_SelectList_TechInfo_TestResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Test_Language_SelectList(languageId);

                    TechInfo_Tests = Results.GetResult<TechInfo_Test_Language_SelectList_TechInfo_TestResult>().ToList();

                    TranslateTechInfo_TestGridViewModel Model = new TranslateTechInfo_TestGridViewModel(TechInfo_Tests, languageId);

                    return PartialView("TechInfo_Test/_TranslateTechInfo_TestGridView", Model);
                }
                else
                {
                    List<TechInfo_Test_Language_SelectList_TechInfo_TestResult> TechInfo_Tests = new List<TechInfo_Test_Language_SelectList_TechInfo_TestResult>();

                    IMultipleResults Results = UnitOfWork.Context.TechInfo_Test_Language_SelectList(languageId);

                    TechInfo_Tests = Results.GetResult<TechInfo_Test_Language_SelectList_TechInfo_TestResult>().ToList();

                    TranslateTechInfo_TestGridViewModel Model = new TranslateTechInfo_TestGridViewModel(TechInfo_Tests, languageId);

                    return PartialView("TechInfo_Test/_TranslateTechInfo_TestGridView", Model);
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