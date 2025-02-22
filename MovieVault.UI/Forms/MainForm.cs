using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Core.Services;
using MovieVault.Core.TMDB;
using MovieVault.Data.Models;
using MovieVault.UI.UserControls;

namespace MovieVault.UI.Forms
{
    public partial class MainForm : Form
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly LoginUserControl _loginUserControl;
        private readonly UserLibraryUserControl _userLibraryUserControl;
        private readonly SearchMoviesUserControl _searchMoviesUserControl;

        private readonly ILogger<MainForm> _logger;
        public MainForm(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            
            _loginUserControl = new LoginUserControl(
                new Action<User>(OnLoginSuccess),
                serviceProvider.GetRequiredService<IUserService>(),
                serviceProvider,
                serviceProvider.GetRequiredService<ILogger<LoginUserControl>>());
            
            _userLibraryUserControl = new UserLibraryUserControl(
                    _serviceProvider.GetRequiredService<IMovieService>(),
                    _serviceProvider.GetRequiredService<IUserMoviesService>(),
                    _serviceProvider.GetRequiredService<ILogger<UserLibraryUserControl>>())
                { Dock = DockStyle.Fill };

            _searchMoviesUserControl = new SearchMoviesUserControl(
                _serviceProvider.GetRequiredService<IMovieService>(),
                _serviceProvider.GetRequiredService<ITmdbService>(),
                _serviceProvider.GetRequiredService<ILogger<SearchMoviesUserControl>>())
                { Dock = DockStyle.Fill };
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Charger l'UserControl de connexion dans l'onglet Login
            tabPageLogin.Controls.Add(_loginUserControl);

            _loginUserControl.Location = new Point(
                (tabPageLogin.Width - _loginUserControl.Width) / 2,
                (tabPageLogin.Height - _loginUserControl.Height) / 2
            );

            // Supprimer les autres onglets au démarrage
            tabControlMain.TabPages.Remove(tabPageUserLibrary);
            //tabControlMain.TabPages.Remove(tabPageMovies);
            //tabControlMain.TabPages.Remove(tabPageUsers);
        }

        public void OnLoginSuccess(User user)
        {
            UserSession.SetUser(user);
            MessageBox.Show($"Bienvenue {user.UserName}", "Connexion réussie", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            tabControlMain.TabPages.Remove(tabPageLogin);

            tabControlMain.TabPages.Add(tabPageUserLibrary);

            tabPageUserLibrary.Controls.Add(_userLibraryUserControl);

            tabPageSearch.Controls.Add(_searchMoviesUserControl);

            //tabPageUserLibrary.Controls.Add(new HomeControl() { Dock = DockStyle.Fill });
            //tabPageMovies.Controls.Add(new MovieDetailsControl() { Dock = DockStyle.Fill });
            //tabPageUsers.Controls.Add(new UserManagementControl() { Dock = DockStyle.Fill });

            tabControlMain.SelectedTab = tabPageUserLibrary;
        }

    }
}
