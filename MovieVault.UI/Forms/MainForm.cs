using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieVault.UI.Forms
{
    public partial class MainForm: Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Charger l'UserControl de connexion dans l'onglet Login
            //tabPageLogin.Controls.Add(new LoginControl(OnLoginSuccess) { Dock = DockStyle.Fill });

            // Supprimer les autres onglets au démarrage
            tabControlMain.TabPages.Remove(tabPageHome);
            tabControlMain.TabPages.Remove(tabPageMovies);
            tabControlMain.TabPages.Remove(tabPageUsers);
        }

    }
}
