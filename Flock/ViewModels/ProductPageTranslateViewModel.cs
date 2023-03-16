using DevExpress.Web;
using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class ProductPageTranslateViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public ProductPageTranslateViewModelBuilder(UnitOfWork<FlockDataContext> UnitOfWork)
        {
            UoW = UnitOfWork;
        }

        public ProductPageTranslateViewModel Build(int? ProductPageID, byte? LanguageID)
        {
            ProductPageTranslateViewModel Model = new ProductPageTranslateViewModel();

            if (ProductPageID != null)
            {
                ProductPage original = UoW.GetRepository<ProductPage>().GetAll().Where(p => p.ProductPageID == ProductPageID).Single();

                Model.Heading = original.Heading;
                Model.SubHeading = original.SubHeading;
                Model.Description = original.Description;
                Model.TitleTag = original.TitleTag;
                Model.URLKeywords = original.URLKeywords;
                Model.MetaDescription = original.MetaDescription;
                Model.Name = original.Name;
                Model.ImageID = UoW.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.TemplateImageID == 1 && p.SortOrder == 0).Select(p => p.ImageID).SingleOrDefault();
                Model.ID = original.ProductPageID;

                if (LanguageID != null)
                {
                    ProductPage_Language translation = UoW.GetRepository<ProductPage_Language>().GetAll().Where(p => p.ProductPageID == ProductPageID && p.LanguageID == LanguageID).Single();

                    Model.SelectedTargetLanguage = UoW.GetRepository<Language>().GetAll().Where(p => p.LanguageID == translation.LanguageID).Select(p => p.Name).Single();
                    Model.HeadingTranslation = translation.Heading;
                    Model.SubHeadingTranslation = translation.SubHeading;
                    Model.DescriptionTranslation = translation.Description;
                    Model.TitleTagTranslation = translation.TitleTag;
                    Model.URLKeywordsTranslation = translation.URLKeywords;
                    Model.MetaDescriptionTranslation = translation.MetaDescription;
                    Model.SelectedTargetLanguageID = LanguageID;
                }

                return Model;
            }
            else
            {
                return Model;
            }
        }
    }

    public class ProductPageTranslateViewModel : BaseTranslateViewModel
    {
        public ProductPageTranslateViewModelHeader HeaderFilters { get; set; }

        public string WebStatus { get; set; }
    }

    public class ProductPageTranslateViewModelHeader
    {
        public byte? ProductTypeID { get; set; }
        public byte? BrandID { get; set; }
        public short? LaunchID { get; set; }
        public byte? TemplateID { get; set; }
        public int? ID { get; set; }
        public byte? WebStatusID { get; set; }
        public byte? TranslationStatusID { get; set; }
        public byte? TargetLanguageID { get; set; }
    }
}