namespace MovieVault.Data.Models;

public class MoviesPerson
{
    public int MovieId { get; set; }
    public int PersonId { get; set; }
    public int Role { get; set; } // 1 = Réalisateur, 2 = Acteur
}