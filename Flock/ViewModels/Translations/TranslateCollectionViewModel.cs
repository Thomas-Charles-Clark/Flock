using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateCollectionViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public short? CollectionID { get; set; }

        public TranslateCollectionViewModel()
        {

        }
    }
}