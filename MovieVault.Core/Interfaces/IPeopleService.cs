using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IPeopleService
    {
        Task<IEnumerable<Person>> GetAllPeopleAsync(int offset, int limit);
        Task<Person?> GetPersonByIdAsync(int personId);
        Task<bool> PersonExistsAsync(Person person);
        Task<int> CreatePersonAsync(Person person);
        Task<bool> DeletePersonAsync(int personId);
    }
}