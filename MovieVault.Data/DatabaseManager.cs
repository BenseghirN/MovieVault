using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Data;

namespace MovieVault.Data
{
    public static class DatabaseManager
    {
        private static readonly string _connectionString;
        private static readonly Microsoft.Extensions.Logging.ILogger _logger;
        private static readonly ILoggerFactory _loggerFactory;

        static DatabaseManager()
        {
            var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MovieVault", "logs", "database.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);

            // Initiate Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7) // Keep 7 days
                .CreateLogger();

            Log.Information("Initializing DatabaseManager...");
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });
            _logger = _loggerFactory.CreateLogger("DatabaseManager");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = config.GetConnectionString("MovieVaultDB")
                ?? throw new InvalidOperationException("Database connection string is missing.");
        }

        public static async Task<SqlConnection> OpenConnectionAsync()
        {
            try
            {
                _logger.LogInformation("Opening SQL Connection...");
                var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                _logger.LogInformation("SQL Connection Opened");
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error opening SQL connection: {ex.Message}");
                throw;
            }
        }

        public static async Task CloseConnectionAsync(SqlConnection connection)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
                _logger.LogInformation("SQL Connection Closed");
            }
        }

        // For SQL `INSERT`, `UPDATE`, `DELETE`
        public static async Task<int> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
        {
            try
            {
                await using var connection = await OpenConnectionAsync();
                await using var command = new SqlCommand(query, connection);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation($"Executing Query: {query}");

                int result = await command.ExecuteNonQueryAsync();
                _logger.LogInformation($"Query executed successfully, affected rows: {result}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Execution Error: {ex.Message}");
                throw;
            }
        }

        // For SQL `SELECT`
        public static async Task<SqlDataReader> ExecuteReaderAsync(string query, params SqlParameter[] parameters)
        {
            try
            {
                var connection = await OpenConnectionAsync();
                var command = new SqlCommand(query, connection);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation($"Executing Reader Query: {query}");

                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Reader Execution Error: {ex.Message}");
                throw;
            }
        }

        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                await using var connection = await OpenConnectionAsync();
                _logger.LogInformation("Test done: Database connection successful!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Test done: Database connection failed: {ex.Message}");
                return false;
            }
        }
    }
}