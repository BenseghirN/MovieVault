using System.Windows.Forms;

namespace MovieVault.UI.UserControls
{
    partial class SearchMoviesUserControl
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
            panelHeader = new Panel();
            pictureBoxLogo = new PictureBox();
            labelSearch = new Label();
            textBoxSearch = new TextBox();
            buttonSearch = new Button();
            flowLayoutPanelResults = new FlowLayoutPanel();
            noMoviesLabel = new Label();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            flowLayoutPanelResults.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.Controls.Add(pictureBoxLogo);
            panelHeader.Controls.Add(labelSearch);
            panelHeader.Controls.Add(textBoxSearch);
            panelHeader.Controls.Add(buttonSearch);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1400, 159);
            panelHeader.TabIndex = 6;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Image = Properties.Resources.MainLogo;
            pictureBoxLogo.Location = new Point(20, 25);
            pictureBoxLogo.Margin = new Padding(3, 4, 3, 4);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(124, 125);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxLogo.TabIndex = 9;
            pictureBoxLogo.TabStop = false;
            // 
            // labelSearch
            // 
            labelSearch.AutoSize = true;
            labelSearch.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            labelSearch.Location = new Point(150, 51);
            labelSearch.Name = "labelSearch";
            labelSearch.Size = new Size(229, 32);
            labelSearch.TabIndex = 6;
            labelSearch.Text = "Rechercher un film";
            // 
            // textBoxSearch
            // 
            textBoxSearch.Font = new Font("Segoe UI", 12F);
            textBoxSearch.Location = new Point(150, 91);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.PlaceholderText = "Titre du film";
            textBoxSearch.Size = new Size(400, 34);
            textBoxSearch.TabIndex = 7;
            // 
            // buttonSearch
            // 
            buttonSearch.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            buttonSearch.Location = new Point(570, 91);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(150, 35);
            buttonSearch.TabIndex = 8;
            buttonSearch.Text = "Rechercher";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += ButtonSearch_Click;
            // 
            // flowLayoutPanelResults
            // 
            flowLayoutPanelResults.AutoScroll = true;
            flowLayoutPanelResults.Controls.Add(noMoviesLabel);
            flowLayoutPanelResults.Dock = DockStyle.Fill;
            flowLayoutPanelResults.Location = new Point(0, 159);
            flowLayoutPanelResults.Name = "flowLayoutPanelResults";
            flowLayoutPanelResults.Size = new Size(1400, 741);
            flowLayoutPanelResults.TabIndex = 7;
            // 
            // noMoviesLabel
            // 
            noMoviesLabel.AutoSize = true;
            noMoviesLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            noMoviesLabel.Location = new Point(3, 0);
            noMoviesLabel.Name = "noMoviesLabel";
            noMoviesLabel.Size = new Size(430, 32);
            noMoviesLabel.TabIndex = 4;
            noMoviesLabel.Text = "Pas de films dans votre bibliotheque";
            noMoviesLabel.Visible = false;
            // 
            // SearchMoviesUserControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flowLayoutPanelResults);
            Controls.Add(panelHeader);
            Name = "SearchMoviesUserControl";
            Size = new Size(1400, 900);
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            flowLayoutPanelResults.ResumeLayout(false);
            flowLayoutPanelResults.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelResults;
        private Label noMoviesLabel;
        private Panel panelHeader;
        private PictureBox pictureBoxLogo;
        private Label labelSearch;
        private TextBox textBoxSearch;
        private Button buttonSearch;
        private Panel panel2;
    }
}
