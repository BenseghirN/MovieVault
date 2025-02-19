using MovieVault.Data.Models;

namespace MovieVault.Data.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>?> GetReviewsByMovieIdAsync(int movieId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
        Task<Review?> GetReviewsByIdAsync(int reviewId);
        Task<int> CreateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<bool> UpdateReviewAsync(Review review);
        Task<IEnumerable<Review>?> GetAllReviews(int offset, int limit);
    }
}