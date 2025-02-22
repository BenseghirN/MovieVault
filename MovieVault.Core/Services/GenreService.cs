using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger<GenreService> _logger;

        public GenreService(IGenreRepository genreRepository, ILogger<GenreService> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<int> CreateGenreAsync(Genre genre)
        {
            _logger.LogInformation("Creating new genre: {GenreName}", genre.GenreName);
            var result = await _genreRepository.CreateGenreAsync(genre);

            if (result > 0)
                _logger.LogInformation("Genre registered successfully for ID {genreId}", genre.GenreId);
            else
                _logger.LogError("Failed to register genre for ID {genreId}", genre.GenreId);

            return result;
        }

        public async Task<bool> GenreExistsAsync(Genre genre)
        {
            _logger.LogInformation("Checking if genre with ID {genreId} exists", genre.GenreId);
            return await _genreRepository.GenreExistsAsync(genre);
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            _logger.LogInformation("Fetching all genres.");
            var genres = await _genreRepository.GetAllGenresAsync();
            if (!genres.Any()) _logger.LogWarning("No movie genres found in database");

            return genres;
        }

        public async Task<Genre?> GetGenreByIdAsync(int genreId)
        {
            _logger.LogInformation("Fetching genre with ID {genreId}", genreId);
            var genre = await _genreRepository.GetGenreByIdAsync(genreId);

            if (genre == null)
            {
                _logger.LogWarning("Genre with ID: {genreId} not found.", genreId);
                throw new KeyNotFoundException($"La genre de film avec l'ID: {genreId} est introuvable.");
            }
            return genre;
        }
    }
}