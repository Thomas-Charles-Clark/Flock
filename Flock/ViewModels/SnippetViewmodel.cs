using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;
using System.ComponentModel.DataAnnotations;

namespace Flock.ViewModels
{
    public class SnippetViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public SnippetViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public SnippetViewModel Build(short? snippetID)
        {
            var Model = new SnippetViewModel();

            var snippet = UoW.GetRepository<Snippet>().GetAll().Where(p => p.SnippetID == snippetID).Single();
            Model.SnippetID = snippetID;

            Model.SnippetLocationID = snippet.SnippetLocationID;
            Model.Name = snippet.Name;
            Model.HTMLContent = snippet.HTMLContent == null ? string.Empty : snippet.HTMLContent;
            Model.OnDate = snippet.OnDate;
            Model.OffDate = snippet.OffDate;
            Model.CreatedDT = snippet.CreatedDT;
            Model.ModifiedDT = snippet.ModifiedDT;
            Model.DeletedDT = snippet.DeletedDT;

            var CountryLinker = UoW.GetRepository<Snippet_Country>().GetAll().Where(p => p.SnippetID == snippetID).Select(p => p.CountryID).ToList();
            Model.CountryRestrictions = UoW.GetRepository<Country>().GetAll().Where(p => CountryLinker.Contains(p.CountryID)).ToList();

            return Model;
        }
    }

    public class SnippetViewModel : BaseViewModel
    {
        public short? SnippetID { get; set; }

        public byte SnippetLocationID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string HTMLContent { get; set; }

        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime? ModifiedDT { get; set; }

        public DateTime? DeletedDT { get; set; }

        public List<Country> CountryRestrictions { get; set; }
    }
}