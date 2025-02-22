using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MovieVault.Core.Services;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Test.UnitTests.Services
{
    public class GenreServiceTests
    {
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly Mock<ILogger<GenreService>> _loggerMock;
        private readonly GenreService _genreService;

        public GenreServiceTests()
        {
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _loggerMock = new Mock<ILogger<GenreService>>();
            _genreService = new GenreService(_genreRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateGenreAsync_ShouldReturnGenreId_WhenSuccessful()
        {
            var genre = new Genre { GenreId = 1, GenreName = "Action" };
            _genreRepositoryMock.Setup(repo => repo.CreateGenreAsync(genre)).ReturnsAsync(1);

            var result = await _genreService.CreateGenreAsync(genre);

            result.Should().Be(1);
        }

        [Fact]
        public async Task GetAllGenresAsync_ShouldReturnList_WhenGenresExist()
        {
            var genres = new List<Genre> { new Genre { GenreId = 1, GenreName = "Action" } };
            _genreRepositoryMock.Setup(repo => repo.GetAllGenresAsync()).ReturnsAsync(genres);

            var result = await _genreService.GetAllGenresAsync();

            result.Should().BeEquivalentTo(genres);
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnGenre_WhenGenreExists()
        {
            var genre = new Genre { GenreId = 1, GenreName = "Action" };
            _genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(1)).ReturnsAsync(genre);

            var result = await _genreService.GetGenreByIdAsync(1);

            result.Should().BeEquivalentTo(genre);
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldThrowKeyNotFoundException_WhenGenreDoesNotExist()
        {
            _genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(99)).ReturnsAsync((Genre)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _genreService.GetGenreByIdAsync(99));
        }
    }
}
