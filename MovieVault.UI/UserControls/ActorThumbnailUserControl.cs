using MovieVault.Data.Models;

namespace MovieVault.UI.UserControls
{
    public partial class ActorThumbnailUserControl : UserControl
    {
        private readonly Person _actor;
        public ActorThumbnailUserControl(Person actor)
        {
            InitializeComponent();
            _actor = actor;
            UpdateUI();
        }

        private void UpdateUI()
        {
            nameLabel.Text = $"{_actor.FirstName} {_actor.LastName}";
            pictureBox.LoadAsync(_actor.PhotoUrl ?? "default_actor.png");
        }
    }
}
