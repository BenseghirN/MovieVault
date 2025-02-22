using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Core.Services;
using MovieVault.Data.Models;

namespace MovieVault.UI.UserControls
{
    public partial class UserLibraryUserControl : UserControl
    {
        private readonly IUserMoviesService _userMoviesService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMovieService _movieService;
        private readonly ILogger<UserLibraryUserControl> _logger;
        public UserLibraryUserControl(IMovieService movieService, IUserMoviesService userMoviesService,
            ILogger<UserLibraryUserControl> logger, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _movieService = movieService;
            _userMoviesService = userMoviesService;
            _logger = logger;
            LoadUserName();
        }

        private void LoadUserName()
        {
            if (UserSession.IsLoggedIn)
            {
                labelUsername.Text = $"Bienvenue, {UserSession.CurrentUser.UserName}!";
            }
            else
            {
                labelUsername.Text = "Bienvenue !";
            }
        }

        private async Task LoadUserMovies()
        {
            noMoviesLabel.Visible = false;
            var userId = UserSession.CurrentUser.UserId;
            var movieList = new List<Movie>();
            if (UserSession.IsLoggedIn)
            {
                try
                {
                    _logger.LogInformation("Fetching movie collection for user with id {userId}", userId);
                    var movieCollection = await _userMoviesService.GetUserMovieCollectionAsync(userId);
                    if (movieCollection.Any())
                    {
                        _logger.LogInformation("Fetching movie informations for user with id {userId}", userId);
                        foreach (var userMovie in movieCollection)
                        {
                            var movie = await _movieService.GetMovieByIdAsync(userMovie.MovieId);
                            movieList.Add(movie);
                        }

                        DisplayMovies(movieList);
                    }
                    else noMoviesLabel.Visible = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while fetching movie collection for user with id {userId}", userId);
                    throw;
                }
            }
        }

        private void DisplayMovies(List<Movie> movies)
        {
            flowLayoutPanelMovies.Controls.Clear();
            foreach (var movie in movies)
            {
                var movieControl = new MovieThumbnailUserControl(movie, _serviceProvider);
                flowLayoutPanelMovies.Controls.Add(movieControl);
            }
        }

        private async void UserLibraryUserControl_Load(object sender, EventArgs e)
        {
            await LoadUserMovies();
        }

        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Refreshing user movie collection...");
            flowLayoutPanelMovies.Controls.Clear();
            await LoadUserMovies();
        }
    }
}
