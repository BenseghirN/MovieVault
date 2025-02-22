using Microsoft.Data.SqlClient;
using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IMovieService
    {
        Task<int> CreateMovieAsync(Movie movie, SqlTransaction? transacion = null);
        Task<IEnumerable<Movie>> GetAllMoviesAsync(int offset, int limit);
        Task<Movie?> GetMovieByIdAsync(int movieId);
        Task<IEnumerable<Movie>> GetMoviesByTitleAsync(string title);
        Task<IEnumerable<Movie>> GetMoviesByReleaseYearAsync(int year);
        Task<bool> UpdateMovieAsync(Movie movie);
        Task<bool> DeleteMovieAsync(int movieId);
        Task<IEnumerable<Movie>> SearchMoviesAsync(string? title = null, IEnumerable<int>? years = null, IEnumerable<string>? genres = null, IEnumerable<string>? directors = null, IEnumerable<string>? actors = null);
        Task<bool> MovieExistsAsync(Movie movie, SqlTransaction? transacion = null);
    }
}