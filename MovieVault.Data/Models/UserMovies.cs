namespace MovieVault.Data.Models;

public class UserMovies
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public string Status { get; set; } // Vu / A voir
    public bool Owned { get; set; } // Possédé physiquement
}