using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> RegisterUserAsync(string userName, string email, string password);
        Task<bool> UpdateUserAsync(int userId, string userName, string email, string password);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> ValidatePasswordAsync(string email, string password);
    }
}