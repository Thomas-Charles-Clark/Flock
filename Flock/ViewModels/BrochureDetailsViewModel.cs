using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class BrochureDetailsViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public BrochureDetailsViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public BrochureDetailsViewModel Build(short ID)
        {
            BrochureDetailsViewModel BDVM = new BrochureDetailsViewModel();

            Brochure b = UoW.GetRepository<Brochure>().GetAll().Where(p => p.BrochureID == ID).Single();

            BDVM.BrochureID = b.BrochureID;
            BDVM.Name = b.Name;
            BDVM.Code = b.Code;
            BDVM.Description = b.Description;
            BDVM.CoverImagePath = b.CoverImagePath;
            BDVM.PDFPath = b.PDFPath;
            BDVM.InteractiveBrochureURL = b.InteractiveBrochureURL;
            BDVM.FileSizeMB = b.FileSizeMB;
            BDVM.BrandID = b.BrandID;
            BDVM.LaunchID = b.LaunchID;
            BDVM.CreatedDT = b.CreatedDT;
            BDVM.ModifiedDT = b.ModifiedDT;
            BDVM.DeletedDT = b.DeletedDT;

            return BDVM;
        }
    }

    public class BrochureDetailsViewModel
    {
        public short BrochureID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(300)]
        public string FilePath { get; set; }

        [StringLength(300)]
        public string CoverImagePath { get; set; }

        [StringLength(300)]
        public string InteractiveBrochureURL { get; set; }

        [StringLength(300)]
        public string PDFPath { get; set; }

        [StringLength(50)]
        public String Code { get; set; }

        public short FileSizeMB { get; set; }
        public byte BrandID { get; set; }
        public short LaunchID { get; set; }
        public DateTime? OnDate { get; set; }
        public DateTime? OffDate { get; set; }
        public DateTime CreatedDT { get; set; }
        public DateTime ModifiedDT { get; set; }
        public DateTime? DeletedDT { get; set; }
    }
}