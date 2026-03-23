using Microsoft.AspNetCore.Mvc;
using CRMProductSystem.Services;
using CRMProductSystem.Models;
using Microsoft.AspNetCore.Http;

namespace CRMProductSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Please enter username and password";
                return View();
            }
            var user = _authService.Login(username, password);

            if (user == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username ?? "");
            HttpContext.Session.SetString("Role", user.Role ?? "");

            var trimmedRole = user.Role?.Trim();
            if (trimmedRole != null && trimmedRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("AdminDashboard", "Dashboard");
            else if (trimmedRole != null && trimmedRole.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("EmployeeDashboard", "Dashboard");
            else if(user == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }
            else
            {
                ViewBag.Error = "Role not authorized for dashboard access";
                return View("Login");
            }
            
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}