using CRMProductSystem.Data;
using CRMProductSystem.Models;
using Dapper;
using System.Collections.Generic;

namespace CRMProductSystem.Services
{
    public class TaskService
    {
        private readonly DbConnection _db;

        public TaskService(DbConnection db)
        {
            _db = db;
        }

        // GET ALL TASKS (ADMIN)
        public List<TaskModel> GetAllTasks()
        {
            using var conn = _db.GetConnection();

            string sql = @"
                SELECT t.*, 
                       u.Username AS EmployeeName,
                       c.CustomerName,
                       p.ProductName
                FROM Tasks t
                LEFT JOIN Users u ON t.AssignedTo = u.UserId
                LEFT JOIN Customers c ON t.CustomerId = c.CustomerId
                LEFT JOIN Products p ON t.ProductId = p.ProductId
                ORDER BY t.CreatedDate DESC";

            return conn.Query<TaskModel>(sql).ToList();
        }

        // GET TASKS BY EMPLOYEE
        public List<TaskModel> GetTasksByUser(int userId)
        {
            using var conn = _db.GetConnection();

            string sql = @"
                SELECT t.*, 
                       u.Username AS EmployeeName,
                       c.CustomerName,
                       p.ProductName
                FROM Tasks t
                LEFT JOIN Users u ON t.AssignedTo = u.UserId
                LEFT JOIN Customers c ON t.CustomerId = c.CustomerId
                LEFT JOIN Products p ON t.ProductId = p.ProductId
                WHERE t.AssignedTo = @UserId
                ORDER BY t.CreatedDate DESC";

            return conn.Query<TaskModel>(sql, new { UserId = userId }).ToList();
        }

        // GET TASK BY ID
        public TaskModel GetTaskById(int id)
        {
            using var conn = _db.GetConnection();

            string sql = "SELECT * FROM Tasks WHERE TaskId = @TaskId";

            return conn.QueryFirstOrDefault<TaskModel>(sql, new { TaskId = id }) ?? throw new Exception(" task id not recived") ;
        }

        // ADD TASK
        public void AddTask(TaskModel task)
        {
            using var conn = _db.GetConnection();

            string sql = @"
                INSERT INTO Tasks 
                (Title, Description, AssignedTo, CustomerId, ProductId, Status, DueDate)
                VALUES 
                (@Title, @Description, @AssignedTo, @CustomerId, @ProductId, @Status, @DueDate)";

            conn.Execute(sql, task);
        }

        // UPDATE TASK
        public void UpdateTask(TaskModel task)
        {
            using var conn = _db.GetConnection();

            string sql = @"
                UPDATE Tasks SET
                    Title = @Title,
                    Description = @Description,
                    AssignedTo = @AssignedTo,
                    CustomerId = @CustomerId,
                    ProductId = @ProductId,
                    Status = @Status,
                    DueDate = @DueDate
                WHERE TaskId = @TaskId";

            conn.Execute(sql, task);
        }

        // DELETE TASK
        public void DeleteTask(int id)
        {
            using var conn = _db.GetConnection();

            string sql = "DELETE FROM Tasks WHERE TaskId = @TaskId";

            conn.Execute(sql, new { TaskId = id });
        }

        // UPDATE STATUS
        public void UpdateStatus(int id, string status)
        {
            using var conn = _db.GetConnection();

            string sql = "UPDATE Tasks SET Status = @Status WHERE TaskId = @TaskId";

            conn.Execute(sql, new { TaskId = id, Status = status });
        }

        public List<TaskModel> GetFollowups()
        {
            using var conn = _db.GetConnection();

            string sql = @"
                SELECT t.*, 
                       u.Username AS EmployeeName,
                       c.CustomerName,
                       p.ProductName
                FROM Tasks t
                LEFT JOIN Users u ON t.AssignedTo = u.UserId
                LEFT JOIN Customers c ON t.CustomerId = c.CustomerId
                LEFT JOIN Products p ON t.ProductId = p.ProductId
                WHERE t.Status = 'Pending'
                ORDER BY t.CreatedDate DESC";

            return conn.Query<TaskModel>(sql).ToList();
        }
    }
}
