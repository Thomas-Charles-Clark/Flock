using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateImageGridViewModel
    {
        public List<Image_Language_SelectList_ImageResult> Images { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateImageGridViewModel()
        {
            Images = new List<Image_Language_SelectList_ImageResult>();
        }

        public TranslateImageGridViewModel(
            List<Image_Language_SelectList_ImageResult> images,
            byte? targetLanguageId)
        {
            Images = images;
            TargetLanguageID = targetLanguageId;
        }
    }
}