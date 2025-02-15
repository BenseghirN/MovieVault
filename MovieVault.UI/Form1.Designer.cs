namespace MovieVault.UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridViewUsers = new DataGridView();
            dataGridViewMovies = new DataGridView();
            UserName = new DataGridViewTextBoxColumn();
            Email = new DataGridViewTextBoxColumn();
            Title = new DataGridViewTextBoxColumn();
            ReleaseYear = new DataGridViewTextBoxColumn();
            Duration = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMovies).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewUsers
            // 
            dataGridViewUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsers.Columns.AddRange(new DataGridViewColumn[] { UserName, Email });
            dataGridViewUsers.Location = new Point(36, 62);
            dataGridViewUsers.Name = "dataGridViewUsers";
            dataGridViewUsers.RowHeadersWidth = 51;
            dataGridViewUsers.Size = new Size(818, 227);
            dataGridViewUsers.TabIndex = 0;
            // 
            // dataGridViewMovies
            // 
            dataGridViewMovies.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewMovies.Columns.AddRange(new DataGridViewColumn[] { Title, ReleaseYear, Duration });
            dataGridViewMovies.Location = new Point(36, 352);
            dataGridViewMovies.Name = "dataGridViewMovies";
            dataGridViewMovies.RowHeadersWidth = 51;
            dataGridViewMovies.Size = new Size(818, 247);
            dataGridViewMovies.TabIndex = 1;
            // 
            // UserName
            // 
            UserName.HeaderText = "UserName";
            UserName.MinimumWidth = 6;
            UserName.Name = "UserName";
            UserName.Width = 125;
            // 
            // Email
            // 
            Email.HeaderText = "Email";
            Email.MinimumWidth = 6;
            Email.Name = "Email";
            Email.Width = 125;
            // 
            // Title
            // 
            Title.HeaderText = "Title";
            Title.MinimumWidth = 6;
            Title.Name = "Title";
            Title.Width = 125;
            // 
            // ReleaseYear
            // 
            ReleaseYear.HeaderText = "ReleaseYear";
            ReleaseYear.MinimumWidth = 6;
            ReleaseYear.Name = "ReleaseYear";
            ReleaseYear.Width = 125;
            // 
            // Duration
            // 
            Duration.HeaderText = "Duration";
            Duration.MinimumWidth = 6;
            Duration.Name = "Duration";
            Duration.Width = 125;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1327, 763);
            Controls.Add(dataGridViewMovies);
            Controls.Add(dataGridViewUsers);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMovies).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewUsers;
        private DataGridView dataGridViewMovies;
        private DataGridViewTextBoxColumn UserName;
        private DataGridViewTextBoxColumn Email;
        private DataGridViewTextBoxColumn Title;
        private DataGridViewTextBoxColumn ReleaseYear;
        private DataGridViewTextBoxColumn Duration;
    }
}
