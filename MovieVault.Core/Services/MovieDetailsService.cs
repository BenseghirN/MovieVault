using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;
using MovieVault.Data.Repositories;

namespace MovieVault.Core.Services
{
    public class MovieDetailsService : IMovieDetailsService
    {
        private readonly ILogger<MovieDetailsService> _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IPeopleRepository _peopleRepository;
        private readonly IMoviesPeopleRepository _moviesPeopleRepository;
        public MovieDetailsService(
            ILogger<MovieDetailsService> logger,
            IMovieRepository movieRepository,
            IGenreRepository genreRepository,
            IPeopleRepository peopleRepository,
            IMoviesPeopleRepository moviesPeopleRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _peopleRepository = peopleRepository;
            _moviesPeopleRepository = moviesPeopleRepository;
        }

        public async Task<List<Person>> GetMovieCastAndCrewForViewAsync(int movieId)
        {
            try
            {
                _logger.LogInformation("Fetching cast and crew locally for movie id {movieId}", movieId);
                var moviePeople = await _moviesPeopleRepository.GetMoviesPeopleByMovieAsync(movieId);

                if (moviePeople == null || !moviePeople.Any())
                {
                    _logger.LogWarning("No cast found locally for movie id {movieId}", movieId);
                    return new List<Person>();
                }

                _logger.LogInformation("Cast and crew fetched succefully : {count} people found", moviePeople.Count());

                var castAndCrew = new HashSet<Person>();
                foreach (var pers in moviePeople)
                {
                    castAndCrew.Add(await _peopleRepository.GetPersonByIdAsync(pers.PersonId));
                }
                return castAndCrew.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching cast and crew localy for movie id {movieId}", movieId);
                return new List<Person>();
            }
        }

        public async Task<Movie?> GetMovieDetailsForViewAsync(int movieId)
        {
            _logger.LogInformation("Fetching movie details localy for movie id {movieId}", movieId);

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);
            if (movie == null)
            {
                _logger.LogInformation("Error fetching movie details, movie not found for movie id {movieId}", movieId);
                return null;
            }

            foreach (var item in movie.MoviesGenres)
            {
                movie.Genres.Add(await _genreRepository.GetGenreByIdAsync(item.GenreId));
            }
            // foreach (var pers in movie.MoviesPeople)
            // {
            //     movie.People.Add(await _peopleRepository.GetPersonByIdAsync(pers.PersonId));
            // }

            _logger.LogInformation("All information loaded succesfully for movie id {movieId}", movieId);
            return movie;
        }
    }
}