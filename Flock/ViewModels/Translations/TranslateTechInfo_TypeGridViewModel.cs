using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_TypeGridViewModel
    {
        public List<TechInfo_Type_Language_SelectList_TechInfo_TypeResult> TechInfo_Types { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechInfo_TypeGridViewModel()
        {
            TechInfo_Types = new List<TechInfo_Type_Language_SelectList_TechInfo_TypeResult>();
        }

        public TranslateTechInfo_TypeGridViewModel(
            List<TechInfo_Type_Language_SelectList_TechInfo_TypeResult> techInfo_Types,
            byte? targetLanguageId)
        {
            TechInfo_Types = techInfo_Types;
            TargetLanguageID = targetLanguageId;
        }
    }
}