using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class PageTreeViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;
 
        public PageTreeViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public List<PageTreeViewModel> Build(byte? SiteSectionID)
        {
            var result = new List<PageTreeViewModel>();

            List<Flock.Models.Page> q = new List<Page>();

            if (SiteSectionID != null)
                q = UoW.GetRepository<Page>().GetAll().Where(p => p.DeletedDT == null && p.SiteSectionID == SiteSectionID).OrderBy(p => p.Name).ToList();
            else
                q = UoW.GetRepository<Page>().GetAll().Where(p => p.DeletedDT == null && p.SiteSectionID == 0).OrderBy(p => p.Name).ToList();

            foreach (Page p in q)
            {
                result.Add(new PageTreeViewModel() { 
                    PageID = p.PageID,
                    ParentID = p.ParentID,
                    SiteSectionID = p.SiteSectionID,
                    Name = p.Name,
                    Heading = p.Heading,
                    SubHeading = p.SubHeading,
                    MetaDescription = p.MetaDescription,
                    TitleTag = p.TitleTag,
                    URLKeywords = p.URLKeywords
                });
            }

            return result;
        }
    }

    public class PageTreeViewModel
    {
        public int? PageID { get; set; }
        public int? ParentID { get; set; }
        public byte SiteSectionID { get; set; }
        public byte TemplateID { get; set; }

        public string Name { get; set; }

        [StringLength(70)]
        public string Heading { get; set; }

        [StringLength(70)]
        public string SubHeading { get; set; }

        [StringLength(70)]
        public string TitleTag { get; set; }

        [StringLength(100)]
        [RegularExpression("^([a-z]|[a-z]-|[a-z]/)+[a-z]$", ErrorMessage = @"Regular Expression Validation Failed.
    URLs must not begin or end with a slash.
    URL segments must not contain accented characters, such as é or ë.
    URL segments may only contain the lower case letters 'a' through 'z', as well as hyphens.
    URL segments must not begin or end with a hyphen.")]
        public string URLKeywords { get; set; }

        [StringLength(155)]
        public string MetaDescription { get; set; }

        [StringLength(2000)]
        public string MarketingMessage { get; set; }
    }
}