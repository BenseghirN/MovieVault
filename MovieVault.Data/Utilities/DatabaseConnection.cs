using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MovieVault.Data.Configuration;
using MovieVault.Data.Interfaces;
using System.Data;

namespace MovieVault.Data.Utilities
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseConnection> _logger;
        private SqlConnection _connection;

        public DatabaseConnection(ILogger<DatabaseConnection> logger)
        {
            _connectionString = DBHelperConfiguration.GetConnectionString();
            _connection = new SqlConnection(_connectionString);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SqlConnection> OpenConnectionAsync()
        {
            _logger.LogInformation("Opening SQL Connection...");
            if (_connection.State == System.Data.ConnectionState.Closed || _connection.State == System.Data.ConnectionState.Broken)
            {
                await _connection.OpenAsync();
            }
            _logger.LogInformation("SQL Connection Opened.");
            return _connection;
        }

        public async Task<SqlTransaction> BeginTransactionAsync(SqlConnection? existing = null)
        {
            _logger.LogInformation("Starting SQL Transaction...");
            var connection = existing ?? await OpenConnectionAsync();
            return (SqlTransaction)await connection.BeginTransactionAsync();
        }

        public async Task CloseConnectionAsync(SqlConnection connection)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
                _logger.LogInformation("SQL Connection Closed");
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _logger.LogInformation("Disposing Database Connection.");
                _connection.Dispose();
            }
        }
    }
}