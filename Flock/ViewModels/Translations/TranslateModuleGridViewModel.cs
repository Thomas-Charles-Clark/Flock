using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateModuleGridViewModel
    {
        public List<Module_Language_SelectList_ModuleResult> Modules { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateModuleGridViewModel()
        {
            Modules = new List<Module_Language_SelectList_ModuleResult>();
        }

        public TranslateModuleGridViewModel(
            List<Module_Language_SelectList_ModuleResult> modules,
            byte? targetLanguageId)
        {
            Modules = modules;
            TargetLanguageID = targetLanguageId;
        }
    }
}