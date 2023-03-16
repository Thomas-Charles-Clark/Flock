using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class CalendarViewModelBuilder : ViewModelBuilder, IViewModelBuilder<CalendarViewModel>
    {
        public CalendarViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public CalendarViewModel Build(long? id)
        {
            CalendarViewModel CVM = new CalendarViewModel();
            CVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();

            return CVM;
        }
    }

    public class CalendarViewModel : BaseViewModel
    {
    }
}