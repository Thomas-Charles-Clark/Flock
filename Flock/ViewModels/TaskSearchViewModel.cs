using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TaskSearchViewModel
    {
        public List<TaskSearch_HighlightResult> TaskHighlights { get; set; }
        public List<TaskRender_TaskResult> Tasks { get; set; }
    }
}