using CRMProductSystem.Services;
using Microsoft.AspNetCore.Mvc;

public class ActivityController : Controller
{
    private readonly ActivityService _activityService;

    public ActivityController(ActivityService activityService)
    {
        _activityService = activityService;
    }

    public IActionResult Index(DateTime? fromDate, DateTime? toDate, int? userId, int? customerId)
    {
        var activities = _activityService.GetActivityReport(fromDate, toDate, userId, customerId);
        return View(activities);
    }
}