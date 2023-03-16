using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class AdminViewModelBuilder : ViewModelBuilder, IViewModelBuilder<AdminViewModel>
    {
        public AdminViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public AdminViewModel Build(long? id)
        {
            AdminViewModel AVM = new AdminViewModel();
            AVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();

            return AVM;
        }
    }

    public class AdminViewModel : BaseViewModel
    {
    }
}