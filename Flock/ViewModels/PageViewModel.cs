using Flock.Models;
using Flock.Models.ContentItems;
using Flock.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.ViewModels
{
    public class PageViewModelBuilder : ViewModelBuilder, IViewModelBuilder<PageViewModel>
    {
        public PageViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public PageViewModel Build(long? PageID)
        {
            PageViewModel PVM = new PageViewModel();

            PVM.Page = UoW.GetRepository<Page>().GetAll().Where(p => p.PageID == PageID).Single();
            PVM.PageSectionModels = new List<PageSectionModel>();
            List<PageSection> PageSections = UoW.GetRepository<PageSection>().GetAll().Where(p => p.PageID == PVM.Page.PageID).OrderBy(p => p.SortOrder).ToList();


            foreach(PageSection ps in PageSections)
            {
                PageSectionModel psm = new PageSectionModel();
                psm.PageSection = ps;
                psm.ContentItems = new List<ContentItem>();

                #region Fetch HTML Items
                List<PageSection_HTMLSection> HtmlSectionLinker = UoW.GetRepository<PageSection_HTMLSection>().GetAll().Where(p => p.PageSectionID == ps.PageSectionID).ToList();
                List<HtmlItem> HtmlItems = new List<HtmlItem>();
                foreach (PageSection_HTMLSection pshs in HtmlSectionLinker)
                {
                    HtmlItem item = new HtmlItem();
                    item.Record = UoW.GetRepository<HtmlSection>().GetAll().Where(p => p.HtmlID == pshs.HtmlSectionID).Single();
                    item.ItemType = ContentItemType.HTML;
                    item.SortOrder = item.Record.SortOrder;
                    HtmlItems.Add(item);
                }
                List<HtmlItem> SortedHtmlItems = HtmlItems.OrderBy(p => p.SortOrder).ToList();
                psm.ContentItems.AddRange(SortedHtmlItems.Cast<ContentItem>().ToList());
                #endregion

                #region Fetch SlideShow Items
                List<PageSection_SlideShow> SlideShowLinker = UoW.GetRepository<PageSection_SlideShow>().GetAll().Where(p => p.PageSectionID == ps.PageSectionID).ToList();
                List<SlideshowViewModel> SlideShowItems = new List<SlideshowViewModel>();
                foreach (PageSection_SlideShow ssips in SlideShowLinker)
                {
                    SlideshowViewModel item = new SlideshowViewModel();
                    item.Record = UoW.GetRepository<SlideShow>().GetAll().Where(p => p.SlideShowID == ssips.SlideShowID).Single();
                    item.ItemType = ContentItemType.SlideShow;
                    item.SortOrder = item.Record.SortOrder;
                    SlideShowItems.Add(item);
                }
                psm.ContentItems.AddRange(SlideShowItems.Cast<ContentItem>().ToList());
                #endregion

                #region Fetch GridSections
                List<PageSection_GridSection> GridSectionLinker = UoW.GetRepository<PageSection_GridSection>().GetAll().Where(p => p.PageSectionID == ps.PageSectionID).ToList();
                List<GridSectionItem> GridSections = new List<GridSectionItem>();
                foreach (PageSection_GridSection psgs in GridSectionLinker)
                {
                    GridSectionItem grid = new GridSectionItem();

                    grid.Record = UoW.GetRepository<GridSection>().GetAll().Where(p => p.GridID == psgs.GridSectionID).Single();
                    grid.Contents = new List<ContentItem>();
                    grid.ItemType = ContentItemType.Grid;
                    grid.SortOrder = grid.Record.SortOrder;

                    List<GridSection_ContentItem> ItemsInGridSection = UoW.GetRepository<GridSection_ContentItem>().GetAll().Where(p => p.GridSectionID == psgs.GridSectionID).ToList();
                    foreach (GridSection_ContentItem gsci in ItemsInGridSection)
                    {
                        switch (gsci.ContentTypeID)
                        {
                            case 1: // HTML
                                HtmlItem item = new HtmlItem();
                                item.Record = UoW.GetRepository<HtmlSection>().GetAll().Where(p => p.HtmlID == gsci.ContentItemID).Single();
                                item.ItemType = ContentItemType.HTML;
                                item.SortOrder = item.Record.SortOrder;
                                grid.Contents.Add(item);
                                break;
                            case 2: // SlideShow
                                break;
                            case 3: // GridSection... GridSections probably shouldn't contain GridSections. That'd be pushing it.
                                break;
                            case 4: // Not yet defined.
                                break;
                            default: break;
                        }
                    }

                    List<ContentItem> GridContentsOrdered = grid.Contents.OrderBy(p => p.SortOrder).ToList();
                    grid.Contents = GridContentsOrdered;

                    GridSections.Add(grid);
                }
                psm.ContentItems.AddRange(GridSections.Cast<ContentItem>().ToList());
                #endregion

                #region Fetch Forms
                List<PageSection_Form> FormLinker = UoW.GetRepository<PageSection_Form>().GetAll().Where(p => p.PageSectionID == ps.PageSectionID).ToList();
                List<FormItem> FormItems = new List<FormItem>();
                foreach (PageSection_Form psf in FormLinker)
                {
                    FormItem item = new FormItem();
                    item.Record = UoW.GetRepository<Form>().GetAll().Where(p => p.FormID == psf.FormID).Single();
                    item.ItemType = ContentItemType.Form;
                    //item.SortOrder = item.Record.SortOrder;
                    // do forms need sortorder...?
                    item.ClassName = psf.ClassName;
                    item.SortOrder = 0;
                    FormItems.Add(item);
                }
                psm.ContentItems.AddRange(FormItems.Cast<ContentItem>().ToList());
                #endregion

                #region Fetch blank content items (placeholders)
                List<PageSection_BlankContentItem> BlanksLinker = UoW.GetRepository<PageSection_BlankContentItem>().GetAll().Where(p => p.PageSectionID == ps.PageSectionID).ToList();
                List<Flock.Models.ContentItems.BlankContentItem> Blanks = new List<Flock.Models.ContentItems.BlankContentItem>();
                foreach (PageSection_BlankContentItem psbci in BlanksLinker)
                {
                    Flock.Models.ContentItems.BlankContentItem item = new Flock.Models.ContentItems.BlankContentItem();
                    item.Record = UoW.GetRepository<Flock.Models.BlankContentItem>().GetAll().Where(p => p.BlankContentItemID == psbci.BlankContentItemID).Single();
                    item.ItemType = ContentItemType.Blank;
                    item.SortOrder = item.Record.SortOrder;
                    Blanks.Add(item);
                }
                List<Flock.Models.ContentItems.BlankContentItem> SortedBlankItems = Blanks.OrderBy(p => p.SortOrder).ToList();
                psm.ContentItems.AddRange(SortedBlankItems.Cast<ContentItem>().ToList());
                #endregion

                List<ContentItem> OrderedList = psm.ContentItems.OrderBy(p => p.SortOrder).ToList();
                psm.ContentItems = OrderedList;

                PVM.PageSectionModels.Add(psm);
            }

            PVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();
            PVM.Forms = UoW.GetRepository<Form>().GetAll().ToList();
            PVM.Colours = UoW.GetRepository<Colour>().GetAll().ToList();

            PVM.PageSectionModels.OrderBy(p => p.PageSection.SortOrder);

            PVM.PageID = (long)PageID;
            PVM.InternalName = PVM.Page.Name;
            PVM.Title = PVM.Page.TitleTag;
            PVM.MetaDescription = PVM.Page.MetaDescription;
            PVM.PodImage = "";
            PVM.Heading = PVM.Page.Heading;

            return PVM;
        }

        public PageViewModel Save(PageViewModel PVM)
        {
            // how do we go about saving the records we were sent to a Page.
            Page page = UoW.GetRepository<Page>().GetAll().Where(p => p.PageID == PVM.PageID).Single();

            page.Name = PVM.InternalName;

            UoW.Save();

            return Build(page.PageID);
        }

        public void ReOrderSections(List<PageSectionInformation> PageSectionList)
        {
            foreach (PageSectionInformation psi in PageSectionList)
            {
                PageSection ps = UoW.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == psi.PageSectionID).Single();
                ps.SortOrder = psi.SortOrder;
            }
            UoW.Save();
        }
    }

    public struct PageSectionInformation
    {
        public long PageSectionID;
        public byte SortOrder;
    };


    public class PageViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public long PageID { get; set; }

        [Display(Name = "Internal-Use NameX")]
        [StringLength(50)]
        public string InternalName { get; set; }

        [Display(Name = "Meta Description")]
        [StringLength(180)]
        public string MetaDescription { get; set; }
        
        [Display(Name = "Title")]
        [StringLength(50)]
        public string Title { get; set; }
        
        [Display(Name = "Heading")]
        [StringLength(70)]
        public string Heading { get; set; }

        // ...?
        
        public string PodImage { get; set; }

        public List<PageSectionModel> PageSectionModels { get; set; }

        public List<Form> Forms { get; set; }

        public Page Page { get; set; }
    }
}