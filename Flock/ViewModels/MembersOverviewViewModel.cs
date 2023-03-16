using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class MembersOverviewViewModelBuilder : ViewModelBuilder, IViewModelBuilder<MembersOverviewViewModel>
    {
        public MembersOverviewViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public MembersOverviewViewModel Build(long? id)
        {
            MembersOverviewViewModel MOVM = new MembersOverviewViewModel();
            MOVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();

            return MOVM;
        }
    }

    public class MembersOverviewViewModel : BaseViewModel
    {
    }
}