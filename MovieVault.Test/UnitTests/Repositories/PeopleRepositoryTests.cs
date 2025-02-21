using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;
using System.Data;

namespace MovieVault.Test.UnitTests.Repositories
{
    public class PeopleRepositoryTests
    {
        private readonly Mock<IDBHelper> _dbHelperMock;
        private readonly IPeopleRepository _peopleRepository;

        public PeopleRepositoryTests()
        {
            _dbHelperMock = new Mock<IDBHelper>();
            _peopleRepository = new PeopleRepository(_dbHelperMock.Object);
        }

        [Fact]
        public async Task CreatePersonAsync_ShouldReturnPersonId_WhenInsertIsSuccessful()
        {
            var person = new Person { PersonId = 1, FirstName = "John", LastName = "Doe", TMDBId = 100 };
            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _peopleRepository.CreatePersonAsync(person);

            result.Should().Be(1);
        }

        [Fact]
        public async Task GetAllPeopleAsync_ShouldReturnList_WhenPeopleExist()
        {
            var expectedPeople = new List<Person> { new Person { PersonId = 1, FirstName = "John", LastName = "Doe" } };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Person>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedPeople);

            var result = await _peopleRepository.GetAllPeopleAsync(0, 10);

            result.Should().BeEquivalentTo(expectedPeople);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnPerson_WhenPersonExists()
        {
            var person = new Person { PersonId = 1, FirstName = "John", LastName = "Doe" };
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Person>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<Person> { person });

            var result = await _peopleRepository.GetPersonByIdAsync(1);

            result.Should().BeEquivalentTo(person);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnNull_WhenPersonDoesNotExist()
        {
            _dbHelperMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<Func<IDataReader, Person>>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(new List<Person>());

            var result = await _peopleRepository.GetPersonByIdAsync(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldReturnTrue_WhenDeletionIsSuccessful()
        {
            _dbHelperMock.Setup(db => db.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var result = await _peopleRepository.DeletePersonAsync(1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task PersonExistsAsync_ShouldReturnTrue_WhenPersonExists()
        {
            _dbHelperMock.Setup(db => db.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(1);

            var person = new Person { FirstName = "John", LastName = "Doe", TMDBId = 100 };
            var result = await _peopleRepository.PersonExistsAsync(person);

            result.Should().BeTrue();
        }
    }
}
