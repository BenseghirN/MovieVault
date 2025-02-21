using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MovieVault.Core.Services;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Test.UnitTests.Services
{
    public class UserMoviesServiceTests
    {
        private readonly Mock<IUserMoviesRepository> _userMoviesRepositoryMock;
        private readonly Mock<ILogger<UserMovieService>> _loggerMock;
        private readonly UserMovieService _userMovieService;

        public UserMoviesServiceTests()
        {
            _userMoviesRepositoryMock = new Mock<IUserMoviesRepository>();
            _loggerMock = new Mock<ILogger<UserMovieService>>();
            _userMovieService = new UserMovieService(_userMoviesRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddMovieToUserAsync_ShouldReturnTrue_WhenMovieIsAddedSuccessfully()
        {
            var userMovie = new UserMovie { UserId = 1, MovieId = 101 };
            _userMoviesRepositoryMock.Setup(repo => repo.AddUserMovieAsync(userMovie)).ReturnsAsync(true);

            var result = await _userMovieService.AddMovieToUserAsync(userMovie);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveMovieFromUserAsync_ShouldReturnTrue_WhenMovieIsRemovedSuccessfully()
        {
            _userMoviesRepositoryMock.Setup(repo => repo.RemoveUserMovieAsync(1, 101)).ReturnsAsync(true);

            var result = await _userMovieService.RemoveMovieFromUserAsync(1, 101);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetUserMovieCollectionAsync_ShouldReturnList_WhenMoviesExist()
        {
            var movies = new List<UserMovie> { new UserMovie { UserId = 1, MovieId = 101 } };
            _userMoviesRepositoryMock.Setup(repo => repo.GetUserMovieCollectionAsync(1)).ReturnsAsync(movies);

            var result = await _userMovieService.GetUserMovieCollectionAsync(1);

            result.Should().BeEquivalentTo(movies);
        }
    }
}
