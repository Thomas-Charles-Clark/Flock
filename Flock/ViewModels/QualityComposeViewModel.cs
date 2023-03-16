using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class QualityComposeViewModelBuilder 
    {
        private UnitOfWork<FlockDataContext> UoW;

        public QualityComposeViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public QualityComposeViewModel Build(short QualityID, byte ProductTypeID)
        {
            var res = UoW.Context.Quality_Overview_Detail_Generate(QualityID, ProductTypeID);
            var result = new QualityComposeViewModel();
            result.QualityID = QualityID;
            result.ProductTypeID = ProductTypeID;
            result.Statuses = res.GetResult<Quality_Overview_Detail_StatusResult>().ToList();
            result.ProductPages = res.GetResult<Quality_Overview_Detail_ProductPageResult>().ToList();

            return result;
        }
    }

    public class QualityComposeViewModel : BaseViewModel
    {
        public short QualityID { get; set; }
        public byte ProductTypeID { get; set; }

        public List<Quality_Overview_Detail_StatusResult> Statuses { get; set; }
        public List<Quality_Overview_Detail_ProductPageResult> ProductPages { get; set; }
    }
}