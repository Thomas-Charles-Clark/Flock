using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateColourGridViewModel
    {
        public List<Colour_Language_SelectList_ColourResult> Colours { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateColourGridViewModel()
        {
            Colours = new List<Colour_Language_SelectList_ColourResult>();
        }

        public TranslateColourGridViewModel(
            List<Colour_Language_SelectList_ColourResult> colours,
            byte? targetLanguageId)
        {
            Colours = colours;
            TargetLanguageID = targetLanguageId;
        }
    }
}