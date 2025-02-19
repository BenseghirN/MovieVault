using MovieVault.Data.Models;

namespace MovieVault.Data.Interfaces
{
    public interface IMoviesGenresRepository
    {
        Task<bool> AddMovieGenreAsync(int movieId, int genreId);
        Task<bool> RemoveMovieGenreAsync(int movieId, int genreId);
        Task<IEnumerable<MoviesGenres>?> GetMoviesGenresByMovieAsync(int movieId);
        Task<IEnumerable<MoviesGenres>?> GetMoviesGenresByGenreAsync(int genreId);
    }
}