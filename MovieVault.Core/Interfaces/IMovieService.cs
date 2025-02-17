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
        Task<IEnumerable<Movie>> SearchMoviesAsync(string? title, IEnumerable<int>? years, IEnumerable<string>? genres, IEnumerable<string>? directors, IEnumerable<string>? actors);
        Task<bool> MovieExistAsync(Movie movie, SqlTransaction? transacion = null);
    }
}