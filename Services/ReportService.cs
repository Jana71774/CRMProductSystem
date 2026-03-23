using System;
using System.Collections.Generic;
using System.Linq;
using CRMProductSystem.Data;
using CRMProductSystem.Models;
using Dapper;

namespace CRMProductSystem.Services
{
    public class ReportService
    {
        private readonly DbConnection _db;

        public ReportService(DbConnection db)
        {
            _db = db;
        }

        // Get sales report based on date filter
        public List<Order> GetSalesReport(DateTime? fromDate, DateTime? toDate)
        {
            using var conn = _db.CreateConnection();

            string query = @"
                SELECT 
                    o.OrderId,
                    o.CustomerId,
                    o.OrderDate,
                    o.TotalAmount,
                    c.CustomerName
                FROM Orders o
                JOIN Customers c ON o.CustomerId = c.CustomerId
                WHERE (@FromDate IS NULL OR o.OrderDate >= @FromDate)
                AND (@ToDate IS NULL OR o.OrderDate <= @ToDate)
                ORDER BY o.OrderDate DESC";

            return conn.Query<Order>(query, new
            {
                FromDate = fromDate,
                ToDate = toDate
            }).ToList();
        }
    }
}