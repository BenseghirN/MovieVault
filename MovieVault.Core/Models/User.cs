namespace MovieVault.Core.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public HashSet<MovieStatus> MovieStatuses { get; set; }
        public List<Review> Reviews { get; set; }

        public User()
        {
            MovieStatuses = new HashSet<MovieStatus>();
            Reviews = new List<Review>();
        }
    }
}
