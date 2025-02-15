namespace MovieVault.Data.Models;

public class MoviesPeople
{
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
    public int PersonId { get; set; }
    public People? Person { get; set; }
    public int Role { get; set; } // 1 = Réalisateur, 2 = Acteur
}