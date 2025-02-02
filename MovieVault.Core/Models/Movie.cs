namespace MovieVault.Core.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string? Director { get; set; }
        public int Duration { get; set; }
        public bool Owned { get; set; }

        public HashSet<Genre> Genres { get; set; }
        public HashSet<Actor> Actors { get; set; }
        public List<Review> Reviews { get; set; }
        public List<MovieStatus> Statuses { get; set; }

        public Movie()
        {
            Genres = new HashSet<Genre>();
            Actors = new HashSet<Actor>();
            Reviews = new List<Review>();
            Statuses = new List<MovieStatus>();
        }
    }
}
