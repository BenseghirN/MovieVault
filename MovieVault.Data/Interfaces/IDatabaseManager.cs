using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace MovieVault.Data.Interfaces
{
    public interface IDatabaseManager
    {
        Task<SqlConnection> OpenConnectionAsync();
        Task CloseConnectionAsync(SqlConnection connection);

        Task<int> ExecuteQueryAsync(string query, params SqlParameter[] parameters);
        Task<int> ExecuteQueryAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters);

        Task<object?> ExecuteScalarAsync(string query, params SqlParameter[] parameters);
        Task<object?> ExecuteScalarAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters);

        Task<DbDataReader> ExecuteReaderAsync(string query, params SqlParameter[] parameters);
        Task<DbDataReader> ExecuteReaderAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters);
    }
}