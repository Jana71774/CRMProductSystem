using CRMProductSystem.Data;
using CRMProductSystem.Models;
using Dapper;

namespace CRMProductSystem.Services
{
    public class AuthService
    {
        private readonly DbConnection _db;

        public AuthService(DbConnection db)
        {
            _db = db;
        }

        public User Login(string username, string password)
        {
            using var conn = _db.GetConnection();

string query = @"
            SELECT 
                UserId,
                Username,
                Password,
                Role
            FROM Users
            WHERE Username=@username AND Password=@password";
            return conn.QueryFirstOrDefault<User>(query, new { username, password }) ?? throw new Exception("Invalid username or password") ;
        }

        public void Register(User user)
        {
            using var conn = _db.GetConnection();

string query = @"INSERT INTO Users(Username,Password,Role)
                             VALUES(@Username,@Password,@Role)";

            conn.Execute(query, user);
        }
    }
}