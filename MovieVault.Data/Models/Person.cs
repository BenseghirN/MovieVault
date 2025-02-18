namespace MovieVault.Data.Models;

public class Person
{
    public int PersonId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Nationality { get; set; }
    public string? PhotoUrl { get; set; }
    public int? TMDBId { get; set; }

    public HashSet<MoviesPerson> MoviesPeople { get; set; } = new();
}