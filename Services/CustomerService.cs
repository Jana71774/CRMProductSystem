using System.Collections.Generic;
using System.Linq;
using CRMProductSystem.Data;
using CRMProductSystem.Models;
using Dapper;

namespace CRMProductSystem.Services
{
    public class CustomerService
    {
        private readonly DbConnection _db;

        public CustomerService(DbConnection db)
        {
            _db = db;
        }

        public List<Customer> GetCustomers()
        {
            using var conn = _db.GetConnection();
            string sql = @"
                SELECT 
                    CustomerId,
                    CustomerName,
                    Phone,
                    Email,
                    Address
                FROM Customers
                ORDER BY CustomerName";
            return conn.Query<Customer>(sql).ToList();
        }

        public void AddCustomer(Customer customer)
        {
            using var conn = _db.GetConnection();
            string sql = @"
                INSERT INTO Customers(CustomerName, Phone, Email, Address)
                VALUES(@CustomerName, @Phone, @Email, @Address)";
            conn.Execute(sql, customer);
        }

        public Customer GetCustomerById(int id)
        {
            using var conn = _db.CreateConnection();

            string query = @"
                SELECT *
                FROM Customers
                WHERE CustomerId = @CustomerId";

            return conn.QueryFirstOrDefault<Customer>(query, new { CustomerId = id }) ?? throw new Exception("Customer not found");
        }

        public void UpdateCustomer(Customer customer)
        {
            using var conn = _db.GetConnection();
            string sql = @"
                UPDATE Customers
                SET CustomerName = @CustomerName,
                    Phone = @Phone,
                    Email = @Email,
                    Address = @Address
                WHERE CustomerId = @CustomerId";
            conn.Execute(sql, customer);
        }

        public void DeleteCustomer(int id)
        {
            using var conn = _db.GetConnection();
            string sql = "DELETE FROM Customers WHERE CustomerId = @id";
            conn.Execute(sql, new { id });
        }

        public List<Customer> GetAll()
        {
            return GetCustomers();
        }

        public Customer? GetById(int id)
        {
            using var conn = _db.GetConnection();
            string sql = @"
                SELECT 
                    CustomerId,
                    CustomerName,
                    Phone,
                    Email,
                    Address
                FROM Customers
                WHERE CustomerId = @id";
            return conn.QueryFirstOrDefault<Customer>(sql, new { id });
        }
    }
}
