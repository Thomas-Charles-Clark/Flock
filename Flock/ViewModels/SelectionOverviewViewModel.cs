using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class SelectionOverviewViewModel : BaseViewModel
    {
        public short? SelectionID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(70)]
        public string Heading { get; set; }

        [StringLength(150)]
        public string SubHeading { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression("^([a-z]|[a-z]-|[a-z]/)+[a-z]$", ErrorMessage = @"Regular Expression Validation Failed.
    URLs must not begin or end with a slash.
    URL segments must not contain accented characters, such as é or ë.
    URL segments may only contain the lower case letters 'a' through 'z', as well as hyphens.
    URL segments must not begin or end with a hyphen.")]
        public string URLKeywords { get; set; }

        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }

        public DateTime CreatedDT { get; set; }
        public DateTime ModifiedDT { get; set; }

        public DateTime? DeletedDT { get; set; }
    }


    public class SelectionComposeViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;
        public SelectionComposeViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public SelectionComposeViewModel Build(short? SelectionID)
        {
            SelectionComposeViewModel Model = new SelectionComposeViewModel();

            if(SelectionID != null)
            {
                Selection s = UoW.GetRepository<Selection>().GetAll().Where(p => p.SelectionID == SelectionID && p.DeletedDT == null).SingleOrDefault();
                if(s != null)
                {
                    Model.SelectionID = SelectionID;
                    Model.Name = s.Name;
                    Model.TitleTag = s.TitleTag;
                    Model.URLKeywords = s.URLKeywords;
                    Model.Heading = s.Heading;
                    Model.SubHeading = s.SubHeading;
                    Model.Description = s.Description;
                    Model.MetaDescription = s.MetaDescription;
                    Model.CreatedDT = s.CreatedDT;
                    Model.OnDate = s.OnDate;
                    Model.OffDate = s.OffDate;

                    Model.Images = UoW.GetRepository<Selection_Image>().GetAll().Where(p => p.SelectionID == SelectionID).OrderBy(p => p.SortOrder).ToList();
                }
            }

            return Model;
        }
    }

    public class SelectionComposeViewModel : BaseViewModel
    {
        public short? SelectionID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression("^([a-z]|[a-z]-|[a-z]/)+[a-z]$", ErrorMessage = @"Regular Expression Validation Failed.
    URLs must not begin or end with a slash.
    URL segments must not contain accented characters, such as é or ë.
    URL segments may only contain the lower case letters 'a' through 'z', as well as hyphens.
    URL segments must not begin or end with a hyphen.")]
        public string URLKeywords { get; set; }

        [StringLength(70)]
        public string Heading { get; set; }

        [StringLength(150)]
        public string SubHeading { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(70)]
        public string TitleTag  { get; set; }

        [StringLength(155)]
        public string MetaDescription { get; set; }

        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime ModifiedDT { get; set; }

        public DateTime? DeletedDT { get; set; }

        public List<Selection_Image> Images { get; set; }
    }

    /*
        [StringLength(2000)]
        public string Description { get; set; }
     */
}