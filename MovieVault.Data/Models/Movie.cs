namespace MovieVault.Data.Models;

public class Movie
{
    public int MovieId { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public int Duration { get; set; } // en minutes
    public string Synopsis { get; set; }
    public string PosterUrl { get; set; }

    public HashSet<UserMovies> UserMovies { get; set; } = new();
    public HashSet<Review> Reviews { get; set; } = new();
    public HashSet<MoviesGenres> MoviesGenres { get; set; } = new();
    public HashSet<MoviesPeople> MoviesPeople { get; set; } = new();
}