using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using RestSharp;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using CacheBuster;
using CacheBuster.Models;

namespace Flock.Controllers
{
    public partial class CMSController : Controller
    {
        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

        private UserManager _userManager;
        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: CMS_CacheBuster
        public ActionResult CacheBuster()
        {
            var Model = new CacheBusterViewModel();

            var countryLanguages = UoW.GetRepository<Country_Language>().GetAll().Where(p => UoW.Context.fnc_DateControl(p.OnDate, p.OffDate, DateTime.Now) == true).ToList();

            var sites = new List<PassCountryLanguage>();

            countryLanguages.ForEach(p => {
                sites.Add(new PassCountryLanguage() {
                    Name = $"{p.Name} - {p.Language.Name}",
                    CountryID = p.CountryID,
                    LanguageID = p.LanguageID
                });
            });

            Model.Sites = sites;

            Model.Sites.Insert(0, new Flock.Models.PassCountryLanguage() { CountryID = -1, LanguageID = 0, Name = "All" });

            return View("CacheBuster/CacheBuster", Model);
        }

        [HttpPost]
        public ActionResult LandingPage_Autocomplete(string term)
        {
            var searchResults = UoW.Context.Page_Search(null, term).ToList();

            var returnObj = new List<IdNameTuple>();

            searchResults.ForEach(p => {
                returnObj.Add(new IdNameTuple()
                {
                    ContentID = p.PageID,
                    Name = p.Name
                });
            });

            return Json(returnObj);
        }

        [HttpPost]
        public ActionResult ProductPage_Autocomplete(string term)
        {
            var searchResults = UoW.Context.ProductPage_Search(null, null, null, null, null, null, null, null, term).ToList();

            var returnObj = new List<IdNameTuple>();

            searchResults.ForEach(p => {
                returnObj.Add(new IdNameTuple() {
                    ContentID = p.ProductPageID,
                    Name = p.Name
                });
            });

            return Json(returnObj);
        }

        [HttpPost]
        public ActionResult Collection_Autocomplete(string term)
        {
            var searchResults = UoW.Context.Collection_Search(null, null, null, term).ToList();

            var returnObj = new List<IdNameTuple>();

            searchResults.ForEach(p => {
                returnObj.Add(new IdNameTuple()
                {
                    ContentID = p.CollectionID,
                    Name = p.Name
                });
            });

            return Json(returnObj);
        }

        [HttpPost]
        public ActionResult Article_Autocomplete(string term)
        {
            var searchResults = UoW.Context.Article_Search(null, term).ToList();

            var returnObj = new List<IdNameTuple>();

            searchResults.ForEach(p => {
                returnObj.Add(new IdNameTuple()
                {
                    ContentID = p.ArticleID,
                    Name = p.Name
                });
            });

            return Json(returnObj);
        }

        [HttpPost]
        public ActionResult ProductType_Autocomplete(string term)
        {
            term = term?.ToLowerInvariant();

            var searchResults = UoW.Context.ProductTypes.Where(pt => pt.DeletedDT == null).ToList();

            searchResults = searchResults.Where(pt => (String.IsNullOrEmpty(pt.Name) || pt.Name.ToLowerInvariant().StartsWith(term)) ||
                                                      (String.IsNullOrEmpty(pt.Name) || pt.Name.ToLowerInvariant().Contains(term))).ToList();

            var returnObj = new List<IdNameTuple>();

            searchResults.ForEach(p => {
                returnObj.Add(new IdNameTuple()
                {
                    ContentID = p.ProductTypeID,
                    Name = p.Name
                });
            });

            return Json(returnObj);
        }
    }

    public class PassCacheBuster
    {
        public string ContentType { get; set; }
        public int ContentID { get; set; }
        public string ContentDate { get; set; }

        public List<CountryIDLanguageID> Sites { get; set; }
    }

    public class IdNameTuple
    {
        public int ContentID { get; set; }
        public string Name { get; set; }
    }

    public class CountryIDLanguageID
    {
        public short CountryID { get; set; }
        public byte LanguageID { get; set; }
    }
}