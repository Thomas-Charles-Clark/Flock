using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_ContractFRGridViewModel
    {
        public List<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult> TechInfo_ContractFRs { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechInfo_ContractFRGridViewModel()
        {
            TechInfo_ContractFRs = new List<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult>();
        }

        public TranslateTechInfo_ContractFRGridViewModel(
            List<TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult> techInfo_ContractFRs,
            byte? targetLanguageId)
        {
            TechInfo_ContractFRs = techInfo_ContractFRs;
            TargetLanguageID = targetLanguageId;
        }
    }
}