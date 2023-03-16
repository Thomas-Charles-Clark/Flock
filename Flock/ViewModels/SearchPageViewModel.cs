using Flock.Models;
using Flock.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class SearchPageViewModelBuilder : ViewModelBuilder
    {
        public SearchPageViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public SearchPageViewModel Build(long? PageID, short? countryId, byte? languageId)
        {
            PageComposeViewModel PVM = new PageComposeViewModel();

            PageViewModelBuilder Builder = new PageViewModelBuilder(this.UoW);
            PVM = Builder.Build(Convert.ToInt32(PageID), countryId, languageId);
            SearchPageViewModel SPVM = new SearchPageViewModel();
            SPVM.PVM = PVM;
            return SPVM;
        }
    }

    public class SearchPageViewModel : BaseViewModel
    {
        public PageComposeViewModel PVM { get; set; }
    }
}