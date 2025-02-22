using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Core.TMDB;
using MovieVault.Data.Models;

namespace MovieVault.UI.UserControls
{
    public partial class SearchMoviesUserControl : UserControl
    {
        private readonly IMovieService _movieService;
        private readonly ITmdbService _tmdbService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SearchMoviesUserControl> _logger;
        public SearchMoviesUserControl(IMovieService movieService, ITmdbService tmdbService,
            ILogger<SearchMoviesUserControl> logger, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _movieService = movieService;
            _tmdbService = tmdbService;
            _logger = logger;
        }

        private async void ButtonSearch_Click(object sender, EventArgs e)
        {
            noMoviesLabel.Visible = false;
            string query = textBoxSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                MessageBox.Show("Veuillez entrer un titre de film.", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _logger.LogInformation("Searching for movies with query {query}", query);

            try
            {
                _logger.LogInformation("Searching locally for movies with query {query}", query);
                var localSearch = await _movieService.SearchMoviesAsync(title: query);
                _logger.LogInformation("Searching TMDB for movies with query {query}", query);
                var tmdbSearch = await _tmdbService.SearchMovieAsync(query);

                _logger.LogInformation("Merging results");
                var mergedList = MergeResults(localSearch, tmdbSearch);

                if (!mergedList.Any())
                {
                    _logger.LogInformation("No movies found locally or on TMDB with query {query}", query);
                    noMoviesLabel.Visible = true;
                    return;
                }
                _logger.LogInformation("{movieCount} movies found with query {query}", mergedList.Count(), query);
                DisplayMovies(mergedList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private List<Movie> MergeResults(IEnumerable<Movie> localSearch, List<Movie> tmdbSearch)
        {
            var movieDict = localSearch.ToDictionary(m => m.TMDBId, m => m);
            foreach (var movie in tmdbSearch)
            {
                if (!movieDict.ContainsKey(movie.TMDBId))
                {
                    movieDict[movie.TMDBId] = movie;
                }
                else if (movieDict[movie.TMDBId].MovieId == null)
                {
                    movieDict[movie.TMDBId] = movie;
                }
            }

            return movieDict.Values.ToList();
        }

        private void DisplayMovies(List<Movie> movies)
        {
            flowLayoutPanelResults.Controls.Clear();

            if (movies.Count == 0)
            {
                MessageBox.Show("Aucun film trouvé.", "Résultat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var movie in movies)
            {
                var movieControl = new MovieThumbnailUserControl(movie, _serviceProvider);
                flowLayoutPanelResults.Controls.Add(movieControl);
            }
        }

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSearch.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
    }
}
