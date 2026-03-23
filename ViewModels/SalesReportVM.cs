using System;
using System.Collections.Generic;
using CRMProductSystem.Models;

namespace CRMProductSystem.ViewModels
{
    public class SalesReportVM
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}