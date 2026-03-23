using System.Collections.Generic;
using System.Linq;
using CRMProductSystem.Data;
using CRMProductSystem.Models;
using Dapper;

namespace CRMProductSystem.Services
{
    public class OrderItemService
    {
        private readonly DbConnection _db;

        public OrderItemService(DbConnection db)
        {
            _db = db;
        }

        // Get all items for one order (used in Details page)
        public List<OrderItem> GetItemsByOrderId(int orderId)
        {
            using var conn = _db.GetConnection();

            var items = conn.Query<OrderItem>(@"
                SELECT 
                    oi.OrderItemId,
                    oi.OrderId,
                    oi.ProductId,
                    oi.Quantity,
                    p.ProductName,
                    p.Description,
                    p.Price
                FROM OrderItems oi
                JOIN Products p ON oi.ProductId = p.ProductId
                WHERE oi.OrderId = @orderId
            ", new { orderId }).ToList();

            return items;
        }

        // Add single order item (optional future use)
        public void AddItem(OrderItem item)
        {
            using var conn = _db.GetConnection();

            conn.Execute(@"
                INSERT INTO OrderItems (OrderId, ProductId, Quantity)
                VALUES (@OrderId, @ProductId, @Quantity)", item);
        }

        // Delete items of an order (used while updating order)
        public void DeleteItemsByOrderId(int orderId)
        {
            using var conn = _db.GetConnection();

            conn.Execute("DELETE FROM OrderItems WHERE OrderId = @orderId", new { orderId });
        }
    }
}