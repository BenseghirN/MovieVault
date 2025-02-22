namespace MovieVault.UI.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tabControlMain = new TabControl();
            tabPageLogin = new TabPage();
            tabPageUserLibrary = new TabPage();
            tabPageSearch = new TabPage();
            tabPageUsers = new TabPage();
            tabControlMain.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabPageLogin);
            tabControlMain.Controls.Add(tabPageUserLibrary);
            tabControlMain.Controls.Add(tabPageSearch);
            tabControlMain.Controls.Add(tabPageUsers);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Location = new Point(0, 0);
            tabControlMain.Margin = new Padding(3, 4, 3, 4);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(1371, 1067);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageLogin
            // 
            tabPageLogin.Location = new Point(4, 29);
            tabPageLogin.Margin = new Padding(3, 4, 3, 4);
            tabPageLogin.Name = "tabPageLogin";
            tabPageLogin.Padding = new Padding(3, 4, 3, 4);
            tabPageLogin.Size = new Size(1363, 1034);
            tabPageLogin.TabIndex = 0;
            tabPageLogin.Text = "Login";
            tabPageLogin.UseVisualStyleBackColor = true;
            // 
            // tabPageUserLibrary
            // 
            tabPageUserLibrary.Location = new Point(4, 29);
            tabPageUserLibrary.Margin = new Padding(3, 4, 3, 4);
            tabPageUserLibrary.Name = "tabPageUserLibrary";
            tabPageUserLibrary.Padding = new Padding(3, 4, 3, 4);
            tabPageUserLibrary.Size = new Size(1363, 1034);
            tabPageUserLibrary.TabIndex = 1;
            tabPageUserLibrary.Text = "Accueil";
            tabPageUserLibrary.UseVisualStyleBackColor = true;
            // 
            // tabPageSearch
            // 
            tabPageSearch.Location = new Point(4, 29);
            tabPageSearch.Margin = new Padding(3, 4, 3, 4);
            tabPageSearch.Name = "tabPageSearch";
            tabPageSearch.Padding = new Padding(3, 4, 3, 4);
            tabPageSearch.Size = new Size(1363, 1034);
            tabPageSearch.TabIndex = 2;
            tabPageSearch.Text = "Recherche";
            tabPageSearch.UseVisualStyleBackColor = true;
            // 
            // tabPageUsers
            // 
            tabPageUsers.Location = new Point(4, 29);
            tabPageUsers.Margin = new Padding(3, 4, 3, 4);
            tabPageUsers.Name = "tabPageUsers";
            tabPageUsers.Padding = new Padding(3, 4, 3, 4);
            tabPageUsers.Size = new Size(1363, 1034);
            tabPageUsers.TabIndex = 3;
            tabPageUsers.Text = "Utilisateurs";
            tabPageUsers.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1371, 1067);
            Controls.Add(tabControlMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion des Films";
            Load += MainForm_Load;
            tabControlMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageLogin;
        private System.Windows.Forms.TabPage tabPageUserLibrary;
        private System.Windows.Forms.TabPage tabPageSearch;
        private System.Windows.Forms.TabPage tabPageUsers;
    }
}