using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MovieVault.Core.Services;
using MovieVault.Data.Interfaces;

namespace MovieVault.Test.UnitTests.Services
{
    public class MoviesGenresServiceTests
    {
        private readonly Mock<IMoviesGenresRepository> _moviesGenresRepositoryMock;
        private readonly Mock<ILogger<MoviesGenresService>> _loggerMock;
        private readonly MoviesGenresService _moviesGenresService;

        public MoviesGenresServiceTests()
        {
            _moviesGenresRepositoryMock = new Mock<IMoviesGenresRepository>();
            _loggerMock = new Mock<ILogger<MoviesGenresService>>();
            _moviesGenresService = new MoviesGenresService(_moviesGenresRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddMovieGenreAsync_ShouldReturnTrue_WhenSuccessful()
        {
            _moviesGenresRepositoryMock.Setup(repo => repo.AddMovieGenreAsync(1, 2)).ReturnsAsync(true);

            var result = await _moviesGenresService.AddMovieGenreAsync(1, 2);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveMovieGenreAsync_ShouldReturnTrue_WhenSuccessful()
        {
            _moviesGenresRepositoryMock.Setup(repo => repo.RemoveMovieGenreAsync(1, 2)).ReturnsAsync(true);

            var result = await _moviesGenresService.RemoveMovieGenreAsync(1, 2);

            result.Should().BeTrue();
        }
    }
}
