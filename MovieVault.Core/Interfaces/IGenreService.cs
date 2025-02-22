using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync();
        Task<Genre?> GetGenreByIdAsync(int genreId);
        Task<bool> GenreExistsAsync(Genre genre);
        Task<int> CreateGenreAsync(Genre genre);
    }
}