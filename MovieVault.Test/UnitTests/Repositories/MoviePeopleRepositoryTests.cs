using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;
using System.Data;

namespace MovieVault.Test.UnitTests.Repositories
{
    public class MoviePeopleRepositoryTests
    {
        private readonly Mock<IDBHelper> _dbHelperMock;
        private readonly IMoviesPeopleRepository _moviePeopleRepository;

        public MoviePeopleRepositoryTests()
        {
            _dbHelperMock = new Mock<IDBHelper>();
            _moviePeopleRepository = new MoviePeopleRepository(_dbHelperMock.Object);
        }

        [Fact]
        public async Task AddMoviePersonAsync_ShouldReturnTrue_WhenInsertIsSuccessful()
        {
            var moviePerson = new MoviesPerson { MovieId = 1, PersonId = 10, Role = PersonRole.Actor };
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _moviePeopleRepository.AddMoviePersonAsync(moviePerson);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetMoviesPeopleByMovieAsync_ShouldReturnList_WhenPeopleExist()
        {
            var expectedPeople = new List<MoviesPerson> { new MoviesPerson { MovieId = 1, PersonId = 10, Role = PersonRole.Director } };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, MoviesPerson>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedPeople);

            var result = await _moviePeopleRepository.GetMoviesPeopleByMovieAsync(1);

            result.Should().BeEquivalentTo(expectedPeople);
        }

        [Fact]
        public async Task GetMoviesPeopleByPersonAsync_ShouldReturnList_WhenMoviesExist()
        {
            var expectedMovies = new List<MoviesPerson> { new MoviesPerson { MovieId = 1, PersonId = 10, Role = PersonRole.Actor } };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, MoviesPerson>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedMovies);

            var result = await _moviePeopleRepository.GetMoviesPeopleByPersonAsync(10);

            result.Should().BeEquivalentTo(expectedMovies);
        }

        [Fact]
        public async Task RemoveMoviePersonAsync_ShouldReturnTrue_WhenDeletionIsSuccessful()
        {
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _moviePeopleRepository.RemoveMoviePersonAsync(1, 10);

            result.Should().BeTrue();
        }
    }
}
