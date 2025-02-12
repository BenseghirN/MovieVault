using Microsoft.Extensions.Logging;
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

        public async Task<User> GetUserByIdAsync(int userId)
        {
            _logger.LogInformation($"Fetching user by ID: {userId}");
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User with ID: {userId} not found.");
                throw new KeyNotFoundException($"L'utilisateur avec l'ID: {userId} est introuvable.");
            }
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation($"Fetching user by email: {email}");
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"User with email: {email} not found.");
                throw new KeyNotFoundException($"L'utilisateur avec l'email: {email} est introuvable.");
            }
            return user;
        }

        public async Task<bool> RegisterUserAsync(string userName, string email, string password)
        {
            _logger.LogInformation($"Registering new user: {email}");

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Les informations ne peuvent pas être vides.");

            if (await _userRepository.GetUserByEmailAsync(email) != null)
            {
                _logger.LogWarning($"Email already in use: {email}");
                throw new InvalidOperationException("Cet email est déjà utilisé.");
            }

            string hashedPassword = PasswordHasher.HashPassword(password);
            var user = new User { UserName = userName, Email = email, PasswordHash = hashedPassword };
            bool result = await _userRepository.CreateUserAsync(user);

            if (result)
                _logger.LogInformation($"User registered successfully: {email}");
            else
                _logger.LogError($"Failed to register user: {email}");

            return result;
        }

        public async Task<bool> UpdateUserAsync(int userId, string userName, string email, string password)
        {
            _logger.LogInformation($"Updating user ID: {userId}");

            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                _logger.LogWarning($"User not found: {userId}");
                throw new InvalidOperationException("Utilisateur non trouvé.");
            }

            existingUser.UserName = userName;
            existingUser.Email = email;
            existingUser.PasswordHash = PasswordHasher.HashPassword(password);

            bool result = await _userRepository.UpdateUserAsync(existingUser);

            if (result)
                _logger.LogInformation($"User updated successfully: {userId}");
            else
                _logger.LogError($"Failed to update user: {userId}");

            return result;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            _logger.LogInformation($"Deleting user ID: {userId}");

            bool result = await _userRepository.DeleteUserAsync(userId);

            if (result)
                _logger.LogInformation($"User deleted successfully: {userId}");
            else
                _logger.LogError($"Failed to delete user: {userId}");

            return result;
        }

        public async Task<bool> ValidatePasswordAsync(string email, string password)
        {
            _logger.LogInformation($"Validating password for user: {email}");

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"User not found for password validation: {email}");
                return false;
            }

            bool isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);
            if (isValid)
                _logger.LogInformation($"Password validation successful for user: {email}");
            else
                _logger.LogWarning($"Password validation failed for user: {email}");

            return isValid;
        }
    }
}