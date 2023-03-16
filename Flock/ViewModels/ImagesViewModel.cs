using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class ImagesOverviewViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ImagesOverviewViewModel>
    {
        public ImagesOverviewViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ImagesOverviewViewModel Build(long? id)
        {
            ImagesOverviewViewModel Model = new ImagesOverviewViewModel();

            Model.Markets = UoW.GetRepository<Market>().GetAll().ToList();

            return Model;
        }
    }

    public class ImagesOverviewViewModel : BaseViewModel
    {

    }

    public class ProductImagesOverviewSettings
    {
        public short[] CollectionIDs { get; set; }
        public byte[] ColourIDs { get; set; }
    }
}