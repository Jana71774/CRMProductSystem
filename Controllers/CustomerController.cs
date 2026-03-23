using Microsoft.AspNetCore.Mvc;
using CRMProductSystem.Services;
using CRMProductSystem.Models;

namespace CRMProductSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;
        private readonly ActivityService _activityService;

        public CustomerController(CustomerService customerService, ActivityService activityService)
        {
            _customerService = customerService;
            _activityService = activityService;
        }


        // List all customers
        public IActionResult Index()
        {
            var customers = _customerService.GetCustomers();
            return View(customers);
        }

        // Show create form
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customerService.AddCustomer(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        public IActionResult Details(int id)
        {
            var customer = _customerService.GetCustomerById(id);

            var logs = _activityService.GetLogsByCustomer(id);

            ViewBag.ActivityLogs = logs;

            return View(customer);
        }

        // Show edit form
        public IActionResult Edit(int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customerService.UpdateCustomer(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // Delete customer
        public IActionResult Delete(int id)
        {
            _customerService.DeleteCustomer(id);
            return RedirectToAction("Index");
        }
    }
}