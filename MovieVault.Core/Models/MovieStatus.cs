namespace MovieVault.Core.Models
{
    public class MovieStatus
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public int Status { get; set; } // 0 = Not Watched, 1 = Watched, 2 = To Watch
    }
}
