using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class UserMovieService : IUserMoviesService
    {
        private readonly IUserMoviesRepository _userMoviesRepository;
        private readonly ILogger<UserMovieService> _logger;

        public UserMovieService(IUserMoviesRepository userMoviesRepository, ILogger<UserMovieService> logger)
        {
            _userMoviesRepository = userMoviesRepository;
            _logger = logger;
        }

        public async Task<bool> AddMovieToUserAsync(UserMovie userMovie)
        {
            _logger.LogInformation("Adding movie with ID {movieId} to collection for user with ID {userId}", userMovie.MovieId, userMovie.UserId);

            var result = await _userMoviesRepository.AddUserMovieAsync(userMovie);
            if (result)
                _logger.LogInformation("Movie with ID {movieId} succefully registered into user with ID {userId} collection", userMovie.MovieId, userMovie.UserId);
            else
                _logger.LogError("Failed to register movie {movieId} for user {userId}", userMovie.MovieId, userMovie.UserId);

            return result;
        }

        public async Task<UserMovie?> GetUserMovieByIdAsync(int userId, int movieId)
        {
            _logger.LogInformation("Fetching User Movie details for User ID {userId} and Movie ID {movieId}", userId, movieId);

            var result = await _userMoviesRepository.GetUserMovieByIdAsync(userId, movieId);
            if (result == null)
            {
                _logger.LogWarning("Movie details for User ID {userId} and Movie ID {movieId} not found", userId, movieId);
                throw new KeyNotFoundException($"Les détails du film pour l'utilisateur {userId} et le film {movieId} sont introuvables.");
            }
            return result;
        }

        public async Task<IEnumerable<UserMovie>> GetUserMovieCollectionAsync(int userId)
        {
            _logger.LogInformation("Fetching movie collection for user ID {UserId}", userId);
            var result = await _userMoviesRepository.GetUserMovieCollectionAsync(userId);

            if (!result.Any())
                _logger.LogWarning("No movie found for User with ID {userId}", userId);

            return result;
        }

        public async Task<IEnumerable<UserMovie>> GetUserMoviesByMovieAsync(int movieId)
        {
            _logger.LogInformation("Fetching all users who have movie ID {movieId}", movieId);
            var result = await _userMoviesRepository.GetUserMoviesByMovieAsync(movieId);

            if (!result.Any())
                _logger.LogWarning("No users for this movie with ID {movieId}", movieId);

            return result;
        }

        public async Task<bool> RemoveMovieFromUserAsync(int userId, int movieId)
        {
            _logger.LogInformation("Removing movie ID {movieId} from user ID {userId} collection", movieId, userId);
            var result = await _userMoviesRepository.RemoveUserMovieAsync(userId, movieId);

            if (result)
                _logger.LogInformation("Movie {movieId} successfully removed from User {userId} collection", movieId, userId);
            else
                _logger.LogError("Failed to remove Movie {movieId} from User {userId} collection", movieId, userId);

            return result;
        }

        public async Task<bool> UpdateUserMovieStatusAsync(UserMovie userMovie)
        {
            _logger.LogInformation("Updating informations for User ID {userId} and Movie ID {movieId}", userMovie.UserId, userMovie.MovieId);
            bool result = await _userMoviesRepository.UpdateUserMovieAsync(userMovie);

            if (result)
                _logger.LogInformation("Informations updated succefully for User ID {userId} and Movie ID {movieId}", userMovie.UserId, userMovie.MovieId);
            else
                _logger.LogInformation("Failed to update informations for User ID {userId} and Movie ID {movieId}", userMovie.UserId, userMovie.MovieId);

            return result;
        }
    }
}