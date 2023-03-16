using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductCollectionViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductCollectionViewModel>
    {
        public ProductCollectionViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ProductCollectionViewModel Build(long? id)
        {
            ProductCollectionViewModel PCVM = new ProductCollectionViewModel();
            //PCVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();
            PCVM.Collections = UoW.GetRepository<Collection>().GetAll().ToList();
            return PCVM;
        }
    }

    public class ProductCollectionViewModel : BaseViewModel
    {
    }

    //public class CollectionViewModel
    //{
    //    public short CollectionID { get; set; }
    //    public IEnumerable<vwProductSearch> Products { get; set; }
    //}
}