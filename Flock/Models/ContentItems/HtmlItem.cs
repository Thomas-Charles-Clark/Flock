using Flock.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.Models.ContentItems
{
    public class HtmlItem : ContentItem
    {
        public HtmlSection Record { get; set; }
    }


    public class HtmlSectionModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public HtmlSectionModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        //public HtmlSectionModel Build(long pageSectionID, long htmlSectionID)
        //{
        //    HtmlSectionModel Model = new HtmlSectionModel();
        //    Model.Linker = UoW.GetRepository<PageSection_HTMLSection>().GetAll().Where(p => p.PageSectionID == pageSectionID && p.HtmlSectionID == htmlSectionID).Single();
        //    Model.Record = UoW.GetRepository<HtmlSection>().GetAll().Where(p => p.HtmlID == Model.Linker.HtmlSectionID).Single();

        //    return Model;
        //}

        public HtmlItem Build(long htmlSectionID)
        {
            HtmlItem Model = new HtmlItem() { ItemType = ContentItemType.HTML };
            Model.Record = UoW.GetRepository<HtmlSection>().GetAll().Where(p => p.HtmlID == htmlSectionID).Single();
            Model.SortOrder = Model.Record.SortOrder;
            return Model;
        }


        public bool Save(string html, long htmlSectionID)
        {
            HtmlSection section = UoW.GetRepository<HtmlSection>().GetAll().Where(p => p.HtmlID == htmlSectionID).Single();

            section.Contents = HttpUtility.HtmlDecode(html);

            UoW.Save();
            return true;
        }

        public bool Create(string destinationtype, string className, int sortOrder, long pageSectionID)
        {
            switch (destinationtype)
            {
                case "grid-section":
                    try
                    {
                        HtmlSection section = new HtmlSection() { Contents = "<i>New HTML Section</i>", Class = className, SortOrder = sortOrder };
                        UoW.GetRepository<HtmlSection>().Add(section);
                        UoW.Save();

                        UoW.GetRepository<GridSection_ContentItem>().Add(new GridSection_ContentItem() { ContentItemID = section.HtmlID, ContentTypeID = 1, GridSectionID = pageSectionID });
                        UoW.Save();
                    }
                    catch (Exception e)
                    {
                        throw (e);
                    }
                    break;
                default:
                    try
                    {
                        HtmlSection section = new HtmlSection() { Contents = "<i>New HTML Section</i>", Class = className, SortOrder = sortOrder };
                        UoW.GetRepository<HtmlSection>().Add(section);
                        UoW.Save();

                        UoW.GetRepository<PageSection_HTMLSection>().Add(new PageSection_HTMLSection() { PageSectionID = pageSectionID, HtmlSectionID = section.HtmlID });
                        UoW.Save();
                    }
                    catch (Exception e)
                    {
                        throw (e);
                    }
                    break;
            }

            // Must create a new HTMLSection followed by a PageSection_HTMLSection linkage.


            return true;
        }
    }
}