using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class MoviesPeopleService : IMoviesPeopleService
    {
        private readonly IMoviesPeopleRepository _moviesPeopleRepository;
        private readonly ILogger<MoviesPeopleService> _logger;

        public MoviesPeopleService(IMoviesPeopleRepository moviesPeopleRepository, ILogger<MoviesPeopleService> logger)
        {
            _moviesPeopleRepository = moviesPeopleRepository;
            _logger = logger;
        }
        public async Task<bool> AddMoviePersonAsync(MoviesPerson moviePerson)
        {
            _logger.LogInformation("Adding Person ID {personId} with Role {role} to Movie ID {movieId}", moviePerson.PersonId, moviePerson.Role, moviePerson.MovieId);
            bool result = await _moviesPeopleRepository.AddMoviePersonAsync(moviePerson);

            if (result)
                _logger.LogInformation("Person {personId} registered successfully for movie ID {movieId}", moviePerson.PersonId, moviePerson.MovieId);
            else
                _logger.LogError("Failed to add Person {personId} for movie ID {movieId}", moviePerson.PersonId, moviePerson.MovieId);

            return result;
        }

        public async Task<IEnumerable<MoviesPerson>> GetMoviesPeopleByMovieAsync(int movieId)
        {
            _logger.LogInformation("Fetching People linked to Movie ID {movieId}", movieId);
            var result = await _moviesPeopleRepository.GetMoviesPeopleByMovieAsync(movieId);

            if (!result.Any()) _logger.LogWarning("No Crew found for this movie: {movieId}", movieId);

            return result;
        }

        public async Task<IEnumerable<MoviesPerson>> GetMoviesPeopleByPersonAsync(int personId)
        {
            _logger.LogInformation("Fetching Movies linked to Person ID {personId}", personId);
            var result = await _moviesPeopleRepository.GetMoviesPeopleByPersonAsync(personId);

            if (!result.Any()) _logger.LogWarning("No Movies found for this Person: {personId}", personId);

            return result;
        }

        public async Task<bool> RemoveMoviePersonAsync(int movieId, int personId)
        {
            _logger.LogInformation("Removing Person ID {personId} from Movie ID {movieId}", personId, movieId);
            bool result = await _moviesPeopleRepository.RemoveMoviePersonAsync(movieId, personId);

            if (result)
                _logger.LogInformation("Person {personId} removed successfully for movie ID {movieId}", personId, movieId);
            else
                _logger.LogError("Failed to remove Person {personId} for movie ID {movieId}", personId, movieId);

            return result;
        }
    }
}