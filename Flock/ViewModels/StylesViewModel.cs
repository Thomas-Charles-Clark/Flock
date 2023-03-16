using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class StylesViewModel : BaseViewModel
    {
    }

    public class StylesCreateModel
    {
        [Required]
        [StringLength(100)]
        public String Name { get; set; }
    }

    public class StylesEditModel
    {
        public short StyleID { get; set; }

        [Required]
        [StringLength(100)]
        public String Name { get; set; }

        public byte? FontID { get; set; }

        public string Margin { get; set; }

        public string Padding { get; set; }

        public string BackgroundColour { get; set; }

        public string Colour { get; set; }

        public string BorderColour { get; set; }

        public byte? BorderWidth { get; set; }

        public string BorderStyle { get; set; }

        public decimal? Opacity { get; set; }

        public List<Font> AvailableFonts { get; set; }

        public StylesEditModel()
        {

        }

        public StylesEditModel(UnitOfWork<FlockDataContext> UoW, short? StyleID)
        {
            if (StyleID != null)
            {
                var s = UoW.GetRepository<Style>().GetAll().Where(p => p.StyleID == StyleID).Single();

                this.StyleID = s.StyleID;
                this.Name = s.Name;
                this.FontID = s.FontID;
                this.Margin = s.Margin;
                this.Padding = s.Padding;
                this.BackgroundColour = s.BackgroundColour;
                this.Colour = s.Colour;
                this.BorderColour = s.BorderColour;
                this.BorderWidth = s.BorderWidth;
                this.BorderStyle = s.BorderStyle;
                this.Opacity = s.Opacity;
            }

            this.AvailableFonts = UoW.GetRepository<Font>().GetAll().OrderBy(p => p.Name).ToList();
        }

        public void RebuildLists(UnitOfWork<FlockDataContext> UoW)
        {
            this.AvailableFonts = UoW.GetRepository<Font>().GetAll().OrderBy(p => p.Name).ToList();
        }
    }
}