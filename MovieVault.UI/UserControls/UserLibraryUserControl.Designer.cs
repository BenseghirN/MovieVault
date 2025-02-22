namespace MovieVault.UI.UserControls
{
    partial class UserLibraryUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBoxLogo = new PictureBox();
            labelTitle = new Label();
            flowLayoutPanelMovies = new FlowLayoutPanel();
            labelUsername = new Label();
            noMoviesLabel = new Label();
            buttonRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Image = Properties.Resources.MainLogo;
            pictureBoxLogo.Location = new Point(20, 25);
            pictureBoxLogo.Margin = new Padding(3, 4, 3, 4);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(124, 125);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxLogo.TabIndex = 0;
            pictureBoxLogo.TabStop = false;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            labelTitle.Location = new Point(150, 62);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(172, 32);
            labelTitle.TabIndex = 1;
            labelTitle.Text = "Ma Collection";
            // 
            // flowLayoutPanelMovies
            // 
            flowLayoutPanelMovies.AutoScroll = true;
            flowLayoutPanelMovies.Location = new Point(50, 162);
            flowLayoutPanelMovies.Margin = new Padding(3, 4, 3, 4);
            flowLayoutPanelMovies.Name = "flowLayoutPanelMovies";
            flowLayoutPanelMovies.Size = new Size(1200, 750);
            flowLayoutPanelMovies.TabIndex = 2;
            // 
            // labelUsername
            // 
            labelUsername.AutoSize = true;
            labelUsername.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelUsername.Location = new Point(150, 40);
            labelUsername.Name = "labelUsername";
            labelUsername.Size = new Size(111, 28);
            labelUsername.TabIndex = 1;
            labelUsername.Text = "Utilisateur";
            // 
            // noMoviesLabel
            // 
            noMoviesLabel.AutoSize = true;
            noMoviesLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            noMoviesLabel.Location = new Point(443, 126);
            noMoviesLabel.Name = "noMoviesLabel";
            noMoviesLabel.Size = new Size(430, 32);
            noMoviesLabel.TabIndex = 3;
            noMoviesLabel.Text = "Pas de films dans votre bibliotheque";
            noMoviesLabel.Visible = false;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new Point(167, 121);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(100, 30);
            buttonRefresh.TabIndex = 4;
            buttonRefresh.Text = "Rafraîchir";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // UserLibraryUserControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(buttonRefresh);
            Controls.Add(noMoviesLabel);
            Controls.Add(pictureBoxLogo);
            Controls.Add(labelUsername);
            Controls.Add(labelTitle);
            Controls.Add(flowLayoutPanelMovies);
            Margin = new Padding(3, 4, 3, 4);
            Name = "UserLibraryUserControl";
            Size = new Size(1400, 1125);
            Load += UserLibraryUserControl_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // 🔥 Déclaration explicite des contrôles APRES #endregion
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMovies;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label noMoviesLabel;
        private System.Windows.Forms.Button buttonRefresh;
    }
}
