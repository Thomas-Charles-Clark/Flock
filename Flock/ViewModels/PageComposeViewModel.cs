using Flock.Models;
using Flock.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Flock.ViewModels
{
    //public enum WebStatusEnum
    //{
    //    On,
    //    Pending_Off,
    //    Pending_On,
    //    PreLaunch,
    //    Off
    //};

    public enum SiteSectionEnum
    {
        Home,
        Shop,
        Fabric_and_Wallpaper,
        Furniture,
        Contracts,
        Inspire_Me
    };

    public enum PageSectionTypeEnum
    {
        HTML,
        Slideshow,
        Form,
        Product_Grid,
        Collection_Grid,
        Page_Grid,
        Brochure,
        Banner_Location,
        Article_Grid,
        Video,
        Masonry,
        Header
    };

    public class PageViewModelBuilder : ViewModelBuilder
    {
        public PageViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public PageComposeViewModel Build(int? PageID, short? CountryID, byte? LanguageID)
        {
            PageComposeViewModel PVM = new PageComposeViewModel();

            Page page = UoW.GetRepository<Page>().GetAll().Where(p => p.PageID == PageID).Single();

            PVM.DeletedDT = page.DeletedDT;
            PVM.PageID = page.PageID;
            PVM.Name = page.Name;
            PVM.MetaDescription = page.MetaDescription;
            PVM.Description = page.Description;
            PVM.SiteSectionID = page.SiteSectionID;
            PVM.TitleTag = page.TitleTag;
            PVM.Heading = page.Heading;
            PVM.SubHeading = page.SubHeading;
            PVM.URLKeywords = page.URLKeywords;
            PVM.New = page.New;
            PVM.Images = UoW.GetRepository<Page_Image>().GetAll().Where(p => p.PageID == page.PageID).ToList();
            PVM.RelatedFilterBold = page.RelatedFilterBold;
            PVM.CalculateDiscount = page.CalculateDiscount;
            PVM.RelatedFilterFontColour = page.RelatedFilterFontColour;
            PVM.ProductTypeID = page.ProductTypeID;
            PVM.ProductType = UoW.GetRepository<ProductType>().GetAll().SingleOrDefault(pt => pt.ProductTypeID == PVM.ProductTypeID);
            PVM.EnableFilterIndexing = page.EnableFilterIndexing;
            PVM.Clearance = page.Clearance;
            PVM.ShowRecentlyViewed = page.ShowRecentlyViewed;
            PVM.NoIndex = page.NoIndex;
            PVM.NoFollow = page.NoFollow;
            PVM.CanonicalPageID = page.CanonicalPageID;
            PVM.CanonicalPage = UoW.GetRepository<Page>().GetAll().SingleOrDefault(p => p.PageID == PVM.CanonicalPageID);
            PVM.AttributeList = JsonConvert.DeserializeObject<List<AttributeStub>>(page.CanonicalAttributeList ?? "[]");
            var attributeIds = PVM.AttributeList.Select(al => al.id).ToList();
            PVM.Attributes = UoW.GetRepository<Flock.Models.Attribute>().GetAll().Where(a => attributeIds.Contains(a.AttributeID)).ToList();

            List<short> Linker = UoW.GetRepository<Page_Country>().GetAll().Where(p => p.PageID == PageID).Select(p => p.CountryID).ToList();
            PVM.CountryRestrictions = UoW.GetRepository<Country>().GetAll().Where(p => Linker.Contains(p.CountryID)).ToList();

            List<PageSection> PageSections = UoW.GetRepository<PageSection>().GetAll().Where(p => p.PageID == page.PageID && p.DeletedDT == null).ToList();

            if (CountryID != null)
            {
                PageSections = PageSections.Where(p => p.PageSection_Country_Languages.Count == 0 || p.PageSection_Country_Languages.Select(q => q.CountryID).ToList().Contains(CountryID.Value)).ToList();
            }

            if (LanguageID != null)
            {
                PageSections = PageSections.Where(p => p.PageSection_Country_Languages.Count == 0 || p.PageSection_Country_Languages.Select(q => q.LanguageID).ToList().Contains(LanguageID.Value)).ToList();
            }

            PVM.PageSections = new List<BasePageSection>();

            for (int i = 0; i < PageSections.Count; i++)
            { 
                switch(PageSections[i].PageSectionTypeID)
                {
                    case 1: // html
                        PageSection_HTMLSection HPS = UoW.GetRepository<PageSection_HTMLSection>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).Single();
                        HTMLSection htmlsection = UoW.GetRepository<HTMLSection>().GetAll().Where(p => p.HTMLSectionID == HPS.HtmlSectionID).Single();
                        HTML_PageSection hps = new HTML_PageSection();
                        hps.PageSectionName = PageSections[i].Name;
                        hps.PageID = (int)PageID;
                        hps.PageSectionID = HPS.PageSectionID;
                        hps.PageSectionTypeID = PageSections[i].PageSectionTypeID;
                        hps.OnDate = PageSections[i].OnDate;
                        hps.OffDate = PageSections[i].OffDate;
                        hps.HTMLSectionID = htmlsection.HTMLSectionID;
                        hps.Contents = htmlsection.Contents;
                        hps.SortOrder = PageSections[i].SortOrder;
                        hps.MultiplePodImages = PageSections[i].MultiplePodImages;
                        hps.Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(hps.OnDate, hps.OffDate)).Select(p => p.Name).Single();
                        hps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        hps.Heading = PageSections[i].Heading;
                        hps.PlatformSwitch = PageSections[i].PlatformSwitch;
                        hps.TradeSwitch = PageSections[i].TradeSwitch;
                        hps.IsHeader = PageSections[i].IsHeader;
                        hps.HalfWidth = PageSections[i].HalfWidth;
                        hps.ViewportWidth = PageSections[i].ViewportWidth;
                        hps.PreviewColourways = PageSections[i].PreviewColourways;
                        hps.PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip;
                        hps.CountryLanguageRestrictions = UoW.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == HPS.PageSectionID).ToList();
                        hps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(hps);
                        break;
                    case 2: // slideshow
                        PageSection_SlideShow PSS = UoW.GetRepository<PageSection_SlideShow>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).Single();
                        SlideShow slideshowsection = UoW.GetRepository<SlideShow>().GetAll().Where(p => p.SlideShowID == PSS.SlideShowID).Single();
                        Slideshow_PageSection sps = new Slideshow_PageSection();
                        sps.PageSectionName = PageSections[i].Name;
                        sps.PageID = (int)PageID;
                        sps.PageSectionID = PSS.PageSectionID;
                        sps.OnDate = PageSections[i].OnDate;
                        sps.OffDate = PageSections[i].OffDate;
                        sps.PageSectionTypeID = PageSections[i].PageSectionTypeID;
                        sps.SlideshowID = slideshowsection.SlideShowID;
                        sps.SortOrder = PageSections[i].SortOrder;
                        sps.AspectRatio = slideshowsection.AspectRatio;
                        AspectRatio ar = UoW.GetRepository<AspectRatio>().GetAll().Where(p => p.AspectRatio1 == Convert.ToDecimal(sps.AspectRatio)).Single();
                        sps.ARID = "ar" + ar.AspectRatioID.ToString();
                        sps.ARDescription = ar.Name;
                        sps.MultiplePodImages = PageSections[i].MultiplePodImages;
                        sps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        sps.Heading = PageSections[i].Heading;
                        sps.PlatformSwitch = PageSections[i].PlatformSwitch;
                        sps.TradeSwitch = PageSections[i].TradeSwitch;
                        sps.IsHeader = PageSections[i].IsHeader;
                        sps.HalfWidth = PageSections[i].HalfWidth;
                        sps.ViewportWidth = PageSections[i].ViewportWidth;
                        sps.PreviewColourways = PageSections[i].PreviewColourways;
                        sps.PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip;
                        List<SlideShow_Image> imgs = UoW.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slideshowsection.SlideShowID).OrderBy(p => p.SortOrder).ToList();
                        sps.Images = new List<SlideshowImage>();
                        foreach (SlideShow_Image img in imgs)
                        {
                            SlideshowImage ssi = new SlideshowImage();
                            ssi.Record = img;
                            ssi.Hotspots = UoW.Context.ImageHotspot_Select(ssi.Record.ImageID, 1, DateTime.Now, true).ToList();
                            sps.Images.Add(ssi);
                        }
                        sps.Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(sps.OnDate, sps.OffDate)).Select(p => p.Name).Single();
                        sps.CountryLanguageRestrictions = UoW.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PSS.PageSectionID).ToList();
                        sps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(sps);
                        break;
                    case 3: // form
                        PageSection_Form PSF = UoW.GetRepository<PageSection_Form>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).Single();
                        Form formsection = UoW.GetRepository<Form>().GetAll().Where(p => p.FormID == PSF.FormID).Single();
                        Form_PageSection fps = new Form_PageSection();
                        fps.PageSectionName = formsection.Name;
                        fps.SourceID = formsection.SourceID;
                        fps.PageID = (int)PageID;
                        fps.PageSectionID = PageSections[i].PageSectionID;
                        fps.OnDate = PageSections[i].OnDate;
                        fps.OffDate = PageSections[i].OffDate;
                        fps.PageSectionTypeID = PageSections[i].PageSectionTypeID;
                        fps.FormID = formsection.FormID;
                        fps.SortOrder = PageSections[i].SortOrder;
                        fps.SubHeading = formsection.SubHeading;
                        fps.CompetitionID = formsection.CompetitionID;
                        fps.ThirdPartyID = formsection.ThirdPartyID;
                        fps.BrochureID = formsection.BrochureID;
                        fps.DisplayAddressBlock = formsection.AddressRequired;
                        fps.ConfirmEmail = formsection.ConfirmEmail;
                        fps.RedirectPageID = formsection.RedirectPageID;
                        fps.PromotionCodeID = formsection.PromotionCodeID;
                        fps.Heading = formsection.Heading;
                        fps.Name = formsection.Name;
                        fps.CompetitionID = formsection.CompetitionID;
                        fps.UseCaptcha = formsection.CaptchaRequired;
                        fps.Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(fps.OnDate, fps.OffDate)).Select(p => p.Name).Single();
                        fps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        fps.MultiplePodImages = PageSections[i].MultiplePodImages;
                        fps.PlatformSwitch = PageSections[i].PlatformSwitch;
                        fps.TradeSwitch = PageSections[i].TradeSwitch;
                        fps.IsHeader = PageSections[i].IsHeader;
                        fps.StoreEventID = formsection.StoreEventID;
                        fps.HalfWidth = PageSections[i].HalfWidth;
                        fps.ViewportWidth = PageSections[i].ViewportWidth;
                        fps.PhoneNumberRequired = formsection.PhoneNumberRequired;
                        fps.PreviewColourways = PageSections[i].PreviewColourways;
                        fps.PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip;
                        fps.CountryLanguageRestrictions = UoW.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PSF.PageSectionID).ToList();
                        fps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(fps);
                        break;
                    case 4: // Product Page Grid
                        BasePageSection bps = new BasePageSection() { 
                            PageSectionID = PageSections[i].PageSectionID, 
                            PageSectionName = PageSections[i].Name, 
                            OnDate = PageSections[i].OnDate, 
                            OffDate = PageSections[i].OffDate,
                            PageID = (int)PageID,
                            PageSectionTypeID = PageSections[i].PageSectionTypeID,
                            SortOrder = PageSections[i].SortOrder,
                            MultiplePodImages = PageSections[i].MultiplePodImages,
                            ShowFilters = PageSections[i].Filter,
                            PlatformSwitch = PageSections[i].PlatformSwitch,
                            TradeSwitch = PageSections[i].TradeSwitch,
                            Status =  UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(PageSections[i].OnDate, PageSections[i].OffDate)).Select(p => p.Name).Single(),
                            Paging = PageSections[i].Paging,
                            PreviewColourways = PageSections[i].PreviewColourways,
                            PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip
                        };
                        bps.CountryLanguageRestrictions = UoW.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).ToList();
                        bps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        bps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(bps);
                        break;
                    case 5: // Collection Grid
                        BasePageSection cbps = new BasePageSection()
                        {
                            PageSectionID = PageSections[i].PageSectionID,
                            PageSectionName = PageSections[i].Name,
                            OnDate = PageSections[i].OnDate,
                            OffDate = PageSections[i].OffDate,
                            PageID = (int)PageID,
                            PageSectionTypeID = PageSections[i].PageSectionTypeID,
                            SortOrder = PageSections[i].SortOrder,
                            MultiplePodImages = PageSections[i].MultiplePodImages,
                            Heading = PageSections[i].Heading,
                            PlatformSwitch = PageSections[i].PlatformSwitch,
                            TradeSwitch = PageSections[i].TradeSwitch,
                            IsHeader = PageSections[i].IsHeader,
                            HalfWidth = PageSections[i].HalfWidth,
                            ViewportWidth = PageSections[i].ViewportWidth,
                            Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(PageSections[i].OnDate, PageSections[i].OffDate)).Select(p => p.Name).Single(),
                            PreviewColourways = PageSections[i].PreviewColourways,
                            PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip
                        };
                        cbps.CountryLanguageRestrictions = UoW.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).ToList();
                        cbps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        cbps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(cbps);
                        break;
                    case 6: // Page Grid
                        BasePageSection pbps = new BasePageSection()
                        {
                            PageSectionID = PageSections[i].PageSectionID,
                            PageSectionName = PageSections[i].Name,
                            OnDate = PageSections[i].OnDate,
                            OffDate = PageSections[i].OffDate,
                            PageID = (int)PageID,
                            PageSectionTypeID = PageSections[i].PageSectionTypeID,
                            ShowFilters = PageSections[i].Filter,
                            SortOrder = PageSections[i].SortOrder,
                            MultiplePodImages = PageSections[i].MultiplePodImages,
                            Heading = PageSections[i].Heading,
                            PlatformSwitch = PageSections[i].PlatformSwitch,
                            TradeSwitch = PageSections[i].TradeSwitch,
                            IsHeader = PageSections[i].IsHeader,
                            HalfWidth = PageSections[i].HalfWidth,
                            ViewportWidth = PageSections[i].ViewportWidth,
                            Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(PageSections[i].OnDate, PageSections[i].OffDate)).Select(p => p.Name).Single(),
                            PreviewColourways = PageSections[i].PreviewColourways,
                            PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip
                        };
                        pbps.CountryLanguageRestrictions = UoW.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).ToList();
                        pbps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        pbps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(pbps);
                        break;
                    case 7:
                        BasePageSection bbps = new BasePageSection()
                        {
                            PageSectionID = PageSections[i].PageSectionID,
                            PageSectionName = PageSections[i].Name,
                            OnDate = PageSections[i].OnDate,
                            OffDate = PageSections[i].OffDate,
                            PageID = (int)PageID,
                            PageSectionTypeID = PageSections[i].PageSectionTypeID,
                            SortOrder = PageSections[i].SortOrder,
                            MultiplePodImages = PageSections[i].MultiplePodImages,
                            Heading = PageSections[i].Heading,
                            PlatformSwitch = PageSections[i].PlatformSwitch,
                            TradeSwitch = PageSections[i].TradeSwitch,
                            IsHeader = PageSections[i].IsHeader,
                            HalfWidth = PageSections[i].HalfWidth,
                            ViewportWidth = PageSections[i].ViewportWidth,
                            Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(PageSections[i].OnDate, PageSections[i].OffDate)).Select(p => p.Name).Single(),
                            PreviewColourways = PageSections[i].PreviewColourways,
                            PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip
                        };
                        bbps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        bbps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(bbps);
                        break;
                    case 8: // Banner Locations Page Section
                        BannerLocation_PageSection blps = new BannerLocation_PageSection()
                        {
                            PageSectionID = PageSections[i].PageSectionID,
                            PageSectionName = PageSections[i].Name,
                            OnDate = PageSections[i].OnDate,
                            OffDate = PageSections[i].OffDate,
                            PageID = (int)PageID,
                            PageSectionTypeID = PageSections[i].PageSectionTypeID,
                            SortOrder = PageSections[i].SortOrder,
                            MultiplePodImages = PageSections[i].MultiplePodImages,
                            Heading = PageSections[i].Heading,
                            PlatformSwitch = PageSections[i].PlatformSwitch,
                            TradeSwitch = PageSections[i].TradeSwitch,
                            IsHeader = PageSections[i].IsHeader,
                            HalfWidth = PageSections[i].HalfWidth,
                            ViewportWidth = PageSections[i].ViewportWidth,
                            Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(PageSections[i].OnDate, PageSections[i].OffDate)).Select(p => p.Name).Single(),
                            PreviewColourways = PageSections[i].PreviewColourways,
                            PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip
                        };
                        PageSection_BannerLocationGroup PSBLLinker = UoW.GetRepository<PageSection_BannerLocationGroup>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).Single();
                        blps.BannerLocationGroup = UoW.GetRepository<BannerLocationGroup>().GetAll().Where(p => p.BannerLocationGroupID == PSBLLinker.BannerLocationGroupID).Single();
                        blps.Locations = UoW.GetRepository<BannerLocation>().GetAll().Where(p => p.BannerLocationGroupID == blps.BannerLocationGroup.BannerLocationGroupID).OrderBy(p => p.BannerLocationID).ToList();
                        blps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        blps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(blps);
                        break;
                    case 9: // Article grid
                        BasePageSection abps = new BasePageSection()
                        {
                            PageSectionID = PageSections[i].PageSectionID,
                            PageSectionName = PageSections[i].Name,
                            OnDate = PageSections[i].OnDate,
                            OffDate = PageSections[i].OffDate,
                            PageID = (int)PageID,
                            PageSectionTypeID = PageSections[i].PageSectionTypeID,
                            ShowFilters = PageSections[i].Filter,
                            SortOrder = PageSections[i].SortOrder,
                            ArticleAutoPopulate = PageSections[i].ArticleAutoPopulate,
                            MultiplePodImages = PageSections[i].MultiplePodImages,
                            Heading = PageSections[i].Heading,
                            PlatformSwitch = PageSections[i].PlatformSwitch,
                            TradeSwitch = PageSections[i].TradeSwitch,
                            IsHeader = PageSections[i].IsHeader,
                            HalfWidth = PageSections[i].HalfWidth,
                            ViewportWidth = PageSections[i].ViewportWidth,
                            Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(PageSections[i].OnDate, PageSections[i].OffDate)).Select(p => p.Name).Single(),
                            PreviewColourways = PageSections[i].PreviewColourways,
                            PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip
                        };
                        abps.CountryLanguageRestrictions = UoW.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).ToList();
                        abps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        abps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(abps);
                        break;
                    case 10:
                        Video_PageSection vbps = new Video_PageSection()
                        {
                            PageSectionID = PageSections[i].PageSectionID,
                            PageSectionName = PageSections[i].Name,
                            OnDate = PageSections[i].OnDate,
                            OffDate = PageSections[i].OffDate,
                            PageID = (int)PageID,
                            PageSectionTypeID = PageSections[i].PageSectionTypeID,
                            ShowFilters = PageSections[i].Filter,
                            SortOrder = PageSections[i].SortOrder,
                            ArticleAutoPopulate = PageSections[i].ArticleAutoPopulate,
                            MultiplePodImages = PageSections[i].MultiplePodImages,
                            Heading = PageSections[i].Heading,
                            PlatformSwitch = PageSections[i].PlatformSwitch,
                            TradeSwitch = PageSections[i].TradeSwitch,
                            IsHeader = PageSections[i].IsHeader,
                            HalfWidth = PageSections[i].HalfWidth,
                            ViewportWidth = PageSections[i].ViewportWidth,
                            Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(PageSections[i].OnDate, PageSections[i].OffDate)).Select(p => p.Name).Single(),
                            PreviewColourways = PageSections[i].PreviewColourways,
                            PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip
                        };
                        var VideoLinker = UoW.GetRepository<PageSection_Video>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).SingleOrDefault();
                        if (VideoLinker != null)
                            vbps.VideoID = VideoLinker.VideoID;
                        vbps.CountryLanguageRestrictions = UoW.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).ToList();
                        if (vbps.VideoID != null)
                            vbps.VideoEmbedCode = UoW.GetRepository<Video>().GetAll().Where(p => p.VideoID == vbps.VideoID).Select(p => p.EmbedCode).Single();
                        vbps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        vbps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(vbps);
                        break;

                    case 11:
                        BasePageSection mbps = new BasePageSection()
                        {
                            PageSectionID = PageSections[i].PageSectionID,
                            PageSectionName = PageSections[i].Name,
                            OnDate = PageSections[i].OnDate,
                            OffDate = PageSections[i].OffDate,
                            PageID = (int)PageID,
                            PageSectionTypeID = 9,
                            ShowFilters = PageSections[i].Filter,
                            SortOrder = PageSections[i].SortOrder,
                            ArticleAutoPopulate = PageSections[i].ArticleAutoPopulate,
                            MultiplePodImages = PageSections[i].MultiplePodImages,
                            Heading = PageSections[i].Heading,
                            PlatformSwitch = PageSections[i].PlatformSwitch,
                            TradeSwitch = PageSections[i].TradeSwitch,
                            IsHeader = PageSections[i].IsHeader,
                            HalfWidth = PageSections[i].HalfWidth,
                            ViewportWidth = PageSections[i].ViewportWidth,
                            Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(PageSections[i].OnDate, PageSections[i].OffDate)).Select(p => p.Name).Single(),
                            PreviewColourways = PageSections[i].PreviewColourways,
                            PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip
                        };
                        mbps.CountryLanguageRestrictions = UoW.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).ToList();
                        mbps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        mbps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(mbps);
                        break;

                    case 12:
                        Animation_PageSection aps = new Animation_PageSection();
                        Animation animation = UoW.GetRepository<Animation>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).Single();
                        aps.Animation = animation;
                        aps.PageSectionName = PageSections[i].Name;
                        aps.PageID = (int)PageID;
                        aps.PageSectionID = PageSections[i].PageSectionID;
                        aps.OnDate = PageSections[i].OnDate;
                        aps.OffDate = PageSections[i].OffDate;
                        aps.PageSectionTypeID = PageSections[i].PageSectionTypeID;
                        aps.SortOrder = PageSections[i].SortOrder;
                        aps.MultiplePodImages = PageSections[i].MultiplePodImages;
                        aps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        aps.Heading = PageSections[i].Heading;
                        aps.PlatformSwitch = PageSections[i].PlatformSwitch;
                        aps.TradeSwitch = PageSections[i].TradeSwitch;
                        aps.IsHeader = PageSections[i].IsHeader;
                        aps.HalfWidth = PageSections[i].HalfWidth;
                        aps.ViewportWidth = PageSections[i].ViewportWidth;
                        aps.Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(aps.OnDate, aps.OffDate)).Select(p => p.Name).Single();
                        aps.CountryLanguageRestrictions = UoW.GetRepository<PageSection_Country_Language>().GetAll().Where(p => p.PageSectionID == PageSections[i].PageSectionID).ToList();
                        aps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        aps.PreviewColourways = PageSections[i].PreviewColourways;
                        aps.PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip;
                        aps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(aps);

                        break;

                    case 13:
                        BasePageSection cubps = new BasePageSection()
                        {
                            PageSectionID = PageSections[i].PageSectionID,
                            PageSectionName = PageSections[i].Name,
                            OnDate = PageSections[i].OnDate,
                            OffDate = PageSections[i].OffDate,
                            PageID = (int)PageID,
                            PageSectionTypeID = PageSections[i].PageSectionTypeID,
                            SortOrder = PageSections[i].SortOrder,
                            MultiplePodImages = PageSections[i].MultiplePodImages,
                            Heading = PageSections[i].Heading,
                            PlatformSwitch = PageSections[i].PlatformSwitch,
                            TradeSwitch = PageSections[i].TradeSwitch,
                            IsHeader = PageSections[i].IsHeader,
                            HalfWidth = PageSections[i].HalfWidth,
                            ViewportWidth = PageSections[i].ViewportWidth,
                            Status = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == UoW.Context.fnc_Status(PageSections[i].OnDate, PageSections[i].OffDate)).Select(p => p.Name).Single(),
                            PreviewColourways = PageSections[i].PreviewColourways,
                            PreviewColourwaysCircularClip = PageSections[i].PreviewColourwaysCircularClip
                        };
                        cubps.IsLifestyleGallery = PageSections[i].IsLifestyleGallery;
                        cubps.TradePartnerAvailability = PageSections[i].TradePartnerAvailability;
                        PVM.PageSections.Add(cubps);
                        break;

                    default: break;
                }
            }

            PVM.RelatedPages = UoW.GetRepository<vwPage_RelatedPage>().GetAll().Where(p => p.PageID == PageID).ToList();

            PVM.PageSections = PVM.PageSections.OrderBy(p => p.SortOrder).ToList();

            return PVM;
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

    public class BasePageSection
    {
        public long PageSectionID { get; set; }
        public byte PageSectionTypeID { get; set; }
        public byte SortOrder { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string PageSectionName { get; set; }

        public DateTime? OnDate { get; set; }
        public DateTime? OffDate { get; set; }
        public int PageID { get; set; }

        public bool MultiplePodImages { get; set; }

        public bool ArticleAutoPopulate { get; set; }

        public bool ShowFilters { get; set; }

        public bool IsLifestyleGallery { get; set; }

        public string Status { get; set; }

        public string Heading { get; set; }

        public bool IsHeader { get; set; }

        public bool? PlatformSwitch { get; set; }

        public bool? TradeSwitch { get; set; }

        public bool HalfWidth { get; set; }

        public bool Paging { get; set; }

        public bool PreviewColourways { get; set; }

        public bool PreviewColourwaysCircularClip { get; set; }

        public byte? ProductTypeID { get; set; }

        public List<PageSection_Country_Language> CountryLanguageRestrictions { get; set; }

        public byte TradePartnerAvailability { get; set; }

        public bool ViewportWidth { get; set; }
    }

    public class Video_PageSection : BasePageSection
    {
        public int? VideoID { get; set; }
        public string VideoEmbedCode { get; set; }
    }

    public class BannerLocation_PageSection : BasePageSection
    {
        public BannerLocationGroup BannerLocationGroup { get; set; }
        public List<BannerLocation> Locations { get; set; }
    }

    public class HTML_PageSection : BasePageSection
    {
        public long HTMLSectionID { get; set; }
        public string Contents { get; set; }
    }

    public class Slideshow_PageSection : BasePageSection
    {
        public long SlideshowID { get; set; }
        public decimal AspectRatio { get; set; }
        public string ARID { get; set; } // aspect ratio id for locking slideshow image input.
        public string ARDescription { get; set; }

        public List<SlideshowImage> Images { get; set; }
    }

    public class Animation_PageSection : BasePageSection
    {
        public Animation Animation { get; set; }
    }

    public class Form_PageSection : BasePageSection
    {
        public long FormID { get; set; }

        public int? RedirectPageID { get; set; }

        public short? PromotionCodeID { get; set; }
        
        [Required]
        public byte? SourceID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 0)]
        public new string Heading { get; set; }

        [StringLength(2000, MinimumLength = 3)]
        public string SubHeading { get; set; }
        
        public short UserSourceID { get; set; }

        public short? CompetitionID { get; set; }
        public short? ThirdPartyID { get; set; }

        public short? BrochureID { get; set; }

        public bool UseCaptcha { get; set; }

        public bool DisplayAddressBlock { get; set; }

        public bool ConfirmEmail { get; set; }

        public Int16? StoreEventID { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime? DeletedDT { get; set; }
        public bool? PhoneNumberRequired { get; set; }
    }

    public struct PageSectionInformation
    {
        public long PageSectionID;
        public byte SortOrder;
    };


    public class PageComposeViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int? PageID { get; set; }

        public bool Clearance { get; set; }

        public byte? ProductTypeID { get; set; }

        public ProductType ProductType { get; set; }

        public bool EnableFilterIndexing { get; set; }

        [Display(Name = "Internal Name")]
        [StringLength(100, MinimumLength=3)]
        public string Name { get; set; }

        [Display(Name = "Meta Description")]
        [StringLength(500, MinimumLength=3)]
        public string MetaDescription { get; set; }

        [Display(Name = "Description")]
        [StringLength(1200, MinimumLength = 3)]
        public string Description { get; set; }
        
        [Display(Name = "Title")]
        [StringLength(70, MinimumLength=3)]
        public string TitleTag { get; set; }
        
        [Display(Name = "Heading")]
        [StringLength(70)]
        public string Heading { get; set; }

        [Display(Name = "Sub Heading")]
        [StringLength(100)]
        public string SubHeading { get; set; }

        [StringLength(100)]
        [RegularExpression("^([a-z]|[a-z]-|[a-z]/)+[a-z]$", ErrorMessage = @"Regular Expression Validation Failed.
    URLs must not begin or end with a slash.
    URL segments must not contain accented characters, such as é or ë.
    URL segments may only contain the lower case letters 'a' through 'z', as well as hyphens.
    URL segments must not begin or end with a hyphen.")]
        public string URLKeywords { get; set; }

        public List<Page_Image> Images { get; set; }

        public PageSearchHeader PageSearchHeader { get; set; }

        public List<BasePageSection> PageSections { get; set; }

        public List<Country> CountryRestrictions { get; set; }

        public List<vwPage_RelatedPage> RelatedPages { get; set; }

        public DateTime? DeletedDT { get; set; }

        public Byte SiteSectionID { get; set; }

        public bool? New { get; set; }

        public String RelatedFilterFontColour { get; set; }
        public bool RelatedFilterBold { get; set; }
        public bool CalculateDiscount { get; set; }
        public bool ShowRecentlyViewed { get; set; }

        public bool NoIndex { get; set; }
        public bool NoFollow { get; set; }
        public int? CanonicalPageID { get; set; }
        public Page CanonicalPage { get; set; }
        public List<AttributeStub> AttributeList { get; set; }
        public List<Flock.Models.Attribute> Attributes { get; set; }
        public string TitleTagSuffix { get; set; }

        public PageComposeViewModel()
        {
            var uow = new UnitOfWork<FlockDataContext>();

            TitleTagSuffix = uow.GetRepository<Country_Language>().GetAll().Single(c => c.Country_Language_ID == 3).TitleTagSuffix;
        }
    }
}