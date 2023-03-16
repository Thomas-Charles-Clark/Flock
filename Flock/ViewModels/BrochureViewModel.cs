using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class BrochureViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;
 
        public BrochureViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public BrochureViewModel Build(short? BrochureID)
        {
            Brochure b = UoW.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == BrochureID).Single();
            BrochureViewModel BVM = new BrochureViewModel();
            BVM.BrochureID = BrochureID;
            BVM.Name = b.Name;
            BVM.Heading = b.Heading;
            BVM.Description = b.Description;
            BVM.Code = b.Code;
            BVM.CoverImagePath = b.CoverImagePath;
            BVM.PDFPath = b.PDFPath;
            BVM.InteractiveBrochureURL = b.InteractiveBrochureURL;
            BVM.FileSizeMB = b.FileSizeMB;
            BVM.BrandID = b.BrandID;
            BVM.LaunchID = b.LaunchID;
            BVM.OnDate = b.OnDate;
            BVM.OffDate = b.OffDate;
            BVM.StatusID = UoW.Context.fnc_Status(BVM.OnDate, BVM.OffDate);
            BVM.CreatedDT = b.CreatedDT;
            BVM.ModifiedDT = b.ModifiedDT;
            BVM.DeletedDT = b.DeletedDT;
            return BVM;
        }
    }

    public class BrochureViewModel : BaseViewModel
    {
        public short? BrochureID { get; set; }

        [Required(ErrorMessage = "You must provide a Name.")]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(70)]
        public string Heading { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
        
        [StringLength(255)]
        public string CoverImagePath { get; set; }

        [StringLength(255)]
        public string PDFPath { get; set; }
        
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide a Product Code")]
        public string Code { get; set; }

        [StringLength(255)]
        public string InteractiveBrochureURL { get; set; }
        
        public short? FileSizeMB { get; set; }
        
        [Required(ErrorMessage="Please set a Brand")]
        public byte BrandID { get; set; }

        [Required(ErrorMessage = "Please set a Launch")]
        public short LaunchID { get; set; }
        
        public DateTime? OnDate { get; set; }
        
        public DateTime? OffDate { get; set; }
        
        public DateTime CreatedDT { get; set; }
        
        public DateTime ModifiedDT { get; set; }
        
        public DateTime? DeletedDT { get; set; }

        public byte? StatusID { get; set; }
    }
}