using Flock.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.Models.ContentItems
{
    public class ContentItem
    {
        public int SortOrder { get; set; }
        public ContentItemType ItemType { get; set; }
    }

    public class BlankContentItemBuilder 
    {
        private UnitOfWork<FlockDataContext> UoW;

        public BlankContentItemBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public void Create(string destinationtype, string className, int sortOrder, long pageSectionID)
        {
            Flock.Models.BlankContentItem BCI = new Models.BlankContentItem();
            BCI.SortOrder = Convert.ToByte(sortOrder);
            BCI.Class = className;
            UoW.GetRepository<Flock.Models.BlankContentItem>().Add(BCI);
            UoW.Save();
            UoW.GetRepository<PageSection_BlankContentItem>().Add(new PageSection_BlankContentItem() { PageSectionID = pageSectionID, BlankContentItemID = BCI.BlankContentItemID });
            UoW.Save();
        }
    }

    public class BlankContentItem : ContentItem
    {
        public Flock.Models.BlankContentItem Record { get; set; }
    }

    public enum ContentItemType
    {
        Product,
        Image,
        HTML,
        SlideShow,
        Banner,
        Grid,
        SearchResult,
        Form,
        Blank
    }
}