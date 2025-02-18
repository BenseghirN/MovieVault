using MovieVault.Data.Models;

namespace MovieVault.Data.Interfaces
{
    public interface IPeopleRepository
    {
        Task<IEnumerable<Person>> GetAllPeopleAsync(int offset, int limit);
        Task<Person?> GetPersonByIdAsync(int personId);
        Task<IEnumerable<Person>?> GetPeopleByNameAsync(string personName);
        Task<bool> PersonExistsAsync(Person person);
        Task<int> CreatePersonAsync(Person person);
        Task<bool> DeletePersonAsync(int personId);
    }
}