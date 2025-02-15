using System.Data.SqlTypes;
using Microsoft.Extensions.Logging;
using MovieVault.Core.Configuration;
using MovieVault.Core.Interfaces;
using MovieVault.Core.Utilities;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = LoggingConfig.GetLogger<UserService>();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users");

            var users = await _userRepository.GetAllUsersAsync();
            if (!users.Any())
            {
                _logger.LogWarning("No users found in database");
            }
            return users;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            _logger.LogInformation("Fetching user by ID: {userId}", userId);
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID: {userId} not found.", userId);
                throw new KeyNotFoundException($"L'utilisateur avec l'ID: {userId} est introuvable.");
            }
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation("Fetching user by email: {email}", email);
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User with email: {email} not found.", email);
                throw new KeyNotFoundException($"L'utilisateur avec l'email: {email} est introuvable.");
            }
            return user;
        }

        public async Task<int> RegisterUserAsync(string userName, string email, string password)
        {
            _logger.LogInformation("Registering new user: {email}", email);

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Les informations ne peuvent pas être vides.");

            if (await _userRepository.GetUserByEmailAsync(email) != null)
            {
                _logger.LogWarning($"Email already in use: {email}");
                throw new InvalidOperationException("Cet email est déjà utilisé.");
            }

            string hashedPassword = PasswordHasher.HashPassword(password);
            var user = new User { UserName = userName, Email = email, PasswordHash = hashedPassword };
            var result = await _userRepository.CreateUserAsync(user);

            if (result > 0)
                _logger.LogInformation("User registered successfully: {email}", email);
            else
                _logger.LogError("Failed to register user: {email}", email);

            return result;
        }

        public async Task<bool> UpdateUserAsync(int userId, string userName, string email, string password)
        {
            _logger.LogInformation("Updating user ID: {userId}", userId);

            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                _logger.LogWarning("User not found: {userId}", userId);
                throw new InvalidOperationException("Utilisateur non trouvé.");
            }

            existingUser.UserName = userName;
            existingUser.Email = email;
            existingUser.PasswordHash = PasswordHasher.HashPassword(password);

            bool result = await _userRepository.UpdateUserAsync(existingUser);

            if (result)
                _logger.LogInformation("User updated successfully: {userId}", userId);
            else
                _logger.LogError("Failed to update user: {userId}", userId);

            return result;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            _logger.LogInformation("Deleting user ID: {userId}", userId);

            bool result = await _userRepository.DeleteUserAsync(userId);

            if (result)
                _logger.LogInformation("User deleted successfully: {userId}", userId);
            else
                _logger.LogError("Failed to delete user: {userId}", userId);

            return result;
        }

        public async Task<bool> ValidatePasswordAsync(string email, string password)
        {
            _logger.LogInformation("Validating password for user: {email}", email);

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User not found for password validation: {email}", email);
                return false;
            }

            if (user.PasswordHash == null)
            {
                _logger.LogWarning("Password hash is null for user: {email}", email);
                return false;
            }

            bool isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);
            if (isValid)
                _logger.LogInformation("Password validation successful for user: {email}", email);
            else
                _logger.LogWarning("Password validation failed for user: {email}", email);

            return isValid;
        }
    }
}