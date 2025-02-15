using Microsoft.Data.SqlClient;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Utilities;
using System.Data;

namespace MovieVault.Data.Repositories
{
    public class UserRepository(IDBHelper iDbHelper) : IUserRepository
    {
        private readonly IDBHelper _iDbHelper = iDbHelper ?? throw new ArgumentNullException(nameof(iDbHelper));

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var query = "SELECT * FROM Users";
            return await _iDbHelper.ExecuteReaderAsync(query, MapToUser);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var query = "SELECT * FROM Users WHERE UserId = @UserId";
            var parameters = new SqlParameter[] { new SqlParameter("@UserId", userId) };

            var users = await _iDbHelper.ExecuteReaderAsync(query, MapToUser, parameters);
            return users.SingleOrDefault();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";
            var parameters = new SqlParameter[] { new SqlParameter("@Email", email) };

            var users = await _iDbHelper.ExecuteReaderAsync(query, MapToUser, parameters);
            return users.SingleOrDefault();
        }

        public async Task<int> CreateUserAsync(User user)
        {
            var query = "INSERT INTO Users (UserName, Email, PasswordHash) OUTPUT INSERTED.UserId VALUES (@UserName, @Email, @PasswordHash)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PasswordHash", user.PasswordHash)
            };

            var userId = await _iDbHelper.ExecuteScalarAsync(query, parameters);
            return userId != null ? (int)userId : 0;
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

            int rowsAffected = await _iDbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var query = "DELETE FROM Users WHERE UserId = @UserId";
            var parameters = new SqlParameter[] { new SqlParameter("@UserId", userId) };

            int rowsAffected = await _iDbHelper.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
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