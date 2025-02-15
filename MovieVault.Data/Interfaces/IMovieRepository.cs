using MovieVault.Data.Models;

namespace MovieVault.Data.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(int movieId);
        Task<IEnumerable<Movie>> GetMovieByTitleAsync(string title);
        Task<IEnumerable<Movie>> GetMovieByReleaseYearAsync(int year);
        Task<IEnumerable<Movie>> SearchMoviesAsync(string? title, int? year, string? genre, string? director, string? actor);
        Task<int> CreateMovieAsync(Movie movie);
        Task<bool> UpdateMovieAsync(Movie movie);
        Task<bool> DeleteMovieAsync(int movieId);
    }
}