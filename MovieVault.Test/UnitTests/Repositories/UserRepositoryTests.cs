using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;
using System.Data;
using MovieVault.Test.Fakes;

namespace MovieVault.Test.UnitTests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly Mock<IDatabaseManager> _dbManagerMock;
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests()
        {
            _dbManagerMock = new Mock<IDatabaseManager>();
            _userRepository = new UserRepository(_dbManagerMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            var expectedUser = new User { UserId = 1, UserName = "TestUser", Email = "test@example.com", PasswordHash = "hashedpassword" };

            var fakeData = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "UserId", expectedUser.UserId },
                    { "UserName", expectedUser.UserName },
                    { "Email", expectedUser.Email },
                    { "PasswordHash", expectedUser.PasswordHash }
                }
            };
            var readerMock = new FakeSqlDataReader(fakeData);

            _dbManagerMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(readerMock);

            var result = await _userRepository.GetUserByIdAsync(1);

            result.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var readerMock = new Mock<SqlDataReader>();
            readerMock.Setup(r => r.Read()).Returns(false);

            _dbManagerMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(readerMock.Object);

            var result = await _userRepository.GetUserByIdAsync(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnTrue_WhenUserIsCreated()
        {
            var newUser = new User { UserName = "NewUser", Email = "new@example.com", PasswordHash = "hashedpassword" };

            _dbManagerMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _userRepository.CreateUserAsync(newUser);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserIsDeleted()
        {
            _dbManagerMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _userRepository.DeleteUserAsync(1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            _dbManagerMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(0);

            var result = await _userRepository.DeleteUserAsync(99);

            result.Should().BeFalse();
        }
    }
}
