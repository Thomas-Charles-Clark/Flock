using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class MenuBuilderViewModelBuilder : ViewModelBuilder, IViewModelBuilder<MenuBuilderViewModel>
    {
        public MenuBuilderViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public MenuBuilderViewModel Build(long? id)
        {
            MenuBuilderViewModel MBVM = new MenuBuilderViewModel();
            MBVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();

            return MBVM;
        }
    }

    public class MenuBuilderViewModel : BaseViewModel
    {

    }
}