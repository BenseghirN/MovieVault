using MovieVault.Data.Models;

namespace MovieVault.Data.Interfaces
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync();
        Task<Genre?> GetGenreByIdAsync(int genreId);
        Task<bool> GenreExistsAsync(Genre genre);
        Task<int> CreateGenreAsync(Genre genre);
        Task<bool> DeleteGenreAsync(int genreId);
    }
}