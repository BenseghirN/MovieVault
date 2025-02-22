using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class MoviesGenresService : IMoviesGenresService
    {
        private readonly IMoviesGenresRepository _moviesGenresRepository;
        private readonly ILogger<MoviesGenresService> _logger;

        public MoviesGenresService(IMoviesGenresRepository moviesGenresRepository, ILogger<MoviesGenresService> logger)
        {
            _moviesGenresRepository = moviesGenresRepository;
            _logger = logger;
        }

        public async Task<bool> AddMovieGenreAsync(int movieId, int genreId)
        {
            _logger.LogInformation("Adding Genre ID {genreId} to Movie ID {movieId}", genreId, movieId);
            var result = await _moviesGenresRepository.AddMovieGenreAsync(movieId, genreId);

            if (result)
                _logger.LogInformation("Genre {genreId} registered successfully for movie ID {movieId}", genreId, movieId);
            else
                _logger.LogError("Failed to add Genre {genreId} for movie ID {movieId}", genreId, movieId);

            return result;
        }

        public async Task<IEnumerable<MoviesGenres>> GetMoviesGenresByGenreAsync(int genreId)
        {
            _logger.LogInformation("Fetching Movies for Genre ID {genreId}", genreId);
            var moviesForGenre = await _moviesGenresRepository.GetMoviesGenresByGenreAsync(genreId);

            if (!moviesForGenre.Any()) _logger.LogWarning("No movies found for this Ge,re: {genreId}", genreId);

            return moviesForGenre;
        }

        public async Task<IEnumerable<MoviesGenres>> GetMoviesGenresByMovieAsync(int movieId)
        {
            _logger.LogInformation("Fetching Genres for Movie ID {movieId}", movieId);
            var genresForMovie = await _moviesGenresRepository.GetMoviesGenresByMovieAsync(movieId);

            if (!genresForMovie.Any()) _logger.LogWarning("No Genres found for this movie: {movieId}", movieId);

            return genresForMovie;
        }

        public async Task<bool> RemoveMovieGenreAsync(int movieId, int genreId)
        {
            _logger.LogInformation("Removing Genre ID {genreId} from mMvie ID {movieId}", genreId, movieId);
            var result = await _moviesGenresRepository.RemoveMovieGenreAsync(movieId, genreId);

            if (result)
                _logger.LogInformation("Genre {genreId} deleted successfully for movie ID {movieId}", genreId, movieId);
            else
                _logger.LogError("Failed to remove Genre {genreId} for movie ID {movieId}", genreId, movieId);

            return result;
        }
    }
}