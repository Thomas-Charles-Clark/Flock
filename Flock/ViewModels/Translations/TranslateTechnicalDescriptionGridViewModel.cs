using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechnicalDescriptionGridViewModel
    {
        public List<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult> TechnicalDescriptions { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechnicalDescriptionGridViewModel()
        {
            TechnicalDescriptions = new List<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult>();
        }

        public TranslateTechnicalDescriptionGridViewModel(
            List<TechnicalDescription_Language_SelectList_TechnicalDescriptionResult> technicalDescriptions,
            byte? targetLanguageId)
        {
            TechnicalDescriptions = technicalDescriptions;
            TargetLanguageID = targetLanguageId;
        }
    }
}