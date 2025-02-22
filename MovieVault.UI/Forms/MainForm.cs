using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Data.Models;
using MovieVault.UI.UserControls;

namespace MovieVault.UI.Forms
{
    public partial class MainForm : Form
    {
        private readonly LoginUserControl _loginUserControl;
        private readonly ILogger<MainForm> _logger;
        public MainForm(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _loginUserControl = new LoginUserControl(
                new Action<User>(OnLoginSuccess),
                serviceProvider.GetRequiredService<IUserService>(),
                serviceProvider,
                serviceProvider.GetRequiredService<ILogger<LoginUserControl>>());
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
            tabControlMain.TabPages.Remove(tabPageHome);
            tabControlMain.TabPages.Remove(tabPageMovies);
            tabControlMain.TabPages.Remove(tabPageUsers);
        }

        public void OnLoginSuccess(User user)
        {
            tabControlMain.TabPages.Remove(tabPageLogin); // Supprimer l'onglet Login
            tabControlMain.TabPages.Add(tabPageHome);
            tabControlMain.TabPages.Add(tabPageMovies);
            tabControlMain.TabPages.Add(tabPageUsers);

            //tabPageHome.Controls.Add(new HomeControl() { Dock = DockStyle.Fill });
            //tabPageMovies.Controls.Add(new MovieDetailsControl() { Dock = DockStyle.Fill });
            //tabPageUsers.Controls.Add(new UserManagementControl() { Dock = DockStyle.Fill });

            tabControlMain.SelectedTab = tabPageHome;
        }

    }
}
