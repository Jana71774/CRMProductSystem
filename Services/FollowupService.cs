using System.Collections.Generic;
using CRMProductSystem.Data;
using CRMProductSystem.Models;
using Dapper;

namespace CRMProductSystem.Services
{
    public class FollowupService
    {
        private readonly DbConnection _db;

        public FollowupService(DbConnection db)
        {
            _db = db;
        }
        

        public List<TaskFollowup> GetByTask(int taskId)
        {
            using var conn = _db.GetConnection();

            return conn.Query<TaskFollowup>(
                "SELECT * FROM TaskFollowups WHERE TaskId = @TaskId",
                new { TaskId = taskId }).ToList();
        }
        public List<TaskFollowup> GetAll()
        {
        using var conn = _db.GetConnection();
        return conn.Query<TaskFollowup>("SELECT * FROM TaskFollowups").ToList();
        }

        public void Add(TaskFollowup f)
        {
            using var conn = _db.GetConnection();

            string sql = @"INSERT INTO TaskFollowups
                (TaskId, UserId, Notes, FollowupDate, SalesStatus)
                VALUES(@TaskId, @UserId, @Notes, @FollowupDate, @SalesStatus)";

            conn.Execute(sql, f);
        }
        public void UpdateStatus(int followupId, string status)
        {
            using var conn = _db.GetConnection();
            string sql = @"UPDATE TaskFollowups
                        SET SalesStatus = @SalesStatus
                        WHERE FollowupId = @FollowupId";
            conn.Execute(sql, new { FollowupId = followupId, SalesStatus = status });
        }
        public List<TaskFollowup> GetByUser(int userId)
        {
            using var conn = _db.GetConnection();
            return conn.Query<TaskFollowup>(
            "SELECT * FROM TaskFollowups WHERE UserId = @UserId",
            new { UserId = userId }).ToList();
        }
        public void UpdateStatusByUser(int followupId, string status, int userId)
        {
            using var conn = _db.GetConnection();
            string sql = @"UPDATE TaskFollowups
                   SET SalesStatus = @SalesStatus
                   WHERE FollowupId = @FollowupId AND UserId = @UserId";
            conn.Execute(sql, new
            {
                FollowupId = followupId,
                SalesStatus = status,
                UserId = userId
            });
}
    }
}
