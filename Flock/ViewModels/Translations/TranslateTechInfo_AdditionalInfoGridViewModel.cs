using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_AdditionalInfoGridViewModel
    {
        public List<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult> TechInfo_AdditionalInfos { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechInfo_AdditionalInfoGridViewModel()
        {
            TechInfo_AdditionalInfos = new List<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult>();
        }

        public TranslateTechInfo_AdditionalInfoGridViewModel(
            List<TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult> techInfo_AdditionalInfos,
            byte? targetLanguageId)
        {
            TechInfo_AdditionalInfos = techInfo_AdditionalInfos;
            TargetLanguageID = targetLanguageId;
        }
    }
}