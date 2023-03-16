using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class CollectionTranslateViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public CollectionTranslateViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public CollectionTranslateViewModel Build(short? CollectionID, byte? LanguageID)
        {
            CollectionTranslateViewModel Model = new CollectionTranslateViewModel();

            if (CollectionID != null)
            {
                Collection original = UoW.GetRepository<Collection>().GetAll().Where(p => p.CollectionID == CollectionID).Single();

                Model.Heading = original.Heading;
                Model.SubHeading = original.SubHeading;
                Model.Description = original.Description;
                Model.TitleTag = original.TitleTag;
                Model.URLKeywords = original.URLKeywords;
                Model.MetaDescription = original.MetaDescription;
                Model.Name = original.Name;
                Model.ImageID = UoW.GetRepository<Collection_Image>().GetAll().Where(p => p.CollectionID == CollectionID && p.TemplateImageID == 1 && p.SortOrder == 0).Select(p => p.ImageID).SingleOrDefault();
                Model.ID = original.CollectionID;

                if (LanguageID != null)
                {
                    Collection_Language translation = UoW.GetRepository<Collection_Language>().GetAll().Where(p => p.CollectionID == CollectionID && p.LanguageID == LanguageID).Single();
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

    public class BaseTranslateViewModel : BaseViewModel
    {
        public byte? SelectedTargetLanguageID { get; set; }
        public short? SelectedTargetLanguageCountryID { get; set; }
        public string SelectedTargetLanguage { get; set; }

        public long? ID { get; set; }

        public string Name { get; set; }

        public long? ImageID { get; set; }

        [StringLength(70)]
        public string Heading { get; set; }

        [StringLength(70)]
        public string SubHeading { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(70)]
        public string TitleTag { get; set; }

        [StringLength(100)]
        [RegularExpression("^([a-z]|[a-z]-|[a-z]/)+[a-z]$", ErrorMessage = @"Regular Expression Validation Failed.
    URLs must not begin or end with a slash.
    URL segments must not contain accented characters, such as é or ë.
    URL segments may only contain the lower case letters 'a' through 'z', as well as hyphens.
    URL segments must not begin or end with a hyphen.")]
        public string URLKeywords { get; set; }

        [StringLength(155)]
        public string MetaDescription { get; set; }

        [StringLength(70)]
        public string HeadingTranslation { get; set; }

        [StringLength(70)]
        public string SubHeadingTranslation { get; set; }

        [StringLength(2000)]
        public string DescriptionTranslation { get; set; }

        [StringLength(70)]
        public string TitleTagTranslation { get; set; }

        [StringLength(100)]
        [RegularExpression("^([a-z]|[a-z]-|[a-z]/)+[a-z]$", ErrorMessage = @"Regular Expression Validation Failed.
    URLs must not begin or end with a slash.
    URL segments must not contain accented characters, such as é or ë.
    URL segments may only contain the lower case letters 'a' through 'z', as well as hyphens.
    URL segments must not begin or end with a hyphen.")]
        public string URLKeywordsTranslation { get; set; }

        [StringLength(155)]
        public string MetaDescriptionTranslation { get; set; }
    }

    public class CollectionTranslateViewModel : BaseTranslateViewModel
    {
        public CollectionTranslateViewModelHeader HeaderFilters { get; set; }

        public string WebStatus { get; set; }

    }

    public class CollectionTranslateViewModelHeader
    {
        public byte? ProductTypeID { get; set; }
        public byte? BrandID { get; set; }
        public short? LaunchID { get; set; }
        public int? ID { get; set; }
        public byte? WebStatusID { get; set; }
        public byte? TranslationStatusID { get; set; }
        public byte? TargetLanguageID { get; set; }
    }
}