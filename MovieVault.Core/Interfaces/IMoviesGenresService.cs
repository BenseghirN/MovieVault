using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IMoviesGenresService
    {
        Task<bool> AddMovieGenreAsync(int movieId, int genreId);
        Task<bool> RemoveMovieGenreAsync(int movieId, int genreId);
        Task<IEnumerable<MoviesGenres>> GetMoviesGenresByMovieAsync(int movieId);
        Task<IEnumerable<MoviesGenres>> GetMoviesGenresByGenreAsync(int genreId);
    }
}