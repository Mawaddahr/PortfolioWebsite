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
            _connectionString = _config.GetConnectionString("ConnectionString");
        }
        public async Task<IDbConnection> CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task Init()
        {
            await InitTables();
        }

        private async Task InitTables()
        {
            string createTables = @"
            CREATE TABLE IF NOT EXISTS Experiences(
                id TEXT PRIMARY KEY,
                company TEXT VARCHAR(30) NOT NULL,
                location VARCHAR(50) TEXT NOT NULL,
                role TEXT VARCHAR(20) NOT NULL,
                description TEXT VARCHAR(500),
                startdate DATE NOT NULL,
                enddate DATE,
                duration TEXT
            );
            
            CREATE TABLE IF NOT EXISTS Education(
                id TEXT PRIMARY KEY,
                institution TEXT VARCHAR(50) NOT NULL,
                studyprogram TEXT VARCHAR(50) NOT NULL,
                studyprogramtype TEXT VARCHAR(50) NOT NULL,
                startdate DATE NOT NULL,
                estimatedenddate DATE NOT NULL,
                ongoing BOOLEAN NOT NULL,
                enddate DATE
            );

            CREATE TABLE IF NOT EXISTS Projects(
                id TEXT PRIMARY KEY,
                name TEXT VARCHAR(50) NOT NULL,
                description TEXT VARCHAR(500),
                link TEXT,
                imageurl TEXT
            );
            ";

            var connection = await CreateConnection();
            await connection.ExecuteAsync(createTables);
        }
    }
}
