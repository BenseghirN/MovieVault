namespace MovieVault.Core.Models
{
    public class Actor
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string? Nationality { get; set; }

        public HashSet<Movie> Movies { get; set; }

        public Actor()
        {
            Movies = new HashSet<Movie>();
        }
    }
}
