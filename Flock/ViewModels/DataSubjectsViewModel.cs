using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class DataSubjectsViewModel : BaseViewModel
    {
        public List<DataSubjectRequest_SelectResult> CurrentQueue { get; set; }

        public DataSubjectsViewModel(List<DataSubjectRequest_SelectResult> currentQueue) : base()
        {
            CurrentQueue = currentQueue;
        }
    }
}