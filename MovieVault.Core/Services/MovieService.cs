using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MovieVault.Core.Configuration;
using MovieVault.Core.Interfaces;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IDBHelper _dbHelper;
        private readonly ILogger<MovieService> _logger;

        public MovieService(
            IMovieRepository movieRepository,
            IDBHelper dbHelper,
            ILogger<MovieService> logger)
        {
            _movieRepository = movieRepository;
            _dbHelper = dbHelper;
            _logger = logger;
        }

        public async Task<int> CreateMovieAsync(Movie movie, SqlTransaction? transaction = null)
        {
            _logger.LogInformation("Creating a new movie: {title}", movie.Title);
            var result = await _movieRepository.CreateMovieAsync(movie);

            if (result > 0)
                _logger.LogInformation("Movie registered successfully: {movieTitle}", movie.Title);
            else
                _logger.LogError("Failed to register movie: {movieTitle}", movie.Title);

            return result;
        }

        public async Task<bool> DeleteMovieAsync(int movieId)
        {
            _logger.LogInformation("Attempting to delete movie with ID {movieId}", movieId);

            bool result = await _movieRepository.DeleteMovieAsync(movieId);

            if (result)
                _logger.LogInformation("Successfully deleted movie with ID {movieId}", movieId);
            else
                _logger.LogWarning("Failed to delete movie with ID {movieId} - Movie may not exist.", movieId);

            return result;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync(int offset, int limit)
        {
            _logger.LogInformation("Fetching all movies");

            var result = await _movieRepository.GetAllMoviesAsync(offset, limit);
            if (!result.Any())
            {
                _logger.LogWarning("No movies found in database");
            }
            return result;
        }

        public async Task<Movie?> GetMovieByIdAsync(int movieId)
        {
            _logger.LogInformation("Fetching movie by id: {id}", movieId);

            var result = await _movieRepository.GetMovieByIdAsync(movieId);
            if (result == null)
            {
                _logger.LogWarning("Movie with id: {movieId} not found.", movieId);
                throw new KeyNotFoundException($"Le film avec l'ID: {movieId} est introuvable.");
            }
            return result;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByReleaseYearAsync(int year)
        {
            _logger.LogInformation("Fetching movies by release year: {year}", year);

            var result = await _movieRepository.GetMovieByReleaseYearAsync(year);
            if (!result.Any())
            {
                _logger.LogWarning("No movies found for this year");
            }
            return result;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByTitleAsync(string title)
        {
            _logger.LogInformation("Fetching movies by title: {title}", title);

            var movies = await _movieRepository.GetMovieByTitleAsync(title);
            if (!movies.Any())
            {
                _logger.LogWarning("No movies found for this title");
            }
            return movies;
        }

        public async Task<bool> MovieExistAsync(Movie movie, SqlTransaction? transacion = null)
        {
            _logger.LogInformation("Checking if movie already exist in database: {movieTitle}", movie.Title);
            return await _movieRepository.MovieExistsAsync(movie, transacion);
        }

        public async Task<IEnumerable<Movie>> SearchMoviesAsync(
            string? title,
            IEnumerable<int>? years,
            IEnumerable<string>? genres,
            IEnumerable<string>? directors,
            IEnumerable<string>? actors)
        {
            _logger.LogInformation("Recherche de films avec filtres : Title={title}, Year={years}, Genre={genre}, Directors={directors}, Actors={actors}",
                                    title ?? "N/A",
                                    years != null ? string.Join(",", years) : "N/A",
                                    genres != null ? string.Join(",", genres) : "N/A",
                                    directors != null ? string.Join(",", directors) : "N/A",
                                    actors != null ? string.Join(",", actors) : "N/A"
            );

            var result = await _movieRepository.SearchMoviesAsync(title, years, genres, directors, actors) ?? new List<Movie>();
            if (!result.Any())
            {
                _logger.LogWarning("No movies found for this filter");
            }
            return result;
        }

        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            _logger.LogInformation("Updating movie ID: {movieId}", movie.MovieId);

            var existingMovie = await _movieRepository.GetMovieByIdAsync(movie.MovieId);
            if (existingMovie == null)
            {
                _logger.LogWarning("Movie not found: {movieId}", movie.MovieId);
                throw new InvalidOperationException("Aucun film trouvé.");
            }

            bool result = await _movieRepository.UpdateMovieAsync(movie);

            if (result)
                _logger.LogInformation("Movie updated successfully: {movieId}", movie.MovieId);
            else
                _logger.LogError("Failed to update movie: {movieId}", movie.MovieId);

            return result;
        }
    }
}