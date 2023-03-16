using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class AttributeAssignmentViewModelBuilder : ViewModelBuilder, IViewModelBuilder<AttributeAssignmentViewModel>
    {
        public AttributeAssignmentViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public AttributeAssignmentViewModel Build(long? ID)
        {
            AttributeAssignmentViewModel Model = new AttributeAssignmentViewModel();

            ProductPage PP = UoW.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == ID).Single();

            Model.IsProductPage = true;
            Model.ID = (int?)ID;
            Model.ProductTypeID = PP.ProductTypeID;
            Model.BrandID = PP.BrandID;
            Model.LaunchID = PP.LaunchID;
            Model.ColourID = PP.ColourID;
            Model.PatternID = PP.PatternID;
            Model.Name = PP.Name;
            Model.Heading = PP.Heading;
            Model.SubHeading = PP.SubHeading;
            Model.Description = PP.Description;
            Model.ImageID = UoW.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ID && p.TemplateImageID == 1 && p.SortOrder == 0).Select(p => p.ImageID).SingleOrDefault();
            Model.Products = UoW.GetRepository<Product>().GetAll().Where(p => p.ProductPageID == ID && p.DeletedDT == null).ToList();
            Model.AttributeGroups = new List<AttributeGroupViewModel>();
            Model.ShowHotspots = false;

            var ppi = UoW.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ID && p.TemplateImageID == 2).Select(p => new { p.ImageID, p.SortOrder });
            var ids = ppi.Select(p => p.ImageID);
            var ss = UoW.GetRepository<Image>().GetAll().Where(p => ids.Contains(p.ImageID)).ToList();

            Model.SlideshowImages = new List<SlideshowImage>();

            var quality = UoW.Context.ProductPage_Quality_Select((int)ID).SingleOrDefault();
            if (quality != null)
            {
                Model.QualityID = quality.QualityID;
                Model.Code = quality.Code;
                Model.Width = quality.Width;
                Model.Weight = quality.Weight;
                Model.HPatternRepeat = quality.HorizontalPatternRepeat;
                Model.VPatternRepeat = quality.VerticalPatternRepeat;
                Model.Martindale = quality.Martindale;
                Model.Composition = quality.Composition;
            }

            foreach (var i in ppi)
            {
                SlideshowImage ssi = new SlideshowImage();
                ssi.Record = new SlideShow_Image() { ImageID = i.ImageID, SortOrder = i.SortOrder, SlideShowID = (long)ID };
                ssi.Hotspots = UoW.Context.ImageHotspot_Select(i.ImageID, 1, DateTime.Now, true).ToList();
                Model.SlideshowImages.Add(ssi);
            }

            if (ss.Count > 0)
                Model.AspectRatio = Convert.ToDecimal(ss[0].AspectRatio);


            var groups = UoW.Context.AttributeGroup_Select(PP.ProductTypeID).ToList();

            foreach (var group in groups)
            {
                var available = UoW.Context.Attribute_Select(Model.ProductTypeID, group.AttributeGroupID).OrderBy(p => p.SortOrder).ToList();
                var assigned = UoW.Context.ProductPage_Attribute_Select((int)ID).ToList();

                Model.AttributeGroups.Add(new AttributeGroupViewModel()
                {
                    AttributeGroupID = group.AttributeGroupID,
                    Name = group.Name,
                    Filter = group.Filter,
                    Info = group.Info,
                    IsMultipleSelect = group.MultipleSelect,
                    SortOrder = group.SortOrder,
                    Options = available,
                    SelectedValues = assigned.Select(p => p.AttributeID).ToList(),
                    AutoSync = group.AutoSync
                });
            }

            Model.AttributeGroups = Model.AttributeGroups.OrderBy(p => p.SortOrder).ToList();


            return Model;
        }
    }

    public class AttributeAssignmentViewModel : BaseViewModel
    {
        public int? ID { get; set; }
        public List<AttributeGroupViewModel> AttributeGroups { get; set; }
        public bool IsProductPage { get; set; } // if not, you're looking at a product 

        public byte? ProductTypeID { get; set; }
        public byte? BrandID { get; set; }
        public short? LaunchID { get; set; }
        public byte? ColourID { get; set; }
        public byte? PatternID { get; set; }
        public int? DesignID { get; set; }

        public string Name { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Description { get; set; }
        public long? ImageID { get; set; }
        public List<SlideshowImage> SlideshowImages { get; set; }
        public List<Product> Products { get; set; }

        public bool ShowHotspots { get; set; }
        public decimal AspectRatio { get; set; }

        public short? QualityID { get; set; }
        public string Code { get; set; }
        public double? Width { get; set; }
        public double? Weight { get; set; }
        public double? HPatternRepeat { get; set; }
        public double? VPatternRepeat { get; set; }
        public int? Martindale { get; set; }
        public string Composition { get; set; }
    }
}