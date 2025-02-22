using Microsoft.Extensions.DependencyInjection;
using MovieVault.Data.Models;
using MovieVault.UI.Forms;

namespace MovieVault.UI.UserControls
{
    public partial class MovieThumbnailUserControl : UserControl
    {
        private Movie _movie;
        private IServiceProvider _serviceProvider;
        public MovieThumbnailUserControl(Movie movie, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _movie = movie;
            UpdateUI();
        }

        private void UpdateUI()
        {
            labelTitle.Text = _movie.Title;

            if (!string.IsNullOrEmpty(_movie.PosterUrl))
            {
                try
                {
                    pictureBoxPoster.Load(_movie.PosterUrl);
                }
                catch
                {
                    pictureBoxPoster.Image = Properties.Resources.noPoster;
                }
            }
            else
            {
                pictureBoxPoster.Image = Properties.Resources.noPoster;
            }
        }

        private void MovieThumbnailUserControl_Click(object sender, EventArgs e)
        {
            //MessageBox.Show($"Détails du film : {_movie.Title}", "Film sélectionné", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var movieDetailsForm = _serviceProvider.GetRequiredService<MovieDetailsForm>();
            movieDetailsForm.LoadMovieDetails(_movie);
            movieDetailsForm.ShowDialog();
        }
    }
}
