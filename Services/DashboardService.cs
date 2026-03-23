using System.Collections.Generic;
using System.Linq;
using CRMProductSystem.Data;
using CRMProductSystem.ViewModels;
using Dapper;

namespace CRMProductSystem.Services
{
    public class DashboardService
    {
        private readonly DbConnection _db;

        public DashboardService(DbConnection db)
        {
            _db = db;
        }

        public int GetTotalTasks()
        {
            using var conn = _db.GetConnection();
            return conn.ExecuteScalar<int>("SELECT COUNT(*) FROM tasks");
        }

        public int GetCompletedTasks()
        {
            using var conn = _db.GetConnection();
            return conn.ExecuteScalar<int>("SELECT COUNT(*) FROM tasks WHERE status='Completed'");
        }

        public int GetPendingTasks()
        {
            using var conn = _db.GetConnection();
            return conn.ExecuteScalar<int>("SELECT COUNT(*) FROM tasks WHERE status!='Completed'");
        }

        public List<TaskStat> GetTaskStats()
        {
            using var conn = _db.GetConnection();

            string sql = @"SELECT status AS Status, COUNT(*) AS Count
                           FROM tasks
                           GROUP BY status";

            return conn.Query<TaskStat>(sql).ToList();
        }
    }
}