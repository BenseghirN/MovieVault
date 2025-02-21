using MovieVault.Data.Models;
using Newtonsoft.Json.Linq;

namespace MovieVault.Core.TMDB
{
    public interface ITmdbService
    {
        Task<bool> TestTmdbConnection();
        Task<List<Movie>> SearchMovieAsync(string query);
        Task<Movie> GetMovieDetailsAsync(int tmdbId);
        Task<List<Person>> GetMovieCastAsync(int tmdbId);
    }
}