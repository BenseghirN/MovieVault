using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PeopleService> _logger;

        public PeopleService(IPeopleRepository peopleRepository, ILogger<PeopleService> logger)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
        }

        public async Task<int> CreatePersonAsync(Person person)
        {
            _logger.LogInformation("Creating new Person for person ID {personId}: {personFirstName} {personLastName} ", person.PersonId, person.FirstName, person.LastName);
            var result = await _peopleRepository.CreatePersonAsync(person);

            if (result > 0)
                _logger.LogInformation("Person registered successfully for person ID {personId}: {personFirstName} {personLastName} ", person.PersonId, person.FirstName, person.LastName);
            else
                _logger.LogError("Failed to register person for for person ID {personId}: {personFirstName} {personLastName} ", person.PersonId, person.FirstName, person.LastName);

            return result;
        }

        public async Task<bool> DeletePersonAsync(int personId)
        {
            _logger.LogInformation("Deleting person ID {personId}", personId);
            bool result = await _peopleRepository.DeletePersonAsync(personId);

            if (result)
                _logger.LogInformation("Person deleted successfully: {personId}", personId);
            else
                _logger.LogError("Failed to delete review: {personId}", personId);

            return result;
        }

        public async Task<IEnumerable<Person>> GetAllPeopleAsync(int offset, int limit)
        {
            _logger.LogInformation("Fetching all people with offset {offset} and limit {limit}", offset, limit);
            var people = await _peopleRepository.GetAllPeopleAsync(offset, limit);

            if (!people.Any()) _logger.LogWarning("No people found in database");

            return people;
        }

        public async Task<Person?> GetPersonByIdAsync(int personId)
        {
            _logger.LogInformation("Fetching person with ID {PersonId}", personId);
            var person = await _peopleRepository.GetPersonByIdAsync(personId);

            if (person == null)
            {
                _logger.LogWarning("Person with ID {personId} not found", personId);
                throw new KeyNotFoundException($"La personne avec l'ID: {personId} est introuvable.");
            }

            return person;
        }

        public async Task<bool> PersonExistsAsync(Person person)
        {
            _logger.LogInformation("Checking if person already exist in database: {person.FirstName} {personLastName}", person.FirstName, person.LastName);
            return await _peopleRepository.PersonExistsAsync(person);
        }
    }
}