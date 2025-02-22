namespace MovieVault.UI.UserControls
{
    partial class ActorThumbnailUserControl
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
            pictureBox = new PictureBox();
            nameLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(10, 10);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(80, 80);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            // 
            // nameLabel
            // 
            nameLabel.Font = new Font("Arial", 9F, FontStyle.Bold);
            nameLabel.Location = new Point(0, 90);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(100, 45);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Nom de l'acteur";
            nameLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ActorThumbnailUserControl
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(pictureBox);
            Controls.Add(nameLabel);
            Name = "ActorThumbnailUserControl";
            Size = new Size(100, 144);
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label nameLabel;
    }
}
