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

        public async Task<int> RegisterUserAsync(User user, string password)
        {
            _logger.LogInformation("Registering new user: {email}", user.Email);

            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Les informations ne peuvent pas être vides.");

            if (await _userRepository.GetUserByEmailAsync(user.Email) != null)
            {
                _logger.LogWarning($"Email already in use: {user.Email}");
                throw new InvalidOperationException("Cet email est déjà utilisé.");
            }

            user.PasswordHash = PasswordHasher.HashPassword(password);
            var result = await _userRepository.CreateUserAsync(user);

            if (result > 0)
                _logger.LogInformation("User registered successfully: {email}", user.Email);
            else
                _logger.LogError("Failed to register user: {email}", user.Email);

            return result;
        }

        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            _logger.LogInformation("Updating user ID: {userId}", updatedUser.UserId);

            var existingUser = await _userRepository.GetUserByIdAsync(updatedUser.UserId);
            if (existingUser == null)
            {
                _logger.LogWarning("User not found: {userId}", updatedUser.UserId);
                throw new InvalidOperationException("Utilisateur non trouvé.");
            }

            existingUser.UserName = updatedUser.UserName;
            existingUser.Email = updatedUser.Email;

            bool result = await _userRepository.UpdateUserAsync(existingUser);

            if (result)
                _logger.LogInformation("User updated successfully: {userId}", updatedUser.UserId);
            else
                _logger.LogError("Failed to update user: {userId}", updatedUser.UserId);

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

        public async Task<User> ValidatePasswordAsync(string email, string password)
        {
            _logger.LogInformation("Validating password for user: {email}", email);

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User not found for password validation: {email}", email);
                return null;
            }

            if (user.PasswordHash == null)
            {
                _logger.LogWarning("Password hash is null for user: {email}", email);
                return null;
            }

            bool isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);
            if (isValid)
                _logger.LogInformation("Password validation successful for user: {email}", email);
            else
                _logger.LogWarning("Password validation failed for user: {email}", email);

            return user;
        }
    }
}