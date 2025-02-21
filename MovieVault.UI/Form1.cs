using MovieVault.Core.Interfaces;
using MovieVault.Core.TMDB;
using MovieVault.Data.Models;

namespace MovieVault.UI
{
    public partial class Form1 : Form
    {
        private readonly IUserService _userService;
        private readonly ITmdbService _tmdbService;
        public Form1(ITmdbService tmdbService)
        {
            _tmdbService = tmdbService ?? throw new ArgumentNullException(nameof(tmdbService));
            InitializeComponent();
            moviesBindingSource = new BindingSource();
            moviesListBox.DisplayMember = "Title";
        }

        private async void searchButton_Click(object sender, EventArgs e)
        {
            moviesBindingSource.Clear();
            var searchQuery = searchTextBox.Text.Trim();
            var result = await _tmdbService.SearchMovieAsync(searchQuery);
            foreach (var movie in result)
            {
                if (string.IsNullOrEmpty(movie.Title))
                {
                    MessageBox.Show("Un ou plusieurs films ne contiennent pas de titre !");
                }
            }
            moviesBindingSource.DataSource = result.ToList();
            moviesListBox.DataSource = moviesBindingSource;

        }

        private async void moviesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            crewListBox.Items.Clear();
            var selectedMovie = (Movie)moviesListBox.SelectedItem;
            if (selectedMovie != null)
            {
                var people = await _tmdbService.GetMovieCastAsync((int)selectedMovie.TMDBId);

                foreach (var person in people)
                {
                    crewListBox.Items.Add($"{person.FirstName} {person.LastName} {person.Role}");
                }
            }


            //MessageBox.Show($"test: {selectedMovie.Synopsis}");
        }
    }
}
