namespace MovieVault.UI.Forms
{
    partial class MovieDetailsForm
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
            components = new System.ComponentModel.Container();
            pictureBoxPoster = new PictureBox();
            labelTitle = new Label();
            labelDirector = new Label();
            labelGenres = new Label();
            textBoxSynopsis = new TextBox();
            flowLayoutPanelActors = new FlowLayoutPanel();
            panelRating = new Panel();
            starRatingPanel = new FlowLayoutPanel();
            labelRating = new Label();
            panelReviews = new Panel();
            listBoxReviews = new ListBox();
            labelReviews = new Label();
            buttonSubmitReview = new Button();
            textBoxComment = new TextBox();
            buttonAddToLibrary = new Button();
            buttonRemoveFromLibrary = new Button();
            buttonBack = new Button();
            labelActors = new Label();
            pictureBoxLogo = new PictureBox();
            labelReleaseYear = new Label();
            reviewsBindingSource = new BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)pictureBoxPoster).BeginInit();
            panelRating.SuspendLayout();
            panelReviews.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)reviewsBindingSource).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxPoster
            // 
            pictureBoxPoster.Location = new Point(165, 95);
            pictureBoxPoster.Name = "pictureBoxPoster";
            pictureBoxPoster.Size = new Size(300, 400);
            pictureBoxPoster.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPoster.TabIndex = 0;
            pictureBoxPoster.TabStop = false;
            // 
            // labelTitle
            // 
            labelTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            labelTitle.Location = new Point(498, 20);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(591, 30);
            labelTitle.TabIndex = 1;
            // 
            // labelDirector
            // 
            labelDirector.Location = new Point(498, 95);
            labelDirector.Name = "labelDirector";
            labelDirector.Size = new Size(238, 40);
            labelDirector.TabIndex = 2;
            // 
            // labelGenres
            // 
            labelGenres.Location = new Point(742, 95);
            labelGenres.Name = "labelGenres";
            labelGenres.Size = new Size(262, 40);
            labelGenres.TabIndex = 3;
            // 
            // textBoxSynopsis
            // 
            textBoxSynopsis.BorderStyle = BorderStyle.None;
            textBoxSynopsis.Location = new Point(498, 138);
            textBoxSynopsis.Multiline = true;
            textBoxSynopsis.Name = "textBoxSynopsis";
            textBoxSynopsis.ReadOnly = true;
            textBoxSynopsis.ScrollBars = ScrollBars.Vertical;
            textBoxSynopsis.Size = new Size(500, 80);
            textBoxSynopsis.TabIndex = 4;
            // 
            // flowLayoutPanelActors
            // 
            flowLayoutPanelActors.Location = new Point(498, 244);
            flowLayoutPanelActors.Name = "flowLayoutPanelActors";
            flowLayoutPanelActors.Size = new Size(591, 159);
            flowLayoutPanelActors.TabIndex = 5;
            // 
            // panelRating
            // 
            panelRating.Controls.Add(starRatingPanel);
            panelRating.Controls.Add(labelRating);
            panelRating.Enabled = false;
            panelRating.Location = new Point(498, 409);
            panelRating.Name = "panelRating";
            panelRating.Size = new Size(300, 50);
            panelRating.TabIndex = 6;
            // 
            // starRatingPanel
            // 
            starRatingPanel.Location = new Point(100, 10);
            starRatingPanel.Name = "starRatingPanel";
            starRatingPanel.Size = new Size(200, 30);
            starRatingPanel.TabIndex = 0;
            // 
            // labelRating
            // 
            labelRating.Location = new Point(10, 10);
            labelRating.Name = "labelRating";
            labelRating.Size = new Size(80, 30);
            labelRating.TabIndex = 1;
            labelRating.Text = "Évaluation :";
            // 
            // panelReviews
            // 
            panelReviews.Controls.Add(listBoxReviews);
            panelReviews.Controls.Add(labelReviews);
            panelReviews.Dock = DockStyle.Bottom;
            panelReviews.Location = new Point(0, 565);
            panelReviews.Name = "panelReviews";
            panelReviews.Size = new Size(1182, 188);
            panelReviews.TabIndex = 7;
            // 
            // listBoxReviews
            // 
            listBoxReviews.Location = new Point(20, 32);
            listBoxReviews.Name = "listBoxReviews";
            listBoxReviews.Size = new Size(1125, 144);
            listBoxReviews.TabIndex = 0;
            // 
            // labelReviews
            // 
            labelReviews.Location = new Point(10, 10);
            labelReviews.Name = "labelReviews";
            labelReviews.Size = new Size(200, 20);
            labelReviews.TabIndex = 1;
            labelReviews.Text = "Avis des utilisateurs :";
            // 
            // buttonSubmitReview
            // 
            buttonSubmitReview.Enabled = false;
            buttonSubmitReview.Location = new Point(989, 463);
            buttonSubmitReview.Name = "buttonSubmitReview";
            buttonSubmitReview.Size = new Size(100, 30);
            buttonSubmitReview.TabIndex = 8;
            buttonSubmitReview.Text = "Valider";
            // 
            // textBoxComment
            // 
            textBoxComment.Enabled = false;
            textBoxComment.Location = new Point(498, 465);
            textBoxComment.Name = "textBoxComment";
            textBoxComment.Size = new Size(340, 27);
            textBoxComment.TabIndex = 9;
            // 
            // buttonAddToLibrary
            // 
            buttonAddToLibrary.Location = new Point(498, 504);
            buttonAddToLibrary.Name = "buttonAddToLibrary";
            buttonAddToLibrary.Size = new Size(200, 30);
            buttonAddToLibrary.TabIndex = 10;
            buttonAddToLibrary.Text = "Ajouter à ma bibliothèque";
            buttonAddToLibrary.Click += buttonAddToLibrary_Click;
            // 
            // buttonRemoveFromLibrary
            // 
            buttonRemoveFromLibrary.Location = new Point(704, 504);
            buttonRemoveFromLibrary.Name = "buttonRemoveFromLibrary";
            buttonRemoveFromLibrary.Size = new Size(200, 30);
            buttonRemoveFromLibrary.TabIndex = 11;
            buttonRemoveFromLibrary.Text = "Retirer de ma bibliothèque";
            buttonRemoveFromLibrary.Visible = false;
            // 
            // buttonBack
            // 
            buttonBack.Location = new Point(150, 25);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new Size(80, 30);
            buttonBack.TabIndex = 12;
            buttonBack.Text = "← Retour";
            // 
            // labelActors
            // 
            labelActors.Location = new Point(498, 221);
            labelActors.Name = "labelActors";
            labelActors.Size = new Size(200, 20);
            labelActors.TabIndex = 13;
            labelActors.Text = "Acteurs Principaux :";
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Image = Properties.Resources.MainLogo;
            pictureBoxLogo.Location = new Point(20, 25);
            pictureBoxLogo.Margin = new Padding(3, 4, 3, 4);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(124, 125);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxLogo.TabIndex = 14;
            pictureBoxLogo.TabStop = false;
            // 
            // labelReleaseYear
            // 
            labelReleaseYear.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            labelReleaseYear.Location = new Point(498, 58);
            labelReleaseYear.Name = "labelReleaseYear";
            labelReleaseYear.Size = new Size(238, 30);
            labelReleaseYear.TabIndex = 15;
            // 
            // MovieDetailsForm
            // 
            ClientSize = new Size(1182, 753);
            Controls.Add(labelReleaseYear);
            Controls.Add(pictureBoxLogo);
            Controls.Add(pictureBoxPoster);
            Controls.Add(labelTitle);
            Controls.Add(labelDirector);
            Controls.Add(labelGenres);
            Controls.Add(textBoxSynopsis);
            Controls.Add(flowLayoutPanelActors);
            Controls.Add(panelRating);
            Controls.Add(panelReviews);
            Controls.Add(buttonSubmitReview);
            Controls.Add(textBoxComment);
            Controls.Add(buttonAddToLibrary);
            Controls.Add(buttonRemoveFromLibrary);
            Controls.Add(buttonBack);
            Controls.Add(labelActors);
            Name = "MovieDetailsForm";
            Text = "Détails du Film";
            ((System.ComponentModel.ISupportInitialize)pictureBoxPoster).EndInit();
            panelRating.ResumeLayout(false);
            panelReviews.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)reviewsBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxPoster;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelDirector;
        private System.Windows.Forms.Label labelGenres;
        private System.Windows.Forms.TextBox textBoxSynopsis;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelActors;
        private System.Windows.Forms.Panel panelRating;
        private System.Windows.Forms.Panel panelReviews;
        private System.Windows.Forms.ListBox listBoxReviews;
        private System.Windows.Forms.Button buttonSubmitReview;
        private System.Windows.Forms.TextBox textBoxComment;
        private System.Windows.Forms.Button buttonAddToLibrary;
        private System.Windows.Forms.Button buttonRemoveFromLibrary;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelActors;
        private System.Windows.Forms.Label labelRating;
        private System.Windows.Forms.Label labelReviews;
        private System.Windows.Forms.FlowLayoutPanel starRatingPanel;
        private PictureBox pictureBoxLogo;
        private Label labelReleaseYear;
        private BindingSource reviewsBindingSource;
    }
}