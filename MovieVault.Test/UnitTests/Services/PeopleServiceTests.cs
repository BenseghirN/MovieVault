using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MovieVault.Core.Services;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Test.UnitTests.Services
{
    public class PeopleServiceTests
    {
        private readonly Mock<IPeopleRepository> _peopleRepositoryMock;
        private readonly Mock<ILogger<PeopleService>> _loggerMock;
        private readonly PeopleService _peopleService;

        public PeopleServiceTests()
        {
            _peopleRepositoryMock = new Mock<IPeopleRepository>();
            _loggerMock = new Mock<ILogger<PeopleService>>();
            _peopleService = new PeopleService(_peopleRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreatePersonAsync_ShouldReturnPersonId_WhenSuccessful()
        {
            var person = new Person { PersonId = 1, FirstName = "John", LastName = "Doe" };
            _peopleRepositoryMock.Setup(repo => repo.CreatePersonAsync(person)).ReturnsAsync(1);

            var result = await _peopleService.CreatePersonAsync(person);

            result.Should().Be(1);
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldReturnTrue_WhenSuccessful()
        {
            _peopleRepositoryMock.Setup(repo => repo.DeletePersonAsync(1)).ReturnsAsync(true);

            var result = await _peopleService.DeletePersonAsync(1);

            result.Should().BeTrue();
        }
    }
}
