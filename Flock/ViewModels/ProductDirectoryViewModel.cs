using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductDirectoryViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductDirectoryViewModel>
    {
        public ProductDirectoryViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ProductDirectoryViewModel Build(long? id)
        {
            ProductDirectoryViewModel PDVM = new ProductDirectoryViewModel();
            PDVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();

            return PDVM;
        }
    }

    public class ProductDirectoryViewModel : BaseViewModel
    {
    }
}