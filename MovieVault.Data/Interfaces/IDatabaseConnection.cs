using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace MovieVault.Data.Interfaces
{
    public interface IDatabaseConnection : IDisposable
    {
        Task<SqlConnection> OpenConnectionAsync();
        Task<SqlTransaction> BeginTransactionAsync(SqlConnection connection);
    }
}