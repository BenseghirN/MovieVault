namespace MovieVault.UI.UserControls
{
    partial class LoginUserControl
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
            textBoxEmail = new TextBox();
            textBoxPassword = new TextBox();
            buttonLogin = new Button();
            linkLabelForgotPassword = new LinkLabel();
            titleLabel = new Label();
            buttonRegister = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.BackColor = Color.Gray;
            pictureBoxLogo.Image = Properties.Resources.MainLogo;
            pictureBoxLogo.Location = new Point(297, 66);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(200, 200);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxLogo.TabIndex = 0;
            pictureBoxLogo.TabStop = false;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(295, 335);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.PlaceholderText = "Email";
            textBoxEmail.Size = new Size(200, 27);
            textBoxEmail.TabIndex = 1;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(295, 385);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PlaceholderText = "Password";
            textBoxPassword.Size = new Size(200, 27);
            textBoxPassword.TabIndex = 2;
            textBoxPassword.UseSystemPasswordChar = true;
            // 
            // buttonLogin
            // 
            buttonLogin.Location = new Point(295, 435);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new Size(200, 32);
            buttonLogin.TabIndex = 3;
            buttonLogin.Text = "Login";
            buttonLogin.Click += buttonLogin_Click;
            // 
            // linkLabelForgotPassword
            // 
            linkLabelForgotPassword.LinkColor = Color.Blue;
            linkLabelForgotPassword.Location = new Point(326, 470);
            linkLabelForgotPassword.Name = "linkLabelForgotPassword";
            linkLabelForgotPassword.Size = new Size(132, 25);
            linkLabelForgotPassword.TabIndex = 4;
            linkLabelForgotPassword.TabStop = true;
            linkLabelForgotPassword.Text = "Forgot Password?";
            linkLabelForgotPassword.Visible = false;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Helvetica", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.Location = new Point(301, 277);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(194, 41);
            titleLabel.TabIndex = 5;
            titleLabel.Text = "MovieVault";
            // 
            // buttonRegister
            // 
            buttonRegister.Location = new Point(297, 498);
            buttonRegister.Name = "buttonRegister";
            buttonRegister.Size = new Size(200, 30);
            buttonRegister.TabIndex = 6;
            buttonRegister.Text = "S'inscrire";
            buttonRegister.Click += buttonRegister_Click;
            // 
            // LoginUserControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(titleLabel);
            Controls.Add(pictureBoxLogo);
            Controls.Add(textBoxEmail);
            Controls.Add(textBoxPassword);
            Controls.Add(buttonLogin);
            Controls.Add(buttonRegister);
            Controls.Add(linkLabelForgotPassword);
            Name = "LoginUserControl";
            Size = new Size(800, 600);
            Resize += LoginUserControl_Resize;
            ParentChanged += LoginUserControl_ParentChanged;
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.LinkLabel linkLabelForgotPassword;
        private System.Windows.Forms.Button buttonRegister;
        private Label titleLabel;
    }
}
