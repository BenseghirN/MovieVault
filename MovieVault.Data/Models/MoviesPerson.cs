namespace MovieVault.Data.Models;

public class MoviesPerson
{
    public int MovieId { get; set; }
    public int PersonId { get; set; }
    public PersonRole Role { get; set; } // 1 = Réalisateur, 2 = Acteur
}

public enum PersonRole { Director = 1, Actor = 2 }