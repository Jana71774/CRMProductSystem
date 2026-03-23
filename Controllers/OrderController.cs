using Microsoft.AspNetCore.Mvc;
using CRMProductSystem.Services;
using CRMProductSystem.Models;
using System.Collections.Generic;
using System;
using System.Linq;

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
            try
            {
                var orders = _orderService.GetOrders();
                return View(orders);
            }
            catch (Exception)
            {
                ViewBag.Error = "Error loading orders.";
                return View(new List<Order>());
            }
        }

        // =========================
        // CREATE PAGE (GET)
        // =========================
        public IActionResult Create()
        {
            try
            {
                ViewBag.Customers = _customerService.GetCustomers();
                ViewBag.Products = _productService.GetProducts();
                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "Error loading create order page.";
                return View();
            }
        }

        // =========================
        // CREATE ORDER (POST)
        // =========================
        [HttpPost]
        public IActionResult Create(Order order, List<OrderItem> orderItems)
        {
            try
            {
                // Check if at least one product exists
                if (orderItems == null || orderItems.Count == 0)
                {
                    ViewBag.Error = "Please add at least one product before placing the order.";

                    ViewBag.Customers = _customerService.GetCustomers();
                    ViewBag.Products = _productService.GetProducts();
                    return View(order);
                }

                // Check quantity
                if (orderItems.Any(i => i.Quantity <= 0))
                {
                    ViewBag.Error = "Quantity must be greater than 0.";

                    ViewBag.Customers = _customerService.GetCustomers();
                    ViewBag.Products = _productService.GetProducts();
                    return View(order);
                }

                // Set order date
                order.OrderDate = DateTime.Now;

                // Get logged-in user from session
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId != null)
                {
                    order.UserId = userId.Value;
                }

                // Save order
                _orderService.AddOrder(order);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while placing the order.";

                ViewBag.Customers = _customerService.GetCustomers();
                ViewBag.Products = _productService.GetProducts();

                return View(order);
            }
        }

        // =========================
        // ORDER DETAILS (MASTER + GRID)
        // =========================
        public IActionResult Details(int id)
        {
            try
            {
                var order = _orderService.GetOrderById(id);
                var items = _orderService.GetOrderItemsByOrderId(id);

                ViewBag.Items = items;

                return View(order);
            }
            catch (Exception)
            {
                ViewBag.Error = "Error loading order details.";
                return RedirectToAction("Index");
            }
        }

        // =========================
        // DELETE ORDER
        // =========================
        public IActionResult Delete(int id)
        {
            try
            {
                _orderService.DeleteOrder(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Error = "Error deleting order.";
                return RedirectToAction("Index");
            }
        }
    }
}