using MovieVault.Data.Models;

namespace MovieVault.UI.UserControls
{
    public partial class MovieThumbnailUserControl : UserControl
    {
        private Movie _movie;
        public MovieThumbnailUserControl(Movie movie)
        {
            InitializeComponent();
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
            MessageBox.Show($"Détails du film : {_movie.Title}", "Film sélectionné", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
