using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MovieVault.Core.Services;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Test.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var expectedUser = new User { UserId = 1, UserName = "TestUser", Email = "test@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(1)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldThrowsKeyNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(99)).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.GetUserByIdAsync(99));
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowsInvalidOperationException_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingUser = new User { UserId = 1, UserName = "ExistingUser", Email = "exists@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync("exists@example.com")).ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _userService.RegisterUserAsync("NewUser", "exists@example.com", "Password123"));
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnCreatesUserSuccessfully_WhenValidUser()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync("new@example.com")).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(true);

            // Act
            var result = await _userService.RegisterUserAsync("NewUser", "new@example.com", "Password123");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.DeleteUserAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _userService.DeleteUserAsync(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.DeleteUserAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _userService.DeleteUserAsync(99);

            // Assert
            result.Should().BeFalse();
        }
    }
}