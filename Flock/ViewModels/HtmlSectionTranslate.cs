using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.ViewModels;
using Flock.Models;
using Flock.DAL;

namespace Flock.ViewModels
{
    public class HTMLSectionTranslateViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public HTMLSectionTranslateViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public HTMLSectionTranslateViewModel Build(long? HtmlSectionID, byte? LanguageID)
        {
            HTMLSectionTranslateViewModel Model = new HTMLSectionTranslateViewModel();

            if (HtmlSectionID != null)
            {
                HTMLSection original = UoW.GetRepository<HTMLSection>().GetAll().Where(p => p.HTMLSectionID == HtmlSectionID).Single();
                PageSection_HTMLSection PSHS = UoW.GetRepository<PageSection_HTMLSection>().GetAll().Where(p => p.HtmlSectionID == HtmlSectionID).Single();
                PageSection PS = UoW.GetRepository<PageSection>().GetAll().Where(p => p.PageSectionID == PSHS.PageSectionID).Single();
                Model.ID = HtmlSectionID;
                Model.Description = original.Contents;
                Model.Name = PS.Name;
               
                if (LanguageID != null)
                {
                    HTMLSection_Language translation = UoW.GetRepository<HTMLSection_Language>().GetAll().Where(p => p.HTMLSectionID == PSHS.HtmlSectionID && p.LanguageID == LanguageID).Single();

                    Model.DescriptionTranslation = translation.Contents;
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

    public class HTMLSectionTranslateViewModel : BaseTranslateViewModel
    {

    }
}