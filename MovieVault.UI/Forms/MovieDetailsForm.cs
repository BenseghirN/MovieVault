using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Core.Services;
using MovieVault.Core.TMDB;
using MovieVault.Data.Models;
using MovieVault.UI.UserControls;

namespace MovieVault.UI.Forms
{
    public partial class MovieDetailsForm : Form
    {
        private readonly IMovieDetailsService _movieDetailsService;
        private readonly ITmdbService _tmdbService;
        private readonly IReviewService _reviewService;
        private readonly IMovieManagerService _movieManagerService;
        private readonly ILogger<MovieDetailsForm> _logger;
        private Movie? _movieForView;
        public MovieDetailsForm(IMovieDetailsService movieDetailsService, ITmdbService tmdbService, ILogger<MovieDetailsForm> logger, IReviewService reviewService, IMovieManagerService movieManagerService)
        {
            InitializeComponent();
            _movieDetailsService = movieDetailsService;
            _tmdbService = tmdbService;
            _logger = logger;
            _reviewService = reviewService;
            _movieManagerService = movieManagerService;
        }

        public async Task LoadMovieDetails(Movie movie)
        {
            await LoadCastAndCrew(movie);
            if (_movieForView == null)
            {
                _logger.LogError("No movie details were fetched.");
                return;
            }

            labelTitle.Text = $"{movie.Title}";
            labelReleaseYear.Text = $"({movie.ReleaseYear})";
            LoadPosterImage(movie.PosterUrl);
            textBoxSynopsis.Text = movie.Synopsis;
            labelGenres.Text = string.Join(", ", _movieForView.Genres.Select(g => g.GenreName));
            var directors = _movieForView.People?.Where(p => p.Role == PersonRole.Director);
            if (directors != null && directors.Any())
            {
                labelDirector.Text = "Réalisateur : " + string.Join(", ", directors.Select(d => $"{d.FirstName} {d.LastName}"));
            }
            else
            {
                labelDirector.Text = "Réalisateur : Non disponible";
            }
            var actors = _movieForView.People?.Where(p => p.Role == PersonRole.Actor);
            DisplayActors(actors);
        }

        private async Task LoadCastAndCrew(Movie movie)
        {
            _movieForView = movie.MovieId != 0 && movie.MovieId != null
                ? await FetchLocally(movie)
                : await FetchOnTMDB(movie);

            await LoadReview(movie.MovieId);

            if (_movieForView == null)
            {
                _logger.LogError("Failed to load movie details for {movieId}", movie.MovieId);
            }
        }

        private void DisplayActors(IEnumerable<Person> actors)
        {
            flowLayoutPanelActors.Controls.Clear(); // Nettoyer avant d'ajouter les nouveaux acteurs

            foreach (var actor in actors)
            {
                var actorThumbnail = new ActorThumbnailUserControl(actor);
                flowLayoutPanelActors.Controls.Add(actorThumbnail);
            }
        }

        private async Task<Movie> FetchOnTMDB(Movie movie)
        {
            int tmdbID = movie.TMDBId.Value;
            _logger.LogInformation("Movie has no movieId but TMDBId, fetching movie details online for movie with TMDBId {tmdbId}", tmdbID);
            try
            {
                _logger.LogInformation("GetMovieDetailsAsync on TMDB for movie with TMDBId {tmdbId}", tmdbID);
                var movieWithDetails = await _tmdbService.GetMovieDetailsAsync(tmdbID);

                _logger.LogInformation("GetMovieCastAsync on TMDB for movie with TMDBId {tmdbId}", tmdbID);
                var castAndCrew = await _tmdbService.GetMovieCastAsync(tmdbID);
                if (!castAndCrew.Any())
                {
                    _logger.LogWarning("No cast and crew found for movie with TMDBId {tmdbId}", tmdbID);
                }
                movieWithDetails.People ??= new List<Person>();
                movieWithDetails.People.AddRange(castAndCrew);

                return movieWithDetails;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching details on TMDB for movie with TMDBId {tmdbId}", tmdbID);
                throw;
            }
        }

        private async Task<Movie> FetchLocally(Movie movie)
        {
            _logger.LogInformation("Movie has movieId, fetching movie details locally for movie with id {movieId}", movie.MovieId);
            try
            {
                _logger.LogInformation("GetMovieDetailsForViewAsync for movie with id {movieId}", movie.MovieId);
                var movieWithDetails = await _movieDetailsService.GetMovieDetailsForViewAsync(movie.MovieId);

                _logger.LogInformation("GetMovieCastAndCrewForViewAsync for movie with id {movieId}", movie.MovieId);
                var castAndCrew = await _movieDetailsService.GetMovieCastAndCrewForViewAsync(movie.MovieId);
                if (!castAndCrew.Any())
                {
                    _logger.LogWarning("No cast and crew found for movie with id {movieId}", movie.MovieId);
                }
                movieWithDetails.People ??= new List<Person>();
                movieWithDetails.People.AddRange(castAndCrew);

                return movieWithDetails;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching details locally for movie with id {movieId}", movie.MovieId);
                throw;
            }
        }

        private void LoadPosterImage(string? imageUrl)
        {
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                try
                {
                    pictureBoxPoster.LoadAsync(imageUrl);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "Erreur lors du chargement du poster : {imageUrl}", imageUrl);
                    pictureBoxPoster.Image = Properties.Resources.noPoster; // Image par défaut
                }
            }
            else
            {
                pictureBoxPoster.Image = Properties.Resources.noPoster;
            }
        }

        private async Task LoadReview(int movieId)
        {
            _logger.LogInformation("Fetching reviews for movie with id {movieId}", movieId);

            var review = await _reviewService.GetReviewsByMovieIdAsync(movieId) ?? new List<Review>();

            var reviewList = review.Any() ? review : new List<Review>();

            reviewsBindingSource.DataSource = reviewList;
            listBoxReviews.DisplayMember = "Comment";
            listBoxReviews.DataSource = reviewsBindingSource;
        }

        private async void buttonAddToLibrary_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Adding movie with Title {movieTitle} into collection for user with id {userId}", _movieForView.Title, UserSession.CurrentUser.UserId);
            try
            {
                var insert = await _movieManagerService.AddMovieWithDetailsAsync(_movieForView, UserSession.CurrentUser.UserId,
                    _movieForView.People, _movieForView.Genres);
                if (insert)
                {
                    MessageBox.Show("Film ajouté avec succès.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error Adding movie with Title {movieTitle} into collection for user with id {userId}", _movieForView.Title, UserSession.CurrentUser.UserId);
                MessageBox.Show("Erreur lors de l'ajout du film dans la bibliotheque", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}
