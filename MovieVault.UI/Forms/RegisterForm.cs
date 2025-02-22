using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using User = MovieVault.Data.Models.User;

namespace MovieVault.UI.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly IUserService _userService;
        private ILogger<RegisterForm> _logger;
        public RegisterForm(IUserService userService, ILogger<RegisterForm> logger)
        {
            _userService = userService;
            _logger = logger;
            InitializeComponent();
        }

        private async void buttonRegister_Click(object sender, EventArgs e)
        {
            await RegisterUserAsync();
        }

        private async Task RegisterUserAsync()
        {
            string username = textBoxUsername.Text;
            string mail = textBoxEmail.Text;
            string password = textBoxPassword.Text;
            string confirmPassword = textBoxConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(mail) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (confirmPassword != password)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas. Veuillez réessayer.",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var newUser = new User()
            {
                UserName = username,
                Email = mail
            };

            buttonRegister.Enabled = false;

            try
            {
                _logger.LogInformation("Registering new user with informations: {userName} {mail}", username, mail);
                var insert = await _userService.RegisterUserAsync(newUser, password);
                if (insert > 0)
                {
                    _logger.LogInformation("New user created successfully with id : {userId} {userName} {mail}", insert, username, mail);
                    MessageBox.Show("Utilisateur créé avec succès", "Succes", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Validation error: {message}", ex.Message);
                MessageBox.Show("Veuillez remplir tous les champs.", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Conflit utilisateur: {message}", ex.Message);
                MessageBox.Show(ex.Message, "Conflit utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while creating new user: {userName} {mail}", username, mail);
                MessageBox.Show("Erreur lors de la création", "Erreur", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                buttonRegister.Enabled = true;
            }
        }
    }
}
