namespace MovieVault.Core.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; } = string.Empty;

        public HashSet<Movie> Movies { get; set; }

        public Genre()
        {
            Movies = new HashSet<Movie>();
        }
    }
}
