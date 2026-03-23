using Microsoft.AspNetCore.Mvc;
using CRMProductSystem.Services;
using CRMProductSystem.Models;

namespace CRMProductSystem.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        private readonly UserService _userService;
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;

        public TaskController(
            TaskService taskService,
            UserService userService,
            CustomerService customerService,
            ProductService productService)
        {
            _taskService = taskService;
            _userService = userService;
            _customerService = customerService;
            _productService = productService;
        }

        // ✅ ADMIN: View All Tasks
        public IActionResult Index()
        {
            var tasks = _taskService.GetAllTasks();
            return View(tasks);
        }

        // ✅ EMPLOYEE: View Assigned Tasks
        public IActionResult MyTasks()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            return RedirectToAction("Login", "Auth");
            var tasks = _taskService.GetTasksByUser(userId.Value);
            return View(tasks);
        }

        // ✅ GET: Create Task
        public IActionResult Create()
        {
            ViewBag.Users = _userService.GetAllEmployees();
            ViewBag.Customers = _customerService.GetAll();
            ViewBag.Products = _productService.GetAll();

            return View();
        }

        // ✅ POST: Create Task
        [HttpPost]
        public IActionResult Create(TaskModel model)
        {
            if (ModelState.IsValid && model.AssignedTo.HasValue && model.AssignedTo.Value > 0)
            {
                _taskService.AddTask(model);
                return RedirectToAction("Index");
            }

            // reload dropdowns if error
            ViewBag.Users = _userService.GetAllEmployees();
            ViewBag.Customers = _customerService.GetAll();
            ViewBag.Products = _productService.GetAll();

            ModelState.AddModelError("", "Please select valid Assigned Employee");
            return View(model);
        }

        // ✅ GET: Edit Task
        public IActionResult Edit(int id)
        {
            var task = _taskService.GetTaskById(id);

            ViewBag.Users = _userService.GetAllEmployees();
            ViewBag.Customers = _customerService.GetAll();
            ViewBag.Products = _productService.GetAll();

            return View(task);
        }

        // ✅ POST: Edit Task
        [HttpPost]
        public IActionResult Edit(TaskModel model)
        {
            if (ModelState.IsValid)
            {
                _taskService.UpdateTask(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ✅ DELETE Task
        public IActionResult Delete(int id)
        {
            _taskService.DeleteTask(id);
            return RedirectToAction("Index");
        }

        // ✅ UPDATE STATUS (INLINE / AJAX READY)
        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            _taskService.UpdateStatus(id, status);
            return Json(new { success = true });
        }
    }
}