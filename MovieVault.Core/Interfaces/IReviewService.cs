using MovieVault.Data.Models;

namespace MovieVault.Core.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(int movieId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
        Task<Review?> GetReviewById(int reviewId);
        Task<int> CreateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<bool> UpdateReviewAsync(Review review);
    }
}