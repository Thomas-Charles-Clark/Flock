using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class ProductPageComposeViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductPageComposeViewModel>
    {
        public ProductPageComposeViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ProductPageComposeViewModel Build(long? ProductPageID)
        {
            ProductPageComposeViewModel PPCVM = new ProductPageComposeViewModel();
            ProductPage PP = UoW.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == Convert.ToInt32(ProductPageID)).Single();
            PPCVM.ProductPage = PP;
            List<short> CountryRestrictionLinker = UoW.GetRepository<ProductPage_Country>().GetAll().Where(p => p.ProductPageID == ProductPageID).Select(p => p.CountryID).ToList();
            PPCVM.CountryRestrictions = UoW.GetRepository<Country>().GetAll().Where(p => CountryRestrictionLinker.Contains(p.CountryID)).ToList();
            PPCVM.TradeSwitch = PP.TradeSwitch;
            PPCVM.ProductPageID = PP.ProductPageID;
            PPCVM.TemplateID = PP.TemplateID;
            PPCVM.Name = PP.Name;
            if (PP.BrandID != 0)
            {
                PPCVM.BrandID = PP.BrandID;
                PPCVM.Brand = UoW.GetRepository<Brand>().GetAll().Where(p => p.BrandID == PP.BrandID).Select(p => p.Name).Single();
            }
            else
                PPCVM.Brand = "BRAND NOT SET";
            if (PP.ProductTypeID != 0)
                PPCVM.ProductTypeName = UoW.GetRepository<ProductType>().GetAll().Where(p => p.ProductTypeID == PP.ProductTypeID).Select(p => p.Name).Single();
            else
                PPCVM.ProductTypeName = "PRODUCT TYPE NOT SET";

            PPCVM.TemplateID = PP.TemplateID;
            PPCVM.TemplateName = UoW.GetRepository<Flock.Models.Template>().GetAll().Where(p => p.TemplateID == PP.TemplateID).Select(p => p.Name).Single();
            if (PP.ProductTypeID != 0)
                PPCVM.ProductTypeID = PP.ProductTypeID;
            if (PP.LaunchID != 0)
                PPCVM.LaunchID = PP.LaunchID;
            PPCVM.New = PP.New;
            PPCVM.Heading = PP.Heading;
            PPCVM.SubHeading = PP.SubHeading;
            PPCVM.Description = PP.Description;
            PPCVM.TitleTag = PP.TitleTag;
            PPCVM.URLKeywords = PP.URLKeywords;
            PPCVM.MetaDescription = PP.MetaDescription;
            PPCVM.OnDate = PP.OnDate;
            PPCVM.Clearance = PP.Clearance;
            PPCVM.OffDate = PP.OffDate;
            PPCVM.WebStatusID = UoW.Context.fnc_Status(PP.OnDate, PP.OffDate);
            PPCVM.WebStatus = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == PPCVM.WebStatusID).Select(p => p.Name).Single();
            PPCVM.FurnitureDimensions = PP.FurnitureDimensions;
            //PPCVM.FurnitureCutOut = PP.FurnitureCutOut;
            PPCVM.FurnitureFabricOptions = PP.FurnitureFabricOptions;
            PPCVM.FurnitureLeatherOptions = PP.FurnitureLeatherOptions;
            PPCVM.FurnitureLegStyles = PP.FurnitureLegStyles;
            PPCVM.FurnitureLegFinishes = PP.FurnitureLegFinishes;
            PPCVM.FurnitureStitchingOptions = PP.FurnitureStitchingOptions;
            PPCVM.FurniturePDF = PP.FurniturePDF;
            PPCVM.BrochureID = PP.BrochureID;
            PPCVM.Images = UoW.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID).OrderBy(p => p.SortOrder).ToList();
            PPCVM.VideoURL = PP.VideoURL;
            PPCVM.VideoThumbnail = PP.VideoThumbnail;
            PPCVM.PrimaryProducts = UoW.GetRepository<vwProductPage_PrimaryProduct>().GetAll().Where(p => p.ProductPageID == (int)ProductPageID).OrderBy(p => p.SortOrder).ToList();
            PPCVM.SecondaryProducts = UoW.GetRepository<vwProductPage_SecondaryProduct>().GetAll().Where(p => p.ProductPageID == (int)ProductPageID).OrderBy(p => p.SortOrder).ToList();
            PPCVM.RelatedProductPages = UoW.GetRepository<vwProductPage_RelatedProductPage>().GetAll().Where(p => p.ProductPageID == (int)ProductPageID).ToList();

            PPCVM.AlternateColourwayImageID = PP.AlternateColourwayImageID;

            if (PPCVM.AlternateColourwayImageID.HasValue) 
            {
                PPCVM.AlternateColourwayImage = UoW.GetRepository<Image>().GetAll().SingleOrDefault(p => p.ImageID == PP.AlternateColourwayImageID);
            }

            return PPCVM;
        }
    }

    public class ProductPageComposeViewModel : BaseViewModel 
    {
        public int? ProductPageID { get; set; }
        public byte? TemplateID { get; set; }
        public string TemplateName { get; set; }

        [Required]
        [Display(Name = "Name (100)")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Heading (70)")]
        [StringLength(70)]
        public string Heading { get; set; }

        [Display(Name = "Sub Heading (70)")]
        [StringLength(70)]
        public string SubHeading { get; set; }

        [Display(Name = "Description (3000)")]
        [StringLength(3000)]
        public string Description { get; set; }

        public string Brand { get; set; }
        public byte? BrandID { get; set; }
        public string ProductTypeName { get; set; }
        public byte? ProductTypeID { get; set; }
        public string LaunchName { get; set; }
        public short? LaunchID { get; set; }
        public string DesignName { get; set; }
        public int? DesignID { get; set; }
        public string PatternName { get; set; }
        public byte? PatternID { get; set; }
        public short? BrochureID { get; set; }
        public byte? WebStatusID { get; set; }
        public string WebStatus { get; set; }

        public string FurnitureDimensions { get; set; }
        //public string FurnitureCutOut { get; set; }

        public bool? FurnitureFabricOptions { get; set; }
        public bool? FurnitureLeatherOptions { get; set; }
        public bool? FurnitureLegStyles { get; set; }
        public bool? FurnitureLegFinishes { get; set; }
        public bool? FurnitureStitchingOptions { get; set; }
        public string FurniturePDF { get; set; }

        [Display(Name = "Video URL (100)")]
        [StringLength(100)]
        public string VideoURL { get; set; }

        [Display(Name = "Video Thumbnail")]
        [StringLength(100)]
        public string VideoThumbnail { get; set; }

        [Display(Name = "Title (70)")]
        [StringLength(70, MinimumLength = 3)]
        public string TitleTag { get; set; }

        [Display(Name = "URLKeywords (100)")]
        [StringLength(100)]
        [RegularExpression("^([a-z]|[a-z]-|[a-z]/)+[a-z]$", ErrorMessage = @"Regular Expression Validation Failed.
    URLs must not begin or end with a slash.
    URL segments must not contain accented characters, such as é or ë.
    URL segments may only contain the lower case letters 'a' through 'z', as well as hyphens.
    URL segments must not begin or end with a hyphen.")]
        public string URLKeywords { get; set; }

        [Display(Name = "Meta Description (155)")]
        [StringLength(155)]
        public string MetaDescription { get; set; }

        public long? AlternateColourwayImageID { get; set; }

        public DateTime? OnDate { get; set; }
        public DateTime? OffDate { get; set; }
        public bool Clearance { get; set; }
        public bool New { get; set; }
        public ProductSearchHeader HeaderFilters { get; set; }
        public bool? TradeSwitch { get; set; }

        public ProductPage ProductPage { get; set; }
        public List<Country> CountryRestrictions { get; set; }
        public List<ProductPage_Image> Images { get; set; }
        public List<vwProductPage_PrimaryProduct> PrimaryProducts { get; set; }
        public List<vwProductPage_SecondaryProduct> SecondaryProducts { get; set; }
        public List<vwProductPage_RelatedProductPage> RelatedProductPages { get; set; }

        public Image AlternateColourwayImage { get; set; }
    }

    public class ProductSearchHeader
    {
        public long? ID { get; set; }
        public byte? ProductTypeID { get; set; }
        public byte? BrandID { get; set; }
        public short? LaunchID { get; set; }
        public byte? WebStatusID { get; set; }
        public byte? TemplateID { get; set; }
    }
}