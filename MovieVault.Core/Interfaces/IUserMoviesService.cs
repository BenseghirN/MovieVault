using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IUserMoviesService
    {
        Task<bool> AddMovieToUserAsync(UserMovie userMovie);
        Task<bool> RemoveMovieFromUserAsync(int userId, int movieId);
        Task<IEnumerable<UserMovie>> GetUserMovieCollectionAsync(int userId);
        Task<UserMovie?> GetUserMovieByIdAsync(int userId, int movieId);
        Task<IEnumerable<UserMovie>> GetUserMoviesByMovieAsync(int movieId);
        Task<bool> UpdateUserMovieStatusAsync(UserMovie userMovie);
    }
}