namespace MovieVault.Data.Models;

public class People
{
    public int PersonId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Nationality { get; set; }
    public string? PhotoUrl { get; set; }

    public HashSet<MoviesPeople> MoviesPeople { get; set; } = new();
}