using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_AftercareGridViewModel
    {
        public List<TechInfo_Aftercare_Language_SelectList_AftercareResult> TechInfo_Aftercares { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechInfo_AftercareGridViewModel()
        {
            TechInfo_Aftercares = new List<TechInfo_Aftercare_Language_SelectList_AftercareResult>();
        }

        public TranslateTechInfo_AftercareGridViewModel(
            List<TechInfo_Aftercare_Language_SelectList_AftercareResult> techInfo_Aftercares,
            byte? targetLanguageId)
        {
            TechInfo_Aftercares = techInfo_Aftercares;
            TargetLanguageID = targetLanguageId;
        }
    }
}