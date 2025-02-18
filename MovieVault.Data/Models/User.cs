namespace MovieVault.Data.Models;

public class User
{
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }

    public HashSet<UserMovie> UserMovies { get; set; } = new();
    public HashSet<Review> Reviews { get; set; } = new();
}