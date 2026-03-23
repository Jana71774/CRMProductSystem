using System.Collections.Generic;
using System.Linq;
using CRMProductSystem.Data;
using CRMProductSystem.Models;
using Dapper;


namespace CRMProductSystem.Services
{
    public class OrderService
    {
        private readonly DbConnection _db;

        public OrderService(DbConnection db)
        {
            _db = db;
        }

        // ===============================
        // CREATE ORDER + ORDER ITEMS
        // ===============================
        public void AddOrder(Order order)
        {
            using var conn = _db.GetConnection();

            // Insert Order
            var orderId = conn.QuerySingle<int>(@"
                INSERT INTO Orders (CustomerId, UserId, OrderDate)
                VALUES (@CustomerId, @UserId, @OrderDate);
                SELECT LAST_INSERT_ID();", order);

            // Insert Order Items
            foreach (var item in order.OrderItems)
            {
                if (item.ProductId > 0 && item.Quantity > 0)
                {
                    conn.Execute(@"
                        INSERT INTO OrderItems (OrderId, ProductId, Quantity)
                        VALUES (@OrderId, @ProductId, @Quantity)",
                        new
                        {
                            OrderId = orderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        });
                }
            }
        }

        // ===============================
        // ORDER LIST PAGE
        // ===============================
        public List<Order> GetOrders()
        {
            using var conn = _db.GetConnection();

            var orders = conn.Query<Order>(@"
                SELECT 
                    o.OrderId,
                    o.OrderDate,
                    c.CustomerName,
                    u.Username
                FROM Orders o
                JOIN Customers c ON o.CustomerId = c.CustomerId
                LEFT JOIN Users u ON o.UserId = u.UserId
                ORDER BY o.OrderId DESC
            ").ToList();

            return orders;
        }
        public List<Order> GetAllOrders()
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
                ORDER BY o.OrderId DESC";

            return conn.Query<Order>(query).ToList();
        }

        // ===============================
        // MASTER ORDER DETAILS
        // ===============================
        public Order GetOrderById(int id)
        {
            using var conn = _db.GetConnection();

            var order = conn.QueryFirstOrDefault<Order>(@"
                SELECT 
                    o.OrderId,
                    o.OrderDate,
                    c.CustomerName,
                    u.Username
                FROM Orders o
                JOIN Customers c ON o.CustomerId = c.CustomerId
                LEFT JOIN Users u ON o.UserId = u.UserId
                WHERE o.OrderId = @id", new { id });

            return order ?? new Order();
        }

        // ===============================
        // ORDER ITEMS FOR GRID
        // ===============================
        public List<dynamic> GetOrderItemsByOrderId(int orderId)
        {
            using var conn = _db.GetConnection();

            var items = conn.Query(@"
                SELECT 
                    p.ProductName,
                    p.Price,
                    p.Description,
                    oi.Quantity
                FROM OrderItems oi
                JOIN Products p ON oi.ProductId = p.ProductId
                WHERE oi.OrderId = @orderId
            ", new { orderId }).ToList();

            return items;
        }

        // ===============================
        // DELETE ORDER
        // ===============================
        public void DeleteOrder(int id)
        {
            using var conn = _db.GetConnection();

            conn.Execute("DELETE FROM Orders WHERE OrderId = @id", new { id });
        }
    }
}