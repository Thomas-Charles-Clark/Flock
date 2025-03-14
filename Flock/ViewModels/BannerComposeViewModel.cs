using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Flock.ViewModels
{
    public class BannerComposeViewModelBuilder : ViewModelBuilder, IViewModelBuilder<BannerComposeViewModel>
    {
        public BannerComposeViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public BannerComposeViewModel Build(long? BannerID)
        {
            BannerComposeViewModel BCVM = new BannerComposeViewModel();
            vwBannerOverview b = UoW.GetRepository<vwBannerOverview>().GetAll().Where(p => p.BannerID == BannerID).Single();
            
            List<short> CountryRestrictionLinker = UoW.GetRepository<Banner_Country>().GetAll().Where(p => p.BannerID == BannerID).Select(p => p.CountryID).ToList();
            BCVM.CountryRestrictions = UoW.GetRepository<Country>().GetAll().Where(p => CountryRestrictionLinker.Contains(p.CountryID)).ToList();

            List<int> PageRestrictionLinker = UoW.GetRepository<Banner_Page>().GetAll().Where(p => p.BannerID == BannerID).Select(p => p.PageID).ToList();
            BCVM.PageRestrictions = UoW.GetRepository<Page>().GetAll().Where(p => PageRestrictionLinker.Contains(p.PageID)).ToList();

            BCVM.BannerID = Convert.ToInt32(BannerID);
            List<byte> BannerLocationLinker = UoW.GetRepository<Banner_BannerLocation>().GetAll().Where(p => p.BannerID == BannerID).Select(p => p.BannerLocationID).ToList();
            List<string> BannerLocationLinkerStrings = BannerLocationLinker.ConvertAll(Convert.ToString);
            BCVM.BannerLocationIDs = string.Join(",", BannerLocationLinkerStrings.ToArray());
            
            BCVM.BannerLocations = UoW.GetRepository<Banner_BannerLocation>().GetAll().Where(p => p.BannerID == BannerID).ToList();

            List<byte> SiteSectionLinker = UoW.GetRepository<Banner_SiteSection>().GetAll().Where(p => p.BannerID == BannerID).Select(p => p.SiteSectionID).ToList();
            List<string> SiteSectionLinkerStrings = SiteSectionLinker.ConvertAll(Convert.ToString);
            BCVM.SiteSectionIDs = string.Join(",", SiteSectionLinkerStrings.ToArray());

            List<byte> MenuLinker = UoW.GetRepository<Banner_Menu>().GetAll().Where(p => p.BannerID == BannerID).Select(p => p.MenuID).ToList();
            List<string> MenuLinkerStrings = MenuLinker.ConvertAll(Convert.ToString);
            BCVM.MenuIDs = string.Join(",", MenuLinkerStrings.ToArray());

            List<byte> ProductTypeLinker = UoW.GetRepository<Banner_ProductType>().GetAll().Where(p => p.BannerID == BannerID).Select(p => p.ProductTypeID).ToList();
            List<string> ProductTypeLinkerStrings = ProductTypeLinker.ConvertAll(Convert.ToString);
            BCVM.ProductTypeIDs = string.Join(",", ProductTypeLinkerStrings.ToArray());

            BCVM.CssClasses = UoW.GetRepository<Banner_FontClass>().GetAll().Where(p => p.BannerID == BannerID).ToList();

            BCVM.SiteSections = UoW.GetRepository<SiteSection>().GetAll().Where(p => SiteSectionLinker.Contains(p.SiteSectionID)).ToList();
            BCVM.ProductTypes = UoW.GetRepository<ProductType>().GetAll().Where(p => ProductTypeLinker.Contains(p.ProductTypeID)).ToList();
            BCVM.LanguageID = b.LanguageID;
            BCVM.Name = b.Name;
            BCVM.LinkTypeID = b.BannerLinkTypeID;
            BCVM.TitleText = b.TitleText;
            BCVM.AltText = b.AltText;
            BCVM.FontClassID = b.FontClassID;
            BCVM.OverlayText = b.OverlayText;
            BCVM.LinkURL = b.LinkURL;
            BCVM.ExternalURL = b.ExternalURL;
            BCVM.NewWindow = b.NewWindow;
            BCVM.OnDate = b.OnDate;
            BCVM.OffDate = b.OffDate;
            BCVM.CreatedDT = b.CreatedDT;
            BCVM.DeletedDT = b.DeletedDT;
            BCVM.Version = b.Version;
            BCVM.PageID = b.PageID;
            BCVM.CollectionID = b.CollectionID;
            BCVM.ProductPageID = b.ProductPageID;
            BCVM.ArticleID = b.ArticleID;
            BCVM.TradeSwitch = b.TradeSwitch;
            BCVM.BackgroundColour = b.BackgroundColour;
            BCVM.ForegroundColour = b.ForegroundColour;

            // 156 - 256 - 342 - 512 - 1024 - 1546
            BCVM.ImageWidth = b.Width;

            if (BCVM.ImageWidth != null)
            {
                if (BCVM.ImageWidth >= 1546)
                    BCVM.WidestAvailableGeneratedImage = 1546;
                else if (BCVM.ImageWidth >= 1024)
                    BCVM.WidestAvailableGeneratedImage = 1024;
                else if (BCVM.ImageWidth >= 512)
                    BCVM.WidestAvailableGeneratedImage = 512;
                else if (BCVM.ImageWidth >= 342)
                    BCVM.WidestAvailableGeneratedImage = 342;
                else if (BCVM.ImageWidth >= 256)
                    BCVM.WidestAvailableGeneratedImage = 256;
                else if (BCVM.ImageWidth >= 156)
                    BCVM.WidestAvailableGeneratedImage = 156;
            }
            else
            {
                BCVM.WidestAvailableGeneratedImage = null;
            }
            return BCVM;
        }
    }

    public class BannerComposeViewModel : BaseViewModel
    {
        public int? BannerID { get; set; }
        public string BannerLocationIDs { get; set; }
        public string SiteSectionIDs { get; set; }
        public string ProductTypeIDs { get; set; }
        public string MenuIDs { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(70, MinimumLength = 3)]
        public string TitleText { get; set; }

        [StringLength(70, MinimumLength = 3)]
        public string AltText { get; set; }

        [StringLength(100)]
        public string OverlayText { get; set; }

        public short FontClassID { get; set; }

        public byte? LinkTypeID { get; set; }
        public byte? LanguageID { get; set; }
        public int? PageID { get; set; }
        public string PageName { get; set; }
        public int? ProductPageID { get; set; }
        public string ProductPageName { get; set; }
        public short? CollectionID { get; set; }
        public string CollectionName { get; set; }
        public int? ArticleID { get; set; }
        public string ArticleName { get; set; }
        public string LinkURL { get; set; }
        public string ExternalURL { get; set; }
        public bool? NewWindow { get; set; }
        public string ImageFileExtension { get; set; }
        public byte Version { get; set; }
        public DateTime? OnDate { get; set; }
        public DateTime? OffDate { get; set; }
        public DateTime CreatedDT { get; set; }
        public DateTime? DeletedDT { get; set; }

        public short? ImageWidth { get; set; }
        public short? WidestAvailableGeneratedImage { get; set; }

        public string BackgroundColour { get; set; }

        public string ForegroundColour { get; set; }

        public bool? TradeSwitch { get; set; }

        public BannerSearchHeader HeaderFilters { get; set; }

        public List<Banner_FontClass> CssClasses { get; set; }
        public List<Country> CountryRestrictions { get; set; }
        public List<Page> PageRestrictions { get; set; }
        public List<Banner_BannerLocation> BannerLocations { get; set; }
        public List<SiteSection> SiteSections { get; set; }
        public List<ProductType> ProductTypes { get; set; }

        public List<short> CssClassesToSave { get; set; }
    }

    public class BannerSearchHeader
    {
        public short? BannerID;
        public short? CountryID;
        public byte? BannerLocationID;
        public byte? SiteSectionID;
        public short? BannerCollectionID;
        public short? LanguageID;
        public byte? StatusID;
    }
}