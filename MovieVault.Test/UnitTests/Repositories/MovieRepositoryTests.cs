using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;
using System.Data;

namespace MovieVault.Test.UnitTests.Repositories
{
    public class MovieRepositoryTests
    {
        private readonly Mock<IDBHelper> _dbHelperMock;
        private readonly IMovieRepository _movieRepository;

        public MovieRepositoryTests()
        {
            _dbHelperMock = new Mock<IDBHelper>();
            _movieRepository = new MovieRepository(_dbHelperMock.Object);
        }

        [Fact]
        public async Task CreateMovieAsync_ShouldReturnMovieId_WhenMovieIsCreated()
        {
            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<SqlTransaction?>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(5);

            var newMovie = new Movie { Title = "New Movie", ReleaseYear = 2024, Duration = 120 };
            var result = await _movieRepository.CreateMovieAsync(newMovie);

            result.Should().Be(5);
        }

        [Fact]
        public async Task GetMovieByIdAsync_ShouldReturnMovie_WhenMovieExists()
        {
            var expectedMovie = new Movie { MovieId = 1, Title = "Inception", ReleaseYear = 2010, Duration = 148 };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Movie>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<Movie> { expectedMovie });

            var result = await _movieRepository.GetMovieByIdAsync(1);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedMovie);
        }

        [Fact]
        public async Task UpdateMovieAsync_ShouldReturnTrue_WhenMovieIsUpdated()
        {
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var movieToUpdate = new Movie { MovieId = 1, Title = "Updated Title", ReleaseYear = 2010, Duration = 150 };
            var result = await _movieRepository.UpdateMovieAsync(movieToUpdate);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteMovieAsync_ShouldReturnTrue_WhenMovieIsDeleted()
        {
            // Setup mock for ExecuteScalarAsync to return 0 (no user likes the movie)
            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(It.Is<string>(s => s.Contains("SELECT COUNT(*) FROM UserMovies")),
                    It.IsAny<SqlTransaction>(),
                    It.IsAny<SqlParameter[]>()))
                .ReturnsAsync((object)0);

            // Setup mock for ExecuteQueryAsync to delete movie relations and movie itself
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.Is<string>(s => s.Contains("DELETE FROM MoviesGenres")),
                    It.IsAny<SqlTransaction>(),
                    It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.Is<string>(s => s.Contains("DELETE FROM MoviesPeople")),
                    It.IsAny<SqlTransaction>(),
                    It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.Is<string>(s => s.Contains("DELETE FROM Movies WHERE MovieId")),
                    It.IsAny<SqlTransaction>(),
                    It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            // Act
            var result = await _movieRepository.DeleteMovieAsync(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteMovieAsync_ShouldReturnFalse_WhenMovieDoesNotExist()
        {
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(0);

            var result = await _movieRepository.DeleteMovieAsync(99);

            result.Should().BeFalse();
        }
    }
}