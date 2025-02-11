namespace MovieVault.Data.Models;

public class Review
{
    public int ReviewId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
}