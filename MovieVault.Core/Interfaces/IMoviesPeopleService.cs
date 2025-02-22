using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IMoviesPeopleService
    {
        Task<bool> AddMoviePersonAsync(MoviesPerson moviePerson);
        Task<bool> RemoveMoviePersonAsync(int movieId, int personId);
        Task<IEnumerable<MoviesPerson>> GetMoviesPeopleByMovieAsync(int movieId);
        Task<IEnumerable<MoviesPerson>> GetMoviesPeopleByPersonAsync(int personId);
    }
}