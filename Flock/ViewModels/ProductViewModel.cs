using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductDirectoryGridDetailViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductDirectoryGridDetailViewModel>
    {
        public ProductDirectoryGridDetailViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ProductDirectoryGridDetailViewModel Build(long? ProductID)
        {
            ProductDirectoryGridDetailViewModel PDGDVM = new ProductDirectoryGridDetailViewModel();

            List<Image> ProductImages = UoW.GetRepository<Image>().GetAll().Where(x => x.ProductID == ProductID && x.DeletedDT == null).ToList();

            PDGDVM.FrontImageID = UoW.GetRepository<Product_Image>().GetAll().Where(p => p.ProductID == ProductID && p.PerspectiveID == 1).Select(p => p.ImageID).SingleOrDefault();
            PDGDVM.BackImageID = UoW.GetRepository<Product_Image>().GetAll().Where(p => p.ProductID == ProductID && p.PerspectiveID == 2).Select(p => p.ImageID).SingleOrDefault();
            PDGDVM.CloseupImageID = UoW.GetRepository<Product_Image>().GetAll().Where(p => p.ProductID == ProductID && p.PerspectiveID == 3).Select(p => p.ImageID).SingleOrDefault();
            PDGDVM.StyledImageID = UoW.GetRepository<Product_Image>().GetAll().Where(p => p.ProductID == ProductID && p.PerspectiveID == 4).Select(p => p.ImageID).SingleOrDefault();
            PDGDVM.ProductID = ProductID.Value;
            PDGDVM.UnselectedFrontImages = ProductImages.Where(x => x.PerspectiveID == 1).Select(x => x.ImageID).ToList();
            PDGDVM.UnselectedBackImages = ProductImages.Where(x => x.PerspectiveID == 2).Select(x => x.ImageID).ToList();
            PDGDVM.UnselectedCloseupImages = ProductImages.Where(x => x.PerspectiveID == 3).Select(x => x.ImageID).ToList();
            PDGDVM.UnselectedStyledImages = ProductImages.Where(x => x.PerspectiveID == 4).Select(x => x.ImageID).ToList();

            if(PDGDVM.FrontImageID != null)
                PDGDVM.UnselectedFrontImages.RemoveAll(x => x == (long)PDGDVM.FrontImageID);
            if(PDGDVM.BackImageID != null)
                PDGDVM.UnselectedBackImages.RemoveAll(x => x == (long)PDGDVM.BackImageID);
            if (PDGDVM.CloseupImageID != null)
                PDGDVM.UnselectedCloseupImages.RemoveAll(x => x == (long)PDGDVM.CloseupImageID);
            if (PDGDVM.StyledImageID != null)
                PDGDVM.UnselectedStyledImages.RemoveAll(x => x == (long)PDGDVM.StyledImageID);

            return PDGDVM;
        }
    }

    public class ProductDirectoryGridDetailViewModel : BaseViewModel
    {
        public long ProductID { get; set; }

        public long? FrontImageID { get; set; }
        public List<long> UnselectedFrontImages { get; set; }

        public long? BackImageID { get; set; }
        public List<long> UnselectedBackImages { get; set; }

        public long? CloseupImageID { get; set; }
        public List<long> UnselectedCloseupImages { get; set; }

        public long? StyledImageID { get; set; }
        public List<long> UnselectedStyledImages { get; set; }

        public List<Image> OtherImages { get; set; }
    }
}