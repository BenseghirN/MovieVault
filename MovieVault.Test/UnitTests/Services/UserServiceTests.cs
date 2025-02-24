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
        public async Task GetAllUsersAsync_ShouldReturnListOfUsers_WhenUsersExist()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { UserId = 1, UserName = "TestUser1", Email = "test1@example.com" },
                new User { UserId = 2, UserName = "TestUser2", Email = "test2@example.com" },
                new User { UserId = 3, UserName = "TestUser3", Email = "test3@example.com" },
                new User { UserId = 4, UserName = "TestUser4", Email = "test4@example.com" },
                new User { UserId = 5, UserName = "TestUser5", Email = "test5@example.com" }
            };
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(expectedUsers);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedUsers);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var expectedUsers = new List<User>();
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(expectedUsers);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            result.Should().BeNullOrEmpty();
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

            var newUser = new User { UserName = "NewUser", Email = "exists@example.com" };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _userService.RegisterUserAsync(newUser, "Password123"));
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnCreatesUserSuccessfully_WhenValidUser()
        {
            // Arrange
            var newUser = new User { UserName = "NewUser", Email = "new@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync("new@example.com")).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(1);

            // Act
            var result = await _userService.RegisterUserAsync(newUser, "Password123");

            // Assert
            result.Should().Be(1);
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

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var updatedUser = new User { UserId = 1, UserName = "UpdatedUser", Email = "updated@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(1)).ReturnsAsync(updatedUser);
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync(true);

            // Act
            var result = await _userService.UpdateUserAsync(updatedUser);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldThrowInvalidOperationException_WhenUserDoesNotExist()
        {
            // Arrange
            var updatedUser = new User { UserId = 99, UserName = "NonExistentUser", Email = "nonexistent@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(99)).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.UpdateUserAsync(updatedUser));
        }
    }
}