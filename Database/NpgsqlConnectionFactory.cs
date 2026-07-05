using Npgsql;
using Dapper;
using System.Data;

namespace PortfolioWebsite.Database
{
    public interface IConnectionFactory
    {
        Task<IDbConnection> CreateConnection();
    }

    public class NpgsqlConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public NpgsqlConnectionFactory(IConfiguration config)
        {
            _config = config;
            _connectionString = config.GetConnectionString("ConnectionString");
            Console.WriteLine();
        }
        public async Task<IDbConnection> CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
