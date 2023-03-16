using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class SearchImpactViewModel : BaseViewModel
    {
        public List<ProductTypeNonEntity> AvailableProductTypes { get; set; }

        public List<LaunchNonEntity> AvailableLaunches { get; set; }

        public SearchImpactViewModel(UnitOfWork<FlockDataContext> UoW)
        {
            AvailableProductTypes = UoW.GetRepository<ProductType>().GetAll()
                .Where(p => p.DeletedDT == null)
                .OrderBy(p => p.SortOrder)
                .Select(p => new ProductTypeNonEntity() { ProductTypeID = p.ProductTypeID, Name = p.Name })
                .ToList();

            AvailableLaunches = UoW.GetRepository<Launch>().GetAll()
                .OrderByDescending(p => p.LaunchID)
                .Select(p => new LaunchNonEntity() { LaunchID = p.LaunchID, Name = p.Name })
                .ToList();
        }
    }

    public class LaunchNonEntity
    {
        public short LaunchID { get; set; }
        public string Name { get; set; }
    }

    public class searchImpactSearchTerms
    {
        public byte? selectedProductTypeID { get; set; }
        public short? selectedLaunchID { get; set; }
    }

    public class searchImpactSearchReturn
    {
        public List<ProductPage_SearchSortOrder_SelectResult> results { get; set; }
    }

    public class searchImpactUpdate
    {
        public List<ProductPageSortOrder> searchResults { get; set; }
        public byte? selectedProductTypeID { get; set; }
        public short? selectedLaunchID { get; set; }
    }

    public class ProductPageSortOrder
    {
        public int ProductPageID { get; set; }
        public int SortOrder { get; set; }
    }
}