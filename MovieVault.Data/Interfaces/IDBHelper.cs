using Microsoft.Data.SqlClient;
using System.Data;

namespace MovieVault.Data.Interfaces
{
    public interface IDBHelper
    {
        Task<SqlConnection> OpenConnectionAsync();
        Task CloseConnectionAsync(SqlConnection connection);
        //Task<SqlTransaction> BeginTransactionAsync(SqlConnection connection);

        Task<int> ExecuteQueryAsync(string query, params SqlParameter[] parameters);
        Task<int> ExecuteQueryAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters);

        Task<IEnumerable<T>> ExecuteReaderAsync<T>(string query, Func<IDataReader, T> map, params SqlParameter[] parameters);
        Task<IEnumerable<T>> ExecuteReaderAsync<T>(string query, Func<IDataReader, T> map, SqlTransaction transaction, params SqlParameter[] parameters);

        Task<object?> ExecuteScalarAsync(string query, params SqlParameter[] parameters);
        Task<object?> ExecuteScalarAsync(string query, SqlTransaction transaction, params SqlParameter[] parameters);

        Task<bool> TestConnectionAsync();
    }
}