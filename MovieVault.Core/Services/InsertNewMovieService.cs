using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class InsertNewMovieService : IMovieManagerService
    {
        private readonly IDatabaseConnection _dbHelper;
        private readonly IMovieService _movieService;
        private readonly IUserMoviesService _userMoviesService;
        private readonly IGenreService _genreService;
        private readonly IMoviesGenresService _movieGenreService;
        private readonly IPeopleService _peopleService;
        private readonly IMoviesPeopleService _moviePeopleService;
        private readonly ILogger<InsertNewMovieService> _logger;
        public InsertNewMovieService(
            IDatabaseConnection dBHelper,
            IMovieService movieService,
            IUserMoviesService userMoviesService,
            IGenreService genreService,
            IMoviesGenresService movieGenreService,
            IMoviesPeopleService moviePeopleService,
            IPeopleService peopleService,
            ILogger<InsertNewMovieService> logger
        )
        {
            _dbHelper = dBHelper;
            _movieService = movieService;
            _genreService = genreService;
            _peopleService = peopleService;
            _movieGenreService = movieGenreService;
            _userMoviesService = userMoviesService;
            _moviePeopleService = moviePeopleService;
            _logger = logger;
        }

        public async Task<bool> AddMovieWithDetailsAsync(Movie movie, int userId, List<Person> People, List<Genre> Genres)
        {
            _logger.LogInformation("Attempting to initiate movie creation process with movie {movieTitle}", movie.Title);

            var connection = await _dbHelper.OpenConnectionAsync();
            var transaction = await _dbHelper.BeginTransactionAsync(connection);

            if (await _movieService.MovieExistsAsync(movie, transaction))
            {
                _logger.LogInformation("Movie '{movieTitle}' already exists in the database. Linking it to User {userId}.", movie.Title, userId);
                return await LinkMovieToUserAsync(movie.MovieId, userId); // Movie already exists in DB, simply link to user
            }

            _logger.LogInformation("Movie '{movieTitle}' does not exist in the database. Proceeding with creation.", movie.Title);
            // Movie doesn't exist, starting process to add in DB
            // 1 INSERT MOVIE
            int movieId = await _movieService.CreateMovieAsync(movie, transaction);
            movie.MovieId = movieId;

            _logger.LogInformation("Movie '{movieTitle}' inserted, proceeding with genres.", movie.Title);
            // 2 INSERT GENRES
            await InsertGenreIfNotExists(movie);

            _logger.LogInformation("Genres inserted, proceeding with people.");
            // 3 INSERT PEOPLE
            await InsertPeopleIfNotExists(movie);

            _logger.LogInformation("People inserted, proceeding with links.");
            // 4 LINK PEOPLE AND GENRES TO MOVIE
            await LinkGenresToMovie(movie);
            await LinkGPeopleToMovie(movie);

            _logger.LogInformation("Links done, linking Movie {movieTitle} with User {userId}.", movieId, userId);
            // 5 LINK MOVIE TO USER
            var result = await LinkMovieToUserAsync(movie.MovieId, userId);
            if (!result)
            {
                transaction.Rollback();
                connection.Close();
                return result;
            }

            transaction.Commit();
            connection.Close();
            return result;
        }

        private async Task<bool> LinkMovieToUserAsync(int movieId, int userId)
        {
            var userMovie = new UserMovie
            {
                MovieId = movieId,
                UserId = userId,
                Status = MovieStatus.Unwatched
            };

            var result = await _userMoviesService.AddMovieToUserAsync(userMovie);
            if (result)
                _logger.LogInformation("Linked existing movie ID {MovieId} to user ID {UserId}", movieId, userId);

            return result;
        }

        private async Task LinkGenresToMovie(Movie movie)
        {
            foreach (var genre in movie.Genres)
            {
                if (genre.GenreId > 0)
                {
                    await _movieGenreService.AddMovieGenreAsync(movie.MovieId, genre.GenreId);
                }
            }
        }

        private async Task LinkGPeopleToMovie(Movie movie)
        {
            foreach (var person in movie.People)
            {
                if (person.PersonId > 0)
                {
                    var moviePerson = new MoviesPerson
                    {
                        MovieId = movie.MovieId,
                        PersonId = person.PersonId,
                        Role = person.Role.Value
                    };

                    await _moviePeopleService.AddMoviePersonAsync(moviePerson);
                }
            }
        }

        private async Task InsertPeopleIfNotExists(Movie movie)
        {
            foreach (var person in movie.People)
            {
                if (!person.TMDBId.HasValue)
                {
                    _logger.LogWarning("Person '{firstName} {lastName}' has no TMDBId, skipping.", person.FirstName, person.LastName);
                    continue;
                }

                var tmdbId = person.TMDBId.Value;

                var personExists = await _peopleService.PersonExistsAsync(person);
                if (!personExists)
                {
                    _logger.LogInformation("Creating missing person '{firstName} {lastName}' (TMDB ID {tmdbId})",
                        person.FirstName, person.LastName, tmdbId);
                    person.PersonId = await _peopleService.CreatePersonAsync(person);
                }
            }
        }

        private async Task InsertGenreIfNotExists(Movie movie)
        {
            foreach (var genre in movie.Genres)
            {
                if (!genre.TMDBId.HasValue)
                {
                    _logger.LogWarning("Genre '{genreName}' has no TMDBId, skipping.", genre.GenreName);
                    continue;
                }
                var tmdbId = genre.TMDBId.Value;
                var genreName = GenreOfMovie.GetGenreNameByTMDBId(tmdbId);

                if (string.IsNullOrEmpty(genreName))
                {
                    _logger.LogWarning("TMDBId {tmdbId} not found in dictionary, skipping.", tmdbId);
                    continue;
                }

                if (!await _genreService.GenreExistsAsync(genre))
                {
                    _logger.LogInformation("Creating missing genre '{genreName}' (TMDB ID {tmdbId})", genreName, tmdbId);
                    genre.GenreId = await _genreService.CreateGenreAsync(new Genre { TMDBId = tmdbId, GenreName = genreName });
                }
            }
        }
    }
}