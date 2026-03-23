using System.Collections.Generic;
using CRMProductSystem.Models;
using CRMProductSystem.ViewModels;

namespace CRMProductSystem.ViewModels
{
    public class AdminDashboardVM
    {
        public int TotalTasks { get; set; }

        public int CompletedTasks { get; set; }

        public int PendingTasks { get; set; }

        public List<TaskStat>? TaskStats { get; set; }

        public List<TaskModel>? RecentTasks { get; set; }
    }
}