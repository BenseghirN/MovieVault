namespace MovieVault.Data.Models;

public class Genre
{
    public int GenreId { get; set; }
    public string GenreName { get; set; } = string.Empty;
    public int? TMDBId { get; set; }

    public HashSet<MoviesGenres> MoviesGenres { get; set; } = new();
}