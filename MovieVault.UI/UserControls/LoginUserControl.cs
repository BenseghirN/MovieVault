using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Core.Services;
using MovieVault.Data.Models;
using MovieVault.UI.Forms;

namespace MovieVault.UI.UserControls
{
    public partial class LoginUserControl : UserControl
    {
        private Action<User> _onLoginSuccess;
        private readonly IUserService _userService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LoginUserControl> _logger;
        public LoginUserControl(Action<User> onLoginSuccess, IUserService userService, IServiceProvider serviceProvider, ILogger<LoginUserControl> logger)
        {
            InitializeComponent();
            _onLoginSuccess = onLoginSuccess;
            _userService = userService;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = _serviceProvider.GetRequiredService<RegisterForm>();
            registerForm.ShowDialog();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            LoginUser();
        }

        private async Task LoginUser()
        {
            string mail = textBoxEmail.Text;
            string password = textBoxPassword.Text;
            _logger.LogInformation("User with mail {userMail} trying to login", mail);

            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var userLogged = await _userService.ValidatePasswordAsync(mail, password);
                if (userLogged != null)
                {
                    _logger.LogInformation("User with mail {userMail} succefully logged in", mail);
                    MessageBox.Show($"Bienvenue {userLogged.UserName}", "Connexion réussie", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    _onLoginSuccess?.Invoke(userLogged);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong logging user with mail {userMail}", mail);
                MessageBox.Show("Erreur lors de l'identification", "Erreur", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                throw;
            }
        }

        private void CenterControl()
        {
            if (this.Parent != null)
            {
                int newX = (this.Parent.Width - this.Width) / 2;
                int newY = (this.Parent.Height - this.Height) / 2;
                this.Location = new Point(Math.Max(0, newX), Math.Max(0, newY));
            }
        }

        private void LoginUserControl_ParentChanged(object sender, EventArgs e)
        {
            CenterControl();
        }

        private void LoginUserControl_Resize(object sender, EventArgs e)
        {
            CenterControl();
        }
    }
}
