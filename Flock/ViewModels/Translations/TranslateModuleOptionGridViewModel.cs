using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateModuleOptionGridViewModel
    {
        public List<ModuleOption_Language_SelectList_ModuleOptionResult> ModuleOptions { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateModuleOptionGridViewModel()
        {
            ModuleOptions = new List<ModuleOption_Language_SelectList_ModuleOptionResult>();
        }

        public TranslateModuleOptionGridViewModel(
            List<ModuleOption_Language_SelectList_ModuleOptionResult> moduleOptions,
            byte? targetLanguageId)
        {
            ModuleOptions = moduleOptions;
            TargetLanguageID = targetLanguageId;
        }
    }
}