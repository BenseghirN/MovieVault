using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Utilities;
using System.Data;

namespace MovieVault.Data
{
    public class DBHelper : IDBHelper
    {
        private readonly string _connectionString = DBHelperConfiguration.GetConnectionString();
        private readonly ILogger<DBHelper> _logger = DBHelperConfiguration.GetLogger<DBHelper>();

        public async Task<SqlConnection> OpenConnectionAsync()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                _logger.LogError("Connection string is null or empty.");
                throw new InvalidOperationException("Database connection string is not set.");
            }

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
                _logger.LogError(ex, "Error opening SQL connection: {errorMessage}", ex.Message);
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

                _logger.LogInformation("Executing Query: {query}", query);

                int result = await command.ExecuteNonQueryAsync();
                _logger.LogInformation("Query executed successfully, affected rows: {nbRows}", result);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SQL Execution Error: {errorMessage}", ex.Message);
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

                _logger.LogInformation("Executing Query with Transaction: {query}", query);

                int result = await command.ExecuteNonQueryAsync();
                _logger.LogInformation("Query executed successfully in transaction, affected rows: {result}", result);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SQL Execution Error in Transaction: {errorMessage}", ex.Message);
                throw;
            }
        }

        // For SQL `SELECT`
        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(string query, Func<IDataReader, T> map, params SqlParameter[] parameters)
        {
            var results = new List<T>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand(query, connection);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation("Executing Reader Query: {Query}", query);

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                while (await reader.ReadAsync())
                {
                    results.Add(map(reader)); // Map the data to the object
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SQL Reader Execution Error: {errorMessage}", ex.Message);
                throw;
            }

            return results;
        }

        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(string query, Func<IDataReader, T> map, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            var results = new List<T>();

            try
            {
                await using var command = new SqlCommand(query, transaction.Connection, transaction);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation("Executing Reader Query with Transaction: {query}", query);

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    results.Add(map(reader));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SQL Reader Execution Error in Transaction: {errorMessage}", ex.Message);
                throw;
            }

            return results;
        }

        public async Task<object?> ExecuteScalarAsync(string query, params SqlParameter[] parameters)
        {
            try
            {
                await using var connection = await OpenConnectionAsync();
                await using var command = new SqlCommand(query, connection);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation("Executing Scalar Query: {query}", query);

                var result = await command.ExecuteScalarAsync();
                return result != DBNull.Value ? result : null; // Return null if DBNull from SQL
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SQL Scalar Execution Error: {errorMessage}", ex.Message);
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

                _logger.LogInformation("Executing Scalar Query with Transaction: {query}", query);

                var result = await command.ExecuteScalarAsync();
                return result != DBNull.Value ? result : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SQL Scalar Execution Error in Transaction: {errorMessage}", ex.Message);
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
                _logger.LogError(ex, "Test done: Database connection failed: {errorMessage}", ex.Message);
                return false;
            }
        }
    }
}