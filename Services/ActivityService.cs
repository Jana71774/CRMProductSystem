using System;
using System.Collections.Generic;
using System.Linq;
using CRMProductSystem.Data;
using CRMProductSystem.Models;
using Dapper;

namespace CRMProductSystem.Services
{
    public class ActivityService
    {
        private readonly DbConnection _db;

        public ActivityService(DbConnection db)
        {
            _db = db;
        }

        public void AddLog(int customerId, string description)
        {
            using var conn = _db.CreateConnection();

            string query = @"
                INSERT INTO ActivityLogs (CustomerId, Description, ActivityDate)
                VALUES (@CustomerId, @Description, @ActivityDate)";

            conn.Execute(query, new
            {
                CustomerId = customerId,
                Description = description,
                ActivityDate = DateTime.Now
            });
        }

        // Get activity logs for one customer
        public List<ActivityLog> GetLogsByCustomer(int customerId)
        {
            using var conn = _db.CreateConnection();

            string query = @"
                SELECT * 
                FROM ActivityLogs
                WHERE CustomerId = @CustomerId
                ORDER BY ActivityDate DESC";

            return conn.Query<ActivityLog>(query, new { CustomerId = customerId }).ToList();
        }
        public List<ActivityLog> GetActivityReport(DateTime? fromDate, DateTime? toDate, int? userId = null, int? customerId = null)
        {
            using var conn = _db.GetConnection();

            string sql = @"
                SELECT a.ActivityId, a.CustomerId, c.CustomerName,
                    a.UserId, u.Username,
                    a.Description, a.ActivityDate
                FROM ActivityLogs a
                LEFT JOIN Customers c ON a.CustomerId = c.CustomerId
                LEFT JOIN Users u ON a.UserId = u.UserId
                WHERE (@fromDate IS NULL OR a.ActivityDate >= @fromDate)
                AND (@toDate IS NULL OR a.ActivityDate <= @toDate)
                AND (@userId IS NULL OR a.UserId = @userId)
                AND (@customerId IS NULL OR a.CustomerId = @customerId)
                ORDER BY a.ActivityDate DESC;
            ";

            return conn.Query<ActivityLog>(sql, new { fromDate, toDate, userId, customerId }).ToList();
        }
    }
}