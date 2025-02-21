using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;
using System.Data;

namespace MovieVault.Test.UnitTests.Repositories
{
    public class UserMovieRepositoryTests
    {
        private readonly Mock<IDBHelper> _dbHelperMock;
        private readonly IUserMoviesRepository _userMovieRepository;

        public UserMovieRepositoryTests()
        {
            _dbHelperMock = new Mock<IDBHelper>();
            _userMovieRepository = new UserMovieRepository(_dbHelperMock.Object);
        }

        [Fact]
        public async Task AddUserMovieAsync_ShouldReturnTrue_WhenInsertIsSuccessful()
        {
            var userMovie = new UserMovie { UserId = 1, MovieId = 101, Status = MovieStatus.Watched, Owned = true };
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _userMovieRepository.AddUserMovieAsync(userMovie);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetUserMovieByIdAsync_ShouldReturnUserMovie_WhenExists()
        {
            var expectedUserMovie = new UserMovie { UserId = 1, MovieId = 101, Status = MovieStatus.Watched, Owned = true };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, UserMovie>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<UserMovie> { expectedUserMovie });

            var result = await _userMovieRepository.GetUserMovieByIdAsync(1, 101);

            result.Should().BeEquivalentTo(expectedUserMovie);
        }

        [Fact]
        public async Task RemoveUserMovieAsync_ShouldReturnTrue_WhenDeletionIsSuccessful()
        {
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _userMovieRepository.RemoveUserMovieAsync(1, 101);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateUserMovieAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            var userMovie = new UserMovie { UserId = 1, MovieId = 101, Status = MovieStatus.Watched, Owned = false };
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _userMovieRepository.UpdateUserMovieAsync(userMovie);

            result.Should().BeTrue();
        }
    }
}
