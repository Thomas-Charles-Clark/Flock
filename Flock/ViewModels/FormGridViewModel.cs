using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class FormGridViewModelBuilder : ViewModelBuilder, IViewModelBuilder<FormGridViewModel>
    {
        public FormGridViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public FormGridViewModel Build(long? id)
        {
            FormGridViewModel  FGVM = new FormGridViewModel();

            FGVM.Forms = UoW.GetRepository<Form>().GetAll().ToList();

            FGVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();

            return FGVM;
        }
    }

    public class FormGridViewModel : BaseViewModel
    {
        public List<Form> Forms { get; set; }
    }
}