using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateAftercareGridViewModel
    {
        public List<TechInfo_Aftercare_Language_SelectList_AftercareResult> Aftercares { get; set; }
        public short? TargetCountryLanguageID { get; set; }
        
        public TranslateAftercareGridViewModel()
        {
            Aftercares = new List<TechInfo_Aftercare_Language_SelectList_AftercareResult>();
        }

        public TranslateAftercareGridViewModel(
            List<TechInfo_Aftercare_Language_SelectList_AftercareResult> aftercares,
            short? targetCountryLanguageID)
        {
            Aftercares = aftercares;
            TargetCountryLanguageID = targetCountryLanguageID;
        }
    }
}