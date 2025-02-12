using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Utilities;
using System.Data;
using System.Data.Common;

namespace MovieVault.Data
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly string _connectionString = DatabaseManagerConfig.GetConnectionString();
        private readonly ILogger<DatabaseManager> _logger = DatabaseManagerConfig.GetLogger<DatabaseManager>();

        public async Task<SqlConnection> OpenConnectionAsync()
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

        public async Task CloseConnectionAsync(SqlConnection connection)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
                _logger.LogInformation("SQL Connection Closed");
            }
        }

        // For SQL `INSERT`, `UPDATE`, `DELETE`
        public async Task<int> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
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

        public async Task<int> ExecuteQueryAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            try
            {
                await using var command = new SqlCommand(query, transaction.Connection, transaction);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation($"Executing Query with Transaction: {query}");

                int result = await command.ExecuteNonQueryAsync();
                _logger.LogInformation($"Query executed successfully in transaction, affected rows: {result}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Execution Error in Transaction: {ex.Message}");
                throw;
            }
        }

        // For SQL `SELECT`
        public async Task<DbDataReader> ExecuteReaderAsync(string query, params SqlParameter[] parameters)
        {
            try
            {
                await using var connection = await OpenConnectionAsync();
                await using var command = new SqlCommand(query, connection);

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

        public async Task<DbDataReader> ExecuteReaderAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            try
            {
                await using var command = new SqlCommand(query, transaction.Connection, transaction);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation($"Executing Reader Query with Transaction: {query}");

                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Reader Execution Error in Transaction: {ex.Message}");
                throw;
            }
        }

        public async Task<object?> ExecuteScalarAsync(string query, params SqlParameter[] parameters)
        {
            try
            {
                await using var connection = await OpenConnectionAsync();
                await using var command = new SqlCommand(query, connection);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation($"Executing Scalar Query: {query}");

                var result = await command.ExecuteScalarAsync();
                return result != DBNull.Value ? result : null; // Return null if DBNull from SQL
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Scalar Execution Error: {ex.Message}");
                throw;
            }
        }

        public async Task<object?> ExecuteScalarAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            try
            {
                await using var command = new SqlCommand(query, transaction.Connection, transaction);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation($"Executing Scalar Query with Transaction: {query}");

                var result = await command.ExecuteScalarAsync();
                return result != DBNull.Value ? result : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Scalar Execution Error in Transaction: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> TestConnectionAsync()
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