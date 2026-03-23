using Microsoft.AspNetCore.Mvc;
using CRMProductSystem.Services;
using CRMProductSystem.Models;

namespace CRMProductSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public UserController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            return RedirectToAction("Login", "Auth");
            var users = _userService.GetUsers();
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            _authService.Register(user);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _userService.DeleteUser(id);
            return RedirectToAction("Index");
        }
    }
}