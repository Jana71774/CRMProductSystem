using System.Collections.Generic;
using CRMProductSystem.Data;
using CRMProductSystem.Models;
using Dapper;
using System.Linq;

namespace CRMProductSystem.Services
{
    public class ProductService
    {
        private readonly DbConnection _db;

        public ProductService(DbConnection db)
        {
            _db = db;
        }

        public List<Product> GetProducts()
        {
            using var conn = _db.GetConnection();
            string sql = @"
                SELECT 
                    ProductId,
                    ProductName,
                    Price,
                    Description
                FROM Products
                ORDER BY ProductName";
            return conn.Query<Product>(sql).ToList();
        }

        public void AddProduct(Product product)
        {
            using var conn = _db.GetConnection();
            string sql = @"
                INSERT INTO Products(ProductName, Price, Description)
                VALUES(@ProductName, @Price, @Description)";
            conn.Execute(sql, product);
        }

        public List<Product> GetAll() 
        { 
            using var conn = _db.GetConnection(); 
            string sql = "SELECT * FROM Products ORDER BY ProductName"; 
            return conn.Query<Product>(sql).ToList(); 
        }
    }
}
