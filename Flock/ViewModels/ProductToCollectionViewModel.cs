using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductToCollectionViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductToCollectionViewModel>
    {
        public ProductToCollectionViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ProductToCollectionViewModel Build(long? id)
        {
            ProductToCollectionViewModel PCVM = new ProductToCollectionViewModel();
            PCVM.CollectionID = id;
            //PCVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();
            //PCVM.Collections = UoW.GetRepository<Collection>().GetAll().ToList();
            //PCVM.Brands = UoW.GetRepository<Brand>().GetAll().ToList();
            //PCVM.Launches = UoW.GetRepository<Launch>().GetAll().ToList();
            //PCVM.ProductTypes = UoW.GetRepository<ProductType>().GetAll().ToList();
            //PCVM.Designs = UoW.GetRepository<Design>().GetAll().ToList();
            return PCVM;
        }
    }

    public class ProductToCollectionViewModel : BaseViewModel
    {
        public long? CollectionID { get; set; }

        public CollectionHeaderFilters HeaderFilters { get; set; }
    }

    public class CollectionForEditingViewModel
    {
        public List<PSR> Items { get; set; }
        public long CollectionID { get; set; }
    }

    // product search result: holder for this nonsense...
    public class PSR
    {
        public long ProductID { get; set; }
        public long? ImageID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public byte? SortOrder { get; set; }
        public byte BrandID { get; set; }
        public byte ProductTypeID { get; set; }
        public short LaunchID { get; set; }
    }
}