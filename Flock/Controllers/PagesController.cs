using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public class PagesController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        /*
        @Html.Action("PageSearchProductTypeDropDownPartial", "Pages")
        @Html.Action("PageSearchBrandDropDownPartial", "Pages")
        @Html.Action("PageSearchLaunchDropDownPartial", "Pages")
        @Html.Action("PageSearchCollectionDropDownPartial", "Pages")
        @Html.Action("PageSearchStatusDropDownPartial", "Pages")
        @Html.Action("PageSearchColourDropDownPartial", "Pages")
        @Html.Action("PageSearchPatternDropDownPartial", "Pages")
        @Html.Action("PageSearchNameTextBox", "Pages")
         */

        public ActionResult PageSearchSiteSectionDropDownPartial()
        {
            List<SiteSection> Model = UnitOfWork.GetRepository<SiteSection>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/SiteSectionDropDownPartial.cshtml", Model);
        }

        public ActionResult PageSearchTemplateDropDownPartial()
        {
            List<Template> Model = UnitOfWork.GetRepository<Template>().GetAll().OrderBy(p => p.Name).ToList();
            return PartialView("~/Views/Shared/SearchFilters/TemplateDropDownPartial.cshtml", Model);
        }

        public ActionResult PageSearchNameOrIDTextPartial()
        {
            return PartialView("~/Views/Shared/SearchFilters/NameOrIDTextBoxPartial.cshtml");
        }

        [AllowAnonymous]
        public ActionResult PageSearch(byte? SiteSectionID, string SearchString)
        {
            if (SearchString == "" || SearchString == "Name, Code or ID")
                SearchString = null;

            List<Page_SearchResult> Model = new List<Page_SearchResult>();
            System.Data.Linq.ISingleResult<Page_SearchResult> Results = null;

            Results = UnitOfWork.Context.Page_Search(SiteSectionID, SearchString);

            foreach (Page_SearchResult result in Results)
            {
                Model.Add(result);
            }

            return PartialView("_PageSearchDataViewPartial", Model);
        }
    }
}
