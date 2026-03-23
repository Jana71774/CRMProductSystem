using Microsoft.AspNetCore.Mvc;
using CRMProductSystem.Services;
using CRMProductSystem.Models;

namespace CRMProductSystem.Controllers
{
    public class FollowupController : Controller
    {
        private readonly FollowupService _followupService;

        public FollowupController(FollowupService followupService)
        {
            _followupService = followupService;
        }

        // GET : Show all followups
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<TaskFollowup> followups;

            if (role == "Admin")
            {
            followups = _followupService.GetAll();
            }
            else
            {
            followups = _followupService.GetByUser(userId.Value);
            }
            return View(followups);
        }

        // POST : Add followup
        [HttpPost]
        public IActionResult Create(TaskFollowup model)
        {
            _followupService.Add(model);
            return RedirectToAction("Index");
        }

        // Update Sales Status
        [HttpPost]
        public IActionResult UpdateStatus(int followupId, string salesStatus)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");
            if (userId == null)
                return RedirectToAction("Login", "Auth");
            if (role == "Admin")
            {
                _followupService.UpdateStatus(followupId, salesStatus);
            }
            else
            {
                _followupService.UpdateStatusByUser(followupId, salesStatus, userId.Value);
            }
            TempData["success"] = "Status updated successfully";
            return RedirectToAction("Index");
        }
    }
}