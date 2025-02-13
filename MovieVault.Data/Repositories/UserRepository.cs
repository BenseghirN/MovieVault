using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Utilities;
using System.Data;

namespace MovieVault.Data.Repositories
{
    public class UserRepository(IDBHelper idbHelper) : IUserRepository
    {
        private readonly IDBHelper _idbHelper = idbHelper ?? throw new ArgumentNullException(nameof(idbHelper));

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var query = "SELECT * FROM Users";
            return await _idbHelper.ExecuteReaderAsync(query, MapToUser);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var query = "SELECT * FROM Users WHERE UserId = @UserId";
            var parameters = new SqlParameter[] { new SqlParameter("@UserId", userId) };

            var users = await _idbHelper.ExecuteReaderAsync(query, MapToUser, parameters);
            return users.SingleOrDefault();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";
            var parameters = new SqlParameter[] { new SqlParameter("@Email", email) };

            var users = await _idbHelper.ExecuteReaderAsync(query, MapToUser, parameters);
            return users.SingleOrDefault();
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            await using var connection = await _idbHelper.OpenConnectionAsync();
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

                var userId = await _idbHelper.ExecuteScalarAsync(query, (SqlTransaction)transaction, parameters);

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

            return await _idbHelper.ExecuteQueryAsync(query, parameters) > 0;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            await using var connection = await _idbHelper.OpenConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var query = "DELETE FROM Users WHERE UserId = @UserId";
                var parameters = new SqlParameter[] { new SqlParameter("@UserId", userId) };

                int rowsAffected = await _idbHelper.ExecuteQueryAsync(query, (SqlTransaction)transaction, parameters);

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

        private User MapToUser(IDataReader reader)
        {
            return new User
            {
                UserId = reader.SafeGet<int>("UserId"),
                UserName = reader.SafeGet<string>("UserName") ?? string.Empty,
                Email = reader.SafeGet<string>("Email") ?? string.Empty,
                PasswordHash = reader.SafeGet<string>("PasswordHash") ?? string.Empty
            };
        }
    }
}