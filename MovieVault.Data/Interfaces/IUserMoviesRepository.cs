using MovieVault.Data.Models;

namespace MovieVault.Data.Interfaces
{
    public interface IUserMoviesRepository
    {
        Task<bool> AddUserMovieAsync(UserMovie userMovie);
        Task<bool> RemoveUserMovieAsync(int userId, int movieId);
        Task<IEnumerable<UserMovie>> GetUserMovieCollectionAsync(int userId);  // Fetch all movies for User
        Task<UserMovie?> GetUserMovieByIdAsync(int userId, int movieId);  // Fetch MovieUser Info (Comment, Status, ...)
        Task<IEnumerable<UserMovie>> GetUserMoviesByMovieAsync(int movieId);  // Fetch all Users related to a Povie
        Task<bool> UpdateUserMovieAsync(UserMovie userMovie);  // Update if needed (change status)
    }
}