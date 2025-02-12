using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Data.Repositories
{
    public class UserRepository(IDatabaseManager databaseManager) : IUserRepository
    {
        private readonly IDatabaseManager _databaseManager = databaseManager ?? throw new ArgumentNullException(nameof(databaseManager));

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var query = "SELECT * FROM Users WHERE UserId = @UserId";
            var parameters = new SqlParameter[] { new SqlParameter("@UserId", userId) };

            using (var reader = await _databaseManager.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return new User
                    {
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash"))
                    };
                }
            }
            return null;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";
            var parameters = new SqlParameter[] { new SqlParameter("@Email", email) };

            using (var reader = await _databaseManager.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return new User
                    {
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash"))
                    };
                }
            }
            return null;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            await using var connection = await _databaseManager.OpenConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var query = "INSERT INTO Users (UserName, Email, PasswordHash) OUTPUT INSERTED.UserId VALUES (@UserName, @Email, @PasswordHash)";
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserName", user.UserName),
                    new SqlParameter("@Email", user.Email),
                    new SqlParameter("@PasswordHash", user.PasswordHash)
                };

                var userId = await _databaseManager.ExecuteScalarAsync(query, (SqlTransaction)transaction, parameters);

                if (userId == null)
                    throw new Exception("Échec de la création de l'utilisateur.");

                // Commit if success
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Full rollback if error
                await transaction.RollbackAsync();
                throw new Exception($"Erreur lors de la transaction: {ex.Message}");
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var query = "UPDATE Users SET UserName = @UserName, Email = @Email, PasswordHash = @PasswordHash WHERE UserId = @UserId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", user.UserId),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PasswordHash", user.PasswordHash)
            };

            return await _databaseManager.ExecuteQueryAsync(query, parameters) > 0;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            await using var connection = await _databaseManager.OpenConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var query = "DELETE FROM Users WHERE UserId = @UserId";
                var parameters = new SqlParameter[] { new SqlParameter("@UserId", userId) };

                int rowsAffected = await _databaseManager.ExecuteQueryAsync(query, (SqlTransaction)transaction, parameters);

                if (rowsAffected > 0)
                {
                    await transaction.CommitAsync();
                    return true;
                }

                await transaction.RollbackAsync();
                return false;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erreur lors de la suppression de l'utilisateur: {ex.Message}");
            }
        }
    }
}
