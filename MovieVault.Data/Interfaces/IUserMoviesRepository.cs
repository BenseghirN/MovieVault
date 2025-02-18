using MovieVault.Data.Models;

namespace MovieVault.Data.Interfaces
{
    public interface IUserMoviesRepository
    {
        Task<bool> AddUserMovieAsync(UserMovie userMovie);
        Task<bool> RemoveUserMovieAsync(int userId, int movieId);
        Task<IEnumerable<UserMovie>> GetUserMoviesAsync(int userId);
    }
}