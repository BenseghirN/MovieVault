using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;
using System.Data;

namespace MovieVault.Test.UnitTests.Repositories
{
    public class MovieRepositorySearchTests
    {
        private readonly Mock<IDBHelper> _dbHelperMock;
        private readonly IMovieRepository _movieRepository;

        public MovieRepositorySearchTests()
        {
            _dbHelperMock = new Mock<IDBHelper>();
            _movieRepository = new MovieRepository(_dbHelperMock.Object);
        }

        //Test 1 : Recherche par titre
        [Fact]
        public async Task SearchMoviesAsync_ShouldReturnMovies_WhenSearchingByTitle()
        {
            var expectedMovies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "Inception", ReleaseYear = 2010, Duration = 148 },
                new Movie { MovieId = 2, Title = "Interstellar", ReleaseYear = 2014, Duration = 169 }
            };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Movie>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedMovies);

            var result = await _movieRepository.SearchMoviesAsync("Inception", null, null, null, null);

            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(expectedMovies);
        }

        // Test 2 : Recherche par année
        [Fact]
        public async Task SearchMoviesAsync_ShouldReturnMovies_WhenSearchingByYears()
        {
            var expectedMovies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "Joker", ReleaseYear = 2019, Duration = 122 },
                new Movie { MovieId = 2, Title = "Parasite", ReleaseYear = 2019, Duration = 132 }
            };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Movie>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedMovies);

            var result = await _movieRepository.SearchMoviesAsync(null, new List<int> { 2019 }, null, null, null);

            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(expectedMovies);
        }

        // Test 3 : Recherche par genre
        [Fact]
        public async Task SearchMoviesAsync_ShouldReturnMovies_WhenSearchingByGenre()
        {
            var expectedMovies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "The Dark Knight", ReleaseYear = 2008, Duration = 152 }
            };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Movie>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedMovies);

            var result = await _movieRepository.SearchMoviesAsync(null, null, "Action", null, null);

            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(expectedMovies);
        }

        // Test 4 : Recherche par réalisateur
        [Fact]
        public async Task SearchMoviesAsync_ShouldReturnMovies_WhenSearchingByDirector()
        {
            var expectedMovies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "Dunkirk", ReleaseYear = 2017, Duration = 106 }
            };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Movie>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedMovies);

            var result = await _movieRepository.SearchMoviesAsync(null, null, null, new List<string> { "Christopher Nolan" }, null);

            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(expectedMovies);
        }

        // Test 5 : Recherche par acteur
        [Fact]
        public async Task SearchMoviesAsync_ShouldReturnMovies_WhenSearchingByActor()
        {
            var expectedMovies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "Titanic", ReleaseYear = 1997, Duration = 195 }
            };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Movie>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedMovies);

            var result = await _movieRepository.SearchMoviesAsync(null, null, null, null, new List<string> { "Leonardo DiCaprio" });

            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(expectedMovies);
        }

        // Test 6 : Aucun résultat
        [Fact]
        public async Task SearchMoviesAsync_ShouldReturnEmptyList_WhenNoMoviesMatch()
        {
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Movie>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<Movie>());

            var result = await _movieRepository.SearchMoviesAsync("Unknown Movie", null, null, null, null);

            result.Should().BeEmpty();
        }

        // Test 7 : Recherche combinée (année + réalisateur)
        [Fact]
        public async Task SearchMoviesAsync_ShouldReturnMovies_WhenSearchingByYearAndDirector()
        {
            var expectedMovies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "Oppenheimer", ReleaseYear = 2023, Duration = 180 }
            };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Movie>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedMovies);

            var result = await _movieRepository.SearchMoviesAsync(null, new List<int> { 2023 }, null, new List<string> { "Christopher Nolan" }, null);

            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(expectedMovies);
        }

        [Fact]
        public async Task SearchMoviesAsync_ShouldGenerateCorrectQuery_WhenFilteringByGenre()
        {
            var genre = "Action";

            var expectedMovies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "The Dark Knight", ReleaseYear = 2008, Duration = 152 }
            };

            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.Is<string>(query => query.Contains("WHERE g.GenreName = @Genre")),
                    It.IsAny<Func<IDataReader, Movie>>(),
                    It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedMovies);

            var result = await _movieRepository.SearchMoviesAsync(null, null, genre, null, null);

            _dbHelperMock.Verify(db =>
                    db.ExecuteReaderAsync(
                        It.Is<string>(query => query.Contains("WHERE g.GenreName = @Genre")),
                        It.IsAny<Func<IDataReader, Movie>>(),
                        It.IsAny<SqlParameter[]>()
                    ),
                Times.Once);
        }
    }
}
