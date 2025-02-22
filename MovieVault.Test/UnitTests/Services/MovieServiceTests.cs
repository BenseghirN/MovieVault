using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Moq;
using MovieVault.Core.Services;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Test.UnitTests.Services
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IDBHelper> _dbHelperMock;
        private readonly Mock<ILogger<MovieService>> _loggerMock;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _dbHelperMock = new Mock<IDBHelper>();
            _loggerMock = new Mock<ILogger<MovieService>>();
            _movieService = new MovieService(_movieRepositoryMock.Object, _dbHelperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateMovieAsync_ShouldReturnMovieId_WhenMovieIsCreated()
        {
            var movie = new Movie { Title = "Test Movie" };
            _movieRepositoryMock.Setup(repo => repo.CreateMovieAsync(movie, null)).ReturnsAsync(1);

            var result = await _movieService.CreateMovieAsync(movie);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetAllMoviesAsync_ShouldReturnMovies()
        {
            var movies = new List<Movie> { new Movie { Title = "Movie 1" }, new Movie { Title = "Movie 2" } };
            _movieRepositoryMock.Setup(repo => repo.GetAllMoviesAsync(0, 10)).ReturnsAsync(movies);

            var result = await _movieService.GetAllMoviesAsync(0, 10);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetMovieByIdAsync_ShouldThrowException_WhenMovieDoesNotExist()
        {
            _movieRepositoryMock.Setup(repo => repo.GetMovieByIdAsync(It.IsAny<int>())).ReturnsAsync((Movie?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _movieService.GetMovieByIdAsync(1));
        }

        [Fact]
        public async Task SearchMoviesAsync_ShouldReturnFilteredMovies()
        {
            var movies = new List<Movie> { new Movie { Title = "Filtered Movie" } };
            _movieRepositoryMock.Setup(repo => repo.SearchMoviesAsync(It.IsAny<string>(), It.IsAny<IEnumerable<int>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(movies);

            var result = await _movieService.SearchMoviesAsync("Filtered", null, null, null, null);
            Assert.Single(result);
        }

        [Fact]
        public async Task UpdateMovieAsync_ShouldReturnTrue_WhenMovieIsUpdated()
        {
            var movie = new Movie { MovieId = 1, Title = "Updated Movie" };
            _movieRepositoryMock.Setup(repo => repo.GetMovieByIdAsync(movie.MovieId)).ReturnsAsync(movie);
            _movieRepositoryMock.Setup(repo => repo.UpdateMovieAsync(movie)).ReturnsAsync(true);

            var result = await _movieService.UpdateMovieAsync(movie);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteMovieAsync_ShouldReturnFalse_WhenMovieIsInUserCollection()
        {
            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(
                It.Is<string>(s => s.Contains("SELECT COUNT(*) FROM UserMovies")),
                It.IsAny<SqlParameter[]>()
            )).ReturnsAsync((object)1);

            var result = await _movieService.DeleteMovieAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteMovieAsync_ShouldReturnFalse_WhenMovieIsNotInUserCollection()
        {
            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(
                It.Is<string>(s => s.Contains("SELECT COUNT(*) FROM UserMovies")),
                It.IsAny<SqlParameter[]>()
            )).ReturnsAsync((object)0);

            // Mock suppression réussie
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(
                It.Is<string>(s => s.Contains("DELETE FROM Movies")),
                It.IsAny<SqlParameter[]>()
            )).ReturnsAsync(1);

            var result = await _movieService.DeleteMovieAsync(1);

            Assert.False(result);
        }
    }
}