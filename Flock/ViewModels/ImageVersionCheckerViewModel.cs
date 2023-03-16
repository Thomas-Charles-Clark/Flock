using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ImageVersionCheckerViewModel : BaseViewModel
    {
        public List<Image_MultipleVersions_SelectResult> Images { get; set; }
    }
}