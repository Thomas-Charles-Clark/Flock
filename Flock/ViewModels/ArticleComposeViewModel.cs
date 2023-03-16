using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.Models;
using Flock.DAL;

namespace Flock.ViewModels
{
    public class ArticleViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;
        public ArticleViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ArticleComposeViewModel Build(int? ArticleID)
        {
            ArticleComposeViewModel Model = new ArticleComposeViewModel();

            Article a = UoW.GetRepository<Article>().GetAll().Where(p => p.ArticleID == ArticleID).Single();
            Model.ArticleID = a.ArticleID;
            Model.ArticleTypeID = a.ArticleTypeID;
            Model.ArticleSubTypeID = a.ArticleSubTypeID;
            Model.ArticleTemplateID = a.ArticleTemplateID;
            Model.Name = a.Name;
            Model.TitleTag = a.TitleTag;
            Model.Heading = a.Heading;
            Model.SubHeading = a.SubHeading;
            Model.OnDate = a.OnDate;
            Model.OffDate = a.OffDate;
            Model.MetaDescription = a.MetaDescription;
            Model.Contents = a.Contents;
            Model.VideoID = a.VideoID;
            if(Model.VideoID != null)
                Model.VideoEmbedCode = UoW.GetRepository<Video>().GetAll().Where(p => p.VideoID == Model.VideoID).Select(p => p.EmbedCode).Single();
            Model.URLKeywords = a.URLKeywords;
            Model.RelatedArticles = UoW.GetRepository<vwArticle_RelatedArticle>().GetAll().Where(p => p.ArticleID == ArticleID).ToList();
            Model.Images = UoW.GetRepository<Article_Image>().GetAll().Where(p => p.ArticleID == ArticleID).OrderBy(p => p.SortOrder).ToList();
            
            var cLinker = UoW.GetRepository<Article_Country>().GetAll().Where(p => p.ArticleID == ArticleID).Select(p => p.CountryID).ToList();
            Model.Countries = UoW.GetRepository<Country>().GetAll().Where(p => cLinker.Contains(p.CountryID)).ToList();

            Model.EventDT = a.EventDT;

            return Model;
        }
    }

    public class ArticleComposeViewModel : BaseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int? ArticleID { get; set; }

        public byte ArticleTypeID { get; set; }

        public byte? ArticleSubTypeID { get; set; }

        public byte ArticleTemplateID { get; set; }

        [Display(Name = "TitleTag")]
        [StringLength(70, MinimumLength = 3)]
        public string TitleTag { get; set; }

        [Display(Name = "Name")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Heading")]
        [StringLength(70)]
        public string Heading { get; set; }

        [Display(Name = "Sub Heading")]
        [StringLength(100)]
        public string SubHeading { get; set; }

        [Display(Name = "Contents")]
        public string Contents { get; set; }

        [Display(Name = "Video")]
        public short? VideoID { get; set; }

        public string VideoEmbedCode { get; set; }

        [Display(Name = "Meta Description")]
        [StringLength(155, MinimumLength = 3)]
        public string MetaDescription { get; set; }

        [StringLength(100)]
        [RegularExpression("^([a-z]|[a-z]-|[a-z]/)+[a-z]$", ErrorMessage = @"Regular Expression Validation Failed.
    URLs must not begin or end with a slash.
    URL segments must not contain accented characters, such as é or ë.
    URL segments may only contain the lower case letters 'a' through 'z', as well as hyphens.
    URL segments must not begin or end with a hyphen.")]
        public string URLKeywords { get; set; }

        public List<vwArticle_RelatedArticle> RelatedArticles { get; set; }

        public List<Article_Image> Images { get; set; }
        
        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }

        public DateTime? DeletedDT { get; set; }

        public List<Country> Countries { get; set; }

        public DateTime? EventDT { get; set; }
    }

    public class ArticleCreationModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        public byte ArticleTypeID { get; set; }

        public byte ArticleSubTypeID { get; set; }

        [Required]
        public byte ArticleTemplateID { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 5)]
        public string Heading { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 5)]
        public string TitleTag { get; set; }
    }
}