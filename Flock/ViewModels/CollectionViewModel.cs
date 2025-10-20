using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class CollectionViewModelBuilder : ViewModelBuilder, IViewModelBuilder<CollectionViewModel>
    {
        public CollectionViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public CollectionViewModel Build(long? CollectionID)
        {
            CollectionViewModel CVM = new CollectionViewModel();
            CVM.CollectionID = Convert.ToInt16(CollectionID);
            Collection Coll = UoW.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == (long)CollectionID).Single();
            CVM.Name = Coll.Name;
            CVM.Description = Coll.Description;
            CVM.MetaDescription = Coll.MetaDescription;
            CVM.URLKeywords = Coll.URLKeywords;
            CVM.TitleTag = Coll.TitleTag;
            CVM.Code = Coll.Code;
            CVM.PatternBookCode = Coll.PBCode;
            CVM.VideoID = Coll.VideoID;
            CVM.BrochureID = Coll.BrochureID;
            Brand thisBrand = UoW.GetRepository<Brand>().GetAll().Where(p => p.BrandID == Coll.BrandID).SingleOrDefault();
            CVM.BrandID = 0;
            if(thisBrand != null)
            {
                CVM.BrandName = thisBrand.Name;
                CVM.BrandID = thisBrand.BrandID;
            }

            CVM.Season = "Not Set";
            CVM.LaunchID = 0;
            Launch thisLaunch = UoW.GetRepository<Launch>().GetAll().Where(p => p.LaunchID == Coll.LaunchID).SingleOrDefault();
            if (thisLaunch != null)
            {
                CVM.LaunchID = thisLaunch.LaunchID;
                CVM.Season = UoW.GetRepository<Season>().GetAll().Where(p => p.SeasonID == thisLaunch.SeasonID).Select(s => s.Name).SingleOrDefault();
            }
            CVM.Heading = Coll.Heading;
            CVM.SubHeading = Coll.SubHeading;
            CVM.Launches = UoW.GetRepository<Launch>().GetAll().ToList();
            CVM.Brands = UoW.GetRepository<Brand>().GetAll().ToList();
            CVM.CollectionImages = UoW.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == Convert.ToInt16(CollectionID)).OrderBy(p => p.SortOrder).ToList();
            CVM.Colours = UoW.GetRepository<Colour>().GetAll().ToList();
            CVM.New = Coll.New;
            CVM.OnDate = Coll.OnDate;
            CVM.OffDate = Coll.OffDate;
            CVM.WebStatusID = UoW.Context.fnc_Status(Coll.OnDate, Coll.OffDate);
            CVM.WebStatus = UoW.GetRepository<Status>().GetAll().Where(p => p.StatusID == CVM.WebStatusID).Select(p => p.Name).Single();

            CVM.PreviewColourways = Coll.PreviewColourways;

            CVM.RelatedCollections = UoW.GetRepository<vwCollection_RelatedCollection>().GetAll().Where(p => p.CollectionID == CollectionID).ToList();

            CVM.CountryRestrictions = UoW.GetRepository<Collection_Country>().GetAll().Where(p => p.CollectionID == CollectionID).Select(p => p.Country).ToList();

            return CVM;
        }
    }

    public class CollectionInfoViewModel
    {
        [Required]
        [DisplayName("Name (100)")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [DisplayName("Pattern Book Code (20)")]
        [StringLength(20)]
        public string PatternBookCode { get; set; }

        [DisplayName("Code")]
        [StringLength(20)]
        public string Code { get; set; }

        public short? CollectionID { get; set; }

        public DateTime? OnDate { get; set; }
        public DateTime? OffDate { get; set; }

        public byte? WebStatusID { get; set; }
        public string WebStatus { get; set; }

        public bool New { get; set; }

        public short LaunchID { get; set; }
        public byte BrandID { get; set; }
        public bool PreviewColourways { get; set; }
    }

    public class CollectionDetailsAndPodsViewModel
    {
        public short? CollectionID { get; set; }
        [DisplayName("Meta Description (155)")]
        [StringLength(155, MinimumLength = 3)]
        public string MetaDescription { get; set; }

        [Required]
        [DisplayName("URL Keywords (100)")]
        [StringLength(100, MinimumLength = 3)]
        public string URLKeywords { get; set; }

        [DisplayName("Title (70)")]
        [StringLength(70, MinimumLength = 3)]
        public string TitleTag { get; set; }

        public List<Collection_Image> CollectionImages { get; set; }
    }

    public class CollectionTextAndSlideshowViewModel
    {
        public short? CollectionID { get; set; }
        [DisplayName("Heading (70)")]
        [StringLength(70, MinimumLength = 3)]
        public string Heading { get; set; }

        [DisplayName("Sub Heading (70)")]
        [StringLength(150, MinimumLength = 3)]
        public string SubHeading { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public List<Collection_Image> CollectionImages { get; set; }
    }

    public class CollectionViewModel : BaseViewModel
    {
        public short? CollectionID { get; set; }
        public byte TemplateID { get; set; }

        [DisplayName("Pattern Book Code (20)")]
        [StringLength(20)]
        public string PatternBookCode { get; set; }

        [DisplayName("Code")]
        [StringLength(20)]
        public string Code { get; set; }

        [Required]
        [DisplayName("Name (100)")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [DisplayName("Meta Description (155)")]
        [StringLength(155, MinimumLength=3)]
        public string MetaDescription { get; set; }

        [Required]
        [DisplayName("URL Keywords (100)")]
        [StringLength(100,MinimumLength=3)]
        public string URLKeywords { get; set; }

        [DisplayName("Title (70)")]
        [StringLength(70, MinimumLength=3)]
        public string TitleTag { get; set; }

        [DisplayName("Heading (70)")]
        [StringLength(70, MinimumLength=3)]
        public string Heading { get; set; }

        [DisplayName("Sub Heading (70)")]
        [StringLength(150, MinimumLength=3)]
        public string SubHeading { get; set; }

        [DisplayName("Brand")]
        public byte BrandID { get; set; }
        public string BrandName { get; set; }
        
        [DisplayName("Launch")]
        public short LaunchID { get; set; }

        public long? Spacer1ImageID { get; set; }
        public long? Spacer2ImageID { get; set; }
        public long? ViewAllImageID { get; set; }
        public DateTime? OnDate { get; set; }
        public DateTime? OffDate { get; set; }

        public byte? WebStatusID { get; set; }
        public string WebStatus { get; set; }
        public bool New { get; set; }
        public short? VideoID { get; set; }
        public short? BrochureID { get; set; }
        public string Season { get; set; }
        public string Description { get; set; }

        public bool PreviewColourways { get; set; }

        public List<Collection_Image> CollectionImages { get; set; }
        public List<vwCollection_RelatedCollection> RelatedCollections { get; set; }

        public CollectionHeaderFilters HeaderFilters { get; set; }

        public List<Country> CountryRestrictions { get; set; }

        public string TitleTagSuffix { get; set; }

        public CollectionViewModel()
        {
            var uow = new UnitOfWork<FlockDataContext>();

            TitleTagSuffix = uow.GetRepository<Country_Language>().GetAll().Single(c => c.Country_Language_ID == 3).TitleTagSuffix;
        }
    }

    public class CollectionHeaderFilters
    {
        public short? CollectionID { get; set; }
        public byte? ProductTypeID { get; set; }
        public byte? BrandID { get; set; }
        public short? LaunchID { get; set; }
    }
}