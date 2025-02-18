namespace MovieVault.Data.Models;

public class Review
{
    public int ReviewId { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public decimal Rating { get; set; }
    public string? Comment { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; }
}