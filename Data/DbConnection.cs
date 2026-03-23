using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace CRMProductSystem.Data
{
    public class DbConnection
    {
        private readonly IConfiguration _config;

        public DbConnection(IConfiguration config)
        {
            _config = config;
        }
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        public IDbConnection CreateConnection()
        {
            var connString = _config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connString))
                throw new ArgumentException("Connection string 'DefaultConnection' is missing or empty!");

            return new MySqlConnection(connString);
        }
    }
}