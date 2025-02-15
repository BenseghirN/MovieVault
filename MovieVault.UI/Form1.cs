using System.Data;
using MovieVault.Core.Interfaces;

namespace MovieVault.UI
{
    public partial class Form1 : Form
    {
        private readonly IUserService _userService;
        public Form1(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            InitializeComponent();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var dataTable = new DataTable();
                dataTable.Columns.Add("UserName");
                dataTable.Columns.Add("Email");
                dataTable.Columns.Add("Password");
                foreach (var user in users)
                {
                    dataTable.Rows.Add(user.UserName, user.Email, user.PasswordHash);
                }
                dataGridViewUsers.DataSource = dataTable;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Erreur lors du chargement des utilisateurs : {e.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
