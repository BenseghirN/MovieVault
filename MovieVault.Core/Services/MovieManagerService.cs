using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Data;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public class MovieManagerService : IMovieManagerService
    {
        private readonly IDBHelper _dbHelper;
        private readonly IMovieService _movieService;
        // private readonly IGenreService _genreService;
        // private readonly IPeopleService _peopleService;
        private readonly ILogger<MovieManagerService> _logger;
        public MovieManagerService(
            IDBHelper dBHelper,
            IMovieService movieService,
            // IGenreService genreService,
            // IPeopleService peopleService,
            ILogger<MovieManagerService> logger
        )
        {
            _dbHelper = dBHelper;
            _movieService = movieService;
            _logger = logger;
        }

        public async Task<int> AddMovieWithDetailsAsync(Movie movie)
        {
            _logger.LogInformation("Attempting to initiate movie creation process with movie {movieTitle}", movie.Title);

            var connection = await _dbHelper.OpenConnectionAsync();
            var transaction = await _dbHelper.BeginTransactionAsync(connection);

            if (await _movieService.MovieExistAsync(movie, transaction))
            {
                return 0; // Le film existe déjà
            }

            throw new NotImplementedException();
            // foreach (var movieGenre in movie.MoviesGenres)
            // {
            //     movieGenre.Genre.GenreId = await _genreService.GetOrCreateGenreAsync(movieGenre, transaction);
            // }

            // movie.Director.PersonId = await _peopleService.GetOrCreatePersonAsync(movie.Director, transaction);

            // foreach (var actor in movie.Actors)
            // {
            //     actor.PersonId = await _peopleService.GetOrCreatePersonAsync(actor, transaction);
            // }

            // int movieId = await _movieRepository.CreateMovieAsync(movie, transaction);

            // foreach (var genre in movie.Genres)
            // {
            //     await _movieRepository.LinkMovieToGenreAsync(movieId, genre.GenreId, transaction);
            // }

            // await _movieRepository.LinkMovieToPersonAsync(movieId, movie.Director.PersonId, 1, transaction);

            // foreach (var actor in movie.Actors)
            // {
            //     await _movieRepository.LinkMovieToPersonAsync(movieId, actor.PersonId, 2, transaction);
            // }

            // return movieId;
        }
    }
}