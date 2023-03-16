using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.ViewModels;
using Flock.Models;
using Flock.DAL;

namespace Flock.ViewModels
{
    public class PageTranslateViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public PageTranslateViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public PageTranslateViewModel Build(int? PageID, byte? LanguageID)
        {
            PageTranslateViewModel Model = new PageTranslateViewModel();

            if (PageID != null)
            {
                Page original = UoW.GetRepository<Page>().GetAll().Where(p => p.PageID == PageID).Single();

                Model.Heading = original.Heading;
                Model.SubHeading = original.SubHeading;
                Model.TitleTag = original.TitleTag;
                Model.URLKeywords = original.URLKeywords;
                Model.MetaDescription = original.MetaDescription;
                Model.Name = original.Name;
                Model.ImageID = UoW.GetRepository<ProductPage_Image>().GetAll().Where(p => p.ProductPageID == PageID && p.TemplateImageID == 1 && p.SortOrder == 0).Select(p => p.ImageID).SingleOrDefault();
                Model.ID = original.PageID;

                if (LanguageID != null)
                {
                    Page_Language translation = UoW.GetRepository<Page_Language>().GetAll().Where(p => p.PageID == PageID && p.LanguageID == LanguageID).Single();

                    Model.SelectedTargetLanguage = UoW.GetRepository<Language>().GetAll().Where(p => p.LanguageID == translation.LanguageID).Select(p => p.Name).Single();
                    Model.HeadingTranslation = translation.Heading;
                    Model.SubHeadingTranslation = translation.SubHeading;
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

    public class PageTranslateViewModel : BaseTranslateViewModel
    {
        public PageTranslateViewModelHeader HeaderFilters { get; set; }
    }

    public class PageTranslateViewModelHeader
    {
        public byte? SiteSectionID { get; set; }
        public int? ParentPageID { get; set; }
        public int? ID { get; set; }
        public byte? TargetLanguageID { get; set; }
        public byte? TranslationStatusID { get; set; }
        public string WebStatus { get; set; }
    }
}