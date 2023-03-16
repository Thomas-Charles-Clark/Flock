using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_TestGridViewModel
    {
        public List<TechInfo_Test_Language_SelectList_TechInfo_TestResult> TechInfo_Tests { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechInfo_TestGridViewModel()
        {
            TechInfo_Tests = new List<TechInfo_Test_Language_SelectList_TechInfo_TestResult>();
        }

        public TranslateTechInfo_TestGridViewModel(
            List<TechInfo_Test_Language_SelectList_TechInfo_TestResult> techInfo_Tests,
            byte? targetLanguageId)
        {
            TechInfo_Tests = techInfo_Tests;
            TargetLanguageID = targetLanguageId;
        }
    }
}