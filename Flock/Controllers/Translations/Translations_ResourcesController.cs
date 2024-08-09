using DevExpress.Web.Mvc;
using DocumentFormat.OpenXml.EMMA;

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
        public ActionResult Resources(string targetLanguage)
        {
            var Model = new TranslateResourceViewModel();

            targetLanguage = null;
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
                        Model.TargetCountryLanguageID = lang.LanguageID;
                    }
                }
            }
            return View("Resources/TranslateResource", Model);
        }

        public ActionResult TranslateResourceTargetLanguageDropDown(byte? targetCountryLanguageId)
        {
            var Model = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => ((p.OffDate == null) || (p.OffDate > DateTime.Now)) && p.Country_Language_ID != 3).ToList();

            foreach (var m in Model)
            {
                m.LanguageText = m.Country.Name + " - " + m.Language.Name;
            }

            Model = Model.OrderBy(p => p.LanguageText).ToList();

            ViewBag.TargetCountryLanguageID = targetCountryLanguageId;

            return PartialView("Shared/_TargetCountryLanguageDropDown", Model);
        }

        public ActionResult TranslateResourceGridView(byte? countryLanguageId, string commandType, short? resourceId, bool acrossCountries = false)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        if (acrossCountries)
                        {
                            var cl = UnitOfWork.GetRepository<Models.Country_Language>().GetAll().Where(p => p.Country_Language_ID == countryLanguageId).Single();

                            // careful to never update English translation for the UK!
                            var items = UnitOfWork.GetRepository<Models.Resource_Language>().GetAll().Where(p => p.ResourceID == resourceId &&
                                p.LanguageID == cl.LanguageID &&
                                p.CountryID != 81).ToList();

                            items.ForEach(item => {
                                item.Value = null;
                                item.Stage = null;
                                item.ModifiedDT = DateTime.Now;
                            });

                            UnitOfWork.Save();
                        } 
                        else
                        {
                            var cl = UnitOfWork.GetRepository<Models.Country_Language>().GetAll().Where(p => p.Country_Language_ID == countryLanguageId).Single();

                            var item = UnitOfWork.GetRepository<Models.Resource_Language>().GetAll().Where(p => p.ResourceID == resourceId &&
                                p.CountryID == cl.CountryID &&
                                p.LanguageID == cl.LanguageID).Single();

                            item.Value = null;
                            item.Stage = null;
                            item.ModifiedDT = DateTime.Now;
                            UnitOfWork.Save();
                        }
                        break;
                }

                List<Resource_Language_SelectList_ResourceResult> Resources = new List<Resource_Language_SelectList_ResourceResult>();

                if (countryLanguageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Resource_Language_SelectList(countryLanguageId.Value);

                    Resources = Results.GetResult<Resource_Language_SelectList_ResourceResult>().ToList();
                }

                TranslateResourceGridViewModel Model = new TranslateResourceGridViewModel(Resources, countryLanguageId);

                return PartialView("Resources/_TranslateResourceGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }
        
        //[ValidateInput(false)]
        //public ActionResult TranslateResourceGridViewUpdate(Resource_Language_SelectList_ResourceResult item, byte countryLanguageId, bool acrossCountries = false)
        //{
        //    try
        //    {
        //        if(ModelState.IsValid)
        //        {
        //            List<short> countryIds = new List<short>();
        //            byte? languageId = null;

        //            TranslateResourceGridViewModel saveModel = new TranslateResourceGridViewModel();
        //            saveModel.Resources.Add(item);

        //            if (acrossCountries)
        //            {
        //                var cl = UnitOfWork.GetRepository<Models.Country_Language>().GetAll().Where(p => p.Country_Language_ID == countryLanguageId).Single();

        //                languageId = cl.LanguageID;

        //                // careful to never update English translation for the UK!
        //                var resources = UnitOfWork.GetRepository<Models.Resource_Language>().GetAll().Where(p => p.ResourceID == item.ResourceID && p.LanguageID == cl.LanguageID && p.CountryID != 81).ToList();

        //                var items = new List<Resource_Language_SelectList_ResourceResult>();

        //                resources.ForEach(resource => {
        //                    if (resource.CountryID != item.CountryID)
        //                    {
        //                        countryIds.Add(resource.CountryID);

        //                        items.Add(new Resource_Language_SelectList_ResourceResult()
        //                        {
        //                            ResourceID = resource.ResourceID,
        //                            CountryID = resource.CountryID,
        //                            LanguageID = cl.LanguageID,
        //                            Key = item.Key,
        //                            Value = item.Value,
        //                            TranslatedValue = item.TranslatedValue,
        //                            Stage = item.Stage,
        //                            FieldsToReview = item.FieldsToReview
        //                        });
        //                    }
        //                });

        //                saveModel.Resources.AddRange(items);
        //            }

        //            UnitOfWork.Context.Resource_Language_Save(countryIds, languageId.Value, countryLanguageId, saveModel);

        //            List<Resource_Language_SelectList_ResourceResult> Resources = new List<Resource_Language_SelectList_ResourceResult>();

                    
        //            IMultipleResults Results = UnitOfWork.Context.Resource_Language_SelectList(countryLanguageId);

        //            Resources = Results.GetResult<Resource_Language_SelectList_ResourceResult>().ToList();
                    
        //            TranslateResourceGridViewModel Model = new TranslateResourceGridViewModel(Resources, countryLanguageId);

        //            return PartialView("Resources/_TranslateResourceGridView", Model);
        //        }
        //        else
        //        {
        //            List<Resource_Language_SelectList_ResourceResult> Resources = new List<Resource_Language_SelectList_ResourceResult>();

        //            IMultipleResults Results = UnitOfWork.Context.Resource_Language_SelectList(countryLanguageId);

        //            Resources = Results.GetResult<Resource_Language_SelectList_ResourceResult>().ToList();

        //            TranslateResourceGridViewModel Model = new TranslateResourceGridViewModel(Resources, countryLanguageId);

        //            return PartialView("Resources/_TranslateResourceGridView", Model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        //        Response.StatusCode = 400;
        //        return Content(ex.Message);
        //    }    
        //}


        [ValidateInput(false)]
        public ActionResult TranslateResourceGridViewBatchUpdate(MVCxGridViewBatchUpdateValues<Resource_Language_SelectList_ResourceResult, int> updateValues, byte countryLanguageId, bool acrossCountries = false)
        {
            foreach(var item in updateValues.Update)
            {
                try
                {
                    List<short> countryIds = new List<short>();
                    byte? languageId = null;

                    TranslateResourceGridViewModel saveModel = new TranslateResourceGridViewModel();

                    var cl = UnitOfWork.GetRepository<Models.Country_Language>().GetAll().Where(p => p.Country_Language_ID == countryLanguageId).Single();

                    languageId = cl.LanguageID;

                    if (acrossCountries)
                    {
                        // careful to never update English translation for the UK!
                        var resources = UnitOfWork.GetRepository<Models.Resource_Language>().GetAll().Where(p => p.ResourceID == item.ResourceID && p.LanguageID == cl.LanguageID && p.CountryID != 81).ToList();

                        var items = new List<Resource_Language_SelectList_ResourceResult>();

                        resources.ForEach(resource => {
                            if (resource.CountryID != item.CountryID)
                            {
                                countryIds.Add(resource.CountryID);

                                items.Add(new Resource_Language_SelectList_ResourceResult()
                                {
                                    ResourceID = resource.ResourceID,
                                    CountryID = resource.CountryID,
                                    LanguageID = cl.LanguageID,
                                    Key = item.Key,
                                    Value = item.Value,
                                    TranslatedValue = item.TranslatedValue,
                                    Stage = item.Stage,
                                    FieldsToReview = item.FieldsToReview
                                });
                            }
                        });

                        saveModel.Resources.AddRange(items);
                    }
                    else
                    {
                        item.LanguageID = cl.LanguageID;
                        item.CountryID = cl.CountryID;

                        saveModel.Resources.Add(item);
                    }

                    UnitOfWork.Context.Resource_Language_Save(countryIds, languageId.Value, countryLanguageId, saveModel);
                }
                catch (Exception ex)
                {
                    updateValues.SetErrorText(item, $"{ex}");
                }
            }

            IMultipleResults Results = UnitOfWork.Context.Resource_Language_SelectList(countryLanguageId);

            var Model = new TranslateResourceGridViewModel(Results.GetResult<Resource_Language_SelectList_ResourceResult>().ToList(), countryLanguageId);

            return PartialView("Resources/_TranslateResourceGridView", Model);
        }
    }
}