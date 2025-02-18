namespace MovieVault.Data.Models;

public class UserMovie
{
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public MovieStatus Status { get; set; } = MovieStatus.Unwatched;
    public bool Owned { get; set; } = false;
    public DateTime? LastWatched { get; set; }
}

public enum MovieStatus { Unwatched = 0, Watched = 1, Owned = 2 }