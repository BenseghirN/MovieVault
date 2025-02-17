using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IMovieManagerService
    {
        Task<int> AddMovieWithDetailsAsync(Movie movie);
    }
}