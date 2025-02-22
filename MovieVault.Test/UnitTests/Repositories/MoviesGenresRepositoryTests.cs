using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;
using System.Data;

namespace MovieVault.Test.UnitTests.Repositories
{
    public class MoviesGenresRepositoryTests
    {
        private readonly Mock<IDBHelper> _dbHelperMock;
        private readonly IMoviesGenresRepository _moviesGenresRepository;

        public MoviesGenresRepositoryTests()
        {
            _dbHelperMock = new Mock<IDBHelper>();
            _moviesGenresRepository = new MoviesGenresRepository(_dbHelperMock.Object);
        }

        [Fact]
        public async Task AddMovieGenreAsync_ShouldReturnTrue_WhenInsertIsSuccessful()
        {
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _moviesGenresRepository.AddMovieGenreAsync(1, 2);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetMoviesGenresByGenreAsync_ShouldReturnList_WhenGenresExist()
        {
            var expectedGenres = new List<MoviesGenres> { new MoviesGenres { MovieId = 1, GenreId = 2 } };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, MoviesGenres>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedGenres);

            var result = await _moviesGenresRepository.GetMoviesGenresByGenreAsync(2);

            result.Should().BeEquivalentTo(expectedGenres);
        }

        [Fact]
        public async Task GetMoviesGenresByMovieAsync_ShouldReturnList_WhenMoviesExist()
        {
            var expectedMovies = new List<MoviesGenres> { new MoviesGenres { MovieId = 1, GenreId = 2 } };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, MoviesGenres>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedMovies);

            var result = await _moviesGenresRepository.GetMoviesGenresByMovieAsync(1);

            result.Should().BeEquivalentTo(expectedMovies);
        }

        [Fact]
        public async Task RemoveMovieGenreAsync_ShouldReturnTrue_WhenDeletionIsSuccessful()
        {
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _moviesGenresRepository.RemoveMovieGenreAsync(1, 2);

            result.Should().BeTrue();
        }
    }
}
