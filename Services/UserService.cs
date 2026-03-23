using System.Collections.Generic;
using System.Linq;
using CRMProductSystem.Data;
using CRMProductSystem.Models;
using Dapper;

namespace CRMProductSystem.Services
{
    public class UserService
    {
        private readonly DbConnection _db;

        public UserService(DbConnection db)
        {
            _db = db;
        }

        public List<User> GetUsers()
        {
        using var conn = _db.GetConnection();

string sql = @"
            SELECT 
            UserId,
            Username,
            Password,
            Role
            FROM Users";
            return conn.Query<User>(sql).ToList();
        }

        public void DeleteUser(int id)
        {
            using var conn = _db.GetConnection();
conn.Execute("DELETE FROM Users WHERE UserId=@id", new { id });
        }
        public List<User> GetAllEmployees()
        {
            using var conn = _db.GetConnection();
string sql = "SELECT * FROM Users WHERE Role = 'Employee'";
            return conn.Query<User>(sql).ToList();
        }
        
    }
}