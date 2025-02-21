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
            components = new System.ComponentModel.Container();
            searchTextBox = new TextBox();
            searchButton = new Button();
            moviesListBox = new ListBox();
            crewListBox = new ListBox();
            genresListBox = new ListBox();
            moviesBindingSource = new BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)moviesBindingSource).BeginInit();
            SuspendLayout();
            // 
            // searchTextBox
            // 
            searchTextBox.Location = new Point(112, 92);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(125, 27);
            searchTextBox.TabIndex = 0;
            // 
            // searchButton
            // 
            searchButton.Location = new Point(12, 90);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(94, 29);
            searchButton.TabIndex = 1;
            searchButton.Text = "button1";
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += searchButton_Click;
            // 
            // moviesListBox
            // 
            moviesListBox.FormattingEnabled = true;
            moviesListBox.Location = new Point(12, 125);
            moviesListBox.Name = "moviesListBox";
            moviesListBox.Size = new Size(349, 224);
            moviesListBox.TabIndex = 2;
            moviesListBox.SelectedIndexChanged += moviesListBox_SelectedIndexChanged;
            // 
            // crewListBox
            // 
            crewListBox.FormattingEnabled = true;
            crewListBox.Location = new Point(383, 185);
            crewListBox.Name = "crewListBox";
            crewListBox.Size = new Size(312, 164);
            crewListBox.TabIndex = 3;
            // 
            // genresListBox
            // 
            genresListBox.FormattingEnabled = true;
            genresListBox.Location = new Point(718, 185);
            genresListBox.Name = "genresListBox";
            genresListBox.Size = new Size(315, 164);
            genresListBox.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1327, 763);
            Controls.Add(genresListBox);
            Controls.Add(crewListBox);
            Controls.Add(moviesListBox);
            Controls.Add(searchButton);
            Controls.Add(searchTextBox);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)moviesBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox searchTextBox;
        private Button searchButton;
        private ListBox moviesListBox;
        private ListBox crewListBox;
        private ListBox genresListBox;
        private BindingSource moviesBindingSource;
    }
}
