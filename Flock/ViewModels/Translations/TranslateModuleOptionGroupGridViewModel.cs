using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateModuleOptionGroupGridViewModel
    {
        public List<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult> ModuleOptionGroups { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateModuleOptionGroupGridViewModel()
        {
            ModuleOptionGroups = new List<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult>();
        }

        public TranslateModuleOptionGroupGridViewModel(
            List<ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult> moduleOptionGroups,
            byte? targetLanguageId)
        {
            ModuleOptionGroups = moduleOptionGroups;
            TargetLanguageID = targetLanguageId;
        }
    }
}