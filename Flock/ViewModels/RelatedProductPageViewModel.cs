using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class RelatedProductPageViewModelBuilder : ViewModelBuilder
    {
        public RelatedProductPageViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public RelatedProductPageViewModel Build()
        {
            var Model = new RelatedProductPageViewModel();
            return Model;
        }
    }

    public class RelatedProductPagePartialViewModel
    {
        public List<PPRPPS_TotalCount> PrimaryResultSetTotal { get; set; }
        public List<PPRPPS_One> PrimaryResultSet { get; set; }
        public List<PPRPPS_Two> SecondaryResultSet { get; set; }
        public int NumPages { get; set; }
        public int CurrentPage { get; set; }
    }

    public class RelatedProductPageViewModel : BaseViewModel
    {
        public RelatedProductPage_PPFilters PPFilters { get; set; }
        public RelatedProductPage_PPFilters RPPFilters { get; set; }
    }

    public class RelatedProductPage_PPFilters
    {
        public byte? ProductTypeID { get; set; }
        public short? CollectionID { get; set; }
        public byte? BrandID { get; set; }
        public byte? LaunchID { get; set; }
        public byte? TemplateID { get; set; }
        public byte? StatusID { get; set; }
    }

    public class RelatedProductPage_RPPFilters
    {
        public byte? ProductTypeID { get; set; }
        public short? CollectionID { get; set; }
        public byte? BrandID { get; set; }
        public byte? LaunchID { get; set; }
        public byte? TemplateID { get; set; }
        public byte? StatusID { get; set; }
    }

    public class RelatedSuggestions
    {
        public List<PPRPPS_Two> ResultSet { get; set; }
    }
}