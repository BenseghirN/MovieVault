using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IMovieDetails
    {
        Task<Movie> GetMovieDetailsForViewAsync(int movidId);
        Task<List<Person>> GetMovieCastAndCrewForViewAsync(int movieId);
    }
}