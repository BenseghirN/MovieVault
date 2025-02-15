using System.Data;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;

namespace MovieVault.Test.UnitTests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly Mock<IDBHelper> _dbHelperMock;
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests()
        {
            _dbHelperMock = new Mock<IDBHelper>();
            _userRepository = new UserRepository(_dbHelperMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            var expectedUser = new User { UserId = 1, UserName = "TestUser", Email = "test@example.com", PasswordHash = "hashedpassword" };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, User>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<User> { expectedUser });

            var result = await _userRepository.GetUserByIdAsync(1);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, User>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<User>());

            var result = await _userRepository.GetUserByIdAsync(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
        {
            var expectedUser = new User { UserId = 1, UserName = "TestUser", Email = "test@example.com", PasswordHash = "hashedpassword" };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, User>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<User> { expectedUser });

            var result = await _userRepository.GetUserByEmailAsync("test@example.com");

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnListOfUsers_WhenUsersExist()
        {
            var expectedUsers = new List<User>
            {
                new User { UserId = 1, UserName = "TestUser1", Email = "test1@example.com" },
                new User { UserId = 2, UserName = "TestUser2", Email = "test2@example.com" },
                new User { UserId = 3, UserName = "TestUser3", Email = "test3@example.com" },
                new User { UserId = 4, UserName = "TestUser4", Email = "test4@example.com" },
                new User { UserId = 5, UserName = "TestUser5", Email = "test5@example.com" }
            };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, User>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedUsers);

            var result = await _userRepository.GetAllUsersAsync();

            result.Should().BeEquivalentTo(expectedUsers);
            result.Should().HaveCount(5);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, User>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<User>());

            var result = await _userRepository.GetAllUsersAsync();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnTrue_WhenUserIsCreatedSuccessfully()
        {
            var newUser = new User { UserName = "NewUser", Email = "new@example.com", PasswordHash = "hashedpassword" };
            var expectedUserid = 1;

            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _userRepository.CreateUserAsync(newUser);

            result.Should().Be(expectedUserid);
            _dbHelperMock.Verify(db => db.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnTrue_WhenUserIsUpdatedSuccessfully()
        {
            var user = new User { UserId = 1, UserName = "updateduser", Email = "updateduser@example.com", PasswordHash = "newpasswordhash" };

            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _userRepository.UpdateUserAsync(user);

            result.Should().BeTrue();
            _dbHelperMock.Verify(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserIsDeletedSuccessfully()
        {
            int userId = 1;

            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _userRepository.DeleteUserAsync(userId);

            result.Should().BeTrue();
            _dbHelperMock.Verify(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenDeletionIsSuccessful()
        {
            int userId = 1;

            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _userRepository.DeleteUserAsync(userId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenNoRowsAffected()
        {
            int userId = 1;

            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(0);

            var result = await _userRepository.DeleteUserAsync(userId);

            result.Should().BeFalse();
        }
    }
}