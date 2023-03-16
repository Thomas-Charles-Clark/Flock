using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.ViewModels;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductStockViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductStockViewModel>
    {
        public ProductStockViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ProductStockViewModel Build(long? id)
        {
            ProductStockViewModel PSVM = new ProductStockViewModel();

            PSVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();
            PSVM.Collections = UoW.GetRepository<Collection>().GetAll().ToList();
            PSVM.Colours = UoW.GetRepository<Colour>().GetAll().ToList();
            return PSVM;
        }
    }

    public class ProductStockViewModel : BaseViewModel
    {
    }

    public class ProductStockGridDetailViewModel : BaseViewModel
    {

    }
}