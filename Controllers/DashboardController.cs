using Microsoft.AspNetCore.Mvc;
using CRMProductSystem.Services;
using CRMProductSystem.ViewModels;
using Microsoft.AspNetCore.Http;

namespace CRMProductSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardService _dashboardService;
        private readonly TaskService _taskService;

        public DashboardController(DashboardService dashboardService, TaskService taskService)
        {
            _dashboardService = dashboardService;
            _taskService = taskService;
        }

        public IActionResult AdminDashboard()
        {
            var role = HttpContext.Session.GetString("Role")?.Trim();
            if (role != "Admin")
                return RedirectToAction("Login", "Auth");

            var model = new AdminDashboardVM
            {
                TotalTasks = _dashboardService.GetTotalTasks(),
                CompletedTasks = _dashboardService.GetCompletedTasks(),
                PendingTasks = _dashboardService.GetPendingTasks(),
                TaskStats = _dashboardService.GetTaskStats(),
                RecentTasks = _taskService.GetAllTasks()
            };

            return View(model);
        }

        public IActionResult EmployeeDashboard()
{
    var role = HttpContext.Session.GetString("Role")?.Trim();

    if (!string.Equals(role, "Employee", StringComparison.OrdinalIgnoreCase))
        return RedirectToAction("Login", "Auth");

    var userId = HttpContext.Session.GetInt32("UserId");

    if (userId == null)
        return RedirectToAction("Login", "Auth");

    var tasks = _taskService.GetTasksByUser(userId.Value);

    return View(tasks);
}
    }
}
