using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.ViewModels
{
    public class FormViewModelBuilder : ViewModelBuilder, IViewModelBuilder<FormViewModel>
    {
        public FormViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public FormViewModel Build(long? FormID)
        {
            FormViewModel FVM = new FormViewModel();
            Form f = UoW.GetRepository<Form>().GetAll().Where(p => p.FormID == FormID).Single();
            FVM.FormID = (long)FormID;
            FVM.Name = f.Name;
            FVM.CompetitionID = f.CompetitionID;

            return FVM;
        }
    }

    public class FormViewModel : BaseViewModel
    {
        public long FormID { get; set; }

        public string Name { get; set; }

        public string Heading { get; set; }

        public string SubHeading { get; set; }

        public bool UseCaptcha { get; set; }

        public short UserSourceID { get; set; }

        public short? CompetitionID { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime DeletedDT { get; set; }
    }
}