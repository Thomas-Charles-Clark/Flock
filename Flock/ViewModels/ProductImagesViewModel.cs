using Flock.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductImagesViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductImagesViewModel>
    {
        public ProductImagesViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ProductImagesViewModel Build(long? id)
        {
            ProductImagesViewModel PIVM = new ProductImagesViewModel();
            PIVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();
            PIVM.Collections = UoW.GetRepository<Collection>().GetAll().ToList();
            PIVM.Colours = UoW.GetRepository<Colour>().GetAll().ToList();
            return PIVM;
        }
    }

    public class ProductImagesViewModel : BaseViewModel
    {
    }
}