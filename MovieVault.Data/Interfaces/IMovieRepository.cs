using Microsoft.Data.SqlClient;
using MovieVault.Data.Models;

namespace MovieVault.Data.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync(int offset, int limit);
        Task<Movie?> GetMovieByIdAsync(int movieId);
        Task<IEnumerable<Movie>> GetMovieByTitleAsync(string title);
        Task<IEnumerable<Movie>> GetMovieByReleaseYearAsync(int year);
        Task<IEnumerable<Movie>> SearchMoviesAsync(string? title, IEnumerable<int>? years, IEnumerable<string>? genres, IEnumerable<string>? directors, IEnumerable<string>? actors);
        Task<int> CreateMovieAsync(Movie movie, SqlTransaction? transaction = null);
        Task<bool> UpdateMovieAsync(Movie movie);
        Task<bool> DeleteMovieAsync(int movieId, SqlTransaction? transaction = null);
        Task<bool> MovieExistsAsync(Movie movie, SqlTransaction? transaction);
    }
}