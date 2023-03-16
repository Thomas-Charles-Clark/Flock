using Flock.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class PassPagePodFetch
    {
        public long PageSectionID { get; set; }
        public int PageID { get; set; }
    }

    public class PassPageFetch
    {
        public int PageID { get; set; }
        public int ParentPageID { get; set; }
    }

    public class PassPagePodSave
    {
        public long PageSectionID { get; set; }
        public int PageID { get; set; }
        public List<AttributeStub> AttributeIDs { get; set; }
    }

    public class PassCanonicalPageAttributeSave
    {
        public int PageID { get; set; }
        public int DestinationPageID { get; set; }
        public List<AttributeStub> AttributeIDs { get; set; }
    }

    public class AttributeStub
    {
        public int id { get; set; }
    }

    public class Attribute
    {
        public short AttributeID { get; set; }
        public string Name { get; set; }
        public bool EnableFilterIndexing { get; set; }
        public short SortOrder { get; set; }
        public string FormValue
        {
            get
            {
                return HttpUtility.UrlEncode(Name?.ToLowerInvariant().Replace(" ", "-"));
            }
        }
    }

    public class AttributeGroupStub
    {
        public short AttributeGroupID { get; set; }
        public string Name { get; set; }
        public short SortOrder { get; set; }

        public List<Attribute> Attributes { get; set; }
    }

    public class PagePodAdvancedSettingsViewModel
    {
        public long PageSectionID { get; set; }

        public int PageID { get; set; }

        public List<AttributeStub> AttributeIDs { get; set; }

        public List<Flock.ViewModels.Attribute> Attributes { get; set; }

        public List<AttributeGroupStub> AttributeGroups { get; set; }

        public Page Page { get; set; }

        public string ResultantURL { get; set; }

        public bool FilterIndexingEnabled { get; set; }

        public PagePodAdvancedSettingsViewModel(PassPagePodFetch objFetch)
        {
            PageSectionID = objFetch.PageSectionID;

            PageID = objFetch.PageID;

            var uow = new Flock.DAL.UnitOfWork<FlockDataContext>();

            var pageSectionPage = uow.GetRepository<PageSection_Page>().GetAll().Single(psp => psp.PageSectionID == PageSectionID && psp.PageID == PageID);

            Page = uow.GetRepository<Page>().GetAll().Single(p => p.PageID == PageID);

            FilterIndexingEnabled = Page.EnableFilterIndexing;

            if (FilterIndexingEnabled)
            {
                var attributeGroupLinker = uow.GetRepository<AttributeGroup_ProductType>().GetAll().Where(agpt => agpt.ProductTypeID == Page.ProductTypeID).Select(agpt => agpt.AttributeGroupID).ToList();

                AttributeIDs = JsonConvert.DeserializeObject<List<AttributeStub>>(pageSectionPage.AttributeList ?? "[]");

                AttributeGroups = uow.GetRepository<AttributeGroup>().GetAll().Where(ag => attributeGroupLinker.Contains(ag.AttributeGroupID)).Select(ag => new AttributeGroupStub()
                {
                    AttributeGroupID = ag.AttributeGroupID,
                    Name = ag.Heading,
                    SortOrder = ag.SortOrder,
                    Attributes = ag.Attributes.Select(at => new Attribute()
                    {
                        AttributeID = at.AttributeID,
                        Name = at.Name,
                        EnableFilterIndexing = at.EnableFilterIndexing,
                        SortOrder = at.SortOrder
                    }).ToList()
                }).ToList();

                Attributes = AttributeGroups.SelectMany(ag => ag.Attributes.Where(at => at.EnableFilterIndexing)).ToList();
            }
        }
    }

    public class CanonicalPageAttributeSettingsViewModel
    {
        public int PageID { get; set; }

        public int ParentPageID { get; set; }

        public List<AttributeStub> AttributeIDs { get; set; }

        public List<Flock.ViewModels.Attribute> Attributes { get; set; }

        public List<AttributeGroupStub> AttributeGroups { get; set; }

        public Page Page { get; set; }

        public Page ParentPage { get; set; }

        public string ResultantURL { get; set; }

        public bool FilterIndexingEnabled { get; set; }

        public CanonicalPageAttributeSettingsViewModel(PassPageFetch objFetch)
        {
            PageID = objFetch.PageID;

            ParentPageID = objFetch.ParentPageID;

            var uow = new Flock.DAL.UnitOfWork<FlockDataContext>();

            Page = uow.GetRepository<Page>().GetAll().Single(p => p.PageID == PageID);

            ParentPage = uow.GetRepository<Page>().GetAll().Single(p => p.PageID == objFetch.ParentPageID);

            FilterIndexingEnabled = Page.EnableFilterIndexing;

            if (FilterIndexingEnabled)
            {
                var attributeGroupLinker = uow.GetRepository<AttributeGroup_ProductType>().GetAll().Where(agpt => agpt.ProductTypeID == Page.ProductTypeID).Select(agpt => agpt.AttributeGroupID).ToList();

                AttributeIDs = JsonConvert.DeserializeObject<List<AttributeStub>>(ParentPage.CanonicalAttributeList ?? "[]");

                AttributeGroups = uow.GetRepository<AttributeGroup>().GetAll().Where(ag => attributeGroupLinker.Contains(ag.AttributeGroupID)).Select(ag => new AttributeGroupStub()
                {
                    AttributeGroupID = ag.AttributeGroupID,
                    Name = ag.Heading,
                    SortOrder = ag.SortOrder,
                    Attributes = ag.Attributes.Select(at => new Attribute()
                    {
                        AttributeID = at.AttributeID,
                        Name = at.Name,
                        EnableFilterIndexing = at.EnableFilterIndexing,
                        SortOrder = at.SortOrder
                    }).ToList()
                }).ToList();

                Attributes = AttributeGroups.SelectMany(ag => ag.Attributes.Where(at => at.EnableFilterIndexing)).ToList();
            }
        }
    }
}