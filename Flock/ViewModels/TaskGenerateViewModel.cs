using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TaskGenerateViewModel
    {
        public List<TaskGenerate_StatusCountResult> TasksInStatus { get; set; }
        public List<TaskGenerate_Perf_New_Count_Result> PerformanceNew { get; set; }
        public List<TaskGenerate_Perf_Completed_Count_Result> PerformanceCompleted { get; set; }
        public List<TaskRender_TaskResult> Tasks { get; set; }
        public List<TaskGenerate_TaskNoteResult> TaskNotes { get; set; }
        public List<TaskGenerate_TaskTypeResult> TaskTypes { get; set; }
        public List<TaskGenerate_PriorityResult> Priorities { get; set; }
        public List<TaskGenerate_TaskStatusResult> Statuses { get; set; }

        public String SearchTerm { get; set; }
        public Boolean Active { get; set; }
        public Boolean Queued { get; set; }
        public Boolean Closed { get; set; }
    }
}