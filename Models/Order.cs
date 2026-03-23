using System;
using System.Collections.Generic;

namespace CRMProductSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }
        public int? UserId { get; set; }

        public DateTime OrderDate { get; set; }

        // For Order List & Details Page
        public string? CustomerName { get; set; }
        public string? Username { get; set; }

        public decimal TotalAmount { get; set; }


        // Navigation for Create Order
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}