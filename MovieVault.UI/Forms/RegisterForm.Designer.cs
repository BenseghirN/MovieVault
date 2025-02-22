namespace MovieVault.UI.Forms
{
    partial class RegisterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterForm));
            textBoxUsername = new TextBox();
            textBoxEmail = new TextBox();
            textBoxPassword = new TextBox();
            textBoxConfirmPassword = new TextBox();
            buttonRegister = new Button();
            labelUsername = new Label();
            labelEmail = new Label();
            labelPassword = new Label();
            labelConfirmPassword = new Label();
            SuspendLayout();
            // 
            // textBoxUsername
            // 
            textBoxUsername.Location = new Point(180, 30);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new Size(200, 27);
            textBoxUsername.TabIndex = 1;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(180, 70);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(200, 27);
            textBoxEmail.TabIndex = 3;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(180, 110);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(200, 27);
            textBoxPassword.TabIndex = 5;
            textBoxPassword.UseSystemPasswordChar = true;
            // 
            // textBoxConfirmPassword
            // 
            textBoxConfirmPassword.Location = new Point(180, 150);
            textBoxConfirmPassword.Name = "textBoxConfirmPassword";
            textBoxConfirmPassword.Size = new Size(200, 27);
            textBoxConfirmPassword.TabIndex = 7;
            textBoxConfirmPassword.UseSystemPasswordChar = true;
            // 
            // buttonRegister
            // 
            buttonRegister.Location = new Point(180, 200);
            buttonRegister.Name = "buttonRegister";
            buttonRegister.Size = new Size(200, 30);
            buttonRegister.TabIndex = 8;
            buttonRegister.Text = "S'inscrire";
            buttonRegister.UseVisualStyleBackColor = true;
            buttonRegister.Click += buttonRegister_Click;
            // 
            // labelUsername
            // 
            labelUsername.Location = new Point(30, 30);
            labelUsername.Name = "labelUsername";
            labelUsername.Size = new Size(120, 20);
            labelUsername.TabIndex = 0;
            labelUsername.Text = "Nom d'utilisateur:";
            // 
            // labelEmail
            // 
            labelEmail.Location = new Point(30, 70);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(120, 20);
            labelEmail.TabIndex = 2;
            labelEmail.Text = "Email:";
            // 
            // labelPassword
            // 
            labelPassword.Location = new Point(30, 110);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(120, 20);
            labelPassword.TabIndex = 4;
            labelPassword.Text = "Mot de passe:";
            // 
            // labelConfirmPassword
            // 
            labelConfirmPassword.Location = new Point(30, 150);
            labelConfirmPassword.Name = "labelConfirmPassword";
            labelConfirmPassword.Size = new Size(140, 44);
            labelConfirmPassword.TabIndex = 6;
            labelConfirmPassword.Text = "Vérification mot de passe:";
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 270);
            Controls.Add(labelUsername);
            Controls.Add(textBoxUsername);
            Controls.Add(labelEmail);
            Controls.Add(textBoxEmail);
            Controls.Add(labelPassword);
            Controls.Add(textBoxPassword);
            Controls.Add(labelConfirmPassword);
            Controls.Add(textBoxConfirmPassword);
            Controls.Add(buttonRegister);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Créer un compte";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxConfirmPassword;
        private System.Windows.Forms.Button buttonRegister;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelConfirmPassword;
    }
}