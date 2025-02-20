using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IMovieManagerService
    {
        Task<bool> AddMovieWithDetailsAsync(Movie movie, int userId, List<Person> People, List<Genre> Genres);
        // Task<Movie> FetchMovieWithDetailsAsync(int movieId);
    }
}