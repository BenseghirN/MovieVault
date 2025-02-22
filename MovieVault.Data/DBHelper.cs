using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MovieVault.Data.Interfaces;
using System.Data;

namespace MovieVault.Data
{
    public class DBHelper : IDBHelper
    {
        private readonly IDatabaseConnection _databaseConnection;
        private readonly ILogger<DBHelper> _logger;

        public DBHelper(IDatabaseConnection databaseConnection, ILogger<DBHelper> logger)
        {
            _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SqlConnection> OpenConnectionAsync()
        {
            return await _databaseConnection.OpenConnectionAsync();
        }

        public async Task CloseConnectionAsync(SqlConnection connection)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
                _logger.LogInformation("SQL Connection Closed");
            }
        }

        //public async Task<SqlTransaction> BeginTransactionAsync(SqlConnection connection)
        //{
        //    if (connection == null || connection.State != ConnectionState.Open)
        //        throw new InvalidOperationException("Cannot start a transaction on a closed connection.");

        //    _logger.LogInformation("Starting SQL Transaction...");
        //    return await _databaseConnection.BeginTransactionAsync(connection);
        //}

        // For SQL `INSERT`, `UPDATE`, `DELETE`
        public async Task<int> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
        {
            return await ExecuteQueryWithTransactionAsync(query, null, parameters);
        }

        public async Task<int> ExecuteQueryAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            return await ExecuteQueryWithTransactionAsync(query, transaction, parameters);
        }

        private async Task<int> ExecuteQueryWithTransactionAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            bool isNewConnection = false;
            bool isNewTransaction = false;

            var connection = transaction?.Connection;
            if (connection == null)
            {
                connection = await OpenConnectionAsync();
                isNewConnection = true;
            }

            var localTransaction = transaction ?? await _databaseConnection.BeginTransactionAsync(connection);
            isNewTransaction = transaction == null;

            try
            {
                await using var command = new SqlCommand(query, connection, localTransaction);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation("Executing Query with Transaction: {query}", query);

                int result = await command.ExecuteNonQueryAsync();
                _logger.LogInformation("Query executed successfully in transaction, affected rows: {result}", result);

                if (transaction == null)
                    await localTransaction.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                if (transaction == null)
                    await localTransaction.RollbackAsync();

                _logger.LogError(ex, "SQL Execution Error in Transaction: {errorMessage}", ex.Message);
                throw;
            }
        }

        // For SQL `SELECT`
        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(string query, Func<IDataReader, T> map, params SqlParameter[] parameters)
        {
            return await ExecuteReaderWithTransactionAsync(query, map, null, parameters);
        }

        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(string query, Func<IDataReader, T> map, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            return await ExecuteReaderWithTransactionAsync(query, map, transaction, parameters);
        }

        private async Task<IEnumerable<T>> ExecuteReaderWithTransactionAsync<T>(string query, Func<IDataReader, T> map, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            var results = new List<T>();
            bool isNewConnection = false;
            bool isNewTransaction = false;

            var connection = transaction?.Connection;
            if (connection == null)
            {
                connection = await OpenConnectionAsync();
                isNewConnection = true;
            }

            var localTransaction = transaction ?? await _databaseConnection.BeginTransactionAsync(connection);
            isNewTransaction = transaction == null;

            try
            {
                await using var command = new SqlCommand(query, connection, localTransaction);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation("Executing Reader Query with Transaction: {query}", query);

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    results.Add(map(reader));
                }

                await reader.CloseAsync();

                if (transaction == null)
                    await localTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                if (transaction == null)
                    await localTransaction.RollbackAsync();

                _logger.LogError(ex, "SQL Reader Execution Error in Transaction: {errorMessage}", ex.Message);
                throw;
            }
            finally
            {
                if (transaction == null)
                    await connection.CloseAsync();
            }
            return results;
        }

        // For SQL `Scalar`
        public async Task<object?> ExecuteScalarAsync(string query, params SqlParameter[] parameters)
        {
            return await ExecuteScalarWithTransactionAsync(query, null, parameters);
        }

        public async Task<object?> ExecuteScalarAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            return await ExecuteScalarWithTransactionAsync(query, transaction, parameters);
        }

        private async Task<object?> ExecuteScalarWithTransactionAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            bool isNewConnection = false;
            bool isNewTransaction = false;

            var connection = transaction?.Connection;
            if (connection == null)
            {
                connection = await OpenConnectionAsync();
                isNewConnection = true;
            }

            var localTransaction = transaction ?? await _databaseConnection.BeginTransactionAsync(connection);
            isNewTransaction = transaction == null;

            try
            {
                await using var command = new SqlCommand(query, connection, localTransaction);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                _logger.LogInformation("Executing Scalar Query with Transaction: {query}", query);

                var result = await command.ExecuteScalarAsync();

                if (transaction == null)
                    await localTransaction.CommitAsync();

                return result != DBNull.Value ? result : null;
            }
            catch (Exception ex)
            {
                if (transaction == null)
                    await localTransaction.RollbackAsync();

                _logger.LogError(ex, "SQL Scalar Execution Error in Transaction: {errorMessage}", ex.Message);
                throw;
            }
            finally
            {
                if (transaction == null)
                    await connection.CloseAsync();
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