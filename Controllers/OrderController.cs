using Microsoft.AspNetCore.Mvc;
using CRMProductSystem.Services;
using CRMProductSystem.Models;
using System.Collections.Generic;

namespace CRMProductSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;

        public OrderController(
            CustomerService customerService,
            ProductService productService,
            OrderService orderService)
        {
            _customerService = customerService;
            _productService = productService;
            _orderService = orderService;
        }

        // =========================
        // ORDER LIST PAGE
        // =========================
        public IActionResult Index()
        {
            var orders = _orderService.GetOrders();
            return View(orders);
        }

        // =========================
        // CREATE PAGE (GET)
        // =========================
        public IActionResult Create()
        {
            ViewBag.Customers = _customerService.GetCustomers();
            ViewBag.Products = _productService.GetProducts();
            return View();
        }

        // =========================
        // CREATE ORDER (POST)
        // =========================
        [HttpPost]
        public IActionResult Create(Order order)
        {
            // Get logged-in user from session
            order.OrderDate = DateTime.Now;

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                order.UserId = userId.Value;
            }

            _orderService.AddOrder(order);

            return RedirectToAction("Index");
        }

        // =========================
        // ORDER DETAILS (MASTER + GRID)
        // =========================
        public IActionResult Details(int id)
        {
            var order = _orderService.GetOrderById(id);
            var items = _orderService.GetOrderItemsByOrderId(id);

            ViewBag.Items = items;

            return View(order);
        }

        // =========================
        // DELETE ORDER
        // =========================
        public IActionResult Delete(int id)
        {
            _orderService.DeleteOrder(id);
            return RedirectToAction("Index");
        }
    }
}