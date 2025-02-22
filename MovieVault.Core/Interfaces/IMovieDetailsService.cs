using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IMovieDetailsService
    {
        Task<Movie> GetMovieDetailsForViewAsync(int movieId);
        Task<List<Person>> GetMovieCastAndCrewForViewAsync(int movieId);
    }
}