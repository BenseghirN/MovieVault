using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MovieVault.Core.Services;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Test.UnitTests.Services
{
    public class MoviesPeopleServiceTests
    {
        private readonly Mock<IMoviesPeopleRepository> _moviesPeopleRepositoryMock;
        private readonly Mock<ILogger<MoviesPeopleService>> _loggerMock;
        private readonly MoviesPeopleService _moviesPeopleService;

        public MoviesPeopleServiceTests()
        {
            _moviesPeopleRepositoryMock = new Mock<IMoviesPeopleRepository>();
            _loggerMock = new Mock<ILogger<MoviesPeopleService>>();
            _moviesPeopleService = new MoviesPeopleService(_moviesPeopleRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddMoviePersonAsync_ShouldReturnTrue_WhenSuccessful()
        {
            var moviePerson = new MoviesPerson { MovieId = 1, PersonId = 10, Role = PersonRole.Director };
            _moviesPeopleRepositoryMock.Setup(repo => repo.AddMoviePersonAsync(moviePerson)).ReturnsAsync(true);

            var result = await _moviesPeopleService.AddMoviePersonAsync(moviePerson);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetMoviesPeopleByMovieAsync_ShouldReturnList_WhenPeopleExist()
        {
            var people = new List<MoviesPerson> { new MoviesPerson { MovieId = 1, PersonId = 10, Role = PersonRole.Actor } };
            _moviesPeopleRepositoryMock.Setup(repo => repo.GetMoviesPeopleByMovieAsync(1)).ReturnsAsync(people);

            var result = await _moviesPeopleService.GetMoviesPeopleByMovieAsync(1);

            result.Should().BeEquivalentTo(people);
        }

        [Fact]
        public async Task RemoveMoviePersonAsync_ShouldReturnTrue_WhenSuccessful()
        {
            _moviesPeopleRepositoryMock.Setup(repo => repo.RemoveMoviePersonAsync(1, 10)).ReturnsAsync(true);

            var result = await _moviesPeopleService.RemoveMoviePersonAsync(1, 10);

            result.Should().BeTrue();
        }
    }
}
