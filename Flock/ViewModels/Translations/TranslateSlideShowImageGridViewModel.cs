using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateSlideShowImageGridViewModel
    {
        public List<SlideShowImage_Language_SelectList_SlideShowImageResult> SlideShowImages { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateSlideShowImageGridViewModel()
        {
            SlideShowImages = new List<SlideShowImage_Language_SelectList_SlideShowImageResult>();
        }

        public TranslateSlideShowImageGridViewModel(
            List<SlideShowImage_Language_SelectList_SlideShowImageResult> slideShowImages,
            byte? targetLanguageId)
        {
            SlideShowImages = slideShowImages;
            TargetLanguageID = targetLanguageId;
        }
    }
}