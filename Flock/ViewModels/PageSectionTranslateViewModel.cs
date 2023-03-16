using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.ViewModels;
using Flock.Models;
using Flock.DAL;

namespace Flock.ViewModels
{
    public class PageSectionTranslateViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public PageSectionTranslateViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public PageSectionTranslateViewModel Build(long? PageSectionID, byte? LanguageID)
        {
            PageSectionTranslateViewModel Model = new PageSectionTranslateViewModel();

            if (PageSectionID != null)
            {
                PageSection_HTMLSection original = UoW.GetRepository<PageSection_HTMLSection>().GetAll().Where(p => p.PageSectionID == PageSectionID).Single();
                HTMLSection target = UoW.GetRepository<HTMLSection>().GetAll().Where(p => p.HTMLSectionID == original.HtmlSectionID).Single();

                Model.Description = target.Contents;

                Model.ID = original.HtmlSectionID;

                if (LanguageID != null)
                {
                    HTMLSection_Language translation = UoW.GetRepository<HTMLSection_Language>().GetAll().Where(p => p.HTMLSectionID == original.HtmlSectionID && p.LanguageID == LanguageID).Single();

                    Model.SelectedTargetLanguage = UoW.GetRepository<Language>().GetAll().Where(p => p.LanguageID == translation.LanguageID).Select(p => p.Name).Single();
                    Model.DescriptionTranslation = translation.Contents;
                }

                return Model;
            }
            else
            {
                return Model;
            }
        }
    }

    public class PageSectionTranslateViewModel : BaseTranslateViewModel
    {
        public PageSectionTranslateViewModelHeader HeaderFilters { get; set; }
    }

    public class PageSectionTranslateViewModelHeader
    {
        public byte? SiteSectionID { get; set; }
        public int? PageID { get; set; }
        public int? ID { get; set; }
        public byte? TargetLanguageID { get; set; }
        public byte? TranslationStatusID { get; set; }
        public string WebStatus { get; set; }
    }
}