using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;
using System.Data;

namespace MovieVault.Test.UnitTests.Repositories
{
    public class GenreRepositoryTests
    {
        private readonly Mock<IDBHelper> _dbHelperMock;
        private readonly IGenreRepository _genreRepository;

        public GenreRepositoryTests()
        {
            _dbHelperMock = new Mock<IDBHelper>();
            _genreRepository = new GenreRepository(_dbHelperMock.Object);
        }

        [Fact]
        public async Task CreateGenreAsync_ShouldReturnGenreId_WhenInsertIsSuccessful()
        {
            var genre = new Genre { GenreId = 1, GenreName = "Action", TMDBId = 100 };

            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(0);

            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(It.Is<string>(s => s.Contains("INSERT INTO Genres")), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _genreRepository.CreateGenreAsync(genre);

            result.Should().Be(1);
        }


        [Fact]
        public async Task GetAllGenresAsync_ShouldReturnList_WhenGenresExist()
        {
            var expectedGenres = new List<Genre> { new Genre { GenreId = 1, GenreName = "Action" } };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Genre>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedGenres);

            var result = await _genreRepository.GetAllGenresAsync();

            result.Should().BeEquivalentTo(expectedGenres);
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnGenre_WhenGenreExists()
        {
            var genre = new Genre { GenreId = 1, GenreName = "Action" };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Genre>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<Genre> { genre });

            var result = await _genreRepository.GetGenreByIdAsync(1);

            result.Should().BeEquivalentTo(genre);
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnNull_WhenGenreDoesNotExist()
        {
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Genre>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<Genre>());

            var result = await _genreRepository.GetGenreByIdAsync(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteGenreAsync_ShouldReturnTrue_WhenDeletionIsSuccessful()
        {
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _genreRepository.DeleteGenreAsync(1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GenreExistsAsync_ShouldReturnTrue_WhenGenreExists()
        {
            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var genre = new Genre { GenreId = 1, GenreName = "Action", TMDBId = 100 };
            var result = await _genreRepository.GenreExistsAsync(genre);

            result.Should().BeTrue();
        }
    }
}
